using System;

namespace Shipoopi.Reporter.Model
{
    public class Analytics
    {
        public virtual DateTime Date { get; set; }
        public virtual int TimeOnSite { get; set; }
        public virtual int Visits { get; set; }
        public virtual int Level1 { get; set; }
        public virtual int Level2 { get; set; }
        public virtual int Level3 { get; set; }
        public virtual int Level4 { get; set; }
        public virtual int Level5 { get; set; }
    }
}