using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 仁Common.Services
{
    public interface ISmsService
    {
        Task SendSmsAsync(string userId, string message);
    }
}
