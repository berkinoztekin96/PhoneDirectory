using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PhoneDirectory.Entities.Entities
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Surname { get; set; }

        public virtual Information Information { get; set; }

        [ForeignKey("Information")]
        public int InformationId { get; set; }
    }
}
