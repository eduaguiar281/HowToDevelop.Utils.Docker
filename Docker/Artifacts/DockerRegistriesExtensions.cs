using Docker.DotNet;

namespace HowToDevelop.Utils.Docker.Artifacts
{
    public static class DockerRegistriesExtensions
    {
        /// <summary>
        /// Adiciona o Registrador para SqlServer 2019
        /// </summary>
        /// <param name="testDockerRegistries"></param>
        /// <param name="dockerEngine"></param>
        /// <param name="settings"></param>
        public static void RegisterSqlServer2019(this DockerRegistries testDockerRegistries, DockerEngine dockerEngine, SqlServerDockerSettings settings)
        {
            testDockerRegistries.AddRegistry(new SqlServer2019Registry(dockerEngine, settings));
        }

        /// <summary>
        /// Adiciona o Registrador para SqlServer 2019
        /// </summary>
        /// <param name="testDockerRegistries"></param>
        /// <param name="dockerClient"></param>
        /// <param name="settings"></param>
        public static void RegisterSqlServer2019(this DockerRegistries testDockerRegistries, IDockerClient dockerClient, SqlServerDockerSettings settings)
        {
            var dockerEngine = new DockerEngine(dockerClient);
            testDockerRegistries.AddRegistry(new SqlServer2019Registry(dockerEngine, settings));
        }

        /// <summary>
        /// Adciona um registrador para o MongoDb
        /// </summary>
        /// <param name="testDockerRegistries"></param>
        /// <param name="dockerClient"></param>
        /// <param name="settings"></param>
        public static void RegisterMongoDb(this DockerRegistries testDockerRegistries, IDockerClient dockerClient,
            MongoDbDockerSettings settings)
        {
            var dockerEngine = new DockerEngine(dockerClient);
            testDockerRegistries.AddRegistry(new MongoDbRegistry(dockerEngine, settings));
        }

        /// <summary>
        /// Adciona um registrador para o MongoDb
        /// </summary>
        /// <param name="testDockerRegistries"></param>
        /// <param name="dockerEngine"></param>
        /// <param name="settings"></param>
        public static void RegisterMongoDb(this DockerRegistries testDockerRegistries, DockerEngine dockerEngine,
            MongoDbDockerSettings settings)
        {
            testDockerRegistries.AddRegistry(new MongoDbRegistry(dockerEngine, settings));
        }
    }
}
