using System;
using System.Threading.Tasks;
using Xunit;
using Assert = Should.Core.Assertions.Assert;

namespace Should.Facts.Core
{
    public class RecordTests
    {
        public class MethodsWithoutReturnValues
        {
#if NETFRAMEWORK
            [Fact]
            public void Exception()
            {
                Exception ex = Record.Exception(delegate { throw new InvalidOperationException(); });

                Assert.NotNull(ex);
                Assert.IsType<InvalidOperationException>(ex);
            }

            [Fact]
            public void NoException()
            {
                Exception ex = Record.Exception(delegate { });

                Assert.Null(ex);
            }
#endif

//[Fact]
//public void Exception()
//{
//    Exception ex = Record.Exception(() => throw new InvalidOperationException());
//
//    Assert.NotNull(ex);
//    Assert.IsType<InvalidOperationException>(ex);
//}
//
//[Fact]
//public void NoException()
//{
//    // Test für synchronen Code bleibt gleich
//    Exception ex = Record.Exception(() => { });
//
//    Assert.Null(ex);
//}

#if !NETFRAMEWORK
            [Fact]
            public async Task AsyncException()
            {
                // Beispiel für asynchrone Ausnahme
                Exception ex = await Record.ExceptionAsync(async () =>
                {
                    await Task.Delay(10); // Beispiel für asynchronen Code
                    throw new InvalidOperationException();
                });

                Assert.NotNull(ex);
                Assert.IsType<InvalidOperationException>(ex);
            }

            [Fact]
            public async Task NoAsyncException()
            {
                // Beispiel für asynchronen Code ohne Ausnahme
                Exception ex = await Record.ExceptionAsync(async () =>
                {
                    await Task.Delay(10); // Beispiel für asynchronen Code
                });

                Assert.Null(ex);
            }
#endif
        }

        public class MethodsWithReturnValues
        {
            [Fact]
            public void Exception()
            {
                StubAccessor accessor = new StubAccessor();

                Exception ex = Record.Exception(() => accessor.FailingProperty);

                Assert.NotNull(ex);
                Assert.IsType<InvalidOperationException>(ex);
            }

            [Fact]
            public void NoException()
            {
                StubAccessor accessor = new StubAccessor();

                Exception ex = Record.Exception(() => accessor.SuccessfulProperty);

                Assert.Null(ex);
            }

            class StubAccessor
            {
                public int SuccessfulProperty { get; set; }

                public int FailingProperty
                {
                    get { throw new InvalidOperationException(); }
                }
            }
        }
    }
}