using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneDirectory.Common.Dto.Information
{
    public class CreateInformationDto
    {
        public int PersonId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Location { get; set; } = "ANKARA";
        public string Detail { get; set; }
    }
}
