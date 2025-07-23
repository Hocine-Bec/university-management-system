namespace Applications.DTOs.Program;

public record struct ProgramResponse
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int MinimumAge { get; set; }
    public int Duration { get; set; }
    public decimal Fees { get; set; }
    public bool IsActive { get; set; } 
}