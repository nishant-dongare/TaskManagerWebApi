using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taskmanager_MVCAPI.Data;
using Taskmanager_MVCAPI.Exceptions;
using Taskmanager_MVCAPI.Models;
using Taskmanager_MVCAPI.Repo;

namespace Taskmanager_MVCAPI.Controllers
{
    // Controllers/NewTasksController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class NewTasksController : ControllerBase
    {
        private readonly INewTaskRepository repo;
        private readonly IBatchRepository batchrepo;

        public NewTasksController(INewTaskRepository repo, IBatchRepository batchrepo)
        {
            this.repo = repo;
            this.batchrepo = batchrepo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewTask>>> GetAllNewTasks()
        {
            var newTasks = await repo.GetAllAsync();
            return Ok(newTasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NewTask>> GetNewTask(int id)
        {
            try
            {
                var newTask = await repo.GetByIdAsync(id);
                return Ok(newTask);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<NewTask>> CreateNewTask(NewTaskDto newTaskDto)
        {
            try
            {
                var newTask = new NewTask
                {
                    TaskDescription = newTaskDto.TaskDescription,
                    CreatedDate = newTaskDto.CreatedDate ?? DateTime.UtcNow,
                    DueDate = newTaskDto.DueDate,
                    AttachmentPath = newTaskDto.AttachmentPath,
                    TaskAssignments = new List<TaskAssignment>()
                };

                if (newTaskDto.TaskAssignmentsDto != null && newTaskDto.TaskAssignmentsDto.Any())
                {
                    var firstAssignment = newTaskDto.TaskAssignmentsDto.First();

                    if (firstAssignment.BatchId.HasValue)
                    {
                        // Fetch students based on the batch ID
                        var batch = await batchrepo.GetByIdAsync(firstAssignment.BatchId.Value);
                        if (batch?.Students != null)
                        {
                            foreach (var student in batch.Students)
                            {
                                newTask.TaskAssignments.Add(new TaskAssignment
                                {
                                    StudentId = student.StudentId,
                                    Task = newTask
                                });
                            }
                        }
                    }
                    else if (firstAssignment.Students != null && firstAssignment.Students.Any())
                    {
                        // Use the list of student IDs provided in the DTO
                        foreach (var student in firstAssignment.Students)
                        {
                            newTask.TaskAssignments.Add(new TaskAssignment
                            {
                                StudentId = student.StudentId,
                                Task = newTask
                            });
                        }
                    }
                }

                var createdNewTask = await repo.AddAsync(newTask);

                // Fetch the complete task with assignments and students
                var taskWithDetails = await repo.GetByIdWithDetailsAsync(createdNewTask.TaskId);

                return CreatedAtAction(nameof(GetNewTask), new { id = taskWithDetails.TaskId }, taskWithDetails);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /*[HttpPost]
        public async Task<ActionResult<NewTask>> CreateNewTask(NewTaskDto newTaskdto)
        {
            int BatchId = newTaskdto.TaskAssignmentsDto.First().BatchId ?? 0;
            List<Student> students;
            if (BatchId > 0)
            {
                students = batchrepo.GetByIdAsync(BatchId).Result.Students;
            }

            try
            {
                // Map NewTaskDto to NewTask
                NewTask newTask = new NewTask
                {
                    TaskId = newTaskdto.TaskId,
                    TaskDescription = newTaskdto.TaskDescription,
                    CreatedDate = newTaskdto.CreatedDate,
                    DueDate = newTaskdto.DueDate,
                    AttachmentPath = newTaskdto.AttachmentPath,
                    TaskAssignments = new List<TaskAssignment>()
                };

                // Map TaskAssignmentsDto to TaskAssignments
                if (newTaskdto.TaskAssignmentsDto != null)
                {
                    foreach (var taskAssignmentDto in newTaskdto.TaskAssignmentsDto)
                    {
                        TaskAssignment taskAssignment = new TaskAssignment
                        {
                            TaskAssignmentId = taskAssignmentDto.TaskAssignmentId,
                            TaskId = taskAssignmentDto.TaskId,
                            StudentId = taskAssignmentDto.StudentId,
                            Task = newTask
                        };

                        // Assign students to task assignments if applicable
                        if (taskAssignment.StudentId.HasValue)
                        {
                            var student = students.FirstOrDefault(s => s.StudentId == taskAssignment.StudentId.Value);
                            taskAssignment.Student = student;
                        }

                        newTask.TaskAssignments.Add(taskAssignment);
                    }
                }
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }*/



        /*[HttpPost]
        public async Task<ActionResult<NewTask>> CreateNewTask(NewTaskDto newTaskDto)
        {
            List<Student> students = new List<Student>();

            if (newTaskDto.TaskAssignmentsDto != null && newTaskDto.TaskAssignmentsDto.Any())
            {
                int batchId = newTaskDto.TaskAssignmentsDto.First().BatchId ?? 0;

                if (batchId > 0)
                {
                    // Fetch students based on the batch ID
                    var batch = await batchrepo.GetByIdAsync(batchId);
                    students = batch?.Students ?? new List<Student>();
                }
                else
                {
                    // Use the list of students provided in the DTO
                    foreach (var assignmentDto in newTaskDto.TaskAssignmentsDto.First().Students)
                    {
                        if (assignmentDto.StudentId.HasValue)
                        {
                            var student = new Student
                            {
                                StudentId = assignmentDto.StudentId.Value
                                //StudentName = assignmentDto. // Ensure this property is available in your DTO
                            };
                            students.Add(student);
                        }
                    }
                }
            }

            try
            {
                // Map NewTaskDto to NewTask
                NewTask newTask = new NewTask
                {
                    TaskId = newTaskDto.TaskId,
                    TaskDescription = newTaskDto.TaskDescription,
                    CreatedDate = newTaskDto.CreatedDate ?? DateTime.Now, // Default to current date if null
                    DueDate = newTaskDto.DueDate,
                    AttachmentPath = newTaskDto.AttachmentPath,
                    TaskAssignments = new List<TaskAssignment>()
                };

                // Map TaskAssignmentsDto to TaskAssignments
                foreach (var taskAssignmentDto in newTaskDto.TaskAssignmentsDto)
                {
                    TaskAssignment taskAssignment = new TaskAssignment
                    {
                        TaskAssignmentId = taskAssignmentDto.TaskAssignmentId,
                        TaskId = taskAssignmentDto.TaskId,
                        StudentId = taskAssignmentDto.StudentId,
                        Task = newTask
                    };

                    // Assign students to task assignments if applicable
                    if (taskAssignment.StudentId.HasValue)
                    {
                        var student = students.FirstOrDefault(s => s.StudentId == taskAssignment.StudentId.Value);
                        taskAssignment.Student = student;
                    }

                    newTask.TaskAssignments.Add(taskAssignment);
                }

                var createdNewTask = await repo.AddAsync(newTask);
                return CreatedAtAction(nameof(GetNewTask), new { id = createdNewTask.TaskId }, createdNewTask);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }*/



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNewTask(int id, NewTask newTask)
        {
            if (id != newTask.TaskId)
            {
                return BadRequest("Id mismatch");
            }

            try
            {
                repo.UpdateAsync(newTask);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNewTask(int id)
        {
            try
            {
                repo.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{newTaskId}/assign")]
        public async Task<IActionResult> AssignNewTask(int newTaskId, [FromBody] TaskAssignment assignment)
        {
            if (newTaskId != assignment.TaskId)
            {
                return BadRequest("Id mismatch");
            }

            try
            {
                repo.AddTaskAssignmentAsync(assignment);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while assigning the task: {ex.Message}");
            }
        }
    }

    // Controllers/BatchesController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class BatchesController : ControllerBase
    {
        private readonly IBatchRepository repo;

        public BatchesController(IBatchRepository repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Batch>>> GetAllBatches()
        {
            var batches = await repo.GetAllAsync();
            return Ok(batches);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Batch>> GetBatch(int id)
        {
            try
            {
                var batch = await repo.GetByIdAsync(id);
                return Ok(batch);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Batch>> CreateBatch(Batch batch)
        {
            try
            {
                var createdBatch = await repo.AddAsync(batch);
                return CreatedAtAction(nameof(GetBatch), new { id = createdBatch.BatchId }, createdBatch);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBatch(int id, Batch batch)
        {
            if (id != batch.BatchId)
            {
                return BadRequest("Id mismatch");
            }

            try
            {
                repo.UpdateAsync(batch);
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBatch(int id)
        {
            try
            {
                repo.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext db;

        public StudentsController(ApplicationDbContext context)
        {
            db = context;
        }



        // GET: api/Students/5
        /*[HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await db.Students.FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            // Load the related Batch entity
            student.Batch = await db.Batches.FirstOrDefaultAsync(b => b.BatchId == student.BatchId);

            // Load the related TaskAssignments
            student.TaskAssignments = await db.TaskAssignments.Where(ta => ta.StudentId == student.StudentId)
                                                    .ToListAsync();

            return student;
        }*/

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(int id)
        {
            var student = await db.Students.FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            var batch = await db.Batches.FirstOrDefaultAsync(b => b.BatchId == student.BatchId);
            var taskAssignments = await db.TaskAssignments
                                                .Where(ta => ta.StudentId == student.StudentId)
                                                .ToListAsync();

            var studentDto = new StudentDto
            {
                StudentId = student.StudentId,
                StudentName = student.StudentName,
                Batch = batch != null ? new Batch { BatchId = batch.BatchId, BatchName = batch.BatchName } : null,
                TaskAssignmentsDto = taskAssignments.Select(ta => new TaskAssignmentDto
                {
                    TaskAssignmentId = ta.TaskAssignmentId,
                    TaskId = ta.TaskId,
                    StudentId = ta.StudentId
                }).ToList()
            };

            return studentDto;
        }



        /*// GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            var student = await db.Students.Include(s => s.Batch).Include(s => s.TaskAssignments).FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return student;
        }*/
    }
}
