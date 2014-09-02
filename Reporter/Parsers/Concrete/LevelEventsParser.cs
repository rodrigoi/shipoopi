using System;
using System.Globalization;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Parsers.Concrete
{
    [TargetFile("EventObjectActionLabelDetailReport")]
    public class LevelEventsParser : TextParser
    {
        private int level;
        public LevelEventsParser(string filePath) : base(filePath) { }

        protected override void ParseLine(string line)
        {
            var values = line.Split('\t');
            if (values.Length == 2)
            {
                if (values[1].ToLower().Contains("level"))
                {
                    int.TryParse(values[1].Substring(values[1].Length - 1), out this.level);
                }

                values[1] = values[1].Replace(".", string.Empty);

                DateTime date;
                int eventCount;
                if (DateTime.TryParse(values[0], new CultureInfo("es-AR"), DateTimeStyles.None, out date) && 
                    int.TryParse(values[1], out eventCount))
                {
                    string levelName = string.Concat(new string[] { "Level", level.ToString() });
                    var levelProperty = typeof(Analytics).GetProperty(levelName);

                    var analytics = repository.Get<Analytics, DateTime>(date);
                    if (analytics == null)
                    {
                        analytics = new Analytics { Date = date };
                        levelProperty.SetValue(analytics, eventCount, null);
                        repository.Save<Analytics>(analytics);
                    }
                    else
                    {
                        levelProperty.SetValue(analytics, eventCount, null);
                        repository.Update<Analytics>(analytics);
                    }
                }
            }
        }
    }
}