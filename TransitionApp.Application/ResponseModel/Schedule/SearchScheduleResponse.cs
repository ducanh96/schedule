namespace TransitionApp.Application.ResponseModel.Schedule
{
    public class SearchScheduleResponse
    {
        public object Data { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool Success { get; set; }
        public int Total { get; set; }
        public string Message { get; set; }
    }
}
