﻿using System;
using System.Collections.Concurrent;
using System.IO;
using Microsoft.Extensions.Configuration;
using Telstra.Core.Api;
using Telstra.Core.Api.Tests.Builders;

namespace WCA.Consumer.Api.Tests.Builders
{
    public class ServiceBuilder
    {
        public ServiceBuilder()
        {
        }

        private static ConcurrentDictionary<Type, object> _types = new ConcurrentDictionary<Type, object>();
        public static IConfiguration configuration { get; set; }
        public static T GetType<T>(T _default = null) where T : class
        {
            if (_types.TryGetValue(typeof(T), out var OPT))
            {
                return (T)OPT;
            }
            else
            {
                if (_default != null)
                    _types.TryAdd(typeof(T), _default);
                return _default;
            }
        }

        public static ServiceBuilder init()
        {
            LaunchSettingsFixture.InitEnv();
            var rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            Console.WriteLine($"Environment - {Environment.GetEnvironmentVariable("WCA_ENVIRONMENT")}");

            var configs = new ConfigurationBuilder().SetBasePath(rootPath);
            configs.AddJsonFile("appsettings.json", true, true);
            configs.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("WCA_ENVIRONMENT")}.json", true);
            configs.AddEnvironmentVariables("WCA");
            configs.AddUserSecrets(typeof(Startup).Assembly);

            var configRoot = configs.Build();
            configuration = configRoot;

            return new ServiceBuilder();
        }

    }
}
