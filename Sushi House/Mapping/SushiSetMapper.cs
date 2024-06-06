using Sushi_House.DTOModels;
using Sushi_House.Models;

namespace Sushi_House.Mapping
{
    public class SushiSetMapper
    {
        public static SushiSetDTO toDTO(SushiSet sushiSet)
        {
            return new SushiSetDTO
            {
                SushiSetId = sushiSet.SushiSetId,
                SushiSetSetId = sushiSet.SushiSetSetId,
                SushiSetSushiId = sushiSet.SushiSetSushiId
            };
        }

        public static SushiSet toEntity(SushiSetDTO sushiSetDTO)
        {
            return new SushiSet
            {
                SushiSetId = sushiSetDTO.SushiSetId,
                SushiSetSetId = sushiSetDTO.SushiSetSetId,
                SushiSetSushiId = sushiSetDTO.SushiSetSushiId
            };
        }
    }
}
