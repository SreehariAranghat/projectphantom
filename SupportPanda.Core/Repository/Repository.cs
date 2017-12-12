using NHibernate;
using NHibernate.Cfg;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SupportPanda.Core
{
    public class Repository : IRepository
    {
        ISessionFactory sessionFactory;
        ILogger logger;

        Configuration cfg = new Configuration();

        public ISessionFactory SessionFactory
        {
            get
            {
                return sessionFactory;
            }
        }

        Repository()
        {
            
        }

        public Repository(ILogger logger)
        {
            if (sessionFactory == null)
            {

                cfg.Configure();

                sessionFactory = cfg.BuildSessionFactory();
            }

            this.logger = logger;
            logger.Info("Initiazed SupportPanda Repository");
        }

    
        public void Delete<T>(T t) where T : PandaObject
        {
            if (t != null)
            {
                t.DeletedDate = DateTime.UtcNow;
                Save<T>(t);
            }
            else
            {
                throw new Exception("Invalid Object");
            }
        }

        public Task DeleteAsync<T>(T t) where T : PandaObject
        {
            return Task.Run(() => { Delete<T>(t); });
        }


        public T Save<T>(T t) where T : PandaObject
        {

            if (t != null)
            {
                if (t.IsValid())
                {
                    int userid   = Util.CurrentIdentityId;

                    if (userid > 0)
                    {
                        User user = Util.CurrentIdentity;

                        using (ISession session = SessionFactory.OpenSession())
                        {
                            using (ITransaction trans = session.BeginTransaction())
                            {

                                if ((user.DeletedDate == null
                                     || user.DeletedUser == null
                                     || user.IsActive == false))
                                {
                                    if (t.Id > 0)
                                    {
                                        if (t.DeletedDate == null) // If its a forward request from delete
                                        {
                                            logger.Info(String.Format("Attempting to update type {0} Id {1}", typeof(T), t.Id));
                                            t.ModifiedUser = user;
                                            t.ModifiedDate = DateTime.UtcNow;

                                        }
                                        else
                                        {
                                            logger.Info(String.Format("Attempting to Deleting type {0} Id {1}", typeof(T), t.Id));
                                            t.DeletedUser = user;

                                        }

                                        session.SaveOrUpdate(t);
                                        logger.Info(String.Format("Update complete for type {0} Id {1}", typeof(T), t.Id));
                                    }
                                    else
                                    {
                                        logger.Info(String.Format("Attempting to create type {0}", typeof(T)));
                                        t.CreatedDate = DateTime.UtcNow;
                                        t.CreatedUser = user;

                                        session.Save(t);

                                        logger.Info(String.Format("Create complete for type {0} assigned Id {1}", typeof(T), t.Id));
                                    }


                                }
                                else
                                {
                                    logger.Fatal(String.Format("Invalid User : User Id Provided was {0} ", userid)
                                                                , new UnauthorizedAccessException(
                                                                     String.Format("Unauthorized access by userid {0}", userid)));

                                    throw new Exception("User is not valid");
                                }

                                trans.Commit();
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Invalid user authentication");
                    }

                }
                else
                {
                    var valresult = t.Validate();
                    throw new ValidationException("Invalid Object : VALIDATION ERROR ", valresult);
                }
            }
            else
            {
                throw new Exception("Null object, Invalid object state");
            }

            return t;
        }
        public Task<T> SaveAsync<T>(T t) where T : PandaObject
        {
            return Task<T>.Run(() => { return Save<T>(t); });
        }


        public T Get<T>(int id) where T : PandaObject
        {
            T t = null;

            if (id > 0)
            {
                using (ISession session = SessionFactory.OpenSession())
                {
                    t = session.Get<T>(id);
                }
            }

            return t;
        }


        public List<T> Get<T>() where T : PandaObject
        {
            List<T> t = null;

            using (ISession session = SessionFactory.OpenSession())
            {
                t = (List<T>)session.Query<T>()
                            .Where(i => i.DeletedDate == null && i.DeletedUser == null).ToList();
            }

            return t;
        }


        public List<T> GetItems<T>(Expression<Func<T, bool>> expression) where T : PandaObject
        {
            List<T> t = null;

            using (ISession session = SessionFactory.OpenSession())
            {
                expression = expression.And<T>(r => r.DeletedDate == null && r.DeletedUser == null);
                t = session.Query<T>().Where(expression).ToList();
            }

            return t;
        }


        public List<T> GetList<T>(Expression<Func<T, bool>> expression)
        {
            List<T> t = null;

            using (ISession session = SessionFactory.OpenSession())
            {
                if (expression != null)
                    t = session.Query<T>().Where(expression).ToList();
                else
                    t = session.Query<T>().ToList();
            }

            return t;
        }

        
        public T Get<T>(Expression<Func<T, bool>> expression) where T : PandaObject
        {
            T t = null;

            using (ISession session = SessionFactory.OpenSession())
            {
                expression = expression.And<T>(r => r.DeletedDate == null && r.DeletedUser == null);
                t = session.Query<T>().Where(expression).FirstOrDefault<T>();

            }

            return t;
        }

        public ISessionFactory GetSessionFactory()
        {
            return this.SessionFactory;
        }

        
        public dynamic GetItems(string query, Dictionary<string, object> parameters)
        {
            using (ISession session = GetSessionFactory().OpenSession())
            {
                var q = session.CreateSQLQuery(query);

                if (parameters != null)
                    foreach (var par in parameters)
                        q.SetParameter(par.Key, par.Value);

                var result = q.DynamicList();

                return result;
            }

        }


        public int ExecuteUpdate(string query, Dictionary<string, object> parameters)
        {
            int i = 0;

            using (ISession session = sessionFactory.OpenSession())
            {
                using (ITransaction trans = session.BeginTransaction())
                {
                    var q = session.CreateSQLQuery(query);

                    if (parameters != null)
                        foreach (var par in parameters)
                            q.SetParameter(par.Key, par.Value);

                    i = q.ExecuteUpdate();
                    trans.Commit();
                }
            }

            return i;
        }

      
        public DataTable GetData(string query, Dictionary<string, object> parameters)
        {
            string conString = cfg.GetProperty(NHibernate.Cfg.Environment.ConnectionString);
            DataTable table = new DataTable();

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);

                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> key in parameters)
                    {
                        cmd.Parameters.AddWithValue(key.Key, key.Value);
                    }
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(table);
            }

            return table;
        }

  
        public object Get(string query, Dictionary<string, object> parameters)
        {
            object retObj = null;
            string conString = cfg.GetProperty(NHibernate.Cfg.Environment.ConnectionString);

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);

                if (parameters != null)
                {
                    foreach (KeyValuePair<string, object> key in parameters)
                    {
                        cmd.Parameters.AddWithValue(key.Key, key.Value);
                    }
                }

                retObj = cmd.ExecuteScalar();
            }

            return retObj;
        }

 
        public bool Any<T>(Expression<Func<T, bool>> expression) where T : PandaObject
        {
            bool hasData = false;

            using (ISession session = SessionFactory.OpenSession())
            {
                expression = expression.And<T>(r => r.DeletedDate == null && r.DeletedUser == null);
                hasData    = session.Query<T>().Any(expression);

            }

            return hasData;
        }

        public List<T> GetItems<T>()
        {
            List<T> list = null;

            using (ISession session = SessionFactory.OpenSession())
            {
                list = session.Query<T>().ToList();

            }

            return list;
        }
    }
}
