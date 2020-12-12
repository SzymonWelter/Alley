using System;
using Alley.Utils.Models;
using Grpc.Core;

namespace Alley.Utils.Helpers
{
    public static class SessionHelper
    {
        public static void HandleIfError(IResult result) => HandleIfError(result, StatusCode.Cancelled);

        private static void HandleIfError(IResult result, StatusCode statusCode)
        {
            if (result.IsFailure)
            {
                throw new RpcException(new Status(statusCode, result.Message));
            }
        }
    }
}