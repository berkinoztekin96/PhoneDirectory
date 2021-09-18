using PhoneDirectory.Common.Dto.Information;
using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneDirectory.Common.Dto.Person
{
    public class PersonDto
    {
        public DateTime CreatedDate { get; set; }
 
        public string Name { get; set; }
   
        public string Surname { get; set; }

        public int Status { get; set; }

        public InformationDto Information { get; set; }
    }
}
