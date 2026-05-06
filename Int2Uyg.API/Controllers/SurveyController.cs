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
        private readonly IMapper _mapper;
        ResultDto _result = new ResultDto();

        public SurveyController(SurveyRepository surveyRepository, IMapper mapper)
        {
            _surveyRepository = surveyRepository;
            _mapper = mapper;
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
            bool isExist = await _surveyRepository.Where(s => s.Title == dto.Title).AnyAsync();

            if (isExist)
            {
                _result.Status = false;
                _result.Message = "Bu başlıkta bir anket zaten var!";
                return _result;
            }

            var survey = _mapper.Map<Survey>(dto);

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
            var survey = _mapper.Map<Survey>(dto);

            survey.Category = null;

            await _surveyRepository.UpdateAsync(survey);
            _result.Status = true;
            _result.Message = "Anket Güncellendi";
            return _result;
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
    }
}