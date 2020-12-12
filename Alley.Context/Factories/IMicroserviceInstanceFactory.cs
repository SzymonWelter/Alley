using System;
using System.Collections.Generic;
using Alley.Context.Models.Interfaces;

namespace Alley.Context.Factories
{
    public interface IMicroserviceInstanceFactory
    {
        IMicroserviceInstance Create(string microserviceName, Uri uri);
    }
}