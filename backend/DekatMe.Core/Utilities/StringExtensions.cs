using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace DekatMe.Core.Utilities
{
    public static class StringExtensions
    {
        public static string Truncate(this string value, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + suffix;
        }

        public static string ToTitleCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.ToLower());
        }

        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var words = value.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            var result = words[0].ToLower();
            for (int i = 1; i < words.Length; i++)
            {
                result += char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
            return result;
        }

        public static string ToSnakeCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return Regex.Replace(
                Regex.Replace(
                    Regex.Replace(value, @"([A-Z]+)([A-Z][a-z])", "$1_$2"), 
                    @"([a-z\d])([A-Z])", "$1_$2"),
                @"\s+", "_")
                .ToLower();
        }

        public static string ToKebabCase(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return Regex.Replace(
                Regex.Replace(
                    Regex.Replace(value, @"([A-Z]+)([A-Z][a-z])", "$1-$2"), 
                    @"([a-z\d])([A-Z])", "$1-$2"),
                @"\s+", "-")
                .ToLower();
        }

        public static string RemoveSpecialCharacters(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return Regex.Replace(value, @"[^\w\s]", string.Empty);
        }

        public static string RemoveAccents(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var normalizedString = value.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static bool IsValidEmail(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            try
            {
                // Use a simple regex for basic validation
                var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
                return regex.IsMatch(value);
                
                // For more comprehensive validation:
                // var addr = new System.Net.Mail.MailAddress(value);
                // return addr.Address == value;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsValidPhoneNumber(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            // Simple validation for international phone numbers
            // This regex matches formats like: +1234567890, 1234567890, 123-456-7890
            var regex = new Regex(@"^(\+\d{1,3})?[\s.-]?\(?\d{3}\)?[\s.-]?\d{3}[\s.-]?\d{4}$");
            return regex.IsMatch(value);
        }

        public static bool IsValidUrl(this string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            return Uri.TryCreate(value, UriKind.Absolute, out var uriResult) && 
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        public static string Reverse(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            var charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static string GetInitials(this string value, int maxInitials = 2)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            
            var words = value.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries);
            var initials = new StringBuilder();
            
            for (int i = 0; i < Math.Min(maxInitials, words.Length); i++)
            {
                if (!string.IsNullOrEmpty(words[i]))
                {
                    initials.Append(char.ToUpper(words[i][0]));
                }
            }
            
            return initials.ToString();
        }

        public static string[] SplitIntoLines(this string value, int maxLineLength)
        {
            if (string.IsNullOrEmpty(value)) return new string[0];
            if (maxLineLength <= 0) throw new ArgumentException("maxLineLength must be positive", nameof(maxLineLength));
            
            var words = value.Split(' ');
            var lines = new List<string>();
            var currentLine = new StringBuilder();
            
            foreach (var word in words)
            {
                if (currentLine.Length + word.Length + 1 > maxLineLength)
                {
                    lines.Add(currentLine.ToString().Trim());
                    currentLine.Clear();
                }
                
                if (currentLine.Length > 0)
                    currentLine.Append(' ');
                
                currentLine.Append(word);
            }
            
            if (currentLine.Length > 0)
                lines.Add(currentLine.ToString().Trim());
            
            return lines.ToArray();
        }

        public static bool ContainsIgnoreCase(this string source, string value)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(value)) return false;
            return source.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        public static string EnsureStartsWith(this string value, string prefix)
        {
            if (string.IsNullOrEmpty(value)) return prefix;
            return value.StartsWith(prefix) ? value : prefix + value;
        }

        public static string EnsureEndsWith(this string value, string suffix)
        {
            if (string.IsNullOrEmpty(value)) return suffix;
            return value.EndsWith(suffix) ? value : value + suffix;
        }

        public static string SafeSubstring(this string value, int startIndex, int length)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            if (startIndex < 0) startIndex = 0;
            if (startIndex >= value.Length) return string.Empty;
            if (length < 0) length = 0;
            
            int remainingLength = value.Length - startIndex;
            if (length > remainingLength) length = remainingLength;
            
            return value.Substring(startIndex, length);
        }

        public static string RemoveWhitespace(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return Regex.Replace(value, @"\s+", string.Empty);
        }

        public static string FormatPhoneNumber(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            
            // Remove all non-digit characters
            var digitsOnly = Regex.Replace(value, @"[^\d]", string.Empty);
            
            // Format based on length (this is a simplified example for US numbers)
            if (digitsOnly.Length == 10)
                return $"({digitsOnly.Substring(0, 3)}) {digitsOnly.Substring(3, 3)}-{digitsOnly.Substring(6)}";
            if (digitsOnly.Length == 11 && digitsOnly[0] == '1')
                return $"+1 ({digitsOnly.Substring(1, 3)}) {digitsOnly.Substring(4, 3)}-{digitsOnly.Substring(7)}";
            
            // If we don't recognize the format, just return with a + prefix if it's international
            if (digitsOnly.Length > 10)
                return $"+{digitsOnly}";
            
            return digitsOnly;
        }
    }
}
