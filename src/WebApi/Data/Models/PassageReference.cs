using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using WebApi.Models;
using WebApi.Utilities;

namespace WebApi.Data.Models
{
    public class PassageReference : IEntity, IEquatable<PassageReference>
    {
        public Books Book { get; set; }

        public byte Chapter { get; set; }

        public byte Verse { get; set; }

        public byte Length { get; set; }

        public byte StartOffset { get; set; }

        public byte EndOffset { get; set; }

        public PassageReference(Books book, byte chapter, byte verse, byte length, byte startOffset, byte endOffset)
        {
            Book = book;
            Chapter = chapter;
            Verse = verse;
            Length = length;
            StartOffset = startOffset;
            EndOffset = endOffset;
        }

        public PassageReference(PassageReference otherReference)
            : this(otherReference.Book, otherReference.Chapter, otherReference.Verse, otherReference.Length, otherReference.StartOffset, otherReference.EndOffset)
        { }

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

        public bool Equals([AllowNull] PassageReference other) =>
            !(other is null) &&
            Book == other.Book &&
            Chapter == other.Chapter &&
            Verse == other.Verse &&
            Length == other.Length &&
            StartOffset == other.StartOffset &&
            EndOffset == other.EndOffset;

        public override bool Equals(object? obj) => (obj is PassageReference reference) ? Equals(reference) : false;

        public override int GetHashCode() => $"{ToString()} {StartOffset} {EndOffset}".GetHashCode();

        public static bool operator ==(PassageReference? lhs, PassageReference? rhs) => lhs?.Equals(rhs) ?? rhs is null;

        public static bool operator !=(PassageReference? lhs, PassageReference? rhs) => !(lhs == rhs);
    }
}
