
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace unitils
{
    public class UriBuilder
    {
        public string Scheme;

        public string User = null;

        public string Password = null;

        public string Host;

        public int Port = 0;

        public string Path;

        public string Fragment;

        public Dictionary<string, string> QueryParams = null;

        public void AddQueryParam(string name, string value)
        {
            if(QueryParams == null)
            {
                QueryParams = new Dictionary<string, string>();
            }
            QueryParams[name] = value;
        }

        public void AddQueryParam<T>(string name, T value)
        {
            AddQueryParam(name, value.ToString());
        }

        public override string ToString()
        {
            var strBuilder = new StringBuilder(Scheme);

            strBuilder.Append("://");

            if(User != null)
            {
                strBuilder.Append(User);
                if(Password != null) {
                    strBuilder.Append(':').Append(Password).Append('@');
                }
            }
            strBuilder.Append(Host);

            if(Port > 0)
            {
                strBuilder.Append(':').Append(Port);
            }

            if(Path != null && Path.Length > 0)
            {
                if(Path[0] != '/')
                {
                    strBuilder.Append('/');
                }
                strBuilder.Append(Path);
            }

            if(Fragment != null && Fragment.Length > 0)
            {
                strBuilder.Append('#').Append(Fragment);
            }

            if(QueryParams != null && QueryParams.Count > 0)
            {
                strBuilder.Append('?');
                foreach(KeyValuePair<string, string> entry in QueryParams)
                {
                    strBuilder.Append(HttpUtility.UrlEncode(entry.Key)).Append('=').Append(HttpUtility.UrlEncode(entry.Value));
                    strBuilder.Append('&');
                }
                strBuilder.Remove(strBuilder.Length - 1, 1);
            }

            return strBuilder.ToString();
        }


    }
}
