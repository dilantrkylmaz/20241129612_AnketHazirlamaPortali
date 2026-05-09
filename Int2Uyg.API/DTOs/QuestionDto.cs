namespace Int2Uyg.API.DTOs
{
    public class QuestionDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int SurveyId { get; set; }
        public bool IsActive { get; set; }
        public List<QuestionOptionDto>? QuestionOptions { get; set; } // ? ekledik
    }
}