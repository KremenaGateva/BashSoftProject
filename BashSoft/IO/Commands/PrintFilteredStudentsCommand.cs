namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;

    [Alias("filter")]
    public class PrintFilteredStudentsCommand : Command
    {
        [Inject]
        private IDatabase repository;

        public PrintFilteredStudentsCommand(string input, string[] data)
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 5)
            {
                var courseName = this.Data[1];
                var filter = this.Data[2].ToLower();
                var takeCommand = this.Data[3].ToLower();
                var takeQuantity = this.Data[4].ToLower();

                ParseParametersForFilterAndTake(takeCommand, takeQuantity, courseName, filter);

            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }

        private void ParseParametersForFilterAndTake(string takeCommand, string takeQuantity, string courseName, string filter)
        {
            if (takeCommand == "take")
            {
                if (takeQuantity == "all")
                {
                    this.repository.FilterAndTake(courseName, filter);
                }
                else
                {
                    int quantity;
                    var hasParsed = int.TryParse(takeQuantity, out quantity);
                    if (hasParsed)
                    {
                        this.repository.FilterAndTake(courseName, filter, quantity);
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
