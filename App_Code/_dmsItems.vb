Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.Web.Script.Serialization
Namespace SIS.DMS
  <DataObject()>
  Partial Public Class dmsItems
#Region " GetICON "
    Public ReadOnly Property GetIconHTML() As String
      Get
        Dim ret As String = ""
        If (ItemTypeID = enumItemTypes.File Or ItemTypeID = enumItemTypes.FileGroup) Then
          Dim ext As String = IO.Path.GetExtension(Description).ToUpper
          Select Case ext.Replace(".", "")
            Case "DOC", "DOCX"
              ret = "<i class='far fa-file-word' style='color:SteelBlue;'></i>"
            Case "XLS", "XLSX", "XLSM", "CSV"
              ret = "<i class='far fa-file-excel' style='color:LimeGreen;'></i>"
            Case "PPT", "PPTX"
              ret = "<i class='far fa-file-powerpoint' style='color:Crimson;'></i>"
            Case "JPG", "JPEG", "PNG", "BMP", "GIF"
              ret = "<i class='far fa-file-image' style='color:Plum;'></i>"
            Case "MP4", "AVI", "MPG", "MPEG", "MKA"
              ret = "<i class='far fa-file-video' style='color:Magenta;'></i>"
            Case "MP3"
              ret = "<i class='far fa-file-audio' style='color:LightCoral;'></i>"
            Case "TXT"
              ret = "<i class='far fa-file-alt' style='color:Teal;'></i>"
            Case "DWG"
              ret = "<i class='fas fa-file-code' style='color:PaleVioletRed;'></i>"
            Case "PDF"
              ret = "<i class='far fa-file-pdf' style='color:red;'></i>"
            Case "ZIP", "RAR"
              ret = "<i class='far fa-file-archive' style='color:gold;'></i>"
            Case Else
              ret = "<i class='far fa-file' style='color:gray;'></i>"
          End Select
        Else
          Select Case ItemTypeID
            Case enumItemTypes.Folder
              ret = "<i class='far fa-folder' style='color:Gold;'></i>"
            Case enumItemTypes.FolderGroup
              ret = "<i class='fas fa-folder' style='color:orange;'></i>"
            Case enumItemTypes.File
              ret = "<i class='far fa-file' style='color:green;'></i>"
            Case enumItemTypes.FileGroup
              ret = "<i class='fas fa-file' style='color:green;'></i>"
            Case enumItemTypes.User
              ret = "<i class='fas fa-user-tie' style='color:DarkMagenta;'></i>"
            Case enumItemTypes.UserGroup
              ret = "<i class='fas fa-group' style='color:Chartreuse;'></i>"
            Case enumItemTypes.Tag
              ret = "<i class='fas fa-tags' style='color:blue;'></i>"
            Case enumItemTypes.Link
              ret = "<i class='fas fa-link' style='color:darkgray;'></i>"
            Case enumItemTypes.UValue
              ret = "<i class='fab fa-playstation' style='color:Crimson;'></i>"
            Case enumItemTypes.Workflow
              ret = "<i class='fas fa-network-wired' style='color:BlueViolet;'></i>"
            Case enumItemTypes.Authorization
              ret = "<i class='fas fa-user-tag' style='color:DeepPink;'></i>"
            Case enumItemTypes.GrantedToMe
              ret = "<i class='fas fa-highlighter' style='color:DarkTurquoise;'></i>"
            Case enumItemTypes.UnderApprovalToMe
              ret = "<i class='fas fa-envelope-open-text' style='color:DarkOrange;'></i>"
            Case enumItemTypes.ApprovedByMe
              ret = "<i class='fas fa-pen-nib' style='color:FireBrick;'></i>"
          End Select
        End If
        Return ret
      End Get
    End Property
#End Region

#Region "  Properties "
    Private Shared _RecordCount As Integer
    Public Property SerialNo As String = ""
    Public Property ItemID As Int32 = 0
    Public Property InheritFromParent As Boolean = False
    Public Property UserID As String = ""
    Public Property Description As String = ""
    Public Property RevisionNo As String = ""
    Public Property ItemTypeID As Int32 = 0
    Public Property StatusID As Int32 = 0
    Public Property CreatedBy As String = ""
    Private _CreatedOn As String = ""
    Public Property MaintainAllLog As Boolean = False
    Public Property BackwardLinkedItemID As String = ""
    Public Property MaintainVersions As Boolean = False
    Public Property MaintainStatusLog As Boolean = False
    Public Property LinkedItemID As String = ""
    Public Property LinkedItemTypeID As String = ""
    Public Property BackwardLinkedItemTypeID As String = ""
    Public Property IsMultiBackward As Boolean = False
    Public Property IsgecVaultID As String = ""
    Public Property DeleteFile As Integer = 1
    Public Property CreateFile As Integer = 1
    Public Property BrowseList As Integer = 1
    Public Property GrantAuthorization As Integer = 1
    Public Property CreateFolder As Integer = 1
    Public Property Publish As Integer = 1
    Public Property DeleteFolder As Integer = 1
    Public Property RenameFolder As Integer = 1
    Public Property ShowInList As Integer = 1
    Public Property CompanyID As String = ""
    Public Property ChildItemID As String = ""
    Public Property DepartmentID As String = ""
    Public Property DivisionID As String = ""
    Public Property IsMultiParent As Boolean = False
    Public Property ConvertedStatusID As String = ""
    Public Property IsMultiChild As Boolean = False
    Public Property ParentItemID As String = ""
    Public Property ProjectID As String = ""
    Public Property ForwardLinkedItemTypeID As String = ""
    Public Property IsMultiForward As Boolean = False
    Public Property IsMultiLinked As Boolean = False
    Public Property ForwardLinkedItemID As String = ""
    Public Property KeyWords As String = ""
    Public Property WBSID As String = ""
    Public Property FullDescription As String = ""
    Public Property EMailID As String = ""
    Public Property SearchInParent As Boolean = False
    Public Property Approved As Boolean = False
    Public Property Rejected As Boolean = False
    Public Property ActionRemarks As String = ""
    Public Property ActionBy As String = ""
    Private _ActionOn As String = ""
    Public Property IsError As String = ""
    Public Property ErrorMessage As String = ""
    Public Property aspnet_Users1_UserFullName As String = ""
    Public Property aspnet_Users2_UserFullName As String = ""
    Public Property DMS_Items3_Description As String = ""
    Public Property DMS_Items4_Description As String = ""
    Public Property DMS_Items5_Description As String = ""
    Public Property DMS_Items6_Description As String = ""
    Public Property DMS_Items7_Description As String = ""
    Public Property DMS_ItemTypes8_Description As String = ""
    Public Property DMS_ItemTypes9_Description As String = ""
    Public Property DMS_ItemTypes10_Description As String = ""
    Public Property DMS_ItemTypes11_Description As String = ""
    Public Property DMS_States12_Description As String = ""
    Public Property DMS_States13_Description As String = ""
    Public Property HRM_Companies14_Description As String = ""
    Public Property HRM_Departments15_Description As String = ""
    Public Property HRM_Divisions16_Description As String = ""
    Public Property IDM_Projects17_Description As String = ""
    Public Property IDM_WBS18_Description As String = ""
    Public Property aspnet_Users19_UserFullName As String = ""
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
    Public Property ActionOn() As String
      Get
        If Not _ActionOn = String.Empty Then
          Return Convert.ToDateTime(_ActionOn).ToString("dd/MM/yyyy")
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
    Public ReadOnly Property DisplayField() As String
      Get
        Return "" & _Description.ToString.PadRight(250, " ")
      End Get
    End Property
    Public ReadOnly Property PrimaryKey() As String
      Get
        Return _ItemID
      End Get
    End Property
    Public Class PKdmsItems
      Private _ItemID As Int32 = 0
      Public Property ItemID() As Int32
        Get
          Return _ItemID
        End Get
        Set(ByVal value As Int32)
          _ItemID = value
        End Set
      End Property
    End Class

#End Region

#Region " Additional Properties "
    Public Property AthProcess As String = "DMS_UPLOAD"
    Public Property AthHandle As String = "J_DMSFILES"
    Public ReadOnly Property AthIndex As String
      Get
        Return ItemID & "_" & RevisionNo
      End Get
    End Property
    Public Property Matched As Boolean = False
    Public Property dmsBase As String = ""
    Public Property dmsItem As String = ""
    Public Property dmsType As String = ""
    Public Property dmsID As String = ""
    Public Property dmsLoaded As String = "0"
    Public Property dmsExpended As String = "0"
    Public Property dmsBottom As String = "0"
    Public Property dmsIndent As String = "0"
    Public Property byParent As Boolean = False

#End Region
#Region " Referenced Objects "

    Private _FK_DMS_Items_UserID As SIS.QCM.qcmUsers = Nothing
    Private _FK_DMS_Items_CreatedBy As SIS.QCM.qcmUsers = Nothing
    Private _FK_DMS_Items_ForwardLinkedItemID As SIS.DMS.dmsItems = Nothing
    Private _FK_DMS_Items_LinkedItemID As SIS.DMS.dmsItems = Nothing
    Private _FK_DMS_Items_BackwardLinkedItemID As SIS.DMS.dmsItems = Nothing
    Private _FK_DMS_Items_ParentItemID As SIS.DMS.dmsItems = Nothing
    Private _FK_DMS_Items_ChildItemID As SIS.DMS.dmsItems = Nothing
    Private _FK_DMS_Items_ItemTypeID As SIS.DMS.dmsItemTypes = Nothing
    Private _FK_DMS_Items_BackwardLinkedItemTypeID As SIS.DMS.dmsItemTypes = Nothing
    Private _FK_DMS_Items_LinkedItemTypeID As SIS.DMS.dmsItemTypes = Nothing
    Private _FK_DMS_Items_ForwardLinkedItemTypeID As SIS.DMS.dmsItemTypes = Nothing
    Private _FK_DMS_Items_StatusID As SIS.DMS.dmsStates = Nothing
    Private _FK_DMS_Items_ConvertedStatusID As SIS.DMS.dmsStates = Nothing
    Private _FK_DMS_Items_CompanyID As SIS.QCM.qcmCompanies = Nothing
    Private _FK_DMS_Items_DepartmentID As SIS.QCM.qcmDepartments = Nothing
    Private _FK_DMS_Items_DivisionID As SIS.QCM.qcmDivisions = Nothing
    Private _FK_DMS_Items_ProjectID As SIS.QCM.qcmProjects = Nothing
    Private _FK_DMS_Items_WBSID As SIS.PAK.pakWBS = Nothing
    Private _FK_DMS_Items_ActionBy As SIS.QCM.qcmUsers = Nothing
    Public Function FK_DMS_Items_UserID() As SIS.QCM.qcmUsers

      If _FK_DMS_Items_UserID Is Nothing Then
        _FK_DMS_Items_UserID = SIS.QCM.qcmUsers.qcmUsersGetByID(_UserID)
      End If
      Return _FK_DMS_Items_UserID

    End Function
    Public Function FK_DMS_Items_CreatedBy() As SIS.QCM.qcmUsers

      If _FK_DMS_Items_CreatedBy Is Nothing Then
        _FK_DMS_Items_CreatedBy = SIS.QCM.qcmUsers.qcmUsersGetByID(_CreatedBy)
      End If
      Return _FK_DMS_Items_CreatedBy

    End Function
    Public Function FK_DMS_Items_ForwardLinkedItemID() As SIS.DMS.dmsItems

      If _FK_DMS_Items_ForwardLinkedItemID Is Nothing Then
        _FK_DMS_Items_ForwardLinkedItemID = SIS.DMS.dmsItems.dmsItemsGetByID(_ForwardLinkedItemID)
      End If
      Return _FK_DMS_Items_ForwardLinkedItemID

    End Function
    Public Function FK_DMS_Items_LinkedItemID() As SIS.DMS.dmsItems

      If _FK_DMS_Items_LinkedItemID Is Nothing Then
        _FK_DMS_Items_LinkedItemID = SIS.DMS.dmsItems.dmsItemsGetByID(_LinkedItemID)
      End If
      Return _FK_DMS_Items_LinkedItemID

    End Function
    Public Function FK_DMS_Items_BackwardLinkedItemID() As SIS.DMS.dmsItems

      If _FK_DMS_Items_BackwardLinkedItemID Is Nothing Then
        _FK_DMS_Items_BackwardLinkedItemID = SIS.DMS.dmsItems.dmsItemsGetByID(_BackwardLinkedItemID)
      End If
      Return _FK_DMS_Items_BackwardLinkedItemID

    End Function
    Public Function FK_DMS_Items_ParentItemID() As SIS.DMS.dmsItems

      If _FK_DMS_Items_ParentItemID Is Nothing Then
        _FK_DMS_Items_ParentItemID = SIS.DMS.dmsItems.dmsItemsGetByID(_ParentItemID)
      End If
      Return _FK_DMS_Items_ParentItemID

    End Function
    Public Function FK_DMS_Items_ChildItemID() As SIS.DMS.dmsItems

      If _FK_DMS_Items_ChildItemID Is Nothing Then
        _FK_DMS_Items_ChildItemID = SIS.DMS.dmsItems.dmsItemsGetByID(_ChildItemID)
      End If
      Return _FK_DMS_Items_ChildItemID

    End Function
    Public Function FK_DMS_Items_ItemTypeID() As SIS.DMS.dmsItemTypes

      If _FK_DMS_Items_ItemTypeID Is Nothing Then
        _FK_DMS_Items_ItemTypeID = SIS.DMS.dmsItemTypes.dmsItemTypesGetByID(_ItemTypeID)
      End If
      Return _FK_DMS_Items_ItemTypeID

    End Function
    Public Function FK_DMS_Items_BackwardLinkedItemTypeID() As SIS.DMS.dmsItemTypes

      If _FK_DMS_Items_BackwardLinkedItemTypeID Is Nothing Then
        _FK_DMS_Items_BackwardLinkedItemTypeID = SIS.DMS.dmsItemTypes.dmsItemTypesGetByID(_BackwardLinkedItemTypeID)
      End If
      Return _FK_DMS_Items_BackwardLinkedItemTypeID

    End Function
    Public Function FK_DMS_Items_LinkedItemTypeID() As SIS.DMS.dmsItemTypes

      If _FK_DMS_Items_LinkedItemTypeID Is Nothing Then
        _FK_DMS_Items_LinkedItemTypeID = SIS.DMS.dmsItemTypes.dmsItemTypesGetByID(_LinkedItemTypeID)
      End If
      Return _FK_DMS_Items_LinkedItemTypeID

    End Function
    Public Function FK_DMS_Items_ForwardLinkedItemTypeID() As SIS.DMS.dmsItemTypes

      If _FK_DMS_Items_ForwardLinkedItemTypeID Is Nothing Then
        _FK_DMS_Items_ForwardLinkedItemTypeID = SIS.DMS.dmsItemTypes.dmsItemTypesGetByID(_ForwardLinkedItemTypeID)
      End If
      Return _FK_DMS_Items_ForwardLinkedItemTypeID

    End Function
    Public Function FK_DMS_Items_StatusID() As SIS.DMS.dmsStates

      If _FK_DMS_Items_StatusID Is Nothing Then
        _FK_DMS_Items_StatusID = SIS.DMS.dmsStates.dmsStatesGetByID(_StatusID)
      End If
      Return _FK_DMS_Items_StatusID

    End Function
    Public Function FK_DMS_Items_ConvertedStatusID() As SIS.DMS.dmsStates

      If _FK_DMS_Items_ConvertedStatusID Is Nothing Then
        _FK_DMS_Items_ConvertedStatusID = SIS.DMS.dmsStates.dmsStatesGetByID(_ConvertedStatusID)
      End If
      Return _FK_DMS_Items_ConvertedStatusID

    End Function
    Public Function FK_DMS_Items_CompanyID() As SIS.QCM.qcmCompanies

      If _FK_DMS_Items_CompanyID Is Nothing Then
        _FK_DMS_Items_CompanyID = SIS.QCM.qcmCompanies.qcmCompaniesGetByID(_CompanyID)
      End If
      Return _FK_DMS_Items_CompanyID

    End Function
    Public Function FK_DMS_Items_DepartmentID() As SIS.QCM.qcmDepartments

      If _FK_DMS_Items_DepartmentID Is Nothing Then
        _FK_DMS_Items_DepartmentID = SIS.QCM.qcmDepartments.qcmDepartmentsGetByID(_DepartmentID)
      End If
      Return _FK_DMS_Items_DepartmentID

    End Function
    Public Function FK_DMS_Items_DivisionID() As SIS.QCM.qcmDivisions

      If _FK_DMS_Items_DivisionID Is Nothing Then
        _FK_DMS_Items_DivisionID = SIS.QCM.qcmDivisions.qcmDivisionsGetByID(_DivisionID)
      End If
      Return _FK_DMS_Items_DivisionID

    End Function
    Public Function FK_DMS_Items_ProjectID() As SIS.QCM.qcmProjects

      If _FK_DMS_Items_ProjectID Is Nothing Then
        _FK_DMS_Items_ProjectID = SIS.QCM.qcmProjects.qcmProjectsGetByID(_ProjectID)
      End If
      Return _FK_DMS_Items_ProjectID

    End Function
    Public Function FK_DMS_Items_WBSID() As SIS.PAK.pakWBS

      If _FK_DMS_Items_WBSID Is Nothing Then
        _FK_DMS_Items_WBSID = SIS.PAK.pakWBS.pakWBSGetByID(_WBSID)
      End If
      Return _FK_DMS_Items_WBSID

    End Function
    Public Function FK_DMS_Items_ActionBy() As SIS.QCM.qcmUsers

      If _FK_DMS_Items_ActionBy Is Nothing Then
        _FK_DMS_Items_ActionBy = SIS.QCM.qcmUsers.qcmUsersGetByID(_ActionBy)
      End If
      Return _FK_DMS_Items_ActionBy

    End Function

#End Region

#Region " Custom Function "
    Public Shared Function ChildCount(ByVal Itm As SIS.DMS.dmsItems) As Integer
      Dim mRet As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Select Case Itm.ItemTypeID
          Case enumItemTypes.Searches
            Sql &= " select isnull(count(*),0) as cnt from dms_searchResults "
            Sql &= " WHERE "
            Sql &= " SearchID =" & Itm.ItemID
          Case Else
            Sql &= " select isnull(count(*),0) as cnt from dms_items "
            Sql &= " WHERE "
            Sql &= " ParentItemID =" & Itm.ItemID
        End Select
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          mRet = Cmd.ExecuteScalar
        End Using
      End Using
      Return mRet
    End Function
    Public Shared Function ChildItems(ByVal ItemID As Integer, ByVal ItemTypeID As Integer) As List(Of SIS.DMS.dmsItems)
      Dim mRet As New List(Of SIS.DMS.dmsItems)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = GetItemSelectStatement()
        Select Case ItemTypeID
          Case enumItemTypes.Searches
            Sql &= "   INNER JOIN DMS_SearchResults on DMS_SearchResults.ItemID = DMS_Items.ItemID "
            Sql &= "   WHERE "
            Sql &= "       [DMS_SearchResults].[SearchID]=" & ItemID
            Sql &= "   ORDER BY [DMS_Items].[ItemID] "
          Case Else
            Sql &= "   WHERE "
            Sql &= "   [DMS_Items].[ParentItemID]=" & ItemID
            Sql &= "   ORDER BY [DMS_Items].[ItemTypeID],[DMS_Items].[ItemID] "
        End Select
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            mRet.Add(New SIS.DMS.dmsItems(Reader))
          End While
          Reader.Close()
        End Using
      End Using
      Return mRet
    End Function

    Public Shared Function GetJsonItem(ByVal ItemID As Int32) As String
      Dim Results As SIS.DMS.dmsItems = Nothing
      Dim mRet As String = ""
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemsSelectByID"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemID", SqlDbType.Int, ItemID.ToString.Length, ItemID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 9, "")
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.DMS.dmsItems(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      If Results IsNot Nothing Then
        mRet = (New JavaScriptSerializer).Serialize(Results)
      End If
      Return mRet
    End Function

#End Region
#Region " Select Statements "
    Public Shared Function GetItemSelectStatement(Optional ByVal TopOne As Boolean = False) As String
      Dim Sql As String = ""
      If TopOne Then
        Sql &= "   SELECT TOP 1 "
      Else
        Sql &= "   SELECT "
      End If
      Sql &= "     [DMS_Items].* , "
      Sql &= "     [aspnet_Users1].[UserFullName] AS aspnet_Users1_UserFullName, "
      Sql &= "     [aspnet_Users2].[UserFullName] AS aspnet_Users2_UserFullName, "
      Sql &= "     [DMS_Items3].[Description] AS DMS_Items3_Description, "
      Sql &= "     [DMS_Items4].[Description] AS DMS_Items4_Description, "
      Sql &= "     [DMS_Items5].[Description] AS DMS_Items5_Description, "
      Sql &= "     [DMS_Items6].[Description] AS DMS_Items6_Description, "
      Sql &= "     [DMS_Items7].[Description] AS DMS_Items7_Description, "
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
      Sql &= "   FROM [DMS_Items]  "
      Sql &= "   LEFT OUTER JOIN [aspnet_users] AS [aspnet_users1] "
      Sql &= "     ON [DMS_Items].[UserID] = [aspnet_users1].[LoginID] "
      Sql &= "   LEFT OUTER JOIN [aspnet_users] AS [aspnet_users2] "
      Sql &= "     ON [DMS_Items].[CreatedBy] = [aspnet_users2].[LoginID] "
      Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items3] "
      Sql &= "     ON [DMS_Items].[ForwardLinkedItemID] = [DMS_Items3].[ItemID] "
      Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items4] "
      Sql &= "     ON [DMS_Items].[LinkedItemID] = [DMS_Items4].[ItemID] "
      Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items5] "
      Sql &= "     ON [DMS_Items].[BackwardLinkedItemID] = [DMS_Items5].[ItemID] "
      Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items6] "
      Sql &= "     ON [DMS_Items].[ParentItemID] = [DMS_Items6].[ItemID] "
      Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items7] "
      Sql &= "     ON [DMS_Items].[ChildItemID] = [DMS_Items7].[ItemID] "
      Sql &= "   INNER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes8] "
      Sql &= "     ON [DMS_Items].[ItemTypeID] = [DMS_ItemTypes8].[ItemTypeID] "
      Sql &= "   LEFT OUTER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes9] "
      Sql &= "     ON [DMS_Items].[BackwardLinkedItemTypeID] = [DMS_ItemTypes9].[ItemTypeID] "
      Sql &= "   LEFT OUTER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes10] "
      Sql &= "     ON [DMS_Items].[LinkedItemTypeID] = [DMS_ItemTypes10].[ItemTypeID] "
      Sql &= "   LEFT OUTER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes11] "
      Sql &= "     ON [DMS_Items].[ForwardLinkedItemTypeID] = [DMS_ItemTypes11].[ItemTypeID] "
      Sql &= "   INNER JOIN [DMS_States] AS [DMS_States12] "
      Sql &= "     ON [DMS_Items].[StatusID] = [DMS_States12].[StatusID] "
      Sql &= "   LEFT OUTER JOIN [DMS_States] AS [DMS_States13] "
      Sql &= "     ON [DMS_Items].[ConvertedStatusID] = [DMS_States13].[StatusID] "
      Sql &= "   LEFT OUTER JOIN [HRM_Companies] AS [HRM_Companies14] "
      Sql &= "     ON [DMS_Items].[CompanyID] = [HRM_Companies14].[CompanyID] "
      Sql &= "   LEFT OUTER JOIN [HRM_Departments] AS [HRM_Departments15] "
      Sql &= "     ON [DMS_Items].[DepartmentID] = [HRM_Departments15].[DepartmentID] "
      Sql &= "   LEFT OUTER JOIN [HRM_Divisions] AS [HRM_Divisions16] "
      Sql &= "     ON [DMS_Items].[DivisionID] = [HRM_Divisions16].[DivisionID] "
      Sql &= "   LEFT OUTER JOIN [IDM_Projects] AS [IDM_Projects17] "
      Sql &= "     ON [DMS_Items].[ProjectID] = [IDM_Projects17].[ProjectID] "
      Sql &= "   LEFT OUTER JOIN [IDM_WBS] AS [IDM_WBS18] "
      Sql &= "     ON [DMS_Items].[WBSID] = [IDM_WBS18].[WBSID] "
      Sql &= "   LEFT OUTER JOIN [aspnet_users] AS [aspnet_users19] "
      Sql &= "     ON [DMS_Items].[ActionBy] = [aspnet_users19].[LoginID] "
      Return Sql
    End Function
#End Region

    <DataObjectMethod(DataObjectMethodType.Select)>
    Public Shared Function dmsItemsGetNewRecord() As SIS.DMS.dmsItems
      Return New SIS.DMS.dmsItems()
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)>
    Public Shared Function dmsItemsGetByID(ByVal ItemID As Int32) As SIS.DMS.dmsItems
      Dim Results As SIS.DMS.dmsItems = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemsSelectByID"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemID", SqlDbType.Int, ItemID.ToString.Length, ItemID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 9, "")
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.DMS.dmsItems(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)>
    Public Shared Function dmsItemsSelectList(ByVal StartRowIndex As Integer, ByVal MaximumRows As Integer, ByVal OrderBy As String, ByVal SearchState As Boolean, ByVal SearchText As String) As List(Of SIS.DMS.dmsItems)
      Dim Results As List(Of SIS.DMS.dmsItems) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          If SearchState Then
            Cmd.CommandText = "spdmsItemsSelectListSearch"
            SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@KeyWord", SqlDbType.NVarChar, 250, SearchText)
          Else
            Cmd.CommandText = "spdmsItemsSelectListFilteres"
          End If
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StartRowIndex", SqlDbType.Int, -1, StartRowIndex)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaximumRows", SqlDbType.Int, -1, MaximumRows)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 9, HttpContext.Current.Session("LoginID"))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OrderBy", SqlDbType.NVarChar, 50, OrderBy)
          Cmd.Parameters.Add("@RecordCount", SqlDbType.Int)
          Cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Results = New List(Of SIS.DMS.dmsItems)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.dmsItems(Reader))
          End While
          Reader.Close()
          _RecordCount = Cmd.Parameters("@RecordCount").Value
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function dmsItemsSelectCount(ByVal SearchState As Boolean, ByVal SearchText As String) As Integer
      Return _RecordCount
    End Function
    'Select By ID One Record Filtered Overloaded GetByID
    <DataObjectMethod(DataObjectMethodType.Insert, True)>
    Public Shared Function dmsItemsInsert(ByVal Record As SIS.DMS.dmsItems) As SIS.DMS.dmsItems
      Dim _Rec As SIS.DMS.dmsItems = SIS.DMS.dmsItems.dmsItemsGetNewRecord()
      With _Rec
        .InheritFromParent = Record.InheritFromParent
        .UserID = Record.UserID
        .Description = Record.Description
        .RevisionNo = Record.RevisionNo
        .ItemTypeID = Record.ItemTypeID
        .StatusID = Record.StatusID
        .CreatedBy = Record.CreatedBy
        .CreatedOn = Record.CreatedOn
        .MaintainAllLog = Record.MaintainAllLog
        .BackwardLinkedItemID = Record.BackwardLinkedItemID
        .MaintainVersions = Record.MaintainVersions
        .MaintainStatusLog = Record.MaintainStatusLog
        .LinkedItemID = Record.LinkedItemID
        .LinkedItemTypeID = Record.LinkedItemTypeID
        .BackwardLinkedItemTypeID = Record.BackwardLinkedItemTypeID
        .IsMultiBackward = Record.IsMultiBackward
        .IsgecVaultID = Record.IsgecVaultID
        .DeleteFile = Record.DeleteFile
        .CreateFile = Record.CreateFile
        .BrowseList = Record.BrowseList
        .GrantAuthorization = Record.GrantAuthorization
        .CreateFolder = Record.CreateFolder
        .Publish = Record.Publish
        .DeleteFolder = Record.DeleteFolder
        .RenameFolder = Record.RenameFolder
        .CompanyID = Record.CompanyID
        .ChildItemID = Record.ChildItemID
        .DepartmentID = Record.DepartmentID
        .DivisionID = Record.DivisionID
        .IsMultiParent = Record.IsMultiParent
        .ConvertedStatusID = Record.ConvertedStatusID
        .IsMultiChild = Record.IsMultiChild
        .ParentItemID = Record.ParentItemID
        .ProjectID = Record.ProjectID
        .ForwardLinkedItemTypeID = Record.ForwardLinkedItemTypeID
        .IsMultiForward = Record.IsMultiForward
        .IsMultiLinked = Record.IsMultiLinked
        .ForwardLinkedItemID = Record.ForwardLinkedItemID
        .KeyWords = Record.KeyWords
        .WBSID = Record.WBSID
        .FullDescription = Record.FullDescription
        .EMailID = Record.EMailID
        .SearchInParent = Record.SearchInParent
        .ShowInList = Record.ShowInList
        .Approved = Record.Approved
        .Rejected = Record.Rejected
        .ActionRemarks = Record.ActionRemarks
        .ActionBy = Record.ActionBy
        .ActionOn = Record.ActionOn
        .IsError = Record.IsError
        .ErrorMessage = Record.ErrorMessage
      End With
      Return SIS.DMS.dmsItems.InsertData(_Rec)
    End Function
    Public Shared Function InsertData(ByVal Record As SIS.DMS.dmsItems) As SIS.DMS.dmsItems
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemsInsert"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@InheritFromParent", SqlDbType.Bit, 3, Record.InheritFromParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@UserID", SqlDbType.NVarChar, 9, IIf(Record.UserID = "", Convert.DBNull, Record.UserID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Description", SqlDbType.NVarChar, 251, Record.Description)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@RevisionNo", SqlDbType.NVarChar, 51, Record.RevisionNo)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemTypeID", SqlDbType.Int, 11, Record.ItemTypeID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StatusID", SqlDbType.Int, 11, Record.StatusID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedBy", SqlDbType.NVarChar, 9, Record.CreatedBy)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedOn", SqlDbType.DateTime, 21, Record.CreatedOn)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaintainAllLog", SqlDbType.Bit, 3, Record.MaintainAllLog)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@BackwardLinkedItemID", SqlDbType.Int, 11, IIf(Record.BackwardLinkedItemID = "", Convert.DBNull, Record.BackwardLinkedItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaintainVersions", SqlDbType.Bit, 3, Record.MaintainVersions)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaintainStatusLog", SqlDbType.Bit, 3, Record.MaintainStatusLog)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LinkedItemID", SqlDbType.Int, 11, IIf(Record.LinkedItemID = "", Convert.DBNull, Record.LinkedItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LinkedItemTypeID", SqlDbType.Int, 11, IIf(Record.LinkedItemTypeID = "", Convert.DBNull, Record.LinkedItemTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@BackwardLinkedItemTypeID", SqlDbType.Int, 11, IIf(Record.BackwardLinkedItemTypeID = "", Convert.DBNull, Record.BackwardLinkedItemTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiBackward", SqlDbType.Bit, 3, Record.IsMultiBackward)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsgecVaultID", SqlDbType.NVarChar, 51, IIf(Record.IsgecVaultID = "", Convert.DBNull, Record.IsgecVaultID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DeleteFile", SqlDbType.Int, 11, Record.DeleteFile)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreateFile", SqlDbType.Int, 11, Record.CreateFile)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@BrowseList", SqlDbType.Int, 11, Record.BrowseList)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@GrantAuthorization", SqlDbType.Int, 11, Record.GrantAuthorization)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreateFolder", SqlDbType.Int, 11, Record.CreateFolder)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Publish", SqlDbType.Int, 11, Record.Publish)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DeleteFolder", SqlDbType.Int, 11, Record.DeleteFolder)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@RenameFolder", SqlDbType.Int, 11, Record.RenameFolder)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ShowInList", SqlDbType.Int, 11, Record.ShowInList)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CompanyID", SqlDbType.NVarChar, 7, IIf(Record.CompanyID = "", Convert.DBNull, Record.CompanyID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ChildItemID", SqlDbType.Int, 11, IIf(Record.ChildItemID = "", Convert.DBNull, Record.ChildItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DepartmentID", SqlDbType.NVarChar, 7, IIf(Record.DepartmentID = "", Convert.DBNull, Record.DepartmentID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DivisionID", SqlDbType.NVarChar, 7, IIf(Record.DivisionID = "", Convert.DBNull, Record.DivisionID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiParent", SqlDbType.Bit, 3, Record.IsMultiParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ConvertedStatusID", SqlDbType.Int, 11, IIf(Record.ConvertedStatusID = "", Convert.DBNull, Record.ConvertedStatusID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiChild", SqlDbType.Bit, 3, Record.IsMultiChild)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ParentItemID", SqlDbType.Int, 11, IIf(Record.ParentItemID = "", Convert.DBNull, Record.ParentItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ProjectID", SqlDbType.NVarChar, 7, IIf(Record.ProjectID = "", Convert.DBNull, Record.ProjectID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ForwardLinkedItemTypeID", SqlDbType.Int, 11, IIf(Record.ForwardLinkedItemTypeID = "", Convert.DBNull, Record.ForwardLinkedItemTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiForward", SqlDbType.Bit, 3, Record.IsMultiForward)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiLinked", SqlDbType.Bit, 3, Record.IsMultiLinked)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ForwardLinkedItemID", SqlDbType.Int, 11, IIf(Record.ForwardLinkedItemID = "", Convert.DBNull, Record.ForwardLinkedItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@KeyWords", SqlDbType.NVarChar, 251, IIf(Record.KeyWords = "", Convert.DBNull, Record.KeyWords))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@WBSID", SqlDbType.NVarChar, 9, IIf(Record.WBSID = "", Convert.DBNull, Record.WBSID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@FullDescription", SqlDbType.NVarChar, 1001, IIf(Record.FullDescription = "", Convert.DBNull, Record.FullDescription))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@EMailID", SqlDbType.NVarChar, 251, IIf(Record.EMailID = "", Convert.DBNull, Record.EMailID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@SearchInParent", SqlDbType.Bit, 3, Record.SearchInParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Approved", SqlDbType.Bit, 3, Record.Approved)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Rejected", SqlDbType.Bit, 3, Record.Rejected)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ActionRemarks", SqlDbType.NVarChar, 251, IIf(Record.ActionRemarks = "", Convert.DBNull, Record.ActionRemarks))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ActionBy", SqlDbType.NVarChar, 9, IIf(Record.ActionBy = "", Convert.DBNull, Record.ActionBy))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ActionOn", SqlDbType.DateTime, 21, IIf(Record.ActionOn = "", Convert.DBNull, Record.ActionOn))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsError", SqlDbType.Bit, 3, IIf(Record.IsError = "", Convert.DBNull, Record.IsError))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ErrorMessage", SqlDbType.NVarChar, 501, IIf(Record.ErrorMessage = "", Convert.DBNull, Record.ErrorMessage))
          Cmd.Parameters.Add("@Return_ItemID", SqlDbType.Int, 11)
          Cmd.Parameters("@Return_ItemID").Direction = ParameterDirection.Output
          Con.Open()
          Cmd.ExecuteNonQuery()
          Record.ItemID = Cmd.Parameters("@Return_ItemID").Value
        End Using
      End Using
      Return Record
    End Function
    <DataObjectMethod(DataObjectMethodType.Update, True)>
    Public Shared Function dmsItemsUpdate(ByVal Record As SIS.DMS.dmsItems) As SIS.DMS.dmsItems
      Dim _Rec As SIS.DMS.dmsItems = SIS.DMS.dmsItems.dmsItemsGetByID(Record.ItemID)
      With _Rec
        .InheritFromParent = Record.InheritFromParent
        .UserID = Record.UserID
        .Description = Record.Description
        .RevisionNo = Record.RevisionNo
        .ItemTypeID = Record.ItemTypeID
        .StatusID = Record.StatusID
        .CreatedBy = Record.CreatedBy
        .CreatedOn = Record.CreatedOn
        .MaintainAllLog = Record.MaintainAllLog
        .BackwardLinkedItemID = Record.BackwardLinkedItemID
        .MaintainVersions = Record.MaintainVersions
        .MaintainStatusLog = Record.MaintainStatusLog
        .LinkedItemID = Record.LinkedItemID
        .LinkedItemTypeID = Record.LinkedItemTypeID
        .BackwardLinkedItemTypeID = Record.BackwardLinkedItemTypeID
        .IsMultiBackward = Record.IsMultiBackward
        .IsgecVaultID = Record.IsgecVaultID
        .DeleteFile = Record.DeleteFile
        .CreateFile = Record.CreateFile
        .BrowseList = Record.BrowseList
        .GrantAuthorization = Record.GrantAuthorization
        .CreateFolder = Record.CreateFolder
        .Publish = Record.Publish
        .DeleteFolder = Record.DeleteFolder
        .RenameFolder = Record.RenameFolder
        .CompanyID = Record.CompanyID
        .ChildItemID = Record.ChildItemID
        .DepartmentID = Record.DepartmentID
        .DivisionID = Record.DivisionID
        .IsMultiParent = Record.IsMultiParent
        .ConvertedStatusID = Record.ConvertedStatusID
        .IsMultiChild = Record.IsMultiChild
        .ParentItemID = Record.ParentItemID
        .ProjectID = Record.ProjectID
        .ForwardLinkedItemTypeID = Record.ForwardLinkedItemTypeID
        .IsMultiForward = Record.IsMultiForward
        .IsMultiLinked = Record.IsMultiLinked
        .ForwardLinkedItemID = Record.ForwardLinkedItemID
        .KeyWords = Record.KeyWords
        .WBSID = Record.WBSID
        .FullDescription = Record.FullDescription
        .EMailID = Record.EMailID
        .SearchInParent = Record.SearchInParent
        .ShowInList = Record.ShowInList
        .Approved = Record.Approved
        .Rejected = Record.Rejected
        .ActionRemarks = Record.ActionRemarks
        .ActionBy = Record.ActionBy
        .ActionOn = Record.ActionOn
        .IsError = Record.IsError
        .ErrorMessage = Record.ErrorMessage
      End With
      Return SIS.DMS.dmsItems.UpdateData(_Rec)
    End Function
    Public Shared Function UpdateData(ByVal Record As SIS.DMS.dmsItems) As SIS.DMS.dmsItems
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemsUpdate"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_ItemID", SqlDbType.Int, 11, Record.ItemID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@InheritFromParent", SqlDbType.Bit, 3, Record.InheritFromParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@UserID", SqlDbType.NVarChar, 9, IIf(Record.UserID = "", Convert.DBNull, Record.UserID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Description", SqlDbType.NVarChar, 251, Record.Description)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@RevisionNo", SqlDbType.NVarChar, 51, Record.RevisionNo)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemTypeID", SqlDbType.Int, 11, Record.ItemTypeID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StatusID", SqlDbType.Int, 11, Record.StatusID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedBy", SqlDbType.NVarChar, 9, Record.CreatedBy)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedOn", SqlDbType.DateTime, 21, Record.CreatedOn)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaintainAllLog", SqlDbType.Bit, 3, Record.MaintainAllLog)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@BackwardLinkedItemID", SqlDbType.Int, 11, IIf(Record.BackwardLinkedItemID = "", Convert.DBNull, Record.BackwardLinkedItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaintainVersions", SqlDbType.Bit, 3, Record.MaintainVersions)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaintainStatusLog", SqlDbType.Bit, 3, Record.MaintainStatusLog)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LinkedItemID", SqlDbType.Int, 11, IIf(Record.LinkedItemID = "", Convert.DBNull, Record.LinkedItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LinkedItemTypeID", SqlDbType.Int, 11, IIf(Record.LinkedItemTypeID = "", Convert.DBNull, Record.LinkedItemTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@BackwardLinkedItemTypeID", SqlDbType.Int, 11, IIf(Record.BackwardLinkedItemTypeID = "", Convert.DBNull, Record.BackwardLinkedItemTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiBackward", SqlDbType.Bit, 3, Record.IsMultiBackward)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsgecVaultID", SqlDbType.NVarChar, 51, IIf(Record.IsgecVaultID = "", Convert.DBNull, Record.IsgecVaultID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DeleteFile", SqlDbType.Int, 11, Record.DeleteFile)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreateFile", SqlDbType.Int, 11, Record.CreateFile)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@BrowseList", SqlDbType.Int, 11, Record.BrowseList)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@GrantAuthorization", SqlDbType.Int, 11, Record.GrantAuthorization)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreateFolder", SqlDbType.Int, 11, Record.CreateFolder)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Publish", SqlDbType.Int, 11, Record.Publish)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DeleteFolder", SqlDbType.Int, 11, Record.DeleteFolder)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@RenameFolder", SqlDbType.Int, 11, Record.RenameFolder)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ShowInList", SqlDbType.Int, 11, Record.ShowInList)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CompanyID", SqlDbType.NVarChar, 7, IIf(Record.CompanyID = "", Convert.DBNull, Record.CompanyID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ChildItemID", SqlDbType.Int, 11, IIf(Record.ChildItemID = "", Convert.DBNull, Record.ChildItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DepartmentID", SqlDbType.NVarChar, 7, IIf(Record.DepartmentID = "", Convert.DBNull, Record.DepartmentID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DivisionID", SqlDbType.NVarChar, 7, IIf(Record.DivisionID = "", Convert.DBNull, Record.DivisionID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiParent", SqlDbType.Bit, 3, Record.IsMultiParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ConvertedStatusID", SqlDbType.Int, 11, IIf(Record.ConvertedStatusID = "", Convert.DBNull, Record.ConvertedStatusID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiChild", SqlDbType.Bit, 3, Record.IsMultiChild)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ParentItemID", SqlDbType.Int, 11, IIf(Record.ParentItemID = "", Convert.DBNull, Record.ParentItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ProjectID", SqlDbType.NVarChar, 7, IIf(Record.ProjectID = "", Convert.DBNull, Record.ProjectID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ForwardLinkedItemTypeID", SqlDbType.Int, 11, IIf(Record.ForwardLinkedItemTypeID = "", Convert.DBNull, Record.ForwardLinkedItemTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiForward", SqlDbType.Bit, 3, Record.IsMultiForward)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiLinked", SqlDbType.Bit, 3, Record.IsMultiLinked)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ForwardLinkedItemID", SqlDbType.Int, 11, IIf(Record.ForwardLinkedItemID = "", Convert.DBNull, Record.ForwardLinkedItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@KeyWords", SqlDbType.NVarChar, 251, IIf(Record.KeyWords = "", Convert.DBNull, Record.KeyWords))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@WBSID", SqlDbType.NVarChar, 9, IIf(Record.WBSID = "", Convert.DBNull, Record.WBSID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@FullDescription", SqlDbType.NVarChar, 1001, IIf(Record.FullDescription = "", Convert.DBNull, Record.FullDescription))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@EMailID", SqlDbType.NVarChar, 251, IIf(Record.EMailID = "", Convert.DBNull, Record.EMailID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@SearchInParent", SqlDbType.Bit, 3, Record.SearchInParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Approved", SqlDbType.Bit, 3, Record.Approved)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Rejected", SqlDbType.Bit, 3, Record.Rejected)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ActionRemarks", SqlDbType.NVarChar, 251, IIf(Record.ActionRemarks = "", Convert.DBNull, Record.ActionRemarks))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ActionBy", SqlDbType.NVarChar, 9, IIf(Record.ActionBy = "", Convert.DBNull, Record.ActionBy))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ActionOn", SqlDbType.DateTime, 21, IIf(Record.ActionOn = "", Convert.DBNull, Record.ActionOn))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsError", SqlDbType.Bit, 3, IIf(Record.IsError = "", Convert.DBNull, Record.IsError))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ErrorMessage", SqlDbType.NVarChar, 501, IIf(Record.ErrorMessage = "", Convert.DBNull, Record.ErrorMessage))
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
    Public Shared Function dmsItemsDelete(ByVal Record As SIS.DMS.dmsItems) As Int32
      Dim _Result As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemsDelete"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_ItemID", SqlDbType.Int, Record.ItemID.ToString.Length, Record.ItemID)
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
    Public Shared Function SelectdmsItemsAutoCompleteList(ByVal Prefix As String, ByVal count As Integer, ByVal contextKey As String) As String()
      Dim Results As List(Of String) = Nothing
      Dim aVal() As String = contextKey.Split("|".ToCharArray)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemsAutoCompleteList"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NVarChar, 9, HttpContext.Current.Session("LoginID"))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@prefix", SqlDbType.NVarChar, 50, Prefix)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@records", SqlDbType.Int, -1, count)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@bycode", SqlDbType.Int, 1, IIf(IsNumeric(Prefix), 0, 1))
          Results = New List(Of String)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Not Reader.HasRows Then
            Results.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem("---Select Value---".PadRight(250, " "), ""))
          End If
          While (Reader.Read())
            Dim Tmp As SIS.DMS.dmsItems = New SIS.DMS.dmsItems(Reader)
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
