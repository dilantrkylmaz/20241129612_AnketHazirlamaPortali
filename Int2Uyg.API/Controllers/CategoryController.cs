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
        ResultDto _result = new ResultDto();

        public CategoryController(CategoryRepository categoryRepository, SurveyRepository surveyRepository, IMapper mapper)
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

            var sortedCategories = categories.OrderByDescending(c => c.Id).ToList();

            var categoryDtos = _mapper.Map<List<CategoryDto>>(sortedCategories);

            foreach (var cat in categoryDtos)
            {
                cat.SurveyCount = await _surveyRepository.Where(s => s.CategoryId == cat.Id).CountAsync();
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
            var surveysQuery = _surveyRepository.Where(s => s.CategoryId == id)
                                                .Include(i => i.Category)
                                                .Include(i => i.User);

            var surveys = await surveysQuery.ToListAsync();
            var surveyDtos = _mapper.Map<List<SurveyDto>>(surveys);

            // ✅ BUG #5 FIX: Removed incorrect filter that was hiding other users' surveys.
            // All authenticated users can now see all surveys in a category.
            return surveyDtos;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ResultDto> Add(CategoryDto dto)
        {
            var categories = await _categoryRepository.GetAllAsync();

            if (categories.Any(c => c.Name != null && c.Name.ToLower() == dto.Name.ToLower()))
            {
                _result.Status = false;
                _result.Message = "Bu isimde bir kategori zaten kayıtlıdır!";
                return _result;
            }

            var category = _mapper.Map<Category>(dto);
            await _categoryRepository.AddAsync(category);

            _result.Status = true;
            _result.Message = "Kategori başarıyla eklendi.";
            return _result;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ResultDto> Update(CategoryDto dto)
        {
            var categories = await _categoryRepository.GetAllAsync();

            if (categories.Any(c => c.Name != null && c.Name.ToLower() == dto.Name.ToLower() && c.Id != dto.Id))
            {
                _result.Status = false;
                _result.Message = "Bu isimde başka bir kategori zaten mevcut!";
                return _result;
            }

            var orjinalKategori = categories.FirstOrDefault(c => c.Id == dto.Id);

            if (orjinalKategori != null)
            {
                orjinalKategori.Name = dto.Name;
                orjinalKategori.Description = dto.Description;
                orjinalKategori.IsActive = dto.IsActive;

                await _categoryRepository.UpdateAsync(orjinalKategori);
                _result.Status = true;
                _result.Message = "Kategori başarıyla güncellendi.";
            }
            else
            {
                _result.Status = false;
                _result.Message = "Kategori bulunamadı!";
            }

            return _result;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ResultDto> Delete(int id)
        {
            var hasSurveys = await _surveyRepository.Where(s => s.CategoryId == id).AnyAsync();

            if (hasSurveys)
            {
                _result.Status = false;
                _result.Message = "Üzerinde Anket Kaydı Olan Kategori Silinemez! Lütfen önce içindeki anketleri silin.";
                return _result;
            }

            await _categoryRepository.DeleteAsync(id);
            _result.Status = true;
            _result.Message = "Kategori Silindi";
            return _result;
        }
    }
}