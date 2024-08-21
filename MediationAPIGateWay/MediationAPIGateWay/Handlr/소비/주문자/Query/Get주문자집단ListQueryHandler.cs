using MediatR;
using 주문Infra;
using 주문Common.Model;
using 仁주문자.For주문자.Query;
using Microsoft.EntityFrameworkCore;

namespace MediationAPIGateWay.Handlr.소비.주문자.Query
{
    public class Get주문자집단ListQueryHandler : IRequestHandler<Get주문자집단ListQuery, List<주문자집단>>
    {
        private readonly 주문DbContext _context;

        public Get주문자집단ListQueryHandler(주문DbContext context)
        {
            _context = context;
        }

        public async Task<List<주문자집단>> Handle(Get주문자집단ListQuery request, CancellationToken cancellationToken)
        {
            return await _context.주문자집단목록.ToListAsync(cancellationToken);
        }
    }
}
