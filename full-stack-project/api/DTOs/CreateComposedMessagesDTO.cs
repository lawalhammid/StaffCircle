using Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace api.DTOs
{
    /// <summary>
    /// This keeps users composed messages
    /// </summary>
    public class CreateComposedMessagesDTO
    {
        [Required]
        [MaxLength(160)]
        public string Message { get; set; }
        [Required]
        public long UserId { get; set; }
    }
}