namespace Folders.DTOs
{
    public class FolderStorageDto(string name)
    {
        public long Id { get; set; }
        public string Name { get; set; } = name;
        public long? ParentId { get; set; }
    }
}
