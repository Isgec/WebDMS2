<%@ WebHandler Language="VB" Class="SIS.DMS.Services.dmsMain" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization
Namespace SIS.DMS.Services
  Public Class dmsMain : Implements IHttpHandler, System.Web.SessionState.IRequiresSessionState

    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
      context.Response.ContentType = "application/json"
      Dim json As New System.Web.Script.Serialization.JavaScriptSerializer()

      Dim Req As SIS.DMS.Handlers.dmsRequest = SIS.DMS.Handlers.dmsRequest.getDMSRequest(context)

      Dim tmp As SIS.DMS.Handlers.dmsResponse = SIS.DMS.Handlers.ProcessRequest.Execute(Req)

      Dim output As String = json.Serialize(tmp)
      context.Response.Write(output)

    End Sub

    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
      Get
        Return False
      End Get
    End Property

  End Class
End Namespace
