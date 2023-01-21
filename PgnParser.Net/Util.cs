using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Neondactyl.PgnParser.Net
{
    class Util
    {
        private static Dictionary<char, char> translations = new Dictionary<char, char> {
            { 'Š', 'S' },
            {'š', 's'},
            {'Ž', 'Z'},
            {'ž', 'z'},
            {'Č', 'C'},
            {'č', 'c'},
            {'Ć', 'C'},
            {'ć', 'c'},
            {'À', 'A'},
            {'Á', 'A'},
            {'Â', 'A'},
            {'Ã', 'A'},
            {'Ä', 'A'},
            {'Å', 'A'},
            {'Æ', 'A'},
            {'Ç', 'C'},
            {'È', 'E'},
            {'É', 'E'},
            {'Ê', 'E'},
            {'Ë', 'E'},
            {'Ì', 'I'},
            {'Í', 'I'},
            {'Î', 'I'},
            {'Ï', 'I'},
            {'Ñ', 'N'},
            {'Ò', 'O'},
            {'Ó', 'O'},
            {'Ô', 'O'},
            {'Õ', 'O'},
            {'Ö', 'O'},
            {'Ø', 'O'},
            {'Ù', 'U'},
            {'Ú', 'U'},
            {'Û', 'U'},
            {'Ü', 'U'},
            {'Ý', 'Y'},
            {'Þ', 'B'},
            {'ß', 'S'},
            {'à', 'a'},
            {'á', 'a'},
            {'â', 'a'},
            {'ã', 'a'},
            {'ä', 'a'},
            {'å', 'a'},
            {'æ', 'a'},
            {'ç', 'c'},
            {'è', 'e'},
            {'é', 'e'},
            {'ê', 'e'},
            {'ë', 'e'},
            {'ì', 'i'},
            {'í', 'i'},
            {'î', 'i'},
            {'ï', 'i'},
            {'ð', 'o'},
            {'ñ', 'n'},
            {'ò', 'o'},
            {'ó', 'o'},
            {'ô', 'o'},
            {'õ', 'o'},
            {'ö', 'o'},
            {'ø', 'o'},
            {'ù', 'u'},
            {'ú', 'u'},
            {'û', 'u'},
            {'ý', 'y'},
            {'ý', 'y'},
            {'þ', 'b'},
            {'ÿ', 'y'},
            {'Ŕ', 'R'},
            {'ŕ', 'r'},
            {'´', '\''}
            };
        public static string ForeignLettersToEnglishLetters(string text)
        {
            return translations.Aggregate(text, (current, value) => current.Replace(value.Key, value.Value));
        }

        public static string TitleCaseIfCurrentlyAllCaps(string text)
        {
            TextInfo ti = new CultureInfo("en-US", false).TextInfo;
            return (text.ToUpper() == text) ? ti.ToTitleCase(text) : text;
        }

        private static string[] CommonAbbreviationsForUnknownPlayer =
        {
            "?",
            "nn",
            "anonymous",
            "unknown"
        };

        public static string NormalizePlayerName(string name)
        {
            if (CommonAbbreviationsForUnknownPlayer.Contains(name)) return null;

            return TitleCaseIfCurrentlyAllCaps(ForeignLettersToEnglishLetters(name)).Replace("..", ".");
        }
    }
}
