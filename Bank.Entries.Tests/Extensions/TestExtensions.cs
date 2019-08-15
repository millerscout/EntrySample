using Moq;
using Moq.Language;
using Moq.Language.Flow;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace Bank.Entries.Tests
{
    public static class MockExtensions
    {
        public static ISetup<T, T2> SetupOn<T, T2>(this Expression<Func<T, T2>> expr, Mock<T> mock) where T : class
            => mock.Setup(expr);

        public static ISetupSequentialResult<T2> SetupSequenceOn<T, T2>(this Expression<Func<T, T2>> expr, Mock<T> mock) where T : class
            => mock.SetupSequence(expr);

        public static ISetup<T> SetupOn<T>(this Expression<Action<T>> expr, Mock<T> mock) where T : class
            => mock.Setup(expr);

        public static void VerifyOn<T, T2>(this Expression<Func<T, T2>> expr, Mock<T> mock, Times times) where T : class
            => mock.Verify(expr, times);

        public static void VerifyOn<T>(this Expression<Action<T>> expr, Mock<T> mock, Times times) where T : class
            => mock.Verify(expr, times);

        public static IEnumerable<IInvocation> GetInvocationsByName<T>(this Mock<T> mock, string methodName) where T : class
            => mock.Invocations.Where(a => a.Method.Name == methodName);

        public static T TryGetArgument<T>(this IInvocation invocation) where T : class
            => invocation.Arguments.FirstOrDefault(arg => arg is T) as T;

    }
}
