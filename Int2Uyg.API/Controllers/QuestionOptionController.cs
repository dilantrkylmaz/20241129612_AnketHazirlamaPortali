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

        public QuestionOptionController(QuestionOptionRepository optionRepository, IMapper mapper)
        {
            _optionRepository = optionRepository;
            _mapper = mapper;
        }

        [HttpGet("{questionId}")]
        public async Task<List<QuestionOptionDto>> GetOptionsByQuestionId(int questionId)
        {
            var filteredOptions = await _optionRepository
                .Where(o => o.QuestionId == questionId && o.IsActive && !o.IsDeleted)
                .ToListAsync();
            return _mapper.Map<List<QuestionOptionDto>>(filteredOptions);
        }

        [HttpPost]
        public async Task<ResultDto> Add(QuestionOptionDto dto)
        {
            var result = new ResultDto(); 

            if (string.IsNullOrWhiteSpace(dto.OptionText))
            {
                result.Status = false;
                result.Message = "Seçenek metni boş olamaz!";
                return result;
            }

            var option = _mapper.Map<QuestionOption>(dto);
            option.Question = null;

            await _optionRepository.AddAsync(option);

            result.Status = true;
            result.Message = "Seçenek eklendi.";
            return result;
        }

        [HttpPut]
        public async Task<ResultDto> Update(QuestionOptionDto dto)
        {
            var result = new ResultDto(); 
            var option = _mapper.Map<QuestionOption>(dto);
            option.Question = null;

            await _optionRepository.UpdateAsync(option);

            result.Status = true;
            result.Message = "Seçenek güncellendi.";
            return result;
        }

        [HttpDelete("{id}")]
        public async Task<ResultDto> Delete(int id)
        {
            var result = new ResultDto(); 

            var option = await _optionRepository.GetByIdAsync(id);
            if (option == null)
            {
                result.Status = false;
                result.Message = "Silinmek istenen seçenek bulunamadı.";
                return result;
            }

            option.IsDeleted = true;
            option.IsActive = false;

            await _optionRepository.UpdateAsync(option);

            result.Status = true;
            result.Message = "Seçenek başarıyla silindi.";
            return result;
        }
    }
}