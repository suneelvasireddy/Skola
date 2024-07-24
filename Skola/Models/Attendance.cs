using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Skola.API.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceID { get; set; }

        [Required]
        public int StudentID { get; set; }

        [ForeignKey("StudentID")]
        public Student Student { get; set; }

        [Required]
        public bool IsPresent { get; set; }

        [Required]
        public int WorkingDayID { get; set; }

        [ForeignKey("WorkingDayID")]
        public WorkingDays WorkingDays { get; set; }
    }
}
