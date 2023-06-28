using Container_Cat.Containers.Models;
using Container_Cat.Utilities.Models;

namespace Container_Cat.Utilities.Interfaces
{
    public interface IHostSystemOperations<T>
        where T : BaseContainer
    {
        Task<int> InitialiseHostSystemsAsync(); //Full initialisation
        Task<HostAddress.HostAvailability> IsAPIAvailableAsync(HostAddress hostAddr); //Host adress availability check
        bool InitHostSystem(HostSystem<T> hostSystem);
    }
}
