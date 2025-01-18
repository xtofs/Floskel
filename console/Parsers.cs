namespace Tongue;

using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Primitives;

public delegate bool TryParse<TResult>(StringSegment input, [MaybeNullWhen(false)] out TResult result, out StringSegment remainder);

// public delegate bool TryParse<TToken, TResult>(ReadOnlyMemory<TToken> input, [MaybeNullWhen(false)] out TResult result, out ReadOnlyMemory<TToken> remainder);

public static partial class Parsers
{
    public static TryParse<G> SelectMany<E, F, G>(this TryParse<E> tryParse, Func<E, TryParse<F>> secondary, Func<E, F, G> selector)
    {
        return (StringSegment input, [MaybeNullWhen(false)] out G result, out StringSegment remainder) =>
        {
            if (tryParse(input, out var intermediate, out var intermediateRemainder))
            {
                var second = secondary(intermediate);
                if (second(intermediateRemainder, out var secondaryResult, out remainder))
                {
                    result = selector(intermediate, secondaryResult);
                    return true;
                }
            }
            result = default!;
            remainder = input;
            return false;
        };
    }

    public static TryParse<F> Select<E, F>(this TryParse<E> tryParse, Func<E, F> selector)
    {
        return (StringSegment input, [MaybeNullWhen(false)] out F result, out StringSegment remainder) =>
        {
            if (tryParse(input, out var intermediate, out remainder))
            {
                result = selector(intermediate);
                return true;
            }
            result = default!;
            return false;
        };
    }

    /// <summary>
    /// Transforming a StringSegment parser into and E parser using the ISpanParsable interface.
    /// </summary>
    /// <typeparam name="E"></typeparam>
    /// <param name="parse"></param>
    /// <returns></returns>
    public static TryParse<E> Parse<E>(this TryParse<StringSegment> parse, IFormatProvider? provider = null) where E : ISpanParsable<E>
    {
        return (StringSegment input, [MaybeNullWhen(false)] out E result, out StringSegment remainder) =>
        {
            if (parse(input, out var intermediate, out remainder))
            {
                if (E.TryParse(intermediate.AsSpan(), provider, out result))
                {
                    return true;
                }
            }
            result = default!;
            return false;
        };
    }

    public static TryParse<StringSegment> While(Func<char, bool> predicate)
    {
        return (StringSegment input, [MaybeNullWhen(false)] out StringSegment result, out StringSegment remainder) =>
        {
            var index = 0;
            while (index < input.Length && predicate(input[index]))
            {
                index++;
            }
            result = input.Subsegment(0, index);
            remainder = input.Subsegment(index);
            return true;
        };
    }

    public static TryParse<E> Or<E>(this TryParse<E> first, TryParse<E> second)
    {
        return (StringSegment input, [MaybeNullWhen(false)] out E result, out StringSegment remainder) =>
            first(input, out result, out remainder) || second(input, out result, out remainder);
    }

    internal static TryParse<string> String(string v)
    {
        return (StringSegment input, [MaybeNullWhen(false)] out string result, out StringSegment remainder) =>
        {
            if (input.StartsWith(v, StringComparison.Ordinal))
            {
                result = v;
                remainder = input.Subsegment(v.Length);
                return true;
            }
            result = default!;
            remainder = input;
            return false;
        };
    }


}


