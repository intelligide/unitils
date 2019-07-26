using System.IO;

namespace unitils
{
    public class HttpException : IOException
    {
        public long ResponseCode { get; private set; }

        public string Response { get; private set; }

        public HttpException(string message, long responseCode, string response) : base(message)
        {
            ResponseCode  = responseCode;
            Response = response;
        }
    }
}
