using Container_Cat.Containers.Models;

namespace Container_Cat.Utilities.Models
{
    public class HostAddress
    {
        public Guid Id { get; set; }

        public HostAvailability Availability { get; set; }
        public string Hostname = "";
        //public string Ip
        //{
        //    get { return hostname; }
        //    set
        //    {
        //        if (value == null || value.Length < 1)
        //        {
        //            hostname = "";
        //        }
        //        else
        //            hostname = value;
        //    }
        //}
        public string? Port { get; set; }

        public HostAddress(string _hostname, string _port)
        {
            Id = Guid.NewGuid();
            Hostname = _hostname;
            Port = ":" + _port;
            Availability = HostAvailability.NotTested;
        }

        public HostAddress(string _hostname)
        {
            Id = Guid.NewGuid();
            Port = "";
            Hostname = _hostname;
            Availability = HostAvailability.NotTested;
        }

        public HostAddress() { }

        public HostAddress(HostAddress newHost)
        {
            Id = newHost.Id;
            Hostname = newHost.Hostname;
            Port = newHost.Port;
            Availability = newHost.Availability;
        }

        public void SetStatus(HostAvailability _status)
        {
            Availability = _status;
        }
    }
}
