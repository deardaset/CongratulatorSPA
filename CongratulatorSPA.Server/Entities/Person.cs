using CongratulatorSPA.Server.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CongratulatorSPA.Server.Entities
{
    public class Person
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public RelationshipType RelationshipType { get; set; }

        [NotMapped]
        public int Age => CalculateAge();

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

        private int CalculateAge()
        {
            var today = DateTime.Today;
            var nextbirthday = new DateTime(today.Year, BirthDate.Month, BirthDate.Day);
            var age = today.Year - BirthDate.Year;

            if (BirthDate < nextbirthday)
                age--;

            return age;
        }
        // TODO: Photos
    }
}
