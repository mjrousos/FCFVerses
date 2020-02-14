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

        public override string ToString() => $"{Book.GetDisplayName()} {Chapter}:{Verse}";
    }
}
