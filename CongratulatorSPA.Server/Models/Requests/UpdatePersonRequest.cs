using CongratulatorSPA.Server.Models.Enums;

namespace CongratulatorSPA.Server.Models.Requests
{
    public class UpdatePersonRequest
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public RelationshipType RelationshipType { get; set; }
    }
}
