using Sushi_House.DTOModels;
using Sushi_House.Models;

namespace Sushi_House.Mapping
{
    public class SushiMapper
    {
        public static SushiDTO toDTO(Sushi sushi)
        {
            return new SushiDTO
            {
                SushiId = sushi.SushiId,
                SushiName = sushi.SushiName,
                SushiPrice = sushi.SushiPrice,
                SushiInqr = sushi.SushiInqr,
                SushiPicName = sushi.SushiPicName,
                SushiTypeId = sushi.SushiTypeId
            };
        }

        public static Sushi toEntity(SushiDTO sushiDTO)
        {
            return new Sushi
            {
                SushiId = sushiDTO.SushiId,
                SushiName = sushiDTO.SushiName,
                SushiPrice = sushiDTO.SushiPrice,
                SushiInqr = sushiDTO.SushiInqr,
                SushiPicName = sushiDTO.SushiPicName,
                SushiTypeId = sushiDTO.SushiTypeId
            };
        }
    }
}
