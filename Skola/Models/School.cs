using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Skola.API.Models
{
    public class School
    {
        [Key]
        public int SchoolID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string UniqueID { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
