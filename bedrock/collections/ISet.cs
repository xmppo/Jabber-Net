/* --------------------------------------------------------------------------
 *
 * License
 *
 * The contents of this file are subject to the Jabber Open Source License
 * Version 1.0 (the "License").  You may not copy or use this file, in either
 * source code or executable form, except in compliance with the License.  You
 * may obtain a copy of the License at http://www.jabber.com/license/ or at
 * http://www.opensource.org/.  
 *
 * Software distributed under the License is distributed on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, either express or implied.  See the License
 * for the specific language governing rights and limitations under the
 * License.
 *
 * Copyrights
 * 
 * Portions created by or assigned to Cursive Systems, Inc. are 
 * Copyright (c) 2002 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at http://www.cursive.net/.
 *
 * Portions Copyright (c) 2002 Joe Hildebrand.
 * 
 * Acknowledgements
 * 
 * Special thanks to the Jabber Open Source Contributors for their
 * suggestions and support of Jabber.
 * 
 * --------------------------------------------------------------------------*/
using System;
using System.Collections;
using bedrock.util;

namespace bedrock.collections
{
	/// <summary>
	/// Set operations.
	/// </summary>
	[RCS(@"$Header$")]
    public interface ISet : ICollection
	{
        /// <summary>
        /// Add an object to the set
        /// </summary>
        /// <param name="o">The object to add</param>
        /// <exception cref="ArgumentException">object was already in the set.</exception>
        void Add(object o);

        /// <summary>
        /// Remove the given object from the set.  If the object is not in the set, this is a no-op.
        /// </summary>
        /// <param name="o">The object to remove.</param>
        void Remove(object o);

        /// <summary>
        /// Remove all items from the set.
        /// </summary>
        void Clear();

        /// <summary>
        /// Is the given object in the set?
        /// </summary>
        /// <param name="o">The object to search for.</param>
        /// <returns>True if the object is in the set.</returns>
        bool Contains(object o);

        /// <summary>
        /// Return a new collection that contains all of the items that
        /// are in this set or the other set.
        /// </summary>
        ISet Union(ISet other);

        /// <summary>
        /// Return a new collection that contains all of the items that 
        /// are in this list *and* the other set.
        /// </summary>
        ISet Intersection(ISet other);
	}
}
