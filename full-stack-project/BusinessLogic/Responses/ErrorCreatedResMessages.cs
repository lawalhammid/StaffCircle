using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Responses
{
    /// <summary>
    /// Note: This messages will be store on Redis
    /// </summary>
    public static class ErrorCreatedResMessages
    {
        public static ResponseClass GetMessageById(int MessageId)
        {
            Dictionary<int, ResponseList> DictionaryMessage = new Dictionary<int, ResponseList>
            {
                  { 1, new ResponseList() { MessageId = 1,IsSuccessful = false, ResponseMessage = "Your account couldn't be created this time. Please try again after sometimes." }},
                  { 2, new ResponseList() { MessageId = 1,IsSuccessful = false, ResponseMessage = "Your message couldn't be created at this time. Please try again after sometimes." }},
                  { 3, new ResponseList() { MessageId = 1,IsSuccessful = false, ResponseMessage = "Your message couldn't be sent at this time. Please try again after sometimes." }},
                { 4, new ResponseList() { MessageId = 1,IsSuccessful = false, ResponseMessage = "Your message couldn't be sent via twilio at this time. Please try again after sometimes." }}

            };

            var res = DictionaryMessage[MessageId];

            return new ResponseClass
            {
                IsSuccessful = res.IsSuccessful,
                ResponseMessage = res.ResponseMessage,
            };
        }

    }
}
