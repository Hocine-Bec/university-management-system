namespace Applications.DTOs.People
{
    public record struct PersonRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? ImagePath { get; set; }
        public int? CountryId { get; set; }
    }
}
