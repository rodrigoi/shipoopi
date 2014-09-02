using System;
using System.Collections.Generic;
using Shipoopi.Reporter.Model;

namespace Shipoopi.Reporter.Persistance
{
    public interface IRepository
    {
        IList<T> Get<T>(string sortProperty, bool ascending) where T : class;
        Ta Get<Ta, Tb>(Tb id);
        void Save<T>(T item);
        void Update<T>(T item);

        ItemEvents GetItemEvent(int itemId, DateTime startDate);
        ItemEvents GetItemEvent(int itemId, DateTime startDate, DateTime endDate);
        IList<DateTime> GetItemEventDates();
    }
}