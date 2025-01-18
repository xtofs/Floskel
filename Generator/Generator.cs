using Humanizer;

static class Generator
{

    public static void Generate()
    {

        using var file = File.CreateText("generated.cs");

        file.Write("namespace Floskel;\r\n\r\n");
        file.Write("public partial class Parsers\r\n{");

        static string Seq(int n, Func<int, string> fmt, string sep = ", ")
        {
            return string.Join(sep, Enumerable.Range(1, n).Select(fmt));
        }

        for (int j = 2; j < 5; j++)
        {
            var i = j;

            string tupleParser = $$"""

                public static TryParse<({{Seq(j, i => $"T{i} {i.ToOrdinalWords(WordForm.Abbreviation).Pascalize()}")}})> Tuple<{{Seq(j, i => $"T{i}")}}>({{Seq(j, i => $"TryParse<T{i}> parser{i}")}})
                {
                    return (StringSegment input, [MaybeNullWhen(false)] out ({{Seq(j, i => $"T{i}")}}) result, out StringSegment remainder) =>
                    {
                        var remainder0 = input;
                        if ({{Seq(i, i => $"parser{i}(remainder{i - 1}, out var result{i}, out var remainder{i})", " &&\r\n            ")}})
                        {
                            result = ({{Seq(i, i => $"result{i}")}});
                            remainder = remainder{{i}};
                            return true;
                        }
                        result = default!;
                        remainder = input;
                        return false;
                    };
                }
            """;

            file.Write(tupleParser);
        }

        for (int i = 2; i < 5; i++)
        {
            string alternativeParser = $$"""

                public static TryParse<T> Or<T>(this {{Seq(i, i => $"TryParse<T> parser{i}")}})
                {
                    return (StringSegment input, [MaybeNullWhen(false)] out T result, out StringSegment remainder) =>
                    { 
                        return {{Seq(i, i => $"parser{i}(input, out result, out remainder)", " || ")}};                        
                    };
                }
            """;

            file.Write(alternativeParser);
        }

        file.Write("\r\n}\r\n");
    }
}