using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helsing.Client.Api;

namespace Helsing.Client
{
    public class TurnTakerGroup : ITurnTaker
    {
        public IList<ITurnTaker> TurnTakers => turnTakers;
        List<ITurnTaker> turnTakers = new List<ITurnTaker>();

        public async Task TakeTurn()
        {
            var tasks = TurnTakers.Where(t => t != null).Select(t => t.TakeTurn());
            await Task.WhenAll(tasks);
        }
    }
}
