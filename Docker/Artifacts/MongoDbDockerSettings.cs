namespace HowToDevelop.Utils.Docker.Artifacts
{
    public class MongoDbDockerSettings : IDockerSettings
    {
        private const string MongoUri = @"mongodb://";
        private const int DefaultTimeWaitUntilDatabaseStartedInSeconds = 60;
        private const string DefaultDatabaseName = "DefaultDatabase";
        private const string DefaultHostName = "localhost";
        private const string DefaultMongoUser = "mainuser";
        private const string DefaultMongoPassword = "P4ssw0rd!";
        private const string DefaultImageName = "mongo";
        private const string DefaultImageTag = "4.0.24";
        private const string DefaultContainerPrefix = "MyProjectIntegrationTestsSql-";

        public static MongoDbDockerSettings Default =>
            new (DefaultImageName, DefaultImageTag, DefaultContainerPrefix);

        public MongoDbDockerSettings()
        {
            
        }

        public MongoDbDockerSettings(string dockerImageName, string dockerImageTag, string dockerContainerPrefix)
            :this(null, null, dockerImageName, dockerImageTag, dockerContainerPrefix, null, null, null)
        {
            
        }

        public MongoDbDockerSettings(string mongoUser, string mongoPassword, 
            string dockerImageName, string dockerImageTag, string dockerContainerPrefix, 
            string databaseName, int? waitUntil, int? port)
        {
            MongoUser = !string.IsNullOrEmpty(mongoUser) ? mongoUser : DefaultMongoUser;
            MongoPassword = !string.IsNullOrEmpty(mongoPassword) ? mongoPassword : DefaultMongoPassword;
            DockerImageName = dockerImageName;
            DockerImageTag = dockerImageTag;
            DockerContainerPrefix = dockerContainerPrefix;
            DatabaseName = !string.IsNullOrEmpty(databaseName) ? databaseName : DefaultDatabaseName;
            WaitUntil = waitUntil ?? DefaultTimeWaitUntilDatabaseStartedInSeconds;
            _port = port.HasValue ? port.ToString() : TcpPortSelector.GetFreePort().ToString();
        }

        /// <summary>
        /// Usuário do Mongo Db.
        /// </summary>
        public string MongoUser { get; set; }

        /// <summary>
        /// Senha do Mongo Db.
        /// </summary>
        public string MongoPassword { get; set; }

        /// <summary>
        /// Endereço para a imagem do docker a ser utilizado.
        /// Por exemplo: mongo
        /// </summary>
        public string DockerImageName { get; set; }

        /// <summary>
        /// Utilizado para determinar qual é a Tag da imagem do docker que desejamos baixar.
        /// Por exemplo: 4.0.24
        /// </summary>
        public string DockerImageTag { get; set; }

        /// <summary>
        /// Utilizado para facilitar na limpeza de containers durante a execução dos testes.
        /// Por exemplo: MyProjectIntegrationTestsSql
        /// </summary>
        public string DockerContainerPrefix { get; set; }

        /// <summary>
        /// Nome do Banco de dados MongoDb
        /// </summary>
        public string DatabaseName { get; set; }

        private string _port;
        /// <summary>
        /// Porta que o mongo deve utilizar no container.
        /// Caso null, será selecionada uma porta aleatória durante a criação do container.
        /// </summary>
        public string Port
        {
            get
            {
                if (string.IsNullOrEmpty(_port))
                {
                    _port = TcpPortSelector.GetFreePort().ToString();
                }
                return _port;
            }
            set
            {
                _port = value;
            }
        }

        /// <summary>
        /// Define quantidade de tempo a ser aguardando enquanto o banco de dados inicializa.
        /// </summary>
        public int WaitUntil { get; set; }

        public string GetConnectionString()
        {
            return $"{MongoUri}{MongoUser}:{MongoPassword}@{DefaultHostName}:{Port}";
        }


    }
}
