Imports System
Imports System.Web
Imports System.Xml
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.Web.Script.Serialization
Namespace SIS.DMS.Handlers
  Partial Public Class Core
    Public Shared Function RejectWF(ByVal itm As SIS.DMS.dmsItems, ByVal usr As SIS.DMS.dmsItems, ByVal Remarks As String) As SIS.DMS.dmsItems
      Dim wf As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItem(itm.LinkedItemID)
      If itm.StatusID <> wf.ConvertedStatusID Then
        Throw New Exception("Item status does not allow this action.")
      Else
        'Create History
        SIS.DMS.dmsHistory.InsertData(itm)
        'Current Status as per Next WF
        With itm
          .StatusID = enumDMSStates.Created
          .Approved = False
          .Rejected = True
          .ActionBy = usr.UserID
          .ActionOn = Now
          .LinkedItemID = ""
          .LinkedItemTypeID = ""
          .ActionRemarks = Remarks
        End With
        itm = SIS.DMS.dmsItems.UpdateData(itm)
      End If
      Return itm
    End Function

    Public Shared Function ApproveWF(ByVal itm As SIS.DMS.dmsItems, ByVal usr As SIS.DMS.dmsItems, ByVal Remarks As String) As SIS.DMS.dmsItems
      Dim wf As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItem(itm.LinkedItemID)
      If itm.StatusID <> wf.ConvertedStatusID Then
        Throw New Exception("Item status does not allow this action.")
      Else
        'Get Next wf step
        Dim nextWF As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetTopOneChild(wf)
        If nextWF IsNot Nothing Then
          'Create History
          SIS.DMS.dmsHistory.InsertData(itm)
          'Current Status as per Next WF
          With itm
            .StatusID = nextWF.ConvertedStatusID
            .Approved = True
            .Rejected = False
            .ActionBy = usr.UserID
            .ActionOn = Now
            .LinkedItemID = nextWF.ItemID
            .LinkedItemTypeID = nextWF.ItemTypeID
            .ActionRemarks = Remarks
          End With
          itm = SIS.DMS.dmsItems.UpdateData(itm)
        End If
      End If
      Return itm
    End Function

    Public Shared Function InitiateWF(ByVal itm As SIS.DMS.dmsItems, ByVal usr As SIS.DMS.dmsItems, ByVal WF As SIS.DMS.dmsItems) As SIS.DMS.dmsItems
      If itm.StatusID <> WF.ConvertedStatusID Then
        Return itm
      Else
        'Get Next wf step
        Dim nextWF As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetTopOneChild(WF)
        If nextWF IsNot Nothing Then
          'Initial History
          SIS.DMS.dmsHistory.InsertData(itm)
          'WF Initiated History
          With itm
            .StatusID = WF.ConvertedStatusID
            .ActionBy = usr.UserID
            .ActionOn = Now
            .LinkedItemID = WF.ItemID
            .LinkedItemTypeID = nextWF.ItemTypeID
            .ActionRemarks = "WorkflowInitiated"
          End With
          SIS.DMS.dmsHistory.InsertData(itm)
          'Current Status as per Next WF
          With itm
            .StatusID = nextWF.ConvertedStatusID
            .ActionBy = ""
            .ActionOn = ""
            .LinkedItemID = nextWF.ItemID
            .LinkedItemTypeID = nextWF.ItemTypeID
            .ActionRemarks = ""
          End With
          itm = SIS.DMS.dmsItems.UpdateData(itm)
        End If
      End If
      Return itm
    End Function
    Public Shared Function PublishItem(ByVal itm As SIS.DMS.dmsItems, ByVal usr As SIS.DMS.dmsItems) As SIS.DMS.dmsItems
      SIS.DMS.dmsHistory.InsertData(itm)
      With itm
        .StatusID = enumDMSStates.Published
        .ActionBy = usr.UserID
        .ActionOn = Now
        .ActionRemarks = "Published"
      End With
      itm = SIS.DMS.dmsItems.UpdateData(itm)
      Return itm
    End Function
    Public Shared Function IsDownloadable(ByVal Item As SIS.DMS.dmsItems) As Boolean
      '1. Published Item can be downloaded
      If Item.StatusID = enumDMSStates.Published Then Return True
      Dim User As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetSessionUserObject()
      '2. In created state, it can be download by creater
      If Item.StatusID = enumDMSStates.Created Then
        If Item.CreatedBy = User.UserID Then Return True
      End If
      '3. Item can be downloaded by creater, ir-respective of its state
      If Item.CreatedBy = User.UserID Then Return True
      '4. In workflow, only by workflow state associated user [User can be user group]
      Dim wf As SIS.DMS.dmsItems = GetAssociatedWF(Item.ItemID)
      If wf Is Nothing Then
        If Item.CreatedBy = User.UserID Then Return True
      End If
      '5. Item state as per wf, and assocoated user with wf state
      Dim wfAllApprovers As New List(Of SIS.DMS.dmsItems)
      Dim wfApprovers As New List(Of SIS.DMS.dmsItems)
      wfApprovers = getItemStateUsers(Item, wf, wfApprovers, wfAllApprovers)
      '6. Can be downloaded approvers of that wf step
      For Each apr As SIS.DMS.dmsItems In wfApprovers
        If apr.ItemID = User.ItemID Then Return True
      Next
      '7. Can be downloaded by all approvers in any wf step
      For Each apr As SIS.DMS.dmsItems In wfAllApprovers
        If apr.ItemID = User.ItemID Then Return True
      Next
      '8. otherwise return false
      Return True
    End Function
    Public Shared Function getItemStateUsers(ByVal Item As SIS.DMS.dmsItems, ByVal wf As SIS.DMS.dmsItems, ByRef Users As List(Of SIS.DMS.dmsItems), ByRef AllUsers As List(Of SIS.DMS.dmsItems)) As List(Of SIS.DMS.dmsItems)
      Dim tmpwfStateUser As SIS.DMS.dmsItems = getAssociatedUser(wf)
      If tmpwfStateUser IsNot Nothing Then
        If Item.StatusID = wf.ConvertedStatusID Then
          Users.Add(tmpwfStateUser)
        End If
        AllUsers.Add(tmpwfStateUser)
        Dim childUsers As List(Of SIS.DMS.dmsItems) = SIS.DMS.Handlers.dmsData.GetChildItems(tmpwfStateUser.ItemID, enumItemTypes.User)
        Users.AddRange(childUsers)
        AllUsers.AddRange(childUsers)
      End If
      Dim NextWfs As List(Of SIS.DMS.dmsItems) = SIS.DMS.Handlers.dmsData.GetChildItems(wf.ItemID, enumItemTypes.Workflow)
      For Each nWF As SIS.DMS.dmsItems In NextWfs
        getItemStateUsers(Item, nWF, Users, AllUsers)
      Next
      Return Users
    End Function
    Public Shared Function getAssociatedUser(ByVal wf As SIS.DMS.dmsItems) As SIS.DMS.dmsItems
      Dim mRet As SIS.DMS.dmsItems = Nothing
      Dim tmpUser As String = ""
      Dim Sql As String = ""
      Sql &= " select top 1 isnull(MultiItemID,'') from dms_multiItems where itemid=" & wf.ItemID
      Sql &= " and multitypeID=" & enumMultiTypes.Associated
      Sql &= " and multiItemtypeID=" & enumItemTypes.User
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          tmpUser = Cmd.ExecuteScalar()
        End Using
      End Using
      If tmpUser <> "" Then mRet = SIS.DMS.dmsItems.dmsItemsGetByID(tmpUser)
      Return mRet
    End Function
    Public Shared Function GetInitialStatus(ByVal ParentItemID As String, Optional ByVal ItemID As String = "") As enumDMSStates
      Dim mRet As enumDMSStates = enumDMSStates.Created
      Dim tmp As SIS.DMS.dmsItems = Nothing
      If ParentItemID <> "" Then
        tmp = getWorkFlow(ParentItemID)
      ElseIf ItemID <> "" Then
        tmp = getWorkFlow(ItemID)
      End If
      If (tmp IsNot Nothing) Then
        mRet = tmp.ConvertedStatusID
      End If
      Return mRet
    End Function
    Public Shared Function GetAssociatedWF(ByVal ItemID As String) As SIS.DMS.dmsItems
      Dim tmp As SIS.DMS.dmsItems = Nothing
      tmp = getWorkFlow(ItemID)
      Return tmp
    End Function
    Private Shared Function getWorkFlow(ByVal ItemID As Integer) As SIS.DMS.dmsItems
      Dim mTmp As Integer = 0
      Dim mRet As String = ""
      Dim Sql As String = ""
      Sql &= " select top 1 isnull(MultiItemID,'') from dms_multiItems where itemid=" & ItemID
      Sql &= " and multitypeID=" & enumMultiTypes.Associated
      Sql &= " and multiItemtypeID=" & enumItemTypes.Workflow
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          mRet = Cmd.ExecuteScalar()
        End Using
      End Using
      If mRet <> "" Then
        Return SIS.DMS.dmsItems.dmsItemsGetByID(mRet)
      Else
        Sql = " select isnull(ParentItemID,'') from dms_Items where itemid=" & ItemID
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Con.Open()
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            mRet = Cmd.ExecuteScalar()
          End Using
        End Using
        If mRet <> "" Then
          Return getWorkFlow(mRet)
        Else
          Return Nothing
        End If
      End If
    End Function

  End Class
  Partial Public Class dmsResponse
    Public Property Request As SIS.DMS.Handlers.dmsRequest = Nothing
    Public Property isError As Boolean = False
    Public Property ErrorMessage As String = ""
    Public Property strHTML As String = ""
    Public Property dmsTarget As String = ""
    Private _Rows As New List(Of TableRow)
    Public Function Rows(Optional ByVal tr As TableRow = Nothing) As List(Of TableRow)
      If tr IsNot Nothing Then _Rows.Add(tr)
      Return _Rows
    End Function


  End Class
  Partial Public Class dmsTarget
    Public Property Base As String = ""
    Public Property Item As String = ""
    Public Property Type As String = ""
    Public Property dmsID As String = ""
    Public Property Loaded As String = "0"
    Public Property Expended As String = "0"
    Public Property Bottom As String = "0"
    Public Property Indent As String = "0"
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
  Partial Public Class ProcessRequest
    Public Shared Function Execute(ByVal Req As SIS.DMS.Handlers.dmsRequest, Optional ByVal IsServerCall As Boolean = False) As SIS.DMS.Handlers.dmsResponse
      Dim Res As New SIS.DMS.Handlers.dmsResponse
      Select Case Req.dmsBase.ToUpper
        Case "ROOT"
          Res = SIS.DMS.Handlers.ProcessRequest.LoadRoot(Req, IsServerCall)
        Case "TYPE"
          Res = SIS.DMS.Handlers.ProcessRequest.LoadType(Req, Nothing, IsServerCall)
        Case "ITEM"
          Res = SIS.DMS.Handlers.ProcessRequest.LoadItem(Req, Nothing, IsServerCall)
      End Select
      Res.Request = Req
      Res.dmsTarget = Req.dmsID
      Return Res
    End Function
    Public Shared Function LoadRoot(ByVal Req As SIS.DMS.Handlers.dmsRequest, Optional ByVal IsServerCalll As Boolean = False) As SIS.DMS.Handlers.dmsResponse
      Dim Res As New SIS.DMS.Handlers.dmsResponse
      Dim tbl As Table = Nothing
      Dim Indent As Integer = Req.dmsIndent
      tbl = New Table
      With tbl
        .Width = Unit.Percentage(100)
        .Attributes.Add("style", "border-collapse:collapse;border:none;")
        .ClientIDMode = ClientIDMode.Static
        .ID = Req.dmsID.Split("_".ToCharArray)(0)
      End With
      Dim tr As TableRow = Nothing
      Dim User As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItem(Req.dmsItem)
      tr = GetRootRow(Req, IsServerCalll)
      If Req.byParent Then
        tbl.Rows.Add(tr)
      End If
      Dim tmpTypes As List(Of SIS.DMS.dmsItemTypes) = SIS.DMS.dmsItemTypes.dmsItemTypesSelectList("Sequence")
      For Each tmpType As SIS.DMS.dmsItemTypes In tmpTypes
        If Not tmpType.ShowInList Then Continue For
        Dim TypReq As New SIS.DMS.Handlers.dmsRequest
        With TypReq
          .dmsBase = "Type"
          .dmsIndent = Indent + 1
          .dmsItem = ""
          .dmsLoaded = "1"
          .dmsExpended = "0"
          .dmsBottom = "1"
          .dmsType = tmpType.ItemTypeID
          .dmsID = Req.dmsID & "_" & tmpType.ItemTypeID
          .byParent = True
        End With
        Dim typRes As SIS.DMS.Handlers.dmsResponse = LoadType(TypReq, tmpType, IsServerCalll)
        For Each tr In typRes.Rows
          tbl.Rows.Add(tr)
        Next
      Next
      Dim sb As StringBuilder = New StringBuilder()
      Dim sw As IO.StringWriter = New IO.StringWriter(sb)
      Dim hw As System.Web.UI.HtmlTextWriter = New System.Web.UI.HtmlTextWriter(sw)
      tbl.RenderControl(hw)
      Res.dmsTarget = Req.dmsID
      Res.strHTML = sb.ToString
      Return Res
    End Function
    Private Shared Function GetRootRow(ByVal Req As SIS.DMS.Handlers.dmsRequest, Optional ByVal IsServerCall As Boolean = False) As TableRow
      Dim Childs As Integer = 1
      Dim Indent As Integer = Req.dmsIndent
      Dim tr As TableRow = Nothing
      Dim td As TableCell = Nothing
      tr = New TableRow
      tr.ClientIDMode = ClientIDMode.Static
      tr.ID = Req.dmsID
      tr.Attributes.Add("onclick", "return tree_toggle(this);")
      tr.Attributes.Add("oncontextmenu", "dms_cmMenu.dmsCmenu(this);")
      tr.Attributes.Add("data-base", Req.dmsBase)
      tr.Attributes.Add("data-expended", Req.dmsExpended)
      tr.Attributes.Add("data-indent", Req.dmsIndent)
      tr.Attributes.Add("data-bottom", Req.dmsBottom)
      tr.Attributes.Add("data-loaded", Req.dmsLoaded)
      tr.Attributes.Add("data-item", Req.dmsItem)
      tr.Attributes.Add("data-type", Req.dmsType)
      tr.Attributes.Add("style", "display:table-row;")
      tr.CssClass = "treeRow"
      td = New TableCell
      With td
        .CssClass = "btn-outline-info"
        .Attributes.Add("style", "border-collapse:collapse;border:none;margin:0px;")
        Dim xTbl As New Table
        xTbl.Attributes.Add("style", "border-collapse:collapse;border:none;")
        Dim xTr As New TableRow
        Dim xTd As New TableCell
        xTd.Attributes.Add("style", "border-collapse:collapse;border:none;")
        Dim mg As New Image
        With mg
          .AlternateText = Req.dmsItem
          If IsServerCall Then
            .ImageUrl = "~/TreeImgs/Minus.gif"
          Else
            .ImageUrl = "../../WebDMS2/TreeImgs/Minus.gif"
          End If
          mg.ClientIDMode = ClientIDMode.Static
          mg.ID = "img_" & tr.ID
          mg.Attributes.Add("onclick", "return tree_chkreload(this);")
          xTd.Controls.Add(mg)
          xTr.Cells.Add(xTd)
        End With
        xTd = New TableCell
        xTd.Attributes.Add("style", "border-collapse:collapse;border:none;cursor:pointer;margin:0px;")
        xTd.Text = "<i class='far fa-user' style='color:Red;'></i>"
        xTr.Cells.Add(xTd)
        xTd = New TableCell
        xTd.Attributes.Add("style", "border-collapse:collapse;border:none;cursor:pointer;margin:0px;")
        Dim itm As SIS.DMS.dmsItems = SIS.DMS.dmsItems.dmsItemsGetByID(Req.dmsItem)
        xTd.Text = itm.Description
        xTr.Cells.Add(xTd)
        xTbl.Rows.Add(xTr)
        td.Controls.Add(xTbl)
      End With
      tr.Cells.Add(td)
      Return tr
    End Function

    Public Shared Function LoadType(ByVal Req As SIS.DMS.Handlers.dmsRequest, Optional ByVal tmpType As SIS.DMS.dmsItemTypes = Nothing, Optional ByVal IsServerCall As Boolean = False) As SIS.DMS.Handlers.dmsResponse
      Dim Res As New SIS.DMS.Handlers.dmsResponse
      Dim mRet As String = ""
      Dim Indent As Integer = Req.dmsIndent
      Dim Prefix As String = Req.dmsID
      Dim ForItemID As String = Req.dmsID.Split("_".ToCharArray)(1) 'User=>ItemID
      If tmpType Is Nothing Then
        tmpType = SIS.DMS.dmsItemTypes.dmsItemTypesGetByID(Req.dmsType)
      End If
      Dim tr As TableRow = GetTypeRow(Req, tmpType, IsServerCall)
      Dim sb As StringBuilder = New StringBuilder()
      Dim sw As IO.StringWriter = New IO.StringWriter(sb)
      Dim hw As System.Web.UI.HtmlTextWriter = New System.Web.UI.HtmlTextWriter(sw)
      tr.RenderControl(hw)
      If Req.byParent Then
        mRet = sb.ToString
        Res.Rows(tr)
      End If
      Select Case Req.dmsType
        Case enumItemTypes.UserGroup
          Dim UserGroups As List(Of SIS.DMS.dmsItems) = SIS.DMS.Handlers.dmsData.GetUserGroups(ForItemID)
          For Each itm As SIS.DMS.dmsItems In UserGroups
            Dim itmReq As New SIS.DMS.Handlers.dmsRequest
            With itmReq
              .dmsBase = "Item"
              .dmsIndent = Indent + 1
              .dmsItem = itm.ItemID
              .dmsLoaded = "0"
              .dmsExpended = "0"
              .dmsBottom = "0"
              .dmsType = itm.ItemTypeID
              .dmsID = Req.dmsID & "_" & itm.ItemID
              .byParent = True
            End With
            Dim itmRes As SIS.DMS.Handlers.dmsResponse = LoadItem(itmReq, itm, IsServerCall)
            For Each xtr As TableRow In itmRes.Rows
              Res.Rows(xtr)
            Next
            mRet &= "##" & itmRes.strHTML
          Next
        Case enumItemTypes.UnderApprovalToMe
          Dim UnderAprs As List(Of SIS.DMS.dmsItems) = SIS.DMS.Handlers.dmsData.GetUnderApprovalToMe(ForItemID)
          For Each itm As SIS.DMS.dmsItems In UnderAprs
            Dim itmReq As New SIS.DMS.Handlers.dmsRequest
            With itmReq
              .dmsBase = "Item"
              .dmsIndent = Indent + 1
              .dmsItem = itm.ItemID
              .dmsLoaded = "0"
              .dmsExpended = "0"
              .dmsBottom = "0"
              .dmsType = itm.ItemTypeID
              .dmsID = Req.dmsID & "_" & itm.ItemID
              .byParent = True
            End With
            Dim itmRes As SIS.DMS.Handlers.dmsResponse = LoadItem(itmReq, itm, IsServerCall)
            For Each xtr As TableRow In itmRes.Rows
              Res.Rows(xtr)
            Next
            mRet &= "##" & itmRes.strHTML
          Next
        Case Else
          Dim LinkedItems As List(Of SIS.DMS.dmsItems) = SIS.DMS.Handlers.dmsData.GetLinkedAndAuthorizedItems(ForItemID, Req.dmsType)
          For Each itm As SIS.DMS.dmsItems In LinkedItems
            '1. check on ItemType
            If Not itm.FK_DMS_Items_ItemTypeID.Active Then Continue For
            If Not itm.FK_DMS_Items_ItemTypeID.ShowInList Then Continue For
            '2. Check on Item
            'If Not itm.BrowseList AndAlso itm.StatusID <> enumDMSStates.Published Then Continue For
            Dim itmReq As New SIS.DMS.Handlers.dmsRequest
            With itmReq
              .dmsBase = "Item"
              .dmsIndent = Indent + 1
              .dmsItem = itm.ItemID
              .dmsLoaded = "0"
              .dmsExpended = "0"
              .dmsBottom = "0"
              .dmsType = itm.ItemTypeID
              .dmsID = Req.dmsID & "_" & itm.ItemID
              .byParent = True
            End With
            Dim itmRes As SIS.DMS.Handlers.dmsResponse = LoadItem(itmReq, itm, IsServerCall)
            For Each xtr As TableRow In itmRes.Rows
              Res.Rows(xtr)
            Next
            mRet = itmRes.strHTML & "##" & mRet
          Next
      End Select
      Res.dmsTarget = Req.dmsID
      Res.strHTML = mRet
      Return Res
    End Function
    Private Shared Function GetTypeRow(ByVal Req As SIS.DMS.Handlers.dmsRequest, ByVal itm As SIS.DMS.dmsItemTypes, Optional ByVal IsServerCall As Boolean = False) As TableRow
      Dim ForItemID As String = Req.dmsID.Split("_".ToCharArray)(1) 'User=>ItemID
      Dim Childs As Integer = 0
      If Req.dmsType = enumItemTypes.UnderApprovalToMe Then
        Childs = SIS.DMS.Handlers.dmsData.GetUnderApprovalToMeCount(ForItemID)
      ElseIf Req.dmsType = enumItemTypes.UserGroup Then
        Childs = SIS.DMS.Handlers.dmsData.GetUserGroupsCount(ForItemID)
      Else
        Childs = SIS.DMS.Handlers.dmsData.GetLinkedAndAuthorizedItemCount(ForItemID, Req.dmsType)
      End If
      Dim Indent As Integer = Req.dmsIndent
      Dim Prefix As String = Req.dmsID
      Dim tr As TableRow = Nothing
      Dim td As TableCell = Nothing
      tr = New TableRow
      tr.ClientIDMode = ClientIDMode.Static
      tr.ID = Prefix
      tr.Attributes.Add("onclick", "return tree_toggle(this);")
      tr.Attributes.Add("oncontextmenu", "dms_cmMenu.dmsCmenu(this);")
      tr.Attributes.Add("data-base", "Type")
      tr.Attributes.Add("data-expended", Req.dmsExpended)
      tr.Attributes.Add("data-indent", Req.dmsIndent)
      tr.Attributes.Add("data-bottom", Childs)
      tr.Attributes.Add("data-loaded", Req.dmsLoaded)
      tr.Attributes.Add("data-item", Req.dmsItem)
      tr.Attributes.Add("data-type", Req.dmsType)
      tr.Attributes.Add("style", "display:table-row;")
      tr.CssClass = "treeRow"

      td = New TableCell
      With td
        .CssClass = "btn-outline-info"
        .Attributes.Add("style", "border-collapse:collapse;border:none;margin:0px;")
        Dim xTbl As New Table
        xTbl.Attributes.Add("style", "border-collapse:collapse;border:none;")
        Dim xTr As New TableRow
        Dim xTd As New TableCell
        For imgs As Integer = 1 To Indent
          xTd = New TableCell
          xTd.Attributes.Add("style", "border-collapse:collapse;border:none;margin:0px;")
          Dim mg As New Image
          With mg
            .AlternateText = Req.dmsType
            If imgs = Indent Then
              If Childs > 0 Then
                If IsServerCall Then
                  .ImageUrl = "~/TreeImgs/Plus.gif"
                Else
                  .ImageUrl = "../../WebDMS2/TreeImgs/Plus.gif"
                End If
              Else
                If IsServerCall Then
                  .ImageUrl = "~/TreeImgs/LineTopMidBottom.gif"
                Else
                  .ImageUrl = "../../WebDMS2/TreeImgs/LineTopMidBottom.gif"
                End If
              End If
              mg.ClientIDMode = ClientIDMode.Static
              mg.ID = "img_" & tr.ID
              mg.Attributes.Add("onclick", "return tree_chkreload(this);")
              xTd.Controls.Add(mg)
              xTr.Cells.Add(xTd)
            Else
              If IsServerCall Then
                .ImageUrl = "~/TreeImgs/LineTopBottom.gif"
              Else
                .ImageUrl = "../../WebDMS2/TreeImgs/LineTopBottom.gif"
              End If
              xTd.Controls.Add(mg)
              xTr.Cells.Add(xTd)
            End If
          End With
        Next
        xTd = New TableCell
        xTd.Attributes.Add("style", "border-collapse:collapse;border:none;cursor:pointer;margin:0px;")
        xTd.Text = GetIconHTML(Req.dmsType, "")

        xTr.Cells.Add(xTd)
        xTd = New TableCell
        xTd.Attributes.Add("style", "border-collapse:collapse;border:none;cursor:pointer;margin:0px;")
        xTd.Text = itm.Description
        xTr.Cells.Add(xTd)
        xTbl.Rows.Add(xTr)
        td.Controls.Add(xTbl)
      End With
      tr.Cells.Add(td)
      Return tr
    End Function
    Public Shared Function LoadItem(ByVal Req As SIS.DMS.Handlers.dmsRequest, Optional ByVal itm As SIS.DMS.dmsItems = Nothing, Optional ByVal IsServerCall As Boolean = False) As SIS.DMS.Handlers.dmsResponse
      Dim Res As New SIS.DMS.Handlers.dmsResponse
      If itm Is Nothing Then
        itm = SIS.DMS.dmsItems.dmsItemsGetByID(Req.dmsItem)
      End If
      Dim tr As TableRow = GetItemRow(Req, itm, IsServerCall)
      Dim sb As StringBuilder = New StringBuilder()
      Dim sw As IO.StringWriter = New IO.StringWriter(sb)
      Dim hw As System.Web.UI.HtmlTextWriter = New System.Web.UI.HtmlTextWriter(sw)
      tr.RenderControl(hw)
      Dim mRet As String = ""
      If Req.byParent Then
        mRet = sb.ToString
        Res.Rows(tr)
      Else
        Dim Indent As Integer = Req.dmsIndent
        Dim tmpChilds As List(Of SIS.DMS.dmsItems) = SIS.DMS.Handlers.dmsData.GetChildItems(Req.dmsItem, Req.dmsType)
        For Each chld As SIS.DMS.dmsItems In tmpChilds
          With chld
            .byParent = True
            .dmsBase = "Item"
            .dmsBottom = "0"
            .dmsExpended = "0"
            .dmsLoaded = "0"
            .dmsIndent = Indent + 1
            .dmsID = Req.dmsID & "_" & chld.ItemID
            .dmsType = chld.ItemTypeID
            .dmsItem = chld.ItemID
          End With
          Dim tmpReq As New SIS.DMS.Handlers.dmsRequest
          With tmpReq
            .byParent = True
            .dmsBase = "Item"
            .dmsBottom = "0"
            .dmsExpended = "0"
            .dmsLoaded = "0"
            .dmsIndent = Indent + 1
            .dmsID = Req.dmsID & "_" & chld.ItemID
            .dmsType = chld.ItemTypeID
            .dmsItem = chld.ItemID
          End With
          Dim xtr As TableRow = GetItemRow(tmpReq, chld, IsServerCall)
          sb = New StringBuilder()
          sw = New IO.StringWriter(sb)
          hw = New System.Web.UI.HtmlTextWriter(sw)
          xtr.RenderControl(hw)
          Res.Rows(xtr)
          If mRet = "" Then
            mRet = sb.ToString
          Else
            mRet &= "##" & sb.ToString
          End If
        Next
      End If
      Res.dmsTarget = Req.dmsID
      Res.strHTML = mRet
      Return Res
    End Function
    Private Shared Function GetItemRow(ByVal Req As SIS.DMS.Handlers.dmsRequest, ByVal dt As SIS.DMS.dmsItems, Optional ByVal IsServerCall As Boolean = False) As TableRow
      Dim Childs As Integer = SIS.DMS.dmsItems.ChildCount(dt)
      Dim Indent As Integer = Req.dmsIndent
      Dim Prefix As String = Req.dmsID
      Dim tr As TableRow = Nothing
      Dim td As TableCell = Nothing
      tr = New TableRow
      tr.ClientIDMode = ClientIDMode.Static
      tr.ID = Prefix
      tr.Attributes.Add("onclick", "return tree_toggle(this);")
      tr.Attributes.Add("oncontextmenu", "dms_cmMenu.dmsCmenu(this);")
      tr.Attributes.Add("data-base", "Item")
      tr.Attributes.Add("data-expended", Req.dmsExpended)
      tr.Attributes.Add("data-indent", Req.dmsIndent)
      tr.Attributes.Add("data-bottom", Childs)
      tr.Attributes.Add("data-loaded", Req.dmsLoaded)
      tr.Attributes.Add("data-item", dt.ItemID)
      tr.Attributes.Add("data-type", dt.ItemTypeID)
      tr.Attributes.Add("data-value", SIS.DMS.dmsItems.GetJsonItem(dt.ItemID))
      tr.Attributes.Add("style", "display:none;")
      tr.CssClass = "treeRow"

      td = New TableCell
      With td
        .CssClass = "btn-outline-info"
        .Attributes.Add("style", "border-collapse:collapse;border:none;margin:0px;")
        Dim xTbl As New Table
        xTbl.Attributes.Add("style", "border-collapse:collapse;border:none;")
        Dim xTr As New TableRow
        Dim xTd As New TableCell
        For imgs As Integer = 1 To Indent
          xTd = New TableCell
          xTd.Attributes.Add("style", "border-collapse:collapse;border:none;margin:0px;")
          Dim mg As New Image
          With mg
            .AlternateText = Req.dmsType
            If imgs = Indent Then
              If Childs > 0 Then
                If IsServerCall Then
                  .ImageUrl = "~/TreeImgs/Plus.gif"
                Else
                  .ImageUrl = "../../WebDMS2/TreeImgs/Plus.gif"
                End If
              Else
                If IsServerCall Then
                  .ImageUrl = "~/TreeImgs/LineTopMidBottom.gif"
                Else
                  .ImageUrl = "../../WebDMS2/TreeImgs/LineTopMidBottom.gif"
                End If
              End If
              mg.ClientIDMode = ClientIDMode.Static
              mg.ID = "img_" & tr.ID
              mg.Attributes.Add("onclick", "return tree_chkreload(this);")
              xTd.Controls.Add(mg)
              xTr.Cells.Add(xTd)
            Else
              If IsServerCall Then
                .ImageUrl = "~/TreeImgs/LineTopBottom.gif"
              Else
                .ImageUrl = "../../WebDMS2/TreeImgs/LineTopBottom.gif"
              End If
              xTd.Controls.Add(mg)
              xTr.Cells.Add(xTd)
            End If
          End With
        Next
        xTd = New TableCell
        xTd.Attributes.Add("style", "border-collapse:collapse;border:none;cursor:pointer;margin:0px;")
        xTd.Text = dt.GetIconHTML
        xTr.Cells.Add(xTd)
        'Get Associated Items
        Dim tmpAssociateds As List(Of SIS.DMS.dmsItems) = SIS.DMS.Handlers.dmsData.GetAssociatedItems(dt.ItemID)
        For Each tmpas As SIS.DMS.dmsItems In tmpAssociateds
          xTd = New TableCell
          xTd.Attributes.Add("style", "border-collapse:collapse;border:none;cursor:pointer;margin:0px;")
          xTd.Attributes.Add("data-itemid", tmpas.ItemID)
          xTd.Text = tmpas.GetIconHTML
          xTd.ToolTip = dt.Description
          xTr.Cells.Add(xTd)
        Next
        xTd = New TableCell
        xTd.Attributes.Add("style", "border-collapse:collapse;border:none;cursor:pointer;margin:0px;")
        xTd.Text = dt.Description
        xTr.Cells.Add(xTd)
        xTbl.Rows.Add(xTr)
        td.Controls.Add(xTbl)
      End With
      tr.Cells.Add(td)
      Return tr
    End Function
    Private Shared Function GetIconHTML(ByVal t As Integer, ByVal f As String) As String
      Dim ret As String = ""
      If (t = 3 Or t = 4) Then
        Dim ext As String = IO.Path.GetExtension(f).ToUpper
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
        Select Case t
          Case 1    ' folder
            ret = "<i class='far fa-folder' style='color:Gold;'></i>"
          Case 2   'folder group
            ret = "<i class='fas fa-folder' style='color:orange;'></i>"
          Case 3   'file
            ret = "<i class='far fa-file' style='color:green;'></i>"
          Case 4   'file group
            ret = "<i class='fas fa-file' style='color:green;'></i>"
          Case 5   'user
            ret = "<i class='fas fa-user-tie' style='color:DarkMagenta;'></i>"
          Case 6   'user group
            ret = "<i class='fas fa-group' style='color:Chartreuse;'></i>"
          Case 7   'tag
            ret = "<i class='fas fa-tags' style='color:blue;'></i>"
          Case 8   'link
            ret = "<i class='fas fa-link' style='color:darkgray;'></i>"
          Case 9   'u-value
            ret = "<i class='fab fa-playstation' style='color:Crimson;'></i>"
          Case 10   'Workflow
            ret = "<i class='fas fa-network-wired' style='color:BlueViolet;'></i>"
          Case 11   'Authorization
            ret = "<i class='fas fa-user-tag' style='color:DeepPink;'></i>"
          Case 12   'granted To Me
            ret = "<i class='fas fa-highlighter' style='color:DarkTurquoise;'></i>"
          Case 13   'under approval
            ret = "<i class='fas fa-envelope-open-text' style='color:DarkOrange;'></i>"
          Case 14   'approved by Me
            ret = "<i class='fas fa-pen-nib' style='color:FireBrick;'></i>"
        End Select
      End If
      Return ret


    End Function

  End Class

#Region " ******* DMS DATA Class **********"
  Partial Public Class dmsData

    Public Shared Function GetUserGroupsCount(ByVal ForItemID As String) As Integer
      Dim user As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetSessionUserObject()
      Dim Results As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= " select isnull(count(dms_items.itemid),0) as cnt from dms_items "
        Sql &= "    where dms_items.itemTypeID=" & enumItemTypes.UserGroup
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Results = Cmd.ExecuteScalar()
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function GetUserGroups(ByVal ForItemID As String) As List(Of SIS.DMS.dmsItems)
      Dim user As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetSessionUserObject()
      Dim Results As New List(Of SIS.DMS.dmsItems)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= GetItemSelectStatement()
        Sql &= "    where dms_items.itemTypeID=" & enumItemTypes.UserGroup
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.dmsItems(Reader))
          End While
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function

    Public Shared Function GetUnderApprovalToMeCount(ByVal ForItemID As String) As Integer
      Dim user As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetSessionUserObject()
      Dim Results As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= " select isnull(count(dms_items.itemid),0) as cnt from dms_items "
        Sql &= "   inner join dms_items as bb on dms_items.linkeditemid=bb.itemid "
        Sql &= "    where dms_items.linkeditemtypeid=" & enumItemTypes.Workflow
        Sql &= "    and (bb.userid='" & user.UserID & "' "
        Sql &= "          or "
        Sql &= "          bb.userid in "
        Sql &= "          (select cc.userid from dms_items as cc "
        Sql &= "           inner join dms_multiitems as dd on dd.itemid=cc.itemid "
        Sql &= "           where cc.itemtypeid = " & enumItemTypes.UserGroup
        Sql &= "           and dd.MultiTypeID = " & enumMultiTypes.Linked
        Sql &= "           and dd.MultiItemTypeID = " & enumItemTypes.User
        Sql &= "           and dd.MultiItemID = " & user.ItemID
        Sql &= "           ) "
        Sql &= "         ) "
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Results = Cmd.ExecuteScalar()
        End Using
      End Using
      Return Results
    End Function

    Public Shared Function GetUnderApprovalToMe(ByVal ForItemID As String) As List(Of SIS.DMS.dmsItems)
      Dim user As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetSessionUserObject()
      Dim Results As New List(Of SIS.DMS.dmsItems)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= GetItemSelectStatement()
        Sql &= "   inner join dms_items as bb on dms_items.linkeditemid=bb.itemid "
        Sql &= "    where dms_items.linkeditemtypeid=" & enumItemTypes.Workflow
        Sql &= "    and (bb.userid='" & user.UserID & "' "
        Sql &= "          or "
        Sql &= "          bb.userid in "
        Sql &= "          (select cc.userid from dms_items as cc "
        Sql &= "           inner join dms_multiitems as dd on dd.itemid=cc.itemid "
        Sql &= "           where cc.itemtypeid = " & enumItemTypes.UserGroup
        Sql &= "           and dd.MultiTypeID = " & enumMultiTypes.Linked
        Sql &= "           and dd.MultiItemTypeID = " & enumItemTypes.User
        Sql &= "           and dd.MultiItemID = " & user.ItemID
        Sql &= "           ) "
        Sql &= "         ) "
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.dmsItems(Reader))
          End While
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function

    Public Shared Function GetLinkedAndAuthorizedItemCount(ByVal ForItemID As String, ByVal ItemTypeID As String) As Integer
      '======================================
      ' Linked and Authorised Items to User
      '======================================
      Dim Results As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= "   SELECT ISNULL(Count(*),0) As cnt FROM DMS_Items "
        Sql &= "   LEFT OUTER JOIN DMS_MultiItems on DMS_MultiItems.MultiItemID = DMS_Items.ItemID "
        Sql &= "   WHERE "
        Sql &= "   [DMS_MultiItems].[MultiTypeID] IN (" & enumMultiTypes.Linked & "," & enumMultiTypes.Authorized & ")"
        Sql &= "   AND [DMS_MultiItems].[ItemID]=" & ForItemID
        Sql &= "   AND [DMS_MultiItems].[MultiItemTypeID]=" & ItemTypeID
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Results = Cmd.ExecuteScalar()
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function GetLinkedAndAuthorizedItems(ByVal ForItemID As String, ByVal ItemTypeID As String) As List(Of SIS.DMS.dmsItems)
      '======================================
      ' Linked and Authorised Items to User
      '======================================
      Dim Results As New List(Of SIS.DMS.dmsItems)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= GetItemSelectStatement()
        Sql &= "   LEFT OUTER JOIN DMS_MultiItems on DMS_MultiItems.MultiItemID = DMS_Items.ItemID "
        Sql &= "   WHERE "
        Sql &= "   [DMS_MultiItems].[MultiTypeID] IN (" & enumMultiTypes.Linked & "," & enumMultiTypes.Authorized & ")"
        Sql &= "   AND [DMS_MultiItems].[ItemID]=" & ForItemID
        Sql &= "   AND [DMS_MultiItems].[MultiItemTypeID]=" & ItemTypeID
        Sql &= "   ORDER BY [DMS_Items].[ItemTypeID], [DMS_Items].[ItemID] "
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.dmsItems(Reader))
          End While
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function

    Public Shared Function FileAlreadyExists(ByVal ItemID As Integer, ByVal FileName As String) As Boolean
      Dim mRet As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= " SELECT isnull(count(itemid),0) as cnt "
        Sql &= "   FROM DMS_ITEMS "
        Sql &= "   WHERE "
        Sql &= "   [DMS_Items].[ParentItemID]=" & ItemID
        Sql &= "   AND UPPER([DMS_Items].[Description]) ='" & FileName & "'"
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          mRet = Cmd.ExecuteScalar
        End Using
      End Using
      Return IIf(mRet = 0, False, True)
    End Function
    Public Shared Function DeleteItem(ByVal ItemID As Integer) As Integer
      'Delete file if file type
      Dim mRet As Integer = 0
      Dim tmp As SIS.DMS.dmsItems = SIS.DMS.Handlers.dmsData.GetItem(ItemID)
      Select Case tmp.ItemTypeID
        Case enumItemTypes.Authorization
          Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
            Con.Open()
            '1. Delete from user authorization
            Dim Sql As String = " delete dms_multiitems where multiitemid=" & tmp.LinkedItemID
            Sql &= " and itemid=" & tmp.ForwardLinkedItemID
            Sql &= " and multitypeid=" & enumMultiTypes.Authorized
            Using Cmd As SqlCommand = Con.CreateCommand()
              Cmd.CommandType = CommandType.Text
              Cmd.CommandText = Sql
              Cmd.ExecuteNonQuery()
            End Using
            '2. delete from my link
            Sql = " delete dms_multiitems where multiitemid=" & tmp.ItemID
            Sql &= " and itemid=" & tmp.BackwardLinkedItemID
            Sql &= " and multitypeid=" & enumMultiTypes.Linked
            Using Cmd As SqlCommand = Con.CreateCommand()
              Cmd.CommandType = CommandType.Text
              Cmd.CommandText = Sql
              Cmd.ExecuteNonQuery()
            End Using
            '3. 
            Sql = " delete dms_items where itemid=" & ItemID
            Sql &= " and itemtypeid=" & enumItemTypes.Authorization
            Using Cmd As SqlCommand = Con.CreateCommand()
              Cmd.CommandType = CommandType.Text
              Cmd.CommandText = Sql
              Cmd.ExecuteNonQuery()
            End Using
          End Using
        Case Else
          If tmp.StatusID = enumDMSStates.Created Then
            Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
              Con.Open()
              Dim Sql As String = " delete from dms_multiitems where multiitemid=" & ItemID
              Using Cmd As SqlCommand = Con.CreateCommand()
                Cmd.CommandType = CommandType.Text
                Cmd.CommandText = Sql
                Cmd.ExecuteNonQuery()
              End Using
              Sql = " delete from dms_items where itemid=" & ItemID
              Using Cmd As SqlCommand = Con.CreateCommand()
                Cmd.CommandType = CommandType.Text
                Cmd.CommandText = Sql
                Cmd.ExecuteNonQuery()
              End Using
            End Using
          Else
            mRet = 1
          End If

      End Select
      Return mRet
    End Function
    Public Shared Function DeleteSearch(ByVal ItemID As Integer, ByVal UserID As String, ByVal IsParent As Boolean) As Integer
      Dim mRet As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Con.Open()
        If IsParent Then
          Dim Sql As String = " delete from dms_SearchResults where multitypeid=" & enumMultiTypes.Searched & " and SearchID=" & ItemID
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            Cmd.ExecuteNonQuery()
          End Using
          Sql = " delete from dms_MultiItems where Multiitemtypeid=" & enumItemTypes.Searches & " and itemid=" & UserID & " and MultiItemID=" & ItemID
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            Cmd.ExecuteNonQuery()
          End Using
          Sql = " delete from dms_items where itemtypeid=" & enumItemTypes.Searches & " and itemid=" & ItemID
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            Cmd.ExecuteNonQuery()
          End Using
        Else
          Dim Sql As String = " delete from dms_SearchResults where multitypeid=" & enumMultiTypes.Searched & " and itemid=" & ItemID
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            Cmd.ExecuteNonQuery()
          End Using
        End If
      End Using
      Return mRet
    End Function

    Public Shared Function GetAuthorizationCountForDelete(ByVal ItemID As Integer) As Integer
      Dim mRet As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = " select isnull(count(itemid),0) as cnt from dms_items "
        Sql &= "   WHERE "
        Sql &= "   [DMS_Items].[LinkedItemID]=" & ItemID
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          mRet = Cmd.ExecuteScalar
        End Using
      End Using
      Return mRet
    End Function
    Public Shared Function GetChildsCountForDelete(ByVal ItemID As Integer) As Integer
      Dim mRet As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = " select isnull(count(itemid),0) as cnt from dms_items "
        Sql &= "   WHERE "
        Sql &= "   [DMS_Items].[ParentItemID]=" & ItemID
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          mRet = Cmd.ExecuteScalar
        End Using
      End Using
      Return mRet
    End Function
    Public Shared Function GetAuthorizationEntry(ByVal itm As SIS.DMS.dmsItems, ByVal usr As SIS.DMS.dmsItems) As SIS.DMS.dmsItems
      Dim Results As SIS.DMS.dmsItems = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = GetItemSelectStatement(True)
        Sql &= "   WHERE "
        Sql &= "   [DMS_Items].[LinkedItemID]=" & itm.ItemID
        'Sql &= "   AND [DMS_Items].[LinkedItemTypeID]=" & itm.ItemTypeID
        'Sql &= "   AND [DMS_Items].[ForwardLinkedItemTypeID]=" & usr.ItemTypeID
        Sql &= "   AND [DMS_Items].[ForwardLinkedItemID]=" & usr.ItemID
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
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
    Public Shared Function GetJsonChilds(ByVal ItemID As Integer, ByVal ItemTypeID As Integer) As List(Of SIS.DMS.dmsJsonItems)
      '=============================================================
      '1. Used to Render Grid to show all types child Including Self
      'So there is No filter on ItemType
      '=============================================================
      Dim mRet As New List(Of SIS.DMS.dmsJsonItems)
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
            mRet.Add(New SIS.DMS.dmsJsonItems(Reader))
          End While
          Reader.Close()
        End Using
      End Using
      Return mRet
    End Function
    Public Shared Function GetJsonLinked(ByVal UserID As Integer, ByVal ItemTypeID As Integer) As List(Of SIS.DMS.dmsJsonItems)
      '=============================================================
      ' Used to Render Grid to show all Linked and Authorised
      ' Items of Selected Type for Selected User
      '=============================================================
      Dim mRet As New List(Of SIS.DMS.dmsJsonItems)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = GetItemSelectStatement()
        Select Case ItemTypeID
          Case enumItemTypes.UserGroup
            Sql = "select * from dms_items "
            Sql &= "   LEFT OUTER JOIN DMS_MultiItems on DMS_MultiItems.ItemID = DMS_Items.ItemID "
            Sql &= "   WHERE "
            Sql &= "       [DMS_Items].[ItemTypeID] = " & ItemTypeID
            Sql &= "   AND [DMS_MultiItems].[MultiTypeID] IN (" & enumMultiTypes.Linked & ", " & enumMultiTypes.Created & ")"
            'Sql &= "   AND [DMS_MultiItems].[MultiItemTypeID]=" & ItemTypeID
            Sql &= "   AND [DMS_MultiItems].[MultiItemID]=" & UserID
            Sql &= "   ORDER BY [DMS_Items].[ItemID] "
          Case Else
            Sql &= "   LEFT OUTER JOIN DMS_MultiItems on DMS_MultiItems.MultiItemID = DMS_Items.ItemID "
            Sql &= "   WHERE "
            Sql &= "       [DMS_MultiItems].[MultiTypeID] IN (" & enumMultiTypes.Linked & "," & enumMultiTypes.Authorized & ")"
            Sql &= "   AND [DMS_MultiItems].[MultiItemTypeID]=" & ItemTypeID
            Sql &= "   AND [DMS_MultiItems].[ItemID]=" & UserID
            Sql &= "   ORDER BY [DMS_Items].[ItemID] "
        End Select
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            mRet.Add(New SIS.DMS.dmsJsonItems(Reader))
          End While
          Reader.Close()
        End Using
      End Using
      Return mRet
    End Function
    Public Shared Function GetAssociatedItems(ByVal ItemID As String) As List(Of SIS.DMS.dmsItems)
      Dim Results As New List(Of SIS.DMS.dmsItems)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = GetItemSelectStatement()
        Sql &= "   INNER JOIN DMS_MultiItems on DMS_MultiItems.MultiItemID = DMS_Items.ItemID "
        Sql &= "   WHERE "
        Sql &= "   [DMS_MultiItems].[MultiTypeID]=" & enumMultiTypes.Associated
        Sql &= "   AND [DMS_MultiItems].[ItemID]=" & ItemID
        Sql &= "   ORDER BY [DMS_ItemTypes8].[Sequence] "
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.dmsItems(Reader))
          End While
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function

    Public Shared Function GetChildFiles(ByVal ItemID As Integer) As List(Of SIS.DMS.dmsItems)
      Dim mRet As New List(Of SIS.DMS.dmsItems)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = GetItemSelectStatement()
        Sql &= "   WHERE "
        Sql &= "   [DMS_Items].[ParentItemID]=" & ItemID
        Sql &= "   AND [DMS_Items].[ItemTypeID]=" & enumItemTypes.File
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

    Public Shared Function GetChildItems(ByVal ItemID As Integer, ByVal ItemTypeID As Integer) As List(Of SIS.DMS.dmsItems)
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
            'Sql &= "   AND [DMS_Items].[ItemTypeID]=" & ItemTypeID
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
    Public Shared Function GetChildItemsCount(ByVal itm As SIS.DMS.dmsItems) As Integer
      Dim mRet As Integer = 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Select Case itm.ItemTypeID
          Case enumItemTypes.Searches
            Sql &= " select isnull(count(*),0) as cnt from dms_searchResults "
            Sql &= " WHERE "
            Sql &= " SearchID =" & itm.ItemID
          Case Else
            Sql &= " select isnull(count(*),0) as cnt from dms_items "
            Sql &= " WHERE "
            Sql &= " ParentItemID =" & itm.ItemID
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
    Public Shared Function GetItem(ByVal ItemID As String) As SIS.DMS.dmsItems
      Dim Results As SIS.DMS.dmsItems = Nothing
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
            Results = New SIS.DMS.dmsItems(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function

    Public Shared Function GetMultiItemSelectStatement() As String
      Dim Sql As String = ""
      Sql &= "   SELECT "
      Sql &= "     [DMS_MultiItems].* , "
      Sql &= "     [DMS_Items1].[Description] AS DMS_Items1_Description, "
      Sql &= "     [DMS_Items2].[Description] AS DMS_Items2_Description, "
      Sql &= "     [DMS_ItemTypes3].[Description] AS DMS_ItemTypes3_Description, "
      Sql &= "     [DMS_MultiTypes4].[Description] AS DMS_MultiTypes4_Description  "
      Sql &= "   FROM [DMS_MultiItems]  "
      Sql &= "   LEFT OUTER JOIN [DMS_Items] AS [DMS_Items1] "
      Sql &= "     ON [DMS_MultiItems].[MultiItemID] = [DMS_Items1].[ItemID] "
      Sql &= "   INNER JOIN [DMS_Items] AS [DMS_Items2] "
      Sql &= "     ON [DMS_MultiItems].[ItemID] = [DMS_Items2].[ItemID] "
      Sql &= "   LEFT OUTER JOIN [DMS_ItemTypes] AS [DMS_ItemTypes3] "
      Sql &= "     ON [DMS_MultiItems].[MultiItemTypeID] = [DMS_ItemTypes3].[ItemTypeID] "
      Sql &= "   LEFT OUTER JOIN [DMS_MultiTypes] AS [DMS_MultiTypes4] "
      Sql &= "     ON [DMS_MultiItems].[MultiTypeID] = [DMS_MultiTypes4].[MultiTypeID] "
      Return Sql
    End Function

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
    Public Class SelectedTarget
      Public Property TargetID As String = ""
      Public Property TreeID As String = ""
      Public Property ItemID As String = ""
      Public Property ItemTypeID As String = ""
      Public Property IsRoot As Boolean = False
      Public Property UserID As String = ""
      Public Property TreeType As String = ""
      Public Property TreeItem As String = ""
      Public Sub New(ByVal dmsTarget As String)
        Dim tmp() As String = dmsTarget.Split("|".ToCharArray)
        TargetID = tmp(0)
        ItemTypeID = tmp(1)
        ItemID = tmp(2)
        If ItemID = "" Or ItemID = "NaN" Then IsRoot = True
        tmp = TargetID.Split("_".ToCharArray)
        TreeID = tmp(0)
        UserID = tmp(1)
        TreeType = tmp(2)
        TreeItem = tmp(tmp.GetUpperBound(0))
      End Sub

    End Class
    Public Shared Function GetSessionUser() As String
      Return HttpContext.Current.Session("LoginID")
    End Function
    Public Shared Function GetSessionUserObject() As SIS.DMS.dmsItems
      Return SIS.DMS.Handlers.dmsData.GetItemByUserID(HttpContext.Current.Session("LoginID"))
    End Function
    Public Shared Function GetUserIDFromTreeID(ByVal TreeID As String) As String
      Dim mRet As String = ""
      If TreeID IsNot Nothing AndAlso TreeID <> "" Then
        Dim aParts() As String = TreeID.Split("_".ToCharArray)
        mRet = aParts(1)
      End If
      Return mRet
    End Function
    Public Shared Function GetUptoUserFromTreeID(ByVal TreeID As String) As String
      Dim mRet As String = ""
      If TreeID IsNot Nothing AndAlso TreeID <> "" Then
        Dim aParts() As String = TreeID.Split("_".ToCharArray)
        mRet = aParts(0) & "_" & aParts(1)
      End If
      Return mRet
    End Function
    Public Shared Function GetTopOneChild(ByVal itm As SIS.DMS.dmsItems) As SIS.DMS.dmsItems
      Dim mRet As SIS.DMS.dmsItems = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Con.Open()
        Dim Sql As String = GetItemSelectStatement(True)
        Sql &= "   WHERE "
        Sql &= "   [DMS_Items].[ParentItemID]=" & itm.ItemID
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            mRet = New SIS.DMS.dmsItems(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return mRet

    End Function
    Public Shared Function GetMultiItems(Optional ByVal ItemID As String = "", Optional ByVal MultiTypeID As String = "", Optional ByVal MultiItemTypeID As String = "", Optional ByVal MultiItemID As String = "") As List(Of SIS.DMS.dmsMultiItems)
      Dim Results As New List(Of SIS.DMS.dmsMultiItems)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= " SELECT * FROM DMS_MultiItems "
        Sql &= "   WHERE 1 = 1 "
        If ItemID <> "" Then
          Sql &= "   and [ItemID]=" & ItemID
        End If
        If MultiTypeID <> "" Then
          Sql &= "  and [MultiTypeID]=" & MultiTypeID
        End If
        If MultiItemTypeID <> "" Then
          Sql &= "  and [MultiItemTypeID]=" & MultiItemTypeID
        End If
        If MultiItemID <> "" Then
          Sql &= "  and [MultiItemID]=" & MultiItemID
        End If
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          Do While Reader.Read()
            Results.Add(New SIS.DMS.dmsMultiItems(Reader))
          Loop
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function

    Public Shared Function GetItemByUserID(ByVal UserID As String) As SIS.DMS.dmsItems
      Dim Results As SIS.DMS.dmsItems = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = GetItemSelectStatement(True)
        Sql &= "   WHERE "
        Sql &= "   [DMS_Items].[ItemTypeID]=" & enumItemTypes.User
        Sql &= "   AND [DMS_Items].[UserID] = '" & UserID & "'"
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
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
    Public Shared Function UpdateMatchingProperties(ByVal s As Object, ByVal t As Object) As Object
      Try
        For Each pi As System.Reflection.PropertyInfo In t.GetType.GetProperties
          If pi.MemberType = Reflection.MemberTypes.Property Then
            If pi.Name.StartsWith("FK_") Then Continue For
            Try
              Dim tmp As Object = CallByName(s, pi.Name, CallType.Get)
              If tmp IsNot Nothing Then
                Try
                  CallByName(t, pi.Name, CallType.Let, CallByName(s, pi.Name, CallType.Get))
                Catch ex As Exception
                End Try
              End If
            Catch ex As Exception
            End Try
          End If
        Next
      Catch ex As Exception
        Dim aa As String = ex.Message
      End Try
      Return t
    End Function

  End Class

#End Region
End Namespace
