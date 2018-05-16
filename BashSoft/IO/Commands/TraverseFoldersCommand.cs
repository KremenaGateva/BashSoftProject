namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;

    [Alias("ls")]
    public class TraverseFoldersCommand : Command
    {
        [Inject]
        private IDirectoryManager inputOutputManager;

        public TraverseFoldersCommand(string input, string[] data)
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                int depth;
                if (!int.TryParse(this.Data[1], out depth))
                {
                    OutputWriter.DisplayException(ExceptionMessages.UnableToParseNumber);
                }
                this.inputOutputManager.TraverseDirectory(depth);
            }
            else if (this.Data.Length == 1)
            {
                this.inputOutputManager.TraverseDirectory(0);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
