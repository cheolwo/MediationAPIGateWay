using MongoDB.Driver;

namespace 仁매칭infra
{
    public class 매칭Service
    {
        private readonly IMongoCollection<이성매칭신청> _이성매칭Collection;
        private readonly IMongoCollection<주문매칭신청> _주문매칭Collection;
        private readonly IMongoCollection<배송매칭신청> _배송매칭Collection;
        private readonly IMongoCollection<판로매칭신청> _판로매칭Collection;
        private readonly IMongoCollection<셔틀매칭신청> _셔틀매칭Collection;
        private readonly IMongoCollection<물류매칭계약> _물류매칭계약Collection;
        private readonly IMongoCollection<근로인력매칭> _근로인력매칭Collection;

        public 매칭Service(IMongoDatabase database)
        {
            _근로인력매칭Collection = database.GetCollection<근로인력매칭>("근로인력매칭");
            _이성매칭Collection = database.GetCollection<이성매칭신청>("이성매칭신청");
            _주문매칭Collection = database.GetCollection<주문매칭신청>("주문매칭신청");
            _배송매칭Collection = database.GetCollection<배송매칭신청>("배송매칭신청");
            _판로매칭Collection = database.GetCollection<판로매칭신청>("판로매칭신청");
            _셔틀매칭Collection = database.GetCollection<셔틀매칭신청>("셔틀매칭신청");
            _물류매칭계약Collection = database.GetCollection<물류매칭계약>("물류매칭계약");
        }
        public async Task<List<근로인력매칭>> Get근로인력매칭By생산자IdAsync(string 생산자Id)
        {
            return await _근로인력매칭Collection
                .Find(매칭 => 매칭.생산자Id == 생산자Id)
                .ToListAsync();
        }
        // 매칭 신청 저장 예시
        public async Task Save이성매칭신청Async(이성매칭신청 신청)
        {
            await _이성매칭Collection.InsertOneAsync(신청);
        }

        public async Task Save주문매칭신청Async(주문매칭신청 신청)
        {
            await _주문매칭Collection.InsertOneAsync(신청);
        }

        public async Task Save배송매칭신청Async(배송매칭신청 신청)
        {
            await _배송매칭Collection.InsertOneAsync(신청);
        }

        public async Task Save판로매칭신청Async(판로매칭신청 신청)
        {
            await _판로매칭Collection.InsertOneAsync(신청);
        }

        public async Task Save물류매칭계약Async(물류매칭계약 계약)
        {
            await _물류매칭계약Collection.InsertOneAsync(계약);
        }
    }
}
