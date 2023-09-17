using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain;

public class Opponent
{
    public int Id { get; set; }

    public string Name { get; set; }

    public Address PlayingAddress { get; set; }

    [ForeignKey("AddressId")]
    public int? PlayingAddressId { get; set; }
}