using Domain.Enums;

namespace Applications.DTOs.ServiceApplication;

public record struct ServiceApplicationUpdateStatusRequest
{
    public ApplicationStatus? Status { get; set; }
}