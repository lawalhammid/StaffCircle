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
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using EFCore.MappedModels;

namespace BusinessLogic.Services
{
    /// <summary>
    /// This service send message via Twilo API.
    /// </summary>
    public class SendSMSTwiloService : ISendSMSTwilo
    {
        private readonly EfDataContext _dbContext;
        private readonly IUnitOfWork _iUnitOfContext;
        private readonly IUsers _iUsers;
        private readonly IMessages _iMessages;
        
        public SendSMSTwiloService(EfDataContext dbContext, IUnitOfWork iUnitOfContext, IUsers iUsers, IMessages iMessages)
        {
            this._dbContext = dbContext;
            this._iUnitOfContext = iUnitOfContext;
            this._iUsers = iUsers;
            this._iMessages = iMessages;
        }

        public async Task<int> SaveMessage(SendMessages sendMessages)
        {
            await _dbContext.AddAsync(sendMessages);
            return await _iUnitOfContext.Save();
        }

        public async Task<ResponseClass> SendMessage(SendMessages sendMessages, ComposedMessages composedMessages, string NewMessage)
        {
            var messageExist = await _iMessages.GetMessageById(composedMessages.MessageNo);
            
            int save = 0;

            //save message to composedMessages since it does not exist in composedMessages
            if (messageExist == null) {
                //will later use mapper here
                var comNewMsg = new ComposedMessages
                {
                    Message = NewMessage,
                    UserId = sendMessages.SenderId,
                };
               
                //Save message 
               var saveComposeMsg = await _iMessages.AddNewMessage(comNewMsg);
               if(saveComposeMsg.IsSuccessful)
                {
                    //will later use mapper here
                     save = await SaveMessage( new SendMessages{
                          RecipientPhoneNo = sendMessages.RecipientPhoneNo,
                          MessageId = comNewMsg.Id,
                          SenderId = sendMessages.SenderId,
                     });
                }
            }
            else
            {
                 save = await SaveMessage(sendMessages);
            }
            string sendViaTwilo = String.Empty;
            if (save > 0)
            {
                //Send message to reciepient
                sendViaTwilo = await Send(sendMessages, NewMessage);
                if (!string.IsNullOrEmpty(sendViaTwilo)) {
                    sendMessages.MessageDelivered = true;
                    await UpdateSaveMessage(sendMessages);

                    var result = CreatedResponseMessages.GetMessageById(4);
                    result.ResponseMessage = $"{result.ResponseMessage}{composedMessages.MessageNo}";

                    return result;
                }
            }
            //error from twilio
            if (string.IsNullOrEmpty(sendViaTwilo)) return ErrorCreatedResMessages.GetMessageById(4);

            return ErrorCreatedResMessages.GetMessageById(3);
        }

        public async Task<int> UpdateSaveMessage(SendMessages sendMessages)
        {
            _dbContext.Update(sendMessages);
            return await _iUnitOfContext.Save();
        }

        //Below handles sending message via twilo
        public async Task<string> Send(SendMessages SendMessages, string messageBody)
        {
            var sender = await _iUsers.GetUsersByUserId(SendMessages.SenderId);
            var twiLoCredential = AppSettingsConfig.TwiloCredential();

            // Find your Account SID and Auth Token at twilio.com/console
            // and set the environment variables. See http://twil.io/secure
            try
            {
                TwilioClient.Init(twiLoCredential.TWILIO_ACCOUNT_SID, twiLoCredential.TWILIO_AUTH_TOKEN);

                var message = MessageResource.Create(
                    body: messageBody,
                    from: new Twilio.Types.PhoneNumber(sender.PhoneNumber),
                    to: new Twilio.Types.PhoneNumber(SendMessages.RecipientPhoneNo)
                );

                return message.Sid;
            }
            catch(Exception ex)
            {
                //to add logger here for ex
                return string.Empty;
            }
        }

        public async Task<List<SentMessagesViewModel>> GetAllSendMsgByUserId(long SenderId)
        {
            return await (from msg in _dbContext.SendMessages
                          where (msg.SenderId == SenderId)
                          select new SentMessagesViewModel
                          {
                              SenderName = msg.Users.FullName,
                              Message = msg.ComposedMessages.Message,
                              MessageNo = msg.ComposedMessages.MessageNo,
                              recipientPhoneNo = msg.RecipientPhoneNo,
                              SentDate = Formatter.FormatTransDate(msg.SentDate),
                              Time = Formatter.FormatTransTime(msg.SentDate)

                          }).AsNoTracking().ToListAsync();
        }

        //below get message with user id and date
        public async Task<List<SentMessagesViewModel>> GetAllSendMsgByUserIdByDate(long SenderId, string sendDate)
        {
            return  await (from msg in _dbContext.SendMessages
                         where (msg.SenderId == SenderId && (DateTime)(object)msg.SentDate.Date == (DateTime)(object)sendDate)
                         select new SentMessagesViewModel
                         {
                             SenderName = msg.Users.FullName,
                             Message = msg.ComposedMessages.Message,
                             MessageNo = msg.ComposedMessages.MessageNo,
                             recipientPhoneNo = msg.RecipientPhoneNo,
                             SentDate = Formatter.FormatTransDate(msg.SentDate),
                             Time = Formatter.FormatTransTime(msg.SentDate)
                         }).AsNoTracking().ToListAsync();
        }
    }
}
