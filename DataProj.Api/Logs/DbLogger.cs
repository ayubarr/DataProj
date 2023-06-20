using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace DataProj.Api.Logs
{
    public class DbLogger : ILogger
    {
        /// <summary>
        /// <summary>  
        /// Instance of <see cref="DbLoggerProvider" />.  
        /// </summary>  
        private readonly DbLoggerProvider _dbLoggerProvider;
        private readonly IServiceScopeFactory _scopeFactory;
        /// <summary>  
        /// Creates a new instance of <see cref="FileLogger" />.  
        /// </summary>  
        /// <param name="fileLoggerProvider">Instance of <see cref="FileLoggerProvider" />.</param>  
        public DbLogger([NotNull] DbLoggerProvider dbLoggerProvider, IServiceScopeFactory scopeFactory)
        {
            _dbLoggerProvider = dbLoggerProvider;
            _scopeFactory = scopeFactory;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>
        /// Whether to log the entry.
        /// </summary>
        /// <param name="logLevel"></param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }


        /// <summary>
        /// Used to log the entry.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel">An instance of <see cref="LogLevel"/>.</param>
        /// <param name="eventId">The event's ID. An instance of <see cref="EventId"/>.</param>
        /// <param name="state">The event's state.</param>
        /// <param name="exception">The event's exception. An instance of <see cref="Exception" /></param>
        /// <param name="formatter">A delegate that formats </param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                // Don't log the entry if it's not enabled.
                return;
            }

            var threadId = Thread.CurrentThread.ManagedThreadId; // Get the current thread ID to use in the log file. 

            // Store record.
            using (var connection = new SqlConnection(_dbLoggerProvider.Options.ConnectionString))
            {
                connection.Open();

                using (var command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = string.Format("INSERT INTO {0} ([Id], [LogLevel], [ThreadId], [EventId], [EventName], [Message], [ExceptionMessage], [ExceptionSource], [ExceptionStackTrace])" +
                        " VALUES (@Id, @LogLevel, @ThreadId, @EventId, @EventName, @Message, @ExceptionMessage, @ExceptionSource, @ExceptionStackTrace)", _dbLoggerProvider.Options.LogTable);


                    command.Parameters.Add(MakeParam("@Id", Guid.NewGuid()));
                    command.Parameters.Add(MakeParam("@LogLevel", logLevel.ToString()));
                    command.Parameters.Add(MakeParam("@ThreadId", threadId));
                    command.Parameters.Add(MakeParam("@EventId", eventId.Id));
                    command.Parameters.Add(MakeParam("@EventName", eventId.Name));
                    command.Parameters.Add(MakeParam("@Message", formatter(state, exception)));
                    command.Parameters.Add(MakeParam("@ExceptionMessage", exception?.Message));
                    command.Parameters.Add(MakeParam("@ExceptionSource", exception?.Source));
                    command.Parameters.Add(MakeParam("@ExceptionStackTrace", exception?.StackTrace));

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        private SqlParameter MakeParam(string key, object value)
        {
            if (value == null)
            {
                value = DBNull.Value;
            }
            SqlParameter param = new SqlParameter(key, value);

            return param;
        }
    }
}
