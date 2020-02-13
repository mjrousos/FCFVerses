#pragma warning disable SA1602 // Enumeration items should be documented
namespace WebApi.Models
{
    public static class BookEnumExtensions
    {
        public static string ToBookName(Books book) =>
            book switch
            {
                Books.FirstSamuel => "1 Samuel",
                Books.SecondSamuel => "2 Samuel",
                Books.FirstKings => "1 Kings",
                Books.SecondKings => "2 Kings",
                Books.FirstChronicles => "1 Chronicles",
                Books.SecondChronicles => "2 Chronicles",
                Books.SongOfSolomon => "Song of Solomon",
                Books.FirstCorinthians => "1 Corinthians",
                Books.SecondCorinthians => "2 Corinthians",
                Books.FirstThessalonians => "1 Thessalonians",
                Books.SecondThessalonians => "2 Thessalonians",
                Books.FirstTimothy => "1 Timothy",
                Books.SecondTimothy => "2 Timothy",
                Books.FirstPeter => "1 Peter",
                Books.SecondPeter => "2 Peter",
                Books.FirstJohn => "1 John",
                Books.SecondJohn => "2 John",
                Books.ThirdJohn => "3 John",
                _ => book.ToString()
            };
    }
}
#pragma warning restore SA1602 // Enumeration items should be documented
