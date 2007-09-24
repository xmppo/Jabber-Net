#--------------------------------------------------------------------------
# Copyrights
#
# Portions created by or assigned to Cursive Systems, Inc. are
# Copyright (c) 2002-2004 Cursive Systems, Inc.  All Rights Reserved.  Contact
# information for Cursive Systems, Inc. is available at
# http://www.cursive.net/.
#
# License
#
# Jabber-Net can be used under either JOSL or the GPL.
# See LICENSE.txt for details.
# --------------------------------------------------------------------------

# The purpose of this Makefile is to facilitate mono builds.

BASEDIR = $(CURDIR)
DEBUGDIR = $(BASEDIR)/bin/debug

SOURCES = \
$(BASEDIR)/jabber/protocol/ElementStreamTest.cs 

# $(BASEDIR)/AssemblyInfo.cs \
# $(BASEDIR)/bedrock/net/SSLAsyncSocketTest.cs \
# $(BASEDIR)/bedrock/net/AsyncSocketTest.cs \
# $(BASEDIR)/bedrock/util/VersionTest.cs \
# $(BASEDIR)/bedrock/util/GetOptBaseTest.cs \
# $(BASEDIR)/bedrock/collections/TrieTest.cs \
# $(BASEDIR)/bedrock/collections/TrieNodeTest.cs \
# $(BASEDIR)/bedrock/collections/SkipListTest.cs \
# $(BASEDIR)/bedrock/collections/SetTest.cs \
# $(BASEDIR)/bedrock/collections/TreeTest.cs \
# $(BASEDIR)/bedrock/collections/ByteStackTest.cs \
# $(BASEDIR)/jabber/JIDTest.cs \
# $(BASEDIR)/jabber/client/RosterManagerTest.cs \
# $(BASEDIR)/jabber/client/PresenceManagerTest.cs \
# $(BASEDIR)/jabber/protocol/x/DataTest.cs \
# $(BASEDIR)/jabber/protocol/x/EventTest.cs \
# $(BASEDIR)/jabber/protocol/iq/AuthTest.cs \
# $(BASEDIR)/jabber/protocol/iq/AgentTest.cs \
# $(BASEDIR)/jabber/protocol/iq/RosterTest.cs \
# $(BASEDIR)/jabber/protocol/iq/RegisterTest.cs \
# $(BASEDIR)/jabber/protocol/PacketTest.cs \
# $(BASEDIR)/jabber/protocol/ElementListTest.cs \
# $(BASEDIR)/jabber/protocol/accept/RouteTest.cs \
# $(BASEDIR)/jabber/protocol/client/MessageTest.cs \
# $(BASEDIR)/jabber/protocol/client/PresenceTest.cs \
# $(BASEDIR)/jabber/protocol/client/IQTest.cs \
# $(BASEDIR)/jabber/protocol/stream/StreamTest.cs \
# $(BASEDIR)/jabber/protocol/stream/FactoryTest.cs \

RESOURCES = 

ASSEMBLIES = -r:nunit.framework.dll -r:jabber-net.dll

SYSTEM_REFERENCES = -r:System.dll -r:System.Xml.dll 

MCS_OPTIONS = -lib:$(DEBUGDIR) -debug -nowarn:0618
DLL = jabber-net-tests.dll
OUTFILE = $(DEBUGDIR)/$(DLL)

all: $(OUTFILE)

$(DEBUGDIR)/jabber-net.dll: $(BASEDIR)/../bin/debug/jabber-net.dll
	-mkdir -p bin/debug
	-cp $(BASEDIR)/../bin/debug/jabber-net.dll $(DEBUGDIR)
	-cp $(BASEDIR)/../lib/*.dll $(DEBUGDIR)

$(OUTFILE): $(DEBUGDIR)/jabber-net.dll $(SOURCES)
	mcs $(MCS_OPTIONS) -target:library \
	-out:"$(OUTFILE)" $(RESOURCES) $(SYSTEM_REFERENCES) \
	$(SOURCES) $(ASSEMBLIES) 

clean:
	$(RM) -r bin

test: $(OUTFILE)
	cd $(DEBUGDIR) && nunit-console --nologo "$(DLL)"
