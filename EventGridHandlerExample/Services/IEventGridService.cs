using Microsoft.Azure.EventGrid;

namespace EventGridHandlerExample.Services
{
    using Microsoft.Azure.EventGrid.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IEventGridService
    {
        Task PublishTopic(EventGridEvent eventPayload);

        Task PublishTopic(string subject, string eventType, object eventData);

        Task PublishTopic(string topic, string subject, string eventType, object eventData);

        Task PublishTopic(string topic, string subject, string eventType, object eventData, string id, string metadataVersion, string dataVersion);

        SubscriptionValidationResponse ValidateWebhook(EventGridEvent payload);
    }
}
