using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Entities
{
    public class AuditTrail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key, Column(Order = 0)]
        public long Id { get; set; }
        [Required]
        public string EmailOrUserid { get; set; }
        [Required]
        public DateTime Eventdateutc { get; set; }
        [Required]
        [MaxLength(1)]
        public string Eventtype { get; set; }
        [Required]
        public string TableName { get; set; }
        [Required]
        public long RecordId { get; set; }
        [Required]
        public string OriginalValue { get; set; }
        [Required]
        public string NewValue { get; set; }
    }
}
