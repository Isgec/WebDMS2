Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.DMS
  <DataObject()>
  Partial Public Class dmsJsonHistory
    Private Shared _RecordCount As Integer
    Public Property HistoryID As Int32 = 0
    Private _ItemID As Int32 = 0
    Private _InheritFromParent As Boolean = False
    Private _UserID As String = ""
    Private _Description As String = ""
    Private _RevisionNo As String = ""
    Private _ItemTypeID As Int32 = 0
    Private _StatusID As Int32 = 0
    Private _CreatedBy As String = ""
    Private _CreatedOn As String = ""
    Private _MaintainAllLog As Boolean = False
    Private _BackwardLinkedItemID As String = ""
    Private _MaintainVersions As Boolean = False
    Private _MaintainStatusLog As Boolean = False
    Private _LinkedItemID As String = ""
    Private _LinkedItemTypeID As String = ""
    Private _BackwardLinkedItemTypeID As String = ""
    Private _IsMultiBackward As Boolean = False
    Private _IsgecVaultID As String = ""
    Public Property DeleteFile As Integer = 1
    Public Property CreateFile As Integer = 1
    Public Property BrowseList As Integer = 1
    Public Property GrantAuthorization As Integer = 1
    Public Property CreateFolder As Integer = 1
    Public Property Publish As Integer = 1
    Public Property DeleteFolder As Integer = 1
    Public Property RenameFolder As Integer = 1
    Public Property ShowInList As Integer = 1
    Private _CompanyID As String = ""
    Private _ChildItemID As String = ""
    Private _DepartmentID As String = ""
    Private _DivisionID As String = ""
    Private _IsMultiParent As Boolean = False
    Private _ConvertedStatusID As String = ""
    Private _IsMultiChild As Boolean = False
    Private _ParentItemID As String = ""
    Private _ProjectID As String = ""
    Private _ForwardLinkedItemTypeID As String = ""
    Private _IsMultiForward As Boolean = False
    Private _IsMultiLinked As Boolean = False
    Private _ForwardLinkedItemID As String = ""
    Private _KeyWords As String = ""
    Private _WBSID As String = ""
    Private _FullDescription As String = ""
    Private _EMailID As String = ""
    Private _SearchInParent As Boolean = False
    Private _Approved As Boolean = False
    Private _Rejected As Boolean = False
    Private _ActionRemarks As String = ""
    Private _ActionBy As String = ""
    Private _ActionOn As String = ""
    Private _IsError As String = ""
    Private _ErrorMessage As String = ""
    Private _aspnet_Users1_UserFullName As String = ""
    Private _aspnet_Users2_UserFullName As String = ""
    Private _DMS_Items3_Description As String = ""
    Private _DMS_Items4_Description As String = ""
    Private _DMS_Items5_Description As String = ""
    Private _DMS_Items6_Description As String = ""
    Private _DMS_Items7_Description As String = ""
    Private _DMS_ItemTypes8_Description As String = ""
    Private _DMS_ItemTypes9_Description As String = ""
    Private _DMS_ItemTypes10_Description As String = ""
    Private _DMS_ItemTypes11_Description As String = ""
    Private _DMS_States12_Description As String = ""
    Private _DMS_States13_Description As String = ""
    Private _HRM_Companies14_Description As String = ""
    Private _HRM_Departments15_Description As String = ""
    Private _HRM_Divisions16_Description As String = ""
    Private _IDM_Projects17_Description As String = ""
    Private _IDM_WBS18_Description As String = ""
    Private _aspnet_Users19_UserFullName As String = ""
    Public Property ItemID() As Int32
      Get
        Return _ItemID
      End Get
      Set(ByVal value As Int32)
        _ItemID = value
      End Set
    End Property
    Public Property InheritFromParent() As Boolean
      Get
        Return _InheritFromParent
      End Get
      Set(ByVal value As Boolean)
        _InheritFromParent = value
      End Set
    End Property
    Public Property UserID() As String
      Get
        Return _UserID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _UserID = ""
        Else
          _UserID = value
        End If
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
    Public Property RevisionNo() As String
      Get
        Return _RevisionNo
      End Get
      Set(ByVal value As String)
        _RevisionNo = value
      End Set
    End Property
    Public Property ItemTypeID() As Int32
      Get
        Return _ItemTypeID
      End Get
      Set(ByVal value As Int32)
        _ItemTypeID = value
      End Set
    End Property
    Public Property StatusID() As Int32
      Get
        Return _StatusID
      End Get
      Set(ByVal value As Int32)
        _StatusID = value
      End Set
    End Property
    Public Property CreatedBy() As String
      Get
        Return _CreatedBy
      End Get
      Set(ByVal value As String)
        _CreatedBy = value
      End Set
    End Property
    Public Property CreatedOn() As String
      Get
        If Not _CreatedOn = String.Empty Then
          Return Convert.ToDateTime(_CreatedOn).ToString("dd/MM/yyyy HH:mm")
        End If
        Return _CreatedOn
      End Get
      Set(ByVal value As String)
        _CreatedOn = value
      End Set
    End Property
    Public Property MaintainAllLog() As Boolean
      Get
        Return _MaintainAllLog
      End Get
      Set(ByVal value As Boolean)
        _MaintainAllLog = value
      End Set
    End Property
    Public Property BackwardLinkedItemID() As String
      Get
        Return _BackwardLinkedItemID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _BackwardLinkedItemID = ""
        Else
          _BackwardLinkedItemID = value
        End If
      End Set
    End Property
    Public Property MaintainVersions() As Boolean
      Get
        Return _MaintainVersions
      End Get
      Set(ByVal value As Boolean)
        _MaintainVersions = value
      End Set
    End Property
    Public Property MaintainStatusLog() As Boolean
      Get
        Return _MaintainStatusLog
      End Get
      Set(ByVal value As Boolean)
        _MaintainStatusLog = value
      End Set
    End Property
    Public Property LinkedItemID() As String
      Get
        Return _LinkedItemID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _LinkedItemID = ""
        Else
          _LinkedItemID = value
        End If
      End Set
    End Property
    Public Property LinkedItemTypeID() As String
      Get
        Return _LinkedItemTypeID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _LinkedItemTypeID = ""
        Else
          _LinkedItemTypeID = value
        End If
      End Set
    End Property
    Public Property BackwardLinkedItemTypeID() As String
      Get
        Return _BackwardLinkedItemTypeID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _BackwardLinkedItemTypeID = ""
        Else
          _BackwardLinkedItemTypeID = value
        End If
      End Set
    End Property
    Public Property IsMultiBackward() As Boolean
      Get
        Return _IsMultiBackward
      End Get
      Set(ByVal value As Boolean)
        _IsMultiBackward = value
      End Set
    End Property
    Public Property IsgecVaultID() As String
      Get
        Return _IsgecVaultID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _IsgecVaultID = ""
        Else
          _IsgecVaultID = value
        End If
      End Set
    End Property
    Public Property CompanyID() As String
      Get
        Return _CompanyID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _CompanyID = ""
        Else
          _CompanyID = value
        End If
      End Set
    End Property
    Public Property ChildItemID() As String
      Get
        Return _ChildItemID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _ChildItemID = ""
        Else
          _ChildItemID = value
        End If
      End Set
    End Property
    Public Property DepartmentID() As String
      Get
        Return _DepartmentID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _DepartmentID = ""
        Else
          _DepartmentID = value
        End If
      End Set
    End Property
    Public Property DivisionID() As String
      Get
        Return _DivisionID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _DivisionID = ""
        Else
          _DivisionID = value
        End If
      End Set
    End Property
    Public Property IsMultiParent() As Boolean
      Get
        Return _IsMultiParent
      End Get
      Set(ByVal value As Boolean)
        _IsMultiParent = value
      End Set
    End Property
    Public Property ConvertedStatusID() As String
      Get
        Return _ConvertedStatusID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _ConvertedStatusID = ""
        Else
          _ConvertedStatusID = value
        End If
      End Set
    End Property
    Public Property IsMultiChild() As Boolean
      Get
        Return _IsMultiChild
      End Get
      Set(ByVal value As Boolean)
        _IsMultiChild = value
      End Set
    End Property
    Public Property ParentItemID() As String
      Get
        Return _ParentItemID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _ParentItemID = ""
        Else
          _ParentItemID = value
        End If
      End Set
    End Property
    Public Property ProjectID() As String
      Get
        Return _ProjectID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _ProjectID = ""
        Else
          _ProjectID = value
        End If
      End Set
    End Property
    Public Property ForwardLinkedItemTypeID() As String
      Get
        Return _ForwardLinkedItemTypeID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _ForwardLinkedItemTypeID = ""
        Else
          _ForwardLinkedItemTypeID = value
        End If
      End Set
    End Property
    Public Property IsMultiForward() As Boolean
      Get
        Return _IsMultiForward
      End Get
      Set(ByVal value As Boolean)
        _IsMultiForward = value
      End Set
    End Property
    Public Property IsMultiLinked() As Boolean
      Get
        Return _IsMultiLinked
      End Get
      Set(ByVal value As Boolean)
        _IsMultiLinked = value
      End Set
    End Property
    Public Property ForwardLinkedItemID() As String
      Get
        Return _ForwardLinkedItemID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _ForwardLinkedItemID = ""
        Else
          _ForwardLinkedItemID = value
        End If
      End Set
    End Property
    Public Property KeyWords() As String
      Get
        Return _KeyWords
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _KeyWords = ""
        Else
          _KeyWords = value
        End If
      End Set
    End Property
    Public Property WBSID() As String
      Get
        Return _WBSID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _WBSID = ""
        Else
          _WBSID = value
        End If
      End Set
    End Property
    Public Property FullDescription() As String
      Get
        Return _FullDescription
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _FullDescription = ""
        Else
          _FullDescription = value
        End If
      End Set
    End Property
    Public Property EMailID() As String
      Get
        Return _EMailID
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _EMailID = ""
        Else
          _EMailID = value
        End If
      End Set
    End Property
    Public Property SearchInParent() As Boolean
      Get
        Return _SearchInParent
      End Get
      Set(ByVal value As Boolean)
        _SearchInParent = value
      End Set
    End Property
    Public Property Approved() As Boolean
      Get
        Return _Approved
      End Get
      Set(ByVal value As Boolean)
        _Approved = value
      End Set
    End Property
    Public Property Rejected() As Boolean
      Get
        Return _Rejected
      End Get
      Set(ByVal value As Boolean)
        _Rejected = value
      End Set
    End Property
    Public Property ActionRemarks() As String
      Get
        Return _ActionRemarks
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _ActionRemarks = ""
        Else
          _ActionRemarks = value
        End If
      End Set
    End Property
    Public Property ActionBy() As String
      Get
        Return _ActionBy
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _ActionBy = ""
        Else
          _ActionBy = value
        End If
      End Set
    End Property
    Public Property ActionOn() As String
      Get
        If Not _ActionOn = String.Empty Then
          Return Convert.ToDateTime(_ActionOn).ToString("dd/MM/yyyy HH:mm")
        End If
        Return _ActionOn
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _ActionOn = ""
        Else
          _ActionOn = value
        End If
      End Set
    End Property
    Public Property IsError() As String
      Get
        Return _IsError
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _IsError = ""
        Else
          _IsError = value
        End If
      End Set
    End Property
    Public Property ErrorMessage() As String
      Get
        Return _ErrorMessage
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _ErrorMessage = ""
        Else
          _ErrorMessage = value
        End If
      End Set
    End Property
    Public Property aspnet_Users1_UserFullName() As String
      Get
        Return _aspnet_Users1_UserFullName
      End Get
      Set(ByVal value As String)
        _aspnet_Users1_UserFullName = value
      End Set
    End Property
    Public Property aspnet_Users2_UserFullName() As String
      Get
        Return _aspnet_Users2_UserFullName
      End Get
      Set(ByVal value As String)
        _aspnet_Users2_UserFullName = value
      End Set
    End Property
    Public Property DMS_Items3_Description() As String
      Get
        Return _DMS_Items3_Description
      End Get
      Set(ByVal value As String)
        _DMS_Items3_Description = value
      End Set
    End Property
    Public Property DMS_Items4_Description() As String
      Get
        Return _DMS_Items4_Description
      End Get
      Set(ByVal value As String)
        _DMS_Items4_Description = value
      End Set
    End Property
    Public Property DMS_Items5_Description() As String
      Get
        Return _DMS_Items5_Description
      End Get
      Set(ByVal value As String)
        _DMS_Items5_Description = value
      End Set
    End Property
    Public Property DMS_Items6_Description() As String
      Get
        Return _DMS_Items6_Description
      End Get
      Set(ByVal value As String)
        _DMS_Items6_Description = value
      End Set
    End Property
    Public Property DMS_Items7_Description() As String
      Get
        Return _DMS_Items7_Description
      End Get
      Set(ByVal value As String)
        _DMS_Items7_Description = value
      End Set
    End Property
    Public Property DMS_ItemTypes8_Description() As String
      Get
        Return _DMS_ItemTypes8_Description
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _DMS_ItemTypes8_Description = ""
        Else
          _DMS_ItemTypes8_Description = value
        End If
      End Set
    End Property
    Public Property DMS_ItemTypes9_Description() As String
      Get
        Return _DMS_ItemTypes9_Description
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _DMS_ItemTypes9_Description = ""
        Else
          _DMS_ItemTypes9_Description = value
        End If
      End Set
    End Property
    Public Property DMS_ItemTypes10_Description() As String
      Get
        Return _DMS_ItemTypes10_Description
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _DMS_ItemTypes10_Description = ""
        Else
          _DMS_ItemTypes10_Description = value
        End If
      End Set
    End Property
    Public Property DMS_ItemTypes11_Description() As String
      Get
        Return _DMS_ItemTypes11_Description
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _DMS_ItemTypes11_Description = ""
        Else
          _DMS_ItemTypes11_Description = value
        End If
      End Set
    End Property
    Public Property DMS_States12_Description() As String
      Get
        Return _DMS_States12_Description
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _DMS_States12_Description = ""
        Else
          _DMS_States12_Description = value
        End If
      End Set
    End Property
    Public Property DMS_States13_Description() As String
      Get
        Return _DMS_States13_Description
      End Get
      Set(ByVal value As String)
        If Convert.IsDBNull(value) Then
          _DMS_States13_Description = ""
        Else
          _DMS_States13_Description = value
        End If
      End Set
    End Property
    Public Property HRM_Companies14_Description() As String
      Get
        Return _HRM_Companies14_Description
      End Get
      Set(ByVal value As String)
        _HRM_Companies14_Description = value
      End Set
    End Property
    Public Property HRM_Departments15_Description() As String
      Get
        Return _HRM_Departments15_Description
      End Get
      Set(ByVal value As String)
        _HRM_Departments15_Description = value
      End Set
    End Property
    Public Property HRM_Divisions16_Description() As String
      Get
        Return _HRM_Divisions16_Description
      End Get
      Set(ByVal value As String)
        _HRM_Divisions16_Description = value
      End Set
    End Property
    Public Property IDM_Projects17_Description() As String
      Get
        Return _IDM_Projects17_Description
      End Get
      Set(ByVal value As String)
        _IDM_Projects17_Description = value
      End Set
    End Property
    Public Property IDM_WBS18_Description() As String
      Get
        Return _IDM_WBS18_Description
      End Get
      Set(ByVal value As String)
        _IDM_WBS18_Description = value
      End Set
    End Property
    Public Property aspnet_Users19_UserFullName() As String
      Get
        Return _aspnet_Users19_UserFullName
      End Get
      Set(ByVal value As String)
        _aspnet_Users19_UserFullName = value
      End Set
    End Property
    Public ReadOnly Property DisplayField() As String
      Get
        Return "" & _Description.ToString.PadRight(250, " ")
      End Get
    End Property
    Public ReadOnly Property PrimaryKey() As String
      Get
        Return HistoryID & "|" & _ItemID
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
    Public Class PKdmsHistory
      Private _ItemID As Int32 = 0
      Public Property HistoryID As Int32 = 0
      Public Property ItemID() As Int32
        Get
          Return _ItemID
        End Get
        Set(ByVal value As Int32)
          _ItemID = value
        End Set
      End Property
    End Class
    <DataObjectMethod(DataObjectMethodType.Select)>
    Public Shared Function dmsHistorySelectList(ByVal ItemID As String) As List(Of SIS.DMS.dmsJsonHistory)
      Dim Results As List(Of SIS.DMS.dmsJsonHistory) = Nothing
      Dim Sql As String = ""
      Sql &= " SELECT "
      Sql &= GetSelectFields()
      Sql &= " WHERE dms_history.ItemID=" & ItemID
      Sql &= " Order by HistoryID DESC "
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          _RecordCount = -1
          Results = New List(Of SIS.DMS.dmsJsonHistory)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.dmsJsonHistory(Reader))
          End While
          Reader.Close()
          _RecordCount = Results.Count
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function GetSelectFields() As String
      Dim Sql As String = ""
      Sql &= "     [DMS_History].* , "
      Sql &= "     [aspnet_Users1].[UserFullName] AS aspnet_Users1_UserFullName, "
      Sql &= "     [aspnet_Users2].[UserFullName] AS aspnet_Users2_UserFullName, "
      Sql &= "     [DMS_History3].[Description] AS DMS_History3_Description, "
      Sql &= "     [DMS_History4].[Description] AS DMS_History4_Description, "
      Sql &= "     [DMS_History5].[Description] AS DMS_History5_Description, "
      Sql &= "     [DMS_History6].[Description] AS DMS_History6_Description, "
      Sql &= "     [DMS_History7].[Description] AS DMS_History7_Description, "
      Sql &= "     [DMS_ItemTypes8].[Description] AS DMS_ItemTypes8_Description, "
      Sql &= "     [DMS_ItemTypes9].[Description] AS DMS_ItemTypes9_Description, "
      Sql &= "     [DMS_ItemTypes10].[Description] AS DMS_ItemTypes10_Description, "
      Sql &= "     [DMS_ItemTypes11].[Description] AS DMS_ItemTypes11_Description, "
      Sql &= "     [DMS_States12].[Description] AS DMS_States12_Description, "
      Sql &= "     [DMS_States13].[Description] AS DMS_States13_Description, "
      Sql &= "     [HRM_Companies14].[Description] AS HRM_Companies14_Description, "
      Sql &= "     [HRM_Departments15].[Description] AS HRM_Departments15_Description, "
      Sql &= "     [HRM_Divisions16].[Description] AS HRM_Divisions16_Description, "
      Sql &= "     [IDM_Projects17].[Description] AS IDM_Projects17_Description, "
      Sql &= "     [IDM_WBS18].[Description] AS IDM_WBS18_Description, "
      Sql &= "     [aspnet_Users19].[UserFullName] AS aspnet_Users19_UserFullName  "
      Sql &= "   FROM [DMS_History]  "
      Sql &= "   LEFT OUTER JOIN [aspnet_users] AS [aspnet_users1] "
      Sql &= "     ON [DMS_History].[UserID] = [aspnet_users1].[LoginID] "
      Sql &= "   LEFT OUTER JOIN [aspnet_users] AS [aspnet_users2] "
      Sql &= "     ON [DMS_History].[CreatedBy] = [aspnet_users2].[LoginID] "
      Sql &= "   LEFT OUTER JOIN [DMS_History] AS [DMS_History3] "
      Sql &= "     ON [DMS_History].[ForwardLinkedItemID] = [DMS_History3].[ItemID] "
      Sql &= "   LEFT OUTER JOIN [DMS_History] AS [DMS_History4] "
      Sql &= "     ON [DMS_History].[LinkedItemID] = [DMS_History4].[ItemID] "
      Sql &= "   LEFT OUTER JOIN [DMS_History] AS [DMS_History5] "
      Sql &= "     ON [DMS_History].[BackwardLinkedItemID] = [DMS_History5].[ItemID] "
      Sql &= "   LEFT OUTER JOIN [DMS_History] AS [DMS_History6] "
      Sql &= "     ON [DMS_History].[ParentItemID] = [DMS_History6].[ItemID] "
      Sql &= "   LEFT OUTER JOIN [DMS_History] AS [DMS_History7] "
      Sql &= "     ON [DMS_History].[ChildItemID] = [DMS_History7].[ItemID] "
      Sql &= "   INNER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes8] "
      Sql &= "     ON [DMS_History].[ItemTypeID] = [DMS_ItemTypes8].[ItemTypeID] "
      Sql &= "   LEFT OUTER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes9] "
      Sql &= "     ON [DMS_History].[BackwardLinkedItemTypeID] = [DMS_ItemTypes9].[ItemTypeID] "
      Sql &= "   LEFT OUTER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes10] "
      Sql &= "     ON [DMS_History].[LinkedItemTypeID] = [DMS_ItemTypes10].[ItemTypeID] "
      Sql &= "   LEFT OUTER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes11] "
      Sql &= "     ON [DMS_History].[ForwardLinkedItemTypeID] = [DMS_ItemTypes11].[ItemTypeID] "
      Sql &= "   INNER JOIN [DMS_States] AS [DMS_States12] "
      Sql &= "     ON [DMS_History].[StatusID] = [DMS_States12].[StatusID] "
      Sql &= "   LEFT OUTER JOIN [DMS_States] AS [DMS_States13] "
      Sql &= "     ON [DMS_History].[ConvertedStatusID] = [DMS_States13].[StatusID] "
      Sql &= "   LEFT OUTER JOIN [HRM_Companies] AS [HRM_Companies14] "
      Sql &= "     ON [DMS_History].[CompanyID] = [HRM_Companies14].[CompanyID] "
      Sql &= "   LEFT OUTER JOIN [HRM_Departments] AS [HRM_Departments15] "
      Sql &= "     ON [DMS_History].[DepartmentID] = [HRM_Departments15].[DepartmentID] "
      Sql &= "   LEFT OUTER JOIN [HRM_Divisions] AS [HRM_Divisions16] "
      Sql &= "     ON [DMS_History].[DivisionID] = [HRM_Divisions16].[DivisionID] "
      Sql &= "   LEFT OUTER JOIN [IDM_Projects] AS [IDM_Projects17] "
      Sql &= "     ON [DMS_History].[ProjectID] = [IDM_Projects17].[ProjectID] "
      Sql &= "   LEFT OUTER JOIN [IDM_WBS] AS [IDM_WBS18] "
      Sql &= "     ON [DMS_History].[WBSID] = [IDM_WBS18].[WBSID] "
      Sql &= "   LEFT OUTER JOIN [aspnet_users] AS [aspnet_users19] "
      Sql &= "     ON [DMS_History].[ActionBy] = [aspnet_users19].[LoginID] "
      Return Sql
    End Function

    <DataObjectMethod(DataObjectMethodType.Select)>
    Public Shared Function dmsHistoryGetNewRecord() As SIS.DMS.dmsHistory
      Return New SIS.DMS.dmsHistory()
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)>
    Public Shared Function dmsHistoryGetByID(ByVal ItemID As Int32) As SIS.DMS.dmsHistory
      Dim Results As SIS.DMS.dmsHistory = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsHistorySelectByID"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemID", SqlDbType.Int, ItemID.ToString.Length, ItemID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 9, "")
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.DMS.dmsHistory(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)>
    Public Shared Function dmsHistorySelectList(ByVal StartRowIndex As Integer, ByVal MaximumRows As Integer, ByVal OrderBy As String, ByVal SearchState As Boolean, ByVal SearchText As String) As List(Of SIS.DMS.dmsHistory)
      Dim Results As List(Of SIS.DMS.dmsHistory) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          If SearchState Then
            Cmd.CommandText = "spdmsHistorySelectListSearch"
            SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@KeyWord", SqlDbType.NVarChar, 250, SearchText)
          Else
            Cmd.CommandText = "spdmsHistorySelectListFilteres"
          End If
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StartRowIndex", SqlDbType.Int, -1, StartRowIndex)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaximumRows", SqlDbType.Int, -1, MaximumRows)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 9, HttpContext.Current.Session("LoginID"))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OrderBy", SqlDbType.NVarChar, 50, OrderBy)
          Cmd.Parameters.Add("@RecordCount", SqlDbType.Int)
          Cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Results = New List(Of SIS.DMS.dmsHistory)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.dmsHistory(Reader))
          End While
          Reader.Close()
          _RecordCount = Cmd.Parameters("@RecordCount").Value
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function dmsHistorySelectCount(ByVal SearchState As Boolean, ByVal SearchText As String) As Integer
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
