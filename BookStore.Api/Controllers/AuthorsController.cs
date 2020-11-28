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
    /// Controller used to interact with the Authors in the book store's database.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorsController : BaseController
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorsController(ILoggerService logger, IMapper mapper, IAuthorRepository authorRepository)
            : base(logger, mapper)
        {
            _authorRepository = authorRepository;
        }

        /// <summary>
        /// Get all Authors from database
        /// </summary>
        /// <returns>List of authors</returns>
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                return LogErrorAndBuildInternalError(ex, "Somethig went wrong getting authors. Please contact the Administrator");
            }
        }

        /// <summary>
        /// Get Author with given id from database
        /// </summary>
        /// <param name="id">Author's id</param>
        /// <returns>An author's record</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var author = await _authorRepository.FindById(id);
                if (author == null)
                {
                    _logger.LogWarn($"Author with id [{id}] wasn't found");
                    return NotFound();
                }
                var response = _mapper.Map<AuthorDto>(author);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return LogErrorAndBuildInternalError(ex, $"Somethig went wrong getting author with id {id}. Please contact the Administrator");
            }
        }

        /// <summary>
        /// Creates a new Author on database
        /// </summary>
        /// <param name="authorDto">Author information</param>
        /// <returns>Created author</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDto authorDto)
        {
            try
            {
                if (authorDto == null)
                {
                    _logger.LogWarn("Empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("Author data is incomplete");
                    return BadRequest(ModelState);
                }
                var author = _mapper.Map<Author>(authorDto);
                var operationSuccess = await _authorRepository.Create(author);
                if (!operationSuccess)
                {
                    _logger.LogError($"Author creation failed");
                    return StatusCode(500, "Author creation failed");
                }
                return Created("Created", author);
            }
            catch (Exception ex)
            {
                return LogErrorAndBuildInternalError(ex, $"Somethig went wrong creating a new author. Please contact the Administrator");
            }
        }

        /// <summary>
        /// Updates an existing Author on database
        /// </summary>
        /// <param name="id">Author's id</param>
        /// <param name="authorDto">Author's information</param>
        /// <returns>Nothing</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] AuthorCreateDto authorDto)
        {
            try
            {
                if (id < 0 || authorDto == null)
                {
                    _logger.LogWarn("Invalid id or empty request was submitted");
                    return BadRequest(ModelState);
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogWarn("Author data is invalid");
                    return BadRequest(ModelState);
                }
                var author = _mapper.Map<Author>(authorDto);
                author.Id = id;
                var operationSuccess = await _authorRepository.Update(author);
                if (!operationSuccess)
                {
                    _logger.LogError($"Author update operation failed");
                    return StatusCode(500, "Author update operation failed");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return LogErrorAndBuildInternalError(ex, $"Somethig went wrong updating author with id [{id}]. Please contact the Administrator");
            }
        }

        /// <summary>
        /// Delete an existent Author in database
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
                var author = await _authorRepository.FindById(id);
                if (author == null)
                {
                    _logger.LogWarn("Author with id [{id}] wasn't found.");
                    return NotFound();
                }
                var operationSuccess = await _authorRepository.Delete(author);
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
