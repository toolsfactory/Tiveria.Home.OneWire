using System;

namespace Tiveria.Home.OneWire.OWServer.Messages
{
    /// <summary>
    ///   OWServer Message control flags
    ///   Word 4 of the protocol header has the following format:
    ///    * 4 bytes long
    ///    * network long order
    ///    * present in both server and client messages
    /// 
    ///   <code>
    ///     Field           Value       pattern      comment 
    ///     device display
    ///             f.i         0x00000000   /10.67C6697351FF
    ///             fi          0x01000000	 /1067C6697351FF
    ///             f, i, c     0x02000000	 /10.67C6697351FF.8D
    ///             f.ic        0x03000000	 /10.67C6697351FF8D
    ///             fi.c        0x04000000	 /1067C6697351FF.8D
    ///             fic         0x05000000	 /1067C6697351FF8D
    ///     temperature scale
    ///             C 	        0x00000000	 Centigrade
    ///             F           0x00010000	 Fahrenheit
    ///             K           0x00020000	 Kelvin
    ///             R           0x00030000	 Rankine
    ///    pressure scale
    ///             mbar        0x00000000   millibar
    ///             atm	        0x00040000	 atmosphere
    ///             mmHg	    0x00080000   mm Mercury
    ///             inHg        0x000C0000	 inch Mercury
    ///             psi         0x00100000	 pounds per square inch
    ///             Pa          0x00140000	 pascal
    ///    ownet_flag	 	    0x00000100	 Ownet request (included for all ownet messages)
    ///    uncached	    0/1 	0x00000020	 Implicit /uncached
    ///    safemode     0/1	    0x00000010	 Restrict operations to reads and cached, more
    ///    alias	    0/1     0x00000008	 Use aliases for known slaves(human readable names)
    ///    persistence	0/1     0x00000004	 Request/Grant persistence
    ///    bus_ret	    0/1     0x00000002	 Include special directories(settings, statistics, uncached,...)
    ///  </code>
    /// </summary>
    [Flags]
    public enum ControlFlags : uint
    {   
        Display_F_I     = 0x0000_0000,
        Display_FI      = 0x0100_0000,
        Display_F_I_C   = 0x0200_0000,
        Display_F_IC    = 0x0300_0000,
        Display_FI_C    = 0x0400_0000,
        Display_FIC     = 0x0500_0000,
        Temp_Celsius    = 0x0000_0000,
        Temp_Fahrenheit = 0x0001_0000,
        Temp_Kelvin     = 0x0002_0000,
        Temp_Rakine     = 0x0003_0000,
        Pressure_MBar   = 0x0000_0000,
        Pressure_Atm    = 0x0004_0000,
        Pressure_mmHg	= 0x0008_0000,
        Pressure_inHg   = 0x000C_0000,
        Pressure_psi    = 0x0010_0000,
        Pressure_Pa     = 0x0014_0000,
        OWNet           = 0x0000_0100,
        Uncached        = 0x0000_0020,
        Safemode        = 0x0000_0010,
        Alias           = 0x0000_0008,
        Persistence     = 0x0000_0004,
        BusRet          = 0x0000_0002,
        Reserved        = 0x0000_0001,
        Default         = OWNet | Persistence | Reserved
    }
}
