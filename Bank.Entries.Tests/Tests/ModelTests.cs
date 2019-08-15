using AutoFixture;
using Bank.Entries.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Entries.Tests
{
    public class ModelTests
    {
        [TestCase(12.40d)]
        [TestCase(1.99d)]
        [TestCase(0.99d)]
        [TestCase(1200.01d)]
        [TestCase(-12.40d)]
        [TestCase(-1.99d)]
        [TestCase(-0.99d)]
        [TestCase(-1200.01d)]
        public void ShouldUpdateValue(decimal amount)
        {
            var fixture = new Fixture();

            
            var bankAccount = fixture.Create<BankAccount>();

            var beforeUpdate = bankAccount.Balance;
            bankAccount.UpdateBalance(amount);
            Assert.AreEqual(beforeUpdate + amount, bankAccount.Balance);
        }
    }
}
