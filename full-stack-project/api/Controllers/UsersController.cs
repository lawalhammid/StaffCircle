using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.ApiResponses;
using api.DTOs;
using AutoMapper;
using BusinessLogic.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models.Entities;

namespace api.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [EnableCors("CorsPolicy")]
    public class UsersController : ControllerBase
    {
        private readonly IUsers _users;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IUsers users, IMapper mapper, ILogger<UsersController> logger)
        {
            this._users = users;
            this._mapper = mapper;
            this._logger = logger;
        }

       
        [HttpPost]
        public async Task<IActionResult> CreateUsers(CreateUsersDTO createUsersDTO)
        {
            try
            {
                if (createUsersDTO == null) return BadRequest(ApiInvalidCallsMessage.InvalidRequestMessages(1));

                var userEntity = _mapper.Map<Users>(createUsersDTO);


                var result = await _users.AddUser(userEntity, createUsersDTO.Password);

                return result.IsSuccessful ? StatusCode((int)ApiResponseCodes.Created, result) : StatusCode((int)ApiResponseCodes.NotCreated, result);
            }
            catch (Exception ex)
            {
                this._logger.LogTrace(ex.StackTrace);
                return StatusCode((int)ApiResponseCodes.ServerError, ApiInvalidCallsMessage.ServerErrorMessage(1));
            }
        }
    }
}
