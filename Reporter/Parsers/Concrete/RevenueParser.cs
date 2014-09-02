using System;
using Shipoopi.Reporter.Model;
using System.Globalization;

namespace Shipoopi.Reporter.Parsers.Concrete
{
    [TargetFile("net_revenue[a-zA-Z0-9_-]{26}offer_all_offers")]
    public class RevenueParser : TextParser
    {
        public RevenueParser(string filePath) : base(filePath) { }

        protected override void ParseLine(string line)
        {
            var values = line.Split(',');
            if (values.Length != 2) return;

            DateTime date;
            float totalRevenue;
            if (DateTime.TryParse(values[0], out date) && 
                float.TryParse(values[1].Replace("$", string.Empty), 
                    NumberStyles.Any, 
                    CultureInfo.InvariantCulture, 
                out totalRevenue))
            {
                var revenue = repository.Get<Revenue, DateTime>(date);
                if (revenue == null)
                    repository.Save<Revenue>(new Revenue
                    {
                        Date = date,
                        TotalRevenue = totalRevenue
                    });
                else
                {
                    revenue.TotalRevenue = totalRevenue;
                    repository.Update<Revenue>(revenue);
                }
            }
        }
    }
}