namespace Yuri.Notes.DB
{
    public class NHUserRepository : NHBaseRepository<User>, IUserRepository
    {
 

        public void Block(long id)
        {
            var user = Load(id);

            if (user != null)
            {
               // user.Status = UserStatus.BLOCKED;
                Save(user);
            }
        }

        public User LoadByName(string name)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var entity = session.QueryOver<User>()
                .And(u => u.Login == name)
                .SingleOrDefault();

            NHibernateHelper.CloseSession();

            return entity;
        }

        public long FindIdByLogin(string login)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var user = session.QueryOver<User>()
                .And(u => u.Login == login)
                .SingleOrDefault();

            NHibernateHelper.CloseSession();

            return user.Id;
        }

        public User LoadByLogin(string login)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var user = session.QueryOver<User>()
                .And(u => u.Login == login)
                .SingleOrDefault();

            NHibernateHelper.CloseSession();

            return user;
        }

        public void UserRegistration(string login, string password)
        {
            var session = NHibernateHelper.GetCurrentSession();

            session.CreateSQLQuery("INSERT INTO [User] ([Login], [Password], [RoleId]) VALUES (:Login, :Password, :RoleId)")
                .SetString("Login", login)
                .SetString("Password", password)
                .SetInt32("RoleId", 2)
                .ExecuteUpdate();
       

            NHibernateHelper.CloseSession();
        }
    }
}
