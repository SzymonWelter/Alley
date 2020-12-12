using System;
using System.IO;
using Alley.Definitions.Factories.Interfaces;
using Alley.Utils.Models;

namespace Alley.Definitions.Factories
{
    public class TextReaderFactory : ITextReaderFactory
    {
        public IResult<TextReader> Create(string fileFullName)
        {
            try
            {
                var stream = new StreamReader(fileFullName);
                return Result<TextReader>.Success(stream);
            }
            catch (Exception e)
            {
                return Result<TextReader>.Failure(e.Message);
            }
        }
    }
}