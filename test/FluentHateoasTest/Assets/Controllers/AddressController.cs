using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Http;
using FluentHateoasTest.Assets.Model;

namespace FluentHateoasTest.Assets.Controllers
{
    public class AddressController : ApiController
    {
        public IEnumerable<Address> Get()
        {
            return new List<Address>();
        }

        public Address GetById(Guid id)
        {
            return new Address();
        }

        public Address Post(CreateAddressRequest request)
        {
            return new Address();
        }

        public Address Put(UpdateAddressRequest request)
        {
            return new Address();
        }

        public void Delete() { }
    }
}