using System;
using System.Collections.Generic;
using System.Text;
using Alley.Models;
using Grpc.Core;

namespace Alley
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
