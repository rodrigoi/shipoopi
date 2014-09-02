using System;

namespace Shipoopi.Reporter.Model
{
    public class FacebookInsights
    {
        public virtual DateTime Date { get; set; }
        public virtual int DailyActiveUsers { get; set; }
        public virtual int MonthlyActiveUsers { get; set; }
        public virtual int DailyAppInstalls { get; set; }
    }
}