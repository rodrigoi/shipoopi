using System;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Parsers.Concrete
{
    [TargetFile("Home_Fest--virality-overview-Growth_Sources")]
    public class ViralityParser : TextParser
    {
        public ViralityParser(string filePath) : base(filePath) { }

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
                        Total = double.Parse(values[1]),
                        Invite = double.Parse(values[2]),
                        Notification = double.Parse(values[3]),
                        FeedStory = double.Parse(values[4]),
                        MultiFeedStory = double.Parse(values[5]),
                        Stream = double.Parse(values[6]),
                        PublishUserAction = double.Parse(values[7]),
                        Email = double.Parse(values[8])
                    });
                else
                {
                    viral.Total = double.Parse(values[1]);
                    viral.Invite = double.Parse(values[2]);
                    viral.Notification = double.Parse(values[3]);
                    viral.FeedStory = double.Parse(values[4]);
                    viral.MultiFeedStory = double.Parse(values[5]);
                    viral.Stream = double.Parse(values[6]);
                    viral.PublishUserAction = double.Parse(values[7]);
                    viral.Email = double.Parse(values[8]);
                    repository.Update<Virality>(viral);
                }
            }
        }
    }
}