namespace PMS_API.LogHandling
{
    public static class ApiLog
    {
        public static void Log(string FileName, string ErrorMessage, string StackTrace, int amoutOfSpacesToInclude = 0)
        {
            string currentPath = Directory.GetCurrentDirectory();
            if (!Directory.Exists(Path.Combine(currentPath, "Log")))
                Directory.CreateDirectory(Path.Combine(currentPath, "Log"));
            var currentDate = DateTime.Now.ToString("MM_dd_yyyy");
            using StreamWriter file = new StreamWriter($"Log\\Log {currentDate + DateTime.Now.Second.ToString()}_{FileName}.txt", append: true);
            for (var i = 1; i <= amoutOfSpacesToInclude; i++)
            {
                file.WriteLineAsync(" ");
            }
            file.WriteLineAsync("StackTrace:- " + StackTrace);
            file.WriteLineAsync("");
            file.WriteLineAsync("Error Message:- " + ErrorMessage);
            file.Dispose();
        }
    }

}
