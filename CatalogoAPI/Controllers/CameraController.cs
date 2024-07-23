﻿using Catalogo.Core.DTOs;
using Catalogo.Core.Entities;
using Catalogo.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CameraController : ControllerBase
    {
        private readonly ICameraRepository _cameraRepository;

        public CameraController(ICameraRepository cameraRepository)
        {
            _cameraRepository = cameraRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var cameras = await _cameraRepository.GetAllAsync();
            return Ok(cameras);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var camera = await _cameraRepository.GetByIdAsync(id);
            if (camera == null)
                return NotFound();

            return Ok(camera);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddAsync(CameraDto cameraDto)
        {
            // Gerar um novo ID único
            int novoId = await _cameraRepository.GetNextIdAsync();

            var camera = new Camera
            {
                Id = novoId,
                Marca = cameraDto.Marca,
                Modelo = cameraDto.Modelo,
                Preco = cameraDto.Preco,
                Descricao = cameraDto.Descricao
            };

            await _cameraRepository.AddAsync(camera);

            return Ok(camera);
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> UpdateAsync(int id, CameraDto cameraDto)
        {
            var existingCamera = await _cameraRepository.GetByIdAsync(id);
            if (existingCamera == null)
                return NotFound();

            existingCamera.Marca = cameraDto.Marca;
            existingCamera.Modelo = cameraDto.Modelo;
            existingCamera.Preco = cameraDto.Preco;
            existingCamera.Descricao = cameraDto.Descricao;

            await _cameraRepository.UpdateAsync(existingCamera);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var existingCamera = await _cameraRepository.GetByIdAsync(id);
            if (existingCamera == null)
                return NotFound();

            await _cameraRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
