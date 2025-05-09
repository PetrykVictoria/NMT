// TestController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NMT.Models;          // для TestQuestionVM, AnswerVM

public class TestController : Controller
{
    private readonly DbNMTContext _db;
    public TestController(DbNMTContext db) => _db = db;

    // GET: /Test/Start?topicId=5
    [HttpGet]
    public IActionResult Start(int topicId)
    {
        var all = _db.Questions
            .Include(q => q.AnswerOptions)
            .Where(q => q.TopicId == topicId)
            .ToList();

        var chosen = all
            .OrderBy(_ => Guid.NewGuid())
            .Take(10)
            .Select(q => new TestQuestionVM
            {
                QuestionId = q.Id,
                Text = q.Text,
                Answers = q.AnswerOptions
                    .Select(a => new AnswerVM
                    {
                        Id = a.Id,
                        Text = a.OptionText,
                        IsCorrect = a.IsCorrect
                    }).ToList()
            })
            .ToList();

        HttpContext.Session.SetObject("CurrentTest", chosen);
        // очищаємо попередні відповіді
        HttpContext.Session.Remove("TestAnswers");

        return RedirectToAction("Take", new { index = 0 });
    }

    // GET: /Test/Take?index=0
    [HttpGet]
    public IActionResult Take(int index)
    {
        var questions = HttpContext.Session.GetObject<List<TestQuestionVM>>("CurrentTest");
        if (questions == null || index < 0 || index >= questions.Count)
            return RedirectToAction("Result");

        ViewBag.Index = index;
        ViewBag.Total = questions.Count;
        return View(questions[index]);
    }

    // POST: /Test/Take
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Take(int index, int selectedOptionId)
    {
        // зчитуємо вже збережені відповіді (index → answerId)
        var answers = HttpContext.Session.GetObject<Dictionary<int, int>>("TestAnswers")
                      ?? new Dictionary<int, int>();

        answers[index] = selectedOptionId;
        HttpContext.Session.SetObject("TestAnswers", answers);

        var questions = HttpContext.Session.GetObject<List<TestQuestionVM>>("CurrentTest");
        if (questions == null) return RedirectToAction("Start");

        if (index + 1 < questions.Count)
            return RedirectToAction("Take", new { index = index + 1 });
        else
            return RedirectToAction("Result");
    }

    // GET: /Test/Result
    [HttpGet]
    public IActionResult Result()
    {
        // Дістаємо з сесії питання й відповіді
        var questions = HttpContext.Session.GetObject<List<TestQuestionVM>>("CurrentTest")
                        ?? new List<TestQuestionVM>();
        var answers = HttpContext.Session.GetObject<Dictionary<int, int>>("TestAnswers")
                      ?? new Dictionary<int, int>();

        // Будуємо List<TestResultVM>
        var results = questions.Select((q, index) =>
        {
            // вибрана відповідь для цього питання
            answers.TryGetValue(index, out var selId);

            return new TestResultVM
            {
                QuestionId = q.QuestionId,
                Text = q.Text,
                Answers = q.Answers,
                SelectedAnswerId = selId,
                IsCorrect = q.Answers.Any(a => a.Id == selId && a.IsCorrect)

            };
        }).ToList();

        // Тепер повертаємо цей список у View
        return View(results);
    }
}
