namespace BashSoft
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BashSoft.Contracts;

    public class RepositorySorter : IDataSorter
    {
        public void OrderAndTake(Dictionary<string, double> studentsWithGrades,
            string comparison, int studentsToTake)
        {
            comparison = comparison.ToLower();
            if (comparison == "ascending")
            {
                this.PrintStudents(studentsWithGrades.OrderBy(x => x.Value)
                    .Take(studentsToTake)
                    .ToDictionary(x => x.Key, y => y.Value));
            }
            else if (comparison == "descending")
            {
                this.PrintStudents(studentsWithGrades.OrderByDescending(x => x.Value)
                    .Take(studentsToTake)
                    .ToDictionary(x => x.Key, y => y.Value));
            }
            else
            {
                throw new ArgumentException(ExceptionMessages.InvalidComparisonQuery);
            }
        }

        private void PrintStudents(Dictionary<string, double> sortedStudents)
        {
            foreach (var student in sortedStudents)
            {
                OutputWriter.PrintStudent(student);
            }
        }

      
    }
}
