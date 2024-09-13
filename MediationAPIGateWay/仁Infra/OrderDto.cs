using MediatR;

namespace 仁Infra
{
    // DTO 정의
    public class OrderDto
    {
        public string OrderId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public string UserId { get; set; }
    }

    // BeforeCommand 정의
    public class BeforeOrderCommand : IRequest
    {
        public OrderDto Order { get; }

        public BeforeOrderCommand(OrderDto order)
        {
            Order = order;
        }
    }

    // 本Command 정의
    public class OrderCommand : IRequest
    {
        public OrderDto Order { get; }

        public OrderCommand(OrderDto order)
        {
            Order = order;
        }
    }

    // AfterCommand 정의
    public class AfterOrderCommand : IRequest
    {
        public OrderDto Order { get; }

        public AfterOrderCommand(OrderDto order)
        {
            Order = order;
        }
    }
}
