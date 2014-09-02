using FluentNHibernate.Mapping;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Mappings
{
    public class ItemEventsMap : ClassMap<ItemEvents>
    {
        public ItemEventsMap()
        {
            Id(x => x.RowId).Unique();
            Map(x => x.ItemId).Not.Nullable();
            Map(x => x.TotalEvents).Not.Nullable();
            Map(x => x.UniqueEvents).Not.Nullable();
            Map(x => x.EventValue).Not.Nullable();
            Map(x => x.AverageValue).Not.Nullable();
            Map(x => x.Visits).Not.Nullable();
            Map(x => x.PagesByVisit).Not.Nullable();
            Map(x => x.AverageTimeOnSite).Not.Nullable();
            Map(x => x.NewVisits).Not.Nullable();
            Map(x => x.StartDate).Not.Nullable();
            Map(x => x.EndDate).Not.Nullable();
        }
    }
}