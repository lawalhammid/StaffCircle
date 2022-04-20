using BusinessLogic.Contracts;
using BusinessLogic.Parameters;
using EFCore.EFContext;
using EFCore.UOF;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Helpers;
using BusinessLogic.Responses;

namespace BusinessLogic.Services
{
    /// <summary>
    /// This service manages all users composed messages.
    /// </summary>
    public class MessagesService : IMessages
    {
        private readonly EfDataContext _dbContext;
        private readonly IUnitOfWork _iUnitOfContext;
        private readonly IUsers _iUsers;
        private const string messageTag = "#";
        public MessagesService(EfDataContext dbContext, IUnitOfWork iUnitOfContext, IUsers iUsers)
        {
            this._dbContext = dbContext;
            this._iUnitOfContext = iUnitOfContext;
            this._iUsers = iUsers;
        }

        public async Task<ResponseClass> AddNewMessage(ComposedMessages composedMessages)
        {
            var userExist = await _iUsers.GetUsersByUserId(composedMessages.UserId);
            if (userExist == null) return ErrorResponses.GetMessageById(5);

            if(!ValidateMessageLenght(composedMessages.Message)) return ErrorResponses.GetMessageById(4);

            composedMessages.MessageNo = GenerateMessageNo(userExist);

            await _dbContext.AddAsync(composedMessages);
            var res = await _iUnitOfContext.Save();
            if(res > 0) {
                var result = CreatedResponseMessages.GetMessageById(3);
                result.ResponseMessage = $"{result.ResponseMessage}{composedMessages.MessageNo}";
                return result;
            }
            return ErrorCreatedResMessages.GetMessageById(2);

        }
        public async Task<ResponseClass> SendSMS(ComposedMessages user)
        {
            var getSenderExist = await _iUsers.GetUsersByUserId(user.UserId);
            if (getSenderExist == null) return ErrorResponses.GetMessageById(3);

           // if (res > 0) return CreatedResponseMessages.GetMessageById(1);
            return ErrorCreatedResMessages.GetMessageById(1);

        }
        // From below, I Generated Unique Id for each message using user Id
        // and datetime ticks and starts picking id from position 8 and selected 5 characters
        public string GenerateMessageNo(Users  user)
        {
            var date = DateTime.Now.Ticks.ToString().Substring(8, 5);
            return $"{messageTag}{user.Id}-{date}";
        }
        public bool ValidateMessageLenght(string message)
        {
            int requirementSMSLength = AppSettingsConfig.MessageLength();
            if (message.Length < requirementSMSLength || message.Length == requirementSMSLength) return true;
            return false;
        }
        public async Task<ComposedMessages> GetMessageById(string MessageNo)
        {
            return await _dbContext.ComposedMessages.AsNoTracking().SingleOrDefaultAsync(c=> c.MessageNo == MessageNo);
        }
    }
}