using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HowToDevelop.Utils.Docker.Artifacts
{
    public interface IDockerSettings
    {
        /// <summary>
        /// Endereço para a imagem do docker a ser utilizado.
        /// Por exemplo: mongo
        /// </summary>
        string DockerImageName { get; set; }

        /// <summary>
        /// Utilizado para determimnar qual é a Tag da imagem do docker que desejamos baixar.
        /// Por exemplo: 4.0.24
        /// </summary>
        string DockerImageTag { get; set; }

    }
}
