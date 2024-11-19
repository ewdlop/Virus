/*++

Module Name:

    public.h

Abstract:

    This module contains the common declarations shared by driver
    and user applications.

Environment:

    user and kernel

--*/

//
// Define an Interface Guid so that app can find the device and talk to it.
//

DEFINE_GUID (GUID_DEVINTERFACE_USBDriver1,
    0xd257ac22,0x9960,0x495f,0x87,0x35,0xa0,0x47,0x7c,0xc8,0x9e,0x9e);
// {d257ac22-9960-495f-8735-a0477cc89e9e}
