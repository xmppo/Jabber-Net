/*
 * Simple C# wrapper for libresolv to perform DNS SRV lookups.
 * Tested with Mono 2.0 on Linux and OSX.
 *
 * Copyright (c) 2008, Eric Butler <eric@extremeboredom.net>
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS 'AS IS'' AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDERS BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using JabberNet.Netlib.Dns.Records;

namespace JabberNet.bedrock.net
{
	public class UnixDnsResolver
	{
		[DllImport("libresolv", EntryPoint="__res_query")]
		private static extern int linux_res_query (string dname, int cls, int type, byte[] header, int headerlen);

		[DllImport("libresolv", EntryPoint="__dn_expand")]
		private unsafe static extern int linux_dn_expand (byte* msg, byte* endorig, byte* comp_dn, byte[] exp_dn, int length);

		[DllImport("libresolv", EntryPoint="res_query")]
		private static extern int bsd_res_query (string dname, int cls, int type, byte[] header, int headerlen);

		[DllImport("libresolv", EntryPoint="dn_expand")]
		private unsafe static extern int bsd_dn_expand (byte* msg, byte* endorig, byte* comp_dn, byte[] exp_dn, int length);

		private unsafe static int res_query (string dname, int cls, int type, byte[] header, int headerlen)
		{
			try {
				return linux_res_query(dname, cls, type, header, headerlen);
			} catch (EntryPointNotFoundException) {
				return bsd_res_query(dname, cls, type, header, headerlen);
			}
		}

		private unsafe static int dn_expand (byte* msg, byte* endorig, byte* comp_dn, byte[] exp_dn, int length)
		{
			try {
				return linux_dn_expand(msg, endorig, comp_dn, exp_dn, length);
			} catch (EntryPointNotFoundException) {
				return bsd_dn_expand(msg, endorig, comp_dn, exp_dn, length);
			}
		}

		[DllImport("libc")]
		private static extern UInt16 ntohs(UInt16 netshort);

		private static readonly int C_IN  = 1;
		private static readonly int T_SRV = 33;

		[StructLayout(LayoutKind.Explicit)]
		private struct HEADER
		{
			/* The first 4 bytes are a bunch of random crap that
			 * nobody cares about */

			[FieldOffset(4)]
			public UInt16 qdcount; /* number of question entries */

			[FieldOffset(6)]
			public UInt16 ancount; /* number of header entries */

			[FieldOffset(8)]
			public UInt16 nscount; /* number of authority entries */

			[FieldOffset(10)]
			public UInt16 arcount; /* number of resource entries */
		}

		private unsafe static ushort GETSHORT (ref byte* buf)
		{
			byte *t_cp = (byte*)(buf);
			ushort s = (ushort) (((ushort)t_cp[0] << 8) | ((ushort)t_cp[1]));
			buf += sizeof(ushort);
			return s;
		}

		public static SRVRecord[] ResolveSRV (string query)
		{
			byte[] buffer = new byte[1024];
			byte[] name = new byte[256];
			ushort type, dlen, priority, weight, port;
			int size;
			GCHandle handle;
			List<SRVRecord> results = new List<SRVRecord>();

			size = res_query(query, C_IN, T_SRV, buffer, buffer.Length);

			handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

			try {
				HEADER header = (HEADER)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(HEADER));

				int qdcount = ntohs(header.qdcount);
				int ancount = ntohs(header.ancount);

				int headerSize = Marshal.SizeOf(header);

				unsafe {
					fixed (byte* pBuffer = buffer) {

						byte *pos = pBuffer + headerSize;
						byte *end = pBuffer + size;

						// We don't care about the question section.
						while (qdcount-- > 0 && pos < end) {
							size = dn_expand(pBuffer, end, pos, name, 256);
							if (size < 0) return null;
							pos += size + 4;
						}

						// The answers, however, we do care about!
						while (ancount-- > 0 && pos < end) {
							size = dn_expand(pBuffer, end, pos, name, 256);
							if (size < 0) return null;

							pos += size;

							type = GETSHORT(ref pos);

							// Skip TTL
							pos += 6;

							dlen = GETSHORT(ref pos);

							if (type == T_SRV) {
								priority = GETSHORT(ref pos);
								weight = GETSHORT(ref pos);
								port = GETSHORT(ref pos);

								size = dn_expand(pBuffer, end, pos, name, 256);
								if (size < 0) return null;

								string nameStr = null;
								fixed (byte* pName = name) {
									nameStr = new String((sbyte*)pName);
								}

								var record = new SRVRecord();
								record.NameNext = nameStr;
								record.Priority = priority;
								record.Weight = weight;
								record.Port = port;
								results.Add(record);

								pos += size;
							} else {
								pos += dlen;
							}
						}
					}
				}

			} finally {
				handle.Free();
			}

			return results.ToArray();
		}
	}
}
