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
    public class QuestionController : ControllerBase
    {
        private readonly QuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public QuestionController(QuestionRepository questionRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<List<QuestionDto>> List()
        {
            var questions = await _questionRepository.GetAllAsync();
            return _mapper.Map<List<QuestionDto>>(questions);
        }

        [HttpGet("{surveyId}")]
        public async Task<List<QuestionDto>> GetQuestionsBySurveyId(int surveyId)
        {
            // ✅ Soft‑delete kontrolü eklenmiş hâli
            var filteredQuestions = await _questionRepository
                .Where(q => q.SurveyId == surveyId && !q.IsDeleted)
                .ToListAsync();
            return _mapper.Map<List<QuestionDto>>(filteredQuestions);
        }



        [HttpPost]
        [Authorize]
        public async Task<ResultDto> Add(QuestionDto dto)
        {
            // ✅ Her istek için yeni ResultDto (thread‑safe)
            var result = new ResultDto();

            if (string.IsNullOrWhiteSpace(dto.Text))
            {
                result.Status = false;
                result.Message = "Soru metni boş olamaz!";
                return result;
            }

            var question = _mapper.Map<Question>(dto);

            // ✅ Survey navigasyonunu null'la (tracking sorunları engellenir)
            question.Survey = null;

            // ✅ QuestionOptions listesini garantile (AutoMapper null yapmış olabilir)
            question.QuestionOptions ??= new List<QuestionOption>();

            // ✅ Eklenen sorunun varsayılan olarak aktif olmasını sağla
            question.IsActive = true;

            await _questionRepository.AddAsync(question);

            result.Status = true;
            result.Message = "Soru Başarıyla Eklendi";
            return result;
        }

        [HttpPut]
        [Authorize]
        public async Task<ResultDto> Update(QuestionDto dto)
        {
            var result = new ResultDto();

            if (string.IsNullOrWhiteSpace(dto.Text))
            {
                result.Status = false;
                result.Message = "Soru metni boş olamaz!";
                return result;
            }

            var question = _mapper.Map<Question>(dto);
            question.Survey = null;
            question.QuestionOptions ??= new List<QuestionOption>();
            question.IsActive = dto.IsActive; // Güncellemede gönderilen değeri kullan

            await _questionRepository.UpdateAsync(question);

            result.Status = true;
            result.Message = "Soru Güncellendi";
            return result;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ResultDto> Delete(int id)
        {
            var result = new ResultDto();

            await _questionRepository.DeleteAsync(id);

            result.Status = true;
            result.Message = "Soru Silindi";
            return result;
        }
    }
}