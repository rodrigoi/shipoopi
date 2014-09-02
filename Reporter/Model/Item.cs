
namespace Shipoopi.Reporter.Model
{
    public class Item
    {
        public virtual int ItemId { get; set; }
        public virtual string Name { get; set; }
        public virtual int Frame { get; set; }
        public virtual int Price { get; set; }
        public virtual string CategoryName { get; set; }
        public virtual string Type { get; set; }
        public virtual string Special { get; set; }
        public virtual string Upgrade { get; set; }
        public virtual string Premium { get; set; }
    }
}