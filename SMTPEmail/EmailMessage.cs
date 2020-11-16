using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMTPEmail
{
    class EmailMessage
    {
        public string EmailAddressTo;
        public string EmailAddressFrom;
        public string EmailSubject;
        public string EmailText;

        //Headers
        public string From;
        public string Date;
        public string To;

        public EmailMessage(string to, string from, string subject, string text)
        {
            EmailAddressTo = to;
            EmailAddressFrom = from;
            EmailSubject = subject;
            EmailText = text;
        }
    }
}
