using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CoreLibrary.Service.Interface;

namespace CoreLibrary.Service
{
    public abstract class MessageSender : IEmailSender, ISmsSender
    {
        public abstract Task SendEmailAsync(string email, string subject, string message);

        public abstract Task SendSmsAsync(string number, string message);
    }
}
