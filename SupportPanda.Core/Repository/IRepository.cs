using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public interface IRepository
    {
        T Save<T>(T t) where T : PandaObject;
        void Delete<T>(T t) where T : PandaObject;

        Task<T> SaveAsync<T>(T t) where T : PandaObject;
        Task DeleteAsync<T>(T t) where T : PandaObject;

        T Get<T>(int id) where T : PandaObject;
        List<T> Get<T>() where T : PandaObject;
 


        T Get<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T              : PandaObject;
        List<T> GetItems<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T   : PandaObject;
        List<T> GetItems<T>();

        bool Any<T>(System.Linq.Expressions.Expression<Func<T, bool>> expression) where T : PandaObject;

        ISessionFactory GetSessionFactory();

        DataTable GetData(string query, Dictionary<string, object> parameters);
        int ExecuteUpdate(string query, Dictionary<string, object> parameters);
        object Get(string query, Dictionary<string, object> parameters);
    }
}
