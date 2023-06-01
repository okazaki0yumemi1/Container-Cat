using Container_Cat.EngineAPI;
using Container_Cat.EngineAPI.Models;
using Container_Cat.Models;
using Container_Cat.Podman_libpod_API.Models;
using Container_Cat.Utilities;
using Container_Cat.Utilities.Linux;
using Container_Cat.Utilities.Models;
using Container_Cat.Utilities.Models.Models;
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
        public IActionResult Index()
        {
            SystemDataGathering dataGatherer = new SystemDataGathering();
            List<HostAddress> hosts = new List<HostAddress>()
                {new HostAddress("127.0.0.1", "3375"), new HostAddress("192.168.56.999", "3375"), new HostAddress("192.168.0.104", "3375"), new HostAddress("google.com", "80") };
            List<SystemDataObj> dataObj = new List<SystemDataObj>();
            foreach (var host in hosts) 
            {
                SystemDataObj item = new SystemDataObj(host);
                item.InstalledContainerEngines = dataGatherer.ContainerEngineInstalled(host);
                dataObj.Add(item);
            }
            var DockerHosts = dataObj
                .Where(item => item.InstalledContainerEngines == Utilities.Containers.ContainerEngine.Docker)
                .Select(host => host.NetworkAddress)
                .ToList();
            var PodmanHosts = dataObj
                .Where(item => item.InstalledContainerEngines == Utilities.Containers.ContainerEngine.Podman)
                .Select(host => host.NetworkAddress)
                .ToList();
            SystemOperations<DockerContainer> DockerSystems = new SystemOperations<DockerContainer>(DockerHosts);
            SystemOperations<PodmanContainer> PodmanSystems = new SystemOperations<PodmanContainer>(PodmanHosts);
            var systems = DockerSystems.GetHostSystems();
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