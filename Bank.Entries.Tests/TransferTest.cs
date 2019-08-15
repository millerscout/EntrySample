using AutoFixture;
using Bank.Entries.Core.DTOs;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TestStack.BDDfy;

namespace Bank.Entries.Tests
{
    public class EntryTest : BaseTest
    {
        private TransferDTO transferDTO;

        [Test]
        public void InternalTransferSuccess()
        {
            var amount = 200m;
            var amountInSendersAccount = 3000m;

            this.Given(s => s.SenderTransfers(amount))
                .And(s => s.ServicesAreAvaiableAndWorkingCorrectly())
                .And(s => s.SenderCurrentyHaveOnAccount(amountInSendersAccount), $"Sender has {amountInSendersAccount}")
               .Then(s => s.ShouldPersist())
               .Then(s => s.ShouldSendChangesToConsumers())
               .When(s => s.ShouldHaveSubtractedFromSender(amount))
               .And(s => s.ShouldHaveAddedToReceiver(amount))
               .BDDfy();
        }
        [Test]
        public void InternalTransferReceiverAccountNotFound()
        {
            var amount = 200m;
            var amountInSendersAccount = 3000m;

            this.Given(s => s.SenderTransfers(amount))
                .And(s => s.SenderCurrentyHaveOnAccount(amountInSendersAccount), $"Sender has {amountInSendersAccount}")
                .And(s => s.ServicesAreAvaiableAndWorkingCorrectly())
               .Then(s => s.ReceiverAccountIsInvalid())
               .Then(s => s.ShouldSendChangesToConsumers())
               .Then(s => s.ShouldNotifyErrorToExchange())
               .BDDfy();
        }
        [Test]
        public void InternalTransferUnexpectedError()
        {
            var amount = 200m;
            var amountInSendersAccount = 3000m;

            this.Given(s => s.SenderTransfers(amount))
                .And(s => s.SenderCurrentyHaveOnAccount(amountInSendersAccount), $"Sender has {amountInSendersAccount}")
                .And(s => s.ServicesAreAvaiableAndWorkingCorrectly())
               .Then(s => s.AnErrorOccurrs())
               .Then(s => s.ShouldSendChangesToConsumers())
               .Then(s => s.ShouldNotifyErrorToExchange())
               .BDDfy();
        }

        [Test]
        public void InternalTransferInsufficientFunds()
        {
            var amount = 10000m;
            var amountInSendersAccount = 3000m;

            this.Given(s => s.SenderTransfers(20m))
                .And(s => s.SenderCurrentyHaveOnAccount(amountInSendersAccount), $"Sender has {amountInSendersAccount}")
                .And(s => s.ServicesAreAvaiableAndWorkingCorrectly())
               .Then(s => s.ShouldPersist())
               .Then(s => s.ShouldSendChangesToConsumers())
               .BDDfy();
        }
        private void ReceiverAccountIsInvalid()
        {
            Assert.Fail();
        }
        private void SenderCurrentyHaveOnAccount(decimal amount)
        {
            Assert.Fail();
        }
        private void ShouldHaveAddedToReceiver(decimal amount)
        {
            Assert.Fail();
        }
        private void ShouldHaveSubtractedFromSender(decimal amount)
        {
            Assert.Fail();
        }
        public void ShouldNotifyErrorToExchange()
        {
            Assert.Fail();
        }
        public void AnErrorOccurrs()
        {
            Assert.Fail();
        }
        public void ServicesAreAvaiableAndWorkingCorrectly()
        {

            Assert.Fail();
        }
        public void SenderTransfers(decimal amount)
        {
            transferDTO = fixture.Create<TransferDTO>();
            transferDTO.Value = amount;

        }

        public void ShouldPersist()
        {
            Assert.Fail();
        }
        public void ShouldSendChangesToConsumers()
        {
            Assert.Fail();
        }

    }
}
