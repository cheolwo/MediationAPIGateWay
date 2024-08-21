using MediatR;

namespace 仁Common.Command
{
    public interface ICommandQueue
    {
        Task EnqueueAsync(IRequest command);
        Task<IRequest> DequeueAsync();
    }

    public class InMemoryCommandQueue : ICommandQueue
    {
        private readonly Queue<IRequest> _commandQueue = new Queue<IRequest>();

        public Task EnqueueAsync(IRequest command)
        {
            _commandQueue.Enqueue(command);
            return Task.CompletedTask;
        }

        public Task<IRequest> DequeueAsync()
        {
            if (_commandQueue.Count == 0)
                return Task.FromResult<IRequest>(null);
            return Task.FromResult(_commandQueue.Dequeue());
        }
    }

}
