using Microsoft.EntityFrameworkCore;

namespace Store.Domain.ValueObjects;

[Owned]
public class Address : ValueObject
{
    private Address() { }

    public Address(
        string street,
        string city,
        string state,
        string zipCode,
        string country,
        string googleMapsLocation)
    {
        Street = street;
        City = city;
        State = state;
        ZipCode = zipCode;
        Country = country;
        GoogleMapsLocation = googleMapsLocation;
    }

    public string Street { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }
    public string Country { get; private set; }
    public string GoogleMapsLocation { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return City;
        yield return State;
        yield return ZipCode;
        yield return Country;
        yield return GoogleMapsLocation;
    }
}
