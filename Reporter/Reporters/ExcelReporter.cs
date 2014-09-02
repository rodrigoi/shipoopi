using System;
using System.Linq;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Ninject;
using Shipoopi.Reporter.Factories;
using Shipoopi.Reporter.Model;
using Shipoopi.Reporter.Persistance;

namespace Shipoopi.Reporter.Reporters
{
    public class ExcelReporter : IReporter
    {
        private string fileName;
        public IRepository repository { get; private set; }

        public ExcelReporter(string fileName)
        {
            this.fileName = fileName;
            repository = KernelFactory.GetKernel().Get<IRepository>();
        }

        public void GenerateReport()
        {
            var workbook = new XLWorkbook();

            GeneratePerformanceReport(workbook);
            GenerateItemsReport(workbook);

            workbook.SaveAs(fileName);
            
            //AddConditionalFormatting();
        }

        private void GeneratePerformanceReport(XLWorkbook workbook)
        {
            Console.WriteLine("Computing Performance Report");
            int cursorLocation = Console.CursorTop;

            var performanceSheet = workbook.Worksheets.Add("Performance");
            AddPerformanceTitles(performanceSheet);
            
            var facebookInsights = repository.Get<FacebookInsights>("Date", false);
            int n = 2;
            foreach (var insight in facebookInsights)
            {
                Console.SetCursorPosition(0, cursorLocation);
                Console.Write("Processing Date: {0} ({1} of {2})", insight.Date.ToString("dd-MM-yyyy"), n-1, facebookInsights.Count);

                AddFacebookInsight(performanceSheet, n, insight);
                n++;
            }
            AddPerformanceFormatting(performanceSheet);

            Console.WriteLine();
            Console.WriteLine();
        }

        private void AddFacebookInsight(IXLWorksheet performanceSheet, int n, FacebookInsights insight)
        {
            performanceSheet.Cell(n, 1).Value = insight.Date; //Date
            performanceSheet.Cell(n, 3).Value = insight.DailyActiveUsers; //DAU
            performanceSheet.Cell(n, 4).FormulaA1 = string.Format("C{0}-C{1}", n, n + 1); //Q Users
            performanceSheet.Cell(n, 5).FormulaA1 = string.Format("IF(C{0}=0,0, D{1}/C{2})", n + 1, n, n + 1); //% Users
            performanceSheet.Cell(n, 6).Value = insight.MonthlyActiveUsers; //MAU
            performanceSheet.Cell(n, 7).FormulaA1 = string.Format("F{0}-F{1}", n, n + 1); //Q users
            performanceSheet.Cell(n, 8).FormulaA1 = string.Format("IF(F{0}=0,0,G{1}/F{2})", n + 1, n, n + 1); //% Users
            performanceSheet.Cell(n, 9).FormulaA1 = string.Format("IF(F{0}=0,0,C{1}/F{2})", n, n, n); //DAU/MAU
            performanceSheet.Cell(n, 10).FormulaA1 = string.Format("IF(G{0}=0,0,D{1}/G{2})", n, n, n); //DAU INDEX
            performanceSheet.Cell(n, 11).Value = insight.DailyAppInstalls; //Instalaciones

            var virality = repository.Get<Virality, DateTime>(insight.Date);
            if (virality == null) virality = new Virality();
            performanceSheet.Cell(n, 12).Value = virality.Total; //K Factor (overall)
            performanceSheet.Cell(n, 13).Value = virality.Invite; //K Factor invite
            performanceSheet.Cell(n, 14).Value = virality.Notification; //K Notification
            performanceSheet.Cell(n, 15).Value = virality.TotalInstallations; //Viral Installs
            performanceSheet.Cell(n, 16).FormulaA1 = string.Format("K{0}-O{0}", n, n); //MKT Installs

            var analytics = repository.Get<Analytics, DateTime>(insight.Date);
            if (analytics == null) analytics = new Analytics();
            performanceSheet.Cell(n, 17).Value = analytics.Visits; //Visits
            performanceSheet.Cell(n, 18).FormulaA1 = string.Format("IF(C{0}=0,0,Q{1}/C{2})", n, n, n); //Daily Sessions
            TimeSpan timeOnSite = new TimeSpan(0, 0, analytics.TimeOnSite);
            performanceSheet.Cell(n, 19).Value = timeOnSite.ToString(); //Time on site

            var revenue = repository.Get<Revenue, DateTime>(insight.Date);
            if (revenue == null) revenue = new Revenue();
            performanceSheet.Cell(n, 20).Value = revenue.TotalRevenue; //Daily Revenue
            performanceSheet.Cell(n, 21).FormulaA1 = string.Format("IF(C{0}=0,0,T{1}/C{2})", n, n, n); //ARP DAU
            performanceSheet.Cell(n, 22).FormulaA1 = string.Format("IF(F{0}=0,0,T{1}/F{2})", n, n, n); //ARP MAU
            performanceSheet.Cell(n, 23).FormulaA1 = string.Format("IF(X{0}=0,0, T{1}/X{2})", n, n, n); //ARP PU
            performanceSheet.Cell(n, 24).Value = revenue.TransactionCount; ///Pay Users
            performanceSheet.Cell(n, 25).FormulaA1 = string.Format("X{0}/C{0}", n, n); //% Pay. Users
            performanceSheet.Cell(n, 26).FormulaA1 = string.Format("L{0}*T{0}*I{0}", n, n); //LTV
            performanceSheet.Cell(n, 27).Value = string.Empty; //CPA

            performanceSheet.Cell(n, 28).FormulaA1 = "K" + n.ToString(); // Level 1
            performanceSheet.Cell(n, 29).FormulaA1 = string.Format("IF(C{0}=0,0,AB{1}/C{2})", n, n, n); //%DAU

            performanceSheet.Cell(n, 30).Value = analytics.Level2; //Level 2
            performanceSheet.Cell(n, 31).FormulaA1 = string.Format("IF(C{0}=0,0,AD{1}/C{2})", n, n, n); //%DAU
            performanceSheet.Cell(n, 32).FormulaA1 = string.Format("IF(AB{0}=0,0,AD{1}/AB{2})", n, n, n); //% Pasaje

            performanceSheet.Cell(n, 33).Value = analytics.Level3; //Level 3
            performanceSheet.Cell(n, 34).FormulaA1 = string.Format("IF(C{0}=0,0,AG{1}/C{2})", n, n, n); //%DAU
            performanceSheet.Cell(n, 35).FormulaA1 = string.Format("IF(AD{0}=0,0,AG{1}/AD{2})", n, n, n); //% Pasaje

            performanceSheet.Cell(n, 36).Value = analytics.Level4; //Level 4
            performanceSheet.Cell(n, 37).FormulaA1 = string.Format("IF(C{0}=0,0,AJ{1}/C{2})", n, n, n); //%DAU
            performanceSheet.Cell(n, 38).FormulaA1 = string.Format("IF(AG{0}=0,0,AJ{1}/AG{2})", n, n, n); //% Pasaje

            performanceSheet.Cell(n, 39).Value = analytics.Level5; //Level 5
            performanceSheet.Cell(n, 40).FormulaA1 = string.Format("IF(C{0}=0,0,AM{1}/C{2})", n, n, n); //%DAU
            performanceSheet.Cell(n, 41).FormulaA1 = string.Format("IF(AJ{0}=0,0,AM{1}/AJ{2})", n, n, n); //% Pasaje

            performanceSheet.Cell(n, 42).Value = string.Empty; //Weekly Average
            performanceSheet.Cell(n, 43).Value = string.Empty; //Monthly Average
            if (insight.Date.DayOfWeek == DayOfWeek.Saturday || insight.Date.DayOfWeek == DayOfWeek.Sunday)
            {
                performanceSheet.Row(n).Style.Fill.BackgroundColor = XLColor.Yellow;
            }
        }
        private void AddPerformanceFormatting(IXLWorksheet performanceSheet)
        {
            int lastRowUsed = performanceSheet.LastRowUsed().RowNumber();
            //freeze panels
            performanceSheet.SheetView.Freeze(1, 2);
            //performance global styles
            performanceSheet.Range(1, 1, performanceSheet.LastCellUsed().Address.RowNumber, performanceSheet.LastCellUsed().Address.ColumnNumber)
                .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            //column specific formatting

            //date columns
            performanceSheet.Range(string.Format("A2:A{0}", lastRowUsed)).Style.DateFormat.Format = "dd/MM/yyyy";

            //percentage formatting (0.00%)
            performanceSheet.Ranges(string.Format("E2:E{0},H2:I{1},Y2:Y{2},AC2:AC{3},AE2:AF{4},AH2:AI{5},AK2:AL{6},AN2:AO{7}",
                lastRowUsed, lastRowUsed, lastRowUsed, lastRowUsed, lastRowUsed, lastRowUsed, lastRowUsed, lastRowUsed
            )).Style.NumberFormat.NumberFormatId = 10;

            //no decimal points
            performanceSheet.Range(string.Format("R2:R{0}", lastRowUsed)).Style.NumberFormat.Format = "0";

            //decimal format (0.00)
            performanceSheet.Ranges(string.Format("J2:J{0},L2:N{1}",
                lastRowUsed, lastRowUsed
            )).Style.NumberFormat.Format = "0.00";

            //three decimal points (0.000)
            performanceSheet.Range(string.Format("U2:U{0}",
                lastRowUsed
            )).Style.NumberFormat.Format = "0.000";

            //money with two decimals ($ 0.00)
            performanceSheet.Ranges(string.Format("T2:T{0},W2:W{1},Z2:Z{2}",
                lastRowUsed, lastRowUsed, lastRowUsed
            )).Style.NumberFormat.Format = "$ 0.00";

            //money with three decimals ($ 0.000)
            performanceSheet.Range(string.Format("V2:V{0}",
                lastRowUsed, lastRowUsed, lastRowUsed
            )).Style.NumberFormat.Format = "$ 0.000";

            // adjust to content
            performanceSheet.Columns().AdjustToContents();
        }
        private void AddPerformanceTitles(IXLWorksheet performanceSheet)
        {
            //global styles
            performanceSheet.Style.Font.FontName = "verdana";
            performanceSheet.Style.Font.FontSize = 10d;
            //headings
            performanceSheet.Cell(1, 1).Value = "Date";
            performanceSheet.Cell(1, 2).Value = "Comment";
            performanceSheet.Cell(1, 3).Value = "DAU";
            performanceSheet.Cell(1, 4).Value = "Q Users";
            performanceSheet.Cell(1, 5).Value = "% Users";
            performanceSheet.Cell(1, 6).Value = "MAU";
            performanceSheet.Cell(1, 7).Value = "Q users";
            performanceSheet.Cell(1, 8).Value = "% users";
            performanceSheet.Cell(1, 9).Value = "DAU/MAU";
            performanceSheet.Cell(1, 10).Value = "DAU INDEX";
            performanceSheet.Cell(1, 11).Value = "Instalaciones";
            performanceSheet.Cell(1, 12).Value = "K Factor (overall)";
            performanceSheet.Cell(1, 13).Value = "K Factor invite";
            performanceSheet.Cell(1, 14).Value = "K Notification";
            performanceSheet.Cell(1, 15).Value = "Viral Installs";
            performanceSheet.Cell(1, 16).Value = "MKT Installs";
            performanceSheet.Cell(1, 17).Value = "Visits";
            performanceSheet.Cell(1, 18).Value = "Daily Sessions";
            performanceSheet.Cell(1, 19).Value = "Time on site";
            performanceSheet.Cell(1, 20).Value = "Daily Revenue";
            performanceSheet.Cell(1, 21).Value = "ARP DAU";
            performanceSheet.Cell(1, 22).Value = "ARP MAU";
            performanceSheet.Cell(1, 23).Value = "ARP PU";
            performanceSheet.Cell(1, 24).Value = "Pay Users";
            performanceSheet.Cell(1, 25).Value = "% Pay. Users";
            performanceSheet.Cell(1, 26).Value = "LTV";
            performanceSheet.Cell(1, 27).Value = "CPA";
            performanceSheet.Cell(1, 28).Value = "LVL 1";
            performanceSheet.Cell(1, 29).Value = "%DAU";
            performanceSheet.Cell(1, 30).Value = "LVL 2";
            performanceSheet.Cell(1, 31).Value = "%DAU";
            performanceSheet.Cell(1, 32).Value = "% Pasaje";
            performanceSheet.Cell(1, 33).Value = "LVL 3";
            performanceSheet.Cell(1, 34).Value = "%DAU";
            performanceSheet.Cell(1, 35).Value = "% Pasaje";
            performanceSheet.Cell(1, 36).Value = "LVL 4";
            performanceSheet.Cell(1, 37).Value = "%DAU";
            performanceSheet.Cell(1, 38).Value = "% Pasaje";
            performanceSheet.Cell(1, 39).Value = "LVL5";
            performanceSheet.Cell(1, 40).Value = "%DAU";
            performanceSheet.Cell(1, 41).Value = "% Pasaje";
            performanceSheet.Cell(1, 42).Value = "Weekly Average";
            performanceSheet.Cell(1, 43).Value = "Monthly Average";
            //row one styles
            performanceSheet.Row(1).Style.Font.Bold = true;
        }

        private void GenerateItemsReport(XLWorkbook workbook)
        {
            Console.WriteLine("Computing Items Report");

            var items = repository.Get<Shipoopi.Reporter.Model.Item>("ItemId", true);
            var dates = repository.GetItemEventDates();

            Console.WriteLine("{0} time periods found", dates.Count);
            int cursorLocation = Console.CursorTop;

            var itemsSheet = workbook.Worksheets.Add("Items");
            //global styles
            itemsSheet.Style.Font.FontName = "verdana";
            itemsSheet.Style.Font.FontSize = 10d;
            //headings
            itemsSheet.Cell(1, 1).Value = "Id";
            itemsSheet.Cell(1, 2).Value = "Nombre";
            itemsSheet.Cell(1, 3).Value = "Frame";
            itemsSheet.Cell(1, 4).Value = "Categoria";
            itemsSheet.Cell(1, 5).Value = "Tipo";
            itemsSheet.Cell(1, 6).Value = "Special";
            itemsSheet.Cell(1, 7).Value = "Upgrade";
            itemsSheet.Cell(1, 8).Value = "Premium";
            for (int i = 0; i < dates.Count(); i++)
            {
                itemsSheet.Cell(1, 9 + i).Value = dates[i];
            }
            itemsSheet.Range("I1:" + itemsSheet.Cell(1, dates.Count + 8).Address).Style.DateFormat.Format = "dd-MM-yyyy";
            itemsSheet.Cell(1, dates.Count() + 9).Value = "Total";
            itemsSheet.Row(1).Style.Font.Bold = true;

            int n = 2;
            foreach (var item in items)
            {
                Console.SetCursorPosition(0, cursorLocation);
                Console.Write(string.Format("Processing Item {0} of {1}: {2} - {3}", n - 1, items.Count, item.ItemId, item.Name).PadRight(80, ' '));

                itemsSheet.Cell(n, 1).Value = item.ItemId;
                itemsSheet.Cell(n, 2).Value = item.Name;
                itemsSheet.Cell(n, 3).Value = item.Frame;
                itemsSheet.Cell(n, 4).Value = item.CategoryName;
                itemsSheet.Cell(n, 5).Value = item.Type;
                itemsSheet.Cell(n, 6).Value = item.Special;
                itemsSheet.Cell(n, 7).Value = item.Upgrade;
                itemsSheet.Cell(n, 8).Value = item.Premium;

                for (int i = 0; i < dates.Count(); i++)
                {
                    var itemEvent = repository.GetItemEvent(item.ItemId, dates[i]);
                    if (itemEvent == null) itemEvent = new ItemEvents();
                    itemsSheet.Cell(n, 9 + i).Value = itemEvent.TotalEvents;
                }
                //+8 because this calculates a range, not the position of the data
                itemsSheet.Cell(n, dates.Count() + 9).FormulaA1 = string.Format("SUM({0}:{1})", itemsSheet.Cell(n, 9).Address, itemsSheet.Cell(n, dates.Count() + 8).Address);

                n++;
            }

            //freeze panels
            itemsSheet.SheetView.Freeze(1, 1);
            //performance global styles
            itemsSheet.Range(1, 1, itemsSheet.LastCellUsed().Address.RowNumber, itemsSheet.LastCellUsed().Address.ColumnNumber)
                .Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            itemsSheet.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
            // adjust to content
            itemsSheet.Columns().AdjustToContents();

            //create table and set autofilters
            //var dataTable = itemsSheet.Range("A1:" + itemsSheet.LastCellUsed().Address).CreateTable();
            //dataTable.ShowAutoFilter = true;
            //dataTable.ShowColumnStripes = false;
            //dataTable.ShowRowStripes = false;
            //dataTable.ShowTotalsRow = false;
            //itemsSheet.Row(1).Style.Fill.BackgroundColor = XLColor.White;
            //itemsSheet.Row(1).Style.Font.FontColor = XLColor.Black;

            Console.WriteLine();
            Console.WriteLine();
        }
        private void AddConditionalFormatting()
        {
            //use openxml directly to apply conditional formatting.
            using (SpreadsheetDocument report = SpreadsheetDocument.Open(fileName, true))
            {
                Workbook workbook = report.WorkbookPart.Workbook;
                WorksheetPart worksheetPart = workbook.WorkbookPart.WorksheetParts.First();

                DifferentialFormats differentialFormats = new DifferentialFormats() { Count = (UInt32Value)2U };

                DifferentialFormat lessThanFormat = new DifferentialFormat();
                Font lessThanFont = new Font();
                lessThanFont.Append(new Condense() { Val = false });
                lessThanFont.Append(new Extend() { Val = false });
                lessThanFont.Append(new Color() { Rgb = "FF9C0006" });
                Fill lessThanFill = new Fill();
                PatternFill lessThanPatternFill = new PatternFill();
                lessThanPatternFill.Append(new BackgroundColor() { Rgb = "FFFFC7CE" });
                lessThanFill.Append(lessThanPatternFill);
                lessThanFormat.Append(lessThanFont);
                lessThanFormat.Append(lessThanFill);

                DifferentialFormat greaterThanFormat = new DifferentialFormat();
                Font greaterThanFont = new Font();
                greaterThanFont.Append(new Condense() { Val = false });
                greaterThanFont.Append(new Extend() { Val = false });
                greaterThanFont.Append(new Color() { Rgb = "FF006100" });
                Fill greatherThanFill = new Fill();
                PatternFill greaterThanPatternFill = new PatternFill();
                greaterThanPatternFill.Append(new BackgroundColor() { Rgb = "FFC6EFCE" });
                greatherThanFill.Append(greaterThanPatternFill);
                greaterThanFormat.Append(greaterThanFont);
                greaterThanFormat.Append(greatherThanFill);

                differentialFormats.Append(lessThanFormat);
                differentialFormats.Append(greaterThanFormat);
                workbook.WorkbookPart.WorkbookStylesPart.Stylesheet.Append(differentialFormats);

                ConditionalFormatting conditionalFormatting = new ConditionalFormatting() { 
                    SequenceOfReferences = new ListValue<StringValue>() { InnerText = "D2:D10" }
                };

                ConditionalFormattingRule greaterThanRule = new ConditionalFormattingRule()
                {
                    Type = ConditionalFormatValues.CellIs,
                    FormatId = (UInt32Value)1U,
                    Priority = 2,
                    Operator = ConditionalFormattingOperatorValues.GreaterThan
                };
                Formula greaterThanFormula = new Formula();
                greaterThanFormula.Text = "0";
                greaterThanRule.Append(greaterThanFormula);

                ConditionalFormattingRule lessThanRule = new ConditionalFormattingRule()
                {
                    Type = ConditionalFormatValues.CellIs,
                    FormatId = (UInt32Value)0U,
                    Priority = 1,
                    Operator = ConditionalFormattingOperatorValues.LessThan
                };
                Formula lessThanFormula = new Formula();
                lessThanFormula.Text = "0";
                lessThanRule.Append(lessThanFormula);

                conditionalFormatting.Append(greaterThanRule);
                conditionalFormatting.Append(lessThanRule);

                worksheetPart.Worksheet.Append(conditionalFormatting);

                report.WorkbookPart.Workbook.Save();
                report.Close();
            }
        }
    }
}