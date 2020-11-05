using Alley.Definitions.Models.Interfaces;

namespace Alley.Definitions.Models
{
    public class AlleyMessageModel : IAlleyMessageModel
    {
        public byte[] Content { get; set; }

        public AlleyMessageModel(byte[] content)
        {
            Content = content;
        }
    }
}
