﻿using System;
using sones.Library.ErrorHandling;

namespace sones.GraphQL.ErrorHandling
{
    public sealed class InvalidAssignOfSetException : AGraphQLException
    {
        public String AttributeName { get; private set; }

        /// <summary>
        /// Creates a new InvalidAssignOfSetException exception
        /// </summary>
        /// <param name="myAttributeName"></param>
        public InvalidAssignOfSetException(String myAttributeName)
        {
            AttributeName = myAttributeName;    
        }

        public override string ToString()
        {
            return String.Format("Assignment of the reference type {0} with a list is not allowed. Use SETOF or REF (REFERENCE) instead.", AttributeName);
        }

        public override ushort ErrorCode
        {
            get { return ErrorCodes.InvalidAssignOfSet; }
        }  
    }
}