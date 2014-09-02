using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Shipoopi.Reporter.Factories;
using Shipoopi.Reporter.Reporters;

namespace Shipoopi.Reporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

            if (args.Length > 0)
            {
                if (Directory.Exists(args[0]))
                {
                    var files = Directory.GetFiles(args[0]);
                    var currentFile = 1;

                    Console.WriteLine("{0} files found.", files.Length);

                    foreach (var filePath in files)
                    {
                        Console.WriteLine(string.Format("Processing file {0} of {1}: {2}", currentFile, files.Length, Path.GetFileName(filePath)));

                        var parser = ParserFactory.GetFileParser(filePath);
                        if (parser != null) 
                            parser.Parse();
                        else
                            Console.WriteLine("No parser found for that file :(");

                        Console.WriteLine();
                        Console.WriteLine();
                        currentFile++;
                    }

                    Console.WriteLine("Creating Report {0}", "report.xlsx");

                    var reporter = new ExcelReporter(Path.Combine(args[0], "report.xlsx"));
                    reporter.GenerateReport();
                }
                else
                {
                    Console.WriteLine("Directory Not Found");
                }
            }
            else
            {
                Console.WriteLine("missing parameter: source data path.");
            }

            Console.WriteLine("So long and thanks for all the fish. Press any key to exit.");
            watch.Stop();
            Console.WriteLine("Time is an illusion, but it took {0} to do all the work.", watch.Elapsed.ToString());
            Console.ReadKey();
        }
    }
}