using System;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Parsers.Concrete
{
    [TargetFile("Home_Fest--dashboard-default-Installs")]
    public class ViralityInstallsParser : TextParser
    {
        public ViralityInstallsParser(string filePath) : base(filePath) { }

        protected override void ParseLine(string line)
        {
            var values = line.Replace("\"", string.Empty).Split(',');
            if (values.Length == 0) return;

            DateTime date;
            if (DateTime.TryParse(values[0], out date))
            {
                var viral = repository.Get<Virality, DateTime>(date);
                if (viral == null)
                    repository.Save<Virality>(new Virality
                    {
                        Date = date,
                        TotalInstallations = int.Parse(values[3]) + int.Parse(values[4])
                    });
                else
                {
                    viral.TotalInstallations = int.Parse(values[3]) + int.Parse(values[4]);
                    repository.Update<Virality>(viral);
                }
            }
        }
    }
}