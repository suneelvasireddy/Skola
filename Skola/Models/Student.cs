using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Skola.API.Models
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }
        [NotNull]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonNumber { get; set; }
        
        [ForeignKey("School")]
        public int SchoolAttendingID { get; set; }
        public int? Age { get; set; }
        public string? ContactNumber { get; set; }
        public string? ClassAttending { get; set; }

        public School School { get; set; }
    }
}
