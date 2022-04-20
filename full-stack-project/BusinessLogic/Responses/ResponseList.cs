using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic.Responses
{
    public class ResponseList
    {
        public int MessageId { get; set; }
        public bool IsSuccessful { get; set; } = false;
        public string ResponseMessage { get; set; }
    }

    public class ResponseClass
    {
        public bool IsSuccessful { get; set; } = false;
        public string ResponseMessage { get; set; }
    }
}
