namespace SampleApi.ApiController
{
    using System;
    using System.Collections.Generic;

    using SampleApi.Model;

    public class PersonController
    {
        public void AddSimplePerson()
        {
            
        }

        public void AddPerson()
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