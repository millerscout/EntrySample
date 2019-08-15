using AutoFixture;
using Bank.Entries.Core;
using Bank.Entries.Core.Interfaces;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Entries.Tests
{
    public class BaseTest
    {
        internal Mock<ITransferRepository> transferRepository;
        internal Fixture fixture = new Fixture();
        private TransferService transferService;

        public BaseTest()
        {
            transferRepository = new Mock<ITransferRepository>(MockBehavior.Strict);
            transferService = new TransferService(transferRepository.Object);
        }


        [TearDown]
        public void CleanUp()
        {
            transferRepository.VerifyAll();
            transferRepository.Reset();
        }

    }
}
