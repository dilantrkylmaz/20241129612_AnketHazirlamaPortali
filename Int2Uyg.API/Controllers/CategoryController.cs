using AutoMapper;
using Int2Uyg.API.DTOs;
using Int2Uyg.API.Models;
using Int2Uyg.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Int2Uyg.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        ResultDto _result = new ResultDto();

        public CategoryController(CategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<List<CategoryDto>> List()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var sortedCategories = categories.OrderByDescending(c => c.Id).ToList();

            return _mapper.Map<List<CategoryDto>>(sortedCategories);
        }

        [HttpGet("{id}")]
        public async Task<CategoryDto> GetById(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return _mapper.Map<CategoryDto>(category);
        }



        [HttpPost]
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
            }

            _result.Status = true;
            _result.Message = "Kategori başarıyla güncellendi.";
            return _result;
        }



        [HttpDelete("{id}")]
        public async Task<ResultDto> Delete(int id)
        {
            await _categoryRepository.DeleteAsync(id);
            _result.Status = true;
            _result.Message = "Kategori Silindi";
            return _result;
        }
    }
}