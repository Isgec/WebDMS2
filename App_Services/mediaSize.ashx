<%@ WebHandler Language="VB" Class="mediaSize" %>

Imports System
Imports System.Web
Imports System.Web.Script.Serialization
Public Class mediaSize : Implements IHttpHandler, System.Web.SessionState.IRequiresSessionState

  Public Class myData
    Public Property isFirst As Boolean = False
  End Class
  Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
    context.Response.ContentType = "application/json"

    Dim json As New System.Web.Script.Serialization.JavaScriptSerializer()
    Dim tmp As New myData
    tmp.isFirst = False
    If context.Session("BrowserWidth") Is Nothing Then
      tmp.isFirst = True
    End If
    Dim output As String = json.Serialize(tmp)
    context.Response.Write(output)
    Try
      context.Session("BrowserWidth") = context.Request.QueryString("Width")
      context.Session("BrowserHeight") = context.Request.QueryString("Height")
    Catch ex As Exception

    End Try

  End Sub

  Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
    Get
      Return False
    End Get
  End Property

End Class