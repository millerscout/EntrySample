using AutoFixture;
using Bank.Entries.Core;
using Bank.Entries.Core.DTOs;
using Bank.Entries.Core.Interfaces;
using Bank.Entries.Core.Models;
using Moq;
using NUnit.Framework;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Entries.Tests
{
    public class BaseTest
    {
        internal Mock<ITransferRepository> transferRepository;
        internal Fixture fixture = new Fixture();
        internal TransferService transferService;
        internal Mock<IConnection> rabbitConn;
        internal Mock<IModel> Channel { get; }
        #region Expressions

        internal readonly Expression<Func<ITransferRepository, Task<BankAccount>>> exprGetBankAccount
            = rep => rep.GetBankAccount(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>());

        internal readonly Expression<Func<ITransferRepository, Task<bool>>> exprTransfer
            = rep => rep.Transfer(It.IsAny<TransferDTO>());


        #endregion


        public BaseTest()
        {
            transferRepository = new Mock<ITransferRepository>(MockBehavior.Strict);
            rabbitConn = new Mock<IConnection>();
            Channel = new Mock<IModel>();

            transferService = new TransferService(transferRepository.Object, rabbitConn.Object);


        }


        [TearDown]
        public void CleanUp()
        {
            transferRepository.VerifyAll();
            transferRepository.Reset();


        }

    }
}
