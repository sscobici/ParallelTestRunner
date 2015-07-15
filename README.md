# ParallelTestRunner
Parallel test runner for Visual Studio tests

# Description
Allows parallel run of Visual Studio tests from the command line. Primary usage is to speed up slow tests during Continuous Integration process. It is possible for example to write [Selenium](http://www.seleniumhq.org/) UI tests using Visual Studio testing framework and scale them by using ParallelTestRunner and [Selenium Grid](http://www.seleniumhq.org/projects/grid/). Basically this tool runs several Visual Studio VSTest.Console.exe processes and executes one [TestClass] in each of them. The tool generates result.trx file by merging all test classes results.

# Usage
```
ParallelTestRunner.exe [options] [assembly]...

Options:
  provider:        specifies which version of VSTest.Console.exe to use: VSTEST_2012, VSTEST_2013
  threadcount:     specifies the number of parallel processes, default is 4
  root:            the working directory where the temporary files will be generated
  out:             resulting trx file, can be absolute path or relative to the working directory
  
assembly           the list of assemblies that contain visual studio tests

Examples:
  ParallelTestRunner.exe provider:VSTEST_2013 threadcount:10 root:./TestResults out:result.trx ./UITests/SeleniumIntegration.Tests.dll
```

# Download
See [releases](https://github.com/sscobici/ParallelTestRunner/releases).
Build was created by [AppVeyor](https://ci.appveyor.com/project/sscobici/paralleltestrunner) Continuous Integration tool

# Additional Information
By default all TestClasses are executed in parallel. TestMethods inside each TestClass are executed consecutively.
There is a possibility to group several TestClasses in order to execute them consecutively.

Create the following Attribute in your test project:
```
    public class TestClassGroupAttribute : Attribute
    {
        public TestClassGroupAttribute()
        {
        }

        public TestClassGroupAttribute(string name)
        {
            Name = name;
        }

        public TestClassGroupAttribute(string name, bool exclusive)
            : this(name)
        {
            Exclusive = exclusive;
        }

        public string Name { get; set; }
        
        public bool Exclusive { get; set; }
    }
```

In the below example two groups are defined to be executed in parallel. ClassA and ClassB tests will be executed consecutively.

```
[TestClassGroup("FirstGroup")]
ClassA { ... }

[TestClassGroup("FirstGroup")]
ClassB { ... }

[TestClassGroup("SecondGroup")]
ClassC { ... }
```

Specify attribute parameter Exclusive = true if there is a need to run some tests exclusively. This will ensure that no other tests are run in parallel at that time.

```
[TestClassGroup("ExclusiveGroup", Exclusive = true)]
ClassExclusive { ... }
```

# Requirements
.Net Framework 4.5 or higher
