using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;
using Xunit.Sdk;
using Assert = Should.Core.Assertions.Assert;

namespace Should.Facts.Core
{
    public class TraceAssertTests
    {
        [Fact]
        public void TraceAssertFailureWithFullDetails()
        {
#if NETFRAMEWORK
            TraceAssertException ex = Assert.Throws<TraceAssertException>(() => Trace.Assert(false, "message", "detailed message"));

            Assert.Equal("message", ex.AssertMessage);
            Assert.Equal("detailed message", ex.AssertDetailedMessage);
            Assert.Equal("Debug.Assert() Failure : message" + Environment.NewLine + "Detailed Message:" + Environment.NewLine + "detailed message", ex.Message);
#else
            Exception ex = Record.Exception(() => Trace.Assert(false, "message", "detailed message"));

            Assert.Equal("Microsoft.VisualStudio.TestPlatform.TestHost.DebugAssertException", ex.GetType().FullName);
            Assert.Equal("Method <method> failed with 'message" + Environment.NewLine + "detailed message', and was translated to Microsoft.VisualStudio.TestPlatform.TestHost.DebugAssertException to avoid terminating the process hosting the test.", ex.Message);
#endif

        }

        [Fact]
        public void TraceAssertFailureWithNoDetailedMessage()
        {
#if NETFRAMEWORK
            TraceAssertException ex = Assert.Throws<TraceAssertException>(() => Trace.Assert(false, "message"));

            Assert.Equal("message", ex.AssertMessage);
            Assert.Equal("", ex.AssertDetailedMessage);
            Assert.Equal("Debug.Assert() Failure : message", ex.Message);
#else
            Exception ex = Record.Exception(() => Trace.Assert(false, "message"));

            Assert.Equal("Microsoft.VisualStudio.TestPlatform.TestHost.DebugAssertException", ex.GetType().FullName);
            Assert.Equal("Method <method> failed with 'message', and was translated to Microsoft.VisualStudio.TestPlatform.TestHost.DebugAssertException to avoid terminating the process hosting the test.", ex.Message);
#endif
        }

        [Fact]
        public void TraceAssertFailureWithNoMessage()
        {
#if NETFRAMEWORK
            TraceAssertException ex = Assert.Throws<TraceAssertException>(() => Trace.Assert(false));

            Assert.Equal("", ex.AssertMessage);
            Assert.Equal("", ex.AssertDetailedMessage);
            Assert.Equal("Debug.Assert() Failure", ex.Message);
#else
            Exception ex = Record.Exception(() => Trace.Assert(false));

            Assert.Equal("Microsoft.VisualStudio.TestPlatform.TestHost.DebugAssertException", ex.GetType().FullName);
            Assert.Equal("Method <method> failed with '', and was translated to Microsoft.VisualStudio.TestPlatform.TestHost.DebugAssertException to avoid terminating the process hosting the test.", ex.Message);
#endif
        }
    }
}

#if !NETFRAMEWORK
namespace Xunit.Sdk
{
    //
    // Zusammenfassung:
    //     Exception that is thrown when a call to Debug.Assert() fails.
    public class TraceAssertException : AssertException
    {
        private readonly string assertDetailedMessage;

        private readonly string assertMessage;

        //
        // Zusammenfassung:
        //     Gets the original assert detailed message.
        public string AssertDetailedMessage => assertDetailedMessage;

        //
        // Zusammenfassung:
        //     Gets the original assert message.
        public string AssertMessage => assertMessage;

        //
        // Zusammenfassung:
        //     Gets a message that describes the current exception.
        public override string Message
        {
            get
            {
                string text = "Debug.Assert() Failure";
                if (AssertMessage != "")
                {
                    text = text + " : " + AssertMessage;
                    if (AssertDetailedMessage != "")
                    {
                        string text2 = text;
                        text = text2 + Environment.NewLine + "Detailed Message:" + Environment.NewLine + AssertDetailedMessage;
                    }
                }

                return text;
            }
        }

        //
        // Zusammenfassung:
        //     Creates a new instance of the Xunit.Sdk.TraceAssertException class.
        //
        // Parameter:
        //   assertMessage:
        //     The original assert message
        public TraceAssertException(string assertMessage)
            : this(assertMessage, "")
        {
        }

        //
        // Zusammenfassung:
        //     Creates a new instance of the Xunit.Sdk.TraceAssertException class.
        //
        // Parameter:
        //   assertMessage:
        //     The original assert message
        //
        //   assertDetailedMessage:
        //     The original assert detailed message
        public TraceAssertException(string assertMessage, string assertDetailedMessage)
        {
            this.assertMessage = assertMessage ?? "";
            this.assertDetailedMessage = assertDetailedMessage ?? "";
        }
    }

    //
    // Zusammenfassung:
    //     The base assert exception class
    public class AssertException : Exception
    {
        private readonly string stackTrace;

        //
        // Zusammenfassung:
        //     Gets a string representation of the frames on the call stack at the time the
        //     current exception was thrown.
        //
        // Rückgabewerte:
        //     A string that describes the contents of the call stack, with the most recent
        //     method call appearing first.
        public override string StackTrace => FilterStackTrace(stackTrace ?? base.StackTrace);

        //
        // Zusammenfassung:
        //     Gets the user message
        public string UserMessage { get; protected set; }

        //
        // Zusammenfassung:
        //     Initializes a new instance of the Xunit.Sdk.AssertException class.
        public AssertException()
        {
        }

        //
        // Zusammenfassung:
        //     Initializes a new instance of the Xunit.Sdk.AssertException class.
        //
        // Parameter:
        //   userMessage:
        //     The user message to be displayed
        public AssertException(string userMessage)
            : base(userMessage)
        {
            UserMessage = userMessage;
        }

        //
        // Zusammenfassung:
        //     Initializes a new instance of the Xunit.Sdk.AssertException class.
        //
        // Parameter:
        //   userMessage:
        //     The user message to be displayed
        //
        //   innerException:
        //     The inner exception
        protected AssertException(string userMessage, Exception innerException)
            : base(userMessage, innerException)
        {
        }

        //
        // Zusammenfassung:
        //     Initializes a new instance of the Xunit.Sdk.AssertException class.
        //
        // Parameter:
        //   userMessage:
        //     The user message to be displayed
        //
        //   stackTrace:
        //     The stack trace to be displayed
        protected AssertException(string userMessage, string stackTrace)
            : base(userMessage)
        {
            this.stackTrace = stackTrace;
        }

        //
        // Zusammenfassung:
        //     Filters the stack trace to remove all lines that occur within the testing framework.
        //
        //
        // Parameter:
        //   stackTrace:
        //     The original stack trace
        //
        // Rückgabewerte:
        //     The filtered stack trace
        protected static string FilterStackTrace(string stackTrace)
        {
            if (stackTrace == null)
            {
                return null;
            }

            List<string> list = new List<string>();
            foreach (string item in SplitLines(stackTrace))
            {
                string text = item.TrimStart();
                if (!text.StartsWith("at Xunit.Assert.") && !text.StartsWith("at Xunit.Sdk."))
                {
                    list.Add(item);
                }
            }

            return string.Join(Environment.NewLine, list.ToArray());
        }

        private static IEnumerable<string> SplitLines(string input)
        {
            while (true)
            {
                int idx = input.IndexOf(Environment.NewLine);
                if (idx < 0)
                {
                    break;
                }

                yield return input.Substring(0, idx);
                input = input.Substring(idx + Environment.NewLine.Length);
            }

            yield return input;
        }
    }
}
#endif