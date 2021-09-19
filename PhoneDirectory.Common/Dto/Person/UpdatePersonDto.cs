using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneDirectory.Common.Dto.Person
{
    public class UpdatePersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string CompanyName { get; set; }
    }
}
