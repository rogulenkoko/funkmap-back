using System;
using NLog;

namespace Funkmap.Common.Logger
{
    public interface IFunkmapLogger<T> where T : class
    {
        void Info(string message);
        void Trace(string message);
        void Error(Exception exception, string message = "");
    }

    public class FunkmapLogger<T> : IFunkmapLogger<T> where T : class
    {
        private ILogger _logger;

        public FunkmapLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Error(Exception exception, string message = "")
        {
            _logger.Error($"{typeof(T).FullName}: {message}", exception);
        }

        public void Info(string message)
        {
            _logger.Info($"{typeof(T).FullName}: {message}");
        }

        public void Trace(string message)
        {
            _logger.Trace($"{typeof(T).FullName}: {message}");
        }
    }
}
