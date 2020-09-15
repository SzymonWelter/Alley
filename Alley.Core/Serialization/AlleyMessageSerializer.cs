using Alley.Core.Models;
using Grpc.Core;

namespace Alley.Core.Serialization
{
    public static class AlleyMessageSerializer
    {

        public static readonly Marshaller<IAlleyMessageModel> AlleyMessageMarshaller = new Marshaller<IAlleyMessageModel>(Serialize, Deserialize);

        private static byte[] Serialize(IAlleyMessageModel alleyMessageModel)
        {
            return alleyMessageModel.Content;
        }

        private static IAlleyMessageModel Deserialize(byte[] alleyMessage)
        {
            return new AlleyMessageModel(alleyMessage);
        }
    }
}
