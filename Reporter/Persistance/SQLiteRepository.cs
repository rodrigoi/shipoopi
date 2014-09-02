using System;
using System.Collections.Generic;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Criterion;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Persistance
{
    public class SQLiteRepository : IRepository
    {
        private ISessionFactory sessionFactory;

        private const string DataFile = "reports.sqlitedb";

        public SQLiteRepository()
        {
            this.sessionFactory = CreateSessionFactory();
        }

        public IList<T> Get<T>(string sortProperty, bool ascending) where T : class
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.CreateCriteria<T>()
                    .AddOrder(new Order(sortProperty, ascending))
                    .List<T>();
            }
        }
        public Ta Get<Ta, Tb>(Tb id)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.Get<Ta>(id);
            }
        }
        public ItemEvents GetItemEvent(int itemId, DateTime startDate)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.CreateCriteria<ItemEvents>()
                    .Add(Expression.Where<ItemEvents>(x => x.ItemId == itemId && x.StartDate == startDate))
                    .AddOrder(new Order("EndDate", false))
                    .SetMaxResults(1)
                    .UniqueResult<ItemEvents>();
            }
        }
        public ItemEvents GetItemEvent(int itemId, DateTime startDate, DateTime endDate)
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.CreateCriteria<ItemEvents>()
                    .Add(Expression.Where<ItemEvents>(x => x.ItemId == itemId && x.StartDate == startDate && x.EndDate == endDate))
                    .UniqueResult<ItemEvents>();
            }
        }
        public IList<DateTime> GetItemEventDates()
        {
            using (var session = sessionFactory.OpenSession())
            {
                return session.CreateCriteria<ItemEvents>()
                    .SetProjection(Projections.Distinct(Projections.ProjectionList().Add(Projections.Property("StartDate"))))
                    .AddOrder(new Order("StartDate", true))
                    .List<DateTime>();
            }
        }

        public void Save<T>(T item)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Save(item);
                    transaction.Commit();
                }
                session.Close();
            }
        }
        public void Update<T>(T item)
        {
            using (var session = sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Update(item);
                    transaction.Commit();
                }
                session.Close();
            }
        }

        private static ISessionFactory CreateSessionFactory()
        {
            return Fluently.Configure()
                .Database(SQLiteConfiguration.Standard
                    .UsingFile(DataFile))
                .Mappings(m =>
                    m.FluentMappings.AddFromAssemblyOf<Program>())
                .ExposeConfiguration(config => {
                    //new SchemaExport(config).Create(false, true);
                })
                .BuildSessionFactory();
        }
    }
}