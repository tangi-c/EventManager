using EventManager.Business;
using EventManager.Business.dsEventManagerTableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class EditPromoter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // if the page wasn't called from the main page's edit buttons, redirect to the main page.
        if (Convert.ToInt32(Session["EventId_Promoter"]) <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Page non accessible. You will be redirected." + "');", true);
            Response.Redirect("Default.aspx");
        }
        // when the page is loaded for the first time set ddl  to -20 (impossible to get otherwise).
        if (!IsPostBack)
            Session["ddl"] = "-20";
    }

    /// <summary>
    /// When update is clicked, update the promoter and event as required then return to the main page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bUpdate_Click(object sender, EventArgs e)
    {
        LinkButton b = sender as LinkButton;
        if (b != null)
        {
            DropDownList ddlPromoters = (DropDownList)b.Parent.FindControl("ddlPromoters");
            TextBox tbContactName = (TextBox)b.Parent.FindControl("tbContactName");
            TextBox tbCompanyName = (TextBox)b.Parent.FindControl("tbCompanyName");
            TextBox tbPhone = (TextBox)b.Parent.FindControl("tbPhone");
            TextBox tbEmail = (TextBox)b.Parent.FindControl("tbEmail");
            int selectedId = Convert.ToInt32(ddlPromoters.SelectedItem.Value);

            // if selectedId is -2 it's a new promoter.
            if (selectedId == -2)
            {
                // create a promoter and add it to the database.
                Promoter newPromoter = new Promoter(tbContactName.Text, tbCompanyName.Text, tbPhone.Text, tbEmail.Text);
                newPromoter.AddToDb();
                // if the promoter was added to the database, link the event and promoter.
                if (newPromoter.PromoterId >= 0)
                {
                    Event_PromoterTableAdapter taEventPromoter = new Event_PromoterTableAdapter();
                    taEventPromoter.UpdateLink(newPromoter.PromoterId, Convert.ToInt32(Session["EventId_Promoter"].ToString()));
                }
                // reset session variable and redirect.
                Session["EventId_Promoter"] = "0";
                Response.Redirect("Default.aspx");
            }
            // -1 means updating the current promoter.
            else if (selectedId == -1)
            {
                // TO DO: check what happens when there is no current promoter.
                EventsTableAdapter taEvent = new EventsTableAdapter();
                int promoterId = Convert.ToInt32(taEvent.GetPromoterIdByEventId(Convert.ToInt32(Session["EventId_Promoter"].ToString())));
                // update the promoter.
                if (promoterId > 0)
                    Promoter.UpdatePromoter(promoterId, tbContactName.Text, tbCompanyName.Text, tbPhone.Text, tbEmail.Text);
                // if not the promoter didn't exist, we need to create a new promoter.
                else
                {
                    Promoter newPromoter = new Promoter(tbContactName.Text, tbCompanyName.Text, tbPhone.Text, tbEmail.Text);
                    newPromoter.AddToDb();
                    // if the promoter was added to the database, create a link.
                    if (newPromoter.PromoterId >= 0)
                    {
                        Event_PromoterTableAdapter taEventPromoter = new Event_PromoterTableAdapter();
                        taEventPromoter.InsertLink(Convert.ToInt32(Session["EventId_Promoter"].ToString()), newPromoter.PromoterId);
                    }
                }
                // reset session variable and redirect.
                Session["EventId_Promoter"] = "0";
                Response.Redirect("Default.aspx");
            }
            // a different existing promoter has been selected.
            else if (selectedId >= 0)
            {
                // get the corresponding promoter from the database.
                Promoter currentPromoter = Promoter.GetPromoterById(selectedId);
                // if any of the fields have changed, update the promoter in the database.
                if (!tbContactName.Text.Equals(currentPromoter.ContactName)
                    || !tbCompanyName.Text.Equals(currentPromoter.CompanyName)
                    || !tbPhone.Text.Equals(currentPromoter.Phone)
                    || !tbEmail.Text.Equals(currentPromoter.Email))
                    Promoter.UpdatePromoter(selectedId, tbContactName.Text, tbCompanyName.Text, tbPhone.Text, tbEmail.Text);
                // once the promoter has been updated if needed, the event needs to be linked to the promoter.
                Event_PromoterTableAdapter taEventPromoter = new Event_PromoterTableAdapter();
                int check = Convert.ToInt32(taEventPromoter.UpdateLink(selectedId, Convert.ToInt32(Session["EventId_Promoter"].ToString())));
                if (check <= 0)
                    taEventPromoter.InsertLink(Convert.ToInt32(Session["EventId_Promoter"].ToString()), selectedId);
                // reset session variable and redirect.
                Session["EventId_Promoter"] = "0";
                Response.Redirect("Default.aspx");
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "The promoter could not be updated." + "');", true);
            }
        }
    }

    /// <summary>
    /// If cancel is clicked return to the main page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bCancel_Click(object sender, EventArgs e)
    {
        // reset session variable and redirect.
        Session["EventId_Promoter"] = "0";
        Response.Redirect("Default.aspx");
    }

    /// <summary>
    /// updates the controls when the dropdownlist selection is changed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPromoters_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList d = sender as DropDownList;
        if (d != null)
        {
            int selectedId = Convert.ToInt32(d.SelectedItem.Value);

            // This function is called on every postback (why?),
            // so to know if it called when the selection was actually changed,
            // check if the value from the dropdownlist is different from the session value.
            if (selectedId != Convert.ToInt32(Session["ddl"].ToString()))
            {
                //if it was different, put the new value in the session variable.
                Session["ddl"] = selectedId.ToString();

                // find the controls.
                TextBox tbContactName = (TextBox)d.Parent.FindControl("tbContactName");
                TextBox tbCompanyName = (TextBox)d.Parent.FindControl("tbCompanyName");
                TextBox tbPhone = (TextBox)d.Parent.FindControl("tbPhone");
                TextBox tbEmail = (TextBox)d.Parent.FindControl("tbEmail");

                // if all controls where found
                if ((tbContactName != null) && (tbCompanyName != null) && (tbPhone != null) && (tbEmail != null))
                {

                    // if selectedId is -2 (new promoter), then we need to clear the control texts.
                    if (selectedId == -2)
                    {
                        tbContactName.Text = "";
                        tbCompanyName.Text = "";
                        tbPhone.Text = "";
                        tbEmail.Text = "";
                    }
                    // if selectedId is -1 (edit current promoter), we need to get the current promoter and populate the controls.
                    else if (selectedId == -1)
                    {
                        // get the current promoter using the event Id stored in a session variable.
                        Promoter currentPromoter = Promoter.GetPromoterByEventId(Convert.ToInt32(Session["EventId_Promoter"].ToString()));
                        // populate the text boxes
                        tbContactName.Text = currentPromoter.ContactName;
                        tbCompanyName.Text = currentPromoter.CompanyName;
                        tbPhone.Text = currentPromoter.Phone;
                        tbEmail.Text = currentPromoter.Email;
                    }
                    // a different existing promoter has been selected.
                    else if (selectedId >= 0)
                    {
                        // get the selected promoter using its Id.
                        Promoter currentPromoter = Promoter.GetPromoterById(selectedId);
                        // populate the text boxes
                        tbContactName.Text = currentPromoter.ContactName;
                        tbCompanyName.Text = currentPromoter.CompanyName;
                        tbPhone.Text = currentPromoter.Phone;
                        tbEmail.Text = currentPromoter.Email;
                    }
                    // if selectedId is less than -2, someone must have messed up with the code!
                    else
                        ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "The promoter could not be found." + "');", true);
                }
                // if the dropdownlist isn't found, alert the user.
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "There was a problem getting your selection." + "');", true);
            }
        }
    }

    /// <summary>
    /// When the dropdownlist is created, set the controls with the current promoter.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPromoters_Init(object sender, EventArgs e)
    {
        DropDownList d = sender as DropDownList;
        if (d != null)
        {
            if (!d.Parent.Page.IsPostBack)
            {
                // find the controls.
                TextBox tbContactName = (TextBox)d.Parent.FindControl("tbContactName");
                TextBox tbCompanyName = (TextBox)d.Parent.FindControl("tbCompanyName");
                TextBox tbPhone = (TextBox)d.Parent.FindControl("tbPhone");
                TextBox tbEmail = (TextBox)d.Parent.FindControl("tbEmail");

                // if the controls are found
                if ((tbContactName != null) && (tbCompanyName != null) && (tbPhone != null) && (tbEmail != null))
                {
                    Promoter currentPromoter = Promoter.GetPromoterByEventId(Convert.ToInt32(Session["EventId_Promoter"].ToString()));
                    // populate the text boxes
                    tbContactName.Text = currentPromoter.ContactName;
                    tbCompanyName.Text = currentPromoter.CompanyName;
                    tbPhone.Text = currentPromoter.Phone;
                    tbEmail.Text = currentPromoter.Email;
                }
            }
        }
    }
}