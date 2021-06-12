﻿using System.Collections.Generic;
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
            var valid = TurnTakers.Where(t => t != null);
            if (valid.Count() == 0)
                return;

            var tasks = valid.Select(t => t.TakeTurn()).ToList();
            await tasks.AsyncForEach(async t => await t);
        }
    }
}