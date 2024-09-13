using MongoDB.Driver;
using 사용자Infra;
using 주문Common.Model;
using 주문Infra;

namespace MediationAPIGateWay.Repository
{
    public class 주문자집단Repository
    {
        private readonly IMongoCollection<사용자> _mongoUserCollection;
        private readonly 주문DbContext _dbContext;

        public 주문자집단Repository(IMongoDatabase mongoDatabase, 주문DbContext dbContext)
        {
            _mongoUserCollection = mongoDatabase.GetCollection<사용자>("사용자");
            _dbContext = dbContext;
        }

        // MongoDB에서 사용자 정보를 가져오는 메서드
        public async Task<사용자> GetUserAsync(string 사용자Id)
        {
            return await _mongoUserCollection.Find(u => u.Id == 사용자Id).FirstOrDefaultAsync();
        }

        // RDBMS에서 집단 정보를 가져오는 메서드
        public async Task<주문자집단> Get집단Async(string 집단코드)
        {
            return await _dbContext.주문자집단들.FindAsync(집단코드);
        }

        // RDBMS에서 집단에 주문자를 추가하는 메서드
        public async Task Add주문자To집단Async(string 집단코드, 주문자 주문자)
        {
            var 집단 = await _dbContext.주문자집단들.FindAsync(집단코드);
            if (집단 != null)
            {
                if (집단.주문자들 == null)
                {
                    집단.주문자들 = new List<주문자>();
                }

                집단.주문자들.Add(주문자);
                _dbContext.Update(집단);  // RDBMS에 업데이트
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("집단이 존재하지 않습니다.");
            }
        }

        // MongoDB에서 사용자의 주문자집단 목록을 업데이트하는 메서드
        public async Task UpdateUser집단Async(string 사용자Id, 주문자집단 집단)
        {
            var 사용자 = await GetUserAsync(사용자Id);
            if (사용자 != null)
            {
                // JSON으로 저장된 사용자 정보 업데이트
                var 주문자 = JsonConvert.DeserializeObject<주문자>(사용자.주문자Json);
                if (주문자.주문자집단목록 == null)
                {
                    주문자.주문자집단목록 = new List<주문자집단>();
                }

                주문자.주문자집단목록.Add(집단);
                사용자.주문자Json = JsonConvert.SerializeObject(주문자);

                // MongoDB 업데이트
                var filter = Builders<사용자>.Filter.Eq(u => u.Id, 사용자.Id);
                var update = Builders<사용자>.Update.Set(u => u.주문자Json, 사용자.주문자Json);
                await _mongoUserCollection.UpdateOneAsync(filter, update);
            }
        }
    }
}
