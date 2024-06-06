using Sushi_House.DTOModels;
using Sushi_House.Models;

namespace Sushi_House.Mapping
{
    public class STypeMapper
    {
        public static STypeDTO toDTO(Stype stype)
        {
            return new STypeDTO
            {
                StypeId = stype.StypeId,
                StypeName = stype.StypeName
            };
        }
    }
}
