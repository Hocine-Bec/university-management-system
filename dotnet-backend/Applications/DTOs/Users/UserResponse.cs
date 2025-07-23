namespace Applications.DTOs.Users
{
    public record struct UserResponse
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public int PersonId { get; set; }
    }
}
