using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class DataNotInitializedException : Exception
    {
        private const string DataNotInitializedExceptionMessage =
    "The data structure must be initialized first in order to make any operations with it.";

        public DataNotInitializedException()
            :base(DataNotInitializedExceptionMessage)
        {

        }

        public DataNotInitializedException(string message)
            :base(message)
        {

        }
    }
}
