using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Taskmanager_MVCAPI.Models
{
    public class Batch
    {
        [Key]
        public int BatchId { get; set; }
        public string BatchName { get; set; }
        public List<Student>? Students { get; set; }
    }

    public class Student
    {
        [Key]
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public int BatchId { get; set; }

        [JsonIgnore]
        public Batch? Batch { get; set; }

        [JsonIgnore]
        public List<TaskAssignment>? TaskAssignments { get; set; }

    }

    public class NewTask
    {
        [Key]
        public int TaskId { get; set; }
        public string TaskDescription { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string AttachmentPath { get; set; }
        public List<TaskAssignment>? TaskAssignments { get; set; }
    }

    public class TaskAssignment
    {
        [Key]
        public int TaskAssignmentId { get; set; }
        public int TaskId { get; set; }

        [JsonIgnore]
        public NewTask? Task { get; set; }
        public int? StudentId { get; set; }
        public Student? Student { get; set; }
    }
}
