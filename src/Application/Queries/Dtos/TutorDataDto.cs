namespace Application.Queries.Dtos;

public  class TutorDataDto
{
    public string Name { get; set; }
    public string Department { get; set; }
    public string Position { get; set; }
    public bool HasConsentToExtendPensum { get; set; }
    public long[] AreasOfInterestIds { get; set; }
}
