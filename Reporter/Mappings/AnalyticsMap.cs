using FluentNHibernate.Mapping;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Mappings
{
    public class AnalyticsMap : ClassMap<Analytics>
    {
        public AnalyticsMap()
        {
            Id(x => x.Date).Unique().Not.Nullable();
            Map(x => x.TimeOnSite).Not.Nullable().Default("0");
            Map(x => x.Visits).Not.Nullable().Default("0");
            Map(x => x.Level1).Not.Nullable().Default("0");
            Map(x => x.Level2).Not.Nullable().Default("0");
            Map(x => x.Level3).Not.Nullable().Default("0");
            Map(x => x.Level4).Not.Nullable().Default("0");
            Map(x => x.Level5).Not.Nullable().Default("0");
        }
    }
}