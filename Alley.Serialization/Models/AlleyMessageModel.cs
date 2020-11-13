namespace Alley.Serialization.Models
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
