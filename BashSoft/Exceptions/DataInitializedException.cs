using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class DataInitializedException : Exception
    {
        private const string DataAlreadyInitializedException = "Data is already initialized!";

        public DataInitializedException()
            :base(DataAlreadyInitializedException)
        {

        }

        public DataInitializedException(string message)
            :base(message)
        {
            
        }
    }
}
