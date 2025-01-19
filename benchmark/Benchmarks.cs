using Floskel;

using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace benchmark
{
    public class Benchmarks
    {

        private static readonly TryParse<int> HexNumber1 =
            from prefix in Parsers.String("0x")
            from num in Parsers.OneOrMore(char.IsAsciiHexDigit).Select(s => int.Parse(s, System.Globalization.NumberStyles.HexNumber))
            select num;

        [Benchmark(Baseline = true)]
        public bool Scenario1()
        {
            return HexNumber1("0xFFFF", out var result, out var remainder);
        }


        private static readonly TryParse<int> HexNumber2 = Parsers.Tuple(
            Parsers.String("0x"),
            Parsers.OneOrMore(char.IsAsciiHexDigit).Select(s => int.Parse(s, System.Globalization.NumberStyles.HexNumber))
        ).Select(t => t.Second);

        [Benchmark]
        public bool Scenario2()
        {
            return HexNumber2("0xFFFF", out var result, out var remainder);
        }
    }
}
