using MongoDB.Driver;

namespace 仁매칭infra.Repository
{
    public class 근로인력매칭Repository : I근로인력매칭Repository
    {
        private readonly IMongoCollection<근로인력매칭> _collection;

        public 근로인력매칭Repository(IMongoDatabase database)
        {
            _collection = database.GetCollection<근로인력매칭>("근로인력매칭");
        }

        public async Task<List<근로인력매칭>> GetBy생산자IdAsync(string 생산자Id)
        {
            return await _collection.Find(m => m.생산자Id == 생산자Id).ToListAsync();
        }

        public async Task InsertAsync(근로인력매칭 매칭신청)
        {
            await _collection.InsertOneAsync(매칭신청);
        }
    }

}
