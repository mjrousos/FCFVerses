using System.ComponentModel.DataAnnotations;
using WebApi.Models;

namespace WebApi.Data.Models
{
    public class Verse : IEntity
    {
        public Books Book { get; }

        public byte Chapter { get; }

        public byte VerseNumber { get; }

        public Translations Translation { get; }

        [Required]
        [MinLength(7)]
        public string Text { get; }

        public Verse(Books book, byte chapter, byte verse, Translations translation, string text)
        {
            Book = book;
            Chapter = chapter;
            VerseNumber = verse;
            Translation = translation;
            Text = text;
        }
    }
}
