namespace BashSoft
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using BashSoft.Contracts;
    using BashSoft.DataStructures;
    using BashSoft.Exceptions;
    using BashSoft.Models;

    public class StudentRepository : IDatabase
    {
        private Dictionary<string, ICourse> courses;
        private Dictionary<string, IStudent> students;
        private bool isDataInitialized;
        private IDataFilter filter;
        private IDataSorter sorter;

        public StudentRepository(IDataFilter filter, IDataSorter sorter)
        {
            this.filter = filter;
            this.sorter = sorter;
        }

        public void UnloadData()
        {
            if (!this.isDataInitialized)
            {
                throw new DataNotInitializedException();
            }
            this.students = null;
            this.courses = null;
            this.isDataInitialized = false;
        }

        public void LoadData(string fileName)
        {
            if (this.isDataInitialized)
            {
                 throw new DataInitializedException();
            }
           
            OutputWriter.WriteMessageOnNewLine("Reading data...");
            this.students = new Dictionary<string, IStudent>();
            this.courses = new Dictionary<string, ICourse>();
            this.ReadData(fileName);
        }

        private void ReadData(string fileName)
        {
            var path = SessionData.currentPath + '\\' + fileName;

            if (File.Exists(path))
            {
                var pattern = @"([A-Z][a-zA-Z#\+]*_[A-Z][a-z]{2}_\d{4})\s+([A-Za-z]+\d{2}_\d{2,4})\s([\s0-9]+)";
                var regex = new Regex(pattern);
                var allInputLines = File.ReadAllLines(path);
                for (int line = 0; line < allInputLines.Length; line++)
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(allInputLines[line]) && regex.IsMatch(allInputLines[line]))
                        {
                            var currentMatch = regex.Match(allInputLines[line]);
                            var courseName = currentMatch.Groups[1].Value;
                            var username = currentMatch.Groups[2].Value;
                            var scoresStr = currentMatch.Groups[3].Value;

                            var scores = scoresStr.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray();

                            if (scores.Any(s => (s > 100 || s < 0)))
                            {
                                OutputWriter.DisplayException(ExceptionMessages.InvalidScore);
                                continue;
                            }
                            if (scores.Length > SoftUniCourse.NumberOfTasksOnExam)
                            {
                                OutputWriter.DisplayException(ExceptionMessages.InvalidNumberOfScores);
                                continue;
                            }

                            if (!this.students.ContainsKey(username))
                            {
                                this.students.Add(username, new SoftUniStudent(username));
                            }
                            if (!this.courses.ContainsKey(courseName))
                            {
                                this.courses.Add(courseName, new SoftUniCourse(courseName));
                            }

                            var course = this.courses[courseName];
                            var student = this.students[username];

                            student.EnrollInCourse(course);
                            student.SetGradesOnCourse(courseName, scores);

                            course.EnrollStudent(student);
                        }
                    }
                    catch (FormatException ex)
                    {
                        OutputWriter.DisplayException(ex.Message + $"at line: {line}");
                    }
                }
            }
            else
            {
                throw new InvalidPathException();
            }
            isDataInitialized = true;
            OutputWriter.WriteMessageOnNewLine("Data read!");
        }

        private bool IsQueryForCoursePossible(string courseName)
        {
            if (isDataInitialized)
            {
                if (this.courses.ContainsKey(courseName))
                {
                    return true;
                }
                else
                {
                    throw new InexistingCourseException();
                }
            }
            else
            {
                throw new DataNotInitializedException();
            }
        }

        private bool IsQueryForStudentPossible(string courseName, string studentUserName)
        {
            if (IsQueryForCoursePossible(courseName) 
                && this.courses[courseName].StudentsByName.ContainsKey(studentUserName))
            {
                return true;
            }
            else
            {
                throw new InexistingStudentException();
            }
        }

        public void GetStudentScoresFromCourse(string courseName, string studentUserName)
        {
            if (IsQueryForStudentPossible(courseName, studentUserName))
            {
                OutputWriter.PrintStudent(
                    new KeyValuePair<string, double>(studentUserName, 
                    this.courses[courseName].StudentsByName[studentUserName].GradesByCourseName[courseName]));
            }
        }

        public void GetAllStudentsFromCourse(string courseName)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                OutputWriter.WriteMessageOnNewLine($"{courseName}:");
                foreach (var studentGrades in this.courses[courseName].StudentsByName)
                {
                    this.GetStudentScoresFromCourse(courseName, studentGrades.Key);
                }
            }
        }

        public void FilterAndTake(string courseName, string givenFilter, int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = this.courses[courseName].StudentsByName.Count;
                }
                var grades = this.courses[courseName].StudentsByName
                    .ToDictionary(x => x.Key, x => x.Value.GradesByCourseName[courseName]);
                filter.FilterAndTake(grades, givenFilter, studentsToTake.Value);
            }
        }

        public void OrderAndTake(string courseName, string comparison, int? studentsToTake = null)
        {
            if (IsQueryForCoursePossible(courseName))
            {
                if (studentsToTake == null)
                {
                    studentsToTake = this.courses[courseName].StudentsByName.Count;
                }
                var grades = this.courses[courseName].StudentsByName
                    .ToDictionary(x => x.Key, x => x.Value.GradesByCourseName[courseName]);
                 sorter.OrderAndTake(grades, comparison, studentsToTake.Value);
            }
        }

        public ISimpleOrderedBag<ICourse> GetAllCoursesSorted(IComparer<ICourse> comparer)
        {
            ISimpleOrderedBag<ICourse> sortedCourses = new SimpleSortedList<ICourse>(comparer);
            sortedCourses.AddAll(this.courses.Values);

            return sortedCourses;
        }

        public ISimpleOrderedBag<IStudent> GetAllStudentsSorted(IComparer<IStudent> comparer)
        {
            ISimpleOrderedBag<IStudent> sortedStudents = new SimpleSortedList<IStudent>(comparer);
            sortedStudents.AddAll(this.students.Values);

            return sortedStudents;
        }
    }
}
