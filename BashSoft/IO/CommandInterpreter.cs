namespace BashSoft
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using BashSoft.Attributes;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;
    using BashSoft.IO.Commands;

    public class CommandInterpreter : IInterpreter
    {
        private IContentComparer judge;
        private IDatabase repository;
        private IDirectoryManager inputOutputManager;

        public CommandInterpreter(IContentComparer judge, IDatabase repository, IDirectoryManager inputOutputManager)
        {
            this.judge = judge;
            this.repository = repository;
            this.inputOutputManager = inputOutputManager;
        }


        public void InterpretCommand(string input)
        {
            var data = input.Split(' ');
            var commandName = data[0];
            try
            {
                IExecutable command = this.ParseCommand(input, data, commandName);
                command.Execute();
            }
            catch (DirectoryNotFoundException dnfe)
            {
                OutputWriter.DisplayException(dnfe.Message);
            }
            catch (ArgumentOutOfRangeException aore)
            {
                OutputWriter.DisplayException(aore.Message);
            }
            catch (ArgumentException ae)
            {
                OutputWriter.DisplayException(ae.Message);
            }
            catch (Exception e)
            {
                OutputWriter.DisplayException(e.Message);
            }
        }

        private IExecutable ParseCommand(string input, string[] data, string commandName)
        {
            object[] parametersForConstruction = new object[]
            {
                input,
                data
            };

            Type commandType = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .First(t => t.GetCustomAttributes(typeof(AliasAttribute))
                .Where(atr => atr.Equals(commandName)).ToArray().Length > 0);

            Type interpreterType = typeof(CommandInterpreter);

            Command command = (Command)Activator.CreateInstance(commandType, parametersForConstruction);

            FieldInfo[] commandFields = commandType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo[] interpreterFields = interpreterType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var field in commandFields)
            {
                Attribute injectAttribute = field.GetCustomAttribute(typeof(InjectAttribute));
                if (injectAttribute != null)
                {
                    if (interpreterFields.Any(f => f.FieldType == field.FieldType))
                    {
                        field.SetValue(command, 
                            interpreterFields
                            .First(f => f.FieldType == field.FieldType)
                            .GetValue(this));
                    }
                }
            }

            return command;
        } 
    }
}
