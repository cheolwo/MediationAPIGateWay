﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 仁Common.Services
{
    public interface INotificationService
    {
        Task NotifyUserAsync(string userId, string message);
        Task NotifyAdminAsync(string message);
    }
}
