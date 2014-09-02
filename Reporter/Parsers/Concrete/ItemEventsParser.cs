using System;
using System.Globalization;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Parsers.Concrete
{
    [TargetFile("EventObjectActionDetailReport")]
    public class ItemEventsParser : TextParser
    {
        private DateTime startDate;
        private DateTime endDate;
        private bool datesDetected = false;

        public ItemEventsParser(string filePath) : base(filePath) { }
        protected override void ParseLine(string line)
        {
            var values = line.Split('\t');
            if (values.Length == 2 && !datesDetected)
            {
                DateTime startDate;
                DateTime endDate;
                if (DateTime.TryParse(values[0], new CultureInfo("es-AR"), DateTimeStyles.None, out startDate) &&
                    DateTime.TryParse(values[1], new CultureInfo("es-AR"), DateTimeStyles.None, out endDate))
                {
                    this.startDate = startDate;
                    this.endDate = endDate;
                    this.datesDetected = true;
                }
            }

            if (values.Length > 2 && values[0].ToLower().StartsWith("item-"))
            {
                var itemId = int.Parse(values[0].ToLower().Replace("item-", string.Empty));
                var itemEvent = repository.GetItemEvent(itemId, startDate, endDate);
                if (itemEvent == null)
                    repository.Save<ItemEvents>(new ItemEvents
                    {
                        ItemId = itemId,
                        TotalEvents = int.Parse(values[1]),
                        UniqueEvents = int.Parse(values[2]),
                        EventValue = int.Parse(values[3]),
                        AverageValue = float.Parse(values[4]),
                        PagesByVisit = float.Parse(values[5]),
                        AverageTimeOnSite = float.Parse(values[6]),
                        NewVisits = float.Parse(values[7]),
                        StartDate = startDate,
                        EndDate = endDate
                    });
                else
                {
                    itemEvent.TotalEvents = int.Parse(values[1]);
                    itemEvent.UniqueEvents = int.Parse(values[2]);
                    itemEvent.EventValue = int.Parse(values[3]);
                    itemEvent.AverageValue = float.Parse(values[4]);
                    itemEvent.PagesByVisit = float.Parse(values[5]);
                    itemEvent.AverageTimeOnSite = float.Parse(values[6]);
                    itemEvent.NewVisits = float.Parse(values[7]);
                }
            }

        }
    }
}