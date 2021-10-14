namespace Solution.Contracts.Responses.Pagination
{
    public class Response<T>
    {
        public T Data { get; set; }

        public Response() { }

        public Response(T response)
        {
            Data = response;
        }
    }
}