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
            var filteredQuestions = await _questionRepository
                .Where(q => q.SurveyId == surveyId && !q.IsDeleted)
                .ToListAsync();
            return _mapper.Map<List<QuestionDto>>(filteredQuestions);
        }



        [HttpPost]
        [Authorize]
        public async Task<ResultDto> Add(QuestionDto dto)
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
            question.IsActive = dto.IsActive; 

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