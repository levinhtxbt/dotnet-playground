using gRPCNet.MessageContract;
using gRPCNet.MessageContract.Service;
using ProtoBuf.Grpc;
using System.Threading.Tasks;

namespace gRPCNet.AspNetServer.Service
{
    public class CustomerService : ICustomerService
    {
        public ValueTask<CustomerResponse> GetAsync(CustomerRequest request, CallContext context = default)
        {
            return new ValueTask<CustomerResponse>(new CustomerResponse
            {
                FirstName = "Vinh",
                LastName = "Le"
            });
        }
    }
}