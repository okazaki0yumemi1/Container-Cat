using Container_Cat.Containers.EngineAPI.Models;
using Container_Cat.Containers.Models;
using Container_Cat.Data;
using Container_Cat.Utilities;
using Container_Cat.Utilities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static Container_Cat.Utilities.Models.HostAddress;

namespace Container_Cat.Controllers
{
    public class SystemsController : Controller
    {
        private readonly ContainerCatContext _context;
        private readonly HttpClient _httpClient;
        private SystemDataGathering _dataGatherer;

        public SystemsController(ContainerCatContext context, HttpClient client)
        {
            _context = context;
            _httpClient = client;
            _dataGatherer = new SystemDataGathering(_httpClient);
        }

        // GET: Systems
        public async Task<IActionResult> Index()
        {
            if (_context.SystemDataObj != null)
            {
                var results = await _context.SystemDataObj
                    .Include(host => host.Containers)
                    .Include(networks => networks.NetworkAddress)
                    .ToListAsync();
                return View(results);
            }
            else
                return Problem("Entity set 'ContainerCatContext.SystemDataObj'  is null.");
        }

        // GET: Systems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.SystemDataObj == null)
            {
                return NotFound();
            }
            var systemDataObj = await _context.SystemDataObj
                .Include(host => host.Containers)
                .ThenInclude(network => network.Ports)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (systemDataObj == null)
            {
                return NotFound();
            }
            return View(systemDataObj);
        }

        // GET: Systems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Systems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ip")] string hostname)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (hostname.IsNullOrEmpty()) return BadRequest();

            var hostStatus = await _dataGatherer.IsAPIAvailableAsync(hostname);
            if (hostStatus != HostAvailability.Connected) return BadRequest();

            HostSystemDTO hostSystemDTO = new HostSystemDTO();
            hostSystemDTO.InstalledContainerEngine = await _dataGatherer.ContainerEngineInstalledAsync(hostname);
            hostSystemDTO.NetworkAddress = new HostAddress(hostname);

            if (hostSystemDTO.InstalledContainerEngine == ContainerEngine.Docker)
            {
                //Get newContainers as BaseContainer:
                HostSystem<DockerContainer> dockerHost = new HostSystem<DockerContainer>(hostSystemDTO);
                var containers = await _dataGatherer.GetContainersAsync(dockerHost);
                _context.Add(dockerHost);
                hostSystemDTO.ConvertToBaseContainers(containers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else return RedirectToAction(nameof(Index));
        }

        // GET: Systems/Update/5
        public async Task<IActionResult> Update(Guid? id)
        {
            if (!ModelState.IsValid) return BadRequest();
            if (id == null) return BadRequest();

            var HostSystem = _context.HostSystems
                .Where(x => x.Id == id)
                .Include(x => x.Containers)
                .Include(x => x.NetworkAddress)
                .FirstOrDefault();
            if (HostSystem  == null)
            {
                return NotFound();
            }

            var connectionStatus = _dataGatherer
                .IsAPIAvailableAsync(HostSystem.NetworkAddress.Hostname)
                .Result;
            HostSystem .NetworkAddress.SetStatus(connectionStatus);
            if (connectionStatus != HostAvailability.Connected) return BadRequest();

            if (HostSystem .InstalledContainerEngine == ContainerEngine.Docker)
            {
                    //Get newContainers as BaseContainer:
                    HostSystem<DockerContainer> dockerHost = new HostSystem<DockerContainer>(HostSystem.NetworkAddress);
                    HostSystem.ReplaceToBaseContainers(await _dataGatherer.GetContainersAsync(dockerHost));
            }
            else if (HostSystem.InstalledContainerEngine == ContainerEngine.Podman)
            {
                    throw new NotImplementedException();
            }
            _context.Update(HostSystem);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.SystemDataObj == null)
            {
                return NotFound();
            }

            var systemDataObj = await _context.SystemDataObj.FindAsync(id);
            if (systemDataObj == null)
            {
                return NotFound();
            }

            return View(systemDataObj);
        }

        // POST: Systems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.SystemDataObj == null)
            {
                return Problem("Entity set 'ContainerCatContext.SystemDataObj'  is null.");
            }
            //Explicit loading of data for deletion:
            var HostSystem  = await _context.HostSystems 
                .Where(x => x.Id == id)
                //by id
                .Include(host => host.Containers)
                .ThenInclude(ports => ports.Ports)
                //related Container.Ports objects
                .Include(host => host.Containers)
                .ThenInclude(mounts => mounts.Mounts)
                //related Container.Mounts objects
                .Include(container => container.Containers)
                //related newContainers
                .Include(networks => networks.NetworkAddress)
                //related network info
                .FirstAsync();
            if (HostSystem  != null)
            {
                foreach (var container in HostSystem .Containers)
                {
                    _context.Mounts.RemoveRange(container.Mounts);
                    _context.Ports.RemoveRange(container.Ports);
                }
                _context.BaseContainer.RemoveRange(HostSystem.Containers);
                _context.HostAddresses.Remove(HostSystem.NetworkAddress);
                _context.HostSystems.Remove(HostSystem);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemDataObjExists(Guid id)
        {
            return (_context.SystemDataObj?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<int> StageDeleteContainerByID(string id)
        {
            var containerToRemove = await _context.BaseContainer
                .Include(x => x.Ports)
                .Include(x => x.Mounts)
                .FirstOrDefaultAsync();
            if (containerToRemove == null)
            {
                return 0;
            }
            _context.Remove(containerToRemove);
            return 1;
        }
    }
}
