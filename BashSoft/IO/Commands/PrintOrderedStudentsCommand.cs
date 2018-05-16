﻿namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;

    [Alias("order")]
    public class PrintOrderedStudentsCommand : Command
    {
        [Inject]
        private IDatabase repository;

        public PrintOrderedStudentsCommand(string input, string[] data)
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 5)
            {
                var courseName = this.Data[1];
                var comparison = this.Data[2].ToLower();
                var takeCommand = this.Data[3].ToLower();
                var takeQuantity = this.Data[4].ToLower();

                ParseParametersForOrderAndTake(takeCommand, takeQuantity, courseName, comparison);

            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }

        private void ParseParametersForOrderAndTake(string takeCommand, string takeQuantity, string courseName, string comparison)
        {
            if (takeCommand == "take")
            {
                if (takeQuantity == "all")
                {
                    this.repository.OrderAndTake(courseName, comparison);
                }
                else
                {
                    int quantity;
                    var hasParsed = int.TryParse(takeQuantity, out quantity);
                    if (hasParsed)
                    {
                        this.repository.OrderAndTake(courseName, comparison, quantity);
                    }
                    else
                    {
                        OutputWriter.DisplayException(ExceptionMessages.InvalidTakeQuantityParameter);
                    }
                }
            }
            else
            {
                OutputWriter.DisplayException(ExceptionMessages.InvalidTakeCommand);
            }
        }
    }
}
