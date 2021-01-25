using ProtoBuf;

namespace gRPCNet.MessageContract
{
    [ProtoContract]
    public class CustomerResponse
    {
        [ProtoMember(1)]
        public string FirstName { get; set; }

        [ProtoMember(2)]
        public string LastName { get; set; }
    }
}