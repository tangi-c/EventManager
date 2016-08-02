using EventManager.Business.dsEventManagerTableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Business
{
    class EventView
    {
        public int EventId { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan DoorTime { get; set; }
        public TimeSpan CurfewTime { get; set; }
        public double PromoterCharge { get; set; }
        public double SecurityCost { get; set; }
        public double SoundCost { get; set; }
        public double LightCost { get; set; }
        public int PromoterId { get; set; }
        public string PromoterName { get; set; }
        public int LineUpId { get; set; }
        public string HeadlineName { get; set; }


        public EventView ()
        {
            EventDate = new DateTime();
            DoorTime = new TimeSpan();
            CurfewTime = new TimeSpan();
        }

        public static List<EventView> DisplayAllEvents ()
        {
            List<EventView> returnEvents = new List<EventView>();
            List<Event> allEvents = Event.GetAllEvents();
            EventsTableAdapter taEvents = new EventsTableAdapter();

            foreach (Event e in allEvents)
            {
                EventView currentEvent = new EventView();
                currentEvent.EventId = e.EventId;
                currentEvent.EventDate = e.EventDate;
                currentEvent.DoorTime = e.DoorTime;
                currentEvent.CurfewTime = e.CurfewTime;
                currentEvent.PromoterCharge = e.PromoterCharge;
                currentEvent.SecurityCost = e.SecurityCost;
                currentEvent.SoundCost = e.SoundCost;
                currentEvent.LightCost = e.LightCost;
                currentEvent.PromoterId = Convert.ToInt32(taEvents.GetPromoterIdByEventId(currentEvent.EventId));
                currentEvent.PromoterName = e.EventPromoter.ContactName;
                currentEvent.LineUpId = Convert.ToInt32(taEvents.GetLineUpIdByEventId(currentEvent.EventId));
                currentEvent.HeadlineName = e.EventLineUp.Headline;
                returnEvents.Add(currentEvent);
            }
            return returnEvents;
        }
    }
}
