using System;

namespace HowToDevelop.Utils.Docker.Artifacts
{
    public class DockerImageInfo
    {
        private DockerImageInfo(string name, string tag)
        {
            Name = name;
            Tag = tag;
        }

        public string Name { get; private set; }

        public string Tag { get; private set; }

        public string Image
        {
            get
            {
                if (string.IsNullOrEmpty(Tag))
                    return Name;
                return $"{Name}:{Tag}";
            }
        }

        public static DockerImageInfo New(string name, string tag)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (string.IsNullOrEmpty(tag))
                throw new ArgumentNullException(nameof(tag));

            return new DockerImageInfo(name, tag);
        }

        public static DockerImageInfo New(IDockerSettings dockerSettings)
        {
            return new DockerImageInfo(
                dockerSettings.DockerImageName,
                dockerSettings.DockerImageTag
            );
        }
    }
}
