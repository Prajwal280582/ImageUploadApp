using Microsoft.Extensions.Logging;
using System;

namespace ImageUploadApp.Shared
{
    public class InMemoryLogger : ILogger
    {
        private readonly string name;

        public InMemoryLog Memory { get; }

        public InMemoryLogger(string name, InMemoryLog memory)
        {
            this.name = name;
            Memory = memory;

        }

        IDisposable ILogger.BeginScope<TState>(TState state) => default;


        bool ILogger.IsEnabled(LogLevel logLevel) => true;


        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine($"[{eventId.Id,2}:{logLevel,-12}] {name} - {formatter(state, exception)}");
            //Memory.LogItem($"[{eventId.Id,2}:{logLevel,-12}] {name} - {formatter(state, exception)}");
        }
    }


}