using System;
using Alley.Utils.Models;
using Grpc.Core;

namespace Alley.Utils.Helpers
{
    public static class SessionHelper
    {
        public static void HandleIfError(IResult result, IAlleyLogger logger) => HandleIfError(result, StatusCode.Cancelled, logger);

        private static void HandleIfError(IResult result, StatusCode statusCode, IAlleyLogger logger)
        {
            if (result.IsFailure)
            {
                logger.Error(result.Message);
                throw new RpcException(new Status(statusCode, result.Message));
            }
        }
    }
}