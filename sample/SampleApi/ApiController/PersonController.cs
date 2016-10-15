namespace SampleApi.ApiController
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Http;

    using SampleApi.Model;

    [ExcludeFromCodeCoverage]
    public class PersonController : ApiController
    {
        public void AddSimplePerson()
        {
            
        }

        public void AddPerson(Person person)
        {
            
        }

        public Person GetPerson(Guid id)
        {
            return null;
        }

        public Person Get()
        {
            return null;
        }

        public IEnumerable<Person> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Person> GetAllWithParams(string searchParam)
        {
            throw new NotImplementedException();
        }
    }
}