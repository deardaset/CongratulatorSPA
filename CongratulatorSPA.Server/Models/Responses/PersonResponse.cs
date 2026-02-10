using CongratulatorSPA.Server.Models.Enums;

namespace CongratulatorSPA.Server.Models.Responses
{
    public class PersonResponse
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public RelationshipType RelationshipType { get; set; }
        public string PhotoUrl { get; set; }
        public int Age { get; set;  }
    }
}
