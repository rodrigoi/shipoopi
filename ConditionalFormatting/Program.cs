using System.Linq;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CF
{
    class Program
    {
        static void Main(string[] args)
        {
            var workbookClosed = new XLWorkbook();

            var sheet = workbookClosed.Worksheets.Add("Sheet1");
            sheet.Cell(1, 1).Value = 1;
            sheet.Cell(2, 1).Value = -1;

            workbookClosed.SaveAs("closed.xlsx");

            using (SpreadsheetDocument report = SpreadsheetDocument.Open("closed.xlsx", true))
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

                ConditionalFormatting conditionalFormatting = new ConditionalFormatting()
                {
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

                worksheetPart.Worksheet.PrependChild<ConditionalFormatting>(conditionalFormatting);

                report.WorkbookPart.Workbook.Save();
                report.Close();
            }
        }
    }
}