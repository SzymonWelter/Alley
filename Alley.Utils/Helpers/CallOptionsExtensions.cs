using Grpc.Core;

namespace Alley.Utils.Helpers
{
    public static class CallOptionsHelper
    {
        public static CallOptions Rewrite(ServerCallContext context)
        {
            return new CallOptions(
                context.RequestHeaders,
                context.Deadline,
                context.CancellationToken,
                context.WriteOptions,
                context.CreatePropagationToken()
                );
        }
    }
}