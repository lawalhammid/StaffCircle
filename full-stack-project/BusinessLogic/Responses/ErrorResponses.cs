using BusinessLogic.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Responses
{
    public static class ErrorResponses
    {
        private  static int requirementSMSLength = AppSettingsConfig.MessageLength();
        public static ResponseClass GetMessageById(int MessageId)
        {
            Dictionary<int, ResponseList> DictionaryMessage = new Dictionary<int, ResponseList>
            {
                  { 1, new ResponseList() { MessageId = 1,IsSuccessful = false, ResponseMessage = "Invalid credentials" }},
                   { 2, new ResponseList() { MessageId = 1,IsSuccessful = false, ResponseMessage = "The account you are trying to create already exists." }},
                   { 3, new ResponseList() { MessageId = 1,IsSuccessful = false, ResponseMessage = "You need to register on this platform before sending an SMS.." }},
                   { 4, new ResponseList() { MessageId = 1,IsSuccessful = false, ResponseMessage = $"Your message should not be more than { requirementSMSLength } characters." }},
                   { 5, new ResponseList() { MessageId = 1,IsSuccessful = false, ResponseMessage = $"You need to log on to perform this action" }}

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
