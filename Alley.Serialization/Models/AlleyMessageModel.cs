namespace Alley.Serialization.Models
{
    public class AlleyMessageModel : IAlleyMessageModel
    {
        public byte[] Content { get; }

        public AlleyMessageModel(byte[] content)
        {
            Content = content;
        }
    }
}
