using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace PhoneDirectory.Common.Helper
{
    public class RegexHelper
    {

        public bool IsValidMail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}
