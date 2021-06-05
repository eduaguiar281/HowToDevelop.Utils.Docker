using System.Collections.Generic;
using System.Threading.Tasks;

namespace HowToDevelop.Utils.Docker.Artifacts
{
    public class DockerRegistries
    {
        private readonly List<DockerRegistry> _registries = new List<DockerRegistry>();

        public void AddRegistry(DockerRegistry dockerRegistry)
        {
            _registries.Add(dockerRegistry);
        }

        public async Task RunAsync()
        {
            foreach (var register in _registries)
            {
                await register.DownloadImageAsync();
                await register.InstallContainerAsync();
                await register.HealthCheckContainerAsync();
            }
        }

        public async Task CleanAsync()
        {
            foreach (var register in _registries)
            {
                await register.CleanAsync();
            }
        }
    }
}
