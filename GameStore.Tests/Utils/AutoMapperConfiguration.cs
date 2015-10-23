using AutoMapper;
using GameStore.Maps;
using GameStore.Web.Utils;

namespace GameStore.Tests.Utils
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new BLLProfile());
                cfg.AddProfile(new WebMapperProfile());
            });
        }
    }
}
