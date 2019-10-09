Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.DMS
  <DataObject()> _
  Partial Public Class dmsStates
    Private Shared _RecordCount As Integer
    Private _StatusID As Int32 = 0
    Private _Description As String = ""
    Private _OpenType As Boolean = False
    Private _WorkflowType As Boolean = False
    Private _RevisionTrigger As Boolean = False
    Private _Active As Boolean = False
    Private _IsStart As Boolean = False
    Private _IsFinish As Boolean = False
    Private _AlertTrigger As Boolean = False
    Private _AlertWithDownloadLink As Boolean = False
    Public ReadOnly Property ForeColor() As System.Drawing.Color
      Get
        Dim mRet As System.Drawing.Color = Drawing.Color.Blue
        Try
          mRet = GetColor()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public ReadOnly Property Visible() As Boolean
      Get
        Dim mRet As Boolean = True
        Try
          mRet = GetVisible()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public ReadOnly Property Enable() As Boolean
      Get
        Dim mRet As Boolean = True
        Try
          mRet = GetEnable()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public Property StatusID() As Int32
      Get
        Return _StatusID
      End Get
      Set(ByVal value As Int32)
        _StatusID = value
      End Set
    End Property
    Public Property Description() As String
      Get
        Return _Description
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _Description = ""
         Else
           _Description = value
         End If
      End Set
    End Property
    Public Property OpenType() As Boolean
      Get
        Return _OpenType
      End Get
      Set(ByVal value As Boolean)
        _OpenType = value
      End Set
    End Property
    Public Property WorkflowType() As Boolean
      Get
        Return _WorkflowType
      End Get
      Set(ByVal value As Boolean)
        _WorkflowType = value
      End Set
    End Property
    Public Property RevisionTrigger() As Boolean
      Get
        Return _RevisionTrigger
      End Get
      Set(ByVal value As Boolean)
        _RevisionTrigger = value
      End Set
    End Property
    Public Property Active() As Boolean
      Get
        Return _Active
      End Get
      Set(ByVal value As Boolean)
        _Active = value
      End Set
    End Property
    Public Property IsStart() As Boolean
      Get
        Return _IsStart
      End Get
      Set(ByVal value As Boolean)
        _IsStart = value
      End Set
    End Property
    Public Property IsFinish() As Boolean
      Get
        Return _IsFinish
      End Get
      Set(ByVal value As Boolean)
        _IsFinish = value
      End Set
    End Property
    Public Property AlertTrigger() As Boolean
      Get
        Return _AlertTrigger
      End Get
      Set(ByVal value As Boolean)
        _AlertTrigger = value
      End Set
    End Property
    Public Property AlertWithDownloadLink() As Boolean
      Get
        Return _AlertWithDownloadLink
      End Get
      Set(ByVal value As Boolean)
        _AlertWithDownloadLink = value
      End Set
    End Property
    Public Readonly Property DisplayField() As String
      Get
        Return "" & _Description.ToString.PadRight(50, " ")
      End Get
    End Property
    Public Readonly Property PrimaryKey() As String
      Get
        Return _StatusID
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
    Public Class PKdmsStates
      Private _StatusID As Int32 = 0
      Public Property StatusID() As Int32
        Get
          Return _StatusID
        End Get
        Set(ByVal value As Int32)
          _StatusID = value
        End Set
      End Property
    End Class
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function dmsStatesSelectList(ByVal OrderBy As String) As List(Of SIS.DMS.dmsStates)
      Dim Results As List(Of SIS.DMS.dmsStates) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsStatesSelectList"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 9, "")
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OrderBy", SqlDbType.NVarChar, 50, OrderBy)
          Cmd.Parameters.Add("@RecordCount", SqlDbType.Int)
          Cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Results = New List(Of SIS.DMS.dmsStates)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.dmsStates(Reader))
          End While
          Reader.Close()
          _RecordCount = Cmd.Parameters("@RecordCount").Value
        End Using
      End Using
      Return Results
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function dmsStatesGetNewRecord() As SIS.DMS.dmsStates
      Return New SIS.DMS.dmsStates()
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function dmsStatesGetByID(ByVal StatusID As Int32) As SIS.DMS.dmsStates
      Dim Results As SIS.DMS.dmsStates = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsStatesSelectByID"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StatusID",SqlDbType.Int,StatusID.ToString.Length, StatusID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NvarChar, 9, HttpContext.Current.Session("LoginID"))
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.DMS.dmsStates(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function dmsStatesSelectList(ByVal StartRowIndex As Integer, ByVal MaximumRows As Integer, ByVal OrderBy As String, ByVal SearchState As Boolean, ByVal SearchText As String) As List(Of SIS.DMS.dmsStates)
      Dim Results As List(Of SIS.DMS.dmsStates) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          If SearchState Then
            Cmd.CommandText = "spdmsStatesSelectListSearch"
            SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@KeyWord", SqlDbType.NVarChar, 250, SearchText)
          Else
            Cmd.CommandText = "spdmsStatesSelectListFilteres"
          End If
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StartRowIndex", SqlDbType.Int, -1, StartRowIndex)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaximumRows", SqlDbType.Int, -1, MaximumRows)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NvarChar, 9, HttpContext.Current.Session("LoginID"))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OrderBy", SqlDbType.NVarChar, 50, OrderBy)
          Cmd.Parameters.Add("@RecordCount", SqlDbType.Int)
          Cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Results = New List(Of SIS.DMS.dmsStates)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.dmsStates(Reader))
          End While
          Reader.Close()
          _RecordCount = Cmd.Parameters("@RecordCount").Value
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function dmsStatesSelectCount(ByVal SearchState As Boolean, ByVal SearchText As String) As Integer
      Return _RecordCount
    End Function
      'Select By ID One Record Filtered Overloaded GetByID
    <DataObjectMethod(DataObjectMethodType.Insert, True)> _
    Public Shared Function dmsStatesInsert(ByVal Record As SIS.DMS.dmsStates) As SIS.DMS.dmsStates
      Dim _Rec As SIS.DMS.dmsStates = SIS.DMS.dmsStates.dmsStatesGetNewRecord()
      With _Rec
        .StatusID = Record.StatusID
        .Description = Record.Description
        .OpenType = Record.OpenType
        .WorkflowType = Record.WorkflowType
        .RevisionTrigger = Record.RevisionTrigger
        .Active = Record.Active
        .IsStart = Record.IsStart
        .IsFinish = Record.IsFinish
        .AlertTrigger = Record.AlertTrigger
        .AlertWithDownloadLink = Record.AlertWithDownloadLink
      End With
      Return SIS.DMS.dmsStates.InsertData(_Rec)
    End Function
    Public Shared Function InsertData(ByVal Record As SIS.DMS.dmsStates) As SIS.DMS.dmsStates
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsStatesInsert"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StatusID",SqlDbType.Int,11, Record.StatusID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Description",SqlDbType.NVarChar,51, Iif(Record.Description= "" ,Convert.DBNull, Record.Description))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OpenType",SqlDbType.Bit,3, Record.OpenType)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@WorkflowType",SqlDbType.Bit,3, Record.WorkflowType)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@RevisionTrigger",SqlDbType.Bit,3, Record.RevisionTrigger)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Active",SqlDbType.Bit,3, Record.Active)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsStart",SqlDbType.Bit,3, Record.IsStart)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsFinish",SqlDbType.Bit,3, Record.IsFinish)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@AlertTrigger",SqlDbType.Bit,3, Record.AlertTrigger)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@AlertWithDownloadLink",SqlDbType.Bit,3, Record.AlertWithDownloadLink)
          Cmd.Parameters.Add("@Return_StatusID", SqlDbType.Int, 11)
          Cmd.Parameters("@Return_StatusID").Direction = ParameterDirection.Output
          Con.Open()
          Cmd.ExecuteNonQuery()
          Record.StatusID = Cmd.Parameters("@Return_StatusID").Value
        End Using
      End Using
      Return Record
    End Function
    <DataObjectMethod(DataObjectMethodType.Update, True)> _
    Public Shared Function dmsStatesUpdate(ByVal Record As SIS.DMS.dmsStates) As SIS.DMS.dmsStates
      Dim _Rec As SIS.DMS.dmsStates = SIS.DMS.dmsStates.dmsStatesGetByID(Record.StatusID)
      With _Rec
        .Description = Record.Description
        .OpenType = Record.OpenType
        .WorkflowType = Record.WorkflowType
        .RevisionTrigger = Record.RevisionTrigger
        .Active = Record.Active
        .IsStart = Record.IsStart
        .IsFinish = Record.IsFinish
        .AlertTrigger = Record.AlertTrigger
        .AlertWithDownloadLink = Record.AlertWithDownloadLink
      End With
      Return SIS.DMS.dmsStates.UpdateData(_Rec)
    End Function
    Public Shared Function UpdateData(ByVal Record As SIS.DMS.dmsStates) As SIS.DMS.dmsStates
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsStatesUpdate"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_StatusID",SqlDbType.Int,11, Record.StatusID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StatusID",SqlDbType.Int,11, Record.StatusID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Description",SqlDbType.NVarChar,51, Iif(Record.Description= "" ,Convert.DBNull, Record.Description))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OpenType",SqlDbType.Bit,3, Record.OpenType)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@WorkflowType",SqlDbType.Bit,3, Record.WorkflowType)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@RevisionTrigger",SqlDbType.Bit,3, Record.RevisionTrigger)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Active",SqlDbType.Bit,3, Record.Active)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsStart",SqlDbType.Bit,3, Record.IsStart)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsFinish",SqlDbType.Bit,3, Record.IsFinish)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@AlertTrigger",SqlDbType.Bit,3, Record.AlertTrigger)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@AlertWithDownloadLink",SqlDbType.Bit,3, Record.AlertWithDownloadLink)
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
    <DataObjectMethod(DataObjectMethodType.Delete, True)> _
    Public Shared Function dmsStatesDelete(ByVal Record As SIS.DMS.dmsStates) As Int32
      Dim _Result as Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsStatesDelete"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_StatusID",SqlDbType.Int,Record.StatusID.ToString.Length, Record.StatusID)
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
'    Autocomplete Method
    Public Shared Function SelectdmsStatesAutoCompleteList(ByVal Prefix As String, ByVal count As Integer, ByVal contextKey As String) As String()
      Dim Results As List(Of String) = Nothing
      Dim aVal() As String = contextKey.Split("|".ToCharArray)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsStatesAutoCompleteList"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NvarChar, 9, HttpContext.Current.Session("LoginID"))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@prefix", SqlDbType.NVarChar, 50, Prefix)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@records", SqlDbType.Int, -1, count)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@bycode", SqlDbType.Int, 1, IIf(IsNumeric(Prefix),0,IIf(Prefix.ToLower=Prefix, 0, 1)))
          Results = New List(Of String)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Not Reader.HasRows Then
            Results.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("---Select Value---".PadRight(50, " "),""))
          End If
          While (Reader.Read())
            Dim Tmp As SIS.DMS.dmsStates = New SIS.DMS.dmsStates(Reader)
            Results.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(Tmp.DisplayField, Tmp.PrimaryKey))
          End While
          Reader.Close()
        End Using
      End Using
      Return Results.ToArray
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
