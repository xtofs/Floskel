using Floskel;

using BenchmarkDotNet;
using BenchmarkDotNet.Attributes;

namespace benchmark
{
    public class Benchmarks
    {

        private static readonly TryParse<int> hexNumber1 =
            from prefix in Parsers.String("0x")
            from num in Parsers.While(char.IsAsciiHexDigit).Select(s => int.Parse(s, System.Globalization.NumberStyles.HexNumber))
            select num;

        [Benchmark]
        public bool Scenario1()
        {
            return hexNumber1("0xFF", out var result, out var remainder);
        }


        private static readonly TryParse<int> hexNumber2 = Parsers.Tuple(
            Parsers.String("0x"),
            Parsers.While(char.IsAsciiHexDigit).Select(s => int.Parse(s, System.Globalization.NumberStyles.HexNumber))
        ).Select(t => t.Second);

        [Benchmark]
        public bool Scenario2()
        {
            return hexNumber2("0xFF", out var result, out var remainder);
        }
    }
}
