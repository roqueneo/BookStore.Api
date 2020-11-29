using AutoMapper;
using BookStore.Api.Contracts;
using BookStore.Api.Data;
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
        /// Get all Books from database
        /// </summary>
        /// <returns>List of authors</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var allBooks = await _bookRepository.FindAll();
                var response = _mapper.Map<IList<BookDto>>(allBooks);
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
        /// <returns>A books's record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Creates a new Book on database
        /// </summary>
        /// <param name="bookDto">Book's data</param>
        /// <returns>Created book</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] BookCreateDto bookDto)
        {
            try
            {
                if (bookDto == null)
                {
                    _logger.LogWarn("Empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("Book data is incomplete");
                    return BadRequest(ModelState);
                }
                var book = _mapper.Map<Book>(bookDto);
                var operationSuccess = await _bookRepository.Create(book);
                if (!operationSuccess)
                {
                    _logger.LogError("Book creation failed");
                    return StatusCode(500, "Book creation failed");
                }
                return Created("Created", book);
            }
            catch (Exception ex)
            {
                return LogErrorAndBuildInternalError(ex, $"Somethig went wrong creating a new book. Please contact the Administrator");
            }
        }

        /// <summary>
        /// Updates an existing Book on database
        /// </summary>
        /// <param name="id">Book's id</param>
        /// <param name="bookDto">Book's information</param>
        /// <returns>Nothing</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] BookCreateDto bookDto)
        {
            try
            {
                if (id < 0 || bookDto == null)
                {
                    _logger.LogWarn("Invalid id or empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("Book data is invalid");
                    return BadRequest(ModelState);
                }
                var authorExists = await _bookRepository.Exists(id);
                if (!authorExists)
                {
                    _logger.LogWarn("Book with id [{id}] wasn't found.");
                    return NotFound();
                }
                var author = _mapper.Map<Book>(bookDto);
                author.Id = id;
                var operationSuccess = await _bookRepository.Update(author);
                if (!operationSuccess)
                {
                    _logger.LogError($"Book update operation failed");
                    return StatusCode(500, "Book update operation failed");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return LogErrorAndBuildInternalError(ex, $"Somethig went wrong updating book with id [{id}]. Please contact the Administrator");
            }
        }

        /// <summary>
        /// Delete an existent Book in database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Nothing</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id < 0)
                {
                    _logger.LogWarn("Invalid id [{id}] attempted.");
                    return BadRequest();
                }
                var author = await _bookRepository.FindById(id);
                if (author == null)
                {
                    _logger.LogWarn("Author with id [{id}] wasn't found.");
                    return NotFound();
                }
                var operationSuccess = await _bookRepository.Delete(author);
                if (!operationSuccess)
                {
                    _logger.LogError($"Author delete operation failed");
                    return StatusCode(500, "Author delete operation failed");
                }
                return NoContent();

            }
            catch (Exception ex)
            {
                return LogErrorAndBuildInternalError(ex, $"Somethig went wrong deleting author with id [{id}]. Please contact the Administrator");
            }
        }

    }
}
