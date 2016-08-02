using EventManager.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    /// <summary>
    /// When the user selects "edit line up", set releveant session variables and redirect.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bLineUp_Click(object sender, EventArgs e)
    {
        LinkButton b = sender as LinkButton;
        if (b != null)
        {
            // get controls
            DataPager dpEvents = (DataPager)this.FindControlRecursive("dpEvents");
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + dpEvents.Parent.ToString() + "');", true);
            TextBox tbEventId = (TextBox)b.Parent.FindControl("tbEventId");
            // if controls have been found,
            if ((tbEventId != null)&&(dpEvents != null))
            {
                // set session variables and redirect.
                Session["Return_Index"] = dpEvents.StartRowIndex.ToString();
                Session["EventId_LineUp"] = tbEventId.Text;
                Response.Redirect("EditLineup.aspx");
            }
        }
    }

    /// <summary>
    /// When the user clicks "edit promoter", set the relevant session variables and then redirect the user.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bPromoter_Click(object sender, EventArgs e)
    {
        LinkButton b = sender as LinkButton;
        if (b != null)
        {
            // get controls.
            DataPager dpEvents = (DataPager)this.FindControlRecursive("dpEvents");
            TextBox tbEventId = (TextBox)b.Parent.FindControl("tbEventId");
            // if controls have been found,
            if ((tbEventId != null)&&(dpEvents != null))
            {
                // set session variables and redirect.
                Session["Return_Index"] = dpEvents.StartRowIndex.ToString();
                Session["EventId_Promoter"] = tbEventId.Text;
                Response.Redirect("EditPromoter.aspx");
            }
        }
    }

    /// <summary>
    /// when the page gets first loaded, set the pager to start displaying form yesterday, or
    /// when the page gets loaded on returning from lineup/promoter edit, set the pager to the page it was before leaving.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void dpEvents_Load(object sender, EventArgs e)
    {
        // only process if it isn't a postback.
        if (!IsPostBack)
        {
            // get the DataPager.
            DataPager dpEvents = sender as DataPager;
            // if successful
            if (dpEvents != null)
            {
                // get the page number from Session variable.
                int returnToIndex = Convert.ToInt32(Session["Return_Index"]);
                // if the number is valid, set the pager to that page.
                if (returnToIndex > 0)
                    dpEvents.SetPageProperties(returnToIndex, dpEvents.MaximumRows, false);
                // otherwise set the pager to start from yesterday's events.
                else
                    dpEvents.SetPageProperties(Event.IndexForYesterday(Event.SearchEvents(Request.QueryString["Search"])), dpEvents.MaximumRows, false);
            }
            // once the pager is set, reset the session variable so the pager doesn't get set to a different page when not needed.
            Session["Return_Index"] = "0";
        }
    }

    /// <summary>
    /// creates the tooltip with the lineup details for display on the lineup fields
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lHeadline_PreRender(object sender, EventArgs e)
    {
        Label lHeadline = sender as Label;
        if (lHeadline != null)
        {
            // get the event id field control.
            Label lEventId = (Label)lHeadline.Parent.FindControl("lEventId");
            // if found
            if (lEventId != null)
            {
                // check there is a value -there should be one at the prerender stage-.
                if (!string.IsNullOrEmpty(lEventId.Text))
                {
                    // get the corresponding lineup and format the string to be tooltipped.
                    LineUp currentLineup = LineUp.GetLineUpByEventId(Convert.ToInt32(lEventId.Text));
                    string tooltip = "Headline:\t" + currentLineup.Headline + System.Environment.NewLine
                                    + "Support:\t\t" + currentLineup.Support + System.Environment.NewLine
                                    + "Opener:\t\t" + currentLineup.Opener;
                    // add the tooltip to the corresponding controls.
                    lHeadline.ToolTip = tooltip;
                    Label lHeadlineDesc = (Label)lHeadline.Parent.FindControl("lHeadlineDesc");
                    if (lHeadlineDesc != null)
                        lHeadlineDesc.ToolTip = tooltip;
                }
            }
        }
    }

    /// <summary>
    /// adds tolltips to promoter fields with all the promoter details.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void lPromoterName_PreRender(object sender, EventArgs e)
    {
        Label lPromoterName = sender as Label;
        if (lPromoterName != null)
        {
            // get the event id control.
            Label lEventId = (Label)lPromoterName.Parent.FindControl("lEventId");
            // if the control was found and there is a value -it should be there at the prerender stage-.
            if (!string.IsNullOrEmpty(lEventId.Text))
            {
                // get the corresponding promoter and format the string to be used as tooltip.
                Promoter currentPromoter = Promoter.GetPromoterByEventId(Convert.ToInt32(lEventId.Text));
                string tooltip = "Name:\t\t" + currentPromoter.ContactName + System.Environment.NewLine
                                + "Company:\t" + currentPromoter.CompanyName + System.Environment.NewLine
                                + "Phone:\t\t" + currentPromoter.Phone + System.Environment.NewLine
                                + "Email:\t\t" + currentPromoter.Email;
                // add the string as tooltip wherever relevant.
                lPromoterName.ToolTip = tooltip;
                Label lPromoterDesc = (Label)lPromoterName.Parent.FindControl("lPromoterDesc");
                if (lPromoterDesc != null)
                    lPromoterDesc.ToolTip = tooltip;
                Label lPromoterEmailDesc = (Label)lPromoterName.Parent.FindControl("lPromoterEmailDesc");
                if (lPromoterEmailDesc != null)
                    lPromoterEmailDesc.ToolTip = tooltip;
                HyperLink hlPromoterEmail = (HyperLink)lPromoterName.Parent.FindControl("hlPromoterEmail");
                if (hlPromoterEmail != null)
                    hlPromoterEmail.ToolTip = tooltip;
            }
        }
    }

    /// <summary>
    /// created the link to be added to the promoter's email.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void hlPromoterEmail_PreRender(object sender, EventArgs e)
    {
        HyperLink hlPromoterEmail = sender as HyperLink;
        // if the email exists
        if (!string.IsNullOrEmpty(hlPromoterEmail.Text))
        {
            // get the event date and the headline for use in the subject.
            Label lEventDate = (Label)hlPromoterEmail.Parent.FindControl("lEventDate");
            Label lHeadline = (Label)hlPromoterEmail.Parent.FindControl("lHeadline");
            // proceed if they were found.
            if ((lEventDate != null)&&(lHeadline != null))
            {
                // if the headline is empty, only use the date in the subject
                if (string.IsNullOrEmpty(lHeadline.Text))
                    hlPromoterEmail.NavigateUrl = "mailto:" + hlPromoterEmail.Text + "?Subject=" + lEventDate.Text;
                // otherwise, use date and headline.
                else
                    hlPromoterEmail.NavigateUrl = "mailto:" + hlPromoterEmail.Text + "?Subject=" + lEventDate.Text + "%20-%20" + lHeadline.Text.Replace(" ", "%20");
            }
        }
    }

    /// <summary>
    /// When the search button is clicked, redirects if the search box is not empty.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void bSearch_Click(object sender, EventArgs e)
    {
        LinkButton b = sender as LinkButton;
        if (b != null)
        {
            TextBox tbSearch = (TextBox)b.Parent.FindControl("tbSearch");
            if (tbSearch != null)
            {
                if (!string.IsNullOrEmpty(tbSearch.Text))
                {
                    string pageRedirect = "Default.aspx?Search=" + tbSearch.Text;
                    Response.Redirect(pageRedirect);
                }
                else
                    Response.Redirect("Default.aspx");
            }
        }
    }

    /// <summary>
    /// if a search is being done, fill the search box with the current search value.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void tbSearch_PreRender(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TextBox tbSearch = sender as TextBox;
            if (tbSearch != null)
            {
                string search = Request.QueryString["Search"];
                if (!string.IsNullOrEmpty(search))
                {
                    tbSearch.Text = search;
                }
            }
        }
    }
}

public static class ControlExtensions
{
    /// <summary>
    /// recursively finds a child control of the specified parent.
    /// </summary>
    /// <param name="control"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static Control FindControlRecursive(this Control control, string id)
    {
        if (control == null) return null;
        //try to find the control at the current level
        Control ctrl = control.FindControl(id);

        if (ctrl == null)
        {
            //search the children
            foreach (Control child in control.Controls)
            {
                ctrl = FindControlRecursive(child, id);

                if (ctrl != null) break;
            }
        }
        return ctrl;
    }
}