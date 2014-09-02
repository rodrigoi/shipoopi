using FluentNHibernate.Mapping;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Mappings
{
    public class ItemMap : ClassMap<Item>
    {
        public ItemMap() {
            Id(x => x.ItemId).Unique().Not.Nullable();
            Map(x => x.Name).Length(50).Not.Nullable().Default(string.Empty);
            Map(x => x.Frame).Not.Nullable().Default("0");
            Map(x => x.Price).Not.Nullable().Default("0");
            Map(x => x.CategoryName).Not.Nullable().Default(string.Empty);
            Map(x => x.Type).Not.Nullable().Default(string.Empty);
            Map(x => x.Special).Not.Nullable().Default(string.Empty);
            Map(x => x.Upgrade).Not.Nullable().Default(string.Empty);
            Map(x => x.Premium).Not.Nullable().Default(string.Empty);
        }
    }
}