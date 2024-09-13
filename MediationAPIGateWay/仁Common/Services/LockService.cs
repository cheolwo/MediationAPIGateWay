using StackExchange.Redis;

namespace 仁Common.Services
{
    public class LockService
    {
        private readonly IDatabase _redisDb;

        public LockService(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        public async Task<bool> AcquireLockAsync(string lockKey, TimeSpan expiration)
        {
            return await _redisDb.StringSetAsync(lockKey, "locked", expiration, When.NotExists);
        }

        public async Task ReleaseLockAsync(string lockKey)
        {
            await _redisDb.KeyDeleteAsync(lockKey);
        }
    }
}
