using System;

namespace SampleApi.Model
{
    public class Car
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PersonId { get; set; }
    }
}