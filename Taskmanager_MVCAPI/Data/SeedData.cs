using Microsoft.EntityFrameworkCore;
using Taskmanager_MVCAPI.Models;

namespace Taskmanager_MVCAPI.Data
{
    public class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Batch>().HasData(
        new Batch { BatchId = 1, BatchName = "Batch1" },
        new Batch { BatchId = 2, BatchName = "Batch2" }
    );

            modelBuilder.Entity<Student>().HasData(
                new Student { StudentId = 1, StudentName = "s1b1", BatchId = 1 },
                new Student { StudentId = 2, StudentName = "s2b1", BatchId = 1 },
                new Student { StudentId = 3, StudentName = "s3b1", BatchId = 1 },
                new Student { StudentId = 4, StudentName = "s4b1", BatchId = 1 },
                new Student { StudentId = 5, StudentName = "s5b1", BatchId = 1 },
                new Student { StudentId = 6, StudentName = "s1b2", BatchId = 2 },
                new Student { StudentId = 7, StudentName = "s2b2", BatchId = 2 },
                new Student { StudentId = 8, StudentName = "s3b2", BatchId = 2 },
                new Student { StudentId = 9, StudentName = "s4b2", BatchId = 2 },
                new Student { StudentId = 10, StudentName = "s5b2", BatchId = 2 }
            );

        }

    }
}
