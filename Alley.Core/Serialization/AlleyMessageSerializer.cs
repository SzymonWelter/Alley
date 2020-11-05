using Grpc.Core;
using Alley.Definitions.Models;
using Alley.Definitions.Models.Interfaces;

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
