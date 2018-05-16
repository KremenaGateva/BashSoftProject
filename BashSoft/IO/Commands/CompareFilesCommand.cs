namespace BashSoft.IO.Commands
{
    using BashSoft.Attributes;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;

    [Alias("cmp")]
    public class CompareFilesCommand : Command
    {
        [Inject]
        private IContentComparer judge;

        public CompareFilesCommand(string input, string[] data)
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 3)
            {
                var firstFilePath = this.Data[1];
                var secondFilePath = this.Data[2];

                this.judge.CompareContent(firstFilePath, secondFilePath);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
