﻿using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Telstra.Core.Api.Tests.Builders
{
    public class LaunchSettingsFixture
    {
        public static void InitEnv()
        {
            if (Environment.GetEnvironmentVariable("WCA_ENVIRONMENT") != null)
                return;

            using (var file = File.OpenText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Properties", "launchSettings.json")))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);

                var variables = jObject
                    .GetValue("profiles")
                    //select a proper profile here
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }
        }
    }
}
