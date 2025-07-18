using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEducationPlatform.Domain.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
    }
}
