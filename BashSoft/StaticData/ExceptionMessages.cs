namespace BashSoft
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public static class ExceptionMessages
    {

        public const string UnauthorizedAccessExceptionMessage =
            "The folder/file you are trying to get access needs a higher level of rights than you currently have.";

        public const string ComparisonOfFilesWithDifferentSizes = "Files not of equal size, certain mismatch.";

        public const string UnableToGoHigherInPartitionHierarchy = "Unable to go higher in partition hierarchy";

        public const string UnableToParseNumber = "The sequence you've written is not a valid number.";

        public const string InvalidStudentFilter = "The given filter is not one of the following: excellent/average/poor";

        public const string InvalidComparisonQuery =
            "The comparison query you want, does not exist in the context of the current program!";

        public const string InvalidTakeCommand = "The take command expected does not match the format wanted!";

        public const string InvalidTakeQuantityParameter = "The take quantity parameter does not match the format wanted!";

        public const string InvalidNumberOfScores = "The number of scores for the given course is greater than the possible.";

        public const string InvalidScore = "The given score is not valid.";

    }
}
