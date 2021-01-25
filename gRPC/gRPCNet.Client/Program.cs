using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using gRPCNet.MessageContract;
using gRPCNet.MessageContract.Service;
using ProtoBuf.Grpc.Client;

namespace gRPCNet.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            GrpcClientFactory.AllowUnencryptedHttp2 = true;
            using (var channel = GrpcChannel.ForAddress("https://localhost:5001"))
            {
                var customerService = channel.CreateGrpcService<ICustomerService>();
                var result = await customerService.GetAsync(new CustomerRequest()
                {
                    CustomerNo = "123"
                });
                Console.WriteLine(result.FirstName);
                Console.WriteLine(result.LastName);
            }

            Console.ReadKey();
        }
    }
}
