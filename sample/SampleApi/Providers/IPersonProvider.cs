using System;
using System.Diagnostics.CodeAnalysis;

namespace SampleApi.Providers
{
    public interface IPersonProvider
    {
        bool HasNextId(object obj);
        Guid GetNextId(object obj);
        Guid GetPreviousId(object obj);
    }

    [ExcludeFromCodeCoverage]
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

        public Guid GetPreviousId(object obj)
        {
            return Guid.Parse("A1557C62-2BA5-402D-A879-EB17E811EDD0");
        }
    }
}