using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreService_AT.MessageBuss
{
    internal interface IMessageBus
    {
        Task PublicMessage(BaseMessage message, string queueName);
    }
}
