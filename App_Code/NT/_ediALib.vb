Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.EDI
  <DataObject()> _
  Partial Public Class ediALib
    Private Shared _RecordCount As Integer
    Private _t_lbcd As String = ""
    Private _t_desc As String = ""
    Private _t_serv As String = ""
    Private _t_path As String = ""
    Private _t_acti As String = ""
    Private _t_Refcntd As Int32 = 0
    Private _t_Refcntu As Int32 = 0
    Public Property t_lbcd() As String
      Get
        Return _t_lbcd
      End Get
      Set(ByVal value As String)
        _t_lbcd = value
      End Set
    End Property
    Public Property t_desc() As String
      Get
        Return _t_desc
      End Get
      Set(ByVal value As String)
        _t_desc = value
      End Set
    End Property
    Public Property t_serv() As String
      Get
        Return _t_serv
      End Get
      Set(ByVal value As String)
        _t_serv = value
      End Set
    End Property
    Public Property t_path() As String
      Get
        Return _t_path
      End Get
      Set(ByVal value As String)
        _t_path = value
      End Set
    End Property
    Public Property t_acti() As String
      Get
        Return _t_acti
      End Get
      Set(ByVal value As String)
        _t_acti = value
      End Set
    End Property
    Public Property t_Refcntd() As Int32
      Get
        Return _t_Refcntd
      End Get
      Set(ByVal value As Int32)
        _t_Refcntd = value
      End Set
    End Property
    Public Property t_Refcntu() As Int32
      Get
        Return _t_Refcntu
      End Get
      Set(ByVal value As Int32)
        _t_Refcntu = value
      End Set
    End Property
    Public Readonly Property DisplayField() As String
      Get
        Return ""
      End Get
    End Property
    Public Readonly Property PrimaryKey() As String
      Get
        Return _t_lbcd
      End Get
    End Property
    Public Shared Property RecordCount() As Integer
      Get
        Return _RecordCount
      End Get
      Set(ByVal value As Integer)
        _RecordCount = value
      End Set
    End Property
    Public Class PKediALib
      Private _t_lbcd As String = ""
      Public Property t_lbcd() As String
        Get
          Return _t_lbcd
        End Get
        Set(ByVal value As String)
          _t_lbcd = value
        End Set
      End Property
    End Class
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function ediALibGetNewRecord() As SIS.EDI.ediALib
      Return New SIS.EDI.ediALib()
    End Function
    Public Shared Function GetActiveLibrary() As SIS.EDI.ediALib
      Dim Results As SIS.EDI.ediALib = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "select top 1 * from ttcisg127200 where t_acti = 'Y'"
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.EDI.ediALib(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)>
    Public Shared Function ediALibGetByID(ByVal t_lbcd As String) As SIS.EDI.ediALib
      Dim Results As SIS.EDI.ediALib = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spediALibSelectByID"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_lbcd", SqlDbType.VarChar, t_lbcd.ToString.Length, t_lbcd)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 9, "0340")
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.EDI.ediALib(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)>
    Public Shared Function ediALibSelectList(ByVal StartRowIndex As Integer, ByVal MaximumRows As Integer, ByVal OrderBy As String, ByVal SearchState As Boolean, ByVal SearchText As String) As List(Of SIS.EDI.ediALib)
      Dim Results As List(Of SIS.EDI.ediALib) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          If SearchState Then
            Cmd.CommandText = "spediALibSelectListSearch"
            SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@KeyWord", SqlDbType.NVarChar, 250, SearchText)
          Else
            Cmd.CommandText = "spediALibSelectListFilteres"
          End If
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StartRowIndex", SqlDbType.Int, -1, StartRowIndex)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaximumRows", SqlDbType.Int, -1, MaximumRows)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 9, "0340")
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OrderBy", SqlDbType.NVarChar, 50, OrderBy)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_acti", SqlDbType.VarChar, 1, True)
          Cmd.Parameters.Add("@RecordCount", SqlDbType.Int)
          Cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Results = New List(Of SIS.EDI.ediALib)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.EDI.ediALib(Reader))
          End While
          Reader.Close()
          _RecordCount = Cmd.Parameters("@RecordCount").Value
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function ediALibSelectCount(ByVal SearchState As Boolean, ByVal SearchText As String) As Integer
      Return _RecordCount
    End Function
    'Select By ID One Record Filtered Overloaded GetByID
    <DataObjectMethod(DataObjectMethodType.Insert, True)>
    Public Shared Function ediALibInsert(ByVal Record As SIS.EDI.ediALib) As SIS.EDI.ediALib
      Dim _Rec As SIS.EDI.ediALib = SIS.EDI.ediALib.ediALibGetNewRecord()
      With _Rec
        .t_lbcd = Record.t_lbcd
        .t_desc = Record.t_desc
        .t_serv = Record.t_serv
        .t_path = Record.t_path
        .t_acti = Record.t_acti
        .t_Refcntd = Record.t_Refcntd
        .t_Refcntu = Record.t_Refcntu
      End With
      Return SIS.EDI.ediALib.InsertData(_Rec)
    End Function
    Public Shared Function InsertData(ByVal Record As SIS.EDI.ediALib) As SIS.EDI.ediALib
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spediALibInsert"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_lbcd", SqlDbType.VarChar, 51, Record.t_lbcd)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_desc", SqlDbType.VarChar, 101, Record.t_desc)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_serv", SqlDbType.VarChar, 51, Record.t_serv)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_path", SqlDbType.VarChar, 51, Record.t_path)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_acti", SqlDbType.VarChar, 2, Record.t_acti)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_Refcntd", SqlDbType.Int, 11, Record.t_Refcntd)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_Refcntu", SqlDbType.Int, 11, Record.t_Refcntu)
          Cmd.Parameters.Add("@Return_t_lbcd", SqlDbType.VarChar, 51)
          Cmd.Parameters("@Return_t_lbcd").Direction = ParameterDirection.Output
          Con.Open()
          Cmd.ExecuteNonQuery()
          Record.t_lbcd = Cmd.Parameters("@Return_t_lbcd").Value
        End Using
      End Using
      Return Record
    End Function
    <DataObjectMethod(DataObjectMethodType.Update, True)>
    Public Shared Function ediALibUpdate(ByVal Record As SIS.EDI.ediALib) As SIS.EDI.ediALib
      Dim _Rec As SIS.EDI.ediALib = SIS.EDI.ediALib.ediALibGetByID(Record.t_lbcd)
      With _Rec
        .t_desc = Record.t_desc
        .t_serv = Record.t_serv
        .t_path = Record.t_path
        .t_acti = Record.t_acti
        .t_Refcntd = Record.t_Refcntd
        .t_Refcntu = Record.t_Refcntu
      End With
      Return SIS.EDI.ediALib.UpdateData(_Rec)
    End Function
    Public Shared Function UpdateData(ByVal Record As SIS.EDI.ediALib) As SIS.EDI.ediALib
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spediALibUpdate"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_t_lbcd", SqlDbType.VarChar, 51, Record.t_lbcd)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_lbcd", SqlDbType.VarChar, 51, Record.t_lbcd)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_desc", SqlDbType.VarChar, 101, Record.t_desc)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_serv", SqlDbType.VarChar, 51, Record.t_serv)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_path", SqlDbType.VarChar, 51, Record.t_path)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_acti", SqlDbType.VarChar, 2, Record.t_acti)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_Refcntd", SqlDbType.Int, 11, Record.t_Refcntd)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_Refcntu", SqlDbType.Int, 11, Record.t_Refcntu)
          Cmd.Parameters.Add("@RowCount", SqlDbType.Int)
          Cmd.Parameters("@RowCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Con.Open()
          Cmd.ExecuteNonQuery()
          _RecordCount = Cmd.Parameters("@RowCount").Value
        End Using
      End Using
      Return Record
    End Function
    <DataObjectMethod(DataObjectMethodType.Delete, True)>
    Public Shared Function ediALibDelete(ByVal Record As SIS.EDI.ediALib) As Int32
      Dim _Result As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spediALibDelete"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_t_lbcd", SqlDbType.VarChar, Record.t_lbcd.ToString.Length, Record.t_lbcd)
          Cmd.Parameters.Add("@RowCount", SqlDbType.Int)
          Cmd.Parameters("@RowCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Con.Open()
          Cmd.ExecuteNonQuery()
          _RecordCount = Cmd.Parameters("@RowCount").Value
        End Using
      End Using
      Return _RecordCount
    End Function
    Public Sub New(ByVal Reader As SqlDataReader)
      Try
        For Each pi As System.Reflection.PropertyInfo In Me.GetType.GetProperties
          If pi.MemberType = Reflection.MemberTypes.Property Then
            Try
              Dim Found As Boolean = False
              For I As Integer = 0 To Reader.FieldCount - 1
                If Reader.GetName(I).ToLower = pi.Name.ToLower Then
                  Found = True
                  Exit For
                End If
              Next
              If Found Then
                If Convert.IsDBNull(Reader(pi.Name)) Then
                  Select Case Reader.GetDataTypeName(Reader.GetOrdinal(pi.Name))
                    Case "decimal"
                      CallByName(Me, pi.Name, CallType.Let, "0.00")
                    Case "bit"
                      CallByName(Me, pi.Name, CallType.Let, Boolean.FalseString)
                    Case Else
                      CallByName(Me, pi.Name, CallType.Let, String.Empty)
                  End Select
                Else
                  CallByName(Me, pi.Name, CallType.Let, Reader(pi.Name))
                End If
              End If
            Catch ex As Exception
            End Try
          End If
        Next
      Catch ex As Exception
      End Try
    End Sub
    Public Sub New()
    End Sub
  End Class
End Namespace
