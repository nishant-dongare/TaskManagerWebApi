using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Taskmanager_MVCAPI.Data;
using Taskmanager_MVCAPI.Exceptions;
using Taskmanager_MVCAPI.Models;
using Taskmanager_MVCAPI.Repo;

namespace Taskmanager_MVCAPI.Service
{
    // Services/NewTaskService.cs
    public class NewTaskService : INewTaskRepository
    {
        private readonly ApplicationDbContext db;

        public NewTaskService(ApplicationDbContext context)
        {
            db = context;
        }
         
        public async Task<IEnumerable<NewTask>> GetAllAsync()
        {
            return await db.NewTasks.ToListAsync();
        }

        public async Task<NewTask> GetByIdAsync(int id)
        {
            var newTask = await db.NewTasks.FindAsync(id); 
            if(newTask == null)
            {
                throw new NotFoundException($"NewTask with id {id} not found");
            }
            return newTask;
        }

        public async Task<NewTask> GetByIdWithDetailsAsync(int taskId)
        {
            return await db.NewTasks.Include(t => t.TaskAssignments).ThenInclude(ta => ta.Student).FirstOrDefaultAsync(t => t.TaskId == taskId);
        }

        public async Task<NewTask> AddAsync(NewTask newTask)
        {
            if (string.IsNullOrWhiteSpace(newTask.TaskDescription))
            {
                throw new ValidationException("Task description is required");
            }
            newTask.CreatedDate = DateTime.UtcNow;
            db.NewTasks.Add(newTask);
            await db.SaveChangesAsync();
            return newTask;
        }

        public async void UpdateAsync(NewTask newTask)
        {
            db.Entry(newTask).State = EntityState.Modified;
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await NewTaskExistsAsync(newTask.TaskId))
                {
                    throw new NotFoundException($"NewTask with id {newTask.TaskId} not found");
                }
                else
                {
                    throw;
                }
            }
        }

        public async void DeleteAsync(int id)
        {
            var newTask = await db.NewTasks.FindAsync(id);
            if (newTask == null)
            {
                throw new NotFoundException($"NewTask with id {id} not found");
            }
            db.NewTasks.Remove(newTask);
            await db.SaveChangesAsync();
        }

        public async void AddTaskAssignmentAsync(TaskAssignment assignment)
        {
            db.TaskAssignments.Add(assignment);
            await db.SaveChangesAsync();
        }

        private async Task<bool> NewTaskExistsAsync(int id)
        {
            return await db.NewTasks.AnyAsync(e => e.TaskId == id);
        }
    }

    // Services/BatchService.cs
    public class BatchService : IBatchRepository
    {
        private readonly ApplicationDbContext _context;

        public BatchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Batch>> GetAllAsync()
        {
            return await _context.Batches.ToListAsync();
        }

        public async Task<Batch> GetByIdAsync(int id)
        {
            var batch = await _context.Batches.Include(i=>i.Students).SingleOrDefaultAsync(b => b.BatchId == id); 
            if (batch == null)
            {
                throw new NotFoundException($"Batch with id {id} not found");
            }
            return batch;
        }

        public async Task<Batch> AddAsync(Batch batch)
        {
            if (string.IsNullOrWhiteSpace(batch.BatchName))
            {
                throw new ValidationException("Batch name is required");
            }
            _context.Batches.Add(batch);
            await _context.SaveChangesAsync();
            return batch;
        }

        public async void UpdateAsync(Batch batch)
        {
            _context.Entry(batch).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await BatchExistsAsync(batch.BatchId))
                {
                    throw new NotFoundException($"Batch with id {batch.BatchId} not found");
                }
                else
                {
                    throw;
                }
            }
        }

        public async void DeleteAsync(int id)
        {
            var batch = await _context.Batches.FindAsync(id);
            if (batch == null)
            {
                throw new NotFoundException($"Batch with id {id} not found");
            }
            _context.Batches.Remove(batch);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> BatchExistsAsync(int id)
        {
            return await _context.Batches.AnyAsync(e => e.BatchId == id);
        }
    }

    // Services/StudentService.cs
    public class StudentService : IStudentRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                throw new NotFoundException($"Student with id {id} not found");
            }
            return student;
        }

        public async Task<IEnumerable<Student>> GetByBatchIdAsync(int batchId)
        {
            return await _context.Students.Where(s => s.BatchId == batchId).ToListAsync();
        }

        public async Task<Student> AddAsync(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.StudentName))
            {
                throw new ValidationException("Student name is required");
            }
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async void UpdateAsync(Student student)
        {
            _context.Entry(student).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await StudentExistsAsync(student.StudentId))
                {
                    throw new NotFoundException($"Student with id {student.StudentId} not found");
                }
                else
                {
                    throw;
                }
            }
        }

        public async void DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                throw new NotFoundException($"Student with id {id} not found");
            }
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }

        private async Task<bool> StudentExistsAsync(int id)
        {
            return await _context.Students.AnyAsync(e => e.StudentId == id);
        }
    }
}
