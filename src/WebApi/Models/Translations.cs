namespace WebApi.Models
{
    public enum Translations
    {
        /// <summary>
        /// American Standard Version
        /// </summary>
        ASV,

        /// <summary>
        /// King James Version
        /// </summary>
        KJV,

        /// <summary>
        /// New International Version
        /// </summary>
        NIV,

        /// <summary>
        /// Englsh Standard Version
        /// </summary>
        ESV,

        /// <summary>
        /// New Living Translation
        /// </summary>
        NLT,

        /// <summary>
        /// Revised Standard Version
        /// </summary>
        RSV
    }

    public static class TranslationExtensions
    {
        /// <summary>
        /// Gets copyright notices for different Bible translations.
        /// </summary>
        /// <param name="translation">The translation to get a copyright notice for.</param>
        /// <returns>The copyright notice, or null if none exists.</returns>
        public static string? GetCopyrightNotice(this Translations translation) => translation switch
        {
            _ => null
        };
    }
}
