using MediatR;
using 仁Common.Command.Queue;

namespace 仁Common.Command
{
    public class ErrorCommand : IRequest<Unit>, IQueueableCommand
    {
        public string ErrorMessage { get; set; }
        public string ErrorType { get; set; }
        public string UserId { get; set; }  // 사용자에게 오류를 전달할 때 필요한 정보

        public string GetQueueName()
        {
            return "ErrorQueue";  // 오류 처리 전용 Queue
        }
    }
}
