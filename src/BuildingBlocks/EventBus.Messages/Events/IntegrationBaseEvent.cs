using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Messages.Events
{
    /***
     * Every rabbitmq message will be inherited from this intergration base event 
     * including common properities from all events.
    ***/

    public class IntegrationBaseEvent
    {

        public IntegrationBaseEvent()
        {
            Id = Guid.NewGuid().ToString();
            CreationDate = DateTime.UtcNow;
        }

        public IntegrationBaseEvent(string id, DateTime creationdate)
        {
            Id=id;
            CreationDate=creationdate;
        }
        public string Id { get; private set; } = Guid.NewGuid().ToString();
        public DateTime CreationDate { get; private set; }
    }
}
