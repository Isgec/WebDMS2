Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.Configuration
Imports Microsoft.VisualBasic
Imports System.Diagnostics
Namespace SIS.SYS.SQLDatabase
  Partial Public Class DBCommon
    Implements IDisposable
    Public Shared Property BaaNLive As Boolean = False
    Public Shared Property JoomlaLive As Boolean = False
    Public Shared Function GetFDBaaNConnectionString() As String
      Dim FDBaaNLive As Boolean = Convert.ToBoolean(ConfigurationManager.AppSettings("FDBaaNLive"))

      If FDBaaNLive Then
        Return "Data Source=192.9.200.129;Initial Catalog=inforerpdb;Integrated Security=False;User Instance=False;Persist Security Info=True;User ID=lalit;Password=scorpions"
      Else
        Return "Data Source=192.9.200.45;Initial Catalog=inforerpdb;Integrated Security=False;User Instance=False;Persist Security Info=True;User ID=lalit;Password=scorpions"
      End If
    End Function

    Public Shared Function GetBaaNConnectionString() As String
      'This is for CT to run on BaaN Test Server for testing as data is available on Test Server
      Dim ct As New StackTrace
      Dim FunctionName As String = ct.GetFrame(1).GetMethod().Name
      Select Case FunctionName
        Case "UZ_dmisg121200SelectList", "UZ_dmisg121200SelectList_All", "ediAFileGetByHandleIndex"
          'Always Return BaaN Live
          Return "Data Source=192.9.200.129;Initial Catalog=inforerpdb;Integrated Security=False;User Instance=False;Persist Security Info=True;User ID=lalit;Password=scorpions"
      End Select
      BaaNLive = Convert.ToBoolean(ConfigurationManager.AppSettings("BaaNLive"))

      If BaaNLive Then
        Return "Data Source=192.9.200.129;Initial Catalog=inforerpdb;Integrated Security=False;User Instance=False;Persist Security Info=True;User ID=lalit;Password=scorpions"
      Else
        Return "Data Source=192.9.200.45;Initial Catalog=inforerpdb;Integrated Security=False;User Instance=False;Persist Security Info=True;User ID=lalit;Password=scorpions"
      End If
    End Function
    Public Shared Function GetVaultConnection(Optional ByVal vaultDB As String = "BOILER") As String
      Return "Data Source=192.9.200.119\autodeskvault;Initial Catalog=" & vaultDB & ";Integrated Security=False;User Instance=False;Persist Security Info=True;User ID=sa;Password=Isgec@123"
    End Function
    Public Shared Function GetConnectionString() As String
      JoomlaLive = Convert.ToBoolean(ConfigurationManager.AppSettings("JoomlaLive"))

      If JoomlaLive Then
        Return "Data Source=192.9.200.150;Initial Catalog=IJTPerks;Integrated Security=False;User Instance=False;Persist Security Info=True;User ID=sa;Password=isgec12345"
      Else
        Return "Data Source=.\LGSQL;Initial Catalog=IJTPerks;Integrated Security=False;User Instance=False;Persist Security Info=True;User ID=sa;Password=isgec12345"
      End If
    End Function
    Public Shared Sub AddDBParameter(ByRef Cmd As SqlCommand, ByVal name As String, ByVal type As SqlDbType, ByVal size As Integer, ByVal value As Object)
      Dim Parm As SqlParameter = Cmd.CreateParameter()
      Parm.ParameterName = name
      Parm.SqlDbType = type
      Parm.Size = size
      Parm.Value = value
      Cmd.Parameters.Add(Parm)
    End Sub
    Private disposedValue As Boolean = False    ' To detect redundant calls

    Shared Sub New()
      BaaNLive = Convert.ToBoolean(ConfigurationManager.AppSettings("BaaNLive"))
      JoomlaLive = Convert.ToBoolean(ConfigurationManager.AppSettings("JoomlaLive"))
    End Sub
    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
      If Not Me.disposedValue Then
        If disposing Then
          ' TODO: free unmanaged resources when explicitly called
        End If

        ' TODO: free shared unmanaged resources
      End If
      Me.disposedValue = True
    End Sub
#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
      ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
      Dispose(True)
      GC.SuppressFinalize(Me)
    End Sub
#End Region

  End Class
End Namespace
