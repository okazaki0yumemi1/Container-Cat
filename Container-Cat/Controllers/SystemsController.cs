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
            var hosts = await _context.SystemEntities
                                .Include(networks => networks.NetworkAddress)
                                .ToListAsync();

            return View(hosts);
        }

        // GET: Systems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.SystemEntities == null)
            {
                return NotFound();
            }
            
            //Get host without containers:

            var result = await _context.SystemEntities
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (result == null)
            {
                return NotFound();
            }
            
            //Create host obj with containers
            HostSystemDTO hostDTO = new HostSystemDTO(result);

            if (result.InstalledContainerEngine == ContainerEngine.Docker)
            {
                //Get host containers (Docker)
                var containers = _context.DockerContainers.Where(x => result.ContainerIDs.Contains(x.objId)).ToList<DockerContainer>();
                //Convert Docker to Base
                hostDTO.ConvertToBaseContainers(containers);
            }

            return View(hostDTO);
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
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));
            if (hostname.IsNullOrEmpty()) return RedirectToAction(nameof(Index));

            var hostStatus = await _dataGatherer.IsAPIAvailableAsync(hostname);
            if (hostStatus != HostAvailability.Connected) return RedirectToAction(nameof(Index));

            HostSystemDTO hostSystemDTO = new HostSystemDTO();
            hostSystemDTO.InstalledContainerEngine = await _dataGatherer.ContainerEngineInstalledAsync(hostname);
            hostSystemDTO.NetworkAddress = new HostAddress(hostname);

            if (hostSystemDTO.InstalledContainerEngine == ContainerEngine.Docker)
            {
                //Get newContainers as BaseContainer:
                HostSystem<DockerContainer> dockerHost = new HostSystem<DockerContainer>(hostSystemDTO);
                var containers = await _dataGatherer.GetContainersAsync(dockerHost);
                _context.Add(containers);
                //Get containers IDs
                var containerIds = containers.Select(x => x.objId).ToList<string>();
                //Put the into SystemObject:
                SystemEntity hostObj = new SystemEntity(hostSystemDTO.NetworkAddress);
                hostObj.ContainerIDs.AddRange(containers.Select(x => x.objId).ToList<string>());
                _context.Add(hostObj);
                //hostSystemDTO.ConvertToBaseContainers(containers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Systems/Update/5
        public async Task<IActionResult> Update(Guid? id)
        {
            if (!ModelState.IsValid) return RedirectToAction(nameof(Index));
            if (id == null) return RedirectToAction(nameof(Index));

            var systemEntity = await _context.SystemEntities
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (systemEntity  == null)
            {
                return NotFound();
            }
            //Delete, then create new

//            _context.Update(HostSystem);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.SystemEntities == null)
            {
                return NotFound();
            }

            var result = await _context.SystemEntities.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        // POST: Systems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if ((_context.SystemEntities == null) || (_context.DockerContainers == null))
            {
                return Problem("Entity sets are null.");
            }
            //Explicit loading of data for deletion:
            var hostSystem  = await _context.SystemEntities 
                .Where(x => x.Id == id).FirstOrDefaultAsync();
                //by id
            var containers = await _context.DockerContainers.Where(x => hostSystem.ContainerIDs.Contains(x.objId))
                .Include(ports => ports.Ports)
                //related Container.Ports objects
                .Include(mounts => mounts.Mounts)
                //related Container.Mounts objects
                .ToListAsync();
            if (hostSystem != null)
            {
                foreach (var container in containers)
                {
                    _context.RemoveRange(container.Mounts);
                    _context.RemoveRange(container.Ports);
                }
                _context.DockerContainers.RemoveRange(containers);
                _context.HostAddresses.Remove(hostSystem.NetworkAddress);
                _context.SystemEntities.Remove(hostSystem);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemDataObjExists(Guid id)
        {
            return (_context.SystemEntities?.Any(e => e.Id == id)).GetValueOrDefault();
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
