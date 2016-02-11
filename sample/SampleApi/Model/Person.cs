using System;

namespace SampleApi.Model
{
    public class Person
    {
        public Guid Id { get; set; }
        public string Slug { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}