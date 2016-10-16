using System;

namespace FluentHateoas.Attributes
{
    public class MinValueAttribute : Attribute
    {
        public int MinimumValue { get; private set; }

        public MinValueAttribute(int minimumValue)
        {
            MinimumValue = minimumValue;
        }
    }
}