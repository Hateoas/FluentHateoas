using System.Linq;
using System.Web.Mvc;
using AutoMapper;

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
        public IEnumerable<Person> Get()
        {
            return new List<Data.Model.Person>
            {
                new Data.Model.Person {Id = Guid.Parse("0CFA46CC-116D-45F0-B57B-4C5586130072")},
                new Data.Model.Person {Id = Guid.Parse("F6D0CF9E-5153-4218-8A22-8E71C5AC5A4A")}
            }.Select(Mapper.Map<Person>);
        }

        [Authorize]
        [Route("all")]
        public IEnumerable<Person> GetAll()
        {
            return new List<Person>();
        }

        [Route("byparams")]
        public IEnumerable<Person> GetAllWithParams(string searchParam)
        {
            return new List<Person>();
        }

        public Person GetById(Guid id)
        {
            return new Person { Id = Guid.Parse("96D74B0D-4456-4643-A5FD-D0A31AF0C284"), HouseId = Guid.Parse("46B18438-A460-4C65-BFC7-8538E7F93568")};
        }

        [Route("{id}/father")]
        public Person GetFather(Guid id)
        {
            return new Person { Id = Guid.Parse("058D7A99-FCDD-4E67-8569-9B050CAD85EB") };
        }

        [Route("{id}/parents")]
        public IEnumerable<Person> GetParents()
        {
            return new[]
            {
                new Person {Id = Guid.Parse("0CFA46CC-116D-45F0-B57B-4C5586130072")},
                new Person {Id = Guid.Parse("F6D0CF9E-5153-4218-8A22-8E71C5AC5A4A")}
            };
        }

        public Person Post(CreatePersonRequest request)
        {
            return new Person { Id = Guid.Parse("C527F33E-D150-4A5D-BE8A-C10DE0451662") };
        }

        [HttpPost]
        [Route("child")]
        public Person AddChild(CreatePersonRequest request)
        {
            return new Person { Id = Guid.Parse("625E3C75-2330-423D-9D2B-2E9A883706EC") };
        }

        public Person Put(UpdatePersonRequest request)
        {
            return new Person { Id = Guid.Parse("82878C8F-083F-4741-BA84-BCDE4EE51697") };
        }

        public void Delete(Guid id) { }
    }
}