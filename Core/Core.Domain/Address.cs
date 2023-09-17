namespace Core.Domain;

public class Address
{
    public int AddressId { get; set; }

    public string Street { get; set; }

    public int Number { get; set; }

    public string Extension { get; set; }

    public string City { get; set; }
}