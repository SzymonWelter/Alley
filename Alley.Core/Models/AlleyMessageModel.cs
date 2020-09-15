namespace Alley.Core.Models
{
    internal class AlleyMessageModel : IAlleyMessageModel
    {
        public byte[] Content { get; set; }

        public AlleyMessageModel(byte[] content)
        {
            Content = content;
        }
    }
}
