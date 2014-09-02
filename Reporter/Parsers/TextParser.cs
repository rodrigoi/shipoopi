using System;
using System.IO;
using System.Linq;
using Ninject;
using Shipoopi.Reporter.Factories;
using Shipoopi.Reporter.Persistance;

namespace Shipoopi.Reporter.Parsers
{
    public abstract class TextParser : IParser
    {
        private string fileName;
        public IRepository repository { get; private set; }

        public TextParser(string filePath)
        {
            this.fileName = filePath;
            repository = KernelFactory.GetKernel().Get<IRepository>();
        }

        public void Parse()
        {
            if (!File.Exists(fileName)) return;
            var lineCount = File.ReadLines(fileName).Count();
            using (var reader = File.OpenText(fileName))
            {
                string line;
                int currentLine = 1;
                int cursorLocation = Console.CursorTop;
                while ((line = reader.ReadLine()) != null)
                {
                    Console.SetCursorPosition(0, cursorLocation);
                    Console.Write("Processing line {0} of {1}", currentLine, lineCount);
                    ParseLine(line);
                    currentLine++;
                }
            }
        }

        protected virtual void ParseLine(string line) { }
    }
}