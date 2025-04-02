using System.Text;
using System.Text.RegularExpressions;

namespace DekatMe.Core.Utilities
{
    public static class SlugGenerator
    {
        public static string GenerateSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Convert to lowercase
            var slug = input.ToLowerInvariant();

            // Remove diacritics (accents)
            slug = RemoveDiacritics(slug);

            // Replace spaces with hyphens
            slug = Regex.Replace(slug, @"\s+", "-");

            // Remove invalid characters
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", string.Empty);

            // Remove duplicate hyphens
            slug = Regex.Replace(slug, @"\-{2,}", "-");

            // Trim hyphens from start and end
            slug = slug.Trim('-');

            return slug;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string GenerateUniqueSlug(string input, Func<string, Task<bool>> existsAsync)
        {
            var slug = GenerateSlug(input);
            
            // Ensure the slug is unique by appending numbers if necessary
            return EnsureUniqueSlugAsync(slug, existsAsync).GetAwaiter().GetResult();
        }

        private static async Task<string> EnsureUniqueSlugAsync(string slug, Func<string, Task<bool>> existsAsync)
        {
            if (!await existsAsync(slug))
                return slug;

            var i = 1;
            var newSlug = slug;

            while (await existsAsync(newSlug))
            {
                newSlug = $"{slug}-{i}";
                i++;
            }

            return newSlug;
        }
    }
}
