using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 仁Common.Command.Queue
{
    public interface IQueueableCommand
    {
        string GetQueueName();
        string UserId { get; } // 사용자 ID 반환
    }
}
