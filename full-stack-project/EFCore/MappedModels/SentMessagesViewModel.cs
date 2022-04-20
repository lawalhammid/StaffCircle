using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCore.MappedModels
{
    public  class SentMessagesViewModel
    {
        public string MessageNo { get; set; }
        public string Message { get; set; }
        public string SenderName { get; set; }
        public string recipientPhoneNo { get; set; }
        public string SentDate { get; set; }
        public string Time { get; set; }

    }
}
