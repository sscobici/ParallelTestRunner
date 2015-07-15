# ParallelTestRunner
Parallel test runner for Visual Studio tests

# Description
Allows parallel run of Visual Studio tests from command line. Primary usage is to speed up slow tests during Continuous Integration. It is possible for example to write [Selenium](http://www.seleniumhq.org/) UI tests using Visual Studio testing framework and scale them by using ParallelTestRunner and [Selenium Grid](http://www.seleniumhq.org/projects/grid/). Basically this tool runs several Visual Studio VSTest.Console.exe processes and executes one [TestClass] in each of them. The tool generates result.trx file by merging all test classes results.

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

# Requirements
.Net Framework 4.5 or higher

# Download
See gihub [releases](https://github.com/sscobici/ParallelTestRunner/releases).
Build was created by [AppVeyor](https://ci.appveyor.com/project/sscobici/paralleltestrunner) Continuous Integration tool
