internal class HostAddress
{
    internal string Ip
    {
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
    }
    internal string Port
    {
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
    }
    internal HostAddress(string _ip, string _port)
    {
        Ip = _ip;
        Port = _port;
    }
}