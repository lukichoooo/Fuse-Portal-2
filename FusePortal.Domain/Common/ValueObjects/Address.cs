using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Common.ValueObjects
{
    public sealed class Address : ValueObject
    {
        public string Country { get; private set; }
        public string City { get; private set; }

        public Address(string country, string city)
        {
            Country = country;
            City = city;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Country;
            yield return City;
        }

        private Address() { }
    }
}
