using AutoMapper;
using ITJob.Services.ViewModels.AlbumImage;

namespace ITJob.Services.ViewModels.Configs;

public static class AlbumImageMapper
{
    public static void ConfigAlbumImage(this IMapperConfigurationExpression configuration)
    {
        configuration.CreateMap<ITJob.Entity.Entities.AlbumImage, GetAlbumImageDetail>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.AlbumImage, CreateAlbumImageModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.AlbumImage, CreateAlbumImageUrlModel>().ReverseMap();
        configuration.CreateMap<ITJob.Entity.Entities.AlbumImage, UpdateAlbumImageModel>().ReverseMap();
    }
}