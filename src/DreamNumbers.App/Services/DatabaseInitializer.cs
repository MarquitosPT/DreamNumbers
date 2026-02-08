namespace DreamNumbers.Services
{
    internal static class DatabaseInitializer
    {
        public static string InitializeDatabase()
        {
            var dbName = "DreamNumbers.db";
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, dbName);

            if (!File.Exists(dbPath))
            {
                using var stream = FileSystem.OpenAppPackageFileAsync(dbName).Result;
                using var newFile = File.Create(dbPath);

                stream.CopyTo(newFile);
            }

            return dbPath;
        }

    }
}
