using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Container_Cat.Containers.Models;
using Container_Cat.Data;

namespace Container_Cat.Controllers
{
    public class BaseContainersController : Controller
    {
        private readonly ContainerCatContext _context;

        public BaseContainersController(ContainerCatContext context)
        {
            _context = context;
        }

        // GET: BaseContainers
        public async Task<IActionResult> Index()
        {
              return _context.BaseContainer != null ? 
                          View(await _context.BaseContainer.ToListAsync()) :
                          Problem("Entity set 'ContainerCatContext.BaseContainer'  is null.");
        }

        // GET: BaseContainers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.BaseContainer == null)
            {
                return NotFound();
            }

            var baseContainer = await _context.BaseContainer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baseContainer == null)
            {
                return NotFound();
            }

            return View(baseContainer);
        }

        // GET: BaseContainers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BaseContainers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,State,Names,Image")] BaseContainer baseContainer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(baseContainer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(baseContainer);
        }

        // GET: BaseContainers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.BaseContainer == null)
            {
                return NotFound();
            }

            var baseContainer = await _context.BaseContainer.FindAsync(id);
            if (baseContainer == null)
            {
                return NotFound();
            }
            return View(baseContainer);
        }

        // POST: BaseContainers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,State,Names,Image")] BaseContainer baseContainer)
        {
            if (id != baseContainer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(baseContainer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BaseContainerExists(baseContainer.Id))
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
            return View(baseContainer);
        }

        // GET: BaseContainers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.BaseContainer == null)
            {
                return NotFound();
            }

            var baseContainer = await _context.BaseContainer
                .FirstOrDefaultAsync(m => m.Id == id);
            if (baseContainer == null)
            {
                return NotFound();
            }

            return View(baseContainer);
        }

        // POST: BaseContainers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.BaseContainer == null)
            {
                return Problem("Entity set 'ContainerCatContext.BaseContainer'  is null.");
            }
            var baseContainer = await _context.BaseContainer.FindAsync(id);
            if (baseContainer != null)
            {
                _context.BaseContainer.Remove(baseContainer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BaseContainerExists(string id)
        {
          return (_context.BaseContainer?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
