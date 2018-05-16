namespace BashSoft
{
    using System;
    using BashSoft.Contracts;

    public class InputReader : IReader
    {
        private const string endCommand = "quit";

        private IInterpreter interpreter;

        public InputReader(IInterpreter interpreter)
        {
            this.interpreter = interpreter;
        }

        public void StartReadingCommands()
        {
            OutputWriter.WriteMessage($"{SessionData.currentPath}>");
            var input = Console.ReadLine().Trim();

            while (input != endCommand)
            {
                this.interpreter.InterpretCommand(input);
                OutputWriter.WriteMessage($"{SessionData.currentPath}>");
                input = Console.ReadLine().Trim();
            }

        }
    }
}
