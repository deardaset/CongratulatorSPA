using CongratulatorSPA.Server.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CongratulatorSPA.Server.Models
{
    public class PersonModel
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public RelationshipType RelationshipType { get; set; }
        public string PhotoUrl { get; set; }

        [NotMapped]
        public int Age => DateTime.Today.Year - BirthDate.Year - (DateTime.Today.DayOfYear < BirthDate.DayOfYear ? 1 : 0);

        [NotMapped]
        public DateTime NextBirthday
        {
            get
            {
                var today = DateTime.Today;
                var next = new DateTime(today.Year, BirthDate.Month, BirthDate.Day);
                if (next < today) next = next.AddYears(1);
                return next;
            }
        }
    }
}
