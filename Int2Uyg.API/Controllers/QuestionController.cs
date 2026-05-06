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
        ResultDto _result = new ResultDto();

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
            var filteredQuestions = await _questionRepository.Where(q => q.SurveyId == surveyId).ToListAsync();
            return _mapper.Map<List<QuestionDto>>(filteredQuestions);
        }

        [HttpPost]
        [Authorize]
        public async Task<ResultDto> Add(QuestionDto dto)
        {
            var question = _mapper.Map<Question>(dto);

            question.Survey = null;

            await _questionRepository.AddAsync(question);
            _result.Status = true;
            _result.Message = "Soru Başarıyla Eklendi";
            return _result;
        }

        [HttpPut]
        [Authorize]
        public async Task<ResultDto> Update(QuestionDto dto)
        {
            var question = _mapper.Map<Question>(dto);

            question.Survey = null;

            await _questionRepository.UpdateAsync(question);
            _result.Status = true;
            _result.Message = "Soru Güncellendi";
            return _result;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ResultDto> Delete(int id)
        {
            await _questionRepository.DeleteAsync(id);
            _result.Status = true;
            _result.Message = "Soru Silindi";
            return _result;
        }
    }
}