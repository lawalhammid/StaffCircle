using BusinessLogic.Parameters;
using BusinessLogic.Responses;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IMessages
    {
        Task<ResponseClass> AddNewMessage(ComposedMessages user);
        string GenerateMessageNo(Users user);
        Task<ResponseClass> SendSMS(ComposedMessages user);
        bool ValidateMessageLenght(string message);
        Task<ComposedMessages> GetMessageById(string MessageNo);
    }
}