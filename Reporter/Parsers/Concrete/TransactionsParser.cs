using System;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Parsers.Concrete
{
    [TargetFile("transaction_count[a-zA-Z0-9_-]{26}offer_all_offers")]
    public class TransactionsParser : TextParser
    {
        public TransactionsParser(string filePath)
            : base(filePath)
        { }

        protected override void ParseLine(string line)
        {
            var values = line.Split(',');
            if (values.Length != 2) return;

            DateTime date;
            int transactionCount;
            if (DateTime.TryParse(values[0], out date) && int.TryParse(values[1], out transactionCount))
            {
                var revenue = repository.Get<Revenue, DateTime>(date);
                if (revenue == null)
                    repository.Save<Revenue>(new Revenue
                    {
                        Date = date,
                        TransactionCount = transactionCount
                    });
                else
                {
                    revenue.TransactionCount = transactionCount;
                    repository.Update<Revenue>(revenue);
                }
            }
        }
    }
}