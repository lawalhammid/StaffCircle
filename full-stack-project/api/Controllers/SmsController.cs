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
    public class SmsController : ControllerBase
    {
        private readonly IMessages _iMessages;
        private readonly IMapper _mapper;
        private readonly ILogger<UsersController> _logger;
        private readonly ISendSMSTwilo _iSendSMSTwilo;  
        public SmsController(IMessages iMessages, IMapper mapper, ILogger<UsersController> logger, ISendSMSTwilo iSendSMSTwilo)
        {
            this._iMessages = iMessages;
            this._mapper = mapper;
            this._logger = logger;
            this._iSendSMSTwilo = iSendSMSTwilo;
        }

        [HttpPost("ComposeMessage")]
        public async Task<IActionResult> ComposeMessage(CreateComposedMessagesDTO createComposedMessagesDTO)
        {
            try
            {
                if (createComposedMessagesDTO == null) return BadRequest(ApiInvalidCallsMessage.InvalidRequestMessages(1));

                var messages = _mapper.Map<ComposedMessages>(createComposedMessagesDTO);


                var result = await _iMessages.AddNewMessage(messages);

                return result.IsSuccessful ? StatusCode((int)ApiResponseCodes.Created, result) : StatusCode((int)ApiResponseCodes.NotCreated, result);
            }
            catch (Exception ex)
            {
                this._logger.LogTrace(ex.StackTrace);
                return StatusCode((int)ApiResponseCodes.ServerError, ApiInvalidCallsMessage.ServerErrorMessage(1));
            }
        }

        [HttpPost("SendSMS")]
        public async Task<IActionResult> SendSMS(SendMsgDTO SendMsgDTO)
        {
            try
            {
                if (SendMsgDTO == null) return BadRequest(ApiInvalidCallsMessage.InvalidRequestMessages(1));

                var sendMessages = _mapper.Map<SendMessages>(SendMsgDTO.SendMessagesDTO);
                var composedMessages = _mapper.Map<ComposedMessages>(SendMsgDTO.ComposedMessagesDTO);
                

                var result = await _iSendSMSTwilo.SendMessage(sendMessages, composedMessages, SendMsgDTO.NewMessage);

                return result.IsSuccessful ? StatusCode((int)ApiResponseCodes.Created, result) : StatusCode((int)ApiResponseCodes.NotCreated, result);
            }
            catch (Exception ex)
            {
                this._logger.LogTrace(ex.StackTrace);
                return StatusCode((int)ApiResponseCodes.ServerError, ApiInvalidCallsMessage.ServerErrorMessage(1));
            }
        }

        [HttpGet("GetMessagebyUser")]
        public async Task<IActionResult> GetMessagebyUser(long UserId)
        {
            var result = await _iSendSMSTwilo.GetAllSendMsgByUserId(UserId);
            if (result.Count == 0) return NotFound();
            return Ok(result);
        }
        
        [HttpGet("GetMessageByUserAndDate/{Id}/{date}")]
        public async Task<IActionResult> GetbyDateAndUserId(long Id, string date)
        {
            var result = await _iSendSMSTwilo.GetAllSendMsgByUserIdByDate(Id, date);
            if (result.Count == 0) return NotFound();
            return Ok(result);
        }
    }
}