using System;

namespace FluentHateoasTest.Assets.Providers
{
    public interface IPersonProvider
    {
        bool HasNextId(object obj);
        Guid GetNextId(object obj);
        Guid GetPreviousId(object obj);
    }
}