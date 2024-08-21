using MediatR;
using Quartz;

namespace 仁Common.Command
{
    public class CommandProcessingJob : IJob
    {
        private readonly ICommandQueue _commandQueue;
        private readonly IMediator _mediator;

        public CommandProcessingJob(ICommandQueue commandQueue, IMediator mediator)
        {
            _commandQueue = commandQueue;
            _mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            // 큐에서 Command를 꺼냄
            var command = await _commandQueue.DequeueAsync();
            if (command == null)
            {
                Console.WriteLine("큐에 명령이 없습니다.");
                return;
            }

            // IMediator를 통해 Command 전달
            await _mediator.Send(command);
            Console.WriteLine("명령이 성공적으로 처리되었습니다.");
        }
    }
}
