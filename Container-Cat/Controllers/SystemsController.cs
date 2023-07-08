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
            var results = await _context.HostSystemBase
                                .Include(networks => networks.NetworkAddress)
                                .Include(x => x.Containers)
                                .ToListAsync();
            return View(results);
        }

        // GET: Systems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.HostSystemBase == null)
            {
                return NotFound();
            }
            
            //Get host without containers:

            var result = await _context.HostSystemBase
                .Include(x => x.Containers)
                .Include(x => x.Containers).ThenInclude(y => y.Ports)
                .Include(x => x.Containers).ThenInclude(y => y.Mounts)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (result == null)
            {
                return NotFound();
            }
            
            //Create host obj with containers
            HostSystemDTO hostDTO = new HostSystemDTO(result);
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
                HostSystem<BaseContainer> host = new HostSystem<BaseContainer>(hostSystemDTO);
                var containers = await _dataGatherer.GetContainersAsync(host);
                host.Containers.AddRange(containers);

                //Add HostSystem & containers:
                _context.Add(host);

                //Save changes:
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

            var systemEntity = await _context.HostSystemBase
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (systemEntity  == null)
            {
                return NotFound();
            }
            //Delete, then create new

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.HostSystemBase == null)
            {
                return NotFound();
            }

            var result = await _context.HostSystemBase.FindAsync(id);
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
            if ((_context.HostSystemBase == null) || (_context.BaseContainers == null))
            {
                return Problem("Entity sets are null.");
            }
            //Explicit loading of data for deletion:
            var systemEntity  = await _context.HostSystemBase 
                .Where(x => x.Id == id).FirstOrDefaultAsync();
            
            //Explicit loading of HostSystem & containers
            var hostSystem = await _context.HostSystemBase.Where(x => x.Id == id)
                .Include(items => items.Containers)
                .Include(containers => containers.Containers).ThenInclude(ports => ports.Ports)
                .Include(containers => containers.Containers).ThenInclude(mounts => mounts.Mounts)
                .FirstOrDefaultAsync();

            if (hostSystem != null)
            {
                foreach (var container in hostSystem.Containers)
                {
                    _context.RemoveRange(container.Mounts);
                    _context.RemoveRange(container.Ports);
                }
                _context.BaseContainers.RemoveRange(hostSystem.Containers);
                _context.HostAddresses.Remove(hostSystem.NetworkAddress);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemDataObjExists(Guid id)
        {
            return (_context.HostSystemBase?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<int> StageDeleteContainerByID(string id)
        {
            var containerToRemove = await _context.BaseContainers
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
