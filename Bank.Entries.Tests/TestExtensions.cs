using Moq;
using Moq.Language.Flow;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Bank.Entries.Tests
{
    public static class MockExtensions
    {
        public static ISetup<T, T2> SetupOn<T, T2>(this Expression<Func<T, T2>> expr, Mock<T> mock) where T : class
        {
            return mock.Setup(expr);
        }

        public static ISetup<T> SetupOn<T>(this Expression<Action<T>> expr, Mock<T> mock) where T : class
        {
            return mock.Setup(expr);
        }

        public static void VerifyOn<T, T2>(this Expression<Func<T, T2>> expr, Mock<T> mock, Times times) where T : class
        {
            mock.Verify(expr, times);
        }

        public static void VerifyOn<T>(this Expression<Action<T>> expr, Mock<T> mock, Times times) where T : class
        {
            mock.Verify(expr, times);
        }

    }
}
