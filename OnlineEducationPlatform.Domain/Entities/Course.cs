using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEducationPlatform.Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string InstructorId { get; set; }
        public List<Lecture> Lectures { get; set; } = new();
        public List<Enrollment> Enrollments { get; set; } = new();
    }
}
