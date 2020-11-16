using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
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
            MainWindow.console.OutputBlock.Inlines.Add("Send method was ...\n");

            TcpClient tcpclient = new TcpClient();
            SslStream sslstream;
            StreamWriter sw;
            StreamReader reader;

            try
            {
                tcpclient.Connect($"{settings.EmailServer}", 465);
                sslstream = new SslStream(tcpclient.GetStream());
                sslstream.AuthenticateAsClient($"{settings.EmailServer}");
                sw = new StreamWriter(sslstream);
                reader = new StreamReader(sslstream);
            }
            catch (Exception e)
            {
                ErrorHappend(e.ToString());
                return;
            }

            // sw.WriteLine("HELO " + "smtp.yandex.ru");
            sw.WriteLine("EHLO " + $"{settings.EmailServer}");
            sw.Flush();

            string str;

            int chars = -1;
            char[] buffer = new char[2048];

            str = reader.ReadLine();
            LogWrite(str);

            chars = reader.Read(buffer, 0, buffer.Length);

            LogWrite(new string(buffer)); // 250 code if success 502 if command unrecognized

            var anser_s = new string(buffer).Split(' ');
            if (anser_s[0].Equals("502"))
            {
                ErrorHappend(new string(buffer));
            }

            var name = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(settings.UserName.Split('@')[0]));
            var password = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(settings.UserPassword)); // need the Application password

            sw.WriteLine("AUTH LOGIN");
            sw.Flush();

            str = reader.ReadLine();
            string[] tokens = str.Split(' ');
            var s1 = tokens[0];
            var s2 = Encoding.UTF8.GetString(System.Convert.FromBase64String(tokens[1]));
            LogWrite($"{s1} {s2}");

            sw.WriteLine(name);
            sw.Flush();

            str = reader.ReadLine();
            tokens = str.Split(' ');
            s1 = tokens[0];
            s2 = Encoding.UTF8.GetString(System.Convert.FromBase64String(tokens[1]));
            LogWrite($"{s1} {s2}");

            sw.WriteLine(password);
            sw.Flush();

            str = reader.ReadLine(); // 235 success 535 - incorrect password
            if (str.Split(' ')[0].Equals("535"))
            {
                ErrorHappend(str);
            }
            LogWrite(str);

            // if can pipeline...
            sw.WriteLine("MAIL FROM:<" +$"{message.EmailAddressFrom}" + ">");
            sw.WriteLine("RCPT TO:<" + $"{message.EmailAddressTo}" + ">");
            sw.WriteLine("DATA");
            sw.Flush();

            str = reader.ReadLine();
            ValidateServerAnswer(str, "250");

            str = reader.ReadLine();
            ValidateServerAnswer(str, "250");

            str = reader.ReadLine();
            ValidateServerAnswer(str, "354");

            //headers
            sw.WriteLine("MIME-Version: 1.0");
            sw.WriteLine(message.From);
            sw.WriteLine(message.Date);
            sw.WriteLine(message.To);
            sw.WriteLine($"Subject: {message.EmailSubject}");
            sw.WriteLine("Content-Type: text/plain; charset=\"UTF-8\"");
            sw.WriteLine("");
            //body
            sw.Write(message.EmailText + "\r\n");  
            sw.WriteLine(".");
            sw.Flush();

            str = reader.ReadLine();
            if (ValidateServerAnswer(str, "250"))
            {
                ErrorHappend("Сообщение отправлено");
            }

            sw.WriteLine("QUIT");
            sw.Flush();

            str = reader.ReadLine();
            ValidateServerAnswer(str, "221");
        }

        private bool ValidateServerAnswer(string answer, string expected)
        {
            if (answer.Split(' ')[0].Equals(expected))
            {
                LogWrite(answer);
                return true;
            }
            ErrorHappend(answer);
            return false;
        }

        private void ErrorHappend(string error)
        {
            MessageBox.Show($"Error happend\n{error}");
            LogWrite(error);
        }

        private void LogWrite(string str)
        {
            MainWindow.console.OutputBlock.Inlines.Add(str + '\n');
        }

    }
}
