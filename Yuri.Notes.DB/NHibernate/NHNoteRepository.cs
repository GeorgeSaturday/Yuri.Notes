using System;
using System.Collections.Generic;
using Yuri.Notes.DB;

namespace Yuri.Notes.DB
{
    public class NHNoteRepository : NHBaseRepository<User>, INoteRepository
    {


        public IList<Note> GetPublicNotes(long userId)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var notes = session.QueryOver<Note>()
                .And(u => u.Author.Id != userId)
                .And(u => u.Draft == true)
                
                .List();

            NHibernateHelper.CloseSession();

            return notes;
        }

        public IList<Note> GetMyNotes(long userId)
        {
            var session = NHibernateHelper.GetCurrentSession();

            var notes = session.QueryOver<Note>()
                .And(u => u.Author.Id == userId)
                .List();

            NHibernateHelper.CloseSession();

            return notes;
        }

        public void Save(Note entity)
        {
            throw new NotImplementedException();
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

      

       
    }
}
