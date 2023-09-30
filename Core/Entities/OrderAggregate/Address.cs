using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderAggregate
{
    public class Address
    {
        public Address() { }

        public Address(string firstName, string lastName, string street, string state, string city, string zibCode)
        {
            FirstName = firstName;
            LastName = lastName;
            Street = street;
            State = state;
            City = city;
            ZibCode = zibCode;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string ZibCode { get; set; }
    }
}
