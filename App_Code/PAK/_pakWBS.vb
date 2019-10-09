Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.PAK
  <DataObject()> _
  Partial Public Class pakWBS
    Private Shared _RecordCount As Integer
    Private _WBSID As String = ""
    Private _Description As String = ""
    Private _WBSLevel As Int32 = 0
    Private _ResponsibleAgencyID As String = ""
    Private _IDM_ResponsibleAgency1_Description As String = ""
    Private _FK_WBS_ResponsibleAgencyID As SIS.PAK.pakResponsibleAgency = Nothing
    Public Property WBSID() As String
      Get
        Return _WBSID
      End Get
      Set(ByVal value As String)
        _WBSID = value
      End Set
    End Property
    Public Property Description() As String
      Get
        Return _Description
      End Get
      Set(ByVal value As String)
        _Description = value
      End Set
    End Property
    Public Property WBSLevel() As Int32
      Get
        Return _WBSLevel
      End Get
      Set(ByVal value As Int32)
        _WBSLevel = value
      End Set
    End Property
    Public Property ResponsibleAgencyID() As String
      Get
        Return _ResponsibleAgencyID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _ResponsibleAgencyID = ""
         Else
           _ResponsibleAgencyID = value
         End If
      End Set
    End Property
    Public Property IDM_ResponsibleAgency1_Description() As String
      Get
        Return _IDM_ResponsibleAgency1_Description
      End Get
      Set(ByVal value As String)
        _IDM_ResponsibleAgency1_Description = value
      End Set
    End Property
    Public Readonly Property DisplayField() As String
      Get
        Return "" & _Description.ToString.PadRight(50, " ")
      End Get
    End Property
    Public Readonly Property PrimaryKey() As String
      Get
        Return _WBSID
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
    Public Class PKpakWBS
      Private _WBSID As String = ""
      Public Property WBSID() As String
        Get
          Return _WBSID
        End Get
        Set(ByVal value As String)
          _WBSID = value
        End Set
      End Property
    End Class
    Public ReadOnly Property FK_WBS_ResponsibleAgencyID() As SIS.PAK.pakResponsibleAgency
      Get
        If _FK_WBS_ResponsibleAgencyID Is Nothing Then
          _FK_WBS_ResponsibleAgencyID = SIS.PAK.pakResponsibleAgency.pakResponsibleAgencyGetByID(_ResponsibleAgencyID)
        End If
        Return _FK_WBS_ResponsibleAgencyID
      End Get
    End Property
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function pakWBSSelectList(ByVal OrderBy As String) As List(Of SIS.PAK.pakWBS)
      Dim Results As List(Of SIS.PAK.pakWBS) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "sppakWBSSelectList"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NvarChar, 9, HttpContext.Current.Session("LoginID"))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OrderBy", SqlDbType.NVarChar, 50, OrderBy)
          Cmd.Parameters.Add("@RecordCount", SqlDbType.Int)
          Cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Results = New List(Of SIS.PAK.pakWBS)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.PAK.pakWBS(Reader))
          End While
          Reader.Close()
          _RecordCount = Cmd.Parameters("@RecordCount").Value
        End Using
      End Using
      Return Results
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function pakWBSGetNewRecord() As SIS.PAK.pakWBS
      Return New SIS.PAK.pakWBS()
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function pakWBSGetByID(ByVal WBSID As String) As SIS.PAK.pakWBS
      Dim Results As SIS.PAK.pakWBS = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "sppakWBSSelectByID"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@WBSID",SqlDbType.NVarChar,WBSID.ToString.Length, WBSID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NvarChar, 9, HttpContext.Current.Session("LoginID"))
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.PAK.pakWBS(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function pakWBSSelectList(ByVal StartRowIndex As Integer, ByVal MaximumRows As Integer, ByVal OrderBy As String, ByVal SearchState As Boolean, ByVal SearchText As String) As List(Of SIS.PAK.pakWBS)
      Dim Results As List(Of SIS.PAK.pakWBS) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          If SearchState Then
            Cmd.CommandText = "sppakWBSSelectListSearch"
            SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@KeyWord", SqlDbType.NVarChar, 250, SearchText)
          Else
            Cmd.CommandText = "sppakWBSSelectListFilteres"
          End If
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StartRowIndex", SqlDbType.Int, -1, StartRowIndex)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaximumRows", SqlDbType.Int, -1, MaximumRows)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NvarChar, 9, HttpContext.Current.Session("LoginID"))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OrderBy", SqlDbType.NVarChar, 50, OrderBy)
          Cmd.Parameters.Add("@RecordCount", SqlDbType.Int)
          Cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Results = New List(Of SIS.PAK.pakWBS)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.PAK.pakWBS(Reader))
          End While
          Reader.Close()
          _RecordCount = Cmd.Parameters("@RecordCount").Value
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function pakWBSSelectCount(ByVal SearchState As Boolean, ByVal SearchText As String) As Integer
      Return _RecordCount
    End Function
      'Select By ID One Record Filtered Overloaded GetByID
    <DataObjectMethod(DataObjectMethodType.Insert, True)> _
    Public Shared Function pakWBSInsert(ByVal Record As SIS.PAK.pakWBS) As SIS.PAK.pakWBS
      Dim _Rec As SIS.PAK.pakWBS = SIS.PAK.pakWBS.pakWBSGetNewRecord()
      With _Rec
        .WBSID = Record.WBSID
        .Description = Record.Description
        .WBSLevel = Record.WBSLevel
        .ResponsibleAgencyID = Record.ResponsibleAgencyID
      End With
      Return SIS.PAK.pakWBS.InsertData(_Rec)
    End Function
    Public Shared Function InsertData(ByVal Record As SIS.PAK.pakWBS) As SIS.PAK.pakWBS
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "sppakWBSInsert"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@WBSID",SqlDbType.NVarChar,9, Record.WBSID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Description",SqlDbType.NVarChar,51, Record.Description)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@WBSLevel",SqlDbType.Int,11, Record.WBSLevel)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ResponsibleAgencyID",SqlDbType.Int,11, Iif(Record.ResponsibleAgencyID= "" ,Convert.DBNull, Record.ResponsibleAgencyID))
          Cmd.Parameters.Add("@Return_WBSID", SqlDbType.NVarChar, 9)
          Cmd.Parameters("@Return_WBSID").Direction = ParameterDirection.Output
          Con.Open()
          Cmd.ExecuteNonQuery()
          Record.WBSID = Cmd.Parameters("@Return_WBSID").Value
        End Using
      End Using
      Return Record
    End Function
    <DataObjectMethod(DataObjectMethodType.Update, True)> _
    Public Shared Function pakWBSUpdate(ByVal Record As SIS.PAK.pakWBS) As SIS.PAK.pakWBS
      Dim _Rec As SIS.PAK.pakWBS = SIS.PAK.pakWBS.pakWBSGetByID(Record.WBSID)
      With _Rec
        .Description = Record.Description
        .WBSLevel = Record.WBSLevel
        .ResponsibleAgencyID = Record.ResponsibleAgencyID
      End With
      Return SIS.PAK.pakWBS.UpdateData(_Rec)
    End Function
    Public Shared Function UpdateData(ByVal Record As SIS.PAK.pakWBS) As SIS.PAK.pakWBS
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "sppakWBSUpdate"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_WBSID",SqlDbType.NVarChar,9, Record.WBSID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@WBSID",SqlDbType.NVarChar,9, Record.WBSID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Description",SqlDbType.NVarChar,51, Record.Description)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@WBSLevel",SqlDbType.Int,11, Record.WBSLevel)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ResponsibleAgencyID",SqlDbType.Int,11, Iif(Record.ResponsibleAgencyID= "" ,Convert.DBNull, Record.ResponsibleAgencyID))
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
    Public Shared Function pakWBSDelete(ByVal Record As SIS.PAK.pakWBS) As Int32
      Dim _Result as Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "sppakWBSDelete"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_WBSID",SqlDbType.NVarChar,Record.WBSID.ToString.Length, Record.WBSID)
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
    Public Shared Function SelectpakWBSAutoCompleteList(ByVal Prefix As String, ByVal count As Integer, ByVal contextKey As String) As String()
      Dim Results As List(Of String) = Nothing
      Dim aVal() As String = contextKey.Split("|".ToCharArray)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "sppakWBSAutoCompleteList"
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
            Dim Tmp As SIS.PAK.pakWBS = New SIS.PAK.pakWBS(Reader)
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
