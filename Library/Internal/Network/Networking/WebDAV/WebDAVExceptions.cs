/*
* sones GraphDB - Community Edition - http://www.sones.com
* Copyright (C) 2007-2011 sones GmbH
*
* This file is part of sones GraphDB Community Edition.
*
* sones GraphDB is free software: you can redistribute it and/or modify
* it under the terms of the GNU Affero General Public License as published by
* the Free Software Foundation, version 3 of the License.
* 
* sones GraphDB is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU Affero General Public License for more details.
*
* You should have received a copy of the GNU Affero General Public License
* along with sones GraphDB. If not, see <http://www.gnu.org/licenses/>.
* 
*/

/*
 * WebDAVExceptions
 * 
 * This is a class for all Graph WebDAVException
 *  
 * */

#region Usings

using System;
using System.Text;

#endregion

namespace sones.GraphDS.Connectors.WebDAV
{

    /// <summary>
    /// This is a class for all WebDAV exceptions!
    /// </summary>

    #region WebDAVException Superclass

    public class WebDAVException : ApplicationException
    {
        public WebDAVException(String message)
            : base(message) 
		{
			// do nothing extra
		}
    }

    #endregion

    #region WebDAVException

    public class WebDAVException_ProtocolNotSupported : WebDAVException
    {
        public WebDAVException_ProtocolNotSupported(String message)
            : base(message)
        {
            // do nothing extra
        }
    }

    #endregion



}
