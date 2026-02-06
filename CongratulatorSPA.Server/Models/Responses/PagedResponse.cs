namespace CongratulatorSPA.Server.Models.Responses
{
    public class PagedResponse<PersonResponse>
    {
        public List<PersonResponse> People { get; set; }
        public int TotalCount { get; set; }
        public int Page {  get; set; }
        public int PageSize { get; set; }
    }
}
