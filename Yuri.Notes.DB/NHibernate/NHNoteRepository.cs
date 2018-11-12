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
            var session = NHibernateHelper.GetCurrentSession();

            if (entity.Id > 0)
            {
                session.CreateSQLQuery("UPDATE [Notes] SET [Name] = :Name, [Text] = :Text, [Draft] = :Draft, " +
                    "[Tags] = :Tags, [Date] = :Date, [Author] = :Author, [BinaryFile] = :BinaryFile WHERE [Id] = :Id")
                    .SetInt64("Id", entity.Id)
                    .SetString("Name", entity.Name)
                    .SetString("Text", entity.Text)
                    .SetBoolean("Draft", false)
                    .SetString("TagList", entity.Tags)
                    .SetDateTime("Date", DateTime.Now)
                    .SetInt64("Author", entity.Author.Id)
                    .SetString("File", entity.BinaryFile)
                    .ExecuteUpdate();
            }
            else
            {
                session.CreateSQLQuery("INSERT INTO [Notes] ([Name], [Text], [Draft], [Tags], [Date], [Author], [BinaryFile])" +
                    " VALUES (:Name, :Text, :Draft, :Tags, :Date, :Author, :BinaryFile)")
                    .SetString("Name", entity.Name)
                    .SetString("Text", entity.Text)
                    .SetBoolean("Draft", false)
                    .SetString("Tags", entity.Tags)
                    .SetDateTime("Date", DateTime.Now)
                    .SetInt64("Author", entity.Author.Id)
                    .SetString("BinaryFile", entity.BinaryFile)
                    .ExecuteUpdate();
            }
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
