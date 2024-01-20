using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace ParallelExecutionBenchmark
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkMethods>();
        }

        internal class ParallelResultObject
        {
            public uint Sum;
        }
    }

    [Config(typeof(Config))]
    public class BenchmarkMethods
    {
        [Arguments(1_000_000)]
        [Arguments(10_000)]
        [Arguments(20_000)]
        [Benchmark]
        public uint SynchronouslySum(int iterationsCount)
        {
            uint sum = default;
            for (var i = 0; i < iterationsCount; i++)
            {
                ++sum;
            }

            return sum;
        }

        [Arguments(5, 200_000)]
        [Arguments(5, 2000)]
        [Arguments(5, 4000)]
        [Benchmark]
        public async Task<uint> ParallelAsynchronouslySum(int tasksCount, int iterationsCount)
        {
            var tasks = new List<Task>();
            var res = new Program.ParallelResultObject();

            for (var i = 0; i < tasksCount; i++) // threads
            {
                var task = Task.Run(() =>
                {
                    uint sum = default;
                    for (var j = 0; j < iterationsCount; j++)
                    {
                        ++sum;
                    }

                    Interlocked.Add(ref res.Sum, sum);
                });

                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            return res.Sum;
        }

        private class Config : ManualConfig
        {
            public Config()
            {
                AddColumn(
                    StatisticColumn.Median,
                    StatisticColumn.P50,
                    StatisticColumn.P85,
                    StatisticColumn.P95
                );
            }
        }
    }
}
