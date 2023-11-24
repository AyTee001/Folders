namespace Folders.DAL.Entities
{
    public class Folder(string name)
    {
        public long Id { get; set; }
        public string Name { get; set; } = name;
        public long? ParentId { get; set; }
        public Folder? Parent { get; set; }
        public ICollection<Folder>? Children { get; set;}
    }
}
