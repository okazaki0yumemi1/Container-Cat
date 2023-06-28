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
                    Console.WriteLine("Null or empty value of Hostname.Ip passed.");
                    hostname = "";
                }
                else
                    hostname = value;
            }
            /*
            get { return Ip; }
            set
            {
                string _ip = value;
                var addressParts = _ip.Split('.');
                if (addressParts.Length != 4) throw new ArgumentException("The passed IP address was in incorrect format.");
                foreach (var part in addressParts)
                {
                    byte _ipPart;
                    if (Byte.TryParse(part, out _ipPart))
                    {
                        if (!((_ipPart >= 0) && (_ipPart <= 255))) throw new ArgumentOutOfRangeException("The passed IP address fragment had incorrect value.");
                    }
                    else throw new ArgumentException("Unable to parse passed IP address fragment to Byte.");
                }
                Ip = value;
            }
            */
        }
        public string? Port { get; set;
            /*
            get { return Port; }
            set
            {
                Int32 _port;
                if (Int32.TryParse(value, out _port))
                {
                    if ((_port > 0) && (_port <= 65535)) Port = value;
                    else throw new ArgumentOutOfRangeException("The passed Port number was succesfully converted to Int32, but it's value was incorrect.");
                }
                else throw new ArgumentException("Unable to parse passed Port number to Int32");
            }
            */
        }

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
