using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SMTPEmail
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class MainWindow : Window
    {
        private MessageSender messageSender;

        public static LogConsole console;

        public MainWindow()
        {
            InitializeComponent();
            messageSender = new MessageSender(new EmailSettings(ServerName.Text, UserName.Text, UserPassword.Password), new EmailMessage(MessageAddressFrom.Text,
                MessageAddressFrom.Text, MessageSubject.Text, MessageSubject.Text));



            StartLogConsole();
        }

        public void SendMessage(object sender, RoutedEventArgs routedEventArgs)
        {
            messageSender.message = new EmailMessage(MessageAddressFrom.Text,
                MessageAddressFrom.Text, MessageSubject.Text, MessageSubject.Text);
            messageSender.Send();
        }

        public void SaveSettings(object sender, RoutedEventArgs routedEventArgs)
        {
            messageSender.settings = new EmailSettings(ServerName.Text, UserName.Text, UserPassword.Password);


            console.OutputBlock.Inlines.Add("Save Settings Button was clicked...\n");
        }

        public void StartLogConsole()
        {
            MainWindow mainwindow = this;
            console = new LogConsole(mainwindow);
            // window position
            console.Left = this.Left + 0;
            console.Top = this.Top + 0;
            console.Hide();
        }

        private void buttonLogConsole_Click(object sender, RoutedEventArgs e)
        {
            console.Left = this.Left + 0;
            console.Top = this.Top + 0;
            console.Show();
        }

    }
}
