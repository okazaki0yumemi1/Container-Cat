using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Container_Cat.Data;
using Container_Cat.Utilities.Models;
using System.Net;
using Container_Cat.Containers.Models;
using Container_Cat.Utilities;
using Container_Cat.Containers.EngineAPI.Models;
using Container_Cat.Containers.EngineAPI;

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
              return _context.SystemDataObj != null ? 
                          View(await _context.SystemDataObj.ToListAsync()) :
                          Problem("Entity set 'ContainerCatContext.SystemDataObj'  is null.");
        }

        // GET: Systems/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.SystemDataObj == null)
            {
                return NotFound();
            }

            var systemDataObj = await _context.SystemDataObj
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
        public async Task<IActionResult> Create([Bind("Ip")] string Ip)
        {
            HostAddress host = new HostAddress(Ip);
            SystemDataObj systemDataObj = new SystemDataObj(host);
            if (ModelState.IsValid)
            {
                //_context.Add(systemDataObj);
                HostSystem<BaseContainer> systemObj = new HostSystem<BaseContainer>(systemDataObj);
                //await _context.SaveChangesAsync();
                if (systemObj.InstalledContainerEngine != ContainerEngine.Unknown)
                {
                    //Get containers as BaseContainer:
                    HostSystem<DockerContainer> dockerHost = new HostSystem<DockerContainer>(systemDataObj);
                    var containers = await _dataGatherer.GetContainersAsync(dockerHost);
                    systemDataObj.AddBaseContainers(containers);
                    _context.Add(systemDataObj);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else return RedirectToAction(nameof(Index));
            }
            else return RedirectToAction(nameof(Index));

        }
        // GET: Systems/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
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

        // POST: Systems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,InstalledContainerEngines")] SystemDataObj systemDataObj)
        {
            if (id != systemDataObj.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(systemDataObj);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SystemDataObjExists(systemDataObj.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(systemDataObj);
        }

        // GET: Systems/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.SystemDataObj == null)
            {
                return NotFound();
            }

            var systemDataObj = await _context.SystemDataObj
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var systemDataObj = await _context.SystemDataObj.FindAsync(id);
            if (systemDataObj != null)
            {
                _context.SystemDataObj.Remove(systemDataObj);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SystemDataObjExists(Guid id)
        {
          return (_context.SystemDataObj?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
