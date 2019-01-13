using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CoreLibrary.BusinessLogic.Utilities
{
    public class MessageSender : CoreLibrary.Service.MessageSender
    {
        public MessageSender()
        {
            
        }
        public override async Task SendEmailAsync(string email, string subject, string message)
        {
            throw new NotImplementedException();
        }

        public override async Task SendSmsAsync(string number, string message)
        {
            throw new NotImplementedException();
        }
    }
}
