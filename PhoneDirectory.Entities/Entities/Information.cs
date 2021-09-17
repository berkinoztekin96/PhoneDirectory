using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoneDirectory.Entities.Entities
{
    public class Information
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }
        [StringLength(15)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string Email { get; set; }
        public string Location { get; set; }

        public string Detail { get; set; } // Information Content

        public int Status { get; set; }

        public virtual Person Person { get; set; }
        [ForeignKey("Person")]
        public int PersonId { get; set; }
    }
}
