using System.ComponentModel.Composition;

namespace ClientApp.Helper
{
    [Export(typeof(IServerConnectionResolver))]
    public class ServerConnectionResolver : IServerConnectionResolver
    {
        public string GetServerApiBaseAddress()
        {
            return "http://localhost:1171/api";
        }
    }
}
