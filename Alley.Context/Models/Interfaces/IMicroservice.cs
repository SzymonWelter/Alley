using System;
using System.Collections;
using System.Collections.Generic;
using Alley.Utils.Models;

namespace Alley.Context.Models.Interfaces
{
    public interface IMicroservice : IReadonlyMicroservice
    {
        string Name { get; }
        IEnumerable<string> ServiceNames { get; }
        IResult RegisterInstance(IMicroserviceInstance microServiceInstance);
        IResult UnregisterInstance(Uri instanceUri);
        IResult<IEnumerable<Uri>> UnregisterAllInstances();

    }

    public interface IReadonlyMicroservice
    {
        IEnumerable<IReadonlyMicroserviceInstance> GetInstances();
    }
}