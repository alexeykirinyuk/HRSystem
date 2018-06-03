namespace HRSystem.Domain
{
    public class Document
    {
        public string Name { get; }
        
        public byte[] Content { get; }

        public Document(string name, byte[] content)
        {
            Name = name;
            Content = content;
        }
    }
}