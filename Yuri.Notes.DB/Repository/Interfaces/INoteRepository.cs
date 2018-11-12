using System.Collections.Generic;

namespace Yuri.Notes.DB
{
    public interface INoteRepository : IEntityRepository<Note>
    {
        
         IList<Note> GetPublicNotes(long userId);

         IList<Note> GetMyNotes(long userId);

    }
}
