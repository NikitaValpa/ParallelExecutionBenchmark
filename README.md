# ParallelExecutionBenchmark

<p align="center">
  <img src="https://img.shields.io/badge/state-release-blue" alt="State" /> 
  <img src="https://img.shields.io/badge/.NET-8-blue"  alt=".NET Version" /> 
  <img src="https://img.shields.io/github/contributors/NikitaValpa/ParallelExecutionBenchmark?color=blue" alt="All contributors" />
  <img src="https://img.shields.io/github/last-commit/NikitaValpa/ParallelExecutionBenchmark/master?color=blue" />
  <img src="https://img.shields.io/github/v/tag/NikitaValpa/ParallelExecutionBenchmark?color=blue&label=latest%20tag" alt="Latest tag" />
</p>

This is a small example of a benchmark for parallel code execution on a thread pool, versus linear code execution. I've always wondered where the line is when it makes sense to use parallelism.

And in my small example, this line passes somewhere around 10,000 iterations of incrementing the variable. The results of the run below clearly show that already at 20,000 iterations, parallel execution in 5 threads of 4000 iterations each, begins to overtake linear execution.

```

BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.3930/22H2/2022Update)
AMD Ryzen 5 5600X, 1 CPU, 12 logical and 6 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2


```
| Method                    | tasksCount | iterationsCount | Mean       | Error     | StdDev    | Median     | P50        | P85        | P95        |
|-------------------------- |----------- |---------------- |-----------:|----------:|----------:|-----------:|-----------:|-----------:|-----------:|
| **ParallelAsynchronouslySum** | **5**          | **2000**            |   **2.242 μs** | **0.0076 μs** | **0.0067 μs** |   **2.241 μs** |   **2.241 μs** |   **2.248 μs** |   **2.253 μs** |
| **ParallelAsynchronouslySum** | **5**          | **4000**            |   **3.024 μs** | **0.0160 μs** | **0.0142 μs** |   **3.026 μs** |   **3.026 μs** |   **3.042 μs** |   **3.044 μs** |
| **ParallelAsynchronouslySum** | **5**          | **200000**          |  **71.538 μs** | **1.3950 μs** | **1.4926 μs** |  **71.296 μs** |  **71.296 μs** |  **73.250 μs** |  **73.957 μs** |
| **SynchronouslySum**          | **?**          | **10000**           |   **2.181 μs** | **0.0093 μs** | **0.0087 μs** |   **2.182 μs** |   **2.182 μs** |   **2.188 μs** |   **2.190 μs** |
| **SynchronouslySum**          | **?**          | **20000**           |   **4.355 μs** | **0.0321 μs** | **0.0300 μs** |   **4.367 μs** |   **4.367 μs** |   **4.384 μs** |   **4.395 μs** |
| **SynchronouslySum**          | **?**          | **1000000**         | **217.276 μs** | **1.0472 μs** | **0.9283 μs** | **217.397 μs** | **217.397 μs** | **218.194 μs** | **218.346 μs** |


In fact, the key here is the granularity of the execution of the work items that are placed on the thread pool. My example is very artificial and has little to do with real practice. Since often, the granularity of the executed code depends on many conditions, input data, business logic, etc., and it is not constant.

However, this benchmark, in my opinion, clearly demonstrates that switching to parallel code execution makes sense only when the costs of organizing parallel execution no longer outweigh simple linear code execution.