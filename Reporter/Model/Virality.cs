using System;

namespace Shipoopi.Reporter.Model
{
    public class Virality
    {
        public virtual DateTime Date { get; set; }
        public virtual double Total { get; set; }
        public virtual double Invite { get; set; }
        public virtual double Notification { get; set; }
        public virtual double FeedStory { get; set; }
        public virtual double MultiFeedStory { get; set; }
        public virtual double Stream { get; set; }
        public virtual double PublishUserAction { get; set; }
        public virtual double Email { get; set; }
        public virtual int TotalInstallations { get; set; }
    }
}