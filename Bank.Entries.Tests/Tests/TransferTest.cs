using AutoFixture;
using Bank.Entries.Core.DTOs;
using Bank.Entries.Core.Models;
using Moq;
using NUnit.Framework;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestStack.BDDfy;

namespace Bank.Entries.Tests
{
    public class EntryTest : BaseTest
    {
        private TransferDTO transferDTO;
        private Dictionary<string, BankAccount> AccountsBeforeChange = new Dictionary<string, BankAccount>();
        private Dictionary<string, BankAccount> AccountsAfterChange = new Dictionary<string, BankAccount>();

        [Test]
        public void InternalTransferSuccess()
        {
            var amount = 200m;
            var amountInSendersAccount = 3000m;

            this.Given(s => s.SenderTransfers(amount))
                .And(s => s.DatabaseIsWorking())
                .And(s => s.SenderCurrentyHaveOnAccount(amountInSendersAccount), $"Sender has {amountInSendersAccount}")
                .And(s => s.ReceiverExists())
                .And(s => s.ShouldPersistWith(true))
                .Then(s => s.ShouldSendChangesToConsumers())
                .When(s => s.TransferOccurs())
               .When(s => s.ShouldHaveSubtractedFromSender())
                .And(s => s.ShouldHaveAddedToReceiver())
               .BDDfy();
        }
        [Test]
        public void InternalTransferReceiverAccountNotFound()
        {
            var amount = 200m;
            var amountInSendersAccount = 3000m;

            this.Given(s => s.SenderTransfers(amount))
                .And(s => s.SenderCurrentyHaveOnAccount(amountInSendersAccount), $"Sender has {amountInSendersAccount}")
               .Then(s => s.ReceiverAccountIsInvalid())
               .Then(s => s.ShouldSendChangesToConsumers())
               .Then(s => s.TransferOccurs())
               .BDDfy();
        }
        [Test]
        public void InternalTransferUnexpectedError()
        {
            var amount = 200m;
            var amountInSendersAccount = 3000m;

            this.Given(s => s.SenderTransfers(amount))
                .And(s => s.SenderCurrentyHaveOnAccount(amountInSendersAccount), $"Sender has {amountInSendersAccount}")
               .Then(s => s.AnErrorOccurrs())
               .Then(s => s.ShouldSendChangesToConsumers())
               .Then(s=>s.TransferOccurs())
               .BDDfy();
        }

        [Test]
        public void InternalTransferInsufficientFunds()
        {
            var amount = 10000m;
            var amountInSendersAccount = 3000m;

            this.Given(s => s.SenderTransfers(amount))
                .And(s => s.SenderCurrentyHaveOnAccount(amountInSendersAccount), $"Sender has {amountInSendersAccount}")
                .And(s => s.DatabaseIsWorking())
               .Then(s => s.ShouldSendChangesToConsumers())
               .When(s => s.TransferOccurs())
               .BDDfy();
        }
        private void ReceiverAccountIsInvalid()
        {
            Assert.IsFalse(AccountsBeforeChange.ContainsKey(Constants.Receiver));
        }
        private void SenderCurrentyHaveOnAccount(decimal amount) =>
            AccountsBeforeChange.Add(Constants.Sender, new BankAccount(fixture.Create<int>(),
                                                   transferDTO.SenderBranch,
                                                   transferDTO.SenderNumber,
                                                   transferDTO.SenderDigit,
                                                   amount));
        private void ReceiverExists() =>
            AccountsBeforeChange.Add(Constants.Receiver,
                new BankAccount(fixture.Create<int>(),
                                transferDTO.ReceiverBranch,
                                transferDTO.ReceiverNumber,
                                transferDTO.ReceiverDigit,
                                fixture.Create<decimal>()));
        private void ShouldHaveAddedToReceiver()
        {
            var before = AccountsBeforeChange[Constants.Receiver];

            var after = AccountsAfterChange[Constants.Receiver];

            Assert.AreEqual(before.Balance + transferDTO.Value, after.Balance);
        }
        private void ShouldHaveSubtractedFromSender()
        {
            var before = AccountsBeforeChange[Constants.Sender];

            var after = AccountsAfterChange[Constants.Sender];

            Assert.AreEqual(before.Balance + transferDTO.Value * -1, after.Balance);

        }
        public void AnErrorOccurrs() =>
            exprGetBankAccount.SetupOn(transferRepository).Returns(() => { throw new Exception("some Err"); });
        public void DatabaseIsWorking()
    {
        exprGetBankAccount.SetupOn(transferRepository)
          .ReturnsAsync((int branch, int number, int digit) =>
              AccountsBeforeChange.FirstOrDefault(b =>
                  b.Value.Branch == branch
                  && b.Value.Number == number
                  && digit == b.Value.Digit).Value
          );
    }
    public void SenderTransfers(decimal amount)
    {
        transferDTO = fixture.Create<TransferDTO>();
        transferDTO.Value = amount;

    }
    public void TransferOccurs()
    {
        transferService.InternalTransfer(transferDTO).GetAwaiter().GetResult();
    }

    public void ShouldPersistWith(bool Success) =>
        exprTransfer
            .SetupOn(transferRepository)
            .ReturnsAsync(Success).Callback(() =>
            {
                var beforeSender = AccountsBeforeChange[Constants.Sender];
                var beforeReceiver = AccountsBeforeChange[Constants.Receiver];

                var afterSender = new BankAccount(beforeSender.Id,
                                                    beforeSender.Branch,
                                                    beforeSender.Number,
                                                    beforeSender.Digit,
                                                    beforeSender.Balance);

                var afterReceiver = new BankAccount(beforeReceiver.Id,
                                                beforeReceiver.Branch,
                                                beforeReceiver.Number,
                                                beforeReceiver.Digit,
                                                beforeReceiver.Balance);

                if (Success)
                {
                    afterSender.UpdateBalance(transferDTO.Value * -1);
                    afterReceiver.UpdateBalance(transferDTO.Value);
                }

                AccountsAfterChange.Add(Constants.Receiver, afterReceiver);
                AccountsAfterChange.Add(Constants.Sender, afterSender);
            });

    public void ShouldSendChangesToConsumers()
    {
        rabbitConn.Setup(m => m.CreateModel()).Returns(Channel.Object);

        Channel.Setup(q => q.BasicPublish(It.IsAny<string>(),
                                          It.IsAny<string>(),
                                          It.IsAny<bool>(),
                                          It.IsAny<IBasicProperties>(),
                                          It.IsAny<byte[]>()));

        Channel.Setup(q => q.ExchangeDeclare(It.IsAny<string>(),
                                          It.IsAny<string>(),
                                          It.IsAny<bool>(),
                                          It.IsAny<bool>(),
                                          It.IsAny<Dictionary<string, object>>()
                                          ));

    }


    [TearDown]
    public new void CleanUp()
    {
        transferDTO = null;
        AccountsBeforeChange = new Dictionary<string, BankAccount>();
        AccountsAfterChange = new Dictionary<string, BankAccount>();
        base.CleanUp();
    }

}
}
