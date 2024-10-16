using System;
using System.IO;
using System.Threading;

public enum LogLevel
{
    INFO,
    WARNING,
    ERROR
}

public class Logger
{
    private static Logger instance;
    private static readonly object padlock = new object();
    private LogLevel logLevel = LogLevel.INFO;
    private string logFilePath = "log.txt";

    private Logger() { }

    public static Logger GetInstance()
    {
        lock (padlock)
        {
            if (instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }
    }

    public void SetLogLevel(LogLevel level)
    {
        logLevel = level;
    }

    public void SetLogFilePath(string path)
    {
        logFilePath = path;
    }

    public void Log(string message, LogLevel level)
    {
        if (level >= logLevel)
        {
            lock (padlock)
            {
                File.AppendAllText(logFilePath, $"{DateTime.Now}: [{level}] {message}{Environment.NewLine}");
            }
        }
    }
}

class Program
{
    static void Main()
    {
        Logger logger = Logger.GetInstance();
        logger.SetLogLevel(LogLevel.INFO);

        Thread thread1 = new Thread(() =>
        {
            logger.Log("Thread 1: Information message.", LogLevel.INFO);
            logger.Log("Thread 1: Warning message.", LogLevel.WARNING);
            logger.Log("Thread 1: Error message.", LogLevel.ERROR);
        });

        Thread thread2 = new Thread(() =>
        {
            logger.Log("Thread 2: Information message.", LogLevel.INFO);
            logger.Log("Thread 2: Warning message.", LogLevel.WARNING);
            logger.Log("Thread 2: Error message.", LogLevel.ERROR);
        });

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Console.WriteLine("Logging completed.");
    }
}
