using CongratulatorSPA.Server.Models.Enums;

namespace CongratulatorSPA.Server.Entities
{
    public class Person
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public RelationshipType RelationshipType { get; set; }
        // TODO: Photos
    }
}
