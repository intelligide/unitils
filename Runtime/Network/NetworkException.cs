using System.IO;

namespace unitils
{
    public class NetworkException : IOException
    {
        public NetworkException(string message) : base(message)
        {
        }
    }
}
