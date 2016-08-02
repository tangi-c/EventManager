using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManager.Business.dsEventManagerTableAdapters;

namespace EventManager.Business
{
    public class LineUp
    {
        public int LineUpId { get; set; }
        public string Headline { get; set; }
        public string Support { get; set; }
        public string Opener { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public LineUp ()
        {
            this.LineUpId = -1;
            this.Headline = "";
            this.Support = "";
            this.Opener = "";

        }

        /// <summary>
        /// constructor with all properties but Id.
        /// </summary>
        /// <param name="headline"></param>
        /// <param name="support"></param>
        /// <param name="opener"></param>
        public LineUp (string headline, string support, string opener)
        {
            // check for null strings
            if (headline == null)
                headline = "";
            if (support == null)
                support = "";
            if (opener == null)
                opener = "";

            this.LineUpId = -1;
            this.Headline = headline;
            this.Support = support;
            this.Opener = opener;
        }

        /// <summary>
        /// Creates a new line up in the database.
        /// </summary>
        /// <param name="headline"></param>
        /// <param name="support"></param>
        /// <param name="opener"></param>
        /// <returns>Line up Id (-1 means empty line up was not added)</returns>
        public static int AddLineUpToDb (string headline, string support, string opener)
        {
            LineUp newLineUp = new LineUp(headline, support, opener);
            newLineUp.AddToDb();
            return newLineUp.LineUpId;
        }

        /// <summary>
        /// Adds this line up to the database if it's not empty.
        /// </summary>
        /// <returns>lineUp db Id</returns>
        public int AddToDb()
        {
            // if LineUpId is greater than 0, then it's already in the database.
            if (LineUpId < 0)
            {
                // check if the line up is not empty.
                if ((!string.IsNullOrWhiteSpace(Headline)) || (!string.IsNullOrWhiteSpace(Support)) || (!string.IsNullOrWhiteSpace(Opener)))
                {
                    LineUpTableAdapter taLineUp = new LineUpTableAdapter();
                    LineUpId = Convert.ToInt32(taLineUp.InsertNewLineUp(Headline, Support, Opener));
                }
            }
            // if the line up was not added to the database, then -1 is retutrned.
            return LineUpId;
        }

        /// <summary>
        /// Creates a new line up in the database and creates an entry in the Event_LineUp linking table.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="headline"></param>
        /// <param name="support"></param>
        /// <param name="opener"></param>
        /// <returns></returns>
        public static bool AddToDbAndLink(int eventId, string headline, string support, string opener)
        {
            Event_LineUpTableAdapter taEventLineup = new Event_LineUpTableAdapter();
            int lineupId = AddLineUpToDb(headline, support, opener);
            // if the line up was added to the database, create the link entry.
            if (lineupId > 0)
            {
                taEventLineup.InsertLink(eventId, lineupId);
                return true;
            }
            // if not return false to notify it did not work.
            else
                return false;
        }

        /// <summary>
        /// From the database, gets the line up corresponding to this Id.
        /// </summary>
        /// <param name="lineUpId"></param>
        /// <returns></returns>
        public static LineUp GetLineUpById(int lineUpId)
        {
            LineUpTableAdapter taLineUps = new LineUpTableAdapter();
            var dtLineUps = taLineUps.GetLineUpById(lineUpId);
            LineUp returnLineUp = new LineUp();

            // There is only one line up per Id, so there can only be one row returned.
            foreach (dsEventManager.LineUpRow lineUpsRow in dtLineUps.Rows)
            {
                returnLineUp.LineUpId = lineUpsRow.Id;
                returnLineUp.Headline = lineUpsRow.Headline;
                returnLineUp.Support = lineUpsRow.Support;
                returnLineUp.Opener = lineUpsRow.Opener;
            }
            dtLineUps.Clear();
            return returnLineUp;
        }

        /// <summary>
        /// returns LineUp corresponding to event which Id is passed as parameter.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public static LineUp GetLineUpByEventId (int eventId)
        {
            EventsTableAdapter taEvent = new EventsTableAdapter();
            int lineUpId = Convert.ToInt32(taEvent.GetLineUpIdByEventId(eventId));

            return GetLineUpById(lineUpId);
        }

        /// <summary>
        /// Updates the corresponding line up in the database.
        /// </summary>
        /// <param name="lineupId"></param>
        /// <param name="headline"></param>
        /// <param name="support"></param>
        /// <param name="opener"></param>
        /// <returns></returns>
        public static bool UpdateLineUp(int lineupId, string headline, string support, string opener)
        {
            LineUpTableAdapter taLineup = new LineUpTableAdapter();

            // TO DO: correct stupid verification scheme...
            if (lineupId >= 0)
            {
                taLineup.UpdateLineUp(headline, support, opener, lineupId);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Search the database for line ups containing the string query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<LineUp> SearchLineUps(string query)
        {
            // create an empty list.
            List<LineUp> returnLinups = new List<LineUp>();
            // check if query is usable.
            if (!string.IsNullOrWhiteSpace(query))
            {
                // add percentage to the start and end of the string for the SQL query.
                query = "%" + query + "%";
                // search the database.
                LineUpTableAdapter taLineUp = new LineUpTableAdapter();
                var dtLineUps = taLineUp.SearchLineUps(query);
                // parse result and add to the list.
                foreach (dsEventManager.LineUpRow row in dtLineUps)
                {
                    LineUp currentLineUp = new LineUp();
                    currentLineUp.LineUpId = row.Id;
                    currentLineUp.Headline = row.Headline;
                    currentLineUp.Support = row.Support;
                    currentLineUp.Opener = row.Opener;
                    returnLinups.Add(currentLineUp);
                }
            }
            // return list.
            return returnLinups;
        }

        /// <summary>
        /// Deletes a line up from database. NOTE: the link with the event needs to be removed before calling this function.
        /// </summary>
        /// <param name="lineUpId"></param>
        /// <returns></returns>
        public static bool DeleteLineUp (int lineUpId)
        {
            // check the lineup can exist.
            if (lineUpId >= 0)
            {
                LineUpTableAdapter taLineUp = new LineUpTableAdapter();
                int result = taLineUp.DeleteLineUp(lineUpId);
                // check the line up was deleted.
                if (result > 0)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }
    }
}
