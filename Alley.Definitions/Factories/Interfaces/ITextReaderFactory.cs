using System.IO;
using Alley.Utils.Models;

namespace Alley.Definitions.Factories.Interfaces
{
    public interface ITextReaderFactory
    {
        IResult<TextReader> Create(string fileFullName);
    }
}