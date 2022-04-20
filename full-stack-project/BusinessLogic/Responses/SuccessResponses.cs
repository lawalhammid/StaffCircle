using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Responses
{
    public static class SuccessResponses
    {
        public static ResponseClass GetMessageById(int MessageId)
        {
            Dictionary<int, ResponseList> DictionaryMessage = new Dictionary<int, ResponseList>
            {
                  { 1, new ResponseList() { MessageId = 1,IsSuccessful = true, ResponseMessage = "Your logged in was successfully." }},
                  
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
