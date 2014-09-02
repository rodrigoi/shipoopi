using System;

namespace Shipoopi.Reporter.Model
{
    public class Revenue
    {
        public virtual DateTime Date { get; set; }
        public virtual int TransactionCount { get; set; }
        public virtual double TotalRevenue { get; set; }
    }
}