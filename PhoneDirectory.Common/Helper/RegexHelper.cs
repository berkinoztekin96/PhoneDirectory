using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;

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

        public bool IsPhoneNumber(string phone)
        {
            string pattern = @"^(0(\d{3})(\d{3})(\d{2})(\d{2}))$";
            Match match = Regex.Match(phone, pattern, RegexOptions.IgnoreCase);

            return match.Success;
        }

    }
}
