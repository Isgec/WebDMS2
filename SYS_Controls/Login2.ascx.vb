Partial Class Login2
  Inherits System.Web.UI.UserControl
  Private Sub ChangePassword1_ChangingPassword(sender As Object, e As LoginCancelEventArgs) Handles ChangePassword1.ChangingPassword
    Dim x As ChangePassword = CType(sender, ChangePassword)
    Dim UserID As String = HttpContext.Current.Session("LoginID")
    Dim wUser As SIS.QCM.qcmUsers = SIS.QCM.qcmUsers.qcmUsersGetByID(UserID)
    wUser.PW = x.ConfirmNewPassword
    SIS.QCM.qcmUsers.UpdateData(wUser)
  End Sub
End Class
