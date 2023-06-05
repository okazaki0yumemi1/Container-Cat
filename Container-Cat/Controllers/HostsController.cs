using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Container_Cat.Utilities;
using Container_Cat.Utilities.Models;
using Container_Cat.Containers.Models;

namespace Container_Cat.Controllers
{
    [Route("[controller]")]
    public class HostsController : Controller
    {
        private readonly SystemDataGathering _dataGatherer;
        private readonly SystemOperations<BaseContainer> _GenericSystems;
        private readonly List<HostAddress> _Hosts;
        public HostsController(SystemDataGathering dataGatherer, List<HostAddress> Hosts, SystemOperations<BaseContainer> GenericSystems)
        {
            _dataGatherer = dataGatherer;
            _Hosts = Hosts;
            _GenericSystems = GenericSystems;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
        public IActionResult AddHost()
        {
            return View("AddHost");
        }
        public IActionResult HostSystems()
        {
            return View("HostSystems");
        }
        /*
            SystemDataGathering dataGatherer = new SystemDataGathering();
            SystemOperations<DockerContainer> DockerSystems = new SystemOperations<DockerContainer>();
            SystemOperations<PodmanContainer> PodmanSystems;
            List<HostAddress> Hosts = new List<HostAddress>()
            {
                new HostAddress("127.0.0.1", "3375"),
                new HostAddress("192.168.56.99", "3375"),
                new HostAddress("192.168.0.104", "3375"),
                new HostAddress("google.com", "80") 
            };
            await foreach (var dataObj in dataGatherer.FetchDataObjectRangeAsync(Hosts))
            {
                if (dataObj.InstalledContainerEngines == ContainerEngine.Docker)
                {
                    DockerSystems.AddHostSystem(new HostSystem<DockerContainer>(dataObj));
                    Console.WriteLine($"{dataObj.NetworkAddress.Ip}:{dataObj.NetworkAddress.Port} is {dataObj.InstalledContainerEngines}.");
                    Console.WriteLine($"There are {DockerSystems.SystemsCount()} systems with Docker.");
                }
                else if (dataObj.InstalledContainerEngines == ContainerEngine.Podman)
                {
                    throw new NotImplementedException("Podman is not implemented.");
                } 
            }
        */
    }
}