namespace api.DTOs
{
    public class SendMessagesDTO
    {
        public long SenderId { get; set; }
        public long MessageId { get; set; }
        public string RecipientPhoneNo { get; set; }
     
    }
}
