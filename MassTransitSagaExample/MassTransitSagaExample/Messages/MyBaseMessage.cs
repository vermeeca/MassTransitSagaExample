using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MassTransit;

namespace MassTransitSagaExample.Messages
{
    public abstract class MyBaseMessage : CorrelatedBy<Guid>
    {

        #region CorrelatedBy<Guid> Members

        public Guid CorrelationId { get; set; }

        #endregion
    }
}
