using MongoDB.Driver;
using Newtonsoft.Json;
using 사용자Infra;
using 주문Infra;
using 주문Infra.Model;

namespace MediationAPIGateWay.Repository
{
    public class 개별주문Repository
    {
        private readonly IMongoCollection<사용자> _mongoUserCollection;
        private readonly 주문DbContext _dbContext;

        public 개별주문Repository(IMongoDatabase mongoDatabase, 주문DbContext dbContext)
        {
            _mongoUserCollection = mongoDatabase.GetCollection<사용자>("사용자");
            _dbContext = dbContext;
        }

        // MongoDB에서 사용자 정보를 조회하는 메서드
        public async Task<사용자> GetUserAsync(string 사용자Id)
        {
            return await _mongoUserCollection.Find(u => u.Id == 사용자Id).FirstOrDefaultAsync();
        }

        // MongoDB에서 사용자 정보를 업데이트하는 메서드
        public async Task UpdateUserAsync(사용자 사용자, 주문자 주문자)
        {
            사용자.주문자Json = JsonConvert.SerializeObject(주문자);
            var filter = Builders<사용자>.Filter.Eq(u => u.Id, 사용자.Id);
            var update = Builders<사용자>.Update.Set(u => u.주문자Json, 사용자.주문자Json);
            await _mongoUserCollection.UpdateOneAsync(filter, update);
        }

        // RDBMS에 새로운 주문을 저장하는 메서드
        public async Task SaveOrderAsync(주문상품 newOrder)
        {
            _dbContext.주문상품목록.Add(newOrder);
            await _dbContext.SaveChangesAsync();
        }
    }

}
