namespace WebApi.ViewModels
{
    public class Passage
    {
        public int PassageId { get; }

        public string Reference { get; }

        public string Text { get; }

        public string? Translation { get; }

        public Passage(int passageId, string reference, string text, string? translation)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                throw new System.ArgumentException("Passage reference must not be null or empty", nameof(reference));
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new System.ArgumentException("Passage text must not be null or empty", nameof(text));
            }

            PassageId = passageId;
            Reference = reference;
            Text = text;
            Translation = translation;
        }
    }
}
