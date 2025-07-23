namespace Applications.DTOs.People
{
    public record struct PersonResponse
    {
        public int Id { get; set; }
        public required string FullName { get; set; }
        public DateTime DOB { get; set; }
        public string? Email { get; set; }
        public string? CountryName { get; set; }
    }
}
