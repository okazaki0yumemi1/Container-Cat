namespace Container_Cat.Containers.Models
{
    public interface IContainerOperations<T> where T : BaseContainer
    {
        //List, inspect are important to implement
        //start, stop, restart, kill - not sure if they are needed right now
        public Task<List<T>> ListContainersAsync();
        public Task<T> GetContainerByIDAsync(string Id);
        public Task<T> GetContainerByNameAsync(string Name);
        public Task<bool> StartContainerAsync(string Id);
        public Task<bool> StopContainerAsync(string Id);
        public Task<bool> RestartContainerAsync(string Id);


    }
}
