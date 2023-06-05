namespace Container_Cat.Utilities.Models
{
    public class HostAddress
    {
        public Guid Id { get; set; }
        public enum HostAvailability
        {
            Unreachable,
            Connected,
            WaitingForResult,
            NotTested
        }
        public HostAvailability Availability { get; set; }
        public string Ip
        {
            get; set;
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
        public string? Port
        {
            get; set;
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
            Ip = _ip;
            Port = _port;
            Availability = HostAvailability.NotTested;
        }
        public HostAddress(string _ip)
        {
            Ip = _ip;
            Port = "";
            Availability = HostAvailability.NotTested;
        }
        public void SetStatus(HostAvailability _status)
        {
            Availability = _status;
        }
    }
}