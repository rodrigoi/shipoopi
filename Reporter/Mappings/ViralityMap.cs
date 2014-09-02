using FluentNHibernate.Mapping;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Mappings
{
    public class ViralityMap : ClassMap<Virality>
    {
        public ViralityMap()
        {
            Id(x => x.Date).Not.Nullable().Unique();
            Map(x => x.Total).Not.Nullable().Default("0");
            Map(x => x.Invite).Not.Nullable().Default("0");
            Map(x => x.Notification).Not.Nullable().Default("0");
            Map(x => x.FeedStory).Not.Nullable().Default("0");
            Map(x => x.MultiFeedStory).Not.Nullable().Default("0");
            Map(x => x.Stream).Not.Nullable().Default("0");
            Map(x => x.PublishUserAction).Not.Nullable().Default("0");
            Map(x => x.Email).Not.Nullable().Default("0");
            Map(x => x.TotalInstallations).Not.Nullable().Default("0");
        }
    }
}