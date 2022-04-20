using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Responses
{
    public static class CreatedResponseMessages
    {
        public static ResponseClass GetMessageById(int MessageId)
        {
            Dictionary<int, ResponseList> DictionaryMessage = new Dictionary<int, ResponseList>
            {
                  { 1, new ResponseList() { MessageId = 1,IsSuccessful = true, ResponseMessage = "Your account has been created." }},
                  { 2, new ResponseList() { MessageId = 1,IsSuccessful = true, ResponseMessage = "User(s) created successfully" }},
                  { 3, new ResponseList() { MessageId = 1,IsSuccessful = true, ResponseMessage = "You just created a new message with Id " }},
                  { 4, new ResponseList() { MessageId = 1,IsSuccessful = true, ResponseMessage = "Message(SMS) send successfully" }}
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
