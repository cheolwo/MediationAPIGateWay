using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 仁Common.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string userId, string subject, string message);
    }
}
