using ClosedXML.Excel;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Parsers.Concrete
{
    [TargetFile(@"^reporte_items_\d{8}.xlsx")]
    public class ItemsCategoryParser : ExcelParser
    {
        public ItemsCategoryParser(string pathName) : base(pathName, "analytics_discostu.atommica.com") { }

        protected override void ParseLine(IXLRow row)
        {
            if (row.Cell(1).DataType == XLCellValues.Number)
            {
                int itemId = row.Cell(1).GetValue<int>();
                var item = repository.Get<Item, int>(itemId);
                if (item == null)
                    repository.Save<Item>(new Item
                    {
                        ItemId = itemId,
                        Name = row.Cell(2).GetValue<string>(),
                        Frame = 0,
                        Price = 0,
                        Type = row.Cell(4).GetValue<string>(),
                        CategoryName = row.Cell(3).GetValue<string>(),
                        Special = row.Cell(5).GetValue<string>(),
                        Upgrade = row.Cell(6).GetValue<string>(),
                        Premium = row.Cell(7).GetValue<string>()
                    });
                else
                {
                    item.Name = row.Cell(2).GetValue<string>();
                    item.Type = row.Cell(4).GetValue<string>();
                    item.CategoryName = row.Cell(3).GetValue<string>();
                    item.Special = row.Cell(5).GetValue<string>();
                    item.Upgrade = row.Cell(6).GetValue<string>();
                    item.Premium = row.Cell(7).GetValue<string>();
                    repository.Update<Item>(item);
                }
            }
        }
    }
}