namespace Int2Uyg.API.DTOs
{
    public class UserAnswerDto
    {
        public int Id { get; set; }
        public string SurveyTitle { get; set; }
        public string QuestionText { get; set; }
        public string? SelectedOptionText { get; set; }
        public string? TextAnswer { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}