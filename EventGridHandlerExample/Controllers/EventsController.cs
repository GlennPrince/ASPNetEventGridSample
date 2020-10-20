using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventGridHandlerExample.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EventGridHandlerExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IEventGridService _eventGrid;

        public EventsController(ILogger<EventsController> logger, IEventGridService eventGrid)
        {
            _logger = logger;
            _eventGrid = eventGrid;
        }

        [HttpPost]
        public IActionResult Listener([FromBody] EventGridEvent[] events)
        {
            foreach (var eventGridEvent in events)
            {
                _logger.Log(LogLevel.Information, "Event Grid Event Received. Type: " + eventGridEvent.EventType.ToString());

                // 1. If there is no EventType through a bad request
                if (eventGridEvent == null) return BadRequest();

                // 2. If the EventType is the Event Grid handshake event, respond with a SubscriptionValidationResponse.
                else if (eventGridEvent.EventType == EventTypes.EventGridSubscriptionValidationEvent)
                    return Ok(_eventGrid.ValidateWebhook(eventGridEvent));

                // 3. If the EventType is a return message, send a message to Discord
                else if (eventGridEvent.EventType == "sample.eventgridhandler.testappmessage")
                {
                    _logger.Log(LogLevel.Information, "Message Received: " + eventGridEvent.Data.ToString());
                    return Ok();
                }
                else
                {
                    _logger.Log(LogLevel.Error, "Unhandled Message Type: " + eventGridEvent.EventType);
                    return BadRequest();
                }
            }
            return BadRequest();
        }
    }
}
