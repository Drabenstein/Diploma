using Application.Common;

namespace Application.Queries.Dtos;

public class FieldOfStudyInitialTableDto<T>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int Degree { get; set; }
    public string StudyForm { get; set; }
    public string LectureLanguage { get; set; }
    public string DefenceYear { get; set; }
    public PagedResultDto<T> Data { get; set; }
}