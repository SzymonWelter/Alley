using Grpc.Core;

namespace Alley.Core.Utilities
{
    internal static class MethodTypeHelper
    {
        private static readonly MethodType[] MethodTypeMatrix =
        {
            MethodType.Unary, MethodType.ClientStreaming, 
            MethodType.ServerStreaming, MethodType.ServerStreaming
        };

        public static MethodType GetMethodType(bool clientStreaming, bool serverStreaming)
        {
            var x = clientStreaming ? 1 : 0;
            var y = serverStreaming ? 2 : 0;

            return MethodTypeMatrix[x + y];
        }
    }
}