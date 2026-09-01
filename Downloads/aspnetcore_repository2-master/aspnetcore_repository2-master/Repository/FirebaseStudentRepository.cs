using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using mvc; // Подключает класс Student, если он лежит напрямую в namespace mvc
using mvc.Repository;

namespace mvc.Repository
{
    public class FirebaseStudentRepository : IEntityRepository<Student>
    {
        private readonly FirebaseClient _firebaseClient;
        private const string CollectionName = "students";

        public FirebaseStudentRepository(Microsoft.Extensions.Configuration.IConfiguration config)
        {
            string databaseUrl = config["Firebase:DatabaseUrl"]
                ?? throw new InvalidOperationException("Firebase:DatabaseUrl не настроено в appsettings.json.");

            _firebaseClient = new FirebaseClient(databaseUrl);
        }

        // Реализация GetEntityListAsync
        public async Task<List<Student>> GetEntityListAsync()
        {
            try
            {
                var studentsDict = await _firebaseClient
                    .Child(CollectionName)
                    .OnceSingleAsync<Dictionary<string, Student>>();

                if (studentsDict == null) return new List<Student>();

                return studentsDict
                    .Where(pair => pair.Value != null)
                    .Select(pair =>
                    {
                        var student = pair.Value;
                        if (student.Id == 0 && int.TryParse(pair.Key, out int id))
                        {
                            student.Id = id;
                        }
                        return student;
                    }).ToList();
            }
            catch
            {
                var studentsList = await _firebaseClient
                    .Child(CollectionName)
                    .OnceSingleAsync<List<Student>>();

                if (studentsList == null) return new List<Student>();

                return studentsList.Where(s => s != null).ToList();
            }
        }

        // Реализация GetEntityAsync
        public async Task<Student> GetEntityAsync(int id)
        {
            var student = await _firebaseClient
                .Child(CollectionName)
                .Child(id.ToString())
                .OnceSingleAsync<Student>();

            if (student == null)
                throw new InvalidOperationException($"Студента с id {id} не найдено.");

            student.Id = id;
            return student;
        }

        // Реализация CreateAsync
        public async Task CreateAsync(Student item)
        {
            if (item.Id == 0)
            {
                var list = await GetEntityListAsync();
                item.Id = list.Any() ? list.Max(s => s.Id) + 1 : 1;
            }

            await _firebaseClient
                .Child(CollectionName)
                .Child(item.Id.ToString())
                .PutAsync(item);
        }

        // Реализация Update
        public void Update(Student item)
        {
            _firebaseClient
                .Child(CollectionName)
                .Child(item.Id.ToString())
                .PutAsync(item)
                .GetAwaiter()
                .GetResult();
        }

        // Реализация DeleteAsync
        public async Task DeleteAsync(int id)
        {
            await _firebaseClient
                .Child(CollectionName)
                .Child(id.ToString())
                .DeleteAsync();
        }

        // Реализация SaveChangesAsync
        public Task SaveChangesAsync()
        {
            return Task.CompletedTask;
        }
    }
}