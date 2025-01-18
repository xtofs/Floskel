namespace Floskel;

public partial class Parsers
{
    public static TryParse<(T1 First, T2 Second)> Tuple<T1, T2>(TryParse<T1> parser1, TryParse<T2> parser2)
    {
        return (StringSegment input, [MaybeNullWhen(false)] out (T1, T2) result, out StringSegment remainder) =>
        {
            var remainder0 = input;
            if (parser1(remainder0, out var result1, out var remainder1) &&
            parser2(remainder1, out var result2, out var remainder2))
            {
                result = (result1, result2);
                remainder = remainder2;
                return true;
            }
            result = default!;
            remainder = input;
            return false;
        };
    }
    public static TryParse<(T1 First, T2 Second, T3 Third)> Tuple<T1, T2, T3>(TryParse<T1> parser1, TryParse<T2> parser2, TryParse<T3> parser3)
    {
        return (StringSegment input, [MaybeNullWhen(false)] out (T1, T2, T3) result, out StringSegment remainder) =>
        {
            var remainder0 = input;
            if (parser1(remainder0, out var result1, out var remainder1) &&
            parser2(remainder1, out var result2, out var remainder2) &&
            parser3(remainder2, out var result3, out var remainder3))
            {
                result = (result1, result2, result3);
                remainder = remainder3;
                return true;
            }
            result = default!;
            remainder = input;
            return false;
        };
    }
    public static TryParse<(T1 First, T2 Second, T3 Third, T4 Fourth)> Tuple<T1, T2, T3, T4>(TryParse<T1> parser1, TryParse<T2> parser2, TryParse<T3> parser3, TryParse<T4> parser4)
    {
        return (StringSegment input, [MaybeNullWhen(false)] out (T1, T2, T3, T4) result, out StringSegment remainder) =>
        {
            var remainder0 = input;
            if (parser1(remainder0, out var result1, out var remainder1) &&
            parser2(remainder1, out var result2, out var remainder2) &&
            parser3(remainder2, out var result3, out var remainder3) &&
            parser4(remainder3, out var result4, out var remainder4))
            {
                result = (result1, result2, result3, result4);
                remainder = remainder4;
                return true;
            }
            result = default!;
            remainder = input;
            return false;
        };
    }
}
