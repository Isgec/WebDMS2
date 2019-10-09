Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.DMS
  <DataObject()> _
  Partial Public Class dmsItemTypes
    Private Shared _RecordCount As Integer
    Public Property ItemTypeID As Int32 = 0
    Public Property Description As String = ""
    Public Property Active As Boolean = False
    Public Property UQuality As Boolean = False
    Public Property IQuality As Boolean = False
    Public Property InheritFromParent As Boolean = False
    Public Property SearchInParent As Boolean = False
    Public Property ShowInList As Boolean = False
    Public Property Sequence As Int32 = 0
    Public ReadOnly Property DisplayField() As String
      Get
        Return "" & _Description.ToString.PadRight(50, " ")
      End Get
    End Property
    Public ReadOnly Property PrimaryKey() As String
      Get
        Return _ItemTypeID
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
    Public Shared Function dmsItemTypesSelectList(ByVal OrderBy As String) As List(Of SIS.DMS.dmsItemTypes)
      Dim Results As List(Of SIS.DMS.dmsItemTypes) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemTypesSelectList"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 9, "") 'HttpContext.Current.Session("LoginID"))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OrderBy", SqlDbType.NVarChar, 50, OrderBy)
          Cmd.Parameters.Add("@RecordCount", SqlDbType.Int)
          Cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Results = New List(Of SIS.DMS.dmsItemTypes)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.dmsItemTypes(Reader))
          End While
          Reader.Close()
          _RecordCount = Cmd.Parameters("@RecordCount").Value
        End Using
      End Using
      Return Results
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function dmsItemTypesGetNewRecord() As SIS.DMS.dmsItemTypes
      Return New SIS.DMS.dmsItemTypes()
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function dmsItemTypesGetByID(ByVal ItemTypeID As Int32) As SIS.DMS.dmsItemTypes
      Dim Results As SIS.DMS.dmsItemTypes = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemTypesSelectByID"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemTypeID",SqlDbType.Int,ItemTypeID.ToString.Length, ItemTypeID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 9, "")
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.DMS.dmsItemTypes(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function dmsItemTypesSelectList(ByVal StartRowIndex As Integer, ByVal MaximumRows As Integer, ByVal OrderBy As String, ByVal SearchState As Boolean, ByVal SearchText As String) As List(Of SIS.DMS.dmsItemTypes)
      Dim Results As List(Of SIS.DMS.dmsItemTypes) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          If SearchState Then
            Cmd.CommandText = "spdmsItemTypesSelectListSearch"
            SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@KeyWord", SqlDbType.NVarChar, 250, SearchText)
          Else
            Cmd.CommandText = "spdmsItemTypesSelectListFilteres"
          End If
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StartRowIndex", SqlDbType.Int, -1, StartRowIndex)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaximumRows", SqlDbType.Int, -1, MaximumRows)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NvarChar, 9, HttpContext.Current.Session("LoginID"))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OrderBy", SqlDbType.NVarChar, 50, OrderBy)
          Cmd.Parameters.Add("@RecordCount", SqlDbType.Int)
          Cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Results = New List(Of SIS.DMS.dmsItemTypes)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.dmsItemTypes(Reader))
          End While
          Reader.Close()
          _RecordCount = Cmd.Parameters("@RecordCount").Value
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function dmsItemTypesSelectCount(ByVal SearchState As Boolean, ByVal SearchText As String) As Integer
      Return _RecordCount
    End Function
      'Select By ID One Record Filtered Overloaded GetByID
    <DataObjectMethod(DataObjectMethodType.Insert, True)> _
    Public Shared Function dmsItemTypesInsert(ByVal Record As SIS.DMS.dmsItemTypes) As SIS.DMS.dmsItemTypes
      Dim _Rec As SIS.DMS.dmsItemTypes = SIS.DMS.dmsItemTypes.dmsItemTypesGetNewRecord()
      With _Rec
        .ItemTypeID = Record.ItemTypeID
        .Description = Record.Description
        .Active = Record.Active
        .UQuality = Record.UQuality
        .IQuality = Record.IQuality
        .InheritFromParent = Record.InheritFromParent
        .SearchInParent = Record.SearchInParent
        .ShowInList = Record.ShowInList
        .Sequence = Record.Sequence
      End With
      Return SIS.DMS.dmsItemTypes.InsertData(_Rec)
    End Function
    Public Shared Function InsertData(ByVal Record As SIS.DMS.dmsItemTypes) As SIS.DMS.dmsItemTypes
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemTypesInsert"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemTypeID",SqlDbType.Int,11, Record.ItemTypeID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Description",SqlDbType.NVarChar,51, Iif(Record.Description= "" ,Convert.DBNull, Record.Description))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Active",SqlDbType.Bit,3, Record.Active)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@UQuality",SqlDbType.Bit,3, Record.UQuality)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IQuality",SqlDbType.Bit,3, Record.IQuality)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@InheritFromParent",SqlDbType.Bit,3, Record.InheritFromParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@SearchInParent",SqlDbType.Bit,3, Record.SearchInParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ShowInList",SqlDbType.Bit,3, Record.ShowInList)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Sequence",SqlDbType.Int,11, Record.Sequence)
          Cmd.Parameters.Add("@Return_ItemTypeID", SqlDbType.Int, 11)
          Cmd.Parameters("@Return_ItemTypeID").Direction = ParameterDirection.Output
          Con.Open()
          Cmd.ExecuteNonQuery()
          Record.ItemTypeID = Cmd.Parameters("@Return_ItemTypeID").Value
        End Using
      End Using
      Return Record
    End Function
    <DataObjectMethod(DataObjectMethodType.Update, True)> _
    Public Shared Function dmsItemTypesUpdate(ByVal Record As SIS.DMS.dmsItemTypes) As SIS.DMS.dmsItemTypes
      Dim _Rec As SIS.DMS.dmsItemTypes = SIS.DMS.dmsItemTypes.dmsItemTypesGetByID(Record.ItemTypeID)
      With _Rec
        .Description = Record.Description
        .Active = Record.Active
        .UQuality = Record.UQuality
        .IQuality = Record.IQuality
        .InheritFromParent = Record.InheritFromParent
        .SearchInParent = Record.SearchInParent
        .ShowInList = Record.ShowInList
        .Sequence = Record.Sequence
      End With
      Return SIS.DMS.dmsItemTypes.UpdateData(_Rec)
    End Function
    Public Shared Function UpdateData(ByVal Record As SIS.DMS.dmsItemTypes) As SIS.DMS.dmsItemTypes
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemTypesUpdate"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_ItemTypeID",SqlDbType.Int,11, Record.ItemTypeID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemTypeID",SqlDbType.Int,11, Record.ItemTypeID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Description",SqlDbType.NVarChar,51, Iif(Record.Description= "" ,Convert.DBNull, Record.Description))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Active",SqlDbType.Bit,3, Record.Active)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@UQuality",SqlDbType.Bit,3, Record.UQuality)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IQuality",SqlDbType.Bit,3, Record.IQuality)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@InheritFromParent",SqlDbType.Bit,3, Record.InheritFromParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@SearchInParent",SqlDbType.Bit,3, Record.SearchInParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ShowInList",SqlDbType.Bit,3, Record.ShowInList)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Sequence",SqlDbType.Int,11, Record.Sequence)
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
    Public Shared Function dmsItemTypesDelete(ByVal Record As SIS.DMS.dmsItemTypes) As Int32
      Dim _Result as Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemTypesDelete"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_ItemTypeID",SqlDbType.Int,Record.ItemTypeID.ToString.Length, Record.ItemTypeID)
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
    Public Shared Function SelectdmsItemTypesAutoCompleteList(ByVal Prefix As String, ByVal count As Integer, ByVal contextKey As String) As String()
      Dim Results As List(Of String) = Nothing
      Dim aVal() As String = contextKey.Split("|".ToCharArray)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemTypesAutoCompleteList"
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
            Dim Tmp As SIS.DMS.dmsItemTypes = New SIS.DMS.dmsItemTypes(Reader)
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
