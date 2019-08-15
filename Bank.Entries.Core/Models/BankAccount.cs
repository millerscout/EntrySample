using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Entries.Core.Models
{
    public class BankAccount
    {
        public int Id { get; private set; }
        public int Branch { get; private set; }
        public int Number { get; private set; }
        public int Digit { get; private set; }
        public decimal Balance { get; private set; }

        protected BankAccount() { }

        public BankAccount(int id, int branch, int number, int digit, decimal balance)
        {
            this.Id = id;
            this.Branch = branch;
            this.Number = number;
            this.Digit = digit;
            this.Balance = balance;
        }
    }
}
