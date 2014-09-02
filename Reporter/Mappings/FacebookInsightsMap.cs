using FluentNHibernate.Mapping;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Mappings
{
    public class FacebookInsightsMap : ClassMap<FacebookInsights>
    {
        public FacebookInsightsMap() {
            Id(x => x.Date).Not.Nullable().Unique();
            Map(x => x.DailyActiveUsers).Not.Nullable().Default("0");
            Map(x => x.MonthlyActiveUsers).Not.Nullable().Default("0");
            Map(x => x.DailyAppInstalls).Not.Nullable().Default("0");
        }
    }
}