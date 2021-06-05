using Docker.DotNet.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HowToDevelop.Utils.Docker.Artifacts
{
    public class MongoDbRegistry : DockerRegistry
    {
        private readonly MongoDbDockerSettings _settings;
        private readonly DockerImageInfo _dockerImageInfo;

        public MongoDbRegistry(DockerEngine dockerEngine, MongoDbDockerSettings settings) : base(dockerEngine)
        {
            _settings = settings;
            _dockerImageInfo = DockerImageInfo.New(_settings);
        }


        public override async Task DownloadImageAsync()
        {
            await _dockerEngine.DownloadImageAsync(_dockerImageInfo);
        }

        public override async Task InstallContainerAsync()
        {
            await _dockerEngine.RemoveContainersByPrefixAsync(_settings.DockerContainerPrefix);
            await _dockerEngine.CreateImageAsync(_dockerImageInfo);
            var containerId = await _dockerEngine.EnsureCreateAndStartContainer(MongoParameters(_dockerImageInfo));
            StoreContainerId(containerId);
        }


        private CreateContainerParameters MongoParameters(DockerImageInfo dockerImageInfo)
        {
            return new CreateContainerParameters
            {
                Name = _settings.DockerContainerPrefix + Guid.NewGuid(),
                Image = dockerImageInfo.Image,
                Env = new List<string>
                {
                    $"MONGO_INITDB_ROOT_USERNAME={_settings.MongoUser}",
                    $"MONGO_INITDB_ROOT_PASSWORD={_settings.MongoPassword}"
                },
                HostConfig = new HostConfig
                {
                    PortBindings = new Dictionary<string, IList<PortBinding>>
                    {
                        {
                            "27017/tcp",
                            new PortBinding[]
                            {
                                new PortBinding
                                {
                                    HostPort = _settings.Port
                                }
                            }
                        }
                    }
                }
            };
        }

        public override async Task HealthCheckContainerAsync()
        {
            await WaitUntilDatabaseAvailableAsync(
                _settings.WaitUntil,
                _settings.GetConnectionString()
            );
        }

        private async Task WaitUntilDatabaseAvailableAsync(int maxWaitTimeInSeconds, string connectionString)
        {
            var start = DateTime.UtcNow;
            var connectionEstablished = false;

            while (!connectionEstablished && start.AddSeconds(maxWaitTimeInSeconds) > DateTime.UtcNow)
            {
                try
                {
                    var client = new MongoClient(connectionString);
                    var database = client.GetDatabase(_settings.DatabaseName);
                    await database.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
                    connectionEstablished = true;
                }
                catch
                {
                    // If opening the SQL connection fails, SQL Server is not ready yet
                    await Task.Delay(500);
                }
            }

            if (!connectionEstablished)
            {
                throw new Exception($"Connection to the MongoDb docker database could not be established within {maxWaitTimeInSeconds} seconds.");
            }
        }
    }
}

