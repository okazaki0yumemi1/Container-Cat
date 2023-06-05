using Container_Cat.Containers.EngineAPI.Models;
using Container_Cat.Containers.Models;
using Container_Cat.Models;
using Container_Cat.PodmanAPI.Models;
using Container_Cat.Utilities;
using Container_Cat.Utilities.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Container_Cat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
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
            //var tasks = Hosts.Select(async host =>
            //{
            //    SystemDataObj item = new SystemDataObj(host);
            //    item.InstalledContainerEngines = await dataGatherer.ContainerEngineInstalledAsync(host);
            //    dataObj.Add(item);
            //});
            //await Task.WhenAll(tasks);
            //var DockerHosts = dataObj
            //    .Where(item => item.InstalledContainerEngines == ContainerEngine.Docker)
            //    .Select(host => host.NetworkAddress)
            //    .ToList();
            //var PodmanHosts = dataObj
            //    .Where(item => item.InstalledContainerEngines == ContainerEngine.Podman)
            //    .Select(host => host.NetworkAddress)
            //    .ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}