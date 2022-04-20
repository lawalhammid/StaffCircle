using System.ComponentModel.DataAnnotations;

namespace api.DTOs
{
    public class ComposedMessagesDTO
    {
        [Required]
        [MaxLength(160)]
        public string Message { get; set; }
        public string MessageNo { get; set; }
        public long UserId { get; set; }
    }
}
