using System;

namespace Shipoopi.Reporter.Parsers
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class TargetFile : Attribute
    {
        public string NamePattern { get; set; }

        public TargetFile(string namePattern)
        {
            this.NamePattern = namePattern;
        }
    }
}