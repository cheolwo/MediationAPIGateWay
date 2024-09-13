using MediatR;
using 도서Infra;
using 仁도서관련자.Command;

namespace MediationAPIGateWay.Handlr.사서.Command
{
    public class Create도서CommandHandler : IRequestHandler<Create도서Command, bool>
    {
        private readonly 도서DbContext _dbContext;
        private readonly ILogger<Create도서CommandHandler> _logger;

        public Create도서CommandHandler(도서DbContext dbContext, ILogger<Create도서CommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> Handle(Create도서Command command, CancellationToken cancellationToken)
        {
            var newBook = new 도서
            {
                제목 = command.제목,
                저자 = command.저자,
                도서적재 = new 도서적재
                {
                    적재일 = command.적재일
                }
            };

            _dbContext.도서들.Add(newBook);
            var result = await _dbContext.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                _logger.LogInformation("Successfully added 도서: {제목}", command.제목);
                return true;
            }

            _logger.LogError("Failed to add 도서: {제목}", command.제목);
            return false;
        }
    }
}
