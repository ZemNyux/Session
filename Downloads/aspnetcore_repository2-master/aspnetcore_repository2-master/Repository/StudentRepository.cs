using Microsoft.EntityFrameworkCore;

namespace mvc.Repository
{
    // конкретний репозиторій для роботи з даними студентів.
    // реалізує інтерфейс IRepository для абстракції доступу до БД.
    public class StudentRepository : IEntityRepository<Student>
    {
        // поле зберігає контекст БД (Entity Framework), який інжектується через DI.
        // потрібне для виконання всіх операцій з базою даних (читання, запис, тощо).
        // readonly — щоб контекст не можна було випадково замінити після створення репозиторію.
        private readonly StudentContext _context;

        public StudentRepository(StudentContext context) // Dependency Injection: контекст передається через конструктор
        {
            _context = context; // зберігає переданий контекст бази даних
        }

        public async Task<List<Student>> GetEntityListAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetEntityAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
                throw new InvalidOperationException($"Студента з айді {id} не знайдено.");
            return student;
        }

        public async Task CreateAsync(Student entity)
        {
            await _context.Students.AddAsync(entity);
        }

        public void Update(Student entity)
        {
            _context.Entry(entity).State = EntityState.Modified; // позначає сутність як змінену
        }

        public async Task DeleteAsync(int id)
        {
            Student? student = await _context.Students.FindAsync(id); // шукає студента для видалення
            if (student != null)
                _context.Students.Remove(student); // видаляє знайденого студента
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}