using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SMTPEmail
{
    class MessageSender
    {
        public EmailSettings settings;
        public EmailMessage message;

        public MessageSender(EmailSettings es, EmailMessage em)
        {
            settings = es;
            message = em;
        }

        public void Send()
        {
            //init connect with server and send message;
            MainWindow.console.OutputBlock.Inlines.Add("Send method was ...\n");
        }

        private void SendData()
        {

        }

        private bool ValidateServerAnswer(string serverAnswer)
        {
            return true;
        }

        private void ErrorHappend()
        {
            MessageBox.Show("Error Happend");
        }

    }
}
