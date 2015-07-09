using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITests
{
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
}
