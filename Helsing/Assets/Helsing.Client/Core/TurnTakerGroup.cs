using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Helsing.Client.Core.Api;
using Helsing.Client.Extensions;

namespace Helsing.Client.Core
{
    public class TurnTakerGroup : ITurnTaker
    {
        public IList<ITurnTaker> TurnTakers => turnTakers;
        List<ITurnTaker> turnTakers = new List<ITurnTaker>();

        public async Task TakeTurn()
        {
            var valid = TurnTakers.Where(t => t != null).ToList();
            if (valid.Count == 0)
                return;

            await valid.AsyncForEach(async t => await t.TakeTurn());
        }
    }
}
