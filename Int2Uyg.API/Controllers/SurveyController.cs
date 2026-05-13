using AutoMapper;
using Int2Uyg.API.DTOs;
using Int2Uyg.API.Models;
using Int2Uyg.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Int2Uyg.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class SurveyController : ControllerBase
    {
        private readonly SurveyRepository _surveyRepository;
        private readonly QuestionRepository _questionRepository;
        private readonly AnswerRepository _answerRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        ResultDto _result = new ResultDto();

        public SurveyController(
            SurveyRepository surveyRepository,
            QuestionRepository questionRepository,
            AnswerRepository answerRepository,
            IMapper mapper,
            IWebHostEnvironment env)
        {
            _surveyRepository = surveyRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet]
        public async Task<List<SurveyDto>> List()
        {
            var surveys = await _surveyRepository.GetAllAsync();
            return _mapper.Map<List<SurveyDto>>(surveys);
        }

        [HttpGet("{id}")]
        public async Task<SurveyDto> GetById(int id)
        {
            var survey = await _surveyRepository.GetByIdAsync(id);
            return _mapper.Map<SurveyDto>(survey);
        }

        [HttpPost]
        [Authorize]
        public async Task<ResultDto> Add(SurveyDto dto)
        {
            var survey = _mapper.Map<Survey>(dto);

            if (!string.IsNullOrEmpty(dto.PhotoUrl) && dto.PhotoUrl.Contains("base64"))
            {
                survey.PhotoUrl = SaveImage(dto.PhotoUrl) ?? string.Empty;
            }
            else
            {
                survey.PhotoUrl = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(survey.Description))
            {
                survey.Description = "Açıklama belirtilmedi.";
            }

            survey.Category = null;
            await _surveyRepository.AddAsync(survey);
            _result.Status = true;
            _result.Message = "Anket Eklendi";
            return _result;
        }

        [HttpPut]
        [Authorize]
        public async Task<ResultDto> Update(SurveyDto dto)
        {
            var existSurvey = await _surveyRepository.Where(s => s.Id == dto.Id).AsNoTracking().FirstOrDefaultAsync();

            if (existSurvey == null)
            {
                _result.Status = false;
                _result.Message = "Anket bulunamadı";
                return _result;
            }

            var survey = _mapper.Map<Survey>(dto);

            if (dto.PhotoUrl == "DELETE")
            {
                survey.PhotoUrl = string.Empty;
            }
            else if (!string.IsNullOrEmpty(dto.PhotoUrl) && dto.PhotoUrl.Contains("base64"))
            {
                survey.PhotoUrl = SaveImage(dto.PhotoUrl) ?? string.Empty;
            }
            else
            {
                survey.PhotoUrl = existSurvey.PhotoUrl ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(survey.Description))
            {
                survey.Description = "Açıklama belirtilmedi.";
            }

            survey.Category = null;
            await _surveyRepository.UpdateAsync(survey);

            _result.Status = true;
            _result.Message = "Anket Güncellendi";
            return _result;
        }

        private string? SaveImage(string base64String)
        {
            try
            {
                string folder = Path.Combine(
                    _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"),
                    "Files", "Surveys");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string fullPath = Path.Combine(folder, fileName);

                var base64Data = base64String.Substring(base64String.IndexOf(",") + 1);
                byte[] imageBytes = Convert.FromBase64String(base64Data);
                System.IO.File.WriteAllBytes(fullPath, imageBytes);

                return fileName;
            }
            catch
            {
                return null;
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ResultDto> Delete(int id)
        {
            await _surveyRepository.DeleteAsync(id);
            _result.Status = true;
            _result.Message = "Anket Silindi";
            return _result;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        [ActionName("Report")]
        public async Task<ActionResult<SurveyReportDto>> GetReport(int id)
        {
            var survey = await _surveyRepository.GetByIdAsync(id);
            if (survey == null)
                return NotFound("Anket bulunamadı.");

            var questions = await _questionRepository
                .Where(q => q.SurveyId == id && !q.IsDeleted)
                .Include(q => q.QuestionOptions.Where(o => !o.IsDeleted))
                .ToListAsync();

            var report = new SurveyReportDto
            {
                SurveyId = id,
                SurveyTitle = survey.Title
            };

            foreach (var question in questions)
            {
                var qDto = new QuestionReportDto
                {
                    QuestionId = question.Id,
                    QuestionText = question.Text
                };

                var answers = await _answerRepository
                    .Where(a => a.QuestionId == question.Id && a.SurveyId == id && !a.IsDeleted)
                    .ToListAsync();

                foreach (var option in question.QuestionOptions)
                {
                    var count = answers.Count(a => a.SelectedOptionId == option.Id);
                    qDto.Options.Add(new OptionReportDto
                    {
                        OptionId = option.Id,
                        OptionText = option.OptionText,
                        Count = count
                    });
                }

                report.Questions.Add(qDto);
            }

            return Ok(report);
        }
    }
}