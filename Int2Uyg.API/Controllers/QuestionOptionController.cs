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
    public class QuestionOptionController : ControllerBase
    {
        private readonly QuestionOptionRepository _optionRepository;
        private readonly IMapper _mapper;
        ResultDto _result = new ResultDto();

        public QuestionOptionController(QuestionOptionRepository optionRepository, IMapper mapper)
        {
            _optionRepository = optionRepository;
            _mapper = mapper;
        }

        [HttpGet("{questionId}")]
        public async Task<List<QuestionOptionDto>> GetOptionsByQuestionId(int questionId)
        {
            var filteredOptions = await _optionRepository.Where(o => o.QuestionId == questionId).ToListAsync();
            return _mapper.Map<List<QuestionOptionDto>>(filteredOptions);
        }

        [HttpPost]
        public async Task<ResultDto> Add(QuestionOptionDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.OptionText))
            {
                _result.Status = false;
                _result.Message = "Seçenek metni boş olamaz!";
                return _result;
            }

            var option = _mapper.Map<QuestionOption>(dto);


            option.Question = null;

            await _optionRepository.AddAsync(option);
            _result.Status = true;
            _result.Message = "Seçenek eklendi.";
            return _result;
        }

        [HttpPut]
        public async Task<ResultDto> Update(QuestionOptionDto dto)
        {
            var option = _mapper.Map<QuestionOption>(dto);
            option.Question = null;

            await _optionRepository.UpdateAsync(option);
            _result.Status = true;
            _result.Message = "Seçenek güncellendi.";
            return _result;
        }

        [HttpDelete("{id}")]
        public async Task<ResultDto> Delete(int id)
        {
            await _optionRepository.DeleteAsync(id);
            _result.Status = true;
            _result.Message = "Seçenek silindi.";
            return _result;
        }
    }
}