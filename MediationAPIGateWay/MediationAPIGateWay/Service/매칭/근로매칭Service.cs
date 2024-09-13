using MediationAPIGateWay.Service.검색;
using 근로Infra;
using 네이버Infra;

namespace MediationAPIGateWay.Service.매칭
{
    public class 근로매칭Service
    {
        private readonly GeocodeAPIService _geocodeService;
        private readonly 생산자SearchService _elasticService;

        public 근로매칭Service(GeocodeAPIService geocodeService, 생산자SearchService elasticService)
        {
            _geocodeService = geocodeService;
            _elasticService = elasticService;
        }

        public async Task<List<근로신청>> GetMatchingWorkersByLocationAsync(double latitude, double longitude)
        {
            // 1. 생산자의 위도/경도 정보로 Naver API를 통해 주소를 도출
            var geocodeResponse = await _geocodeService.GetGeocodeAsync(latitude, longitude);
            var producerAddress = geocodeResponse.Addresses?.FirstOrDefault()?.RoadAddress;

            if (string.IsNullOrEmpty(producerAddress))
            {
                throw new Exception("Could not retrieve address for the given coordinates.");
            }

            // 2. 엘라스틱 서치를 통해 키워드 매칭을 수행
            var matchingWorkers = await _elasticService.GetMatchingWorkersSplitAddressAsync(producerAddress);

            return matchingWorkers;
        }
    }
}
