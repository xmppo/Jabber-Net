// This is the main DLL file.

#include "stdafx.h"
#include <Windows.h>
#include <Windns.h>
#include <Vcclr.h>

#include "Wrappers.h"


cli::array<Wrappers::SRVRecord^>^ Wrappers::DNS::SRVLookup(String ^name)
{
        
        pin_ptr<const wchar_t> wch;
        if (name == nullptr)
                throw gcnew ArgumentException("Name must not be null", "name");

        // TODO: I wonder if this leaks?
        wch = PtrToStringChars(name);
        
        PDNS_RECORD results = NULL;
        DNS_STATUS stat = DnsQuery_W((PCWSTR)wch, DNS_TYPE_SRV, DNS_QUERY_STANDARD, NULL, &results, NULL);

        if (stat == DNS_INFO_NO_RECORDS)
                return gcnew cli::array<Wrappers::SRVRecord^>(0);;

        if (stat != DNS_ERROR_RCODE_NO_ERROR)
                throw gcnew System::Net::Sockets::SocketException(stat);

        // How many records are there, so we can just allocate an array.
        PDNS_RECORD i;
        int count = 0;
        for (i=results; i != NULL; i = i->pNext)
        {
                if (i->wType == DNS_TYPE_SRV)
                        count++;
        }

        cli::array<Wrappers::SRVRecord^>^ arr = gcnew cli::array<Wrappers::SRVRecord^>(count);
        count=0;
        for (i=results; i != NULL; i = i->pNext)
        {
                if (i->wType == DNS_TYPE_SRV)
                {
                        arr[count] = gcnew Wrappers::SRVRecord();
                        arr[count]->Priority = i->Data.SRV.wPriority;
                        arr[count]->Weight = i->Data.SRV.wWeight;
                        arr[count]->Port = i->Data.SRV.wPort;
                        // TODO: I wonder if this leaks?
                        arr[count]->Target = System::Runtime::InteropServices::Marshal::PtrToStringUni((IntPtr)i->Data.SRV.pNameTarget);
                        count++;
                }
        }
        
        if (results != NULL)
        {
                DnsRecordListFree(results,  DnsFreeRecordList);
        }
        return arr;
}
