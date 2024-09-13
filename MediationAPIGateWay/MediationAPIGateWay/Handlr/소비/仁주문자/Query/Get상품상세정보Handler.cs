using MediatR;
using Microsoft.EntityFrameworkCore;
using 생산Infra;
using 판매Infra;
using 仁주문자.For주문자.Query;

namespace MediationAPIGateWay.Handlr.소비.仁주문자.Query
{
    public class Get상품상세정보Handler : IRequestHandler<Get상품상세정보Query, Get상품상세정보Response>
    {
        private readonly 판매DbContext _판매Context;
        private readonly 생산DbContext _생산Context;

        public Get상품상세정보Handler(판매DbContext 판매Context, 생산DbContext 생산Context)
        {
            _판매Context = 판매Context;
            _생산Context = 생산Context;
        }

        public async Task<Get상품상세정보Response> Handle(Get상품상세정보Query request, CancellationToken cancellationToken)
        {
            // Step 1: 판매상품 및 상세정보 조회
            var 상품 = await _판매Context.판매상품들
                .Include(p => p.상세정보)
                .FirstOrDefaultAsync(p => p.Id == request.상품Id, cancellationToken);

            if (상품 == null)
            {
                throw new KeyNotFoundException("상품을 찾을 수 없습니다.");
            }

            // Step 2: 생산자 및 농협 정보 조회
            var 생산자 = await _생산Context.생산자들
                .Include(s => s.후기목록)
                .FirstOrDefaultAsync(s => s.Id == 상품.생산자Id, cancellationToken);

            var 농협 = await _생산Context.농협들
                .Include(n => n.후기목록)
                .FirstOrDefaultAsync(n => n.Id == 상품.농협Id, cancellationToken);

            // Step 3: 상품 후기를 조회
            var 상품후기목록 = 상품.상세정보.후기목록.Select(f => new Get상품상세정보Response.후기Response
            {
                작성자Id = f.작성자Id,
                내용 = f.내용,
                평점 = f.평점,
                작성일 = f.작성일
            }).ToList();

            // Step 4: 생산자 및 농협 후기를 조회
            var 생산자후기목록 = 생산자.후기목록.Select(f => new Get상품상세정보Response.후기Response
            {
                작성자Id = f.작성자Id,
                내용 = f.내용,
                평점 = f.평점,
                작성일 = f.작성일
            }).ToList();

            var 농협후기목록 = 농협.후기목록.Select(f => new Get상품상세정보Response.후기Response
            {
                작성자Id = f.작성자Id,
                내용 = f.내용,
                평점 = f.평점,
                작성일 = f.작성일
            }).ToList();

            // Step 5: 응답 데이터 구성
            var response = new Get상품상세정보Response
            {
                이름 = 상품.이름,
                상품코드 = 상품.상품코드,
                가격 = 상품.가격,
                설명 = 상품.상세정보.설명,
                원산지 = 상품.상세정보.원산지,
                관계매칭지원가능 = 상품.상세정보.관계매칭지원가능,
                근로매칭지원가능 = 상품.상세정보.근로매칭지원가능,
                생산자이름 = 생산자.이름,
                농협이름 = 농협.이름,
                상품후기목록 = 상품후기목록,
                생산자후기목록 = 생산자후기목록,
                농협후기목록 = 농협후기목록
            };

            return response;
        }
    }
}
