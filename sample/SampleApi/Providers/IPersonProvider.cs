using System;

namespace SampleApi.Providers
{
    public interface IPersonProvider
    {
        bool HasNextId(object obj);
        Guid GetNextId(object obj);
    }
}