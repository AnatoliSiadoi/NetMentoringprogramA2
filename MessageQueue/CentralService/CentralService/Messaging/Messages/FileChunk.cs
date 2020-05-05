
namespace Messages
{
    public class FileChunk
    {
        public string FileName { get; set; }
        public byte[] Chunk { get; set; }
        public int Order { get; set; }
        public double Count { get; set; }
    }
}
