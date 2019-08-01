
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace unitils
{
    public class Uri
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

    public class UriBuilder
    {
        private Uri m_uri;

        public UriBuilder()
        {
            m_uri = new Uri();
        }

        public UriBuilder Scheme(string val)
        {
            m_uri.Scheme = val;
            return this;
        }

        public UriBuilder User(string val)
        {
            m_uri.User = val;
            return this;
        }

        public UriBuilder Password(string val)
        {
            m_uri.Password = val;
            return this;
        }

        public UriBuilder Host(string val)
        {
            m_uri.Host = val;
            return this;
        }

        public UriBuilder Port(int val)
        {
            m_uri.Port = val;
            return this;
        }

        public UriBuilder Path(string val)
        {
            m_uri.Path = val;
            return this;
        }

        public UriBuilder Fragment(string val)
        {
            m_uri.Fragment = val;
            return this;
        }

        public UriBuilder QueryParams(Dictionary<string, string> val)
        {
            m_uri.QueryParams = val;

            return this;
        }

        public UriBuilder AddQueryParam(string name, string value)
        {
            if(m_uri.QueryParams == null)
            {
                m_uri.QueryParams = new Dictionary<string, string>();
            }
            m_uri.QueryParams[name] = value;

            return this;
        }

        public UriBuilder AddQueryParam<T>(string name, T value)
        {
            AddQueryParam(name, value.ToString());
            return this;
        }

        public Uri Build()
        {
            return m_uri;
        }
    }
}
