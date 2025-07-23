namespace Applications.DTOs.ServiceOffer;

public record struct ServiceOfferResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Fees { get; set; }
    public bool IsActive { get; set; }
}