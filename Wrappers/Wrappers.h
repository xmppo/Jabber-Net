// Wrappers.h

#pragma once

using namespace System;

namespace Wrappers {

        public ref class SRVRecord
        {
        public:
                int TTL;
                int Priority;
                int Weight;
                int Port;
                String ^Target;

				virtual String^ ToString() override
				{
					return String::Format("{0} {1} {2} {3}", Priority, Weight, Port, Target);
				}
        };

        /// <summary>
        /// Wrap SRV lookups.
        /// </summary>
        public ref class DNS
        {
        public:
                static cli::array<SRVRecord^>^ SRVLookup(String ^name);
        };
}
