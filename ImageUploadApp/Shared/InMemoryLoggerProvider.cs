using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace ImageUploadApp.Shared
{
    public class InMemoryLoggerProvider : ILoggerProvider
    {
        //Defining the Class objects
        private readonly ConcurrentDictionary<string, InMemoryLogger> logger =
            new ConcurrentDictionary<string, InMemoryLogger>();

        public InMemoryLog Log { get; }

        //Iniliazing the class object
        public InMemoryLoggerProvider(InMemoryLog log)
        {
            Log = log;
        }

        //Creating and fetching the logger 
        public ILogger CreateLogger(string categoryName) =>
            logger.GetOrAdd(categoryName, name => new(name, Log));

        //Clearing the logger object
        public void Dispose() => logger.Clear();
    }
}