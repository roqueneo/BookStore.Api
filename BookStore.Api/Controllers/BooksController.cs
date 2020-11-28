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
    /// Controller used to interact with the Books in the book store's database.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class BooksController : Controller
    {
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        private readonly IBookRepository _bookRepository;

        public BooksController(ILoggerService logger, IMapper mapper, IBookRepository bookRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _bookRepository = bookRepository;
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
                var allAuthors = await _bookRepository.FindAll();
                var response = _mapper.Map<IList<BookDto>>(allAuthors);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {ex.InnerException}");
                return StatusCode(500, "Somethig went wrong getting books. Please contact the Administrator");
            }
        }

        /// <summary>
        /// Get Book with given id from database
        /// </summary>
        /// <param name="id">Book's id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var book = await _bookRepository.FindById(id);
                var response = _mapper.Map<BookDto>(book);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {ex.InnerException}");
                return StatusCode(500, $"Somethig went wrong getting book with id {id}. Please contact the Administrator");
            }
        }
    }

}
