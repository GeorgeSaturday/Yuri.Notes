using System;
using System.ComponentModel.DataAnnotations;

namespace Yuri.Notes.DB
{
    public class Note : IEntity
    {
        public virtual long Id { get; set; }

        [Required(ErrorMessage ="Введите название заметки")]
        public virtual string Name { get; set; }

        public virtual string Text { get; set; }

        public virtual bool Draft { get; set; }

        public virtual string Tags { get; set; }

        public virtual DateTime Date { get; set; }

        public virtual User Author { get; set; }

        public virtual string BinaryFile { get; set; }

    }
}
