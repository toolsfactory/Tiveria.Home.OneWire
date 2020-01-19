namespace Tiveria.Home.OneWire.OWServer.Messages
{
    public enum MessageTypes : uint
    {
        Error = 0,
        Nop = 1,
        Read = 2,
        Write = 3,
        Dir = 4,
        Size = 5,
        Presence = 6,
        DirAll = 7,
        Get = 8,
        Read_Any = 99999
    }
}
