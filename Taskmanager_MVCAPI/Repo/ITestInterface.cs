using Taskmanager_MVCAPI.Models;

namespace Taskmanager_MVCAPI.Repo
{
    // Interfaces/IBatchRepository.cs
    public interface IBatchRepository
    {
        Task<IEnumerable<Batch>> GetAllAsync();
        Task<Batch> GetByIdAsync(int id);
        Task<Batch> AddAsync(Batch batch);
        void UpdateAsync(Batch batch);
        void DeleteAsync(int id);
    }

    // Interfaces/IStudentRepository.cs
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student> GetByIdAsync(int id);
        Task<IEnumerable<Student>> GetByBatchIdAsync(int batchId);
        Task<Student> AddAsync(Student student);
        void UpdateAsync(Student student);
        void DeleteAsync(int id);
    }

    // Interfaces/INewTaskRepository.cs
    public interface INewTaskRepository
    {
        Task<IEnumerable<NewTask>> GetAllAsync();
        Task<NewTask> GetByIdAsync(int id);
        Task<NewTask> GetByIdWithDetailsAsync(int taskId);
        Task<NewTask> AddAsync(NewTask newTask);
        void UpdateAsync(NewTask newTask);
        void DeleteAsync(int id);
        void AddTaskAssignmentAsync(TaskAssignment assignment);
    }
}
