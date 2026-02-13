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
        public string PhotoUrl { get; set; }
    }
}
