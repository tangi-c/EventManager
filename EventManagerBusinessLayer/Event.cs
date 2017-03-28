using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManager.Business.dsEventManagerTableAdapters;
using System.Globalization;

namespace EventManager.Business
{
    public class Event : IComparable<Event>
    {
        // properties
        public int EventId { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan DoorTime { get; set; }
        public TimeSpan CurfewTime { get; set; }
        public double PromoterCharge { get; set; }
        public double SecurityCost { get; set; }
        public double SoundCost { get; set; }
        public double LightCost { get; set; }
        public Promoter EventPromoter { get; set; }
        public LineUp EventLineUp { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public Event()
        {
            EventDate = new DateTime();
            DoorTime = new TimeSpan();
            CurfewTime = new TimeSpan();
            EventPromoter = new Promoter();
            EventLineUp = new LineUp();
        }

        /// <summary>
        /// constructor with all properties intialised
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="eventDate"></param>
        /// <param name="doorTime"></param>
        /// <param name="curfewTime"></param>
        /// <param name="promoterCharge"></param>
        /// <param name="securityCost"></param>
        /// <param name="soundCost"></param>
        /// <param name="lightCost"></param>
        /// <param name="eventPromoter"></param>
        /// <param name="eventLineUp"></param>
        public Event(int eventId, DateTime eventDate, TimeSpan doorTime, TimeSpan curfewTime, double promoterCharge, double securityCost, double soundCost, double lightCost, Promoter eventPromoter, LineUp eventLineUp)
            : this()
        {
            this.EventId = eventId;
            this.EventDate = eventDate;
            this.DoorTime = doorTime;
            this.CurfewTime = curfewTime;
            this.PromoterCharge = promoterCharge;
            this.SecurityCost = securityCost;
            this.SoundCost = soundCost;
            this.LightCost = lightCost;
            this.EventPromoter = eventPromoter;
            this.EventLineUp = eventLineUp;
        }

        /// <summary>
        /// Contructor for an event without id, line up and promoter.
        /// </summary>
        /// <param name="eventDate"></param>
        /// <param name="doorTime"></param>
        /// <param name="curfewTime"></param>
        /// <param name="promoterCharge"></param>
        /// <param name="securityCost"></param>
        /// <param name="soundCost"></param>
        /// <param name="lightCost"></param>
        /// <param name="eventPromoter"></param>
        /// <param name="eventLineUp"></param>
        public Event(DateTime eventDate, TimeSpan doorTime, TimeSpan curfewTime, double promoterCharge, double securityCost, double soundCost, double lightCost)
             : this()
        {
            this.EventId = -1;
            this.EventDate = eventDate;
            this.DoorTime = doorTime;
            this.CurfewTime = curfewTime;
            this.PromoterCharge = promoterCharge;
            this.SecurityCost = securityCost;
            this.SoundCost = soundCost;
            this.LightCost = lightCost;
        }

        /// <summary>
        /// constructor for an event without an Id.
        /// </summary>
        /// <param name="eventDate"></param>
        /// <param name="doorTime"></param>
        /// <param name="curfewTime"></param>
        /// <param name="promoterCharge"></param>
        /// <param name="securityCost"></param>
        /// <param name="soundCost"></param>
        /// <param name="lightCost"></param>
        /// <param name="eventPromoter"></param>
        /// <param name="eventLineUp"></param>
        public Event(DateTime eventDate, TimeSpan doorTime, TimeSpan curfewTime, double promoterCharge, double securityCost, double soundCost, double lightCost, Promoter eventPromoter, LineUp eventLineUp)
             : this()
        {
            this.EventId = -1;
            this.EventDate = eventDate;
            this.DoorTime = doorTime;
            this.CurfewTime = curfewTime;
            this.PromoterCharge = promoterCharge;
            this.SecurityCost = securityCost;
            this.SoundCost = soundCost;
            this.LightCost = lightCost;
            this.EventPromoter = eventPromoter;
            this.EventLineUp = eventLineUp;
        }

        /// <summary>
        /// Get all events from the database
        /// </summary>
        /// <returns>list of events</returns>
        public static List<Event> GetAllEventsFromDb()
        {
            EventsTableAdapter taEvents = new EventsTableAdapter();
            var dtEvents = taEvents.GetData();
            List<Event> allEvents = new List<Event>();
            foreach (dsEventManager.EventsRow eventsRow in dtEvents.Rows)
            {
                Event currentEvent = new Event();
                currentEvent.EventId = eventsRow.Id;
                currentEvent.EventDate = eventsRow.EventDate;
                currentEvent.DoorTime = eventsRow.DoorTime;
                currentEvent.CurfewTime = eventsRow.CurfewTime;
                currentEvent.PromoterCharge = eventsRow.PromoterCharge;
                currentEvent.SecurityCost = eventsRow.SecurityCost;
                currentEvent.SoundCost = eventsRow.SoundCost;
                currentEvent.LightCost = eventsRow.LightCost;
                allEvents.Add(currentEvent);
            }
            dtEvents.Clear();
            return allEvents;
        }

        /// <summary>
        /// Get all stored events from database and the corresponding promoter and line up.
        /// </summary>
        /// <returns>List of Event</returns>
        public static List<Event> GetAllEvents()
        {
            List<Event> allEvents = GetAllEventsFromDb();
            EventsTableAdapter taEvents = new EventsTableAdapter();

            foreach (Event currentEvent in allEvents)
            {
                currentEvent.EventPromoter = Promoter.GetPromoterById(Convert.ToInt32(taEvents.GetPromoterIdByEventId(currentEvent.EventId)));
                currentEvent.EventLineUp = LineUp.GetLineUpById(Convert.ToInt32(taEvents.GetLineUpIdByEventId(currentEvent.EventId)));
            }
            return allEvents;
        }

        /// <summary>
        /// Get events from database and sorts them chronoligically.
        /// </summary>
        /// <returns></returns>
        public static List<Event> GetAllEventsSorted()
        {
            List<Event> allEvents = GetAllEvents();
            allEvents.Sort();
            return allEvents;
        }

        /// <summary>
        /// returns a list of events containing the query string.
        /// if the query is invalid, i.e. empty string, returns all events.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<Event> SearchEvents(string query)
        {
            //1. check if the search is valid.
            if (!string.IsNullOrWhiteSpace(query))
            {
                //2. if the query is valid create an empty list.
                List<Event> returnEvents = new List<Event>();

                //3. search line ups and add corresponding events to the list
                List<LineUp> foundLineUp = LineUp.SearchLineUps(query);
                Event_LineUpTableAdapter taE_LU = new Event_LineUpTableAdapter();
                foreach (LineUp lu in foundLineUp)
                {
                    returnEvents.Add(GetEventById(Convert.ToInt32(taE_LU.GetEventIdByLineUpId(lu.LineUpId))));
                }

                //4. serch promoters and add corresponding events to the list without creating duplicates
                List<Promoter> foundPromoter = Promoter.SearchPromoters(query);
                Event_PromoterTableAdapter taE_P = new Event_PromoterTableAdapter();
                // for each promoter found
                foreach (Promoter p in foundPromoter)
                {
                    // get the corrsponding events
                    var dtE_P = taE_P.GetEventIdsByPromoterId(p.PromoterId);
                    // for each one of those events
                    foreach (dsEventManager.Event_PromoterRow row in dtE_P)
                    {
                        // check if they already are in the return list
                        bool duplicate = false;
                        foreach (Event e in returnEvents)
                        {
                            if (e.EventId == row.EventId)
                                duplicate = true;
                        }
                        // if not, add them
                        if (!duplicate)
                            returnEvents.Add(GetEventById(row.EventId));
                    }

                }

                //5. order the final list and return it.
                returnEvents.Sort();
                return returnEvents;
            }
            //6. if the query isn't valid, return all the events in order.
            else
                return GetAllEventsSorted();
        }

        /// <summary>
        /// Returns the event corresponding to the ID.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        private static Event GetEventById(int eventId)
        {
            // get the event information from the database.
            EventsTableAdapter taEvent = new EventsTableAdapter();
            var dtEvent = taEvent.GetEventById(eventId);

            // parse the received information.
            Event returnEvent = new Event();
            // there should have only been one row returned.
            foreach (dsEventManager.EventsRow eventsRow in dtEvent.Rows)
            {
                returnEvent.EventId = eventsRow.Id;
                returnEvent.EventDate = eventsRow.EventDate;
                returnEvent.DoorTime = eventsRow.DoorTime;
                returnEvent.CurfewTime = eventsRow.CurfewTime;
                returnEvent.PromoterCharge = eventsRow.PromoterCharge;
                returnEvent.SecurityCost = eventsRow.SecurityCost;
                returnEvent.SoundCost = eventsRow.SoundCost;
                returnEvent.LightCost = eventsRow.LightCost;
            }
            // get the promoter and the line up, the return the event.
            returnEvent.EventPromoter = Promoter.GetPromoterById(Convert.ToInt32(taEvent.GetPromoterIdByEventId(returnEvent.EventId)));
            returnEvent.EventLineUp = LineUp.GetLineUpById(Convert.ToInt32(taEvent.GetLineUpIdByEventId(returnEvent.EventId)));
            return returnEvent;
        }

        /// <summary>
        /// finds the position of the first event happening from yesterday.
        /// We look for yesterday rather than today as events regularly run past midnight.
        /// if all the events are in the past, it returns the last event's postition.
        /// </summary>
        /// <param name="allEvents"></param>
        /// <returns></returns>
        public static int IndexForYesterday(List<Event> allEvents)
        {
            int index = 0;
            foreach (Event current in allEvents)
            {
                // compare the current event date to yeaterday and break or increment accordingly.
                if (current.EventDate.CompareTo(DateTime.Today.AddDays(-1)) >= 0)
                    break;
                // make sure the index doesn't get incremented if it's the last event.
                else if (index < (allEvents.Count - 1))
                    index++;
            }
            // return the index found.
            return index;
        }

        /// <summary>
        /// add this event to the database.
        /// </summary>
        public void AddToDb()
        {
            // PROMOTER
            // only add the promoter if it doesn't already exists and is not empty.
            if ((EventPromoter.PromoterId < 0) && ((EventPromoter.ContactName != "") || (EventPromoter.CompanyName != "") || (EventPromoter.Phone != "") || (EventPromoter.Email != "")))
            {
                EventPromoter.PromoterId = EventPromoter.AddToDb();
            }

            // LINE UP
            // only add the line up if it doesn't already exists and is not empty.
            if ((EventLineUp.LineUpId < 0) && ((EventLineUp.Headline != "") || (EventLineUp.Support != "") || (EventLineUp.Opener != "")))
            {
                EventLineUp.LineUpId = EventLineUp.AddToDb();
            }

            // EVENT
            EventsTableAdapter taEvent = new EventsTableAdapter();
            // only create the event if it isn't already in the database.
            if (EventId < 0)
            {
                EventId = Convert.ToInt32(taEvent.InsertNewEvent(EventDate.ToString(), DoorTime.ToString(), CurfewTime.ToString(), PromoterCharge, SecurityCost, SoundCost, LightCost));
            }

            // LINK TABLES
            // only add the link if the promoter exists in the database
            if (EventPromoter.PromoterId >= 0)
            {
                // and if the link doesn't already exist.
                if (EventPromoter.PromoterId != Convert.ToInt32(taEvent.GetPromoterIdByEventId(EventId)))
                {
                    Event_PromoterTableAdapter taEventPromoter = new Event_PromoterTableAdapter();
                    taEventPromoter.InsertLink(EventId, EventPromoter.PromoterId);
                }
            }
            // only add the link if the line up exists in the database
            if (EventLineUp.LineUpId >= 0)
            {
                // and if the link doesn't already exist
                if (EventLineUp.LineUpId != Convert.ToInt32(taEvent.GetLineUpIdByEventId(EventId)))
                {
                    Event_LineUpTableAdapter taEventLineup = new Event_LineUpTableAdapter();
                    taEventLineup.InsertLink(EventId, EventLineUp.LineUpId);
                }
            }
        }

        /// <summary>
        /// compare the start dates of each event. needed to sort the events in chronological order.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Event other)
        {
            // check if the other event exists.
            if (other == null)
                return 1;
            // compare the event dates
            int result = this.EventDate.CompareTo(other.EventDate);
            if (result == 0) // if the event dates are the same, compare the start times.
                return this.DoorTime.CompareTo(other.DoorTime);
            else
                return result;
        }

        /// <summary>
        ///  updates the corresponding event in the database.
        /// </summary>
        public static bool UpdateEvent(int eventId, string eventDate, string doorTime, string curfewTime, double promoterCharge, double securityCost, double soundCost, double lightCost)
        {
            EventsTableAdapter taEvents = new EventsTableAdapter();
            int newId = 0;

            if ((eventDate != null || doorTime != null || curfewTime != null) && eventId > 0)
            {
                DateTime eventDateConverted;
                if (DateTime.TryParse(eventDate, CultureInfo.CreateSpecificCulture("fr-FR"), DateTimeStyles.None, out eventDateConverted))
                {
                    newId = Convert.ToInt32(taEvents.UpdateEvent(eventDateConverted, doorTime, curfewTime, promoterCharge, securityCost, soundCost, lightCost, eventId));
                    if (eventId == newId)
                        return true;
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;
        }

        /// <summary>
        /// Deletes an event from the database as well as it's corresponding lineup and unlinks the event from the promoter.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="eventDate"></param>
        /// <param name="doorTime"></param>
        /// <param name="curfewTime"></param>
        /// <param name="promoterCharge"></param>
        /// <param name="securityCost"></param>
        /// <param name="soundCost"></param>
        /// <param name="lightCost"></param>
        /// <returns></returns>
        public static bool DeleteEvent(int eventId)
        {
            // get the line up id.
            EventsTableAdapter taEvents = new EventsTableAdapter();
            int lineUpId = Convert.ToInt32(taEvents.GetLineUpIdByEventId(eventId));
            // delete the links between the event and the promoter and line up.
            Event_LineUpTableAdapter taE_LU = new Event_LineUpTableAdapter();
            taE_LU.DeleteLink(eventId);
            Event_PromoterTableAdapter taE_P = new Event_PromoterTableAdapter();
            taE_P.DeleteLink(eventId);
            // delete the line up.
            LineUp.DeleteLineUp(lineUpId);
            // finally delete the event.
            taEvents.DeleteEvent(eventId);
            return true;
        }

        /// <summary>
        /// adds an event to the database and adds entries to the link tables.
        /// </summary>
        /// <param name="eventDate"></param>
        /// <param name="doorTime"></param>
        /// <param name="curfewTime"></param>
        /// <param name="promoterCharge"></param>
        /// <param name="securityCost"></param>
        /// <param name="soundCost"></param>
        /// <param name="lightCost"></param>
        /// <param name="lineupId"></param>
        /// <param name="promoterId"></param>
        /// <returns></returns>
        public static int AddToDbAndLink(string eventDate, string doorTime, string curfewTime, double promoterCharge, double securityCost, double soundCost, double lightCost, int lineupId, int promoterId)
        {
            Event newEvent = new Event(Convert.ToDateTime(eventDate), TimeSpan.Parse(doorTime), TimeSpan.Parse(curfewTime), promoterCharge, securityCost, soundCost, lightCost);

            if (eventDate != "") // an event needs to have a date
            {
                newEvent.AddToDb();
            }
            return newEvent.EventId;
        }
    }
}
