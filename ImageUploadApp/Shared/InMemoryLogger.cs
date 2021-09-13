using Microsoft.Extensions.Logging;
using System;

namespace ImageUploadApp.Shared
{
    public class InMemoryLogger : ILogger
    {
        //Declaring the properties and object
        
        private readonly string name;
        public InMemoryLog Memory { get; }

        //Initializinf the private property and class object
        public InMemoryLogger(string name, InMemoryLog memory)
        {
            this.name = name;
            Memory = memory;

        }

        //Scoping the logger to default
        IDisposable ILogger.BeginScope<TState>(TState state) => default;

        //Loglevel enabling link debug , information, warning, error. True states all log level are captured
        bool ILogger.IsEnabled(LogLevel logLevel) => true;

        //Used to write the log on the browser console
        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Console.WriteLine($"[{eventId.Id,2}:{logLevel,-12}] {name} - {formatter(state, exception)}");
            //Memory.LogItem($"[{eventId.Id,2}:{logLevel,-12}] {name} - {formatter(state, exception)}");
        }
    }


}