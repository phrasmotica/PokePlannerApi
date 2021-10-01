using System;
using System.Runtime.CompilerServices;

namespace PokePlannerApi.Data.Util
{
    /// <summary>
    /// Disposable for timing code block execution.
    /// </summary>
    public class CodeTimer : IDisposable
    {
        /// <summary>
        /// The name of the calling method or some other identifier for the code block.
        /// </summary>
        private readonly string MethodName;

        /// <summary>
        /// The start time of the code execution.
        /// </summary>
        private readonly DateTime Start;

        /// <summary>
        /// The end time of the code execution.
        /// </summary>
        private DateTime End;

        /// <summary>
        /// Constructor.
        /// </summary>
        public CodeTimer([CallerMemberName] string methodName = "")
        {
            MethodName = methodName;
            Start = DateTime.Now;
        }

        /// <summary>
        /// Print timing to console.
        /// </summary>
        public void Dispose()
        {
            End = DateTime.Now;
            var timeTaken = (End - Start).TotalMilliseconds;
            Console.WriteLine($"{MethodName}() ran in {timeTaken:0ms}");
        }
    }
}
