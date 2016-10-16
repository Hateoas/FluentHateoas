using System;

namespace FluentHateoas.Attributes
{
    public class MaxValueAttribute : Attribute
    {
        public int MaximumValue { get; private set; }

        public MaxValueAttribute(int maximumValue)
        {
            MaximumValue = maximumValue;
        }
    }
}