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
    public class BooksController : BaseController
    {
        private readonly IBookRepository _bookRepository;

        public BooksController(ILoggerService logger, IMapper mapper, IBookRepository bookRepository)
            : base(logger, mapper)
        {
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
                return LogErrorAndBuildInternalError(ex, "Somethig went wrong getting books. Please contact the Administrator");
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
                if (book == null)
                {
                    _logger.LogWarn($"Book with id [{id}] wasn't found");
                    return NotFound();
                }
                var response = _mapper.Map<BookDto>(book);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return LogErrorAndBuildInternalError(ex, $"Somethig went wrong getting book with id {id}. Please contact the Administrator");
            }
        }
    }

}
