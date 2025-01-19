using Floskel;

var input = "ab1 0xFF";

var decNumber = Parsers.OneOrMore(char.IsAsciiDigit).Parse<int>();

var hexNumber = Parsers.Tuple(
    Parsers.String("0x"),
    Parsers.OneOrMore(char.IsAsciiHexDigit).Select(s => int.Parse(s, System.Globalization.NumberStyles.HexNumber))
).Select(t => t.Second);

var number = hexNumber.Or(decNumber); // the order is important since hexNumber and decNumber share the "0" prefix

// var identifier = Parsers.OneOrMore(char.IsLetter);
var identifier = Parsers.Regex("[a-zA-Z_][a-zA-Z0-9_]*");

var parser =
    from ident in identifier
    from _ in Parsers.OneOrMore(char.IsWhiteSpace)
    from num in number
    select (ident, num);

if (parser(input, out var result, out var remainder))
{
    Console.WriteLine($"""success: result = {result}, remainder = "{remainder}" """);
}


public static class ValueTupleExtensions
{
    public static ValueTuple<T1> Select1<T1, T2>(this ValueTuple<T1, T2> tuple)
    {
        return ValueTuple.Create(tuple.Item1);
    }

    public static ValueTuple<T2> Select2<T1, T2>(this ValueTuple<T1, T2> tuple)
    {
        return ValueTuple.Create(tuple.Item2);
    }

    public static ValueTuple<T1> Select1<T1, T2, T3>(this ValueTuple<T1, T2, T3> tuple) => ValueTuple.Create(tuple.Item1);
    public static ValueTuple<T2> Select2<T1, T2, T3>(this ValueTuple<T1, T2, T3> tuple) => ValueTuple.Create(tuple.Item2);
    public static ValueTuple<T3> Select3<T1, T2, T3>(this ValueTuple<T1, T2, T3> tuple) => ValueTuple.Create(tuple.Item3);
    public static ValueTuple<T1, T2> Select12<T1, T2, T3>(this ValueTuple<T1, T2, T3> tuple) => ValueTuple.Create(tuple.Item1, tuple.Item2);
    public static ValueTuple<T1, T3> Select13<T1, T2, T3>(this ValueTuple<T1, T2, T3> tuple) => ValueTuple.Create(tuple.Item1, tuple.Item3);
    public static ValueTuple<T2, T3> Select23<T1, T2, T3>(this ValueTuple<T1, T2, T3> tuple) => ValueTuple.Create(tuple.Item2, tuple.Item3);

}