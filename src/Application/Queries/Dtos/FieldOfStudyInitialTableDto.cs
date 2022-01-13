using Application.Common;

namespace Application.Queries.Dtos;

public class FieldOfStudyInitialTableDto<T>
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public int Degree { get; set; }
    public string StudyForm { get; set; } = null!;
    public string LectureLanguage { get; set; } = null!;
    public string DefenceYear { get; set; } = null!;
    public PagedResultDto<T> Data { get; set; } = null!;
}