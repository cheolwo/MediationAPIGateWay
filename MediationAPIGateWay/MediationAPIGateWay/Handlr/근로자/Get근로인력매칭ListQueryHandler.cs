using MediatR;
using MongoDB.Driver;
using 仁Infra.근로자;
using 仁근로자.Query;
using 仁매칭infra;

namespace MediationAPIGateWay.Handlr.근로자
{
    public class Get근로인력매칭ListQueryHandler : IRequestHandler<Get근로인력매칭ListQuery, List<근로인력매칭DTO>>
    {
        private readonly IMongoCollection<근로인력매칭> _근로인력매칭Collection;

        public Get근로인력매칭ListQueryHandler(IMongoDatabase database)
        {
            _근로인력매칭Collection = database.GetCollection<근로인력매칭>("근로인력매칭");
        }

        public async Task<List<근로인력매칭DTO>> Handle(Get근로인력매칭ListQuery request, CancellationToken cancellationToken)
        {
            var 매칭목록 = await _근로인력매칭Collection.Find(_ => true).ToListAsync(cancellationToken);
            var dtos = 매칭목록.Select(m => new 근로인력매칭DTO
            {
                매칭신청Id = m.Id,
                생산자Id = m.생산자Id,  // 생산자의 ID만 포함되어 있을 것으로 가정
                매칭신청일자 = m.매칭신청일자,
                근로자목록 = m.근로자목록.Select(g => new 근로자정보DTO
                {
                    근로자Id = g.근로자Id,
                    이름 = g.이름,
                    연락처 = g.연락처,
                    상태 = g.상태
                }).ToList()
            }).ToList();

            return dtos;
        }
    }
}
