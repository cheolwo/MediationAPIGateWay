using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using 仁Common.Services;

namespace 仁Common.Query
{
    public abstract class QueryControllerBase<T> : ControllerBase
    {
        private readonly LockService _lockService;
        private readonly IDatabase _redisDb;

        protected QueryControllerBase(LockService lockService, IConnectionMultiplexer redis)
        {
            _lockService = lockService;
            _redisDb = redis.GetDatabase();
        }

        protected abstract string GetCacheKey(string id);

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(string id)
        {
            var lockKey = GetLockKey(id);

            // 락을 획득하려고 시도
            if (await _lockService.AcquireLockAsync(lockKey, TimeSpan.FromSeconds(10)))
            {
                try
                {
                    // 데이터 조회
                    var cacheKey = GetCacheKey(id);
                    var itemData = await _redisDb.StringGetAsync(cacheKey);

                    if (itemData.IsNullOrEmpty)
                    {
                        return NotFound($"{typeof(T).Name} data not found");
                    }

                    var item = JsonConvert.DeserializeObject<T>(itemData);
                    return Ok(item);
                }
                finally
                {
                    // 락 해제
                    await _lockService.ReleaseLockAsync(lockKey);
                }
            }
            else
            {
                return StatusCode(423, "Resource is locked"); // 423 Locked 상태 코드
            }
        }

        private string GetLockKey(string id)
        {
            return $"{typeof(T).Name}:{id}:Lock";
        }
    }
}
