using Sushi_House.Models;

namespace Sushi_House.Services
{
    public interface ISushiQueryService
    {
        List<Sushi> GetSushi();
        List<Set> GetSet();
    }
}
