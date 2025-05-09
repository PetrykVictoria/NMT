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
    public class QuestionController : Controller
    {
        private readonly DbNMTContext _context;

        public QuestionController(DbNMTContext context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index()
        {
            var dbNMTContext = _context.Questions.Include(q => q.QuestionType).Include(q => q.Topic);
            return View(await dbNMTContext.ToListAsync());
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.QuestionType)
                .Include(q => q.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            ViewData["QuestionTypeId"] = new SelectList(_context.QuestionTypes, "Id", "Name");
            ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name");

            var vm = new Question
            {
                AnswerOptions = new List<AnswerOption>
        {
            new AnswerOption(),
            new AnswerOption(),
            new AnswerOption(),
            new AnswerOption()
        }
            };
            return View(vm);
        }

        // POST: Questions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Question question)
        {
            // Відфільтрувати ті відповіді, де не введено текст
            if (question.AnswerOptions != null)
            {
                question.AnswerOptions = question.AnswerOptions
                    .Where(a => !string.IsNullOrWhiteSpace(a.OptionText))
                    .ToList();
            }

            if (ModelState.IsValid)
            {
                // EF Core сам підхопить навігацію і збереже і Question, і його AnswerOptions
                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Якщо модель невалідна — треба заново
            ViewData["QuestionTypeId"] = new SelectList(_context.QuestionTypes, "Id", "Name", question.QuestionTypeId);
            ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name", question.TopicId);
            return View(question);
        }


        // GET: Questions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
            .Include(q => q.AnswerOptions)
            .FirstOrDefaultAsync(q => q.Id == id);

            if (question == null)
            {
                return NotFound();
            }
            ViewData["QuestionTypeId"] = new SelectList(_context.QuestionTypes, "Id", "Name", question.QuestionTypeId);
            ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name", question.TopicId);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TopicId,QuestionTypeId,Difficulty,Text,Image,Hint")] Question question)
        {
            if (id != question.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.Id))
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
            ViewData["QuestionTypeId"] = new SelectList(_context.QuestionTypes, "Id", "Name", question.QuestionTypeId);
            ViewData["TopicId"] = new SelectList(_context.Topics, "Id", "Name", question.TopicId);
            return View(question);
        }

        // GET: Questions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.QuestionType)
                .Include(q => q.Topic)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.Id == id);
        }
    }
}
