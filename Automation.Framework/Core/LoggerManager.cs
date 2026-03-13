using Serilog;
using Serilog.Events;
using Serilog.Core;

namespace Automation.Framework.Core
{
    /// <summary>
    /// Manages logging for the automation framework.
    ///
    /// Each test receives it's own logger instance. 
    /// Logs are written to memory while test runs. 
    /// A physical log file is only created if the test fails. 
    /// </summary>
    public class LoggerManager
    {
        private readonly Dictionary<string, List<string>> _logs = new();
        private ILogger _currentLogger;

        public ILogger CreateTestLogger(string testName)
        {
            /// <summary>
            /// Create a logger for a specific test. 
            /// Logs are stored in memory in a list until the test finishes.
            /// </summary>

            _logs[testName] = new List<string>();

            //Simple logger that writes into the list
            _currentLogger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Sink(new InMemoryListSink(_logs[testName]))
                .CreateLogger();

            return _currentLogger;
        }

        public void LogInfo(string message)
        {
            _currentLogger?.Information(message);
        }

        public void LogWarning(string message)
        {
            _currentLogger?.Warning(message);
        }

        public void LogError(string message)
        {
            _currentLogger?.Error(message);
        }

        public void WriteLogIfFailed(string testName, Exception ex)
        {
            /// <summary>
            /// Writes the captured logs to the test output if the test failed.
            /// This is called from BaseTest.DisposeAsync when a test failure is detected.
            /// </summary>
            if (!_logs.ContainsKey(testName))
                return;

            string currentDir = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = new DirectoryInfo(currentDir);
            string pathToUse = directoryInfo.Parent?.Parent?.Parent?.FullName ?? currentDir;
            var logDir = Path.Combine(pathToUse,"TestResults", "Logs");
            if (!Directory.Exists(logDir))
                Directory.CreateDirectory(logDir);

            var logName = $"{testName}.log";
            var logPath = Path.Combine(logDir, logName);
            File.WriteAllLines(logPath, _logs[testName]);
            File.AppendAllText(logPath, Environment.NewLine + ex.ToString());
        }
    }

    public class InMemoryListSink : ILogEventSink
    {
        /// <summary>
        /// Custom Serilog sink used to capture log events
        /// into a simple List<string>
        /// This allows us to store logs in memory until we determine
        /// whether the test passed or failed.
        /// </summary>
        private readonly List<string> _entries;
        public InMemoryListSink(List<string> entries)
        {
            //Constructor receives the storage list where log entries will be added 
            _entries = entries;
        }

        public void Emit(LogEvent logEvent)
        {
            //Called by the Serlog wheever a log even occurs. 
            //Converts the event into a formatted string and adds it to the list.
            var renderedMessage = logEvent.RenderMessage();
            _entries.Add($"[{logEvent.Timestamp:HH:mm:ss}][{logEvent.Level}] {renderedMessage}");
            if (logEvent.Exception != null)
                _entries.Add(logEvent.Exception.ToString());
        }
    }
}
