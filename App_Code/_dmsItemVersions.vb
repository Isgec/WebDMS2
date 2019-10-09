Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.DMS
  <DataObject()> _
  Partial Public Class dmsItemVersions
    Private Shared _RecordCount As Integer
    Private _ItemID As Int32 = 0
    Private _SerialNo As Int32 = 0
    Private _UserID As String = ""
    Private _Description As String = ""
    Private _FullDescription As String = ""
    Private _RevisionNo As String = ""
    Private _EMailID As String = ""
    Private _ItemTypeID As Int32 = 0
    Private _StatusID As Int32 = 0
    Private _ConvertedStatusID As String = ""
    Private _IsMultiParent As Boolean = False
    Private _ParentItemID As String = ""
    Private _IsMultiChild As Boolean = False
    Private _ChildItemID As String = ""
    Private _CompanyID As String = ""
    Private _DivisionID As String = ""
    Private _DepartmentID As String = ""
    Private _ProjectID As String = ""
    Private _WBSID As String = ""
    Private _KeyWords As String = ""
    Private _CreatedBy As String = ""
    Private _CreatedOn As String = ""
    Private _IsMultiForward As Boolean = False
    Private _ForwardLinkedItemTypeID As String = ""
    Private _ForwardLinkedItemID As String = ""
    Private _IsMultiLinked As Boolean = False
    Private _LinkedItemTypeID As String = ""
    Private _LinkedItemID As String = ""
    Private _IsMultiBackward As Boolean = False
    Private _BackwardLinkedItemTypeID As String = ""
    Private _BackwardLinkedItemID As String = ""
    Private _MaintainAllLog As Boolean = False
    Private _MaintainStatusLog As Boolean = False
    Private _MaintainVersions As Boolean = False
    Private _IsgecVaultID As String = ""
    Private _Publish As Boolean = False
    Private _CreateFolder As Boolean = False
    Private _RenameFolder As Boolean = False
    Private _DeleteFolder As Boolean = False
    Private _CreateFile As Boolean = False
    Private _DeleteFile As Boolean = False
    Private _GrantAuthorization As Boolean = False
    Private _BrowseList As Boolean = False
    Private _InheritFromParent As Boolean = False
    Private _SearchInParent As Boolean = False
    Private _ShowInList As Boolean = False
    Private _Approved As Boolean = False
    Private _Rejected As Boolean = False
    Private _ActionRemarks As String = ""
    Private _ActionBy As String = ""
    Private _ActionOn As String = ""
    Private _IsError As String = ""
    Private _ErrorMessage As String = ""
    Private _aspnet_Users1_UserFullName As String = ""
    Private _aspnet_Users2_UserFullName As String = ""
    Private _DMS_ItemTypes3_Description As String = ""
    Private _DMS_ItemTypes4_Description As String = ""
    Private _DMS_ItemTypes5_Description As String = ""
    Private _DMS_ItemTypes6_Description As String = ""
    Private _DMS_States7_Description As String = ""
    Private _DMS_States8_Description As String = ""
    Private _HRM_Companies9_Description As String = ""
    Private _HRM_Departments10_Description As String = ""
    Private _HRM_Divisions11_Description As String = ""
    Private _IDM_Projects12_Description As String = ""
    Private _IDM_WBS13_Description As String = ""
    Private _DMS_Items14_Description As String = ""
    Private _aspnet_Users19_UserFullName As String = ""
    Private _FK_DMS_ItemVersions_CreatedBy As SIS.QCM.qcmUsers = Nothing
    Private _FK_DMS_ItemVersions_UserID As SIS.QCM.qcmUsers = Nothing
    Private _FK_DMS_ItemVersions_BackwardLinkedItemTypeID As SIS.DMS.dmsItemTypes = Nothing
    Private _FK_DMS_ItemVersions_ForwardLinkedItemTypeID As SIS.DMS.dmsItemTypes = Nothing
    Private _FK_DMS_ItemVersions_ItemTypeID As SIS.DMS.dmsItemTypes = Nothing
    Private _FK_DMS_ItemVersions_LinkedItemTypeID As SIS.DMS.dmsItemTypes = Nothing
    Private _FK_DMS_ItemVersions_StatusID As SIS.DMS.dmsStates = Nothing
    Private _FK_DMS_ItemVersions_ConvertedStatusID As SIS.DMS.dmsStates = Nothing
    Private _FK_DMS_ItemVersions_CompanyID As SIS.QCM.qcmCompanies = Nothing
    Private _FK_DMS_ItemVersions_DepartmentID As SIS.QCM.qcmDepartments = Nothing
    Private _FK_DMS_ItemVersions_DivisionID As SIS.QCM.qcmDivisions = Nothing
    Private _FK_DMS_ItemVersions_ProjectID As SIS.QCM.qcmProjects = Nothing
    Private _FK_DMS_ItemVersions_WBSID As SIS.PAK.pakWBS = Nothing
    Private _FK_DMS_ItemVersions_ItemID As SIS.DMS.dmsItems = Nothing
    Private _FK_DMS_ItemVersions_ActionBy As SIS.QCM.qcmUsers = Nothing
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
    Public Property ItemID() As Int32
      Get
        Return _ItemID
      End Get
      Set(ByVal value As Int32)
        _ItemID = value
      End Set
    End Property
    Public Property SerialNo() As Int32
      Get
        Return _SerialNo
      End Get
      Set(ByVal value As Int32)
        _SerialNo = value
      End Set
    End Property
    Public Property UserID() As String
      Get
        Return _UserID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
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
    Public Property FullDescription() As String
      Get
        Return _FullDescription
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _FullDescription = ""
         Else
           _FullDescription = value
         End If
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
    Public Property EMailID() As String
      Get
        Return _EMailID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _EMailID = ""
         Else
           _EMailID = value
         End If
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
    Public Property ConvertedStatusID() As String
      Get
        Return _ConvertedStatusID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _ConvertedStatusID = ""
         Else
           _ConvertedStatusID = value
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
    Public Property ParentItemID() As String
      Get
        Return _ParentItemID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _ParentItemID = ""
         Else
           _ParentItemID = value
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
    Public Property ChildItemID() As String
      Get
        Return _ChildItemID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _ChildItemID = ""
         Else
           _ChildItemID = value
         End If
      End Set
    End Property
    Public Property CompanyID() As String
      Get
        Return _CompanyID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _CompanyID = ""
         Else
           _CompanyID = value
         End If
      End Set
    End Property
    Public Property DivisionID() As String
      Get
        Return _DivisionID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _DivisionID = ""
         Else
           _DivisionID = value
         End If
      End Set
    End Property
    Public Property DepartmentID() As String
      Get
        Return _DepartmentID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _DepartmentID = ""
         Else
           _DepartmentID = value
         End If
      End Set
    End Property
    Public Property ProjectID() As String
      Get
        Return _ProjectID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _ProjectID = ""
         Else
           _ProjectID = value
         End If
      End Set
    End Property
    Public Property WBSID() As String
      Get
        Return _WBSID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _WBSID = ""
         Else
           _WBSID = value
         End If
      End Set
    End Property
    Public Property KeyWords() As String
      Get
        Return _KeyWords
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _KeyWords = ""
         Else
           _KeyWords = value
         End If
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
          Return Convert.ToDateTime(_CreatedOn).ToString("dd/MM/yyyy")
        End If
        Return _CreatedOn
      End Get
      Set(ByVal value As String)
         _CreatedOn = value
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
    Public Property ForwardLinkedItemTypeID() As String
      Get
        Return _ForwardLinkedItemTypeID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _ForwardLinkedItemTypeID = ""
         Else
           _ForwardLinkedItemTypeID = value
         End If
      End Set
    End Property
    Public Property ForwardLinkedItemID() As String
      Get
        Return _ForwardLinkedItemID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _ForwardLinkedItemID = ""
         Else
           _ForwardLinkedItemID = value
         End If
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
    Public Property LinkedItemTypeID() As String
      Get
        Return _LinkedItemTypeID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _LinkedItemTypeID = ""
         Else
           _LinkedItemTypeID = value
         End If
      End Set
    End Property
    Public Property LinkedItemID() As String
      Get
        Return _LinkedItemID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _LinkedItemID = ""
         Else
           _LinkedItemID = value
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
    Public Property BackwardLinkedItemTypeID() As String
      Get
        Return _BackwardLinkedItemTypeID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _BackwardLinkedItemTypeID = ""
         Else
           _BackwardLinkedItemTypeID = value
         End If
      End Set
    End Property
    Public Property BackwardLinkedItemID() As String
      Get
        Return _BackwardLinkedItemID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _BackwardLinkedItemID = ""
         Else
           _BackwardLinkedItemID = value
         End If
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
    Public Property MaintainStatusLog() As Boolean
      Get
        Return _MaintainStatusLog
      End Get
      Set(ByVal value As Boolean)
        _MaintainStatusLog = value
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
    Public Property IsgecVaultID() As String
      Get
        Return _IsgecVaultID
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _IsgecVaultID = ""
         Else
           _IsgecVaultID = value
         End If
      End Set
    End Property
    Public Property Publish() As Boolean
      Get
        Return _Publish
      End Get
      Set(ByVal value As Boolean)
        _Publish = value
      End Set
    End Property
    Public Property CreateFolder() As Boolean
      Get
        Return _CreateFolder
      End Get
      Set(ByVal value As Boolean)
        _CreateFolder = value
      End Set
    End Property
    Public Property RenameFolder() As Boolean
      Get
        Return _RenameFolder
      End Get
      Set(ByVal value As Boolean)
        _RenameFolder = value
      End Set
    End Property
    Public Property DeleteFolder() As Boolean
      Get
        Return _DeleteFolder
      End Get
      Set(ByVal value As Boolean)
        _DeleteFolder = value
      End Set
    End Property
    Public Property CreateFile() As Boolean
      Get
        Return _CreateFile
      End Get
      Set(ByVal value As Boolean)
        _CreateFile = value
      End Set
    End Property
    Public Property DeleteFile() As Boolean
      Get
        Return _DeleteFile
      End Get
      Set(ByVal value As Boolean)
        _DeleteFile = value
      End Set
    End Property
    Public Property GrantAuthorization() As Boolean
      Get
        Return _GrantAuthorization
      End Get
      Set(ByVal value As Boolean)
        _GrantAuthorization = value
      End Set
    End Property
    Public Property BrowseList() As Boolean
      Get
        Return _BrowseList
      End Get
      Set(ByVal value As Boolean)
        _BrowseList = value
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
    Public Property SearchInParent() As Boolean
      Get
        Return _SearchInParent
      End Get
      Set(ByVal value As Boolean)
        _SearchInParent = value
      End Set
    End Property
    Public Property ShowInList() As Boolean
      Get
        Return _ShowInList
      End Get
      Set(ByVal value As Boolean)
        _ShowInList = value
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
         If Convert.IsDBNull(Value) Then
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
         If Convert.IsDBNull(Value) Then
           _ActionBy = ""
         Else
           _ActionBy = value
         End If
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
         If Convert.IsDBNull(Value) Then
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
         If Convert.IsDBNull(Value) Then
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
         If Convert.IsDBNull(Value) Then
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
    Public Property DMS_ItemTypes4_Description() As String
      Get
        Return _DMS_ItemTypes4_Description
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _DMS_ItemTypes4_Description = ""
         Else
           _DMS_ItemTypes4_Description = value
         End If
      End Set
    End Property
    Public Property DMS_ItemTypes5_Description() As String
      Get
        Return _DMS_ItemTypes5_Description
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _DMS_ItemTypes5_Description = ""
         Else
           _DMS_ItemTypes5_Description = value
         End If
      End Set
    End Property
    Public Property DMS_ItemTypes6_Description() As String
      Get
        Return _DMS_ItemTypes6_Description
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _DMS_ItemTypes6_Description = ""
         Else
           _DMS_ItemTypes6_Description = value
         End If
      End Set
    End Property
    Public Property DMS_States7_Description() As String
      Get
        Return _DMS_States7_Description
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _DMS_States7_Description = ""
         Else
           _DMS_States7_Description = value
         End If
      End Set
    End Property
    Public Property DMS_States8_Description() As String
      Get
        Return _DMS_States8_Description
      End Get
      Set(ByVal value As String)
         If Convert.IsDBNull(Value) Then
           _DMS_States8_Description = ""
         Else
           _DMS_States8_Description = value
         End If
      End Set
    End Property
    Public Property HRM_Companies9_Description() As String
      Get
        Return _HRM_Companies9_Description
      End Get
      Set(ByVal value As String)
        _HRM_Companies9_Description = value
      End Set
    End Property
    Public Property HRM_Departments10_Description() As String
      Get
        Return _HRM_Departments10_Description
      End Get
      Set(ByVal value As String)
        _HRM_Departments10_Description = value
      End Set
    End Property
    Public Property HRM_Divisions11_Description() As String
      Get
        Return _HRM_Divisions11_Description
      End Get
      Set(ByVal value As String)
        _HRM_Divisions11_Description = value
      End Set
    End Property
    Public Property IDM_Projects12_Description() As String
      Get
        Return _IDM_Projects12_Description
      End Get
      Set(ByVal value As String)
        _IDM_Projects12_Description = value
      End Set
    End Property
    Public Property IDM_WBS13_Description() As String
      Get
        Return _IDM_WBS13_Description
      End Get
      Set(ByVal value As String)
        _IDM_WBS13_Description = value
      End Set
    End Property
    Public Property DMS_Items14_Description() As String
      Get
        Return _DMS_Items14_Description
      End Get
      Set(ByVal value As String)
        _DMS_Items14_Description = value
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
    Public Readonly Property DisplayField() As String
      Get
        Return ""
      End Get
    End Property
    Public Readonly Property PrimaryKey() As String
      Get
        Return _ItemID & "|" & _SerialNo
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
    Public Class PKdmsItemVersions
      Private _ItemID As Int32 = 0
      Private _SerialNo As Int32 = 0
      Public Property ItemID() As Int32
        Get
          Return _ItemID
        End Get
        Set(ByVal value As Int32)
          _ItemID = value
        End Set
      End Property
      Public Property SerialNo() As Int32
        Get
          Return _SerialNo
        End Get
        Set(ByVal value As Int32)
          _SerialNo = value
        End Set
      End Property
    End Class
    Public ReadOnly Property FK_DMS_ItemVersions_CreatedBy() As SIS.QCM.qcmUsers
      Get
        If _FK_DMS_ItemVersions_CreatedBy Is Nothing Then
          _FK_DMS_ItemVersions_CreatedBy = SIS.QCM.qcmUsers.qcmUsersGetByID(_CreatedBy)
        End If
        Return _FK_DMS_ItemVersions_CreatedBy
      End Get
    End Property
    Public ReadOnly Property FK_DMS_ItemVersions_UserID() As SIS.QCM.qcmUsers
      Get
        If _FK_DMS_ItemVersions_UserID Is Nothing Then
          _FK_DMS_ItemVersions_UserID = SIS.QCM.qcmUsers.qcmUsersGetByID(_UserID)
        End If
        Return _FK_DMS_ItemVersions_UserID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_ItemVersions_BackwardLinkedItemTypeID() As SIS.DMS.dmsItemTypes
      Get
        If _FK_DMS_ItemVersions_BackwardLinkedItemTypeID Is Nothing Then
          _FK_DMS_ItemVersions_BackwardLinkedItemTypeID = SIS.DMS.dmsItemTypes.dmsItemTypesGetByID(_BackwardLinkedItemTypeID)
        End If
        Return _FK_DMS_ItemVersions_BackwardLinkedItemTypeID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_ItemVersions_ForwardLinkedItemTypeID() As SIS.DMS.dmsItemTypes
      Get
        If _FK_DMS_ItemVersions_ForwardLinkedItemTypeID Is Nothing Then
          _FK_DMS_ItemVersions_ForwardLinkedItemTypeID = SIS.DMS.dmsItemTypes.dmsItemTypesGetByID(_ForwardLinkedItemTypeID)
        End If
        Return _FK_DMS_ItemVersions_ForwardLinkedItemTypeID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_ItemVersions_ItemTypeID() As SIS.DMS.dmsItemTypes
      Get
        If _FK_DMS_ItemVersions_ItemTypeID Is Nothing Then
          _FK_DMS_ItemVersions_ItemTypeID = SIS.DMS.dmsItemTypes.dmsItemTypesGetByID(_ItemTypeID)
        End If
        Return _FK_DMS_ItemVersions_ItemTypeID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_ItemVersions_LinkedItemTypeID() As SIS.DMS.dmsItemTypes
      Get
        If _FK_DMS_ItemVersions_LinkedItemTypeID Is Nothing Then
          _FK_DMS_ItemVersions_LinkedItemTypeID = SIS.DMS.dmsItemTypes.dmsItemTypesGetByID(_LinkedItemTypeID)
        End If
        Return _FK_DMS_ItemVersions_LinkedItemTypeID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_ItemVersions_StatusID() As SIS.DMS.dmsStates
      Get
        If _FK_DMS_ItemVersions_StatusID Is Nothing Then
          _FK_DMS_ItemVersions_StatusID = SIS.DMS.dmsStates.dmsStatesGetByID(_StatusID)
        End If
        Return _FK_DMS_ItemVersions_StatusID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_ItemVersions_ConvertedStatusID() As SIS.DMS.dmsStates
      Get
        If _FK_DMS_ItemVersions_ConvertedStatusID Is Nothing Then
          _FK_DMS_ItemVersions_ConvertedStatusID = SIS.DMS.dmsStates.dmsStatesGetByID(_ConvertedStatusID)
        End If
        Return _FK_DMS_ItemVersions_ConvertedStatusID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_ItemVersions_CompanyID() As SIS.QCM.qcmCompanies
      Get
        If _FK_DMS_ItemVersions_CompanyID Is Nothing Then
          _FK_DMS_ItemVersions_CompanyID = SIS.QCM.qcmCompanies.qcmCompaniesGetByID(_CompanyID)
        End If
        Return _FK_DMS_ItemVersions_CompanyID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_ItemVersions_DepartmentID() As SIS.QCM.qcmDepartments
      Get
        If _FK_DMS_ItemVersions_DepartmentID Is Nothing Then
          _FK_DMS_ItemVersions_DepartmentID = SIS.QCM.qcmDepartments.qcmDepartmentsGetByID(_DepartmentID)
        End If
        Return _FK_DMS_ItemVersions_DepartmentID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_ItemVersions_DivisionID() As SIS.QCM.qcmDivisions
      Get
        If _FK_DMS_ItemVersions_DivisionID Is Nothing Then
          _FK_DMS_ItemVersions_DivisionID = SIS.QCM.qcmDivisions.qcmDivisionsGetByID(_DivisionID)
        End If
        Return _FK_DMS_ItemVersions_DivisionID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_ItemVersions_ProjectID() As SIS.QCM.qcmProjects
      Get
        If _FK_DMS_ItemVersions_ProjectID Is Nothing Then
          _FK_DMS_ItemVersions_ProjectID = SIS.QCM.qcmProjects.qcmProjectsGetByID(_ProjectID)
        End If
        Return _FK_DMS_ItemVersions_ProjectID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_ItemVersions_WBSID() As SIS.PAK.pakWBS
      Get
        If _FK_DMS_ItemVersions_WBSID Is Nothing Then
          _FK_DMS_ItemVersions_WBSID = SIS.PAK.pakWBS.pakWBSGetByID(_WBSID)
        End If
        Return _FK_DMS_ItemVersions_WBSID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_ItemVersions_ItemID() As SIS.DMS.dmsItems
      Get
        If _FK_DMS_ItemVersions_ItemID Is Nothing Then
          _FK_DMS_ItemVersions_ItemID = SIS.DMS.dmsItems.dmsItemsGetByID(_ItemID)
        End If
        Return _FK_DMS_ItemVersions_ItemID
      End Get
    End Property
    Public ReadOnly Property FK_DMS_ItemVersions_ActionBy() As SIS.QCM.qcmUsers
      Get
        If _FK_DMS_ItemVersions_ActionBy Is Nothing Then
          _FK_DMS_ItemVersions_ActionBy = SIS.QCM.qcmUsers.qcmUsersGetByID(_ActionBy)
        End If
        Return _FK_DMS_ItemVersions_ActionBy
      End Get
    End Property
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function dmsItemVersionsGetNewRecord() As SIS.DMS.dmsItemVersions
      Return New SIS.DMS.dmsItemVersions()
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function dmsItemVersionsGetByID(ByVal ItemID As Int32, ByVal SerialNo As Int32) As SIS.DMS.dmsItemVersions
      Dim Results As SIS.DMS.dmsItemVersions = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemVersionsSelectByID"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemID",SqlDbType.Int,ItemID.ToString.Length, ItemID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@SerialNo",SqlDbType.Int,SerialNo.ToString.Length, SerialNo)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NvarChar, 9, HttpContext.Current.Session("LoginID"))
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.DMS.dmsItemVersions(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function dmsItemVersionsSelectList(ByVal StartRowIndex As Integer, ByVal MaximumRows As Integer, ByVal OrderBy As String, ByVal SearchState As Boolean, ByVal SearchText As String, ByVal ItemID As Int32) As List(Of SIS.DMS.dmsItemVersions)
      Dim Results As List(Of SIS.DMS.dmsItemVersions) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          If SearchState Then
            Cmd.CommandText = "spdmsItemVersionsSelectListSearch"
            SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@KeyWord", SqlDbType.NVarChar, 250, SearchText)
          Else
            Cmd.CommandText = "spdmsItemVersionsSelectListFilteres"
            SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Filter_ItemID",SqlDbType.Int,10, IIf(ItemID = Nothing, 0,ItemID))
          End If
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StartRowIndex", SqlDbType.Int, -1, StartRowIndex)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaximumRows", SqlDbType.Int, -1, MaximumRows)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NvarChar, 9, HttpContext.Current.Session("LoginID"))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OrderBy", SqlDbType.NVarChar, 50, OrderBy)
          Cmd.Parameters.Add("@RecordCount", SqlDbType.Int)
          Cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Results = New List(Of SIS.DMS.dmsItemVersions)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.dmsItemVersions(Reader))
          End While
          Reader.Close()
          _RecordCount = Cmd.Parameters("@RecordCount").Value
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function dmsItemVersionsSelectCount(ByVal SearchState As Boolean, ByVal SearchText As String, ByVal ItemID As Int32) As Integer
      Return _RecordCount
    End Function
      'Select By ID One Record Filtered Overloaded GetByID
    <DataObjectMethod(DataObjectMethodType.Select)> _
    Public Shared Function dmsItemVersionsGetByID(ByVal ItemID As Int32, ByVal SerialNo As Int32, ByVal Filter_ItemID As Int32) As SIS.DMS.dmsItemVersions
      Return dmsItemVersionsGetByID(ItemID, SerialNo)
    End Function
    <DataObjectMethod(DataObjectMethodType.Insert, True)> _
    Public Shared Function dmsItemVersionsInsert(ByVal Record As SIS.DMS.dmsItemVersions) As SIS.DMS.dmsItemVersions
      Dim _Rec As SIS.DMS.dmsItemVersions = SIS.DMS.dmsItemVersions.dmsItemVersionsGetNewRecord()
      With _Rec
        .ItemID = Record.ItemID
        .UserID = Record.UserID
        .Description = Record.Description
        .FullDescription = Record.FullDescription
        .RevisionNo = Record.RevisionNo
        .EMailID = Record.EMailID
        .ItemTypeID = Record.ItemTypeID
        .StatusID = Record.StatusID
        .ConvertedStatusID = Record.ConvertedStatusID
        .IsMultiParent = Record.IsMultiParent
        .ParentItemID = Record.ParentItemID
        .IsMultiChild = Record.IsMultiChild
        .ChildItemID = Record.ChildItemID
        .CompanyID = Record.CompanyID
        .DivisionID = Record.DivisionID
        .DepartmentID = Record.DepartmentID
        .ProjectID = Record.ProjectID
        .WBSID = Record.WBSID
        .KeyWords = Record.KeyWords
        .CreatedBy = Record.CreatedBy
        .CreatedOn = Record.CreatedOn
        .IsMultiForward = Record.IsMultiForward
        .ForwardLinkedItemTypeID = Record.ForwardLinkedItemTypeID
        .ForwardLinkedItemID = Record.ForwardLinkedItemID
        .IsMultiLinked = Record.IsMultiLinked
        .LinkedItemTypeID = Record.LinkedItemTypeID
        .LinkedItemID = Record.LinkedItemID
        .IsMultiBackward = Record.IsMultiBackward
        .BackwardLinkedItemTypeID = Record.BackwardLinkedItemTypeID
        .BackwardLinkedItemID = Record.BackwardLinkedItemID
        .MaintainAllLog = Record.MaintainAllLog
        .MaintainStatusLog = Record.MaintainStatusLog
        .MaintainVersions = Record.MaintainVersions
        .IsgecVaultID = Record.IsgecVaultID
        .Publish = Record.Publish
        .CreateFolder = Record.CreateFolder
        .RenameFolder = Record.RenameFolder
        .DeleteFolder = Record.DeleteFolder
        .CreateFile = Record.CreateFile
        .DeleteFile = Record.DeleteFile
        .GrantAuthorization = Record.GrantAuthorization
        .BrowseList = Record.BrowseList
        .InheritFromParent = Record.InheritFromParent
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
      Return SIS.DMS.dmsItemVersions.InsertData(_Rec)
    End Function
    Public Shared Function InsertData(ByVal Record As SIS.DMS.dmsItemVersions) As SIS.DMS.dmsItemVersions
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemVersionsInsert"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemID",SqlDbType.Int,11, Record.ItemID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@UserID",SqlDbType.NVarChar,9, Iif(Record.UserID= "" ,Convert.DBNull, Record.UserID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Description",SqlDbType.NVarChar,251, Record.Description)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@FullDescription",SqlDbType.NVarChar,1001, Iif(Record.FullDescription= "" ,Convert.DBNull, Record.FullDescription))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@RevisionNo",SqlDbType.NVarChar,51, Record.RevisionNo)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@EMailID",SqlDbType.NVarChar,251, Iif(Record.EMailID= "" ,Convert.DBNull, Record.EMailID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemTypeID",SqlDbType.Int,11, Record.ItemTypeID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StatusID",SqlDbType.Int,11, Record.StatusID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ConvertedStatusID",SqlDbType.Int,11, Iif(Record.ConvertedStatusID= "" ,Convert.DBNull, Record.ConvertedStatusID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiParent",SqlDbType.Bit,3, Record.IsMultiParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ParentItemID",SqlDbType.Int,11, Iif(Record.ParentItemID= "" ,Convert.DBNull, Record.ParentItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiChild",SqlDbType.Bit,3, Record.IsMultiChild)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ChildItemID",SqlDbType.Int,11, Iif(Record.ChildItemID= "" ,Convert.DBNull, Record.ChildItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CompanyID",SqlDbType.NVarChar,7, Iif(Record.CompanyID= "" ,Convert.DBNull, Record.CompanyID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DivisionID",SqlDbType.NVarChar,7, Iif(Record.DivisionID= "" ,Convert.DBNull, Record.DivisionID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DepartmentID",SqlDbType.NVarChar,7, Iif(Record.DepartmentID= "" ,Convert.DBNull, Record.DepartmentID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ProjectID",SqlDbType.NVarChar,7, Iif(Record.ProjectID= "" ,Convert.DBNull, Record.ProjectID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@WBSID",SqlDbType.NVarChar,9, Iif(Record.WBSID= "" ,Convert.DBNull, Record.WBSID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@KeyWords",SqlDbType.NVarChar,251, Iif(Record.KeyWords= "" ,Convert.DBNull, Record.KeyWords))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedBy",SqlDbType.NVarChar,9, Record.CreatedBy)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedOn",SqlDbType.DateTime,21, Record.CreatedOn)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiForward",SqlDbType.Bit,3, Record.IsMultiForward)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ForwardLinkedItemTypeID",SqlDbType.Int,11, Iif(Record.ForwardLinkedItemTypeID= "" ,Convert.DBNull, Record.ForwardLinkedItemTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ForwardLinkedItemID",SqlDbType.Int,11, Iif(Record.ForwardLinkedItemID= "" ,Convert.DBNull, Record.ForwardLinkedItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiLinked",SqlDbType.Bit,3, Record.IsMultiLinked)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LinkedItemTypeID",SqlDbType.Int,11, Iif(Record.LinkedItemTypeID= "" ,Convert.DBNull, Record.LinkedItemTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LinkedItemID",SqlDbType.Int,11, Iif(Record.LinkedItemID= "" ,Convert.DBNull, Record.LinkedItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiBackward",SqlDbType.Bit,3, Record.IsMultiBackward)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@BackwardLinkedItemTypeID",SqlDbType.Int,11, Iif(Record.BackwardLinkedItemTypeID= "" ,Convert.DBNull, Record.BackwardLinkedItemTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@BackwardLinkedItemID",SqlDbType.Int,11, Iif(Record.BackwardLinkedItemID= "" ,Convert.DBNull, Record.BackwardLinkedItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaintainAllLog",SqlDbType.Bit,3, Record.MaintainAllLog)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaintainStatusLog",SqlDbType.Bit,3, Record.MaintainStatusLog)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaintainVersions",SqlDbType.Bit,3, Record.MaintainVersions)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsgecVaultID",SqlDbType.NVarChar,51, Iif(Record.IsgecVaultID= "" ,Convert.DBNull, Record.IsgecVaultID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Publish",SqlDbType.Bit,3, Record.Publish)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreateFolder",SqlDbType.Bit,3, Record.CreateFolder)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@RenameFolder",SqlDbType.Bit,3, Record.RenameFolder)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DeleteFolder",SqlDbType.Bit,3, Record.DeleteFolder)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreateFile",SqlDbType.Bit,3, Record.CreateFile)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DeleteFile",SqlDbType.Bit,3, Record.DeleteFile)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@GrantAuthorization",SqlDbType.Bit,3, Record.GrantAuthorization)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@BrowseList",SqlDbType.Bit,3, Record.BrowseList)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@InheritFromParent",SqlDbType.Bit,3, Record.InheritFromParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@SearchInParent",SqlDbType.Bit,3, Record.SearchInParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ShowInList",SqlDbType.Bit,3, Record.ShowInList)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Approved",SqlDbType.Bit,3, Record.Approved)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Rejected",SqlDbType.Bit,3, Record.Rejected)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ActionRemarks",SqlDbType.NVarChar,251, Iif(Record.ActionRemarks= "" ,Convert.DBNull, Record.ActionRemarks))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ActionBy",SqlDbType.NVarChar,9, Iif(Record.ActionBy= "" ,Convert.DBNull, Record.ActionBy))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ActionOn",SqlDbType.DateTime,21, Iif(Record.ActionOn= "" ,Convert.DBNull, Record.ActionOn))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsError",SqlDbType.Bit,3, Iif(Record.IsError= "" ,Convert.DBNull, Record.IsError))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ErrorMessage",SqlDbType.NVarChar,501, Iif(Record.ErrorMessage= "" ,Convert.DBNull, Record.ErrorMessage))
          Cmd.Parameters.Add("@Return_ItemID", SqlDbType.Int, 11)
          Cmd.Parameters("@Return_ItemID").Direction = ParameterDirection.Output
          Cmd.Parameters.Add("@Return_SerialNo", SqlDbType.Int, 11)
          Cmd.Parameters("@Return_SerialNo").Direction = ParameterDirection.Output
          Con.Open()
          Cmd.ExecuteNonQuery()
          Record.ItemID = Cmd.Parameters("@Return_ItemID").Value
          Record.SerialNo = Cmd.Parameters("@Return_SerialNo").Value
        End Using
      End Using
      Return Record
    End Function
    <DataObjectMethod(DataObjectMethodType.Update, True)> _
    Public Shared Function dmsItemVersionsUpdate(ByVal Record As SIS.DMS.dmsItemVersions) As SIS.DMS.dmsItemVersions
      Dim _Rec As SIS.DMS.dmsItemVersions = SIS.DMS.dmsItemVersions.dmsItemVersionsGetByID(Record.ItemID, Record.SerialNo)
      With _Rec
        .UserID = Record.UserID
        .Description = Record.Description
        .FullDescription = Record.FullDescription
        .RevisionNo = Record.RevisionNo
        .EMailID = Record.EMailID
        .ItemTypeID = Record.ItemTypeID
        .StatusID = Record.StatusID
        .ConvertedStatusID = Record.ConvertedStatusID
        .IsMultiParent = Record.IsMultiParent
        .ParentItemID = Record.ParentItemID
        .IsMultiChild = Record.IsMultiChild
        .ChildItemID = Record.ChildItemID
        .CompanyID = Record.CompanyID
        .DivisionID = Record.DivisionID
        .DepartmentID = Record.DepartmentID
        .ProjectID = Record.ProjectID
        .WBSID = Record.WBSID
        .KeyWords = Record.KeyWords
        .CreatedBy = Record.CreatedBy
        .CreatedOn = Record.CreatedOn
        .IsMultiForward = Record.IsMultiForward
        .ForwardLinkedItemTypeID = Record.ForwardLinkedItemTypeID
        .ForwardLinkedItemID = Record.ForwardLinkedItemID
        .IsMultiLinked = Record.IsMultiLinked
        .LinkedItemTypeID = Record.LinkedItemTypeID
        .LinkedItemID = Record.LinkedItemID
        .IsMultiBackward = Record.IsMultiBackward
        .BackwardLinkedItemTypeID = Record.BackwardLinkedItemTypeID
        .BackwardLinkedItemID = Record.BackwardLinkedItemID
        .MaintainAllLog = Record.MaintainAllLog
        .MaintainStatusLog = Record.MaintainStatusLog
        .MaintainVersions = Record.MaintainVersions
        .IsgecVaultID = Record.IsgecVaultID
        .Publish = Record.Publish
        .CreateFolder = Record.CreateFolder
        .RenameFolder = Record.RenameFolder
        .DeleteFolder = Record.DeleteFolder
        .CreateFile = Record.CreateFile
        .DeleteFile = Record.DeleteFile
        .GrantAuthorization = Record.GrantAuthorization
        .BrowseList = Record.BrowseList
        .InheritFromParent = Record.InheritFromParent
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
      Return SIS.DMS.dmsItemVersions.UpdateData(_Rec)
    End Function
    Public Shared Function UpdateData(ByVal Record As SIS.DMS.dmsItemVersions) As SIS.DMS.dmsItemVersions
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemVersionsUpdate"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_ItemID",SqlDbType.Int,11, Record.ItemID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_SerialNo",SqlDbType.Int,11, Record.SerialNo)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemID",SqlDbType.Int,11, Record.ItemID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@UserID",SqlDbType.NVarChar,9, Iif(Record.UserID= "" ,Convert.DBNull, Record.UserID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Description",SqlDbType.NVarChar,251, Record.Description)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@FullDescription",SqlDbType.NVarChar,1001, Iif(Record.FullDescription= "" ,Convert.DBNull, Record.FullDescription))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@RevisionNo",SqlDbType.NVarChar,51, Record.RevisionNo)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@EMailID",SqlDbType.NVarChar,251, Iif(Record.EMailID= "" ,Convert.DBNull, Record.EMailID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ItemTypeID",SqlDbType.Int,11, Record.ItemTypeID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StatusID",SqlDbType.Int,11, Record.StatusID)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ConvertedStatusID",SqlDbType.Int,11, Iif(Record.ConvertedStatusID= "" ,Convert.DBNull, Record.ConvertedStatusID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiParent",SqlDbType.Bit,3, Record.IsMultiParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ParentItemID",SqlDbType.Int,11, Iif(Record.ParentItemID= "" ,Convert.DBNull, Record.ParentItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiChild",SqlDbType.Bit,3, Record.IsMultiChild)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ChildItemID",SqlDbType.Int,11, Iif(Record.ChildItemID= "" ,Convert.DBNull, Record.ChildItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CompanyID",SqlDbType.NVarChar,7, Iif(Record.CompanyID= "" ,Convert.DBNull, Record.CompanyID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DivisionID",SqlDbType.NVarChar,7, Iif(Record.DivisionID= "" ,Convert.DBNull, Record.DivisionID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DepartmentID",SqlDbType.NVarChar,7, Iif(Record.DepartmentID= "" ,Convert.DBNull, Record.DepartmentID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ProjectID",SqlDbType.NVarChar,7, Iif(Record.ProjectID= "" ,Convert.DBNull, Record.ProjectID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@WBSID",SqlDbType.NVarChar,9, Iif(Record.WBSID= "" ,Convert.DBNull, Record.WBSID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@KeyWords",SqlDbType.NVarChar,251, Iif(Record.KeyWords= "" ,Convert.DBNull, Record.KeyWords))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedBy",SqlDbType.NVarChar,9, Record.CreatedBy)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreatedOn",SqlDbType.DateTime,21, Record.CreatedOn)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiForward",SqlDbType.Bit,3, Record.IsMultiForward)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ForwardLinkedItemTypeID",SqlDbType.Int,11, Iif(Record.ForwardLinkedItemTypeID= "" ,Convert.DBNull, Record.ForwardLinkedItemTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ForwardLinkedItemID",SqlDbType.Int,11, Iif(Record.ForwardLinkedItemID= "" ,Convert.DBNull, Record.ForwardLinkedItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiLinked",SqlDbType.Bit,3, Record.IsMultiLinked)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LinkedItemTypeID",SqlDbType.Int,11, Iif(Record.LinkedItemTypeID= "" ,Convert.DBNull, Record.LinkedItemTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LinkedItemID",SqlDbType.Int,11, Iif(Record.LinkedItemID= "" ,Convert.DBNull, Record.LinkedItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsMultiBackward",SqlDbType.Bit,3, Record.IsMultiBackward)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@BackwardLinkedItemTypeID",SqlDbType.Int,11, Iif(Record.BackwardLinkedItemTypeID= "" ,Convert.DBNull, Record.BackwardLinkedItemTypeID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@BackwardLinkedItemID",SqlDbType.Int,11, Iif(Record.BackwardLinkedItemID= "" ,Convert.DBNull, Record.BackwardLinkedItemID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaintainAllLog",SqlDbType.Bit,3, Record.MaintainAllLog)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaintainStatusLog",SqlDbType.Bit,3, Record.MaintainStatusLog)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaintainVersions",SqlDbType.Bit,3, Record.MaintainVersions)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsgecVaultID",SqlDbType.NVarChar,51, Iif(Record.IsgecVaultID= "" ,Convert.DBNull, Record.IsgecVaultID))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Publish",SqlDbType.Bit,3, Record.Publish)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreateFolder",SqlDbType.Bit,3, Record.CreateFolder)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@RenameFolder",SqlDbType.Bit,3, Record.RenameFolder)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DeleteFolder",SqlDbType.Bit,3, Record.DeleteFolder)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@CreateFile",SqlDbType.Bit,3, Record.CreateFile)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@DeleteFile",SqlDbType.Bit,3, Record.DeleteFile)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@GrantAuthorization",SqlDbType.Bit,3, Record.GrantAuthorization)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@BrowseList",SqlDbType.Bit,3, Record.BrowseList)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@InheritFromParent",SqlDbType.Bit,3, Record.InheritFromParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@SearchInParent",SqlDbType.Bit,3, Record.SearchInParent)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ShowInList",SqlDbType.Bit,3, Record.ShowInList)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Approved",SqlDbType.Bit,3, Record.Approved)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Rejected",SqlDbType.Bit,3, Record.Rejected)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ActionRemarks",SqlDbType.NVarChar,251, Iif(Record.ActionRemarks= "" ,Convert.DBNull, Record.ActionRemarks))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ActionBy",SqlDbType.NVarChar,9, Iif(Record.ActionBy= "" ,Convert.DBNull, Record.ActionBy))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ActionOn",SqlDbType.DateTime,21, Iif(Record.ActionOn= "" ,Convert.DBNull, Record.ActionOn))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@IsError",SqlDbType.Bit,3, Iif(Record.IsError= "" ,Convert.DBNull, Record.IsError))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@ErrorMessage",SqlDbType.NVarChar,501, Iif(Record.ErrorMessage= "" ,Convert.DBNull, Record.ErrorMessage))
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
    Public Shared Function dmsItemVersionsDelete(ByVal Record As SIS.DMS.dmsItemVersions) As Int32
      Dim _Result as Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spdmsItemVersionsDelete"
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Original_ItemID",SqlDbType.Int,Record.ItemID.ToString.Length, Record.ItemID)
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
