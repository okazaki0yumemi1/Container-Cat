namespace Container_Cat.Containers.EngineAPI
{
    public static class DockerEngineAPIEndpoints
    {
        public const string Version = "version";
        public const string Events = "events";
        public class Containers
        {
            public const string BaseAddr = "containers";
            public const string GetAllContainers = BaseAddr + "/json";
            public const string GetContainerByID = BaseAddr + "/{id}/json";
            public const string GetContainerStats = BaseAddr + "/{id}/stats";
            public const string StartContainer = BaseAddr + "/{id}/start";
            public const string StopContainer = BaseAddr + "/{id}/stop";
            public const string RestartContainer = BaseAddr + "/{id}/restart";
            public Containers() { }
        }
        public class Images
        {
            public const string BaseAddr = "images";
            public const string ListImages = BaseAddr + "/json";
            public const string GetImageByName = BaseAddr + "/{name}/json";
        }
        public class Networks
        {
            public const string BaseAddr = "networks";
            public const string ListNetworks = BaseAddr;
            public const string GetNetworkByID = BaseAddr + "/{id}";
        }
    }
}
