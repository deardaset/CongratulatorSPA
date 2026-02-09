namespace CongratulatorSPA.Server.Models.Requests
{
    public class GetPeopleOptionsRequest
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortBy { get; set; }
        public string? SearchBy { get; set; }
    }
}
