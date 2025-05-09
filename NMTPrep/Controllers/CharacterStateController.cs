using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NMT.Models;

namespace NMT.Controllers
{
    public class CharacterStateController : Controller
    {
        private readonly DbNMTContext _context;

        public CharacterStateController(DbNMTContext context)
        {
            _context = context;
        }

        // GET: CharacterStates
        public async Task<IActionResult> Index()
        {
            var dbNMTContext = _context.CharacterStates.Include(c => c.User);
            return View(await dbNMTContext.ToListAsync());
        }

        // GET: CharacterStates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterState = await _context.CharacterStates
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (characterState == null)
            {
                return NotFound();
            }

            return View(characterState);
        }

        // GET: CharacterStates/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: CharacterStates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Hat,Outfit,Background,Accessory")] CharacterState characterState)
        {
            if (ModelState.IsValid)
            {
                _context.Add(characterState);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", characterState.UserId);
            return View(characterState);
        }

        // GET: CharacterStates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterState = await _context.CharacterStates.FindAsync(id);
            if (characterState == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", characterState.UserId);
            return View(characterState);
        }

        // POST: CharacterStates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Hat,Outfit,Background,Accessory")] CharacterState characterState)
        {
            if (id != characterState.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(characterState);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CharacterStateExists(characterState.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", characterState.UserId);
            return View(characterState);
        }

        // GET: CharacterStates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var characterState = await _context.CharacterStates
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (characterState == null)
            {
                return NotFound();
            }

            return View(characterState);
        }

        // POST: CharacterStates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var characterState = await _context.CharacterStates.FindAsync(id);
            if (characterState != null)
            {
                _context.CharacterStates.Remove(characterState);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CharacterStateExists(int id)
        {
            return _context.CharacterStates.Any(e => e.Id == id);
        }
    }
}
