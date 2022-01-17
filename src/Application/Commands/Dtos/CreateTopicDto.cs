namespace Application.Commands.Dtos
{
    public class CreateTopicDto
    {   
        public long FieldOfStudyId { get; set; }
        public int YearOfDefence { get; set; }
        public int MaxNoRealizations { get; set; }
        public string PolishName { get; set; }
        public string EnglishName { get; set; }


    }
}
