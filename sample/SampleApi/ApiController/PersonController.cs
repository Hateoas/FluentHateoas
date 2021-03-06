﻿using System.Linq;
using AutoMapper;

namespace SampleApi.ApiController
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Http;

    using SampleApi.Model;

    [ExcludeFromCodeCoverage]
    [RoutePrefix("person")]
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
            return new Person
            {
                Id = Guid.Parse("96D74B0D-4456-4643-A5FD-D0A31AF0C284"),
                HouseId = Guid.Parse("46B18438-A460-4C65-BFC7-8538E7F93568"),
                DadId = Guid.Parse("E89006B3-DE81-47E2-9E1B-B202B8B107A6"),
                Dad = new Person { Id = Guid.Parse("E89006B3-DE81-47E2-9E1B-B202B8B107A6") },
                RelatedPersons = GetParents()
            };
        }

        [Route("{id}/father")]
        public Person GetFather(Guid id)
        {
            return new Person { Id = Guid.Parse("058D7A99-FCDD-4E67-8569-9B050CAD85EB") };
        }

        [Route("{id}/parents")]
        [HttpGet]
        public IEnumerable<Person> GetParents()
        {
            return new[]
            {
                new Person
                {
                    Id = Guid.Parse("0CFA46CC-116D-45F0-B57B-4C5586130072"),
                    DadId = Guid.Parse("E89006B3-DE81-47E2-9E1B-B202B8B107A6"),
                    Dad = new Person
                    {
                        Id = Guid.Parse("E89006B3-DE81-47E2-9E1B-B202B8B107A6")
                    },
                    MomId = Guid.Parse("E25F122B-8D8E-44FB-840E-57F582F7221C")
                },
                new Person
                {
                    Id = Guid.Parse("F6D0CF9E-5153-4218-8A22-8E71C5AC5A4A"),
                    DadId = Guid.Parse("E89006B3-DE81-47E2-9E1B-B202B8B107A6"),
                    Dad = new Person
                    {
                        Id = Guid.Parse("E89006B3-DE81-47E2-9E1B-B202B8B107A6")
                    }
                }
            }.ToList();
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

        [HttpPost]
        [Route("wakeup")]
        public void WakeUp(Guid id)
        {
        }

        public Person Put(UpdatePersonRequest request)
        {
            return new Person { Id = Guid.Parse("82878C8F-083F-4741-BA84-BCDE4EE51697") };
        }

        public void Delete(Guid id) { }
    }
}