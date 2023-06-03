namespace Container_Cat.EngineAPI
{
    public class DockerEngineAPI
    {
        public readonly string Info = "version";
        public readonly string Events = "events";
        public class Containers
        {
            static readonly string BaseAddr = "containers";
            public readonly string GetAllContainers = BaseAddr + "/json";
            public readonly string GetContainerByID = BaseAddr + "/{id}/json";
            public readonly string GetContainerStats = BaseAddr + "/{id}/stats";
            public readonly string StartContainer = BaseAddr + "/{id}/start";
            public readonly string StopContainer = BaseAddr + "/{id}/stop";
            public readonly string RestartContainer = BaseAddr + "/{id}/restart";
            public Containers() { }
        }
        public class Images
        {
            static readonly string BaseAddr = "images";
            public readonly string ListImages = BaseAddr + "/json";
            public readonly string GetImageByName = BaseAddr + "/{name}/json";
        }
        public class Networks
        {
            static readonly string BaseAddr = "networks";
            public readonly string ListNetworks = BaseAddr;
            public readonly string GetNetworkByID = BaseAddr + "/{id}";
        }
        public DockerEngineAPI() { }
    }
}
