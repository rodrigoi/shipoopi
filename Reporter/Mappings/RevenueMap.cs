using FluentNHibernate.Mapping;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Mappings
{
    public class RevenueMap : ClassMap<Revenue>
    {
        public RevenueMap()
        {
            Id(x => x.Date).Unique().Not.Nullable();
            Map(x => x.TransactionCount).Not.Nullable().Default("0");
            Map(x => x.TotalRevenue).Not.Nullable().Default("0");
        }
    }
}