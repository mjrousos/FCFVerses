using System.ComponentModel.DataAnnotations;
using WebApi.Models;

namespace WebApi.Data.Models
{
    public class Verse : IEntity
    {
        public Books Book { get; set; }

        public byte Chapter { get; set; }

        public byte VerseNumber { get; set; }

        public Translations Translation { get; set; }

        [Required]
        [MinLength(7)]
        [MaxLength(255)]
        public string Text { get; set; }

        public Verse(Books book, byte chapter, byte verseNumber, Translations translation, string text)
        {
            Book = book;
            Chapter = chapter;
            VerseNumber = verseNumber;
            Translation = translation;
            Text = text;
        }
    }
}
