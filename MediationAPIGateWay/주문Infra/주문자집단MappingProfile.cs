using AutoMapper;
using 국토교통부_공공데이터Common.Model;
using 주문Common.Model;

namespace 주문Infra
{
    public class 주문자집단MappingProfile : Profile
    {
        public 주문자집단MappingProfile()
        {
            CreateMap<공동주택, 주문자집단>()
                .ForMember(dest => dest.단지코드, opt => opt.MapFrom(src => src.단지코드))
                .ForMember(dest => dest.단지명, opt => opt.MapFrom(src => src.단지명))
                .ForMember(dest => dest.법정동코드, opt => opt.MapFrom(src => src.법정동코드))
                .ForMember(dest => dest.법정동주소, opt => opt.MapFrom(src => src.법정동주소));
        }
    }
}
