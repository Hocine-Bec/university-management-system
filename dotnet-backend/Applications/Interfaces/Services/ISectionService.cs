using Applications.DTOs.Section;
using Applications.Shared;

namespace Applications.Interfaces.Services;

public interface ISectionService
{
    Task<Result<IReadOnlyCollection<SectionResponse>>> GetListAsync();
    Task<Result<SectionResponse>> GetByIdAsync(int id);
    Task<Result<SectionResponse>> GetBySectionNumberAsync(string sectionNumber);
    Task<Result<SectionResponse>> AddAsync(SectionRequest request);
    Task<Result> UpdateAsync(int id, SectionRequest request);
    Task<Result> DeleteAsync(int id);
    Task<Result> DeleteAsync(string sectionNumber);
}