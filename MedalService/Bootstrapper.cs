namespace MedalService
{
    using System.Linq;
    using MongoDB.Driver;
    using MongoDB.Driver.Linq;
    using MongoDB.Driver.Builders;
    using Nancy;
    using System;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson;
    using MedalService.Models;
    using MedalService.Services;
    using MedalService.Services.Impl;

    
    /// <summary>
    /// Bootstraps Nancy WebFramework.
    /// http://nancyfx.org/
    /// </summary>
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);

            //Registers MedalStateServiceMongoDB as IMedalStateService
            container.Register<IMedalStatService>(new MedalStatServiceMongoDB());
        }
    }
}