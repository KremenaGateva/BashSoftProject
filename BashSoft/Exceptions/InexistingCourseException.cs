using System;
using System.Collections.Generic;
using System.Text;

namespace BashSoft.Exceptions
{
    public class InexistingCourseException : Exception
    {
        private const string InexistingCourseInDataBase =
    "The course you are trying to get does not exist in the data base!";

        public InexistingCourseException()
            :base(InexistingCourseInDataBase)
        {

        }

        public InexistingCourseException(string message)
            :base(message)
        {

        }
    }
}
