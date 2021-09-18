using System;
using System.Collections.Generic;
using System.Text;

namespace PhoneDirectory.Common.Dto
{
    public class InformationDto
    {
 
        public DateTime CreatedDate { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }
        public string Location { get; set; }

        public string Detail { get; set; } // Information Content

        public int Status { get; set; }
  
        public int PersonId { get; set; }
    }
}

