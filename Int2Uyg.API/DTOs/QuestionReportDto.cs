namespace Int2Uyg.API.DTOs
{
    public class OptionReportDto
    {
        public int OptionId { get; set; }
        public string OptionText { get; set; }
        public int Count { get; set; }
    }

    public class QuestionReportDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public List<OptionReportDto> Options { get; set; } = new List<OptionReportDto>();
    }

    public class SurveyReportDto
    {
        public int SurveyId { get; set; }
        public string SurveyTitle { get; set; }
        public List<QuestionReportDto> Questions { get; set; } = new List<QuestionReportDto>();
    }
}