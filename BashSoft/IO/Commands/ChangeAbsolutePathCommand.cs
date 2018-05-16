namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;

    [Alias("cdAbs")]
    public class ChangeAbsolutePathCommand : Command
    {
        [Inject]
        private IDirectoryManager inputOutputManager;

        public ChangeAbsolutePathCommand(string input, string[] data)
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                var absolutePath = this.Data[1];
                this.inputOutputManager.ChangeCurrentDirectoryAbsolute(absolutePath);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
