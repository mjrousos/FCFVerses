namespace WebApi.Models
{
    public static class TranslationExtensions
    {
        /// <summary>
        /// Gets copyright notices for different Bible translations.
        /// </summary>
        /// <param name="translation">The translation to get a copyright notice for.</param>
        /// <returns>The copyright notice, or null if none exists.</returns>
        public static string? GetCopyrightNotice(this Translations translation) => translation switch
        {
            // TODO
            _ => null
        };
    }
}
