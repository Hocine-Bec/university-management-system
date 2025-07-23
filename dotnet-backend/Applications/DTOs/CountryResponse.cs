namespace Applications.DTOs
{
    public record struct CountryResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
    }
}
