using System;

namespace Common.Attributes
{
    public class ValueAttribute : Attribute
    {
        public int[] Values { get; set; }


        public ValueAttribute(int[] values)
        {
            Values = values;
        }
    }
}