using AutoMapper;
using BookStore.Api.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookStore.Api.Controllers
{
    public class BaseController : Controller
    {
        internal readonly ILoggerService _logger;
        internal readonly IMapper _mapper;

        public BaseController(ILoggerService logger, IMapper mapper)
        {
            _mapper = mapper;
            _logger = logger;
        }

        internal ObjectResult LogErrorAndBuildInternalError(Exception ex, string message)
        {
            _logger.LogError($"{ex.Message} - {ex.InnerException}");
            return StatusCode(500, message);

        }
    }
}
