using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GameStore.BLL.Utils;
using GameStore.Web.Utils;

namespace GameStore.Tests.Utils
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new BLLMapperProfile());
                cfg.AddProfile(new WebMapperProfile());
            });
        }
    }
}
