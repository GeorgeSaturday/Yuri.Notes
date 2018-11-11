namespace Yuri.Notes.DB
{
    public interface IUserRepository : IEntityRepository<User>
    {
        /// <summary>
        /// Найти пользователя по имени
        /// </summary>
        /// <param name="name">Имя или login</param>
        /// <returns></returns>
        User LoadByName(string name);

        /// <summary>
        /// Заблокировать пользователя
        /// </summary>
        /// <param name="id">Идентификатор</param>
        void Block(long id);

        User LoadByLogin(string login);

        void UserRegistration(string login, string password);

        long FindIdByLogin(string login);
    }
}
