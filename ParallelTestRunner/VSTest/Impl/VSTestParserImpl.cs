using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ParallelTestRunner.Common;

namespace ParallelTestRunner.VSTest.Impl
{
    public class VSTestParserImpl : IParser
    {
        public TestAssembly Parse(Assembly assembly)
        {
            IList<Type> types = new List<Type>();
            foreach (Type type in assembly.ExportedTypes)
            {
                if (type.CustomAttributes.Any(x => x.AttributeType.Name == "TestClassAttribute"))
                {
                    types.Add(type);
                }
            }

            TestAssembly item = new TestAssembly();
            item.Name = assembly.Location;
            item.Fixtures = new List<TestFixture>();
            foreach (Type type in types)
            {
                TestFixture fixture = new TestFixture();
                fixture.Name = type.FullName;
                CustomAttributeData groupAttr = type.CustomAttributes.FirstOrDefault(x => x.AttributeType.Name == "TestGroupAttribute");
                if (null != groupAttr)
                {
                    if (groupAttr.ConstructorArguments.Count > 0)
                    {
                        fixture.Group = (string)groupAttr.ConstructorArguments[0].Value;
                    }

                    if (groupAttr.ConstructorArguments.Count > 1)
                    {
                        fixture.Exclusive = (bool)groupAttr.ConstructorArguments[1].Value;
                    }

                    CustomAttributeNamedArgument name = groupAttr.NamedArguments.FirstOrDefault(x => x.MemberName == "Name");
                    if (name.MemberInfo != null)
                    {
                        fixture.Group = (string)name.TypedValue.Value;
                    }

                    CustomAttributeNamedArgument exclusive = groupAttr.NamedArguments.FirstOrDefault(x => x.MemberName == "Exclusive");
                    if (exclusive.MemberInfo != null)
                    {
                        fixture.Exclusive = (bool)exclusive.TypedValue.Value;
                    }
                }

                item.Fixtures.Add(fixture);
            }

            return item;
        }
    }
}