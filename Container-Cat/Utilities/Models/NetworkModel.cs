using Container_Cat.Containers.Models;

namespace Container_Cat.Utilities.Models
{
    public class HostAddress
    {
        public Guid Id { get; set; }

        public HostAvailability Availability { get; set; }
        private string hostname = "";
        public string Ip
        {
            get { return hostname; }
            set
            {
                if (value == null || value.Length < 1)
                {
                    hostname = "";
                }
                else
                    hostname = value;
            }
        }
        public string? Port { get; set; }

        public HostAddress(string _ip, string _port)
        {
            Id = Guid.NewGuid();
            Ip = _ip;
            Port = ":" + _port;
            Availability = HostAvailability.NotTested;
        }

        public HostAddress(string _ip)
        {
            Id = Guid.NewGuid();
            Port = "";
            Ip = _ip;
            Availability = HostAvailability.NotTested;
        }

        public HostAddress() { }

        public HostAddress(HostAddress newHost)
        {
            Id = newHost.Id;
            Ip = newHost.Ip;
            Port = newHost.Port;
            Availability = newHost.Availability;
        }

        public void SetStatus(HostAvailability _status)
        {
            Availability = _status;
        }
    }
}
