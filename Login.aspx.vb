
Imports System.Web.Script.Serialization
Partial Class Login
  Inherits System.Web.UI.Page
  Public Sub LoggedIn(ByVal sender As Object, ByVal e As System.EventArgs)
    Dim oLogin As System.Web.UI.WebControls.Login = CType(sender, System.Web.UI.WebControls.Login)
    Dim UserID As String = CType(oLogin.FindControl("UserName"), TextBox).Text
    SIS.SYS.Utilities.SessionManager.InitializeEnvironment(UserID)
    Response.Redirect("~/dmsMain.aspx")
  End Sub
End Class
