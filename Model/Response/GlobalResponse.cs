namespace TasklistAPI.Model.Response
{
    public class GlobalResponse
    {
        public string message { get; set; } = string.Empty;
        public int status_code { get; set; }

        public dynamic? data { get; set; }
    }
}
