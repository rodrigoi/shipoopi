using System;
using Shipoopi.Reporter.Model;
using System.Globalization;

namespace Shipoopi.Reporter.Parsers.Concrete
{
    [TargetFile("VisitsReport")]
    public class VisitsParser : TextParser
    {
        public VisitsParser(string filePath) : base(filePath) { }

        protected override void ParseLine(string line)
        {
            var values = line.Split('\t');
            if (values.Length != 2) return;

            values[1] = values[1].Replace(".", string.Empty);

            DateTime date;
            int visits;
            if (DateTime.TryParse(values[0], new CultureInfo("es-AR"), DateTimeStyles.None, out date) && 
                int.TryParse(values[1].Replace(",", string.Empty), out visits))
            {
                var analytics = repository.Get<Analytics, DateTime>(date);
                if (analytics == null)
                    repository.Save<Analytics>(new Analytics
                    {
                        Date = date,
                        Visits = visits
                    });
                else
                {
                    analytics.Visits = visits;
                    repository.Update<Analytics>(analytics);
                }
            }
        }
    }
}