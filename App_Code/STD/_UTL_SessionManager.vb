Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Net
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
Imports System.Web
Imports System
Namespace SIS.SYS.Utilities

  <AttributeUsage(AttributeTargets.All, AllowMultiple:=False, Inherited:=True)>
  Public Class lgSkipAttribute
    Inherits Attribute
    Private _DoNotWrite As Boolean = True
    Public Property DoNotWrite() As Boolean
      Get
        Return _DoNotWrite
      End Get
      Set(ByVal value As Boolean)
        _DoNotWrite = value
      End Set
    End Property
  End Class
  Public Class SessionManager
    Public Shared ci As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-GB", True)
    Public Shared Function DoLogin(ByVal UserID As String) As Boolean
      Dim mRet As Boolean = False
      If Membership.ValidateUser(UserID, GetPassword(UserID)) Then
        FormsAuthentication.RedirectFromLoginPage(UserID, True)
        SIS.SYS.Utilities.SessionManager.InitializeEnvironment(UserID)
        mRet = True
      End If
      Return mRet
    End Function
    Public Shared Function GetComputerName(ByVal clientIP As String) As String
      Dim he As IPHostEntry = Nothing
      Try
        he = System.Net.Dns.GetHostEntry(clientIP)
        Return he.HostName
      Catch ex As Exception
      End Try
      Return ""
    End Function
    Public Shared Sub CreateSessionEnvironement()
      With HttpContext.Current
        .Session("ApplicationID") = 0
        .Session("ApplicationDefaultPage") = ""
        .Session("LoginID") = ""
        .Session("PageSizeProvider") = False
        .Session("PageNoProvider") = False
      End With
      HttpContext.Current.Session("IsAuthenticated") = False
    End Sub
    Public Shared Sub InitializeEnvironment(ByVal UserID As String)
      HttpContext.Current.Session("LoginID") = UserID
      CommonInitialize()
    End Sub
    Private Shared Sub CommonInitialize()
      InitNavBar()
      SIS.SYS.Utilities.ApplicationSpacific.Initialize()
    End Sub
    Public Shared Sub DestroySessionEnvironement()
      With HttpContext.Current
        .Session.Clear()
        .Session.Abandon()
      End With
    End Sub
    Public Class lgNavBar
      Public Property Target As String = ""
      Public Property Popping As Boolean = False
      Public Sub New(ByVal targetUrl As String)
        Target = targetUrl
        Popping = False
      End Sub
      Sub New()
        'dummy  
      End Sub
    End Class
    Public Shared Sub InitNavBar()
      HttpContext.Current.Session("NavBar") = New List(Of lgNavBar)
    End Sub
    Public Shared Sub PushNavBar(ByVal Target As String)
      Dim tmpNav As List(Of lgNavBar) = HttpContext.Current.Session("NavBar")
      Dim Found As Boolean = False
      For Each x As lgNavBar In tmpNav
        If x.Target = Target Then
          Found = True
          Exit For
        End If
      Next
      If Found Then
        For I As Integer = tmpNav.Count - 1 To 0 Step -1
          If tmpNav(I).Target = Target Then
            Exit For
          End If
          tmpNav.Remove(tmpNav(I))
        Next
      Else
        tmpNav.Add(New lgNavBar(Target))
      End If
      HttpContext.Current.Session("NavBar") = tmpNav
    End Sub
    Public Shared Function PopNavBar(Optional ByVal IsBack As Boolean = False) As String
      Dim mRet As String = HttpContext.Current.Session("ApplicationDefaultPage")
      Dim tmp As lgNavBar = Nothing
      Dim tmpNav As List(Of lgNavBar) = HttpContext.Current.Session("NavBar")
      If IsBack Then
        If tmpNav.Count > 1 Then
          mRet = tmpNav(tmpNav.Count - 2).Target
        End If
      Else
        If tmpNav.Count > 0 Then
          mRet = tmpNav(tmpNav.Count - 1).Target
        End If
      End If
      Return mRet
    End Function

    Public Shared Function GetPassword(ByVal Uid As String) As String
      Dim mRet As String = ""
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString)
        Using Cmd As SqlCommand = Con.CreateCommand()
          Dim mSql As String = "Select ISNULL(pw,'') from aspnet_users WHERE UserName = '" & Uid & "'"
          Cmd.CommandType = System.Data.CommandType.Text
          Cmd.CommandText = mSql
          Con.Open()
          mRet = Cmd.ExecuteScalar()
          If mRet Is Nothing Then
            mRet = ""
          End If
        End Using
      End Using
      Return mRet
    End Function
  End Class
  Public Class GlobalVariables
    Public Shared Function PageNo(ByVal PageName As String, ByVal LoginID As String) As Integer
      Dim _Result As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString)
        Using Cmd As SqlCommand = Con.CreateCommand()
          Dim mSql As String = "SELECT TOP 1 [SYS_LGPageSize].[PageNo] FROM [SYS_LGPageSize] WHERE [SYS_LGPageSize].[PageName] = '" & PageName & "' AND [SYS_LGPageSize].[LoginID] = '" & LoginID & "' AND [SYS_LGPageSize].[ApplicationID] = '" & HttpContext.Current.Session("ApplicationID") & "'"
          Cmd.CommandType = System.Data.CommandType.Text
          Cmd.CommandText = mSql
          Con.Open()
          _Result = Cmd.ExecuteScalar()
          If _Result = 0 Then
            _Result = 0
          End If
        End Using
      End Using
      Return _Result
    End Function
    Public Shared Function PageNo(ByVal PageName As String, ByVal LoginID As String, ByVal Position As Integer) As Integer
      Dim _Result As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spSYS_LG_SetPageNumber"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@PageName", SqlDbType.NVarChar, 250, PageName)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 20, LoginID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@PageNo", SqlDbType.Int, 10, Position)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ApplicationID", SqlDbType.Int, 10, Global.System.Web.HttpContext.Current.Session("ApplicationID"))
          Con.Open()
          Cmd.ExecuteNonQuery()
        End Using
      End Using
      Return _Result
    End Function
    Public Shared Function PageSize(ByVal PageName As String, ByVal LoginID As String) As Integer
      Dim _Result As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString)
        Using Cmd As SqlCommand = Con.CreateCommand()
          Dim mSql As String = "SELECT TOP 1 [SYS_LGPageSize].[PageSize] FROM [SYS_LGPageSize] WHERE [SYS_LGPageSize].[PageName] = '" & PageName & "' AND [SYS_LGPageSize].[LoginID] = '" & LoginID & "' AND [SYS_LGPageSize].[ApplicationID] = " & HttpContext.Current.Session("ApplicationID")
          Cmd.CommandType = System.Data.CommandType.Text
          Cmd.CommandText = mSql
          Con.Open()
          _Result = Cmd.ExecuteScalar()
          If _Result = 0 Then
            _Result = 10
          End If
        End Using
      End Using
      Return _Result
    End Function
    Public Shared Function PageSize(ByVal PageName As String, ByVal LoginID As String, ByVal Size As Integer) As Integer
      Dim _Result As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spSYS_LG_SetPageSize"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@PageName", SqlDbType.NVarChar, 250, PageName)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 20, LoginID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@PageSize", SqlDbType.Int, 10, Size)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ApplicationID", SqlDbType.Int, 10, Global.System.Web.HttpContext.Current.Session("ApplicationID"))
          Con.Open()
          Cmd.ExecuteNonQuery()
        End Using
      End Using
      Return _Result
    End Function
  End Class
  Public Class ValidateURL
    Public Shared Function Validate(ByVal PageUrl As String) As Boolean
      Dim aParts() As String = PageUrl.Split("/".ToCharArray)
      If aParts.Length <= 3 Then
        Return True
      End If
      Return ValidateURL(PageUrl)
    End Function
    Private Shared Function ValidateURL(ByVal PageUrl As String) As Boolean
      'Return True
      Dim _Result As Boolean = False
      Dim afile() As String = PageUrl.Split("/".ToCharArray)
      Dim FileName As String = afile(afile.GetUpperBound(0)).ToString
      Dim UserCase As String = FileName.Substring(0, 3)
      If FileName.StartsWith("RP_") Then Return True
      Select Case UserCase
        Case "AF_"
          FileName = FileName.Replace("AF_", "GF_")
        Case "EF_"
          FileName = FileName.Replace("EF_", "GF_")
        Case "DF_"
          FileName = FileName.Replace("DF_", "GD_")
      End Select
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spSYS_LG_VRSessionByUserFile"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@FileName", SqlDbType.NVarChar, 251, FileName)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@UserName", SqlDbType.NVarChar, 20, HttpContext.Current.User.Identity.Name)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ApplicationID", SqlDbType.NVarChar, 50, Global.System.Web.HttpContext.Current.Session("ApplicationID"))
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Select Case UserCase
              Case "AF_"
                If Reader("InsertForm") Then
                  _Result = True
                End If
              Case "EF_"
                If Reader("UpdateForm") Then
                  _Result = True
                End If
              Case "DF_"
                If Reader("DisplayForm") Then
                  _Result = True
                End If
              Case "GD_"
                If Reader("DisplayGrid") Then
                  _Result = True
                End If
              Case Else    '"GF_", "GT_", "GU_", "GP_"
                If Reader("MaintainGrid") Then
                  'SYS.Utilities.SessionManager.InitNavBar()
                  _Result = True
                End If
            End Select
          End If
          Reader.Close()
        End Using
      End Using
      Return _Result
    End Function
  End Class
End Namespace
