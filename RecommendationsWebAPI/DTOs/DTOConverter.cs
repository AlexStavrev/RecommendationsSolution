using AutoMapper;

namespace RecommendationsWebAPI.DTOs;

public static class DTOConverter<T, U>
{
    private readonly static MapperConfiguration config = new(cfg =>
    {
        cfg.CreateMap<T, U>();
    });
    private readonly static Mapper mapper = new(config);

    //      OUTPUT                     <FROM>      <TO>               <SOURCE>
    //var bookingList = DtoConverter<BidDTO, Bid>.FromList(bidDTOList);
    //var bookingDto = DtoConverter<Signal, SignalDTO>.From(signal);
    //var booking = DtoConverter<AssetDTO, Asset>.From(assetDto);
    public static U From(T sourceObject) => mapper.Map<T, U>(sourceObject);
    public static IEnumerable<U> FromList(IEnumerable<T> sourceList) => sourceList.ToList().Select(obj => From(obj));
}
