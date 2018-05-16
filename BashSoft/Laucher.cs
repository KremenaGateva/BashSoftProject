namespace BashSoft
{
    using System;
    using BashSoft.Contracts;

    public class Laucher
    {
        public static void Main()
        {
            IContentComparer tester = new Tester();
            IDirectoryManager inputOutputManager = new IOManager();
            IDatabase repository = new StudentRepository(new RepositoryFilter(), new RepositorySorter());

            IInterpreter interpreter = new CommandInterpreter(tester, repository, inputOutputManager);
            IReader reader = new InputReader(interpreter);

            reader.StartReadingCommands();
        }
    }
}
