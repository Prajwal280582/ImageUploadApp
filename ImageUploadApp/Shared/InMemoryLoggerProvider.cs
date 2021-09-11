using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace ImageUploadApp.Shared
{
    public class InMemoryLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, InMemoryLogger> logger =
            new ConcurrentDictionary<string, InMemoryLogger>();

        public InMemoryLog Log { get; }
        public InMemoryLoggerProvider(InMemoryLog log)
        {
            Log = log;
        }
        public ILogger CreateLogger(string categoryName) =>
            logger.GetOrAdd(categoryName, name => new(name, Log));


        public void Dispose() => logger.Clear();
    }
}