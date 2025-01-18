using Humanizer;

static class Generator
{

    public static void Generate()
    {

        using var file = File.CreateText("generated.cs");

        file.Write("namespace Floskel;\r\n\r\n");
        file.Write("public partial class Parsers\r\n{");
        for (int j = 2; j < 5; j++)
        {
            // string Seq(string s, string sep = ", ") => string.Join(sep, Enumerable.Range(1, i).Select(i => $"{s}{i}"));

            var i = j;

            string Seq(Func<int, string> fmt, string sep = ", ")
            {
                return string.Join(sep, Enumerable.Range(1, i).Select(fmt));
            }

            string template = $$"""

                public static TryParse<({{Seq(i => $"T{i} {i.ToOrdinalWords(WordForm.Abbreviation).Pascalize()}")}})> Tuple<{{Seq(i => $"T{i}")}}>({{Seq(i => $"TryParse<T{i}> parser{i}")}})
                {
                    return (StringSegment input, [MaybeNullWhen(false)] out ({{Seq(i => $"T{i}")}}) result, out StringSegment remainder) =>
                    {
                        var remainder0 = input;
                        if ({{Seq(i => $"parser{i}(remainder{i - 1}, out var result{i}, out var remainder{i})", " &&\r\n            ")}})
                        {
                            result = ({{Seq(i => $"result{i}")}});
                            remainder = remainder{{i}};
                            return true;
                        }
                        result = default!;
                        remainder = input;
                        return false;
                    };
                }
            """;

            file.Write(template);
        }
        file.Write("\r\n}\r\n");
    }
}