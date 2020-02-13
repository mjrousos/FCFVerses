using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebApi.Models;
using WebApi.Utilities;

namespace WebApi.Data.Models
{
    public class PassageReference : IEntity
    {
        public Books Book { get; set; }

        public byte Chapter { get; }

        public byte Verse { get; }

        public byte Length { get; }

        public byte StartOffset { get; }

        public byte EndOffset { get; }

        public PassageReference(Books book, byte chapter, byte verse, byte length, byte startOffset, byte endOffset)
        {
            Book = book;
            Chapter = chapter;
            Verse = verse;
            Length = length;
            StartOffset = startOffset;
            EndOffset = endOffset;
        }

        public IEnumerable<VerseReference> Verses
        {
            get
            {
                if (Length != 0 && Chapter != 0 && Verse != 0)
                {
                    var currentBook = Book;
                    var currentChapter = Chapter;
                    var currentVerse = Verse;
                    for (var i = 0; i < Length; i++)
                    {
                        yield return new VerseReference(currentBook, currentChapter, currentVerse);

                        currentVerse++;

                        if (currentVerse > VerseCounts.Verses[(int)currentBook][currentChapter - 1])
                        {
                            currentVerse = 1;
                            currentChapter++;
                        }

                        if (currentChapter > VerseCounts.Verses[(int)currentBook].Length)
                        {
                            if (currentBook == Books.Revelation)
                            {
                                break;
                            }

                            currentChapter = 1;
                            currentBook = (Books)((int)currentBook + 1);
                        }
                    }
                }
            }
        }

        public override string? ToString()
        {
            if (Length == 0 || Chapter == 0 || Verse == 0)
            {
                return null;
            }

            if (Verses.Count() == 1)
            {
                return Verses.First().ToString();
            }

            var firstVerse = Verses.First();
            var lastVerse = Verses.Last();
            var ret = new StringBuilder();

            ret.Append(firstVerse.ToString());

            if (StartOffset > 0)
            {
                ret.Append("b");
            }

            ret.Append("-");

            if (firstVerse.Book == lastVerse.Book)
            {
                if (firstVerse.Chapter == lastVerse.Chapter)
                {
                    ret.Append(lastVerse.Verse);
                }
                else
                {
                    ret.Append($"{lastVerse.Chapter}:{lastVerse.Verse}");
                }
            }
            else
            {
                ret.Append(lastVerse.ToString());
            }

            if (EndOffset > 0)
            {
                ret.Append("a");
            }

            return ret.ToString();
        }
    }
}
