Imports System.Data.SqlClient
Imports System.Data
Imports System.Web
Imports Microsoft.VisualBasic
Imports System
Namespace SIS.SYS.Utilities
  Public Class Tools
    Public Shared Function UpdateObject(ByVal s As Object, ByVal t As Object) As Object
      Try
        For Each pi As System.Reflection.PropertyInfo In t.GetType.GetProperties
          If pi.MemberType = Reflection.MemberTypes.Property Then
            Try
              CallByName(t, pi.Name, CallType.Let, CallByName(s, pi.Name, CallType.Get))
            Catch ex As Exception
            End Try
          End If
        Next
      Catch ex As Exception
        Dim aa As String = ex.Message
      End Try
      Return t
    End Function
  End Class
  Public Class ApplicationSpacific
    Public Shared Function GenerateSupplierAuthorization(ByVal UserID As String, Optional ByVal Forced As Boolean = False) As Boolean
      Dim mGenerate As Boolean = True
      Dim ForApplication As String = HttpContext.Current.Session("ApplicationID")
      Dim mFile As String = HttpContext.Current.Server.MapPath("~/../UserRights/") & ForApplication & "/" & UserID & "_nMenu.xml"
      If Not Forced Then
        If IO.File.Exists(mFile) Then
          mGenerate = False
        End If
      End If
      If mGenerate Then
        Dim oWS As New WebAuthorization.WebAuthorizationServiceSoapClient
        Dim Roles() As String = Web.Configuration.WebConfigurationManager.AppSettings("SupplierRoleID").ToString.Split(",".ToCharArray)
        Dim str As String = ""
        For Each role As String In Roles
          Try
            str = oWS.CreateWebAuthorization(102, UserID, role)
          Catch ex As Exception
          End Try
          If str <> String.Empty Then
            Exit For
          End If
        Next
      End If
      Return True
    End Function
    Public Shared Sub Initialize()
      With HttpContext.Current
        .Session("ApplicationID") = 103
        .Session("ApplicationDefaultPage") = "~/Default.aspx"
      End With
      ejiVault.EJI.DBCommon.BaaNLive = Convert.ToBoolean(ConfigurationManager.AppSettings("BaaNLive"))
      ejiVault.EJI.DBCommon.ERPCompany = "200"
      ejiVault.EJI.DBCommon.IsLocalISGECVault = Convert.ToBoolean(ConfigurationManager.AppSettings("IsLocalISGECVault"))
      ejiVault.EJI.DBCommon.ISGECVaultIP = ConfigurationManager.AppSettings("ISGECVaultIP")
    End Sub
    Public Shared Function ContentType(ByVal FileName As String) As String
      Dim mRet As String = "application/octet-stream"
      Dim Extn As String = IO.Path.GetExtension(FileName).ToLower.Replace(".", "")
      Select Case Extn
        Case "pdf", "rtf"
          mRet = "application/" & Extn
        Case "doc", "docx"
          mRet = "application/vnd.ms-works"
        Case "xls", "xlsx"
          mRet = "application/vnd.ms-excel"
        Case "gif", "jpg", "jpeg", "png", "tif", "bmp"
          mRet = "image/" & Extn
        Case "pot", "ppt", "pps", "pptx", "ppsx"
          mRet = "application/vnd.ms-powerpoint"
        Case "htm", "html"
          mRet = "text/HTML"
        Case "txt"
          mRet = "text/plain"
        Case "zip"
          mRet = "application/zip"
        Case "rar", "tar", "tgz"
          mRet = "application/x-compressed"
        Case Else
          mRet = "application/octet-stream"
      End Select
      Return mRet
    End Function
  End Class
End Namespace