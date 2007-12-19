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

BASEDIR:=$(shell pwd)

SOURCES = \
$(BASEDIR)/AssemblyInfo.cs \
$(BASEDIR)/Main.cs 

ASSEMBLIES = -r:jabber-net.dll

RESOURCES = 

SYSTEM_REFERENCES = -r:System.dll -r:System.Xml.dll

OUTDIR = $(BASEDIR)/bin/debug
MCS=gmcs
MCS_OPTIONS = -lib:$(OUTDIR) -debug -define:DEBUG

OUTFILE = $(OUTDIR)/console-client.exe
JNDLL = $(BASEDIR)/../bin/debug/jabber-net.dll

all: $(OUTFILE)

$(OUTDIR):
	mkdir -p $(OUTDIR)

$(OUTFILE): $(OUTDIR) $(SOURCES) $(JNDLL)
	cp -f $(JNDLL) $(OUTDIR)
	cp -f $(BASEDIR)/../lib20/*.dll $(OUTDIR)
	$(MCS) $(MCS_OPTIONS) -target:exe \
	-out:$(OUTFILE) $(RESOURCES) $(SYSTEM_REFERENCES) $(ASSEMBLIES)  \
	$(SOURCES) 

clean:
	$(RM) -r bin
