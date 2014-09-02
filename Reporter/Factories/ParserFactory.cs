using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Shipoopi.Reporter.Parsers;

namespace Shipoopi.Reporter.Factories
{
    public sealed class ParserFactory
    {
        private ParserFactory() { }

        public static IParser GetFileParser(string filePath)
        {
            var parserType = System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(TargetFile), true).Length > 0)
                .Where(t => new Regex(t.GetCustomAttributesData()
                    .FirstOrDefault()
                    .ConstructorArguments.First().Value.ToString())
                    .IsMatch(Path.GetFileName(filePath)))
                .SingleOrDefault();

            if (parserType != null)
            {
                return System.Activator.CreateInstance(parserType, new object[] { filePath }) as IParser;
            }
            return null;
        }
    }
}