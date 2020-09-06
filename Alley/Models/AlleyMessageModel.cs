namespace Alley.Models
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
