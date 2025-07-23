namespace Applications.DTOs.ServiceApplication;

public record struct ServiceApplicationResponse
{
    public int Id { get; set; }
    public DateTime ApplicationDate { get; set; }
    public string Status { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string PersonFullName { get; set; }
    public string ServiceOfferName { get; set; }
}