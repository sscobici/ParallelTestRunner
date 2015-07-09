using Autofac;
using ParallelTestRunner.Common;
using ParallelTestRunner.Common.Impl;
using ParallelTestRunner.Impl;
using ParallelTestRunner.VSTest;
using ParallelTestRunner.VSTest.Common;
using ParallelTestRunner.VSTest.Common.Impl;
using ParallelTestRunner.VSTest.Impl;

namespace ParallelTestRunner.Autofac
{
    public class AutofacContainer
    {
        public static IContainer RegisterTypes(ITestRunnerArgs testArgs)
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(testArgs).As<ITestRunnerArgs>().SingleInstance();
            builder.RegisterType<TestRunnerImpl>().As<ITestRunner>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<TrxWriter>().As<ITrxWriter>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<BreakerImpl>().As<IBreaker>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<ThreadFactoryImpl>().As<IThreadFactory>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<WindowsFileHelperImpl>().As<IWindowsFileHelper>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<SummaryCalculatorImpl>().As<ISummaryCalculator>().SingleInstance().PropertiesAutowired();

            if (testArgs.Provider.StartsWith("VSTEST"))
            {
                builder.RegisterType<VSTestParserImpl>().As<IParser>().SingleInstance().PropertiesAutowired();
                builder.RegisterType<VSTestExecutorImpl>().As<IExecutor>().SingleInstance().PropertiesAutowired();
                builder.RegisterType<VSTestCollectorImpl>().As<ICollector>().SingleInstance().PropertiesAutowired();

                builder.RegisterType<ProcessStartInfoFactoryImpl>().As<IProcessStartInfoFactory>().SingleInstance().PropertiesAutowired();
                builder.RegisterType<ProcessFactoryImpl>().As<IProcessFactory>().SingleInstance().PropertiesAutowired();
                builder.RegisterType<ProcessOutputReaderFactoryImpl>().As<IProcessOutputReaderFactory>().SingleInstance().PropertiesAutowired();
                builder.RegisterType<VSTestFileHelperImpl>().As<IVSTestFileHelper>().SingleInstance().PropertiesAutowired();
                builder.RegisterType<TrxParser>().As<ITrxParser>().SingleInstance().PropertiesAutowired();
                builder.RegisterType<VSTestCleanerImpl>().As<ICleaner>().SingleInstance().PropertiesAutowired();
            }
            else if (testArgs.Provider.StartsWith("NUNIT"))
            {
                // builder.RegisterType<NunitParserImpl>().As<IParser>().SingleInstance().PropertiesAutowired();
                // builder.RegisterType<NunitExecutorImpl>().As<IExecutor>().SingleInstance().PropertiesAutowired();
                // builder.RegisterType<NunitCollectorImpl>().As<ICollector>().SingleInstance().PropertiesAutowired();

                // builder.RegisterType<ProcessStartInfoFactoryImpl>().As<IProcessStartInfoFactory>().SingleInstance().PropertiesAutowired();
                // builder.RegisterType<ProcessFactoryImpl>().As<IProcessFactory>().SingleInstance().PropertiesAutowired();
                // builder.RegisterType<ProcessOutputReaderFactoryImpl>().As<IProcessOutputReaderFactory>().SingleInstance().PropertiesAutowired();
                // builder.RegisterType<FileHelperImpl>().As<IFileHelper>().SingleInstance().PropertiesAutowired();
            }

            builder.RegisterType<RunDataBlockingBuilderImpl>().As<IRunDataBuilder>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<RunDataListBuilderImpl>().As<IRunDataListBuilder>().SingleInstance().PropertiesAutowired();
            builder.RegisterType<ExecutorLauncherImpl>().As<IExecutorLauncher>().SingleInstance().PropertiesAutowired();
            return builder.Build();
        }
    }
}