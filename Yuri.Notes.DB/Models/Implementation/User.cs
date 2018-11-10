using System;

namespace Yuri.Notes.DB
{
    public class User : IEntity
    {
        public virtual long Id { get; set; }

        public virtual string Login { get; set; }

        public virtual string Password { get; set; }

    }

    public enum UserStatus
    {
        ACTIVE,
        BLOCKED,
        DELETED
    }
}
