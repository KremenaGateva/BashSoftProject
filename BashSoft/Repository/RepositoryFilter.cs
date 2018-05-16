namespace BashSoft
{
    using System;
    using System.Collections.Generic;
    using BashSoft.Contracts;

    public class RepositoryFilter : IDataFilter
    {
        public void FilterAndTake(Dictionary<string, double> studentWithGrades,
            string wantedFilter, int studentsToTake)
        {
            if (wantedFilter == "excellent")
            {
                FilterAndTake(studentWithGrades, x => x >= 5.50 , studentsToTake);
            }
            else if (wantedFilter == "average")
            {
                FilterAndTake(studentWithGrades, x => x < 5.50 && x >= 3.50, studentsToTake);
            }
            else if (wantedFilter == "poor")
            {
                FilterAndTake(studentWithGrades, x => x < 3.50, studentsToTake);
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.InvalidStudentFilter);
            }
        }

        private void FilterAndTake(Dictionary<string, double> studentsWithGrades, 
            Predicate<double> givenFilter, int studentsToTake)
        {
            var printedStudents = 0;
            foreach (var student in studentsWithGrades)
            {
                if (printedStudents == studentsToTake)
                {
                    break;
                }
                if (givenFilter(student.Value))
                {
                    OutputWriter.PrintStudent(new KeyValuePair<string, double>(student.Key, student.Value));
                    printedStudents++;
                }
            }
        }

    }
}
