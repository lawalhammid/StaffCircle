using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
    /// <summary>
    /// This keeps users composed messages
    /// </summary>
    public class ComposedMessages 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        [Required]
        [MaxLength(160)]
        public string Message { get; set; }
        public string MessageNo { get; set; }
        [Required]
        public virtual Users Users { get; set; }
        [ForeignKey("Users")]
        public long UserId { get; set; }
        [Required]
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }


}