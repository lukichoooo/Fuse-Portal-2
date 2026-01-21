using FusePortal.Domain.SeedWork;

namespace FusePortal.Domain.Common.ValueObjects.Address
{
    public sealed class Address : ValueObject
    {
        public string Country { get; }
        public string City { get; }

        public Address(string country, string city)
        {
            Country = country ?? throw new AddressDomainException($"field cannot be null or empty {nameof(country)}");
            City = city ?? throw new AddressDomainException($"field cannot be null or empty {nameof(city)}");
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Country;
            yield return City;
        }

        private Address() { }
    }
}
