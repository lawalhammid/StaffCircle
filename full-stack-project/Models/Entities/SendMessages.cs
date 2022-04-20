using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
    public class SendMessages 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        [Required]
        public virtual Users Users { get; set; }
        [ForeignKey("Users")]
        public long SenderId { get; set; }
        [Required]
        public virtual ComposedMessages ComposedMessages { get; set; }
        [ForeignKey("ComposedMessages")]
        public long MessageId { get; set; }
        [Required]
        [StringLength(13, MinimumLength = 10)]
        public string RecipientPhoneNo { get; set; }
        public bool MessageDelivered { get; set; } = false;
        public string TwiloResponse { get; set; }
        [Required]
        public DateTime SentDate { get; set; } = DateTime.Now;
    }
}