<%@ WebService Language="VB" Class="dmsService" %>

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Serialization

<System.Web.Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
Public Class dmsService
  Inherits System.Web.Services.WebService
  Private dmsUser As SIS.DMS.UI.apiItem = Nothing
#Region " ValidateSession "
  Private Function ValidateSession() As Boolean
    dmsUser = SIS.DMS.UI.GetSessionUserObject()
    If dmsUser Is Nothing Then Return False
    Return True
  End Function
#End Region
#Region " SessionExpired "
  Private Function SessionExpired() As String
    Return New JavaScriptSerializer().Serialize(New With {
         .err = True,
         .msg = "Session Expired, Login again."
     })
  End Function
#End Region
#Region " Error Do Nothing  "
  Private Function ErrorDoNothing() As String
    Dim mRet As New SIS.DMS.UI.apiResponse
    mRet.err = True
    Return New JavaScriptSerializer().Serialize(mRet)
  End Function
#End Region
#Region " Return Error Message  "
  Private Function ErrorMessage(strMsg As String) As String
    Dim mRet As New SIS.DMS.UI.apiResponse
    mRet.err = True
    mRet.msg = strMsg
    Return New JavaScriptSerializer().Serialize(mRet)

    'Return New JavaScriptSerializer().Serialize(New With {
    '     .err = True,
    '     .msg = strMsg
    ' })
  End Function
#End Region
#Region " Return Default  "
  Private Function DoDefault(Optional strHTML As String = "") As String
    Dim mRet As New SIS.DMS.UI.apiResponse
    If strHTML <> "" Then mRet.strHTML.Add(strHTML)
    Return New JavaScriptSerializer().Serialize(mRet)
  End Function
#End Region
#Region " LoadRoot "
  <WebMethod(EnableSession:=True)>
  Public Function LoadRoot(ByVal context As String) As String
    If Not ValidateSession() Then Return SessionExpired()
    Dim mRet As SIS.DMS.UI.apiResponse = SIS.DMS.UI.Core.strTree(dmsUser.ItemID, context)
    Return New JavaScriptSerializer().Serialize(mRet)
  End Function
#End Region
#Region " LoadAny "
  <WebMethod(EnableSession:=True)>
  Public Function LoadAny(ByVal context As String) As String
    If Not ValidateSession() Then Return SessionExpired()
    Dim Req As SIS.DMS.UI.apiRequest = SIS.DMS.UI.apiRequest.GetRequest(context)

    Dim mRet As SIS.DMS.UI.apiResponse = Nothing

    If Req.Base.ToUpper = "USER" Then
      mRet = SIS.DMS.UI.Core.strTree(Req.Item, context)
    End If
    If Req.Base.ToUpper = "TYPE" Then
      mRet = SIS.DMS.UI.Core.strTree(Req.Type, context)
    End If
    If Req.Base.ToUpper = "ITEM" Then
      mRet = SIS.DMS.UI.Core.strTree(Req.Item, context)
    End If
    Return New JavaScriptSerializer().Serialize(mRet)
  End Function
#End Region

#Region " menuClicked "
  Private Function doPublish(Req As SIS.DMS.UI.apiRequest) As String
    Dim mayDo As Boolean = True
    Dim mRet As New SIS.DMS.UI.apiResponse
    Dim reqItem As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
    Dim wfs As List(Of SIS.DMS.UI.apiItem) = SIS.DMS.UI.GetAssociatedWF(Req.Item)
    Dim wf As SIS.DMS.UI.apiItem = Nothing
    For Each x As SIS.DMS.UI.apiItem In wfs
      If reqItem.StatusID = x.ConvertedStatusID Then
        wf = x
        Exit For
      End If
    Next
    If wf IsNot Nothing AndAlso reqItem.StatusID <> wf.ConvertedStatusID Then
      Return ErrorMessage("File Status is not WF status, Cannot initiate workflow.")
    Else
      If Not dmsUser.IsAdmin Then
        If dmsUser.Publish = enumQualities.NotAllowed Then
          Return ErrorMessage("User permission: Not Allowed.")
        Else
          Dim parentFolder As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetParentFolder(reqItem.ItemID)
          Select Case parentFolder.Publish
            Case enumQualities.NotAllowed
              Return ErrorMessage("Folder Setting: Can not be Published.")
            Case enumQualities.CreatorOnly
              If reqItem.CreatedBy <> dmsUser.UserID Then
                Return ErrorMessage("Folder Setting: Can be Published by creater only.")
              End If
          End Select
          If reqItem.StatusID <> enumDMSStates.Created Then
            Return ErrorMessage("Status Error: Can not be Published.")
          ElseIf reqItem.StatusID = enumDMSStates.Published Then
            Return ErrorMessage("Status Error: Already Published.")
          End If
        End If
      End If
    End If
    If mayDo Then
      If wf IsNot Nothing Then
        SIS.DMS.UI.Core.InitiateWF(reqItem, dmsUser, wf)
      Else
        'Publish Item
        'Check Mandatory Requirements Before Publish
        SIS.DMS.UI.Core.PublishItem(reqItem, dmsUser)
      End If
      mRet.nofrm = True
      mRet.okmsg = "Published successfully"
      mRet.Target = Req.Parent
    End If
    Return New JavaScriptSerializer().Serialize(mRet)
  End Function
  <WebMethod(EnableSession:=True)>
  Public Function menuClicked(context As String, options As String) As String
    If Not ValidateSession() Then Return SessionExpired()
    Dim Req As SIS.DMS.UI.apiRequest = SIS.DMS.UI.apiRequest.GetRequest(context)
    Dim Opt As SIS.DMS.UI.apiOptions = SIS.DMS.UI.apiOptions.GetOptions(options)

    Dim mRet As New SIS.DMS.UI.apiResponse

    If Req.Base.ToUpper = "USER" And Req.Parent = "trRoot" Then
      'context menu should not be opened at Root User
      Return ErrorDoNothing()
    End If

    Select Case Opt.Options
      Case "Attach Workflow", "Remove Workflow"
        Dim reqItem As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
        If Not dmsUser.IsAdmin Then
          If dmsUser.CreateFolder = enumQualities.AllAuthorized Then
            Return ErrorMessage("User permission: Not Allowed.")
          Else
            If dmsUser.RenameFolder = enumQualities.NotAllowed Then
              Return ErrorMessage("User permission: Not Allowed.")
            Else
              If reqItem.RenameFolder = enumQualities.NotAllowed Then
                Return ErrorMessage("Folder Setting: Not Allowed.")
              Else
                If reqItem.RenameFolder = enumQualities.CreatorOnly Then
                  If reqItem.CreatedBy <> dmsUser.UserID Then
                    Return ErrorMessage("Folder Setting: By Creater Only.")
                  End If
                End If
              End If
            End If
          End If
        End If
        mRet.strHTML.Add(New JavaScriptSerializer().Serialize(reqItem))
      Case "Publish"
        Return doPublish(Req)
      Case "Approve / Acknowledge"
        Dim reqItem As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
        mRet.err = False
        mRet.msg = ""
        mRet.Target = ""
        mRet.strHTML.Add(New JavaScriptSerializer().Serialize(reqItem))
        Return New JavaScriptSerializer().Serialize(mRet)
      Case "Authorize to User"
        Dim reqItem As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
        If Not dmsUser.IsAdmin Then
          If dmsUser.GrantAuthorization = enumQualities.NotAllowed Then
            Return ErrorMessage("User permission: Not Allowed.")
          Else
            Select Case reqItem.GrantAuthorization
              Case enumQualities.NotAllowed
                Return ErrorMessage("Folder Setting: Can not Granted to any other.")
              Case enumQualities.CreatorOnly
                If reqItem.CreatedBy <> dmsUser.UserID Then
                  Return ErrorMessage("Folder Setting: Can be Granted by Creater Only.")
                End If
            End Select
          End If
        End If
        Return DoDefault(New JavaScriptSerializer().Serialize(reqItem))
      Case "Upload Files", "Upload Ref. Files"
        Dim mayGo As Boolean = True
        Try
          If Req.Item = 0 Then
            Return ErrorMessage("File can not be uploaded at selected node.")
          End If
          Dim reqItem As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
          If Not dmsUser.IsAdmin Then
            If dmsUser.CreateFile = enumQualities.NotAllowed Then
              Return ErrorMessage("User permission: Not Allowed.")
            Else
              Select Case reqItem.CreateFile
                Case enumQualities.NotAllowed
                  Return ErrorMessage("Folder Setting: Upload File NOT Allowed.")
                Case enumQualities.CreatorOnly
                  If reqItem.CreatedBy <> dmsUser.UserID Then
                    Return ErrorMessage("Folder Setting: Files can be uploaded by Creater Only.")
                  End If
              End Select
              If reqItem.ItemTypeID = enumItemTypes.File Then
                Dim FxStatus As SIS.DMS.UI.apiStatus = reqItem.Status
                If FxStatus.Locked Then
                  Return ErrorMessage("File is NOT Updatable.")
                Else
                  If reqItem.StatusID = enumDMSStates.CheckedOut Then
                    If reqItem.CreatedBy <> dmsUser.UserID Then
                      Return ErrorMessage("File NOT Updatable - checkedout by other user")
                    End If
                  End If
                End If
              End If
            End If
          End If
        Catch ex As Exception
          Return ErrorMessage("Error: During Checking Authorization: " & ex.Message)
        End Try
        If mayGo Then
          Return DoDefault()
        End If
      Case "Edit", "Edit and Publish"      '"Edit User of Group" discontinued only Delete UserGroupUser is used
        Dim mayEdit As Boolean = True
        Dim reqItem As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
        Select Case Req.Type
          Case enumItemTypes.File
            If Not dmsUser.IsAdmin Then
              If dmsUser.CreateFile = enumQualities.NotAllowed Then
                Return ErrorMessage("User permission: Not Allowed.")
              Else
                Dim parentFolder As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetParentFolder(reqItem.ItemID)
                Select Case parentFolder.CreateFile
                  Case enumQualities.NotAllowed
                    Return ErrorMessage("Folder Setting: Editing NOT Allowed.")
                  Case enumQualities.CreatorOnly
                    If reqItem.CreatedBy <> dmsUser.UserID Then
                      Return ErrorMessage("Folder Setting: Can be Edited by Creater Only.")
                    End If
                End Select
                Dim FxStatus As SIS.DMS.UI.apiStatus = reqItem.Status()
                If Not FxStatus.OpenType Then
                  'OpenType Field is Used for IsEditable
                  Return ErrorMessage("File is NOT Updatable.")
                Else
                  If reqItem.StatusID = enumDMSStates.CheckedOut Then
                    If reqItem.CreatedBy <> dmsUser.UserID Then
                      Return ErrorMessage("File NOT Updatable - checkedout by other user")
                    End If
                  End If
                End If
              End If
            End If
          Case enumItemTypes.Folder
            If Not dmsUser.IsAdmin Then
              If dmsUser.RenameFolder = enumQualities.NotAllowed Then
                Return ErrorMessage("User permission: Not Allowed.")
              Else
                Select Case reqItem.RenameFolder
                  Case enumQualities.NotAllowed
                    Return ErrorMessage("Folder Setting: Editing NOT Allowed.")
                  Case enumQualities.CreatorOnly
                    If reqItem.CreatedBy <> dmsUser.UserID Then
                      Return ErrorMessage("Folder Setting: Can be Edited by Creater Only.")
                    End If
                End Select
              End If
            End If
          Case Else
            mayEdit = True
        End Select
        If mayEdit Then
          mRet.err = False
          mRet.msg = ""
          mRet.Target = ""
          mRet.strHTML.Add(New JavaScriptSerializer().Serialize(reqItem))
          Return New JavaScriptSerializer().Serialize(mRet)
        End If
      Case "Delete User from Group"
        Dim reqItem As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
        mRet = SIS.DMS.UI.DeleteUserAuthByGroup(reqItem.ParentItemID, reqItem.LinkedItemID)
        If Not mRet.err Then
          SIS.DMS.UI.DirectDeleteItem(Req.Item)
        End If
        mRet.nofrm = True
        mRet.okmsg = "Deleted successfully"
        mRet.Target = Req.Parent
        Return New JavaScriptSerializer().Serialize(mRet)
      Case "Delete Authorization"
        Dim reqItem As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
        If reqItem.ForwardLinkedItemTypeID = enumItemTypes.Folder AndAlso reqItem.LinkedItemTypeID = enumItemTypes.UserGroup Then
          mRet = SIS.DMS.UI.DeleteGroupAuth(reqItem.ForwardLinkedItemID, reqItem.LinkedItemID)
          mRet = SIS.DMS.UI.DeleteMultiItems(reqItem.LinkedItemID, enumMultiTypes.Authorized, reqItem.ForwardLinkedItemTypeID, reqItem.ForwardLinkedItemID)
        ElseIf reqItem.ForwardLinkedItemTypeID = enumItemTypes.Folder AndAlso reqItem.LinkedItemTypeID = enumItemTypes.User Then
          mRet = SIS.DMS.UI.DeleteMultiItems(reqItem.LinkedItemID, enumMultiTypes.Authorized, reqItem.ForwardLinkedItemTypeID, reqItem.ForwardLinkedItemID)
        End If
        If Not mRet.err Then
          mRet = SIS.DMS.UI.DeleteMultiItems("", enumMultiTypes.Created, enumItemTypes.Authorization, reqItem.ItemID)
          If Not mRet.err Then
            SIS.DMS.UI.DirectDeleteItem(Req.Item)
          End If
        End If
        mRet.nofrm = True
        mRet.okmsg = "Deleted successfully"
        mRet.Target = Req.Parent
      Case "Delete"
        '===============
        Select Case Req.Type
          Case enumItemTypes.Searches
            Try
              SIS.DMS.dmsSearchData.DeleteLastResults(Req.Item)
              SIS.DMS.UI.DeleteItem(Req.Type, Req.Item)
              mRet.err = False
              mRet.msg = ""
              mRet.Target = Req.Parent
              mRet.nofrm = True
              mRet.okmsg = "Deleted successfully"
              Return New JavaScriptSerializer().Serialize(mRet)
            Catch ex As Exception
              Return ErrorMessage(ex.Message)
            End Try
        End Select
        '==============
        Dim reqItem As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
        Dim pItm As SIS.DMS.UI.apiItem = Nothing
        If Req.Childs > 0 Then
          Return ErrorMessage("There are child Items, DELETE child Items first.")
        End If
        Dim mayDelete As Boolean = True
        Select Case Req.Type
          Case enumItemTypes.UserGroup
            If reqItem.CreatedBy = dmsUser.UserID Then
              mayDelete = True
            Else
              Return ErrorMessage("Authorized Item: Deletion NOT allowed.")
            End If
          Case enumItemTypes.Folder
            If Not dmsUser.IsAdmin Then
              If dmsUser.DeleteFolder = enumQualities.NotAllowed Then
                Return ErrorMessage("User Permission: Deletion NOT allowed.")
              Else
                Dim ParentFolder As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetParentFolder(reqItem.ItemID)
                If ParentFolder.DeleteFolder = enumQualities.NotAllowed Then
                  Return ErrorMessage("Folder Permission: Deletion NOT allowed.")
                Else
                  If ParentFolder.DeleteFolder = enumQualities.CreatorOnly Then
                    If reqItem.CreatedBy <> dmsUser.UserID Then
                      Return ErrorMessage("Folder Permission: Can be deleted by Creater Only.")
                    End If
                  End If
                End If
              End If
            End If
          Case Else
            If Not dmsUser.IsAdmin Then
              If dmsUser.DeleteFile = enumQualities.NotAllowed Then
                Return ErrorMessage("User Permission: Deletion NOT allowed.")
              Else
                Dim ParentFolder As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetParentFolder(reqItem.ItemID)
                If ParentFolder.DeleteFile = enumQualities.NotAllowed Then
                  Return ErrorMessage("Folder Permission: Deletion NOT allowed.")
                Else
                  If ParentFolder.DeleteFile = enumQualities.CreatorOnly Then
                    If reqItem.CreatedBy <> dmsUser.UserID Then
                      Return ErrorMessage("Folder Permission: Can be deleted by Creater Only.")
                    End If
                  End If
                  If reqItem.StatusID <> enumDMSStates.Created Then
                    Return ErrorMessage("Item Status Error: Can NOT be deleted.")
                  End If
                End If
              End If
            End If
        End Select
        If mayDelete Then
          Try
            SIS.DMS.UI.DeleteItem(Req.Type, Req.Item)
          Catch ex As Exception
            Return ErrorMessage(ex.Message)
          End Try
          mRet.err = False
          mRet.msg = ""
          mRet.Target = Req.Parent
          mRet.nofrm = True
          mRet.okmsg = "Deleted successfully"
          Return New JavaScriptSerializer().Serialize(mRet)
        End If
      Case "Create"
        Select Case Req.Type
          Case enumItemTypes.Folder
            If dmsUser.IsAdmin Then
              Return DoDefault()
            Else
              If dmsUser.CreateFolder = enumQualities.NotAllowed Then
                Return ErrorMessage("User permission: Not Allowed.")
              Else
                If Req.Base = "Type" Then
                  Return DoDefault()
                Else
                  Dim tmpTarget As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
                  Select Case tmpTarget.CreateFolder
                    Case enumQualities.NotAllowed
                      Return ErrorMessage("Folder Setting: Sub folder can not be created.")
                    Case enumQualities.CreatorOnly
                      If tmpTarget.CreatedBy <> dmsUser.UserID Then
                        Return ErrorMessage("Folder Setting: Sub folder can not be created by any other user.")
                      Else
                        Return DoDefault()
                      End If
                    Case Else
                      Return DoDefault()
                  End Select
                End If
              End If
            End If
        End Select
      Case "Extract BOM"
        Return DoDefault()
    End Select
    Return New JavaScriptSerializer().Serialize(mRet)
  End Function
#End Region
#Region " frmSubmitted "
  <WebMethod(EnableSession:=True)>
  Public Function frmSubmitted(context As String, options As String, data As String) As String
    If Not ValidateSession() Then Return SessionExpired()

    Dim strValidate As String = menuClicked(context, options)
    Dim Validate As SIS.DMS.UI.apiResponse = (New JavaScriptSerializer).Deserialize(strValidate, GetType(SIS.DMS.UI.apiResponse))
    If Validate.err Then Return strValidate

    Dim Req As SIS.DMS.UI.apiRequest = SIS.DMS.UI.apiRequest.GetRequest(context)
    Dim Opt As SIS.DMS.UI.apiOptions = SIS.DMS.UI.apiOptions.GetOptions(options)
    Dim Val As SIS.DMS.UI.apiItem = SIS.DMS.UI.apiItem.GetItem(data)

    Dim mRet As New SIS.DMS.UI.apiResponse

    Select Case Opt.Options
      Case "Attach Workflow"
        Dim reqItm As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
        Dim mItms As List(Of SIS.DMS.dmsMultiItems) = SIS.DMS.UI.GetMultiItems(reqItm.ItemID, enumMultiTypes.Associated, enumItemTypes.Workflow, Val.WorkflowID)
        If mItms.Count > 0 Then
          Return ErrorMessage("Workflow already associated")
        End If
        Dim mItm As New SIS.DMS.dmsMultiItems
        With mItm
          .ItemID = reqItm.ItemID
          .MultiTypeID = enumMultiTypes.Associated
          .MultiItemTypeID = enumItemTypes.Workflow
          .MultiItemID = Val.WorkflowID
          .MultiSequence = 0
          .CreatedBy = dmsUser.ItemID
          .CreatedOn = Now
        End With
        SIS.DMS.dmsMultiItems.InsertData(mItm)
        mRet.Target = Req.Parent
      Case "Remove Workflow"
        Dim reqItm As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
        Dim mItms As List(Of SIS.DMS.dmsMultiItems) = SIS.DMS.UI.GetMultiItems(reqItm.ItemID, enumMultiTypes.Associated, enumItemTypes.Workflow, Val.WorkflowID)
        If mItms.Count = 0 Then
          Return ErrorMessage("Selected Workflow NOT associated")
        End If
        SIS.DMS.UI.DeleteMultiItems(reqItm.ItemID, enumMultiTypes.Associated, enumItemTypes.Workflow, Val.WorkflowID)
        mRet.Target = Req.Parent
      Case "Add User to Group"
        '1. Check User Exists
        'Check User in Group Exists
        Dim valUser As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetUserByUserID(Val.UserID)
        Dim reqItm As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
        Dim qcmUser As SIS.QCM.qcmUsers = SIS.QCM.qcmUsers.qcmUsersGetByID(Val.UserID)
        If valUser IsNot Nothing Then
          '2. Check User Item Exists
          If SIS.DMS.UI.ItemExistsInGroup(Req.Item, valUser.ItemID) Then
            Return ErrorMessage("User already exists in Group.")
          End If
        Else
          '1.1 create User From WebUser
          valUser = New SIS.DMS.UI.apiItem
          With valUser
            .ItemTypeID = enumItemTypes.User
            .Description = qcmUser.UserFullName
            .FullDescription = qcmUser.UserFullName
            .UserID = Val.UserID
            .CreatedBy = dmsUser.UserID
            .CreatedOn = Now
            .RevisionNo = "00"
            .StatusID = enumDMSStates.Published
            .EMailID = qcmUser.EMailID
            .CompanyID = qcmUser.C_CompanyID
            .DivisionID = qcmUser.C_DivisionID
            .DepartmentID = qcmUser.C_DepartmentID
            .ProjectID = reqItm.ProjectID
            .WBSID = reqItm.WBSID
            .KeyWords = Val.KeyWords
            .CreateFolder = reqItm.CreateFolder
            .DeleteFolder = reqItm.DeleteFolder
            .RenameFolder = reqItm.RenameFolder
            .CreateFile = reqItm.CreateFile
            .DeleteFile = reqItm.DeleteFile
            .ShowInList = reqItm.ShowInList
            .BrowseList = reqItm.BrowseList
            .Publish = reqItm.Publish
            .GrantAuthorization = reqItm.GrantAuthorization
          End With
          valUser = SIS.DMS.UI.apiItem.InsertData(valUser)
        End If
        With valUser
          .ParentItemID = Req.Item
          .ItemTypeID = enumItemTypes.UserGroupUser
          .CreatedBy = dmsUser.UserID
          .CreatedOn = Now
          .RevisionNo = "00"
          .StatusID = enumDMSStates.Published
          .ProjectID = reqItm.ProjectID
          .WBSID = reqItm.WBSID
          .KeyWords = reqItm.KeyWords
          .EMailID = reqItm.EMailID
          .CreateFolder = reqItm.CreateFolder
          .DeleteFolder = reqItm.DeleteFolder
          .RenameFolder = reqItm.RenameFolder
          .CreateFile = reqItm.CreateFile
          .DeleteFile = reqItm.DeleteFile
          .ShowInList = reqItm.ShowInList
          .BrowseList = reqItm.BrowseList
          .Publish = reqItm.Publish
          .GrantAuthorization = reqItm.GrantAuthorization
          .LinkedItemTypeID = enumItemTypes.User
          .LinkedItemID = valUser.ItemID
          .ItemID = 0
        End With
        valUser = SIS.DMS.UI.apiItem.InsertData(valUser)
        'Authorize Added User to allready authorized Items of group
        Dim grAuths As List(Of SIS.DMS.UI.apiItem) = SIS.DMS.UI.GetAllTypeLinkedTypeLinkedItem(enumItemTypes.Authorization, enumItemTypes.UserGroup, Req.Item)
        For Each ath As SIS.DMS.UI.apiItem In grAuths
          Dim mItm As New SIS.DMS.dmsMultiItems
          With mItm
            .ItemID = valUser.LinkedItemID
            .MultiTypeID = enumMultiTypes.Authorized
            .MultiItemTypeID = ath.ForwardLinkedItemTypeID
            .MultiItemID = ath.ForwardLinkedItemID
            .MultiSequence = 0
            .CreatedBy = dmsUser.ItemID
            .CreatedOn = Now
          End With
          SIS.DMS.dmsMultiItems.InsertData(mItm)
        Next
        mRet.Target = Req.Parent
      Case "Approve / Acknowledge"
        Dim itmInWF As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
        Try
          If Val.Approved Then
            SIS.DMS.UI.Core.ApproveWF(itmInWF, dmsUser, Val.ActionRemarks)
          Else
            SIS.DMS.UI.Core.RejectWF(itmInWF, dmsUser, Val.ActionRemarks)
          End If
          'Parent to be Refreshed
          mRet.Target = "trRoot_" & dmsUser.ItemID
        Catch ex As Exception
          Return ErrorMessage(ex.Message)
        End Try
      Case "Authorize to User Group"
        '0. Check if already Authorised to User Group
        Dim mItms As List(Of SIS.DMS.dmsMultiItems) = SIS.DMS.UI.GetMultiItems(Val.UserGroupID, enumMultiTypes.Authorized, Req.Type, Req.Item)
        If mItms.Count > 0 Then
          Return ErrorMessage("Already Authorised")
        End If
        Dim mItm As New SIS.DMS.dmsMultiItems
        '1. Create Authorization Entry in each user's multiitems
        Dim gUsers As List(Of SIS.DMS.UI.apiItem) = SIS.DMS.UI.Core.GetChildItems(Val.UserGroupID, 0)
        For Each gu As SIS.DMS.UI.apiItem In gUsers
          mItm = New SIS.DMS.dmsMultiItems
          With mItm
            .ItemID = gu.LinkedItemID
            .MultiTypeID = enumMultiTypes.Authorized
            .MultiItemTypeID = Req.Type
            .MultiItemID = Req.Item
            .MultiSequence = 0
            .CreatedBy = dmsUser.ItemID
            .CreatedOn = Now
          End With
          SIS.DMS.dmsMultiItems.InsertData(mItm)
        Next
        '3. Create an Item of Authorization Type
        Dim reqItm As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
        Dim valItm As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Val.UserGroupID)
        Dim grantedByMe As New SIS.DMS.UI.apiItem
        With grantedByMe
          .ItemTypeID = enumItemTypes.Authorization
          .CreatedBy = dmsUser.UserID
          .CreatedOn = Now
          .StatusID = enumDMSStates.Published
          .RevisionNo = "00"
          .LinkedItemID = Val.UserGroupID
          .LinkedItemTypeID = enumItemTypes.UserGroup
          .ForwardLinkedItemID = Req.Item
          .ForwardLinkedItemTypeID = Req.Type
          .FullDescription = reqItm.Description & " => " & valItm.Description
          .Description = .FullDescription
        End With
        grantedByMe = SIS.DMS.UI.apiItem.InsertData(grantedByMe)
        '4. Create a Link in Myto Me of Authorization
        mItm = New SIS.DMS.dmsMultiItems
        With mItm
          .ItemID = dmsUser.ItemID
          .MultiTypeID = enumMultiTypes.Created
          .MultiItemID = grantedByMe.ItemID
          .MultiItemTypeID = enumItemTypes.Authorization
          .MultiSequence = 0
          .CreatedBy = dmsUser.ItemID
          .CreatedOn = Now
        End With
        mItm = SIS.DMS.dmsMultiItems.InsertData(mItm)
        '5. Create Authorized record in Group ID
        mItm = New SIS.DMS.dmsMultiItems
        With mItm
          .ItemID = Val.UserGroupID
          .MultiTypeID = enumMultiTypes.Authorized
          .MultiItemTypeID = Req.Type
          .MultiItemID = Req.Item
          .MultiSequence = 0
          .CreatedBy = dmsUser.ItemID
          .CreatedOn = Now
        End With
        SIS.DMS.dmsMultiItems.InsertData(mItm)
        'My Authorization is to be Reloaded at Client
        mRet.Target = "trRoot_" & dmsUser.ItemID & "_" & enumItemTypes.Authorization
      Case "Authorize to User"
        '1. Check User Exists
        Dim valUser As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetUserByUserID(Val.UserID)
        If valUser IsNot Nothing Then
          Dim mItms As List(Of SIS.DMS.dmsMultiItems) = SIS.DMS.UI.GetMultiItems(valUser.ItemID, enumMultiTypes.Authorized, Req.Type, Req.Item)
          If mItms.Count > 0 Then
            Return ErrorMessage("Already Authorised")
          End If
        Else
          '1.1 create User Using itm property
          Dim qcmUser As SIS.QCM.qcmUsers = SIS.QCM.qcmUsers.qcmUsersGetByID(Val.UserID)
          valUser = New SIS.DMS.UI.apiItem
          With valUser
            .ItemTypeID = enumItemTypes.User
            .Description = qcmUser.UserFullName
            .FullDescription = qcmUser.UserFullName
            .UserID = Val.UserID
            .CreatedBy = dmsUser.UserID
            .CreatedOn = Now
            .RevisionNo = "00"
            .StatusID = enumDMSStates.Published
            .CompanyID = qcmUser.C_CompanyID
            .DivisionID = qcmUser.C_DivisionID
            .DepartmentID = qcmUser.C_DepartmentID
            .EMailID = qcmUser.EMailID
            .ProjectID = Val.ProjectID
            .WBSID = Val.WBSID
            .KeyWords = Val.KeyWords
            .CreateFolder = Val.CreateFolder
            .DeleteFolder = Val.DeleteFolder
            .RenameFolder = Val.RenameFolder
            .CreateFile = Val.CreateFile
            .DeleteFile = Val.DeleteFile
            .ShowInList = Val.ShowInList
            .BrowseList = Val.BrowseList
            .Publish = Val.Publish
            .GrantAuthorization = Val.GrantAuthorization
          End With
          valUser = SIS.DMS.UI.apiItem.InsertData(valUser)
        End If
        '2. Create an Authorized in TargetUser MultiItem
        ' For Item Under Authorization
        Dim mItm As New SIS.DMS.dmsMultiItems
        With mItm
          .ItemID = valUser.ItemID
          .MultiTypeID = enumMultiTypes.Authorized
          .MultiItemID = Req.Item
          .MultiItemTypeID = Req.Type
          .MultiSequence = 0
          .CreatedBy = dmsUser.ItemID
          .CreatedOn = Now
        End With
        mItm = SIS.DMS.dmsMultiItems.InsertData(mItm)
        '3. Create an Item of Authorization Type
        Dim Itm As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)
        Dim grantedByMe As New SIS.DMS.UI.apiItem
        With grantedByMe
          .ItemTypeID = enumItemTypes.Authorization
          .CreatedBy = dmsUser.UserID
          .CreatedOn = Now
          .StatusID = enumDMSStates.Published
          .RevisionNo = "00"
          .LinkedItemID = valUser.ItemID
          .LinkedItemTypeID = valUser.ItemTypeID
          .ForwardLinkedItemID = Req.Item
          .ForwardLinkedItemTypeID = Req.Type
          .FullDescription = Itm.Description & " => " & valUser.Description
          .Description = .FullDescription
        End With
        grantedByMe = SIS.DMS.UI.apiItem.InsertData(grantedByMe)
        '4. Create a Link in Myto Me of Authorization
        mItm = New SIS.DMS.dmsMultiItems
        With mItm
          .ItemID = dmsUser.ItemID
          .MultiTypeID = enumMultiTypes.Created
          .MultiItemID = grantedByMe.ItemID
          .MultiItemTypeID = enumItemTypes.Authorization
          .MultiSequence = 0
          .CreatedBy = dmsUser.ItemID
          .CreatedOn = Now
        End With
        mItm = SIS.DMS.dmsMultiItems.InsertData(mItm)
        'My Authorization is to be Reloaded at Client
        mRet.Target = "trRoot_" & dmsUser.ItemID & "_" & enumItemTypes.Authorization
      Case "Create"
        Select Case Req.Type
          Case Else
            'checked for Folder, User Group, WorkFlow
            Val.StatusID = enumDMSStates.Published
            Val.CreatedBy = dmsUser.UserID
            Val.CreatedOn = Now
            Val.RevisionNo = "00"
            Val.Description = Val.FullDescription
            If Req.Type = enumItemTypes.Workflow Then
              Val.LinkedItemTypeID = enumItemTypes.UserGroup
            End If
            If SIS.DMS.UI.DuplicateFound(Val.Description, Val.ItemTypeID, Req.Item) Then
              Return ErrorMessage("Duplicate Item: Can not create.")
            End If
            If Req.Item > 0 Then
              Val.ParentItemID = Req.Item
            End If
            Val = SIS.DMS.UI.apiItem.InsertData(Val)
            If Req.Base.ToUpper = "TYPE" Then
              Dim LItm As New SIS.DMS.dmsMultiItems
              With LItm
                .ItemID = dmsUser.ItemID
                .MultiTypeID = enumMultiTypes.Created
                .MultiItemTypeID = Val.ItemTypeID
                .MultiItemID = Val.ItemID
                .MultiSequence = 0
                .CreatedBy = dmsUser.ItemID
                .CreatedOn = Now
              End With
              LItm = SIS.DMS.dmsMultiItems.InsertData(LItm)
            End If
            If Req.Childs > 0 Then
              'Parent will be refreshed
              mRet.Target = Req.Target
            Else
              'Parent of Parent be refreshed
              mRet.Target = Req.Parent
            End If
        End Select
      Case "Edit", "Edit and Publish", "Edit User of Group"
        Dim Itm As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Val.ItemID)
        Select Case Val.ItemTypeID
          Case enumItemTypes.User
            With Itm
              .CompanyID = Val.CompanyID
              .DivisionID = Val.DivisionID
              .DepartmentID = Val.DepartmentID
              .EMailID = Val.EMailID
              .ProjectID = Val.ProjectID
              .WBSID = Val.WBSID
              .CreateFolder = Val.CreateFolder
              .DeleteFolder = Val.DeleteFolder
              .RenameFolder = Val.RenameFolder
              .CreateFile = Val.CreateFile
              .DeleteFile = Val.DeleteFile
              .GrantAuthorization = Val.GrantAuthorization
              .Publish = Val.Publish
              .KeyWords = Val.KeyWords
            End With
            SIS.DMS.UI.apiItem.UpdateData(Itm)
            Dim mItm As List(Of SIS.DMS.dmsMultiItems) = SIS.DMS.UI.GetMultiItems(Val.ItemID, enumMultiTypes.Administrator, "", "")
            If Val.IsAdmin Then
              If mItm.Count <= 0 Then
                Dim tmpItm As New SIS.DMS.dmsMultiItems
                With tmpItm
                  .ItemID = Val.ItemID
                  .MultiTypeID = enumMultiTypes.Administrator
                  .CreatedBy = dmsUser.ItemID
                  .CreatedOn = Now
                End With
                SIS.DMS.dmsMultiItems.InsertData(tmpItm)
              End If
            Else
              If mItm.Count > 0 Then
                SIS.DMS.dmsMultiItems.dmsMultiItemsDelete(mItm(0))
              End If
            End If
            mRet.Target = Req.Parent
          Case Else
            'checked for folder, file, workflow
            With Itm
              If Itm.ItemTypeID <> enumItemTypes.File Then
                .Description = Val.FullDescription
                .FullDescription = Val.FullDescription
                .CreateFolder = Val.CreateFolder
                .DeleteFolder = Val.DeleteFolder
                .RenameFolder = Val.RenameFolder
                .CreateFile = Val.CreateFile
                .DeleteFile = Val.DeleteFile
                .ShowInList = Val.ShowInList
                .BrowseList = Val.BrowseList
                .Publish = Val.Publish
                .GrantAuthorization = Val.GrantAuthorization
              Else
                .CreatedBy = dmsUser.UserID
                .CreatedOn = Now
              End If
              If Itm.ItemTypeID = enumItemTypes.Workflow Then
                Itm.ConvertedStatusID = Val.ConvertedStatusID
                Itm.LinkedItemID = Val.LinkedItemID
              End If
              .KeyWords = Val.KeyWords
              .CompanyID = Val.CompanyID
              .DivisionID = Val.DivisionID
              .DepartmentID = Val.DepartmentID
              .ProjectID = Val.ProjectID
              .WBSID = Val.WBSID
            End With
            SIS.DMS.UI.apiItem.UpdateData(Itm)
            '===========Create History===========
            If Itm.ItemTypeID = enumItemTypes.File Then
              SIS.DMS.dmsHistory.InsertData(Itm)
            End If
            '====================================
            If Opt.Options = "Edit and Publish" Then
              If Opt.Publish Then
                doPublish(Req)
              End If
            End If
            mRet.Target = Req.Parent

        End Select
    End Select

    Return New JavaScriptSerializer().Serialize(mRet)
  End Function

#End Region
#Region " validateFiles "
  <WebMethod(EnableSession:=True)>
  Public Function validateFiles(context As String, options As String, filedata As String) As String
    If Not ValidateSession() Then Return SessionExpired()
    Dim Req As SIS.DMS.UI.apiRequest = SIS.DMS.UI.apiRequest.GetRequest(context)
    Dim Opt As SIS.DMS.UI.apiOptions = SIS.DMS.UI.apiOptions.GetOptions(options)
    Dim fs As List(Of SIS.DMS.UI.apiFiles) = SIS.DMS.UI.apiFiles.GetFiles(filedata)
    Dim Itm As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(Req.Item)

    Dim mRet As New SIS.DMS.UI.apiResponse

    For Each f As SIS.DMS.UI.apiFiles In fs
      If SIS.DMS.UI.IsSelfRef(Itm.ItemID, f.fileName) Then
        f.err = True
        f.cancelled = True
        f.msg = "File can not reference to itself at any level."
        mRet.strHTML.Add((New JavaScriptSerializer).Serialize(f))
        Continue For
      End If
      Dim Fx As SIS.DMS.UI.apiItem = SIS.DMS.UI.FileExists(Req.Item, f.fileName)
      If Fx IsNot Nothing Then
        f.fileID = Fx.ItemID
        Dim FxStatus As SIS.DMS.UI.apiStatus = Fx.Status
        If FxStatus.Locked Then
          If FxStatus.Revisable Then
            f.cancelled = True
            f.err = True
            f.msg = "File Exists, but NOT Updatable, first REVISE it, to upload next revision of file."
          Else
            f.cancelled = True
            f.err = True
            f.msg = "File Exists, and NOT Updatable."
          End If
        Else
          If Fx.StatusID = enumDMSStates.CheckedOut Then
            If Fx.CreatedBy <> dmsUser.UserID Then
              If dmsUser.IsAdmin Then
                f.warn = True
                f.msg = "File will be overwritten by Admin. [Though checkedout by other user]"
              Else
                f.cancelled = True
                f.err = True
                f.msg = "File NOT Updatable - checkedout by other user"
              End If
            End If
          End If
        End If
      End If
      mRet.strHTML.Add((New JavaScriptSerializer).Serialize(f))
    Next
    mRet.err = False
    mRet.msg = ""
    mRet.Target = ""
    Return New JavaScriptSerializer().Serialize(mRet)
  End Function
#End Region
  <WebMethod(EnableSession:=True)>
  Public Function LoadHistory(ByVal context As String) As String
    Dim Req As SIS.DMS.UI.apiRequest = SIS.DMS.UI.apiRequest.GetRequest(context)
    Dim mRet As New SIS.DMS.UI.apiResponse
    Dim tmpItems As New List(Of SIS.DMS.dmsHistory)
    tmpItems = SIS.DMS.dmsHistory.getdmsHistory(Req.Item)
    mRet.strHTML.Add(New JavaScriptSerializer().Serialize(tmpItems))
    mRet.err = False
    mRet.msg = ""
    mRet.Target = ""
    Return New JavaScriptSerializer().Serialize(mRet)
  End Function
  <WebMethod(EnableSession:=True)>
  Public Function InitiateSearch(ByVal context As String, ByVal searchid As String) As String
    If Not ValidateSession() Then Return SessionExpired()
    Dim mRet As New SIS.DMS.UI.apiResponse
    Dim dmsSearchText As String = context
    Dim tmpItem As New SIS.DMS.UI.apiItem
    If searchid <> "" Then
      tmpItem = SIS.DMS.UI.GetItem(searchid)
      If tmpItem Is Nothing Then
        searchid = ""
        tmpItem = New SIS.DMS.UI.apiItem
      End If
    End If
    If searchid <> "" Then
      Try
        tmpItem = SIS.DMS.UI.GetItem(searchid)
        tmpItem.Description = context
        tmpItem.FullDescription = context
        tmpItem = SIS.DMS.UI.apiItem.UpdateData(tmpItem)
        SIS.DMS.dmsSearchData.DeleteLastResults(searchid)

        Dim Req As New SIS.DMS.UI.apiRequest
        With Req
          .Base = "Item"
          .Childs = 0
          .Expended = 0
          .Loaded = 0
          .ParentExpended = 1
          .Target = "trRoot_" & dmsUser.ItemID & "_" & enumItemTypes.Searches
          .Type = enumItemTypes.Searches
          .Indent = 2
          .Item = 0
        End With

        mRet = SIS.DMS.UI.Core.strTree(dmsUser.ItemID, New JavaScriptSerializer().Serialize(Req))
        'mRet.CurrentSearch = Req.Target & "_" & searchid
        mRet.CurrentSearch = searchid
      Catch ex As Exception
        With mRet
          .err = True
          .msg = ex.Message
        End With
      End Try
    Else
      tmpItem = New SIS.DMS.UI.apiItem
      With tmpItem
        .Description = context
        .FullDescription = context
        .ItemTypeID = enumItemTypes.Searches
        .RevisionNo = "00"
        .CreatedBy = dmsUser.UserID
        .CreatedOn = Now
        .StatusID = enumDMSStates.Published
        .LinkedItemID = dmsUser.ItemID
      End With
      tmpItem = SIS.DMS.UI.apiItem.InsertData(tmpItem)
      Dim lItm As New SIS.DMS.dmsMultiItems
      With lItm
        .ItemID = dmsUser.ItemID
        .MultiTypeID = enumMultiTypes.Created
        .MultiItemID = tmpItem.ItemID
        .MultiItemTypeID = enumItemTypes.Searches
        .CreatedBy = dmsUser.ItemID
        .CreatedOn = Now
        .MultiSequence = 0
      End With
      lItm = SIS.DMS.dmsMultiItems.InsertData(lItm)

      Dim Req As New SIS.DMS.UI.apiRequest
      With Req
        .Base = "Type"
        .Childs = 0
        .Expended = 0
        .Loaded = 0
        .ParentExpended = 1
        .Target = "trRoot_" & dmsUser.ItemID & "_" & enumItemTypes.Searches
        .Type = enumItemTypes.Searches
        .Indent = 2
        .Item = 0
      End With

      mRet = SIS.DMS.UI.Core.strTree(dmsUser.ItemID, New JavaScriptSerializer().Serialize(Req))
      'mRet.CurrentSearch = Req.Target & "_" & tmpItem.ItemID
      mRet.CurrentSearch = tmpItem.ItemID
    End If
    Return New JavaScriptSerializer().Serialize(mRet)
  End Function
  <WebMethod()>
  Public Function TriggerSearch(ByVal searchid As String) As String
    Dim json As New System.Web.Script.Serialization.JavaScriptSerializer()
    Dim mRet As New SIS.DMS.UI.apiResponse
    Try
      SIS.DMS.dmsSearchData.SearchID = searchid
      SIS.DMS.dmsSearchData.StopNow = False
      SIS.DMS.dmsSearchData.TriggerSearch(searchid)
      mRet.CurrentSearch = searchid
      mRet.ServerStopped = True
      If SIS.DMS.dmsSearchData.StopNow Then
        mRet.msg = "Search Cancelled"
      Else
        mRet.msg = "Search Completed"
      End If
    Catch ex As Exception
      mRet.err = True
      mRet.msg = ex.Message
    End Try
    Return New JavaScriptSerializer().Serialize(mRet)
  End Function
  <WebMethod()>
  Public Function FetchResults(ByVal searchid As String, ByVal stopit As String, ByVal lastitem As String) As String
    Dim sItm As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(searchid)
    Dim mRet As New SIS.DMS.UI.apiResponse
    Dim ShouldStop As Boolean = Convert.ToBoolean(stopit)
    SIS.DMS.dmsSearchData.SearchID = searchid
    SIS.DMS.dmsSearchData.LastItem = lastitem
    Try
      If ShouldStop Then
        SIS.DMS.dmsSearchData.StopNow = ShouldStop
      End If
      Dim tmpItms As List(Of SIS.DMS.UI.apiItem) = SIS.DMS.dmsSearchData.GetData()
      If tmpItms.Count > 0 Then
        lastitem = tmpItms(0).SearchSerialNo
      End If
      With mRet
        .CurrentSearch = searchid
        .LastItem = lastitem
        .ServerStopped = Not SIS.DMS.dmsSearchData.ServerProcessing
        .strHTML = New List(Of String)
      End With
      If tmpItms.Count > 0 Then
        Dim Req As New SIS.DMS.UI.apiRequest
        With Req
          .Base = "Item"
          .Childs = 0
          .Expended = 0
          .Loaded = 0
          .ParentExpended = 1
          .Target = "trRoot_" & sItm.LinkedItemID & "_" & enumItemTypes.Searches & "_" & searchid
          .Type = enumItemTypes.Searches
          .Indent = 4
          .Item = searchid
        End With
        For Each itm As SIS.DMS.UI.apiItem In tmpItms
          Req.Childs = 0
          mRet.strHTML.Add(SIS.DMS.UI.Core.GetTr(itm.ItemID, Req, itm))
        Next
        mRet.Target = Req.Target
      End If
    Catch ex As Exception
      SIS.DMS.dmsSearchData.StopNow = True
      With mRet
        .err = True
        .msg = ex.Message
        .CurrentSearch = searchid
        .LastItem = lastitem
        .strHTML = New List(Of String)
        .ServerStopped = True
      End With
    End Try
    Return New JavaScriptSerializer().Serialize(mRet)
  End Function

  <WebMethod()>
  Public Function getTags(context As String, cnt As String) As String
    Return SIS.DMS.UI.apiTags.selectAutoComplete(context, cnt)
  End Function

End Class
