using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 仁매칭infra.Repository
{
    public interface I근로인력매칭Repository
    {
        Task<List<근로인력매칭>> GetBy생산자IdAsync(string 생산자Id);
        Task InsertAsync(근로인력매칭 매칭신청);
    }

    public interface I이성매칭Repository
    {
        Task InsertAsync(이성매칭신청 신청);
    }

    public interface I주문매칭Repository
    {
        Task InsertAsync(주문매칭신청 신청);
    }

    public interface I배송매칭Repository
    {
        Task InsertAsync(배송매칭신청 신청);
    }

    public interface I판로매칭Repository
    {
        Task InsertAsync(판로매칭신청 신청);
    }

    public interface I물류매칭Repository
    {
        Task InsertAsync(물류매칭계약 계약);
    }

}
