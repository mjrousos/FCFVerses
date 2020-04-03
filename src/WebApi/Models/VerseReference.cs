using Microsoft.OpenApi.Extensions;

namespace WebApi.Models
{
    public class VerseReference
    {
        public Books Book { get; }

        public byte Chapter { get; }

        public byte Verse { get; }

        public VerseReference(Books book, byte chapter, byte verse)
        {
            Book = book;
            Chapter = chapter;
            Verse = verse;
        }

        public override string ToString()
        {
            var bookName = Book.GetDisplayName()
                .Replace("First", "1 ")
                .Replace("Second", "2 ")
                .Replace("Third", "3 ")
                .Replace("SongOfSolomon", "Song of Solomon");

            return $"{bookName} {Chapter}:{Verse}";
        }
    }
}
