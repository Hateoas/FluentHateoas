using System;

namespace SampleApi.Providers
{
    public interface IPersonProvider
    {
        bool HasNextId(object obj);
        Guid GetNextId(object obj);
    }

    public class PersonProvider : IPersonProvider
    {
        public bool HasNextId(object obj)
        {
            return true;
        }

        public Guid GetNextId(object obj)
        {
            return Guid.Parse("5B8DC86A-72A2-40E8-BDA7-EF35FBD26399");
        }
    }
}