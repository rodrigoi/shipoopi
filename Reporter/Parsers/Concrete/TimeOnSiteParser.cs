using System;
using Shipoopi.Reporter.Model;
using System.Globalization;

namespace Shipoopi.Reporter.Parsers.Concrete
{
    [TargetFile("TimeOnSiteReport")]
    public class TimeOnSiteParser : TextParser
    {
        public TimeOnSiteParser(string filePath) : base(filePath) { }
        protected override void ParseLine(string line)
        {
            var values = line.Replace("\"", string.Empty).Split('\t');
            if (values.Length != 2) return;

            values[1] = values[1].Replace(".", string.Empty);

            DateTime date;
            TimeSpan timeOnSite;
            if (DateTime.TryParse(values[0], new CultureInfo("es-AR"), DateTimeStyles.None, out date) && 
                TimeSpan.TryParse(values[1], out timeOnSite))
            {
                var analytics = repository.Get<Analytics, DateTime>(date);
                if (analytics == null)
                    repository.Save<Analytics>(new Analytics
                    {
                        Date = date,
                        TimeOnSite = (int)timeOnSite.TotalSeconds
                    });
                else
                {
                    analytics.TimeOnSite = (int)timeOnSite.TotalSeconds;
                    repository.Update<Analytics>(analytics);
                }
            }
        }
    }
}