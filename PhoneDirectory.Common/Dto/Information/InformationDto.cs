using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneDirectory.Common.Dto.Information
{
    public class InformationDto
    {
        public int Id{ get; set; }
        public string PersonName { get; set; }
        public string PersonSurname { get; set; }
        public int PersonId { get; set; }
        public DateTime CreatedDate { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
        public string Location { get; set; }

        public string Detail { get; set; } // Information Content

        public int Status { get; set; }

       
    }
}

