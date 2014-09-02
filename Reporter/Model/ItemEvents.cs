using System;

namespace Shipoopi.Reporter.Model
{
    public class ItemEvents
    {
        public virtual int RowId { get; set; }
        public virtual int ItemId { get; set; }
        public virtual int TotalEvents { get; set; }
        public virtual int UniqueEvents { get; set; }
        public virtual int EventValue { get; set; }
        public virtual double AverageValue { get; set; }
        public virtual int Visits { get; set; }
        public virtual double PagesByVisit { get; set; }
        public virtual double AverageTimeOnSite { get; set; }
        public virtual double NewVisits { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
    }
}