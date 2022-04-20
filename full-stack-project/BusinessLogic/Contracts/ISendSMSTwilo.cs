using BusinessLogic.Parameters;
using BusinessLogic.Responses;
using EFCore.MappedModels;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface ISendSMSTwilo
    {
        Task<int> SaveMessage(SendMessages SendMessages);
        Task<ResponseClass> SendMessage(SendMessages SendMessages, ComposedMessages composedMessages, string NewMessage);
        Task<int> UpdateSaveMessage(SendMessages sendMessages);
        Task<string> Send(SendMessages SendMessages, string messageBody);
        Task<List<SentMessagesViewModel>> GetAllSendMsgByUserId(long UserId);
        Task<List<SentMessagesViewModel>> GetAllSendMsgByUserIdByDate(long SenderId, string SendDate);
    }
}