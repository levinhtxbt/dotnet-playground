using System.ServiceModel;
using System.Threading.Tasks;
using ProtoBuf.Grpc;

namespace gRPCNet.MessageContract.Service
{
    [ServiceContract(Name = "Customer.Customer")]
    public interface ICustomerService
    {
        [OperationContract]
        ValueTask<CustomerResponse> GetAsync(CustomerRequest request, CallContext context = default);
    }
}