using Sushi_House.DTOModels;
using Sushi_House.Models;

namespace Sushi_House.Mapping
{
    public class SetMapper
    {
        public static SetDTO toDTO(Set set)
        {
            return new SetDTO
            {
                SetId = set.SetId,
                SetName = set.SetName,
                SetPicName = set.SetPicName
            };
        }

        public static Set toEntity(SetDTO setDTO)
        {
            return new Set
            {
                SetId = setDTO.SetId,
                SetName = setDTO.SetName,
                SetPicName = setDTO.SetPicName
            };
        }
    }
}
