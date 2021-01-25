using System.Runtime.Serialization;
using ProtoBuf;
using ProtoBuf.Grpc;

namespace gRPCNet.MessageContract
{
    [ProtoContract]
    public class CustomerRequest
    {
        [ProtoMember(1)]
        public string CustomerNo { get; set; }

    }
}