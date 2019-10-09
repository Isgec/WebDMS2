<%@ WebService Language="VB" Class="dmsClientRequest" %>

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Serialization

<System.Web.Script.Services.ScriptService()>
<WebService(Namespace:="http://tempuri.org/")>
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
Public Class dmsClientRequest
  Inherits System.Web.Services.WebService

  <WebMethod(EnableSession:=True)>
  Public Function MyDmsSubmit(ByVal context As String, ByVal dmstarget As String, ByVal dmsoption As String) As String
    Dim LoggedUser As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetSessionUserObject()
    If LoggedUser Is Nothing Then
      Return New JavaScriptSerializer().Serialize(New With {
           .isError = True,
           .ErrorMessage = "Session Expired, Login again."
       })
    End If
    Dim json As New System.Web.Script.Serialization.JavaScriptSerializer()
    Dim mRet As New SIS.DMS.Handlers.dmsResponse
    Dim itm As SIS.DMS.dmsItems = json.Deserialize(context, GetType(SIS.DMS.dmsItems))
    Dim aTarget() As String = dmstarget.Split("|".ToCharArray)
    Dim atRoot As Boolean = True
    Dim selectedTarget As String = aTarget(0)
    Dim selectedType As String = aTarget(1)
    Dim selectedItem As String = aTarget(2)
    If aTarget(2) <> "" Then atRoot = False

    Select Case dmsoption.ToUpper
      Case "EDIT"
        Select Case itm.ItemTypeID
          Case enumItemTypes.File
            Dim updItm As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItem(selectedItem)
            With updItm
              .BrowseList = itm.BrowseList
              .CompanyID = itm.CompanyID
              .CreateFile = itm.CreateFile
              .CreateFolder = itm.CreateFolder
              .DeleteFile = itm.DeleteFile
              .DeleteFolder = itm.DeleteFolder
              .DepartmentID = itm.DepartmentID
              .DivisionID = itm.DivisionID
              .GrantAuthorization = itm.GrantAuthorization
              .InheritFromParent = itm.InheritFromParent
              .KeyWords = itm.KeyWords
              .ProjectID = itm.ProjectID
              .Publish = itm.Publish
              .RenameFolder = itm.RenameFolder
              .SearchInParent = itm.SearchInParent
              .ShowInList = itm.ShowInList
              .WBSID = itm.WBSID
            End With
            SIS.DMS.dmsItems.UpdateData(updItm)
          Case enumItemTypes.Workflow
            Dim updItm As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItem(selectedItem)
            '1. Before Update Check User
            Dim targetUser As SIS.DMS.dmsItems = Nothing
            If updItm.UserID <> itm.UserID Then
              If itm.UserID <> "" Then
                targetUser = SIS.DMS.Handlers.dmsData.GetItemByUserID(itm.UserID)
                If targetUser Is Nothing Then
                  targetUser = New SIS.DMS.dmsItems
                  targetUser = SIS.DMS.Handlers.dmsData.UpdateMatchingProperties(itm, targetUser)
                  With targetUser
                    .ItemTypeID = enumItemTypes.User
                    .Description = .FK_DMS_Items_UserID.UserFullName
                    .FullDescription = .Description
                    .CreatedBy = HttpContext.Current.Session("LoginID")
                    .CreatedOn = Now
                    .StatusID = enumDMSStates.Published
                    .RevisionNo = "00"
                    .ConvertedStatusID = ""
                    .ParentItemID = ""
                    targetUser = SIS.DMS.dmsItems.InsertData(targetUser)
                  End With
                End If
              End If
              If updItm.UserID <> "" Then
                If itm.UserID = "" Then
                  Dim usr As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItemByUserID(updItm.UserID)
                  Dim uItm As List(Of SIS.DMS.dmsMultiItems) = SIS.DMS.Handlers.dmsData.GetMultiItems(updItm.ItemID, enumMultiTypes.Associated, enumItemTypes.User, usr.ItemID)
                  SIS.DMS.dmsMultiItems.dmsMultiItemsDelete(uItm(0))
                Else
                  Dim usr As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItemByUserID(updItm.UserID)
                  Dim uItm As List(Of SIS.DMS.dmsMultiItems) = SIS.DMS.Handlers.dmsData.GetMultiItems(updItm.ItemID, enumMultiTypes.Associated, enumItemTypes.User, usr.ItemID)
                  uItm(0).MultiItemID = targetUser.ItemID
                  SIS.DMS.dmsMultiItems.UpdateData(uItm(0))
                End If
              Else
                If itm.UserID <> "" Then
                  Dim lItm As SIS.DMS.dmsMultiItems = New SIS.DMS.dmsMultiItems
                  With lItm
                    .ItemID = itm.ItemID
                    .MultiTypeID = enumMultiTypes.Associated
                    .MultiItemID = targetUser.ItemID
                    .MultiItemTypeID = enumItemTypes.User
                    .MultiSequence = 0
                  End With
                  lItm = SIS.DMS.dmsMultiItems.InsertData(lItm)
                End If
              End If
            End If
            '2. Update
            With updItm
              .Description = itm.FullDescription
              .ConvertedStatusID = itm.ConvertedStatusID
              .UserID = itm.UserID
            End With
            updItm = SIS.DMS.dmsItems.UpdateData(updItm)
          Case enumItemTypes.Folder
            Dim updItm As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItem(selectedItem)
            With updItm
              .Description = itm.FullDescription
              .BrowseList = itm.BrowseList
              .CompanyID = itm.CompanyID
              .CreateFile = itm.CreateFile
              .CreateFolder = itm.CreateFolder
              .DeleteFile = itm.DeleteFile
              .DeleteFolder = itm.DeleteFolder
              .DepartmentID = itm.DepartmentID
              .DivisionID = itm.DivisionID
              .GrantAuthorization = itm.GrantAuthorization
              .InheritFromParent = itm.InheritFromParent
              .KeyWords = itm.KeyWords
              .ProjectID = itm.ProjectID
              .Publish = itm.Publish
              .RenameFolder = itm.RenameFolder
              .SearchInParent = itm.SearchInParent
              .ShowInList = itm.ShowInList
              .WBSID = itm.WBSID
            End With
            updItm = SIS.DMS.dmsItems.UpdateData(updItm)
          Case enumItemTypes.UserGroup
            Dim updItm As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItem(selectedItem)
            With updItm
              .Description = itm.FullDescription
              .FullDescription = itm.FullDescription
              .BrowseList = itm.BrowseList
              .CompanyID = itm.CompanyID
              .CreateFile = itm.CreateFile
              .CreateFolder = itm.CreateFolder
              .DeleteFile = itm.DeleteFile
              .DeleteFolder = itm.DeleteFolder
              .DepartmentID = itm.DepartmentID
              .DivisionID = itm.DivisionID
              .GrantAuthorization = itm.GrantAuthorization
              .InheritFromParent = itm.InheritFromParent
              .KeyWords = itm.KeyWords
              .ProjectID = itm.ProjectID
              .Publish = itm.Publish
              .RenameFolder = itm.RenameFolder
              .SearchInParent = itm.SearchInParent
              .ShowInList = itm.ShowInList
              .WBSID = itm.WBSID
            End With
            updItm = SIS.DMS.dmsItems.UpdateData(updItm)
        End Select
        'Parent to be Refreshed
        Dim aParent() As String = dmstarget.Split("_".ToCharArray)
        ReDim Preserve aParent(aParent.Length - 2)
        mRet.dmsTarget = Join(aParent, "_")
      Case "EXECUTE"
        Dim itmInWF As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItem(itm.ItemID)
        Try
          If itm.Approved Then
            SIS.DMS.Handlers.Core.ApproveWF(itmInWF, LoggedUser, itm.ActionRemarks)
          Else
            SIS.DMS.Handlers.Core.RejectWF(itmInWF, LoggedUser, itm.ActionRemarks)
          End If
          'Parent to be Refreshed
          Dim aParent() As String = dmstarget.Split("_".ToCharArray)
          ReDim Preserve aParent(aParent.Length - 2)
          mRet.dmsTarget = Join(aParent, "_")
        Catch ex As Exception
          Return New JavaScriptSerializer().Serialize(New With {
               .isError = True,
               .ErrorMessage = ex.Message
           })
        End Try
      Case "CREATE"
        Select Case itm.ItemTypeID
          Case enumItemTypes.Workflow
            itm.ParentItemID = selectedItem
            itm.CreatedBy = HttpContext.Current.Session("LoginID")
            itm.CreatedOn = Now
            itm.Description = itm.FullDescription
            itm.StatusID = SIS.DMS.Handlers.Core.GetInitialStatus(selectedItem, "")
            itm.RevisionNo = "00"
            itm = SIS.DMS.dmsItems.InsertData(itm)
            Dim lItm As SIS.DMS.dmsMultiItems = Nothing
            If atRoot Then
              'Link user with itm
              Dim UserID As String = SIS.DMS.Handlers.dmsData.GetSessionUser()
              Dim User As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItemByUserID(UserID)
              lItm = New SIS.DMS.dmsMultiItems
              With lItm
                .ItemID = User.ItemID
                .MultiTypeID = enumMultiTypes.Linked
                .MultiItemID = itm.ItemID
                .MultiItemTypeID = selectedType
                .MultiSequence = 0
              End With
              lItm = SIS.DMS.dmsMultiItems.InsertData(lItm)
            End If
            '1. associate user
            If itm.UserID <> "" Then
              Dim targetUser As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItemByUserID(itm.UserID)
              If targetUser Is Nothing Then
                targetUser = New SIS.DMS.dmsItems
                targetUser = SIS.DMS.Handlers.dmsData.UpdateMatchingProperties(itm, targetUser)
                With targetUser
                  .ItemTypeID = enumItemTypes.User
                  .Description = .FK_DMS_Items_UserID.UserFullName
                  .FullDescription = .Description
                  .CreatedBy = HttpContext.Current.Session("LoginID")
                  .CreatedOn = Now
                  .StatusID = enumDMSStates.Published
                  .RevisionNo = "00"
                  .ConvertedStatusID = ""
                  .ParentItemID = ""
                  targetUser = SIS.DMS.dmsItems.InsertData(targetUser)
                End With
              End If
              lItm = New SIS.DMS.dmsMultiItems
              With lItm
                .ItemID = itm.ItemID
                .MultiTypeID = enumMultiTypes.Associated
                .MultiItemID = targetUser.ItemID
                .MultiItemTypeID = enumItemTypes.User
                .MultiSequence = 0
              End With
              lItm = SIS.DMS.dmsMultiItems.InsertData(lItm)
            End If
            'Parent to be Refreshed
            Dim aParent() As String = dmstarget.Split("_".ToCharArray)
            ReDim Preserve aParent(aParent.Length - 2)
            mRet.dmsTarget = Join(aParent, "_")
          Case enumItemTypes.Folder
            itm.ParentItemID = selectedItem
            itm.CreatedBy = HttpContext.Current.Session("LoginID")
            itm.CreatedOn = Now
            If itm.UserID <> "" Then
              itm.Description = itm.FK_DMS_Items_UserID.UserFullName
              itm.FullDescription = itm.Description
            Else
              itm.Description = itm.FullDescription
            End If
            itm.StatusID = SIS.DMS.Handlers.Core.GetInitialStatus(selectedItem, "")
            itm.RevisionNo = "00"
            itm = SIS.DMS.dmsItems.InsertData(itm)
            If atRoot Then
              'Link user with itm
              Dim UserID As String = SIS.DMS.Handlers.dmsData.GetSessionUser()
              Dim User As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItemByUserID(UserID)
              Dim lItm As New SIS.DMS.dmsMultiItems
              With lItm
                .ItemID = User.ItemID
                .MultiTypeID = enumMultiTypes.Linked
                .MultiItemID = itm.ItemID
                .MultiItemTypeID = selectedType
                .MultiSequence = 0
              End With
              lItm = SIS.DMS.dmsMultiItems.InsertData(lItm)
            End If
            'Parent to be Refreshed
            Dim aParent() As String = dmstarget.Split("_".ToCharArray)
            ReDim Preserve aParent(aParent.Length - 2)
            mRet.dmsTarget = Join(aParent, "_")
          Case enumItemTypes.UserGroup
            itm.ParentItemID = selectedItem
            itm.CreatedBy = HttpContext.Current.Session("LoginID")
            itm.CreatedOn = Now
            itm.Description = itm.FullDescription
            itm.StatusID = SIS.DMS.Handlers.Core.GetInitialStatus(selectedItem, "")
            itm.RevisionNo = "00"
            itm = SIS.DMS.dmsItems.InsertData(itm)
            'Parent to be Refreshed
            Dim aParent() As String = dmstarget.Split("_".ToCharArray)
            ReDim Preserve aParent(aParent.Length - 2)
            mRet.dmsTarget = Join(aParent, "_")
        End Select
      Case "AUTHORIZE"
        Select Case Convert.ToInt32(selectedType)
          Case enumItemTypes.Folder
            '1. Check User Exists
            'itm is FormData
            Dim targetUser As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItemByUserID(itm.UserID)
            If targetUser Is Nothing Then
              '1.1 create User Using itm property
              itm.ItemTypeID = enumItemTypes.User
              itm.Description = itm.FK_DMS_Items_UserID.UserFullName
              itm.FullDescription = itm.Description
              itm.CreatedBy = HttpContext.Current.Session("LoginID")
              itm.CreatedOn = Now
              itm.StatusID = enumDMSStates.Published
              itm.RevisionNo = "00"
              itm = SIS.DMS.dmsItems.InsertData(itm)
              targetUser = itm
            End If
            '2. Create an Authorized in TargetUser MultiItem
            ' For Item Under Authorization
            Dim lItm As New SIS.DMS.dmsMultiItems
            With lItm
              .ItemID = targetUser.ItemID
              .MultiTypeID = enumMultiTypes.Authorized
              .MultiItemID = selectedItem
              .MultiItemTypeID = selectedType
              .MultiSequence = 0
            End With
            lItm = SIS.DMS.dmsMultiItems.InsertData(lItm)
            '3. Create an Item of Authorization Type
            'Using Item Data Property
            If selectedItem <> "" Then
              'NOTE:-Check at entry point, when Authorizing, Only Items can be autorized, NOT ItemType 
              Dim User As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetSessionUserObject()
              Dim grantedByMe As SIS.DMS.dmsItems = itm
              With grantedByMe
                .ItemTypeID = enumItemTypes.Authorization
                .CreatedBy = HttpContext.Current.Session("LoginID")
                .CreatedOn = Now
                .StatusID = enumDMSStates.Created
                .RevisionNo = "00"
                .LinkedItemID = selectedItem
                .LinkedItemTypeID = selectedType
                .ForwardLinkedItemID = targetUser.ItemID
                .ForwardLinkedItemTypeID = enumItemTypes.User
                .BackwardLinkedItemID = User.ItemID
                .BackwardLinkedItemTypeID = enumItemTypes.User
                .FullDescription = .FK_DMS_Items_LinkedItemID.Description & " => " & itm.FK_DMS_Items_UserID.UserFullName
                .Description = .FullDescription
              End With
              grantedByMe = SIS.DMS.dmsItems.InsertData(grantedByMe)
              '4. Create a Link in Myto Me of Authorization
              lItm = New SIS.DMS.dmsMultiItems
              With lItm
                .ItemID = User.ItemID
                .MultiTypeID = enumMultiTypes.Linked
                .MultiItemID = grantedByMe.ItemID
                .MultiItemTypeID = enumItemTypes.Authorization
                .MultiSequence = 0
              End With
              lItm = SIS.DMS.dmsMultiItems.InsertData(lItm)
            End If
            'My Authorization is to be Reloaded at Client
            mRet.dmsTarget = SIS.DMS.Handlers.dmsData.GetUptoUserFromTreeID(dmstarget) & "_" & enumItemTypes.Authorization

        End Select

    End Select


    Dim output As String = json.Serialize(mRet)
    Return output
  End Function

  <WebMethod(EnableSession:=True)>
  Public Function LoadHistory(ByVal context As String) As String
    Dim json As New System.Web.Script.Serialization.JavaScriptSerializer()
    Dim mRet As String = ""
    'Dim aTarget() As String = context.Split("|".ToCharArray)
    Dim atRoot As Boolean = False
    'Dim dmsTarget As String = aTarget(0)
    'Dim dmsType As String = aTarget(1)
    'Dim dmsItem As String = aTarget(2)
    'If dmsItem = "NaN" Then dmsItem = "0"
    'If Not IsNumeric(dmsItem) Then dmsItem = "0"
    'If dmsItem > "0" Then atRoot = False
    Dim tmpItems As New List(Of SIS.DMS.dmsJsonHistory)
    If Not atRoot Then
      tmpItems = SIS.DMS.dmsJsonHistory.dmsHistorySelectList(context)
    End If
    mRet = json.Serialize(tmpItems)
    Return mRet
  End Function
  <WebMethod(EnableSession:=True)>
  Public Function LoadGrid(ByVal context As String) As String
    Dim json As New System.Web.Script.Serialization.JavaScriptSerializer()
    Dim mRet As String = ""
    Dim aTarget() As String = context.Split("|".ToCharArray)
    Dim atRoot As Boolean = True
    Dim dmsTarget As String = aTarget(0)
    Dim dmsType As String = aTarget(1)
    Dim dmsItem As String = aTarget(2)
    If dmsItem = "NaN" Then dmsItem = "0"
    If Not IsNumeric(dmsItem) Then dmsItem = "0"

    If dmsItem > "0" Then atRoot = False

    Dim tmpItem As SIS.DMS.dmsJsonItems = Nothing
    Dim tmpItems As New List(Of SIS.DMS.dmsJsonItems)

    If Not atRoot Then
      'Select All Types of Child Items Including Self
      tmpItems = SIS.DMS.Handlers.dmsData.GetJsonChilds(dmsItem, dmsType)
    Else
      'Select All Linked & Authorized Items of Selected Type
      'For User in dmsTarget
      Dim UserID As String = dmsTarget.Split("_".ToCharArray)(1)
      tmpItems = SIS.DMS.Handlers.dmsData.GetJsonLinked(UserID, dmsType)

    End If
    mRet = json.Serialize(tmpItems)

    Return mRet
  End Function

  <WebMethod(EnableSession:=True)>
  Public Function DeleteItem(ByVal context As String) As String
    Dim json As New System.Web.Script.Serialization.JavaScriptSerializer()
    Dim mRet As String = ""
    Dim aTarget() As String = context.Split("|".ToCharArray)
    Dim atRoot As Boolean = True
    Dim dmsTarget As String = aTarget(0)
    Dim dmsType As String = aTarget(1)
    Dim dmsItem As String = aTarget(2)
    Dim IsSearchItem As Boolean = False
    If dmsItem = "NaN" Then dmsItem = "0"
    If Not IsNumeric(dmsItem) Then dmsItem = "0"

    If aTarget(2) <> "" Then atRoot = False
    If atRoot Then
      mRet = New JavaScriptSerializer().Serialize(New With {
          .isErr = True,
          .errMsg = "Item Types Definition can not be deleted."
      })
      Return mRet
    End If
    Dim cnt As Integer = SIS.DMS.Handlers.dmsData.GetChildsCountForDelete(dmsItem)
    If cnt > 0 Then
      mRet = New JavaScriptSerializer().Serialize(New With {
          .isErr = True,
          .errMsg = "First Delete child Items."
      })
      Return mRet
    End If
    Dim ath As Integer = SIS.DMS.Handlers.dmsData.GetAuthorizationCountForDelete(dmsItem)
    If ath > 0 Then
      mRet = New JavaScriptSerializer().Serialize(New With {
          .isErr = True,
          .errMsg = "First Delete all Links/Authorizations."
      })
      Return mRet
    End If
    Try
      'Get The Type of Immediate Parent to Identify Searched Item
      Dim UserID As String = SIS.DMS.Handlers.dmsData.GetUserIDFromTreeID(dmsTarget)
      If Convert.ToInt32(dmsType) = enumItemTypes.Searches Then
        SIS.DMS.Handlers.dmsData.DeleteSearch(dmsItem, UserID, True)
      Else
        Dim aParentID() As String = dmsTarget.Split("_".ToCharArray)
        Dim ParentID As String = aParentID(aParentID.Length - 2)
        Dim pItm As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItem(ParentID)
        If pItm IsNot Nothing Then
          If pItm.ItemTypeID = enumItemTypes.Searches Then
            SIS.DMS.Handlers.dmsData.DeleteSearch(dmsItem, UserID, False)
          Else
            If SIS.DMS.Handlers.dmsData.DeleteItem(dmsItem) > 0 Then
              mRet = New JavaScriptSerializer().Serialize(New With {
                  .isErr = True,
                  .errMsg = "Item status is not CREATED cannot be deleted."
              })
              Return mRet
            End If
          End If
        Else
          If SIS.DMS.Handlers.dmsData.DeleteItem(dmsItem) > 0 Then
            mRet = New JavaScriptSerializer().Serialize(New With {
                .isErr = True,
                .errMsg = "Item status is not CREATED cannot be deleted."
            })
            Return mRet
          End If
        End If
      End If
    Catch ex As Exception
      mRet = New JavaScriptSerializer().Serialize(New With {
          .isErr = True,
          .errMsg = ex.Message
      })
      Return mRet
    End Try
    'In Delete Parent of Parent is to be refreshed
    Dim aParent() As String = dmsTarget.Split("_".ToCharArray)
    ReDim Preserve aParent(aParent.Length - 3)
    Dim parentTarget As String = Join(aParent, "_")

    mRet = New JavaScriptSerializer().Serialize(New With {
        .isErr = False,
        .errMsg = "",
        .dmsTarget = parentTarget
    })
    Return mRet
  End Function
  <WebMethod(EnableSession:=True)>
  Public Function PublishItem(ByVal context As String) As String
    Dim json As New System.Web.Script.Serialization.JavaScriptSerializer()
    Dim mRet As String = ""
    Dim aTarget() As String = context.Split("|".ToCharArray)
    Dim atRoot As Boolean = True
    Dim dmsTarget As String = aTarget(0)
    Dim dmsType As String = aTarget(1)
    Dim dmsItem As String = aTarget(2)
    If dmsItem = "NaN" Then dmsItem = "0"
    If Not IsNumeric(dmsItem) Then dmsItem = "0"

    If aTarget(2) <> "" Then atRoot = False
    If atRoot Then
      mRet = New JavaScriptSerializer().Serialize(New With {
          .isErr = True,
          .errMsg = "Item Types Definition can not be published."
      })
      Return mRet
    End If
    '1. Is Created
    Dim Itm As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItem(dmsItem)
    If Itm.StatusID <> enumDMSStates.Created Then
      mRet = New JavaScriptSerializer().Serialize(New With {
          .isErr = True,
          .errMsg = "Status is Created, can not be published."
      })
      Return mRet
    End If
    '2. Created by self or Other
    Dim LoggedUser As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetSessionUserObject()
    If LoggedUser Is Nothing Then
      mRet = New JavaScriptSerializer().Serialize(New With {
          .isErr = True,
          .errMsg = "Session Expired, Login again."
      })
      Return mRet
    End If
    Dim canPublish As Boolean = False
    ' is it self created or authorized item
    If Itm.CreatedBy = LoggedUser.UserID Then
      canPublish = True
    Else
      'Get Authorization Record
      Dim auth As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetAuthorizationEntry(Itm, LoggedUser)
      If auth Is Nothing Then
        Dim ParentID As String = Itm.ParentItemID
        Do While ParentID <> ""
          Dim parentItm As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItem(ParentID)
          auth = SIS.DMS.Handlers.dmsData.GetAuthorizationEntry(parentItm, LoggedUser)
          If auth IsNot Nothing Then
            Exit Do
          End If
          ParentID = parentItm.ParentItemID
        Loop
      End If
      If auth Is Nothing Then
        'compare Itm Auth vs User Auth
        If Itm.Publish = enumQualities.NoRestriction AndAlso LoggedUser.Publish = enumQualities.NoRestriction Then
          canPublish = True
        End If
      Else
        'compare auth auth vs User Auth
        If auth.Publish = enumQualities.NoRestriction AndAlso LoggedUser.Publish = enumQualities.NoRestriction Then
          canPublish = True
        End If
      End If
    End If
    If Not canPublish Then
      mRet = New JavaScriptSerializer().Serialize(New With {
          .isErr = True,
          .errMsg = "Item can NOT be published by user."
      })
      Return mRet
    End If
    'Publish Item
    'Check Workflow available
    Dim wf As SIS.DMS.dmsItems = SIS.DMS.Handlers.Core.GetAssociatedWF(Itm.ItemID)
    If wf IsNot Nothing Then
      'Initiate wf
      Itm = SIS.DMS.Handlers.Core.InitiateWF(Itm, LoggedUser, wf)
    Else
      'Publish Item
      'Check Mandatory Requirements Before Publish
      Itm = SIS.DMS.Handlers.Core.PublishItem(Itm, LoggedUser)
    End If
    'In Parent is to be refreshed
    Dim aParent() As String = dmsTarget.Split("_".ToCharArray)
    ReDim Preserve aParent(aParent.Length - 2)
    Dim parentTarget As String = Join(aParent, "_")
    mRet = New JavaScriptSerializer().Serialize(New With {
        .isErr = False,
        .errMsg = "",
        .dmsTarget = parentTarget
    })
    Return mRet
  End Function
  <WebMethod(EnableSession:=True)>
  Public Function GetItem(ByVal context As String, ByVal dmsoption As String) As String
    Dim json As New System.Web.Script.Serialization.JavaScriptSerializer()
    Dim mRet As String = ""
    If context = "NaN" Then
      mRet = New JavaScriptSerializer().Serialize(New With {
          .isErr = True,
          .errMsg = "Item Types Definition can not be used."
      })
      Return mRet
    End If
    Dim tmpItem As SIS.DMS.dmsJsonItems = SIS.DMS.dmsJsonItems.dmsItemsGetByID(context)
    If tmpItem Is Nothing Then
      mRet = New JavaScriptSerializer().Serialize(New With {
          .isErr = True,
          .errMsg = "ItemID is blank or NOT Found"
      })
      Return mRet
    End If
    If dmsoption = "Edit" Then
      If tmpItem.StatusID <> enumDMSStates.Created Then
        mRet = New JavaScriptSerializer().Serialize(New With {
            .isErr = True,
            .errMsg = "Item is not in Created State"
        })
        Return mRet
      End If
    End If
    mRet = New JavaScriptSerializer().Serialize(New With {
        .isErr = False,
        .errMsg = "",
        .strValue = New JavaScriptSerializer().Serialize(tmpItem)
    })
    Return mRet
  End Function

End Class
