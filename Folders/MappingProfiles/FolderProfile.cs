using AutoMapper;
using Folders.DAL.Entities;
using Folders.DTOs;

namespace Folders.MappingProfiles
{
    public class FolderProfile : Profile
    {
        public FolderProfile() 
        {
            CreateMap<Folder, FolderStorageDto>().ReverseMap();
        }
    }
}
