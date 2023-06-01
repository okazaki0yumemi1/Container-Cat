using Container_Cat.Podman_libpod_API.Models;
using Container_Cat.Utilities.Containers;
using Container_Cat.Utilities.Models.Models;

namespace Container_Cat.Podman_libpod_API
{
    public class PodmanContainerOperations : IContainerOperations<PodmanContainer>
    {
        public PodmanContainerOperations(HttpClient _client, HostAddress _nAddr) { }

        public Task<PodmanContainer> GetContainerByIDAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<PodmanContainer> GetContainerByNameAsync(string Name)
        {
            throw new NotImplementedException();
        }

        public Task<List<PodmanContainer>> ListContainersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> RestartContainerAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StartContainerAsync(string Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> StopContainerAsync(string Id)
        {
            throw new NotImplementedException();
        }
    }
}
