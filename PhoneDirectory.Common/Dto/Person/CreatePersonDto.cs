using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneDirectory.Common.Dto.Person
{
   public class CreatePersonDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Location { get; set; }
        public string Detail { get; set; }

    }
}
