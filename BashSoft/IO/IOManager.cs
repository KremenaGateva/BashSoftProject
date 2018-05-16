namespace BashSoft
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;

    public class IOManager : IDirectoryManager
    {
        public void TraverseDirectory(int depth)
        {
            OutputWriter.WriteEmptyLine();
            var initialIdentation = SessionData.currentPath.Split('\\').Length;
            var subFolders = new Queue<string>();
            subFolders.Enqueue(SessionData.currentPath);

            while (subFolders.Count > 0)
            {
                var currantPath = subFolders.Dequeue();
                var identation = currantPath.Split('\\').Length - initialIdentation;
                if (depth - identation < 0)
                {
                    break;
                }

                OutputWriter.WriteMessageOnNewLine($"{new string('-', identation)}{currantPath}");
                try
                {
                    foreach (var file in Directory.GetFiles(currantPath))
                    {
                        var indexOfLastSlash = file.LastIndexOf('\\');
                        var fileName = file.Substring(indexOfLastSlash);
                        OutputWriter.WriteMessageOnNewLine(new string('-', indexOfLastSlash) + fileName);
                    }
                    foreach (var subFolderPath in Directory.GetDirectories(currantPath))
                    {
                        subFolders.Enqueue(subFolderPath);
                    }
                }
                catch (UnauthorizedAccessException)
                {

                    OutputWriter.DisplayException(ExceptionMessages.UnauthorizedAccessExceptionMessage);
                }
                
            }
        }

        public void CreateDirectoryInCurrentFolder(string name)
        {
            var path = Directory.GetCurrentDirectory() + '\\' + name;
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (ArgumentException)
            {
                throw new InvalidFileNameException();
            }
        }

        public void ChangeCurrentDirectoryRelative(string relativePath)
        {
            if (relativePath == "..")
            {
                try
                {
                    var currentPath = SessionData.currentPath;
                    var indexOfLastSlash = currentPath.LastIndexOf('\\');
                    var newPath = currentPath.Substring(0, indexOfLastSlash);
                    SessionData.currentPath = newPath;
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new ArgumentOutOfRangeException("indexOfLastSlash", ExceptionMessages.UnableToGoHigherInPartitionHierarchy);
                }
            }
            else
            {
                var currentPath = SessionData.currentPath;
                currentPath += relativePath;
                ChangeCurrentDirectoryAbsolute(currentPath);
            }
        }

        public void ChangeCurrentDirectoryAbsolute(string absolutePath)
        {
            if (!Directory.Exists(absolutePath))
            {
                throw new InvalidPathException();
            }
            else
            {
                SessionData.currentPath = absolutePath;
            }
        }
    }
}
