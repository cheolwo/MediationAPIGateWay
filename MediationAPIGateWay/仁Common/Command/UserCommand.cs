using MediatR;
using 사용자Infra;
using 仁Common.Command.Queue;

namespace 仁Common.Command
{
    public interface IUserCommand
    {
        사용자 사용자 { get; }
        string RequestUrl { get; }
        string Context { get; }  // 주제 또는 맥락
    }

    public abstract class UserCommand : IRequest<Unit>, IQueueableCommand, IUserCommand
    {
        public 사용자 사용자 { get; set; }
        public DateTime Timestamp { get; set; }
        public string RequestUrl { get; set; }
        public string Context { get; set; }  // 새로운 속성 추가

        public string UserId => 사용자.Id;

        protected UserCommand()
        {
            Timestamp = DateTime.UtcNow; // 명령이 생성된 시점을 자동으로 기록
        }

        public string GetQueueName()
        {
            throw new NotImplementedException();
        }
    }
    public abstract class BeforeCommand : UserCommand
    {
        // Before 단계에 특화된 속성이나 메서드 추가
    }

    public abstract class MainCommand : UserCommand
    {
        // Main 단계에 특화된 속성이나 메서드 추가
    }

    public abstract class AfterCommand : UserCommand
    {
        // After 단계에 특화된 속성이나 메서드 추가
    }
}
