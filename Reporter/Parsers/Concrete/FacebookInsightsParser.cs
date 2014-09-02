using System;
using ClosedXML.Excel;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Parsers.Concrete
{
    [TargetFile(@"^Facebook Insights Data Export - Home Fest - [a-zA-Z0-9_-]{10}.xlsx")]
    public class FacebookInsightsParser : ExcelParser
    {
        public FacebookInsightsParser(string fileName) : base(fileName, "key metrics") { }

        protected override void ParseLine(IXLRow row)
        {
            DateTime date;
            if (row.Cell(1).DataType == XLCellValues.Number)
            {
                date = DateTime.FromOADate(row.Cell(1).GetDouble());
                var facebookInsigh = repository.Get<FacebookInsights, DateTime>(date);
                if (facebookInsigh == null)
                    repository.Save<FacebookInsights>(new FacebookInsights
                    {
                        Date = date,
                        DailyActiveUsers = row.Cell(2).GetValue<int>(),
                        MonthlyActiveUsers = row.Cell(4).GetValue<int>(),
                        DailyAppInstalls = row.Cell(5).GetValue<int>()
                    });
                else
                {
                    facebookInsigh.DailyActiveUsers = row.Cell(2).GetValue<int>();
                    facebookInsigh.MonthlyActiveUsers = row.Cell(4).GetValue<int>();
                    facebookInsigh.DailyAppInstalls = row.Cell(5).GetValue<int>();
                }
            }
        }
    }
}