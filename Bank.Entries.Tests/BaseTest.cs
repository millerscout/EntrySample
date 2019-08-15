using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Entries.Tests
{
    public class BaseTest
    {
        [TearDown]
        public void CleanUp() {
            //Verify Moqs
        }

    }
}
