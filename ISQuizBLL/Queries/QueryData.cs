namespace ISQuizBLL.Queries
{
    public class QueryData
    {
        public HttpMethod method {  get; set; }
        public string endpoint {  get; set; } //Url
        public object data { get; set; } = default;

        public string Credentials { get; set; }
    }
}
