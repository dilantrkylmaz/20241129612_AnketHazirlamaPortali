using System.Security.Claims;
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
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly SurveyRepository _surveyRepository;
        private readonly IMapper _mapper;

        public CategoryController(
            CategoryRepository categoryRepository,
            SurveyRepository surveyRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _surveyRepository = surveyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<List<CategoryDto>> List()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var filtered = categories.Where(c => c.IsActive && !c.IsDeleted)
                                     .OrderByDescending(c => c.Id);
            var categoryDtos = _mapper.Map<List<CategoryDto>>(filtered);

            foreach (var cat in categoryDtos)
            {
                cat.SurveyCount = await _surveyRepository
                    .Where(s => s.CategoryId == cat.Id && !s.IsDeleted)
                    .CountAsync();
            }

            return categoryDtos;
        }

        [HttpGet("{id}")]
        public async Task<CategoryDto> GetById(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return _mapper.Map<CategoryDto>(category);
        }

        [HttpGet("{id}/Surveys")]
        public async Task<List<SurveyDto>> SurveyList(int id)
        {
            var surveysQuery = _surveyRepository
                .Where(s => s.CategoryId == id && s.IsActive && !s.IsDeleted) 
                .Include(i => i.Category)
                .Include(i => i.User);

            var surveys = await surveysQuery.ToListAsync();
            var surveyDtos = _mapper.Map<List<SurveyDto>>(surveys);

            return surveyDtos;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ResultDto> Add(CategoryDto dto)
        {
            var result = new ResultDto();

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                result.Status = false;
                result.Message = "Kategori adı boş olamaz!";
                return result;
            }

            var categories = await _categoryRepository.GetAllAsync();

            if (categories.Any(c =>
                !c.IsDeleted && 
                c.Name != null &&
                c.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                result.Status = false;
                result.Message = "Bu isimde bir kategori zaten kayıtlıdır!";
                return result;
            }

            var category = _mapper.Map<Category>(dto);

            category.Id = 0;

            await _categoryRepository.AddAsync(category);

            result.Status = true;
            result.Message = "Kategori başarıyla eklendi.";
            return result;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ResultDto> Update(CategoryDto dto)
        {
            var result = new ResultDto();

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                result.Status = false;
                result.Message = "Kategori adı boş olamaz!";
                return result;
            }

            var categories = await _categoryRepository.GetAllAsync();

            if (categories.Any(c =>
                !c.IsDeleted && 
                c.Id != dto.Id &&
                c.Name != null &&
                c.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase)))
            {
                result.Status = false;
                result.Message = "Bu isimde başka bir kategori zaten mevcut!";
                return result;
            }

            var orjinalKategori = categories.FirstOrDefault(c => c.Id == dto.Id);
            if (orjinalKategori == null)
            {
                result.Status = false;
                result.Message = "Kategori bulunamadı!";
                return result;
            }

            orjinalKategori.Name = dto.Name;
            orjinalKategori.Description = dto.Description;
            orjinalKategori.IsActive = dto.IsActive;

            await _categoryRepository.UpdateAsync(orjinalKategori);
            result.Status = true;
            result.Message = "Kategori başarıyla güncellendi.";
            return result;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ResultDto> Delete(int id)
        {
            var result = new ResultDto();

            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                result.Status = false;
                result.Message = "Silinmek istenen kategori bulunamadı.";
                return result;
            }

            category.IsDeleted = true;
            category.IsActive = false;

            await _categoryRepository.UpdateAsync(category);

            result.Status = true;
            result.Message = "Kategori başarıyla silindi (Pasife alındı).";
            return result;
        }
    }
}