using Models.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.DTOs
{
    public class SendMessagesListDTO
    {
        public long SenderId { get; set; }
        public virtual ComposedMessages ComposedMessages { get; set; }
        [ForeignKey("ComposedMessages")]
        public long MessageId { get; set; }
        public string RecipientPhoneNo { get; set; }
        public bool MessageDelivered { get; set; } = false;
        public string TwiloResponse { get; set; }
        public DateTime SentDate { get; set; } 
    }
}
