using NHibernate;
using Ninject.Modules;
using Shipoopi.Reporter.Persistance;

namespace Shipoopi.Reporter.IoC
{
    public class ReporterModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ISessionFactory>().ToSelf().InSingletonScope();
            Bind<IRepository>().To<SQLiteRepository>().InSingletonScope();
        }
    }
}