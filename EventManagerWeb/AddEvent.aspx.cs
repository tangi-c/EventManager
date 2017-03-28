using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EventManager.Business;
using System.Globalization;

public partial class AddEvent : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// If canceled is clicked redirect to the main page.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }

    /// <summary>
    /// function called when the create button is clicked.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bCreate_Click(object sender, EventArgs e)
    {
        LinkButton b = sender as LinkButton;
        if (b != null)
        {
            LineUp newLineUp;
            Promoter newPromoter;

            // LINE UP
            TextBox tbHeadline = (TextBox)b.Parent.FindControl("tbHeadline");
            TextBox tbSupport = (TextBox)b.Parent.FindControl("tbSupport");
            TextBox tbOpener = (TextBox)b.Parent.FindControl("tbOpener");

            // if all controls have been found, create line up.
            if ((tbHeadline != null) && (tbSupport != null) && (tbOpener != null))
                newLineUp = new LineUp(tbHeadline.Text, tbSupport.Text, tbOpener.Text);
            // otherwise alert + create empty line up.
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "There was a problem with the line up." + "');", true);
                newLineUp = new LineUp();
            }

            // PROMOTER
            DropDownList ddlPromoters = (DropDownList)b.Parent.FindControl("ddlPromoters");
            if (ddlPromoters != null)
            {
                int selectedId = Convert.ToInt32(ddlPromoters.SelectedItem.Value);
                // get controls.
                TextBox tbContactName = (TextBox)b.Parent.FindControl("tbContactName");
                TextBox tbCompanyName = (TextBox)b.Parent.FindControl("tbCompanyName");
                TextBox tbPhone = (TextBox)b.Parent.FindControl("tbPhone");
                TextBox tbEmail = (TextBox)b.Parent.FindControl("tbEmail");

                // check if all controls have been found.
                if ((tbContactName != null) && (tbCompanyName != null) && (tbPhone != null) && (tbEmail != null))
                {
                    // check if an existing promoter was selected.
                    if (selectedId >= 0)
                    {
                        // get the corresponding promoter
                        newPromoter = Promoter.GetPromoterById(selectedId);
                        // if any of the fields have changed, update the database and update newPromoter.
                        if (!tbContactName.Text.Equals(newPromoter.ContactName)
                            || !tbCompanyName.Text.Equals(newPromoter.CompanyName)
                            || !tbPhone.Text.Equals(newPromoter.Phone)
                            || !tbEmail.Text.Equals(newPromoter.Email))
                        {
                            Promoter.UpdatePromoter(selectedId, tbContactName.Text, tbCompanyName.Text, tbPhone.Text, tbEmail.Text);
                            newPromoter = Promoter.GetPromoterById(selectedId);
                        }
                    }
                    // if not it is a new promoter.
                    else
                        newPromoter = new Promoter(tbContactName.Text, tbCompanyName.Text, tbPhone.Text, tbEmail.Text);
                }
                // promoter could not be created because the controls were not found, alert + create empty promoter.
                else
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "There was a problem creating the promoter." + "');", true);
                    newPromoter = new Promoter();
                }
            }
            // promoter could not be created because the dropdown list was not found, alert + create empty promoter.
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "There was a problem getting the promoter information." + "');", true);
                newPromoter = new Promoter();
            }

            // EVENT
            TextBox dtpBoxEventDate = (TextBox)b.Parent.FindControl("dtpBoxEventDate");
            TextBox dtpBoxDoorTime = (TextBox)b.Parent.FindControl("dtpBoxDoorTime");
            TextBox dtpBoxCurfewTime = (TextBox)b.Parent.FindControl("dtpBoxCurfewTime");
            TextBox tbPromoterCharge = (TextBox)b.Parent.FindControl("tbPromoterCharge");
            TextBox tbSecurityCost = (TextBox)b.Parent.FindControl("tbSecurityCost");
            TextBox tbSoundCost = (TextBox)b.Parent.FindControl("tbSoundCost");
            TextBox tbLightCost = (TextBox)b.Parent.FindControl("tbLightCost");

            if ((dtpBoxEventDate != null) && (dtpBoxDoorTime != null) && (dtpBoxCurfewTime != null) && (tbPromoterCharge != null) && (tbSecurityCost != null) && (tbSoundCost != null) && (tbLightCost != null)) //all controls have been found
            {
                // an event needs a date.
                DateTime eventDate;
                if (DateTime.TryParse(dtpBoxEventDate.Text, CultureInfo.CreateSpecificCulture("fr-FR"), DateTimeStyles.None, out eventDate))
                {
                    // try to parse all the other values and if it doesnt work, use default values.
                    TimeSpan doorTime, curfewTime;
                    int promoterCharge, securityCost, soundCost, lightCost;
                    if (!TimeSpan.TryParse(dtpBoxDoorTime.Text, out doorTime))
                        doorTime = TimeSpan.Parse("19:30");
                    if (!TimeSpan.TryParse(dtpBoxCurfewTime.Text, out curfewTime))
                        curfewTime = TimeSpan.Parse("23:00");
                    if (!int.TryParse(tbPromoterCharge.Text, out promoterCharge))
                        promoterCharge = 0;
                    if (!int.TryParse(tbSecurityCost.Text, out securityCost))
                        securityCost = 0;
                    if (!int.TryParse(tbSoundCost.Text, out soundCost))
                        soundCost = 0;
                    if (!int.TryParse(tbLightCost.Text, out lightCost))
                        lightCost = 0;
                    // create the new event and add to DB.
                    Event newEvent = new Event(eventDate, doorTime, curfewTime, promoterCharge, securityCost, soundCost, lightCost, newPromoter, newLineUp);
                    newEvent.AddToDb();
                    // redirect once the event was created.
                    Response.Redirect("Default.aspx");
                }
                // if the date was invalid, warn the user.
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "You need to enter a valid date." + "');", true);
            }
            else // promoter could not be created because the controls were not found.
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "There was a problem getting the event information." + "');", true);
        }
    }

    /// <summary>
    /// when the drop down list selection changes, update the textboxes accordingly.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void ddlPromoters_SelectedIndexChanged(object sender, EventArgs e)
    {
        DropDownList d = sender as DropDownList;

        if (d != null)
        {
            // find all controls
            TextBox tbContactName = (TextBox)d.Parent.FindControl("tbContactName");
            TextBox tbCompanyName = (TextBox)d.Parent.FindControl("tbCompanyName");
            TextBox tbPhone = (TextBox)d.Parent.FindControl("tbPhone");
            TextBox tbEmail = (TextBox)d.Parent.FindControl("tbEmail");

            // if all controls are found
            if ((tbContactName != null) && (tbCompanyName != null) && (tbPhone != null) && (tbEmail != null))
            {
                int selectedId = Convert.ToInt32(d.SelectedItem.Value);
                // if the selected ID is > 0 then the promoter is already in database so we need to retrive it.
                if (selectedId >= 0)
                {
                    Promoter selectedPromoters = Promoter.GetPromoterById(selectedId);
                    // populate the controls with the right information.
                    tbContactName.Text = selectedPromoters.ContactName;
                    tbCompanyName.Text = selectedPromoters.CompanyName;
                    tbPhone.Text = selectedPromoters.Phone;
                    tbEmail.Text = selectedPromoters.Email;
                }
                // if the selected ID is negative, then "new promoter" was selected so we need to clear the controls.
                else
                {
                    tbContactName.Text = "";
                    tbCompanyName.Text = "";
                    tbPhone.Text = "";
                    tbEmail.Text = "";
                }
            }
            // if the controls are not found, alert the user.
            else
                ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "There was a problem showing the promoter's information." + "');", true);
        }
    }
}