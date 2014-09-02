using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Parsers.Concrete
{
    [TargetFile("^item.*")]
    public class ItemsParser : TextParser
    {
        public ItemsParser(string fileName) : base(fileName) { }

        protected override void ParseLine(string line)
        {
            var values = line.Split(',');
            if (values.Length > 0)
            {
                int itemId = 0;
                if (int.TryParse(values[0], out itemId))
                {
                    var item = repository.Get<Item, int>(itemId);
                    if (item == null)
                        repository.Save<Item>(new Item
                        {
                            ItemId = itemId,
                            Name = values[1],
                            Frame = int.Parse(values[2]),
                            Price = int.Parse(values[3]),
                            CategoryName = string.Empty,
                            Type = string.Empty,
                            Special = string.Empty,
                            Upgrade = string.Empty,
                            Premium = string.Empty
                        });
                    else
                    {
                        item.Name = values[1];
                        item.Frame = int.Parse(values[2]);
                        item.Price = int.Parse(values[3]);
                        repository.Update<Item>(item);
                    }
                }
            }
        }
    }
}