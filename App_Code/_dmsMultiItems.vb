Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.DMS
  <DataObject()> _
  Partial Public Class dmsMultiItems
    Private Shared _RecordCount As Integer
    Private _SerialNo As Int32 = 0
    Private _ItemID As Int32 = 0
    Private _MultiTypeID As String = ""
    Private _MultiItemTypeID As String = ""
    Private _MultiItemID As String = ""
    Private _MultiSequence As Int32 = 0
    Private _DMS_Items1_Description As String = ""
    Private _DMS_Items2_Description As String = ""
    Private _DMS_ItemTypes3_Description As String = ""
    Private _DMS_MultiTypes4_Description As String = ""
    Private _FK_DMS_MultiItems_MultiItemID As SIS.DMS.dmsItems = Nothing
    Private _FK_DMS_MultiItems_ItemID As SIS.DMS.dmsItems = Nothing
    Private _FK_DMS_MultiItems_MultiItemTypeID As SIS.DMS.dmsItemTypes = Nothing
    Private _FK_DMS_MultiItems_MultiTypeID As SIS.DMS.dmsMultiTypes = Nothing
    Public Property CreatedBy As Integer = 0
    Public Property CreatedOn As DateTime = Now
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
    Public Property SerialNo() As Int32
      Get
        Return _SerialNo
      End Get
      Set(ByVal value As Int32)
        _SerialNo = value
      End Set
    End Property
    Public Property ItemID() As Int32
      Get
        Return _ItemID
      End Get
      Set(ByVal value As Int32)
        _ItemID = value
      End Set
    End Property
    Public Property MultiTypeID() As String
      Get
        Return _MultiTypeID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _MultiTypeID = ""
         Else
           _MultiTypeID = value
         End If
      End Set
    End Property
    Public Property MultiItemTypeID() As String
      Get
        Return _MultiItemTypeID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _MultiItemTypeID = ""
         Else
           _MultiItemTypeID = value
         End If
      End Set
    End Property
    Public Property MultiItemID() As String
      Get
        Return _MultiItemID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _MultiItemID = ""
         Else
           _MultiItemID = value
         End If
      End Set
    End Property
    Public Property MultiSequence() As Int32
      Get
        Return _MultiSequence
      End Get
      Set(ByVal value As Int32)
        _MultiSequence = value
      End Set
    End Property
    Public Property DMS_Items1_Description() As String
      Get
        Return _DMS_Items1_Description
      End Get
      Set(ByVal value As String)
        _DMS_Items1_Description = value
      End Set
    End Property
    Public Property DMS_Items2_Description() As String
      Get
        Return _DMS_Items2_Description
      End Get
      Set(ByVal value As String)
        _DMS_Items2_Description = value
      End Set
    End Property
    Public Property DMS_ItemTypes3_Description() As String
      Get
        Return _DMS_ItemTypes3_Description
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _DMS_ItemTypes3_Description = ""
         Else
           _DMS_ItemTypes3_Description = value
         End If
      End Set
    End Property
    Public Property DMS_MultiTypes4_Description() As String
      Get
        Return _DMS_MultiTypes4_Description
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _DMS_MultiTypes4_Description = ""
         Else
           _DMS_MultiTypes4_Description = value
         End If
      End Set
    End Property
    Public Readonly Property DisplayField() As String
      Get
        Return ""
      End Get
    End Property
    Public Readonly Property PrimaryKey() As String
      Get
        Return _SerialNo
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
    Public Class PKdmsMultiItems
      Private _SerialNo As Int32 = 0
      Public Property SerialNo() As Int32
        Get
          Return _SerialNo
        End Get
        Set(ByVal value As Int32)
          _SerialNo = value
        End Set
      End Property
    End Class
    Public ReadOnly Property FK_DMS_MultiItems_MultiItemID() As SIS.DMS.dmsItems
      Get
        If _FK_DMS_MultiItems_MultiItemID Is Nothing Then
          _FK_DMS_MultiItems_MultiItemID = SIS.DMS.dmsItems.dmsItemsGetByID(_MultiItemID)
        End If
        Return _FK_DMS_MultiItems_MultiItemID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_MultiItems_ItemID() As SIS.DMS.dmsItems
      Get
        If _FK_DMS_MultiItems_ItemID Is Nothing Then
          _FK_DMS_MultiItems_ItemID = SIS.DMS.dmsItems.dmsItemsGetByID(_ItemID)
        End If
        Return _FK_DMS_MultiItems_ItemID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_MultiItems_MultiItemTypeID() As SIS.DMS.dmsItemTypes
      Get
        If _FK_DMS_MultiItems_MultiItemTypeID Is Nothing Then
          _FK_DMS_MultiItems_MultiItemTypeID = SIS.DMS.dmsItemTypes.dmsItemTypesGetByID(_MultiItemTypeID)
        End If
        Return _FK_DMS_MultiItems_MultiItemTypeID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_MultiItems_MultiTypeID() As SIS.DMS.dmsMultiTypes
      Get
        If _FK_DMS_MultiItems_MultiTypeID Is Nothing Then
          _FK_DMS_MultiItems_MultiTypeID = SIS.DMS.dmsMultiTypes.dmsMultiTypesGetByID(_MultiTypeID)
        End If
        Return _FK_DMS_MultiItems_MultiTypeID
      End Get
    End Property
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function dmsMultiItemsGetNewRecord() As SIS.DMS.dmsMultiItems
      Return New SIS.DMS.dmsMultiItems()
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function dmsMultiItemsGetByID(ByVal SerialNo As Int32) As SIS.DMS.dmsMultiItems
      Dim Results As SIS.DMS.dmsMultiItems = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsMultiItemsSelectByID"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@SerialNo",SqlDbType.Int,SerialNo.ToString.Length, SerialNo)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NvarChar, 9, HttpContext.Current.Session("LoginID"))
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.DMS.dmsMultiItems(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function dmsMultiItemsSelectList(ByVal StartRowIndex As Integer, ByVal MaximumRows As Integer, ByVal OrderBy As String, ByVal SearchState As Boolean, ByVal SearchText As String) As List(Of SIS.DMS.dmsMultiItems)
      Dim Results As List(Of SIS.DMS.dmsMultiItems) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          If SearchState Then
            Cmd.CommandText = "spdmsMultiItemsSelectListSearch"
            SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@KeyWord", SqlDbType.NVarChar, 250, SearchText)
          Else
            Cmd.CommandText = "spdmsMultiItemsSelectListFilteres"
          End If
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StartRowIndex", SqlDbType.Int, -1, StartRowIndex)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaximumRows", SqlDbType.Int, -1, MaximumRows)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NvarChar, 9, HttpContext.Current.Session("LoginID"))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OrderBy", SqlDbType.NVarChar, 50, OrderBy)
          Cmd.Parameters.Add("@RecordCount", SqlDbType.Int)
          Cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Results = New List(Of SIS.DMS.dmsMultiItems)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.dmsMultiItems(Reader))
          End While
          Reader.Close()
          _RecordCount = Cmd.Parameters("@RecordCount").Value
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function dmsMultiItemsSelectCount(ByVal SearchState As Boolean, ByVal SearchText As String) As Integer
      Return _RecordCount
    End Function
      'Select By ID One Record Filtered Overloaded GetByID
    <DataObjectMethod(DataObjectMethodType.Insert, True)> _
    Public Shared Function dmsMultiItemsInsert(ByVal Record As SIS.DMS.dmsMultiItems) As SIS.DMS.dmsMultiItems
      Dim _Rec As SIS.DMS.dmsMultiItems = SIS.DMS.dmsMultiItems.dmsMultiItemsGetNewRecord()
      With _Rec
        .ItemID = Record.ItemID
        .MultiTypeID = Record.MultiTypeID
        .MultiItemTypeID = Record.MultiItemTypeID
        .MultiItemID = Record.MultiItemID
        .MultiSequence = Record.MultiSequence
        .CreatedBy = Record.CreatedBy
        .CreatedOn = Record.CreatedOn
      End With
      Return SIS.DMS.dmsMultiItems.InsertData(_Rec)
    End Function
    Public Shared Function InsertData(ByVal Record As SIS.DMS.dmsMultiItems) As SIS.DMS.dmsMultiItems
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsMultiItemsInsert"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemID",SqlDbType.Int,11, Record.ItemID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MultiTypeID",SqlDbType.Int,11, Iif(Record.MultiTypeID= "" ,Convert.DBNull, Record.MultiTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MultiItemTypeID",SqlDbType.Int,11, Iif(Record.MultiItemTypeID= "" ,Convert.DBNull, Record.MultiItemTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MultiItemID",SqlDbType.Int,11, Iif(Record.MultiItemID= "" ,Convert.DBNull, Record.MultiItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MultiSequence",SqlDbType.Int,11, Record.MultiSequence)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedBy", SqlDbType.Int, 11, Record.CreatedBy)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedOn", SqlDbType.DateTime, 21, Record.CreatedOn)
          Cmd.Parameters.Add("@Return_SerialNo", SqlDbType.Int, 11)
          Cmd.Parameters("@Return_SerialNo").Direction = ParameterDirection.Output
          Con.Open()
          Cmd.ExecuteNonQuery()
          Record.SerialNo = Cmd.Parameters("@Return_SerialNo").Value
        End Using
      End Using
      Return Record
    End Function
    <DataObjectMethod(DataObjectMethodType.Update, True)> _
    Public Shared Function dmsMultiItemsUpdate(ByVal Record As SIS.DMS.dmsMultiItems) As SIS.DMS.dmsMultiItems
      Dim _Rec As SIS.DMS.dmsMultiItems = SIS.DMS.dmsMultiItems.dmsMultiItemsGetByID(Record.SerialNo)
      With _Rec
        .ItemID = Record.ItemID
        .MultiTypeID = Record.MultiTypeID
        .MultiItemTypeID = Record.MultiItemTypeID
        .MultiItemID = Record.MultiItemID
        .MultiSequence = Record.MultiSequence
        .CreatedBy = Record.CreatedBy
        .CreatedOn = Record.CreatedOn

      End With
      Return SIS.DMS.dmsMultiItems.UpdateData(_Rec)
    End Function
    Public Shared Function UpdateData(ByVal Record As SIS.DMS.dmsMultiItems) As SIS.DMS.dmsMultiItems
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsMultiItemsUpdate"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_SerialNo",SqlDbType.Int,11, Record.SerialNo)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemID",SqlDbType.Int,11, Record.ItemID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MultiTypeID",SqlDbType.Int,11, Iif(Record.MultiTypeID= "" ,Convert.DBNull, Record.MultiTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MultiItemTypeID",SqlDbType.Int,11, Iif(Record.MultiItemTypeID= "" ,Convert.DBNull, Record.MultiItemTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MultiItemID",SqlDbType.Int,11, Iif(Record.MultiItemID= "" ,Convert.DBNull, Record.MultiItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MultiSequence",SqlDbType.Int,11, Record.MultiSequence)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedBy", SqlDbType.Int, 11, Record.CreatedBy)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedOn", SqlDbType.DateTime, 21, Record.CreatedOn)
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
    Public Shared Function dmsMultiItemsDelete(ByVal Record As SIS.DMS.dmsMultiItems) As Int32
      Dim _Result as Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsMultiItemsDelete"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_SerialNo",SqlDbType.Int,Record.SerialNo.ToString.Length, Record.SerialNo)
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
