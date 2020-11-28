using AutoMapper;
using BookStore.Api.Contracts;
using BookStore.Api.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Api.Controllers
{
    /// <summary>
    /// Endpoint used to interact with the Authors in the book store's database.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : Controller
    {
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(ILoggerService logger, IMapper mapper, IAuthorRepository authorRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _authorRepository = authorRepository;
        }

        /// <summary>
        /// Get all Authors from database
        /// </summary>
        /// <returns>List of authors</returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var allAuthors = await _authorRepository.FindAll();
                var response = _mapper.Map<IList<AuthorDto>>(allAuthors);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {ex.InnerException}");
                return StatusCode(500, "Somethig went wrong. Please contact the Administrator");
            }
        }
    }
}
