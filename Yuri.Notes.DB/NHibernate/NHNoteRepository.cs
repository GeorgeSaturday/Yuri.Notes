using System;
using System.Collections.Generic;
using Yuri.Notes.DB;

namespace Yuri.Notes.DB
{
    public class NHNoteRepository : NHBaseRepository<Note>, INoteRepository
    {
        IUserRepository UserRepository;

        public NHNoteRepository()
        {
            UserRepository = new NHUserRepository();
        }

        /// <summary>
        /// Получить все заметки
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Note> GetPublicNotes(long userId)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var notes = session.QueryOver<Note>()
                .And(u => u.Draft == true)               
                .List();

            NHibernateHelper.CloseSession();
            return notes;
        }

        /// <summary>
        /// получить только мои заметки
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Note> GetMyNotes(long userId)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var notes = session.QueryOver<Note>()
                .And(u => u.Author.Id == userId)
                .List();

            NHibernateHelper.CloseSession();

            return notes;
        }
       

        /// <summary>
        /// Сохранить сессию
        /// </summary>
        /// <param name="entity"></param>
        public override void Save(Note entity)
        {
            var session = NHibernateHelper.GetCurrentSession();

            using (session.BeginTransaction())
            {
                var author = UserRepository.Load(entity.Author.Id);

                entity.Author = author;
                entity.Date = DateTime.Now;

                if (entity != null)
                {
                    session.SaveOrUpdate(entity);
                }

                session.Flush();
            }

            NHibernateHelper.CloseSession();
        }

        Note IEntityRepository<Note>.Create()
        {
            throw new NotImplementedException();
        }

        IEnumerable<Note> IEntityRepository<Note>.GetAll()
        {
            throw new NotImplementedException();
        }

        Note IEntityRepository<Note>.Load(long id)
        {
            throw new NotImplementedException();
        }


        public Note ReceiveNoteId(long Id)
        {
            var session = NHibernateHelper.GetCurrentSession();

            using (session.BeginTransaction())
            {
                using (session.BeginTransaction())
                {
                    var note = session.QueryOver<Note>()
                        .And(u => u.Id == Id)
                        .SingleOrDefault();

                    return note;
                }
            }
        }

        /// <summary>
        /// Удалить заметку
        /// </summary>
        /// <param name="id"></param>
        public override void Delete(long id)
        {
            var session = NHibernateHelper.GetCurrentSession();

            using (session.BeginTransaction())
            {
                var entity = ReceiveNoteId(id);

                if (entity != null)
                {
                    session.Delete(entity);
                }

                session.Flush();
            }
        }
    }
}
