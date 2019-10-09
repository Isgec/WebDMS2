Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.Web.Script.Serialization
Namespace SIS.DMS

  <DataObject()>
  Partial Public Class dmsSearchData
    Public Shared Property SearchID As String = ""
    Public Shared Property LastItem As String = ""
    Public Shared Property ServerProcessing As Boolean = False
    Public Shared Property StopNow As Boolean
      Get
        Dim mRet As Boolean = False
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Con.Open()
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = "select isMultiForward from dms_items where itemid=" & SearchID
            mRet = Cmd.ExecuteScalar
          End Using
        End Using
        Return mRet
      End Get
      Set(value As Boolean)
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Con.Open()
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = "update dms_items set isMultiForward=" & IIf(value, "1", "0") & " where itemid=" & SearchID
            Cmd.ExecuteNonQuery()
          End Using
        End Using
      End Set
    End Property
    Public Shared Function GetData() As List(Of SIS.DMS.UI.apiItem)
      Dim mRet As New List(Of SIS.DMS.UI.apiItem)
      If LastItem = "" Then LastItem = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= "   select top 10 dms_items.*,  "
        Sql &= "   dms_states.Description as StatusID_Description, "
        Sql &= "   [DMS_SearchResults].[SerialNo] as SearchSerialNo "
        Sql &= "   from dms_items "
        Sql &= "   inner join dms_states on dms_states.statusid = dms_items.statusid "
        Sql &= "   INNER JOIN DMS_SearchResults on DMS_SearchResults.ItemID = DMS_Items.ItemID "
        Sql &= "   WHERE "
        Sql &= "   [DMS_SearchResults].[SearchID]=" & SearchID
        Sql &= "   AND [DMS_SearchResults].[MultiTypeID] = " & enumMultiTypes.Searched
        Sql &= "   AND [DMS_SearchResults].[SerialNo] > " & LastItem
        Sql &= "   Order By SerialNo DESC "
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            mRet.Add(New SIS.DMS.UI.apiItem(Reader))
          End While
          Reader.Close()
        End Using
      End Using
      Return mRet
    End Function
    Public Shared Sub DeleteLastResults(ByVal searchid As String)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "delete dms_searchResults where searchid=" & searchid
          Cmd.ExecuteNonQuery()
        End Using
      End Using
    End Sub

    Public Shared Sub TriggerSearch(ByVal searchid As String)
      ServerProcessing = True
      Dim sItem As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(searchid)
      Dim UserID As Integer = sItem.LinkedItemID
      Dim matchedItems As List(Of SIS.DMS.UI.apiItem) = SIS.DMS.UI.Core.GetItemsForUserAndType(UserID, enumItemTypes.Folder)
      For Each mItm As SIS.DMS.UI.apiItem In matchedItems
        If StopNow Then
          Exit For
        End If
        MatchingChild(mItm.ItemID, searchid, sItem.Description.ToUpper)
      Next
      ServerProcessing = False
    End Sub
    Private Shared Sub MatchingChild(ByVal pItemID As Integer, ByVal searchid As String, ByVal strSearch As String)
      Dim Items As List(Of SIS.DMS.UI.apiItem) = GetChildItemsMatched(pItemID, strSearch)
      For Each mItm As SIS.DMS.UI.apiItem In Items
        If StopNow Then
          Exit For
        End If
        InsertMatched(mItm, searchid, enumMultiTypes.Searched, False)
      Next
      'Get All Child Items and then search matching items:=>As Child Item may be matching, if NOT parent
      Items = SIS.DMS.UI.Core.GetChildItems(pItemID, 0)
      For Each mItm As SIS.DMS.UI.apiItem In Items
        If StopNow Then
          Exit For
        End If
        MatchingChild(mItm.ItemID, searchid, strSearch)
      Next
    End Sub
    Public Shared Sub InsertMatched(ByVal mItm As SIS.DMS.UI.apiItem, ByVal SearchID As Integer, Optional ByVal MultiType As enumMultiTypes = enumMultiTypes.Searched, Optional ByVal Selected As Boolean = False)
      Dim mSelected As String = "0"
      If Selected Then mSelected = "1"
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "insert into dms_searchResults (SearchID, ItemID, MultiTypeID, ItemTypeID, Selected) Values (" & SearchID & "," & mItm.ItemID & "," & MultiType & "," & enumItemTypes.Searches & "," & mSelected & ")"
          Cmd.ExecuteNonQuery()
        End Using
      End Using
    End Sub
    Public Shared Function GetChildItemsMatched(ByVal ItemID As Integer, ByVal strSearch As String) As List(Of SIS.DMS.UI.apiItem)
      Dim mRet As New List(Of SIS.DMS.UI.apiItem)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())

        Dim Sql As String = ""
        Sql &= "   select dms_items.*,  "
        Sql &= "   dms_states.Description as StatusID_Description "
        Sql &= "   from dms_items "
        Sql &= "   inner join dms_states on dms_states.statusid = dms_items.statusid "
        Sql &= " left outer join DMS_MultiItems on DMS_MultiItems.multiitemid=dms_items.itemid and DMS_MultiItems.MultiTypeID=" & enumMultiTypes.Child
        Sql &= " WHERE [DMS_Items].[ItemTypeID]=" & enumItemTypes.File
        Sql &= " AND (dms_items.ParentItemID =" & ItemID
        Sql &= " or dms_multiitems.ItemID=" & ItemID
        Sql &= "  ) "
        Sql &= "   AND ( "
        Sql &= " UPPER([DMS_Items].[Description]) LIKE '%" & strSearch & "%'"
        Sql &= " OR UPPER([DMS_Items].[FullDescription]) LIKE '% " & strSearch & "%'"
        Sql &= " OR UPPER([DMS_Items].[KeyWords]) LIKE '% " & strSearch & "%'"
        Sql &= " OR UPPER([DMS_Items].[ProjectID]) LIKE '%" & strSearch & "%'"
        Sql &= " OR UPPER([DMS_Items].[WBSID]) LIKE '%" & strSearch & "%'"
        Sql &= " OR UPPER([DMS_Items].[CompanyID]) LIKE '%" & strSearch & "%'"
        Sql &= " OR UPPER([DMS_Items].[DivisionID]) LIKE '%" & strSearch & "%'"
        Sql &= " OR UPPER([DMS_Items].[DepartmentID]) LIKE '%" & strSearch & "%'"
        Sql &= " OR UPPER([DMS_Items].[CreatedBy]) LIKE '%" & strSearch & "%'"
        Sql &= " OR UPPER([DMS_States].[Description]) LIKE '%" & strSearch & "%'"
        Sql &= " ) "
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            mRet.Add(New SIS.DMS.UI.apiItem(Reader))
          End While
          Reader.Close()
        End Using
      End Using
      Return mRet
    End Function


    'Public Shared Function GetChildItems(ByVal ItemID As Integer, ByVal strSearch As String) As List(Of SIS.DMS.dmsItems)
    '  Dim mRet As New List(Of SIS.DMS.dmsItems)
    '  Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
    '    Dim Sql As String = ""  'SIS.DMS.Handlers.dmsData.GetItemSelectStatement()
    '    Sql &= "     SELECT [DMS_Items].* , "
    '    Sql &= "     [aspnet_Users1].[UserFullName] AS aspnet_Users1_UserFullName, "
    '    Sql &= "     [aspnet_Users2].[UserFullName] AS aspnet_Users2_UserFullName, "
    '    Sql &= "     [DMS_Items3].[Description] AS DMS_Items3_Description, "
    '    Sql &= "     [DMS_Items4].[Description] AS DMS_Items4_Description, "
    '    Sql &= "     [DMS_Items5].[Description] AS DMS_Items5_Description, "
    '    Sql &= "     [DMS_Items6].[Description] AS DMS_Items6_Description, "
    '    Sql &= "     [DMS_Items7].[Description] AS DMS_Items7_Description, "
    '    Sql &= "     [DMS_ItemTypes8].[Description] AS DMS_ItemTypes8_Description, "
    '    Sql &= "     [DMS_ItemTypes9].[Description] AS DMS_ItemTypes9_Description, "
    '    Sql &= "     [DMS_ItemTypes10].[Description] AS DMS_ItemTypes10_Description, "
    '    Sql &= "     [DMS_ItemTypes11].[Description] AS DMS_ItemTypes11_Description, "
    '    Sql &= "     [DMS_States12].[Description] AS DMS_States12_Description, "
    '    Sql &= "     [DMS_States13].[Description] AS DMS_States13_Description, "
    '    Sql &= "     [HRM_Companies14].[Description] AS HRM_Companies14_Description, "
    '    Sql &= "     [HRM_Departments15].[Description] AS HRM_Departments15_Description, "
    '    Sql &= "     [HRM_Divisions16].[Description] AS HRM_Divisions16_Description, "
    '    Sql &= "     [IDM_Projects17].[Description] AS IDM_Projects17_Description, "
    '    Sql &= "     [IDM_WBS18].[Description] AS IDM_WBS18_Description, "
    '    Sql &= "     [aspnet_Users19].[UserFullName] AS aspnet_Users19_UserFullName,  "
    '    Sql &= " (case when [DMS_Items].[ItemID] IN ("
    '    Sql &= "     SELECT [DMS_Items].[ItemID] "
    '    Sql &= "   FROM [DMS_Items]  "
    '    Sql &= "   LEFT OUTER JOIN [aspnet_users] AS [aspnet_users1] "
    '    Sql &= "     ON [DMS_Items].[UserID] = [aspnet_users1].[LoginID] "
    '    Sql &= "   INNER JOIN [aspnet_users] AS [aspnet_users2] "
    '    Sql &= "     ON [DMS_Items].[CreatedBy] = [aspnet_users2].[LoginID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items3] "
    '    Sql &= "     ON [DMS_Items].[ForwardLinkedItemID] = [DMS_Items3].[ItemID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items4] "
    '    Sql &= "     ON [DMS_Items].[LinkedItemID] = [DMS_Items4].[ItemID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items5] "
    '    Sql &= "     ON [DMS_Items].[BackwardLinkedItemID] = [DMS_Items5].[ItemID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items6] "
    '    Sql &= "     ON [DMS_Items].[ParentItemID] = [DMS_Items6].[ItemID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items7] "
    '    Sql &= "     ON [DMS_Items].[ChildItemID] = [DMS_Items7].[ItemID] "
    '    Sql &= "   INNER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes8] "
    '    Sql &= "     ON [DMS_Items].[ItemTypeID] = [DMS_ItemTypes8].[ItemTypeID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes9] "
    '    Sql &= "     ON [DMS_Items].[BackwardLinkedItemTypeID] = [DMS_ItemTypes9].[ItemTypeID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes10] "
    '    Sql &= "     ON [DMS_Items].[LinkedItemTypeID] = [DMS_ItemTypes10].[ItemTypeID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes11] "
    '    Sql &= "     ON [DMS_Items].[ForwardLinkedItemTypeID] = [DMS_ItemTypes11].[ItemTypeID] "
    '    Sql &= "   INNER JOIN [DMS_States] AS [DMS_States12] "
    '    Sql &= "     ON [DMS_Items].[StatusID] = [DMS_States12].[StatusID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_States] AS [DMS_States13] "
    '    Sql &= "     ON [DMS_Items].[ConvertedStatusID] = [DMS_States13].[StatusID] "
    '    Sql &= "   LEFT OUTER JOIN [HRM_Companies] AS [HRM_Companies14] "
    '    Sql &= "     ON [DMS_Items].[CompanyID] = [HRM_Companies14].[CompanyID] "
    '    Sql &= "   LEFT OUTER JOIN [HRM_Departments] AS [HRM_Departments15] "
    '    Sql &= "     ON [DMS_Items].[DepartmentID] = [HRM_Departments15].[DepartmentID] "
    '    Sql &= "   LEFT OUTER JOIN [HRM_Divisions] AS [HRM_Divisions16] "
    '    Sql &= "     ON [DMS_Items].[DivisionID] = [HRM_Divisions16].[DivisionID] "
    '    Sql &= "   LEFT OUTER JOIN [IDM_Projects] AS [IDM_Projects17] "
    '    Sql &= "     ON [DMS_Items].[ProjectID] = [IDM_Projects17].[ProjectID] "
    '    Sql &= "   LEFT OUTER JOIN [IDM_WBS] AS [IDM_WBS18] "
    '    Sql &= "     ON [DMS_Items].[WBSID] = [IDM_WBS18].[WBSID] "
    '    Sql &= "   LEFT OUTER JOIN [aspnet_users] AS [aspnet_users19] "
    '    Sql &= "     ON [DMS_Items].[ActionBy] = [aspnet_users19].[LoginID] "
    '    Sql &= "   WHERE "
    '    Sql &= "   [DMS_Items].[ParentItemID]=" & ItemID
    '    Sql &= "   AND [DMS_Items].[ItemTypeID]=" & enumItemTypes.File
    '    Sql &= "   AND ( "
    '    Sql &= " UPPER([DMS_Items].[Description]) LIKE '% " & strSearch & "%'"
    '    Sql &= " OR UPPER([DMS_Items].[FullDescription]) LIKE '% " & strSearch & "%'"
    '    Sql &= " OR UPPER([DMS_Items].[ProjectID]) LIKE '% " & strSearch & "%'"
    '    Sql &= " OR UPPER([DMS_Items].[WBSID]) LIKE '% " & strSearch & "%'"
    '    Sql &= " OR UPPER([DMS_Items].[CompanyID]) LIKE '% " & strSearch & "%'"
    '    Sql &= " OR UPPER([DMS_Items].[DivisionID]) LIKE '% " & strSearch & "%'"
    '    Sql &= " OR UPPER([DMS_Items].[DepartmentID]) LIKE '% " & strSearch & "%'"
    '    Sql &= " OR UPPER([DMS_Items].[CreatedBy]) LIKE '% " & strSearch & "%'"
    '    Sql &= " OR UPPER([aspnet_Users2].[UserFullName]) LIKE '% " & strSearch & "%'"
    '    Sql &= " OR UPPER([HRM_Departments15].[Description]) LIKE '% " & strSearch & "%'"
    '    Sql &= " OR UPPER([HRM_Divisions16].[Description]) LIKE '% " & strSearch & "%'"
    '    Sql &= " OR UPPER([HRM_Companies14].[Description]) LIKE '% " & strSearch & "%'"
    '    Sql &= " OR UPPER([IDM_Projects17].[Description]) LIKE '% " & strSearch & "%'"
    '    Sql &= " OR UPPER([IDM_WBS18].[Description]) LIKE '% " & strSearch & "%'"
    '    Sql &= " OR UPPER([DMS_States12].[Description]) LIKE '% " & strSearch & "%'"
    '    Sql &= " ) ) then 1 else 1 end) as Matched "
    '    Sql &= "   FROM [DMS_Items]  "
    '    Sql &= "   LEFT OUTER JOIN [aspnet_users] AS [aspnet_users1] "
    '    Sql &= "     ON [DMS_Items].[UserID] = [aspnet_users1].[LoginID] "
    '    Sql &= "   INNER JOIN [aspnet_users] AS [aspnet_users2] "
    '    Sql &= "     ON [DMS_Items].[CreatedBy] = [aspnet_users2].[LoginID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items3] "
    '    Sql &= "     ON [DMS_Items].[ForwardLinkedItemID] = [DMS_Items3].[ItemID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items4] "
    '    Sql &= "     ON [DMS_Items].[LinkedItemID] = [DMS_Items4].[ItemID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items5] "
    '    Sql &= "     ON [DMS_Items].[BackwardLinkedItemID] = [DMS_Items5].[ItemID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items6] "
    '    Sql &= "     ON [DMS_Items].[ParentItemID] = [DMS_Items6].[ItemID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items7] "
    '    Sql &= "     ON [DMS_Items].[ChildItemID] = [DMS_Items7].[ItemID] "
    '    Sql &= "   INNER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes8] "
    '    Sql &= "     ON [DMS_Items].[ItemTypeID] = [DMS_ItemTypes8].[ItemTypeID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes9] "
    '    Sql &= "     ON [DMS_Items].[BackwardLinkedItemTypeID] = [DMS_ItemTypes9].[ItemTypeID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes10] "
    '    Sql &= "     ON [DMS_Items].[LinkedItemTypeID] = [DMS_ItemTypes10].[ItemTypeID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes11] "
    '    Sql &= "     ON [DMS_Items].[ForwardLinkedItemTypeID] = [DMS_ItemTypes11].[ItemTypeID] "
    '    Sql &= "   INNER JOIN [DMS_States] AS [DMS_States12] "
    '    Sql &= "     ON [DMS_Items].[StatusID] = [DMS_States12].[StatusID] "
    '    Sql &= "   LEFT OUTER JOIN [DMS_States] AS [DMS_States13] "
    '    Sql &= "     ON [DMS_Items].[ConvertedStatusID] = [DMS_States13].[StatusID] "
    '    Sql &= "   LEFT OUTER JOIN [HRM_Companies] AS [HRM_Companies14] "
    '    Sql &= "     ON [DMS_Items].[CompanyID] = [HRM_Companies14].[CompanyID] "
    '    Sql &= "   LEFT OUTER JOIN [HRM_Departments] AS [HRM_Departments15] "
    '    Sql &= "     ON [DMS_Items].[DepartmentID] = [HRM_Departments15].[DepartmentID] "
    '    Sql &= "   LEFT OUTER JOIN [HRM_Divisions] AS [HRM_Divisions16] "
    '    Sql &= "     ON [DMS_Items].[DivisionID] = [HRM_Divisions16].[DivisionID] "
    '    Sql &= "   LEFT OUTER JOIN [IDM_Projects] AS [IDM_Projects17] "
    '    Sql &= "     ON [DMS_Items].[ProjectID] = [IDM_Projects17].[ProjectID] "
    '    Sql &= "   LEFT OUTER JOIN [IDM_WBS] AS [IDM_WBS18] "
    '    Sql &= "     ON [DMS_Items].[WBSID] = [IDM_WBS18].[WBSID] "
    '    Sql &= "   LEFT OUTER JOIN [aspnet_users] AS [aspnet_users19] "
    '    Sql &= "     ON [DMS_Items].[ActionBy] = [aspnet_users19].[LoginID] "
    '    Sql &= "   WHERE "
    '    Sql &= "   [DMS_Items].[ParentItemID]=" & ItemID



    '    Using Cmd As SqlCommand = Con.CreateCommand()
    '      Cmd.CommandType = CommandType.Text
    '      Cmd.CommandText = Sql
    '      Con.Open()
    '      Dim Reader As SqlDataReader = Cmd.ExecuteReader()
    '      While (Reader.Read())
    '        mRet.Add(New SIS.DMS.dmsItems(Reader))
    '      End While
    '      Reader.Close()
    '    End Using
    '  End Using
    '  Return mRet
    'End Function
    'Public Shared Function GetLinkedAndAuthorizedItems(ByVal ForItemID As String, ByVal ItemTypeID As String, ByVal strSearch As String) As List(Of SIS.DMS.dmsItems)
    '  '======================================
    '  ' Linked and Authorised Items to User
    '  '======================================
    '  Dim Results As New List(Of SIS.DMS.dmsItems)
    '  Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
    '    Dim Sql As String = ""
    '    Sql &= SIS.DMS.Handlers.dmsData.GetItemSelectStatement()
    '    Sql &= "   LEFT OUTER JOIN DMS_MultiItems on DMS_MultiItems.MultiItemID = DMS_Items.ItemID "
    '    Sql &= "   WHERE "
    '    Sql &= "   [DMS_MultiItems].[MultiTypeID] IN (" & enumMultiTypes.Linked & "," & enumMultiTypes.Authorized & "," & enumMultiTypes.Created & ")"
    '    Sql &= "   AND [DMS_MultiItems].[ItemID]=" & ForItemID
    '    Sql &= "   ORDER BY [DMS_Items].[ItemID] "

    '    Using Cmd As SqlCommand = Con.CreateCommand()
    '      Cmd.CommandType = CommandType.Text
    '      Cmd.CommandText = Sql
    '      Con.Open()
    '      Dim Reader As SqlDataReader = Cmd.ExecuteReader()
    '      While (Reader.Read())
    '        Results.Add(New SIS.DMS.dmsItems(Reader))
    '      End While
    '      Reader.Close()
    '    End Using
    '  End Using
    '  Return Results
    'End Function

  End Class
End Namespace
