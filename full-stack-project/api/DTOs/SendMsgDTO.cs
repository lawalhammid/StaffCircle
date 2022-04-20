namespace api.DTOs
{
    public class SendMsgDTO
    {
        public SendMessagesDTO SendMessagesDTO { get; set; }
        public ComposedMessagesDTO ComposedMessagesDTO { get; set; }
        public string NewMessage { get; set; } 
    }
}