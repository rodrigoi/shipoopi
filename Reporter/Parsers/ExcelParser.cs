using System;
using System.IO;
using System.Linq;
using ClosedXML.Excel;
using Ninject;
using Shipoopi.Reporter.Factories;
using Shipoopi.Reporter.Persistance;

namespace Shipoopi.Reporter.Parsers
{
    public abstract class ExcelParser : IParser
    {
        private string fileName;
        private string worksheetName;
        public IRepository repository { get; private set; }

        public ExcelParser(string fileName, string worksheetName)
        {
            this.fileName = fileName;
            this.worksheetName = worksheetName;
            repository = KernelFactory.GetKernel().Get<IRepository>();
        }

        public void Parse()
        {
            if (!File.Exists(fileName)) return;

            var workbook = new ClosedXML.Excel.XLWorkbook(fileName);
            var keymetrics = workbook.Worksheets.Where(x => x.Name.ToLower() == worksheetName.ToLower()).FirstOrDefault();

            var lastRowUsed = keymetrics.LastRowUsed().RowNumber();
            int cursorLocation = Console.CursorTop;
            for (int row = 0; row <= lastRowUsed; row++)
            {
                Console.SetCursorPosition(0, cursorLocation);
                Console.Write("Processing line {0} of {1}", row, lastRowUsed);

                ParseLine(keymetrics.Row(row));
            }
        }

        protected virtual void ParseLine(IXLRow row) { }
    }
}