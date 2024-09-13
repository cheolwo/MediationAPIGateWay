using DotNetCore.EntityFrameworkCore;
using MongoDB.Driver;
using 주문Infra;

namespace 仁역할자.역할자
{
    public class 仁주문자Repository<T> : EFRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _mongoCollection;
        private readonly 주문DbContext _dbContext;

        public 仁주문자Repository(주문DbContext context, IMongoClient mongoClient, string mongoDatabaseName)
            : base(context)
        {
            _dbContext = context;

            // MongoDB 연결 설정
            var database = mongoClient.GetDatabase(mongoDatabaseName);
            _mongoCollection = database.GetCollection<T>(typeof(T).Name);
        }

        // MongoDB에 데이터를 추가하는 메서드
        public async Task AddToMongoAsync(T entity)
        {
            await _mongoCollection.InsertOneAsync(entity);
        }

        // MongoDB에서 데이터를 조회하는 메서드
        public async Task<List<T>> GetAllFromMongoAsync()
        {
            return await _mongoCollection.Find(_ => true).ToListAsync();
        }

        // MongoDB에서 특정 조건에 맞는 데이터를 조회
        public async Task<T> GetFromMongoByIdAsync(string id)
        {
            return await _mongoCollection.Find(e => e.ToString() == id).FirstOrDefaultAsync();
        }

        // MongoDB에서 데이터를 삭제하는 메서드
        public async Task DeleteFromMongoAsync(string id)
        {
            await _mongoCollection.DeleteOneAsync(e => e.ToString() == id);
        }

        // Entity Framework의 SaveChanges와 비슷한 기능
        public async Task SaveChangesAsync()
        {
            // RDB에서 변경 사항 저장 (Entity Framework)
            await _dbContext.SaveChangesAsync();

            // MongoDB에서는 특별히 따로 저장할 트랜잭션 관리가 필요하지 않으므로 이 부분은 비어 있음
        }
    }
}
