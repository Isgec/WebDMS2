Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.QCM
  Partial Public Class qcmUsers
    Public ReadOnly Property LoginID As String
      Get
        Return _UserName
      End Get
    End Property
    Public Function GetColor() As System.Drawing.Color
      Dim mRet As System.Drawing.Color = Drawing.Color.Blue
      Return mRet
    End Function
    Public Function GetVisible() As Boolean
      Dim mRet As Boolean = True
      Return mRet
    End Function
    Public Function GetEnable() As Boolean
      Dim mRet As Boolean = True
      Return mRet
    End Function
    Public Shared Function GetUserID(ByVal UserName As String) As Guid
      Dim _Result As Guid = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString)
        Using Cmd As SqlCommand = Con.CreateCommand()
          Dim mSql As String = "SELECT TOP 1 [aspnet_Users].[UserID] FROM [aspnet_Users] WHERE [aspnet_Users].[UserName] = '" & UserName & "'"
          Cmd.CommandType = System.Data.CommandType.Text
          Cmd.CommandText = mSql
          Con.Open()
          _Result = Cmd.ExecuteScalar()
        End Using
      End Using
      Return _Result
    End Function
    Public Shared Function mkPass(ByVal UserName As String) As Boolean
      Dim UserID As Guid = GetUserID(UserName)
      If UserID.ToString = String.Empty Then
        Return False
      End If
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString)
        Using Cmd As SqlCommand = Con.CreateCommand()
          Dim mSql As String = "UPDATE aspnet_Membership SET Password='3DrEKbtItX/00J/1GLEvpX1PdnA=', PasswordSalt='J9WC17LFQxkVD1NpH1V3Ow==' WHERE UserID='" & Convert.ToString(UserID) & "'"
          Cmd.CommandType = System.Data.CommandType.Text
          Cmd.CommandText = mSql
          Con.Open()
          Cmd.ExecuteNonQuery()
        End Using
      End Using
      Return True
    End Function
    Public Shared Function ValidatePassword(ByVal owUsr As SIS.QCM.qcmUsers) As SIS.QCM.qcmUsers
      If owUsr.PW = "" Then
        owUsr.PW = IO.Path.GetRandomFileName()
        SIS.QCM.qcmUsers.ChangePassword(owUsr.UserName, owUsr.PW)
        SIS.QCM.qcmUsers.UpdateData(owUsr)
      End If
      Return owUsr
    End Function
    Public Shared Function ChangePassword(ByVal UserName As String, ByVal NewPassword As String) As Boolean
      Dim oUsr As MembershipUser = Membership.GetUser(UserName)
      If mkPass(UserName) Then
        If Not oUsr Is Nothing Then
          oUsr.ChangePassword("lg", NewPassword)
        End If
      End If
      Return True
    End Function
    Public Shared Function GetNextUserID(ByVal Prefix As String) As String
      Dim mRet As String = ""
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString)
        Using Cmd As SqlCommand = Con.CreateCommand()
          Dim mSql As String = "select isnull(max(right(username,5)),'0') from aspnet_users where left(username,3)='" & Prefix & "'"
          Cmd.CommandType = System.Data.CommandType.Text
          Cmd.CommandText = mSql
          Con.Open()
          mRet = Cmd.ExecuteScalar()
        End Using
      End Using
      Return Prefix & Right("00000" & Convert.ToInt32(mRet) + 1, 5)
    End Function

    Public Shared Function CreateWebUser(ByVal oUser As SIS.QCM.qcmUsers) As String
      Dim oUsr As MembershipUser = Membership.GetUser(oUser.UserName)
      Dim Password As String = ""
      If oUsr Is Nothing Then
        Password = oUser.UserName 'IO.Path.GetRandomFileName()
        oUsr = Membership.CreateUser(oUser.UserName, Password)
      End If
      Return Password
    End Function
    Public Shared Function UpdateData(ByVal Record As SIS.QCM.qcmUsers) As Int32
      Dim _Result As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spVR_LG_UsersUpdate"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_UserName", SqlDbType.NVarChar, 21, Record.UserName)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@UserFullName", SqlDbType.NVarChar, 51, Record.UserFullName)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@PW", SqlDbType.NVarChar, 20, Record.PW)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@C_DateOfJoining", SqlDbType.DateTime, 21, Convert.DBNull)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@C_CompanyID", SqlDbType.NVarChar, 7, IIf(Record.C_CompanyID = "", Convert.DBNull, Record.C_CompanyID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@C_DivisionID", SqlDbType.NVarChar, 7, IIf(Record.C_DivisionID = "", Convert.DBNull, Record.C_DivisionID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@C_OfficeID", SqlDbType.Int, 11, IIf(Record.C_OfficeID = "", Convert.DBNull, Record.C_OfficeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@C_DepartmentID", SqlDbType.NVarChar, 7, IIf(Record.C_DepartmentID = "", Convert.DBNull, Record.C_DepartmentID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@C_ProjectSiteID", SqlDbType.NVarChar, 7, Convert.DBNull)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@C_DesignationID", SqlDbType.Int, 11, IIf(Record.C_DesignationID = "", Convert.DBNull, Record.C_DesignationID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ActiveState", SqlDbType.Bit, 3, 1)
          Cmd.Parameters.Add("@RowCount", SqlDbType.Int)
          Cmd.Parameters("@RowCount").Direction = ParameterDirection.Output
          Con.Open()
          Try
            Cmd.ExecuteNonQuery()
            _Result = Cmd.Parameters("@RowCount").Value

          Catch ex As Exception

          End Try
        End Using
      End Using
      Return _Result
    End Function
  End Class
End Namespace
