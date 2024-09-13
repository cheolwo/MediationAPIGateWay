using MongoDB.Driver;
using 사용자Infra;
using 仁주문자.For주문자.Command;
using 주문Common.Model;
using 주문Infra;

namespace MediationAPIGateWay.Repository
{
    public class 공동주문Repository
    {
        private readonly IMongoCollection<사용자> _mongoUserCollection;
        private readonly 주문DbContext _dbContext;

        public 공동주문Repository(IMongoDatabase mongoDatabase, 주문DbContext dbContext)
        {
            _mongoUserCollection = mongoDatabase.GetCollection<사용자>("사용자");
            _dbContext = dbContext;
        }

        public async Task<사용자> GetUserAsync(string 사용자Id)
        {
            return await _mongoUserCollection.Find(u => u.Id == 사용자Id).FirstOrDefaultAsync();
        }

        public void AddOrderToUser(주문자 주문자, Create공동주문Command command)
        {
            foreach (var 공동주문상품Dto in command.공동주문상품들)
            {
                var newOrderProduct = new 공동주문상품
                {
                    상품코드 = 공동주문상품Dto.상품Id.ToString(),
                    상품명 = 공동주문상품Dto.상품명,
                    가격 = decimal.Parse(공동주문상품Dto.가격),
                    주문상태 = 주문Status.미확인,
                    집단코드 = command.집단코드
                };

                if (주문자.공동주문상품목록 == null)
                {
                    주문자.공동주문상품목록 = new List<공동주문상품>();
                }

                주문자.공동주문상품목록.Add(newOrderProduct);
                _dbContext.공동주문상품들.Add(newOrderProduct);
            }
        }

        public async Task SaveUserAsync(사용자 사용자, 주문자 주문자)
        {
            사용자.주문자Json = JsonConvert.SerializeObject(주문자);

            var filter = Builders<사용자>.Filter.Eq(u => u.Id, 사용자.Id);
            var update = Builders<사용자>.Update.Set(u => u.주문자Json, 사용자.주문자Json);

            await _mongoUserCollection.UpdateOneAsync(filter, update);
            await _dbContext.SaveChangesAsync();
        }
    }

}
