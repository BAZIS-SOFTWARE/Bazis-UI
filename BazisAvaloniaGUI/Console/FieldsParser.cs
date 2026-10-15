using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BazisAvaloniaGUI.Utilities
{
    public static class FieldsParser
    {
        private static readonly char[] stopCharsAr = { ' ', '\"', '\'' };

        public static List<string> ParseLine(string line)
        {
            var tokenizedString = new List<string>();

            var startIndex = 0;

            while (startIndex < line.Length)
            {
                var nextToken = ReadField(line, startIndex);

                if (line[startIndex] != ' ')
                {
                    tokenizedString.Add(nextToken.Value);
                    startIndex += nextToken.Length;
                }
                else
                {
                    startIndex++;
                }
            }
            
            return tokenizedString;
        }

        private static Token ReadField(string line, int startIndex)
        {
            var startToken = line[startIndex];
            var start = line[startIndex];
            switch (start)
            {
                case ' ':
                    return new Token(" ", startIndex, 1);
                case '"':
                    return ParseQuotedField(line, startIndex, "\"");
                case '\'':
                    return ParseQuotedField(line, startIndex, "'");
                default:
                    return ParseFieldWithoutQuotes(line, startIndex);
            }
        }

        private static Token ParseField(string line, int startIndex, char[] stopChars, int quotesNumber)
        {
            var tokenValue = new StringBuilder();
            var quotesInsideNumber = 0;
            var slashScreen = 0;
            var i = startIndex;
            while (i < line.Length)
            {
                var a = line[i];
                if (stopChars.Contains(a)) break;

                if (stopChars.Length > 1) tokenValue.Append(a);
                else
                {
                    // Обратная косая черта последним символом (незакрытая кавычка): экранировать нечего,
                    // обращение к line[i + 1] выходило за границу строки.
                    var next = i + 1 < line.Length ? line[i + 1] : '\0';
                    if (a == '\\' && next == '\\')
                    {
                        tokenValue.Append(a);
                        slashScreen++;
                        i++;
                    }
                    else if (a == '\\' && next == '\'')
                    {
                        tokenValue.Append('\'');
                        quotesInsideNumber++;
                        i++;
                    }
                    else if (a == '\\' && next == '\"')
                    {
                        tokenValue.Append('\"');
                        quotesInsideNumber++;
                        i++;
                    }

                    else { tokenValue.Append(a); }
                }
                i++;
            }

            return new Token(tokenValue.ToString(), startIndex, tokenValue.Length + quotesNumber + quotesInsideNumber + slashScreen);
        }

        private static Token ParseQuotedField(string line, int startIndex, string quote) => ParseField(line, startIndex + 1, quote.ToCharArray(), 2);
        private static Token ParseFieldWithoutQuotes(string line, int startIndex) => ParseField(line, startIndex, stopCharsAr, 0);
    }
}
