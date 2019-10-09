Imports System
Imports System.Web
Imports System.Xml
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.Web.Script.Serialization
Namespace SIS.DMS
  Public Class API
    Public Class apiUser
      Implements IDisposable
      Public Property IsAdmin As Boolean = False
      Public Property User As Integer = 0
      Public Property CardNo As String = ""
      Public Property UserName As String = ""
      Public Property AllowedTypes As AllowedType = Nothing
      Public Sub Load(ID As Integer)
        User = ID
        Dim usr As apiItem = apiItem.GetItem(User)
        CardNo = usr.UserID
        UserName = usr.Description
        IsAdmin = GetIsAdmin(User)
        AllowedTypes = New AllowedType()
        AllowedTypes.Load(User, IsAdmin)
      End Sub
      Sub New()
        'dummy
      End Sub
      Public Shared Function GetIsAdmin(ID As Integer) As Boolean
        Dim mRet As Boolean = False
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Dim Sql As String = ""
          Sql &= " select IsNull(count(ItemID),0) as cnt from dms_MultiItems where multitypeid=" & enumMultiTypes.Administrator & " and ItemID=" & ID
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            Con.Open()
            Dim x As Integer = Cmd.ExecuteScalar
            If x > 0 Then mRet = True
          End Using
        End Using
        Return mRet
      End Function
#Region "IDisposable Support"
      Private disposedValue As Boolean ' To detect redundant calls

      ' IDisposable
      Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
          If disposing Then
            ' TODO: dispose managed state (managed objects).
            AllowedTypes.Dispose()
          End If
          AllowedTypes = Nothing
          ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
          ' TODO: set large fields to null.
        End If
        disposedValue = True
      End Sub

      ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
      'Protected Overrides Sub Finalize()
      '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
      '    Dispose(False)
      '    MyBase.Finalize()
      'End Sub

      ' This code added by Visual Basic to correctly implement the disposable pattern.
      Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
      End Sub
#End Region
    End Class
    Public Class AllowedType
      Implements IDisposable
      Public Property IsAdmin As Boolean = False
      Public Property User As Integer = 0
      Public Property apiTypes As New List(Of apiType)
      Public Sub Load(ID As Integer, Admin As Boolean)
        User = ID
        IsAdmin = Admin
        setApiTypes(ID)
      End Sub
      Sub New()
        'dummy
      End Sub
      Private Sub setApiTypes(ID As Integer)
        apiTypes = New List(Of apiType)
        '1. Select Types allowed to user
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Dim Sql As String = ""
          Sql &= " select * from dms_ItemTypes where Active=1 and ItemTypeID in (select MultiItemTypeID from dms_multiItems where multitypeid=" & enumMultiTypes.AllowedTypes & " and ItemID=" & ID & ") order by Sequence"
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            Con.Open()
            Dim rd As SqlDataReader = Cmd.ExecuteReader
            While rd.Read
              apiTypes.Add(New apiType(rd))
            End While
          End Using
        End Using
        '2. Folder should be allowed by default
        If apiTypes.Find(Function(x) x.ItemTypeID = enumItemTypes.Folder) Is Nothing Then
          apiTypes.Add(apiType.GetByID(enumItemTypes.Folder))
        End If
        For Each x As apiType In apiTypes
          x.User = ID
          x.Load(ID, IsAdmin)
        Next
      End Sub

#Region "IDisposable Support"
      Private disposedValue As Boolean ' To detect redundant calls

      ' IDisposable
      Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
          If disposing Then
            ' TODO: dispose managed state (managed objects).
            For Each x As apiType In _apiTypes
              x.Dispose()
              x = Nothing
            Next
            apiTypes = Nothing
          End If

          ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
          ' TODO: set large fields to null.
        End If
        disposedValue = True
      End Sub

      ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
      'Protected Overrides Sub Finalize()
      '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
      '    Dispose(False)
      '    MyBase.Finalize()
      'End Sub

      ' This code added by Visual Basic to correctly implement the disposable pattern.
      Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
      End Sub
#End Region
    End Class

    Public Class apiType
      Implements IDisposable
      Public Property IsAdmin As Boolean = False
      Public Property User As Integer = 0
      Public Property ItemTypeID As Int32 = 0
      Public Property Description As String = ""
      Public Property Active As Boolean = False
      Public Property UQuality As Boolean = False
      Public Property IQuality As Boolean = False
      Public Property InheritFromParent As Boolean = False
      Public Property SearchInParent As Boolean = False
      Public Property ShowInList As Boolean = False
      Public Property Sequence As Int32 = 0
      Public Property AuthorizedChilds As AuthorizedChild = Nothing
      Public Sub Load(ID As Integer, Admin As Boolean)
        User = ID
        IsAdmin = Admin
        AuthorizedChilds = New AuthorizedChild()
        AuthorizedChilds.Load(ID, IsAdmin, ItemTypeID)
      End Sub
      Sub New(rd As SqlDataReader)
        API.NewObj(Me, rd)
      End Sub
      Sub New()
      End Sub
      Public Shared Function GetByID(ID As Integer) As apiType
        Dim tmp As apiType = Nothing
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Dim Sql As String = ""
          Sql &= " select * from dms_ItemTypes where ItemTypeID=" & ID
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            Con.Open()
            Dim rd As SqlDataReader = Cmd.ExecuteReader
            While rd.Read
              tmp = New apiType(rd)
            End While
          End Using
        End Using
        Return tmp
      End Function

#Region "IDisposable Support"
      Private disposedValue As Boolean ' To detect redundant calls

      ' IDisposable
      Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
          If disposing Then
            ' TODO: dispose managed state (managed objects).
            AuthorizedChilds.Dispose()
          End If
          AuthorizedChilds = Nothing
          ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
          ' TODO: set large fields to null.
        End If
        disposedValue = True
      End Sub

      ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
      'Protected Overrides Sub Finalize()
      '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
      '    Dispose(False)
      '    MyBase.Finalize()
      'End Sub

      ' This code added by Visual Basic to correctly implement the disposable pattern.
      Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
      End Sub
#End Region
    End Class
    Public Class AuthorizedChild
      Implements IDisposable
      Public Property IsAdmin As Boolean = False
      Public Property User As Integer = 0
      Public Property ItemTypeID As Integer = 0
      Public Property apiItems As New List(Of apiItem)

      Public Sub Load(ID As Integer, Admin As Boolean, ITypeID As Integer)
        User = ID
        IsAdmin = Admin
        ItemTypeID = ITypeID
        setAPIItems(ID, ITypeID)
      End Sub
      Private Sub setAPIItems(ID As Integer, ItemTypeID As Integer)
        apiItems = New List(Of apiItem)
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Dim Sql As String = ""
          Sql &= GetItemSelectStatement()
          Sql &= "   LEFT OUTER JOIN DMS_MultiItems on DMS_MultiItems.MultiItemID = DMS_Items.ItemID "
          Sql &= "   WHERE "
          Sql &= "   [DMS_MultiItems].[MultiTypeID] IN (" & enumMultiTypes.Linked & "," & enumMultiTypes.Authorized & ")"
          Sql &= "   AND [DMS_MultiItems].[ItemID]=" & ID
          Sql &= "   AND [DMS_MultiItems].[MultiItemTypeID]=" & ItemTypeID
          Sql &= "   ORDER BY [DMS_Items].[ItemTypeID], [DMS_Items].[ItemID] "
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            Con.Open()
            Dim rd As SqlDataReader = Cmd.ExecuteReader
            While rd.Read
              apiItems.Add(New apiItem(rd))
            End While
          End Using
        End Using
        For Each x As apiItem In apiItems
          x.User = ID
          x.Init(ID, IsAdmin, x.ItemID)
        Next
      End Sub
      Sub New()
        'dummy
      End Sub
#Region "IDisposable Support"
      Private disposedValue As Boolean ' To detect redundant calls

      ' IDisposable
      Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
          If disposing Then
            ' TODO: dispose managed state (managed objects).
            For Each x As apiItem In apiItems
              x.Dispose()
            Next
          End If
          apiItems = Nothing
          ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
          ' TODO: set large fields to null.
        End If
        disposedValue = True
      End Sub

      ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
      'Protected Overrides Sub Finalize()
      '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
      '    Dispose(False)
      '    MyBase.Finalize()
      'End Sub

      ' This code added by Visual Basic to correctly implement the disposable pattern.
      Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
      End Sub
#End Region
    End Class
    Public Class apiItem
      Implements IDisposable

      Public Property IsAdmin As Boolean = False
      Public Property User As Integer = 0
      Public Property ChildCount As Integer = 0
      Public Property apiItems As New List(Of apiItem)

      Public Property CardNo As String = ""
      Public Property UserName As String = ""
      Public Property AllowedTypes As AllowedType = Nothing


#Region "********GetICON***********"
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

#Region "********Properties**********"
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

#End Region

#Region "********Additional Properties*********"

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
      Public Sub Init(ID As Integer, Admin As Boolean, Parent As Integer)
        User = ID
        IsAdmin = Admin
        InitApiItems(ID, IsAdmin, ItemID)
        'Reset Only Count is Init
        apiItems = New List(Of apiItem)
      End Sub
      Public Sub Load(ID As Integer, Admin As Boolean, Parent As Integer)
        If ItemTypeID <> enumItemTypes.User Then
          User = ID
          IsAdmin = Admin
          InitApiItems(ID, IsAdmin, ItemID)
        Else
          User = ID
          Dim usr As apiItem = apiItem.GetItem(User)
          CardNo = usr.UserID
          UserName = usr.Description
          IsAdmin = apiUser.GetIsAdmin(User)
          AllowedTypes = New AllowedType()
          AllowedTypes.Load(User, IsAdmin)
        End If
      End Sub
      Private Sub InitApiItems(ID As Integer, Admin As Boolean, ItemID As Integer)
        apiItems = New List(Of apiItem)
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
              Sql &= "   OR ([DMS_Items].[ItemTypeID]=" & enumItemTypes.File & " AND [DMS_Items].[ItemID]=" & ItemID & ")"
              Sql &= "   ORDER By [DMS_Items].[ItemTypeID], [DMS_Items].[ItemID]"
          End Select
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            Con.Open()
            Dim Reader As SqlDataReader = Cmd.ExecuteReader()
            While (Reader.Read())
              apiItems.Add(New apiItem(Reader))
            End While
            Reader.Close()
          End Using
        End Using
        ChildCount = apiItems.Count
        For Each x As apiItem In apiItems
          x.User = ID
          x.IsAdmin = Admin
        Next
      End Sub
      Public Shared Function GetItem(ByVal ItemID As String) As apiItem
        Dim Results As apiItem = Nothing
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Dim Sql As String = GetItemSelectStatement(True)
          Sql &= "   WHERE "
          Sql &= "   [DMS_Items].[ItemID]=" & ItemID
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            Con.Open()
            Dim Reader As SqlDataReader = Cmd.ExecuteReader()
            If Reader.Read() Then
              Results = New apiItem(Reader)
            End If
            Reader.Close()
          End Using
        End Using
        Return Results
      End Function

      Sub New(rd As SqlDataReader)
        API.NewObj(Me, rd)
      End Sub
      Sub New()
        'dummy
      End Sub

#Region "IDisposable Support"
      Private disposedValue As Boolean ' To detect redundant calls

      ' IDisposable
      Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
          If disposing Then
            ' TODO: dispose managed state (managed objects).
            For Each x As apiItem In apiItems
              x.Dispose()
            Next
            AllowedTypes.Dispose()
          End If
          apiItems = Nothing
          AllowedTypes = Nothing
          ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
          ' TODO: set large fields to null.
        End If
        disposedValue = True
      End Sub

      ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
      'Protected Overrides Sub Finalize()
      '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
      '    Dispose(False)
      '    MyBase.Finalize()
      'End Sub

      ' This code added by Visual Basic to correctly implement the disposable pattern.
      Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
      End Sub
#End Region

    End Class
    Partial Public Class dmsRequest
      Public Property dmsBase As String = "Root"
      Public Property dmsItem As String = ""
      Public Property dmsType As String = ""
      Public Property dmsID As String = ""
      Public Property dmsLoaded As String = "0"
      Public Property dmsExpended As String = "0"
      Public Property dmsBottom As String = "0"
      Public Property dmsIndent As String = "0"
      Public Property byParent As Boolean = False
      Public Shared Function getDMSRequest(ByVal context As HttpContext) As SIS.DMS.Handlers.dmsRequest
        Dim tmp As New SIS.DMS.Handlers.dmsRequest
        With tmp
          .dmsBase = context.Request.QueryString("Base")
          .dmsItem = context.Request.QueryString("Item")
          .dmsType = context.Request.QueryString("Type")
          .dmsID = context.Request.QueryString("ID")
          .dmsLoaded = context.Request.QueryString("Loaded")
          .dmsExpended = context.Request.QueryString("Expended")
          .dmsBottom = context.Request.QueryString("Bottom")
          .dmsIndent = context.Request.QueryString("Indent")
        End With
        Return tmp
      End Function
    End Class

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
      Sql &= "   INNER JOIN [aspnet_users] AS [aspnet_users2] "
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

    Public Shared Function NewObj(this As Object, Reader As SqlDataReader) As Object
      Try
        For Each pi As System.Reflection.PropertyInfo In this.GetType.GetProperties
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
                      CallByName(this, pi.Name, CallType.Let, "0.00")
                    Case "bit"
                      CallByName(this, pi.Name, CallType.Let, Boolean.FalseString)
                    Case Else
                      CallByName(this, pi.Name, CallType.Let, String.Empty)
                  End Select
                Else
                  CallByName(this, pi.Name, CallType.Let, Reader(pi.Name))
                End If
              End If
            Catch ex As Exception
            End Try
          End If
        Next
      Catch ex As Exception
        Return Nothing
      End Try
      Return this
    End Function
  End Class
End Namespace
