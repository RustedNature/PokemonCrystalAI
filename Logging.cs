namespace PokeTorchAi
{
    internal static class Logging
    {
        private const string LogFilePath = @".\LOGS\";
        private const string LogFileName = @"LogFile.txt";
        private static string logContent = "";
        private static int currentRunNumber = 0;


        internal static void UpdateLogContent(string contentToLog, int runNumber)
        {
            currentRunNumber = runNumber;
            logContent += CreateLogString(contentToLog);
        }
        internal static void UpdateLogContent(string contentToLog)
        {
            logContent += CreateLogString(contentToLog);
        }
        private static string CreateLogString(string contentToLog)
        {
            return $"Run: {currentRunNumber} | Time: {TimeOnly.FromDateTime(DateTime.Now)} ::::: {contentToLog}\n";
        }

        internal static void WriteToFile()
        {
            if (Directory.Exists(LogFilePath))
            {
                if (File.Exists(LogFilePath + LogFileName))
                {
                    File.AppendAllText(LogFilePath + LogFileName, logContent);
                    logContent = string.Empty;
                }
                else
                {
                    File.WriteAllText(LogFilePath + LogFileName, logContent);
                    logContent = string.Empty;
                }
            }
            else
            {
                Directory.CreateDirectory(LogFilePath);

            }
        }

    }
}
