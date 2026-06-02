using System;
using System.IO;

namespace BlockBlast
{
    public sealed class FileBestScoreStore : IBestScoreStore
    {
        private readonly string _filePath;

        public FileBestScoreStore(string fileName)
        {
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        }

        public int Load()
        {
            if (!File.Exists(_filePath))
                return 0;

            string text = File.ReadAllText(_filePath);

            return int.TryParse(text, out int best)
                ? best
                : 0;
        }

        public void Save(int best)
        {
            File.WriteAllText(_filePath, best.ToString());
        }
    }
}