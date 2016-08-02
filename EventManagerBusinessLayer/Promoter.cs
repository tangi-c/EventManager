using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManager.Business.dsEventManagerTableAdapters;

namespace EventManager.Business
{
    public class Promoter : IComparable<Promoter>
    {
        public int PromoterId { get; set; }
        public string ContactName { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// default constructor.
        /// </summary>
        public Promoter()
        {
            this.PromoterId = -1;
            this.ContactName = "";
            this.CompanyName = "";
            this.Phone = "";
            this.Email = "";
        }

        /// <summary>
        /// constructor with all but Id.
        /// </summary>
        /// <param name="contactName"></param>
        /// <param name="companyName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        public Promoter(string contactName, string companyName, string phone, string email)
        {
            // check for null strings
            if (contactName == null)
                contactName = "";
            if (companyName == null)
                companyName = "";
            if (phone == null)
                phone = "";
            if (email == null)
                email = "";

            this.PromoterId = -1;
            this.ContactName = contactName;
            this.CompanyName = companyName;
            this.Phone = phone;
            this.Email = email;
        }

        /// <summary>
        /// Adds a new promoter to the database.
        /// </summary>
        /// <param name="contactName"></param>
        /// <param name="companyName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static int AddPromoterToDb(string contactName, string companyName, string phone, string email)
        {
            Promoter newPromoter = new Promoter(contactName, companyName, phone, email);
            newPromoter.AddToDb();
            return newPromoter.PromoterId;
        }

        /// <summary>
        /// Adds a new promoter to the database and adds an entry to the Event Promoter link table.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="headline"></param>
        /// <param name="support"></param>
        /// <param name="opener"></param>
        /// <returns></returns>
        public static bool AddToDbAndLink(int eventId, string contactName, string companyName, string phone, string email)
        {
            Event_PromoterTableAdapter taEventPromoter = new Event_PromoterTableAdapter();
            int promoterId = AddPromoterToDb(contactName, companyName, phone, email);
            // if the promoter was added to the database, create the link entry.
            if (promoterId >= 0)
            {
                taEventPromoter.InsertLink(eventId, promoterId);
                return true;
            }
            // if not return false to notify it did not work.
            else
                return false;
        }

        /// <summary>
        /// Adds this promoter to the database.
        /// </summary>
        /// <returns>promoter db Id</returns>
        public int AddToDb ()
        {
            // only try to add to the database if it isn't already there.
            if (PromoterId < 0)
            {
                // if the promoter is empty, it is not added to the database.
                if ((!string.IsNullOrWhiteSpace(ContactName)) || (!string.IsNullOrWhiteSpace(CompanyName)) || (!string.IsNullOrWhiteSpace(Phone)) || (!string.IsNullOrWhiteSpace(Email)))
                {
                    PromotersTableAdapter taPromoter = new PromotersTableAdapter();
                    PromoterId = Convert.ToInt32(taPromoter.InsertNewPromoter(ContactName, CompanyName, Phone, Email));
                }
            }
            // if the promoter was not added, PromoterId == -1.
            return PromoterId;
        }

        /// <summary>
        /// returns a list of all the promoters in the database.
        /// </summary>
        /// <returns></returns>
        public static List<Promoter> GetAllPromoters()
        {
            PromotersTableAdapter taPromoters = new PromotersTableAdapter();
            var dtPromoters = taPromoters.GetData();
            List<Promoter> allPromoters = new List<Promoter>();
            foreach (dsEventManager.PromotersRow promotersRow in dtPromoters.Rows)
            {
                Promoter currentPromoter = new Promoter();
                currentPromoter.PromoterId = promotersRow.Id;
                currentPromoter.ContactName = promotersRow.ContactName;
                currentPromoter.CompanyName = promotersRow.CompanyName;
                currentPromoter.Phone = promotersRow.Phone;
                currentPromoter.Email = promotersRow.Email;
                allPromoters.Add(currentPromoter);
            }
            dtPromoters.Clear();
            return allPromoters;
        }

        /// <summary>
        /// Gets all promoters from the database, sorts them in contact name alphabetical order and returns that list.
        /// </summary>
        /// <returns></returns>
        public static List<Promoter> GetAllPromotersSorted()
        {
            //TO DO: problem with the sorting to be fixed.
            List<Promoter> allPromoters = GetAllPromoters();
            allPromoters.Sort();
            return allPromoters;
        }

        /// <summary>
        /// From the database, gets the promoter with this Id.
        /// </summary>
        /// <param name="promoterId"></param>
        /// <returns></returns>
        public static Promoter GetPromoterById(int promoterId)
        {
            PromotersTableAdapter taPromoters = new PromotersTableAdapter();
            var dtPromoters = taPromoters.GetPromoterById(promoterId);
            Promoter returnPromoter = new Promoter();

            // There is only one promoter per Id, so there can only be one row returned.
            foreach (dsEventManager.PromotersRow promotersRow in dtPromoters.Rows)
            {
                returnPromoter.PromoterId = promotersRow.Id;
                returnPromoter.ContactName = promotersRow.ContactName;
                returnPromoter.CompanyName = promotersRow.CompanyName;
                returnPromoter.Phone = promotersRow.Phone;
                returnPromoter.Email = promotersRow.Email;
            }
            dtPromoters.Clear();
            return returnPromoter;
        }

        /// <summary>
        /// returns Promoter corresponding to event which Id is passed as parameter.
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public static Promoter GetPromoterByEventId(int eventId)
        {
            EventsTableAdapter taEvent = new EventsTableAdapter();
            int promoterId = Convert.ToInt32(taEvent.GetPromoterIdByEventId(eventId));

            return GetPromoterById(promoterId);
        }

        /// <summary>
        /// updates the corresponding promoter in the database.
        /// </summary>
        /// <param name="promoterId"></param>
        /// <param name="contactName"></param>
        /// <param name="companyName"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool UpdatePromoter(int promoterId, string contactName, string companyName, string phone, string email)
        {
            PromotersTableAdapter taPromoter = new PromotersTableAdapter();

            // TO DO: correct stupid id check.
            if (promoterId >= 0)
            {
                taPromoter.UpdatePromoter(contactName, companyName, phone, email, promoterId);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// compares the ContactName strings (alphabetical order) and if two are the same,
        /// then compares the CompanyName strings to make sure they always appear in the same order.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Promoter other)
        {
            // check if the other event exists.
            if (other == null)
                return 1;
            // compare the event dates
            int result = this.ContactName.CompareTo(other.ContactName);
            if (result == 0) // if the event dates are the same, compare the start times.
                return this.CompanyName.CompareTo(other.CompanyName);
            else
                return result;
        }

        /// <summary>
        /// Search the database for promoters corresponding to the query.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<Promoter> SearchPromoters (string query)
        {
            // create empty list.
            List<Promoter> promotersList = new List<Promoter>();
            // check if the query string is usable.
            if (!string.IsNullOrWhiteSpace(query))
            {
                // add % at the start and end of the string for the SQL query.
                query = "%" + query + "%";
                // search the database.
                PromotersTableAdapter taPromoters = new PromotersTableAdapter();
                var dtPromoters = taPromoters.SearchPromoters(query);
                // parse results and add to list.
                foreach (dsEventManager.PromotersRow row in dtPromoters)
                {
                    Promoter currentPromoter = new Promoter();
                    currentPromoter.PromoterId = row.Id;
                    currentPromoter.ContactName = row.ContactName;
                    currentPromoter.CompanyName = row.CompanyName;
                    currentPromoter.Phone = row.Phone;
                    currentPromoter.Email = row.Email;
                    promotersList.Add(currentPromoter);
                }
            }
            // return complete list.
            return promotersList;
        }
    }
}
