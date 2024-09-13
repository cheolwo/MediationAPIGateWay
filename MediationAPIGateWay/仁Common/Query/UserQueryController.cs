using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 仁Common.Services;

namespace 仁Common.Query
{
    public abstract class UserQueryController<T> : ControllerBase
    {
        private readonly LockService _lockService;
        private readonly IDatabase _redisDb;

        protected UserQueryController(LockService lockService, IConnectionMultiplexer redis)
        {
            _lockService = lockService;
            _redisDb = redis.GetDatabase();
        }

        protected abstract string GetCacheKey(string userId);

        [HttpGet("{userId}/{queryParam}")]
        public async Task<IActionResult> GetUserQuery(string userId, string queryParam)
        {
            var lockKey = GetUserLockKey(userId);

            // 사용자 락 획득 시도
            if (await _lockService.AcquireLockAsync(lockKey, TimeSpan.FromSeconds(10)))
            {
                try
                {
                    // 데이터 조회
                    var cacheKey = GetCacheKey(userId);
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

        private string GetUserLockKey(string userId)
        {
            return $"User:{userId}:Lock";
        }
    }

}
