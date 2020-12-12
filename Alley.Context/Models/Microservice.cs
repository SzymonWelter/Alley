using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Alley.Context.Models.Interfaces;
using Alley.Utils;
using Alley.Utils.Models;

namespace Alley.Context.Models
{
    public class Microservice : IMicroservice
    {
        private readonly IDictionary<Uri, IMicroserviceInstance> _instances;
        public string Name { get; }
        public IEnumerable<string> ServiceNames { get; }

        public Microservice(string name) : this(
            name,
            new List<string>(),
            new ConcurrentDictionary<Uri, IMicroserviceInstance>())
        { }
        
        public Microservice(
                string name,
                IEnumerable<string> serviceNames) : 
            this(
                name,
                serviceNames,
                new ConcurrentDictionary<Uri, IMicroserviceInstance>())
        { }

        public Microservice(
            string name,
            IEnumerable<string> serviceNames,
            IDictionary<Uri, IMicroserviceInstance> instances 
            )
        {
            _instances = instances;
            Name = name;
            ServiceNames = serviceNames;
        }


        public IResult RegisterInstance(IMicroserviceInstance microServiceInstance)
        {
            var uri = microServiceInstance?.Uri;
            return uri != null && _instances.TryAdd(uri, microServiceInstance) ? 
                Result.Success(Messages.InstanceSuccessfullyRegistered(uri)) : 
                Result.Failure(Messages.CanNotRegisterInstance(microServiceInstance));
        }

        public IResult UnregisterInstance(Uri instanceUri)
        {
            if (instanceUri == null)
            {
                return Result.Failure(Messages.CanNotUnregisterInstanceWithNullUri(Name));
            }

            if (!_instances.ContainsKey(instanceUri))
            {
                return Result.Failure(Messages.CanNotUnregisterNotExistingInstance(instanceUri, Name));
            }
            _instances.Remove(instanceUri);
            return Result.Success(Messages.InstanceSuccessfullyUnregistered(instanceUri));
        }

        public IResult<IEnumerable<Uri>> UnregisterAllInstances()
        {
            var uris = _instances.Keys;
            _instances.Clear();
            return Result<IEnumerable<Uri>>.Success(uris);
        }

        public IEnumerable<IReadonlyMicroserviceInstance> GetInstances()
        {
            return _instances.Values;
        }
    }
}