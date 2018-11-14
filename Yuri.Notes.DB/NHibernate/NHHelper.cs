using System;
using System.Web;
using NHibernate;
using NHibernate.Cfg;

namespace Yuri.Notes.DB
{
    public sealed class NHibernateHelper
    {
        private const string CurrentSessionKey = "nhibernate.current_session";
        private static readonly ISessionFactory sessionFactory;

        static NHibernateHelper()
        {
  
                sessionFactory = new Configuration().Configure().BuildSessionFactory();
  
        }

        public static ISession GetCurrentSession()
        {
            HttpContext context = HttpContext.Current;

            if (!(context.Items[CurrentSessionKey] is ISession currentSession))
            {
                currentSession = sessionFactory.OpenSession();
                context.Items[CurrentSessionKey] = currentSession;
            }

            return currentSession;
        }

        public static void CloseSession()
        {
            HttpContext context = HttpContext.Current;

            if (!(context.Items[CurrentSessionKey] is ISession currentSession))
            {
                // No current session
                return;
            }

            currentSession.Close();
            context.Items.Remove(CurrentSessionKey);
        }

        public static void CloseSessionFactory()
        {
            if (sessionFactory != null)
            {
                sessionFactory.Close();
            }
        }
    }
}