<%@ Control Language="VB" AutoEventWireup="false" CodeFile="Login2.ascx.vb" Inherits="Login2" %>
<asp:ChangePassword 
  ID="ChangePassword1" 
  runat="server" 
  BackColor="#ccffff"
  BorderColor="#0000CC" 
  BorderStyle="Solid" 
  BorderWidth="2px" 
  BorderPadding="25"
  TitleTextStyle-HorizontalAlign="Center"
  CancelDestinationPageUrl="~/dmsMain.aspx" 
  ContinueDestinationPageUrl="~/dmsMain.aspx" 
  MembershipProvider="AspNetSqlMembershipProvider" 
  SuccessPageUrl="~/dmsMain.aspx">
  <TitleTextStyle Font-Size="18px" />
  <ChangePasswordButtonStyle CssClass="btn btn-success" />
  <CancelButtonStyle CssClass="btn btn-danger" />
</asp:ChangePassword>
