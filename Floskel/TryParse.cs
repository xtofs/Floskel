namespace Floskel;

public delegate bool TryParse<TResult>(StringSegment input, [MaybeNullWhen(false)] out TResult result, out StringSegment remainder);

