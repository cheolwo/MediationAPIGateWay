using MediatR;
using Microsoft.EntityFrameworkCore;
using 생산Infra;
using 생산Infra.Model;

namespace 仁근로자.Handlr
{
    public class Get근무지후기QueryHandler : IRequestHandler<Get근무지후기Query, List<후기>>
    {
        private readonly 생산DbContext _context;

        public Get근무지후기QueryHandler(생산DbContext context)
        {
            _context = context;
        }

        public async Task<List<후기>> Handle(Get근무지후기Query request, CancellationToken cancellationToken)
        {
            // LINQ: 해당 근무지의 후기를 조회
            // SQL: SELECT * FROM 후기 WHERE 근무지Id = @근무지Id;
            return await _context.후기들.Where(h => h.근무지Id == request.근무지Id).ToListAsync();
        }
    }
    public class Get농협정보With근무지QueryHandler : IRequestHandler<Get농협정보With근무지Query, 농협?>
    {
        private readonly 생산DbContext _context;

        public Get농협정보With근무지QueryHandler(생산DbContext context)
        {
            _context = context;
        }

        public async Task<농협?> Handle(Get농협정보With근무지Query request, CancellationToken cancellationToken)
        {
            // LINQ: 농협Id로 먼저 특정 농협을 조회하고, 해당 농협의 근무지 목록을 함께 조회
            // SQL: 
            // SELECT n.*, w.* 
            // FROM 농협 AS n
            // LEFT JOIN 근무지 AS w ON n.Id = w.농협Id 
            // WHERE n.Id = @농협Id;
            return await _context.농협들
                .Where(n => n.Id == request.농협Id)  // 먼저 농협Id로 필터링
                .Include(n => n.근무지목록)            // 해당 농협의 근무지 목록을 함께 조회
                .FirstOrDefaultAsync(cancellationToken); // 첫 번째 일치하는 농협을 반환
        }
    }
}
