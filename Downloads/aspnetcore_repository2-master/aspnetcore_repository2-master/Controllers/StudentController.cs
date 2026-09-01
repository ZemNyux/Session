using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc.Repository;

namespace mvc.Controllers
{
    public class StudentController : Controller
    {
        // поле для зберігання посилання на репозиторій, який реалізує інтерфейс IRepository.
        // використання інтерфейсу замість конкретної реалізації дозволяє легко замінити репозиторій
        // в майбутньому (наприклад, на mock для тестів або на іншу реалізацію з іншою логікою/БД),
        // без зміни коду контролера – це ключовий принцип Dependency Inversion (D у SOLID)!
        IEntityRepository<Student> repo;

        // конструктор з ін'єкцією залежності: ASP.NET Core DI-контейнер автоматично передасть
        // зареєстровану (в програм.цс) реалізацію IRepository. це робить контролер незалежним від деталей
        // створення репозиторію і спрощує юніт-тестування (можна буде передати mock-об'єкт)
        public StudentController(IEntityRepository<Student> r)
        {
            repo = r;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            // отримуємо список студентів через інтерфейс репозиторію.
            // профіт: якщо завтра потрібно змінити спосіб отримання списку
            // (з кешу, відфільтровані дані, результат мережевого запиту, інша БД), достатньо змінити лише реалізацію IRepository,
            // контролер залишиться незмінним!
            var model = await repo.GetEntityListAsync();
            return View(model);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || await repo.GetEntityListAsync() == null)
            {
                return NotFound();
            }

            // отримуємо конкретного студента за id через репозиторій.
            // використання інтерфейсу дає гнучкість: реалізація може сама вирішувати,
            // брати дані з контексту БД, кешу чи зовнішнього API
            var student = await repo.GetEntityAsync((int)id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Age,GPA")] Student student)
        {
            if (ModelState.IsValid)
            {
                // додаємо новий об'єкт через репозиторій.
                // профіт від інтерфейсу: логіка створення (валідація, генерація Id тощо)
                // може бути винесена в окремий клас, а не дублюватися в контролері
                await repo.CreateAsync(student);    
                await repo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || await repo.GetEntityListAsync() == null)
            {
                return NotFound();
            }
            var student = await repo.GetEntityAsync((int)id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Age,GPA")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    // оновлення через репозиторій – вся робота з Entity Framework
                    // захована в реалізації IRepository. це зменшує зв'язність контролера
                    // з конкретним DbContext і полегшує підтримку/рефакторинг.
                    repo.Update(student);
                    await repo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await StudentExists(student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || await repo.GetEntityListAsync() == null)
            {
                return NotFound();
            }
            var student = await repo.GetEntityAsync((int)id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await repo.GetEntityListAsync() == null)
            {
                return Problem("Набір сутностей 'StudentContext.Students' порожній.");
            }
            var student = await repo.GetEntityAsync(id);

            // видалення також через інтерфейс репозиторію.
            // в майбутньому можна додати логіку (наприклад, м'яке видалення, логування),
            // змінивши лише реалізацію, а не контролер.
            if (student != null)
            {
                await repo.DeleteAsync(id);
            }
            await repo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> StudentExists(int id)
        {
            List<Student> list = await repo.GetEntityListAsync();
            return (list?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}