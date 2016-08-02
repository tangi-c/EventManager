using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EventManager.Business;

public partial class EditLineUp : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // if the session variable hasn't been set, it means this page wasn't called from the right place.
        // in this case, redirect to the main page.
        if (Convert.ToInt32(Session["EventId_LineUp"]) <= 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Page non accessible. You will be redirected." + "');", true);
            Response.Redirect("Default.aspx");
        }
    }

    protected void bCancel_Click(object sender, EventArgs e)
    {
        Session["EventId_LineUp"] = 0;
        Response.Redirect("Default.aspx");
    }

    protected void bUpdate_Click(object sender, EventArgs e)
    {
        LinkButton b = sender as LinkButton;
        if (b != null)
        {
            TextBox tbLineUpId = (TextBox)b.Parent.FindControl("tbLineUpId");
            if (tbLineUpId != null)
            {
                int lineupId = Convert.ToInt32(tbLineUpId.Text);
                TextBox tbHeadline = (TextBox)b.Parent.FindControl("tbHeadline");
                TextBox tbSupport = (TextBox)b.Parent.FindControl("tbSupport");
                TextBox tbOpener = (TextBox)b.Parent.FindControl("tbOpener");

                if ((tbHeadline != null) && (tbSupport != null) && (tbOpener != null)) //all controls have been found
                {
                    if (lineupId >= 0) //line up already exists and needs updating
                    {
                        if (LineUp.UpdateLineUp(lineupId, tbHeadline.Text, tbSupport.Text, tbOpener.Text))
                        {
                            // reset session variable then redirect.
                            Session["EventId_LineUp"] = 0;
                            Response.Redirect("Default.aspx");
                        }
                        else
                            ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Line up did not update" + "');", true);
                    }
                    else //line up doesn't exist so it needs creating
                    {
                        //check first that all fields are not empty. If they are, it's not worth creating a new line up.
                        if ((tbHeadline.Text != "")||(tbSupport.Text != "")||(tbOpener.Text!=""))
                        {
                            int eventId = Convert.ToInt32(Session["EventId_LineUp"]);
                            LineUp.AddToDbAndLink(eventId, tbHeadline.Text, tbSupport.Text, tbOpener.Text);
                        }
                        // reset the session variable and return to the main page.
                        Session["EventId_LineUp"] = 0;
                        Response.Redirect("Default.aspx");
                    }
                }
                else
                    ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('" + "Line up did not update" + "');", true);
            }
        }
    }
}