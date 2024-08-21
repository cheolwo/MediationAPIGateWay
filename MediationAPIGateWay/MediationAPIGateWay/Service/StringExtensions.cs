namespace MediationAPIGateWay.Service
{
    public static class StringExtensions
    {
        // 이름을 마스킹하는 확장 메서드
        public static string GetMaskedName(this string 이름)
        {
            if (이름.Length <= 2)
            {
                return 이름; // 이름이 2글자 이하인 경우 마스킹하지 않음
            }

            // 첫 글자와 마지막 글자 제외, 나머지를 'x'로 마스킹
            var middleMasked = new string('x', 이름.Length - 2);
            return $"{이름[0]}{middleMasked}{이름[^1]}";
        }
    }

}
