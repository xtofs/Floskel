using Floskel;

var input = "ab 0xFF";

var decNumber = Parsers.OneOrMore(char.IsAsciiDigit).Parse<int>();

var hexNumber = Parsers.Tuple(
    Parsers.String("0x"),
    Parsers.OneOrMore(char.IsAsciiHexDigit).Select(s => int.Parse(s, System.Globalization.NumberStyles.HexNumber))
).Select(t => t.Second);

var number = hexNumber.Or(decNumber); // the order is important since hexNumber is a prefix of decNumber

var parser =
    from identifier in Parsers.OneOrMore(char.IsLetter)
    from _ in Parsers.OneOrMore(char.IsWhiteSpace)
    from num in number
    select (identifier, num);

if (parser(input, out var result, out var remainder))
{
    Console.WriteLine($"""success: result = {result}, remainder = "{remainder}" """);
}
