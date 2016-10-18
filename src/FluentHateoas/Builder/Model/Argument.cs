using System;

namespace FluentHateoas.Builder.Model
{
    public class Argument
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }
        public bool IsTemplateArgument { get; set; }
    }
}