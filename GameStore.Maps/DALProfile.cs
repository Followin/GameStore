using System;
using AutoMapper;
using GameStore.DAL.Northwind;
using GameStore.DAL.Static;
using GameStore.Domain.Entities;
using NorthOrder = GameStore.DAL.Northwind.Order;
using MainOrder = GameStore.Domain.Entities.Order;
using NorthShipper = GameStore.DAL.Northwind.Shipper;
using MainShipper = GameStore.Domain.Entities.Shipper;

namespace GameStore.Maps
{
    public class DALProfile : Profile
    {
        protected override void Configure()
        {

            Mapper.CreateMap<Category, Genre>()
                  .ForMember(x => x.Id, _ => _.MapFrom(x => KeyEncoder.Encode(x.CategoryID, DatabaseTypes.Northwind)))
                  .ForMember(x => x.ChildGenres, _ => _.Ignore())
                  .ForMember(x => x.NameEn, _ => _.MapFrom(x => x.CategoryName))
                  .ForMember(x => x.NameRu, _ => _.MapFrom(x => x.CategoryName))
                  .ForMember(x => x.ParentGenreId, _ => _.Ignore());

            Mapper.CreateMap<Product, Game>()
                  .ForMember(x => x.Id, _ => _.MapFrom(x => KeyEncoder.Encode(x.ProductID, DatabaseTypes.Northwind)))
                  .ForMember(x => x.Name, _ => _.MapFrom(x => x.ProductName))
                  .ForMember(x => x.Genres, _ => _.MapFrom(x => new[] {x.Category}))
                  .ForMember(x => x.Publisher, _ => _.MapFrom(x => x.Supplier))
                  .ForMember(x => x.PublisherId, _ => _.MapFrom(x => KeyEncoder.Encode(x.SupplierID.Value, DatabaseTypes.Northwind)))
                  .ForMember(x => x.Comments, _ => _.UseValue(new Comment[0]))
                  .ForMember(x => x.Discontinued, _ => _.MapFrom(x => x.Discontinued))
                  .ForMember(x => x.IncomeDate, _ => _.UseValue(DateTime.MinValue))
                  .ForMember(x => x.Key, _ => _.MapFrom(x => x.ProductID.ToString()))
                  .ForMember(x => x.Price, _ => _.MapFrom(x => x.UnitPrice))
                  .ForMember(x => x.UnitsInStock, _ => _.MapFrom(x => x.UnitsInStock))
                  .ForMember(x => x.PlatformTypes, _ => _.UseValue(new PlatformType[0]))
                  .ForMember(x => x.PublicationDate, _ => _.UseValue(DateTime.MinValue))
                  .ForMember(x => x.DescriptionEn, _ => _.Ignore())
                  .ForMember(x => x.DescriptionRu, _ => _.Ignore());

            Mapper.CreateMap<Supplier, Publisher>()
                  .ForMember(x => x.Id, _ => _.MapFrom(x => KeyEncoder.Encode(x.SupplierID, DatabaseTypes.Northwind)))
                  .ForMember(x => x.CompanyName, _ => _.MapFrom(x => x.CompanyName))
                  .ForMember(x => x.HomePage, _ => _.MapFrom(x => x.HomePage));

            Mapper.CreateMap<Order_Detail, OrderDetails>()
                  .ForMember(x => x.OrderId, _ => _.MapFrom(x => KeyEncoder.Encode(x.OrderID, DatabaseTypes.Northwind)))
                  .ForMember(x => x.GameId, _ => _.MapFrom(x => KeyEncoder.Encode(x.ProductID, DatabaseTypes.Northwind)))
                  .ForMember(x => x.Order, _ => _.Ignore())
                  .ForMember(x => x.Discount, _ => _.MapFrom(x => x.Discount))
                  .ForMember(x => x.Game, _ => _.MapFrom(x => x.Product))
                  .ForMember(x => x.Price, _ => _.MapFrom(x => x.UnitPrice))
                  .ForMember(x => x.Quantity, _ => _.MapFrom(x => x.Quantity));

            Mapper.CreateMap<NorthOrder, MainOrder>()
                  .ForMember(x => x.Id, _ => _.MapFrom(x => KeyEncoder.Encode(x.OrderID, DatabaseTypes.Northwind)))
                  .ForMember(x => x.OrderDetails, _ => _.MapFrom(x => x.Order_Details))
                  .ForMember(x => x.Payed, _ => _.UseValue(true))
                  .ForMember(x => x.Time, _ => _.MapFrom(x => x.OrderDate))
                  .ForMember(x => x.UserId, _ => _.UseValue(0));

            Mapper.CreateMap<NorthShipper, MainShipper>()
                  .ForMember(x => x.Id, _ => _.MapFrom(x => x.ShipperID))
                  .ForMember(x => x.CompanyName, _ => _.MapFrom(x => x.CompanyName))
                  .ForMember(x => x.Phone, _ => _.MapFrom(x => x.Phone));



        }
    }
}
