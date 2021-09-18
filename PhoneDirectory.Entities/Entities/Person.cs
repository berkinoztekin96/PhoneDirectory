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

        public DateTime CreatedDate { get; set; }

        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Surname { get; set; }

        public int Status { get; set; }
        public virtual ICollection<Information> Information { get; set; }

     
    }
}
