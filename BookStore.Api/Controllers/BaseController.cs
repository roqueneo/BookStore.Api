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

        internal string GetControllerAndActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;
            return $"{controller} » {action}"; 
        }

        internal ObjectResult LogErrorAndBuildInternalError(Exception ex, string message)
        {
            _logger.LogError($"{GetControllerAndActionNames()}: {ex.Message} - {ex.InnerException}");
            return StatusCode(500, message);
        }
    }
}
