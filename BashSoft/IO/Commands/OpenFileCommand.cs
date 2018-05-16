namespace BashSoft.IO.Commands
{
    using System.Diagnostics;
    using BashSoft.Attributes;
    using BashSoft.Exceptions;

    [Alias("open")]
    public class OpenFileCommand : Command
    {
        public OpenFileCommand(string input, string[] data)
            : base(input, data)
        {
        }

        public override void Execute()
        {
            if (this.Data.Length == 2)
            {
                var fileName = this.Data[1];
                Process.Start(SessionData.currentPath + '\\' + fileName);
            }
            else
            {
                throw new InvalidCommandException(this.Input);
            }
        }
    }
}
