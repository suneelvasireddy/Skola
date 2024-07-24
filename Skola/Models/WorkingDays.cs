
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Skola.API.Models
{
    public class WorkingDays
    {
        [Key]
        public int WorkingDayID { get; set; }

        [Required]
        public int SchoolID { get; set; }

        [ForeignKey("SchoolID")]
        public School School { get; set; }

        [Required]
        public DateTime DayDate { get; set; }

        [Required]
        public bool IsWorkingDay { get; set; }
    }
}

