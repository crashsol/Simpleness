using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Simpleness.Infrastructure.AspNetCore.Logger
{
    /// <summary>
    /// 日志扩展类(提交日志记录速度)
    /// </summary>
    public static class LoggerExtensions
    {

        private static readonly Action<ILogger, string, Exception> _quoteAdded;
        private static readonly Action<ILogger, string, int, Exception> _quoteDeleted;
        private static readonly Action<ILogger, int, Exception> _quoteDeleteFailed;

        static LoggerExtensions()
        {



            _quoteAdded = LoggerMessage.Define<string>(
                LogLevel.Information,
                new EventId(2, nameof(QuoteAdded)),
                "Quote added (Quote = '{Quote}')");

            _quoteDeleted = LoggerMessage.Define<string, int>(
                LogLevel.Information,
                new EventId(4, nameof(QuoteDeleted)),
                "Quote deleted (Quote = '{Quote}' Id = {Id})");

            _quoteDeleteFailed = LoggerMessage.Define<int>(
                LogLevel.Error,
                new EventId(5, nameof(QuoteDeleteFailed)),
                "Quote delete failed (Id = {Id})");

        }

     
        public static void QuoteAdded(this ILogger logger, string quote)
        {
            _quoteAdded(logger, quote, null);
        }
     

        public static void QuoteModified(this ILogger logger, string id, string priorQuote, string newQuote)
        {
            // Reserve for future feature
        }

     
        public static void QuoteDeleted(this ILogger logger, string quote, int id)
        {
            _quoteDeleted(logger, quote, id, null);
        }

        public static void QuoteDeleteFailed(this ILogger logger, int id, Exception ex)
        {
            _quoteDeleteFailed(logger, id, ex);
        }


    }
}
