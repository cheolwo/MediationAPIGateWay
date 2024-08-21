using 국토교통부_공공데이터Common.Model;
using 국토교통부_공공데이터Common;
using 네이버Infra;
using 네이버Infra.Request;

namespace MediationAPIGateWay.Handlr.소비.공동주택단지
{
    public class 공동주택좌표수집Handler
    {
        private readonly GeocodeAPIService _geocodeService;
        private readonly 공동주택DbContext _dbContext;

        public 공동주택좌표수집Handler(GeocodeAPIService geocodeService, 공동주택DbContext dbContext)
        {
            _geocodeService = geocodeService;
            _dbContext = dbContext;
        }

        public async Task UpdateCoordinatesAsync(List<공동주택> 공동주택List)
        {
            foreach (var 공동주택 in 공동주택List)
            {
                if (string.IsNullOrEmpty(공동주택.법정동주소)) continue;

                var geocodeRequest = new GeocodeRequest
                {
                    Query = 공동주택.법정동주소
                };

                var geocodeResponse = await _geocodeService.GetGeocodeAsync(geocodeRequest);

                if (geocodeResponse.Status == "OK" && geocodeResponse.Addresses.Count > 0)
                {
                    var address = geocodeResponse.Addresses[0];
                    공동주택.Latitude = double.Parse(address.Y);
                    공동주택.Longitude = double.Parse(address.X);
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
