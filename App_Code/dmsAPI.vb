Imports System
Imports System.Web
Imports System.Xml
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.Web.Mail
Imports System.Net.Mail
Imports System.Web.Script.Serialization
Namespace SIS.DMS
  <DataObject()>
  Partial Public Class UI
    Public Class apiOptions
      Public Property Source As String = ""
      'trRoot, grRoot, hrRoot
      Public Property Options As String = ""
      Public Property Publish As Boolean = False
      Public Shared Function GetOptions(options As String) As SIS.DMS.UI.apiOptions
        Dim mRet As SIS.DMS.UI.apiOptions = Nothing
        If options <> "" Then
          mRet = New SIS.DMS.UI.apiOptions
          mRet = (New JavaScriptSerializer).Deserialize(options, GetType(SIS.DMS.UI.apiOptions))
        End If
        Return mRet
      End Function
    End Class
    Public Class apiResponse
      Public Property err As Boolean = False
      Public Property msg As String = ""
      Public Property nofrm As Boolean = False
      Public Property okmsg As String = ""
      Public Property warnmsg As String = ""
      Public Property Target As String = ""
      Public Property CurrentSearch As String = ""
      Public Property ServerStopped As Boolean = False
      Public Property LastItem As String = ""
      Public Property strHTML As New List(Of String)
    End Class
    Public Class apiRequest
      Public Property ParentExpended As Integer = 0
      Public Property Target As String = ""
      Public Property Parent As String = ""
      Public Property Indent As Integer = 0
      Public Property Expended As Integer = 1
      Public Property Loaded As Integer = 0
      Public Property Childs As Integer = 0
      Public Property Item As Integer = 0
      Public Property Type As Integer = 0
      Public Property Base As String = ""
      Public Shared Function GetRequest(context As String) As SIS.DMS.UI.apiRequest
        Dim mRet As SIS.DMS.UI.apiRequest = Nothing
        If context <> "" Then
          mRet = New SIS.DMS.UI.apiRequest
          mRet = (New JavaScriptSerializer).Deserialize(context, GetType(SIS.DMS.UI.apiRequest))
        End If
        Return mRet
      End Function
    End Class
    Public Class apiFiles
      Public Property fileName As String = ""
      Public Property fileType As String = ""
      Public Property fileSize As String = ""
      Public Property uploaded As Boolean = False
      Public Property warn As Boolean = False
      Public Property err As Boolean = False
      Public Property msg As String = ""
      Public Property cancelled As Boolean = False
      Public Property fileID As Integer = 0
      Public Shared Function GetFiles(filedata As String) As List(Of SIS.DMS.UI.apiFiles)
        Dim mRet As New List(Of SIS.DMS.UI.apiFiles)
        If filedata <> "" Then
          mRet = (New JavaScriptSerializer).Deserialize(filedata, GetType(List(Of SIS.DMS.UI.apiFiles)))
        End If
        Return mRet
      End Function
    End Class
    Public Class toMailID
      Public Property UserName As String = ""
      Public Property EMailID As String = ""
    End Class

    Public Class Core
      Private Shared Sub LoadUser(ID As Integer, mRet As SIS.DMS.UI.apiResponse, Req As SIS.DMS.UI.apiRequest)
        Dim allowedTypes As List(Of SIS.DMS.UI.apiType) = SIS.DMS.UI.apiType.getUsersAllowedTypes(ID)
        For Each at As SIS.DMS.UI.apiType In allowedTypes
          Dim xReq As New SIS.DMS.UI.apiRequest
          With xReq
            .ParentExpended = 1
            .Indent = Req.Indent + 1
            .Childs = GetCountForUserAndType(ID, at.ItemTypeID)
            .Base = "Type"
            .Target = Req.Parent
            .Type = at.ItemTypeID
            .Expended = 0
            .Loaded = 1
            .Item = 0
          End With
          mRet.strHTML.Add(GetTr(at.ItemTypeID, xReq, at))
          Dim AuthorizedItems As List(Of SIS.DMS.UI.apiItem) = GetItemsForUserAndType(ID, xReq.Type)
          For Each ai As SIS.DMS.UI.apiItem In AuthorizedItems
            Dim yReq As New SIS.DMS.UI.apiRequest
            With yReq
              .ParentExpended = 0
              .Indent = xReq.Indent + 1
              If ai.ItemTypeID = enumItemTypes.User Then
                .Childs = 3
                .Base = "User"
              Else
                .Childs = GetCountChildItems(ai.ItemID, ai.ItemTypeID)
                .Base = "Item"
              End If
              .Target = xReq.Parent
              .Type = ai.ItemTypeID
              .Expended = 0
              .Loaded = 0
              .Item = ai.ItemID
            End With
            mRet.strHTML.Add(GetTr(ai.ItemID, yReq, ai))
          Next
        Next

      End Sub
      Public Shared Function strTree(ID As Integer, context As String) As SIS.DMS.UI.apiResponse
        Dim mRet As New SIS.DMS.UI.apiResponse
        Dim Req As SIS.DMS.UI.apiRequest = SIS.DMS.UI.apiRequest.GetRequest(context)
        mRet.Target = Req.Target
        If Req.Target.ToUpper = "TRROOT" Then
          Req.Base = "User"
          Req.Type = enumItemTypes.User
          Req.Item = ID
          Req.ParentExpended = 1
        End If
        Select Case Req.Base.ToUpper
          Case "USER"
            Req.Childs = 3 'default =>folder, project, search
            If Req.Target.ToUpper = "TRROOT" Then
              Req.Expended = 1
              Req.Indent += 1
              Req.Loaded = 1
              Dim tmpStr As String = GetTr(ID, Req, GetItem(ID))
              mRet.strHTML.Add(tmpStr)
            Else
              Req.Expended = 0
              Req.Loaded = 1
              Req.Parent = Req.Target
              ID = Req.Item
            End If
            LoadUser(ID, mRet, Req)
          Case "TYPE"
            Req.Indent += 1
            Req.Base = "Item"
            Req.Expended = 0
            Req.Loaded = 0
            Dim UserID As Integer = Req.Target.Split("_".ToCharArray)(1)
            Dim Parent As String = Req.Target
            Dim AuthorizedItems As List(Of SIS.DMS.UI.apiItem) = GetItemsForUserAndType(UserID, Req.Type)
            For Each ai As SIS.DMS.UI.apiItem In AuthorizedItems
              Req.Childs = GetCountChildItems(ai.ItemID, ai.ItemTypeID)
              mRet.strHTML.Add(GetTr(ai.ItemID, Req, ai))
            Next
          Case "ITEM"
            Req.Indent += 1
            Req.Expended = 0
            Req.Loaded = 0
            Dim ChildItems As List(Of SIS.DMS.UI.apiItem) = GetChildItems(ID, Req.Type)
            For Each ci As SIS.DMS.UI.apiItem In ChildItems
              Req.Childs = GetCountChildItems(ci.ItemID, ci.ItemTypeID)
              mRet.strHTML.Add(GetTr(ci.ItemID, Req, ci))
            Next
        End Select
        Return mRet
      End Function
      Public Shared Function GetTr(ID As Integer, Req As SIS.DMS.UI.apiRequest, tmpItm As Object) As String
        Select Case Req.Base.ToUpper
          Case "USER", "ITEM"
            Req.Item = ID
          Case "TYPE"
            Req.Item = 0
        End Select
        Dim tr As TableRow = Nothing
        Dim td As TableCell = Nothing
        tr = New TableRow
        tr.ID = Req.Target & "_" & ID
        tr.ClientIDMode = ClientIDMode.Static
        tr.CssClass = "dms-row"
        tr.Attributes.Add("onclick", "return dmsScript.treeRowClicked(this,event);")
        tr.Attributes.Add("oncontextmenu", "return dmsScript.showTreeMenu(this,event);")
        tr.Attributes.Add("data-base", Req.Base)
        tr.Attributes.Add("data-indent", Req.Indent)
        tr.Attributes.Add("data-expended", Req.Expended)
        tr.Attributes.Add("data-loaded", Req.Loaded)
        tr.Attributes.Add("data-childs", Req.Childs)
        tr.Attributes.Add("data-item", Req.Item)
        tr.Attributes.Add("data-type", tmpItm.ItemTypeID)
        tr.Attributes.Add("data-data", (New JavaScriptSerializer).Serialize(tmpItm))
        Dim itmProp As SIS.DMS.UI.ItemProperty = SIS.DMS.UI.ItemProperty.GetByID(Req.Item)
        If itmProp IsNot Nothing Then
          tr.Attributes.Add("data-property", (New JavaScriptSerializer).Serialize(itmProp))
        End If
        Req.Parent = tr.ID
        If Req.ParentExpended = 1 Then
          tr.Attributes.Add("style", "display:table-row;")
        Else
          tr.Attributes.Add("style", "display:none;")
        End If
        Dim iTbl As Table = getTreeViewTbl(tr.ID, Req.Indent, Req.Childs, Req.Expended)
        td = New TableCell
        td.Attributes.Add("style", "border-collapse:collapse;border:none;margin:0px;")
        td.Controls.Add(iTbl)
        tr.Cells.Add(td)

        td = New TableCell
        td.Attributes.Add("style", "border-collapse:collapse;border:none;padding:2px;")
        td.Text = GetIconHTML(tmpItm.ItemTypeID, tmpItm.Description)
        iTbl.Rows(0).Cells.Add(td)

        'Get Associated Items
        Dim AssociatedItems As List(Of SIS.DMS.UI.apiItem) = GetAssociatedItems(Req.Item)
        For Each tmpas As SIS.DMS.UI.apiItem In AssociatedItems
          td = New TableCell
          td.Attributes.Add("style", "border-collapse:collapse;border:none;cursor:pointer;padding:2px;")
          td.Attributes.Add("data-itemid", tmpas.ItemID)
          td.Text = tmpas.GetIconHTML
          td.ToolTip = tmpas.Description
          iTbl.Rows(0).Cells.Add(td)
        Next
        td = New TableCell
        td.Attributes.Add("style", "border-collapse:collapse;border:none;margin:0px;text-align:left;cursor:pointer;")
        If Req.Base = "Type" Then
          Select Case Req.Type
            Case enumItemTypes.UnderApprovalToMe
              td.Text = tmpItm.Description & "&nbsp;&nbsp;<span class='btn-danger' style='font-weight:bold;padding:5px;border-radius:10px;'>" & Req.Childs & "</span>"
            Case enumItemTypes.Selected
              td.Text = tmpItm.Description & "&nbsp;&nbsp;<span class='btn-info' style='font-weight:bold;padding:5px;border-radius:10px;'>" & Req.Childs & "</span>"
            Case Else
              td.Text = tmpItm.Description
          End Select
        Else
          td.Text = tmpItm.Description
        End If
        iTbl.Rows(0).Cells.Add(td)
        Return SIS.DMS.UI.Stringify(tr)
      End Function

      Private Shared Function getTreeViewTbl(trID As String, Indent As Integer, Child As Integer, Expended As Integer) As Table
        Dim Tbl As New Table
        Tbl.Attributes.Add("style", "border-collapse:collapse;border:none;")
        Dim tr As New TableRow
        For i As Integer = 1 To Indent
          Dim td As New TableCell
          td.Attributes.Add("style", "border-collapse:collapse;border:none;margin:0px;")
          Dim img As New Image
          img.AlternateText = "Img"
          img.ClientIDMode = ClientIDMode.Static
          If i = Indent Then
            img.CssClass = "treeImg"
            img.ID = "img_" & trID
            img.Attributes.Add("onclick", "return dmsScript.treeImgClicked(this,event);")
            'img.Attributes.Add("ondblclick", "return dmsScript.treeImgClicked(this,event);")
            If Child > 0 Then
              If Expended = 1 Then
                img.ImageUrl = "/Webdms2/TreeImgs/Minus.gif"
              Else
                img.ImageUrl = "/Webdms2/TreeImgs/Plus.gif"
              End If
            Else
              img.ImageUrl = "/Webdms2/TreeImgs/LineTopMidBottom.gif"
            End If
          Else
            img.ImageUrl = "/Webdms2/TreeImgs/LineTopBottom.gif"
          End If
          td.Controls.Add(img)
          tr.Cells.Add(td)
        Next
        Tbl.Rows.Add(tr)
        Return Tbl
      End Function
      Public Shared Function GetCountForUserAndType(ID As Integer, Type As Integer) As Integer
        Dim mRet As Integer = 0
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Dim Sql As String = ""
          Sql &= " select isnull(count(dms_items.itemid),0) as cnt from dms_items "
          Select Case Type
            Case enumItemTypes.UnderApprovalToMe
              Sql &= "   inner join dms_items as bb on dms_items.forwardlinkeditemid=bb.itemid "
              Sql &= "    where dms_items.itemtypeid=" & enumItemTypes.UnderApprovalToMe
              Sql &= "    and dms_items.forwardlinkeditemtypeid=" & enumItemTypes.Workflow
              Sql &= "    and (bb.linkeditemid='" & ID & "' "
              Sql &= "          or "
              Sql &= "          bb.linkeditemid in "
              Sql &= "          (select cc.parentitemid from dms_items as cc "
              Sql &= "           where cc.itemtypeid = " & enumItemTypes.UserGroupUser
              Sql &= "           and cc.linkedItemID = " & ID
              Sql &= "           ) "
              Sql &= "         ) "
            Case enumItemTypes.User
              Sql &= "   WHERE "
              Sql &= "       [DMS_Items].[ItemTypeID] = " & Type
            Case Else
              Sql &= "   left outer join dms_multiitems on dms_multiitems.MULTIITEMID = dms_items.itemid "
              Sql &= "   WHERE "
              Sql &= "       [DMS_MultiItems].[MultiTypeID] IN (" & enumMultiTypes.Authorized & "," & enumMultiTypes.Linked & "," & enumMultiTypes.Created & ")"
              Sql &= "   AND [DMS_MultiItems].[MultiItemTypeID]=" & Type
              Sql &= "   AND [DMS_MultiItems].[ItemID]=" & ID
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


      Public Shared Function GetItemsForUserAndType(ID As Integer, Type As Integer) As List(Of SIS.DMS.UI.apiItem)
        Dim Results As New List(Of SIS.DMS.UI.apiItem)
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Dim Sql As String = ""
          Sql &= "   select distinct dms_items.*,  "
          Sql &= "   dms_states.Description as StatusID_Description "
          Sql &= "   from dms_items "
          Sql &= "   inner join dms_states on dms_states.statusid = dms_items.statusid "
          Select Case Type
            Case enumItemTypes.UnderApprovalToMe
              Sql &= "   inner join dms_items as bb on dms_items.forwardlinkeditemid=bb.itemid "
              Sql &= "    where dms_items.itemtypeid=" & enumItemTypes.UnderApprovalToMe
              Sql &= "    and dms_items.forwardlinkeditemtypeid=" & enumItemTypes.Workflow
              Sql &= "    and (bb.linkeditemid=" & ID
              Sql &= "          or "
              Sql &= "          bb.linkeditemid in "
              Sql &= "          (select cc.parentitemid from dms_items as cc "
              Sql &= "           where cc.itemtypeid = " & enumItemTypes.UserGroupUser
              Sql &= "           and cc.linkedItemID = " & ID
              Sql &= "           ) "
              Sql &= "         ) "
            Case enumItemTypes.User
              Sql &= "   WHERE "
              Sql &= "       [DMS_Items].[ItemTypeID] = " & Type
              Sql &= "   ORDER BY [DMS_Items].[Description] "
            Case Else
              Sql &= "   left outer join dms_multiitems on dms_multiitems.MULTIITEMID = dms_items.itemid "
              Sql &= "   WHERE "
              Sql &= "       [DMS_MultiItems].[MultiTypeID] IN (" & enumMultiTypes.Authorized & "," & enumMultiTypes.Linked & "," & enumMultiTypes.Created & ")"
              Sql &= "   AND [DMS_MultiItems].[MultiItemTypeID]=" & Type
              Sql &= "   AND [DMS_MultiItems].[ItemID]=" & ID
              Sql &= "   ORDER BY [DMS_Items].[Description] "
          End Select
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            Con.Open()
            Dim Reader As SqlDataReader = Cmd.ExecuteReader()
            While (Reader.Read())
              Results.Add(New SIS.DMS.UI.apiItem(Reader))
            End While
            Reader.Close()
          End Using
        End Using
        Return Results
      End Function
      Public Shared Function GetCountChildItems(ID As Integer, Type As Integer) As Integer
        Dim mRet As Integer = 0
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Dim Sql As String = ""
          Select Case Type
            Case enumItemTypes.Searches
              Sql &= " select isnull(count(*),0) as cnt from dms_searchResults "
              Sql &= " WHERE "
              Sql &= " SearchID =" & ID
            Case Else
              Sql &= " select isnull(count(*),0) as cnt from dms_items "
              Sql &= " left outer join DMS_MultiItems on DMS_MultiItems.multiitemid=dms_items.itemid and DMS_MultiItems.MultiTypeID=" & enumMultiTypes.Child
              Sql &= " WHERE "
              Sql &= " dms_items.ParentItemID =" & ID
              Sql &= " or dms_multiitems.ItemID=" & ID
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
      Public Shared Function GetChildItems(ID As Integer, Type As Integer) As List(Of SIS.DMS.UI.apiItem)
        Dim mRet As New List(Of SIS.DMS.UI.apiItem)
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Dim Sql As String = ""
          Sql &= "   select dms_items.*,  "
          Sql &= "   dms_states.Description as StatusID_Description "
          Sql &= "   from dms_items "
          Sql &= "   inner join dms_states on dms_states.statusid = dms_items.statusid "
          Select Case Type
            Case enumItemTypes.Searches
              Sql &= "   INNER JOIN DMS_SearchResults on DMS_SearchResults.ItemID = DMS_Items.ItemID "
              Sql &= "   WHERE "
              Sql &= "       [DMS_SearchResults].[SearchID]=" & ID
              Sql &= "   ORDER BY [DMS_Items].[Description] "
            Case Else
              Sql &= " left outer join DMS_MultiItems on DMS_MultiItems.multiitemid=dms_items.itemid and DMS_MultiItems.MultiTypeID=" & enumMultiTypes.Child
              Sql &= " WHERE "
              Sql &= " dms_items.ParentItemID =" & ID
              Sql &= " or dms_multiitems.ItemID=" & ID
              Sql &= "   ORDER BY [DMS_Items].[ItemTypeID],[DMS_Items].[Description] "
          End Select
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
      Public Shared Function GetIconHTML(ByVal t As Integer, ByVal f As String) As String
        Dim ret As String = ""
        If (t = enumItemTypes.File Or t = enumItemTypes.SelectedFile Or t = enumItemTypes.FileGroup) Then
          Dim ext As String = IO.Path.GetExtension(f).ToUpper
          Select Case ext.Replace(".", "")
            Case "DOC", "DOCX"
              ret = "<i class=""far fa-file-word dms-tree-icon"" style=""color:SteelBlue;""></i>"
            Case "XLS", "XLSX", "XLSM", "CSV"
              ret = "<i class=""far fa-file-excel dms-tree-icon"" style=""color:LimeGreen;""></i>"
            Case "PPT", "PPTX"
              ret = "<i class=""far fa-file-powerpoint dms-tree-icon"" style=""color:Crimson;""></i>"
            Case "JPG", "JPEG", "PNG", "BMP", "GIF"
              ret = "<i class=""far fa-file-image dms-tree-icon"" style=""color:Plum;""></i>"
            Case "MP4", "AVI", "MPG", "MPEG", "MKA"
              ret = "<i class=""far fa-file-video dms-tree-icon"" style=""color:Magenta;""></i>"
            Case "MP3"
              ret = "<i class=""far fa-file-audio dms-tree-icon"" style=""color:LightCoral;""></i>"
            Case "TXT"
              ret = "<i class=""far fa-file-alt dms-tree-icon"" style=""color:Teal;""></i>"
            Case "DWG"
              ret = "<i class=""fas fa-file-code dms-tree-icon"" style=""color:PaleVioletRed;""></i>"
            Case "PDF"
              ret = "<i class=""far fa-file-pdf dms-tree-icon"" style=""color:red;""></i>"
            Case "ZIP", "RAR"
              ret = "<i class=""far fa-file-archive dms-tree-icon"" style=""color:gold;""></i>"
            Case Else
              ret = "<i class=""far fa-file dms-tree-icon"" style=""color:gray;""></i>"
          End Select
        Else
          Select Case t
            Case enumItemTypes.Folder
              ret = "<i class=""fas fa-folder dms-tree-icon"" style=""color:Gold;""></i>"
            Case enumItemTypes.FolderGroup
              ret = "<i class=""far fa-folder dms-tree-icon"" style=""color:orange;""></i>"
            Case enumItemTypes.File
              ret = "<i class=""far fa-file dms-tree-icon"" style=""color:green;""></i>"
            Case enumItemTypes.FileGroup
              ret = "<i class=""fas fa-file dms-tree-icon"" style=""color:green;""></i>"
            Case enumItemTypes.User
              ret = "<i class=""fas fa-user-tie dms-tree-icon"" style=""color:DarkMagenta;""></i>"
            Case enumItemTypes.UserGroup
              ret = "<i class=""fas fa-users dms-tree-icon"" style=""color:Chartreuse;""></i>"
            Case enumItemTypes.Tag
              ret = "<i class=""fas fa-tags dms-tree-icon"" style=""color:blue;""></i>"
            Case enumItemTypes.Link
              ret = "<i class=""fas fa-link dms-tree-icon"" style=""color:darkgray;""></i>"
            Case enumItemTypes.UValue
              ret = "<i class=""fab fa-playstation dms-tree-icon"" style=""color:Crimson;""></i>"
            Case enumItemTypes.Workflow
              ret = "<i class=""fas fa-network-wired dms-tree-icon"" style=""color:BlueViolet;""></i>"
            Case enumItemTypes.Authorization
              ret = "<i class=""fas fa-user-tag dms-tree-icon"" style=""color:DeepPink;""></i>"
            Case enumItemTypes.GrantedToMe
              ret = "<i class=""fas fa-highlighter dms-tree-icon"" style=""color:DarkTurquoise;""></i>"
            Case enumItemTypes.UnderApprovalToMe
              ret = "<i class=""fas fa-envelope-open-text dms-tree-icon"" style=""color:DarkOrange;""></i>"
            Case enumItemTypes.ApprovedByMe
              ret = "<i class=""fas fa-pen-nib dms-tree-icon"" style=""color:FireBrick;""></i>"
            Case enumItemTypes.Searches
              ret = "<i class=""fab fa-searchengin dms-tree-icon"" style=""color:#00bfff;""></i>"
            Case enumItemTypes.Projects
              ret = "<i class=""fas fa-atom dms-tree-icon"" style=""color:#bf00ff;""></i>"
            Case enumItemTypes.UserGroupUser
              ret = "<i class=""fas fa-user-friends dms-tree-icon"" style=""color:coral;""></i>"
            Case enumItemTypes.Selected
              ret = "<i class=""fas fa-globe dms-tree-icon"" style=""color:blue;""></i>"
            Case enumItemTypes.Fevorites
              ret = "<i class=""fab fa-galactic-republic dms-tree-icon"" style=""color:darkgoldenrod;""></i>"
          End Select
        End If
        Return ret
      End Function
      Public Shared Function GetAssociatedItems(ByVal ItemID As Integer) As List(Of SIS.DMS.UI.apiItem)
        Dim mRet As New List(Of SIS.DMS.UI.apiItem)
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Dim Sql As String = ""
          Sql &= "   select dms_items.*,  "
          Sql &= "   dms_states.Description as StatusID_Description "
          Sql &= "   from dms_items "
          Sql &= "   inner join dms_states on dms_states.statusid = dms_items.statusid "

          Sql &= "   INNER JOIN DMS_MultiItems on DMS_MultiItems.MultiItemID = DMS_Items.ItemID "
          Sql &= "   WHERE "
          Sql &= "   [DMS_MultiItems].[MultiTypeID]=" & enumMultiTypes.Associated
          Sql &= "   AND [DMS_MultiItems].[ItemID]=" & ItemID
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
      Public Shared Function RejectWFFiles(itm As SIS.DMS.UI.apiItem, usr As SIS.DMS.UI.apiItem, rootWF As SIS.DMS.UI.apiItem, Remarks As String) As SIS.DMS.UI.apiItem
        With itm
          .StatusID = rootWF.ConvertedStatusID
          .Approved = False
          .Rejected = True
          .CreatedBy = usr.UserID
          .CreatedOn = Now
          .ForwardLinkedItemID = ""
          .ForwardLinkedItemTypeID = ""
          .ActionRemarks = Remarks
        End With
        itm = SIS.DMS.UI.apiItem.UpdateData(itm)
        '===========Create History===========
        If itm.ItemTypeID = enumItemTypes.File Then
          SIS.DMS.dmsHistory.InsertData(itm)
        End If
        '====================================
        Dim cItms As List(Of SIS.DMS.UI.apiItem) = SIS.DMS.UI.Core.GetChildItems(itm.ItemID, itm.ItemTypeID)
        For Each xItm As SIS.DMS.UI.apiItem In cItms
          RejectWFFiles(xItm, usr, rootWF, Remarks)
        Next
        Return itm
      End Function

      Public Shared Function RejectWF(itm As SIS.DMS.UI.apiItem, usr As SIS.DMS.UI.apiItem, Remarks As String) As SIS.DMS.UI.apiItem
        Dim wf As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(itm.ForwardLinkedItemID)
        If itm.StatusID <> wf.ConvertedStatusID Then
          Throw New Exception("Item status does not allow this action.")
        Else
          Dim rootWF As SIS.DMS.UI.apiItem = wf
          If wf.ParentItemID <> "" Then
            rootWF = GetRootWF(wf)
          End If
          With itm
            .StatusID = rootWF.ConvertedStatusID
            .Approved = False
            .Rejected = True
            .CreatedBy = usr.UserID
            .CreatedOn = Now
            .ForwardLinkedItemID = ""
            .ForwardLinkedItemTypeID = ""
            .ActionRemarks = Remarks
          End With
          itm = SIS.DMS.UI.apiItem.UpdateData(itm)
          '===========Create History===========
          If itm.ItemTypeID = enumItemTypes.File Then
            SIS.DMS.dmsHistory.InsertData(itm)
          End If
          '====================================
          Dim xFile As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(itm.LinkedItemID)
          RejectWFFiles(xFile, usr, rootWF, Remarks)
          'After Rejection Delete UnderApprovaltoMe Record
          SIS.DMS.UI.DirectDeleteItem(itm.ItemID)

          'Create Approved By Me Record
          Dim ackItm As New SIS.DMS.UI.apiItem
          With ackItm
            .ItemTypeID = enumItemTypes.ApprovedByMe
            .Description = itm.Description & "=>" & SIS.DMS.UI.apiStatus.GetByID(itm.StatusID).Description
            .FullDescription = ackItm.Description
            .RevisionNo = "00"
            .StatusID = enumDMSStates.Published
            .CreatedBy = usr.UserID
            .CreatedOn = Now
            .LinkedItemID = itm.LinkedItemID
            .LinkedItemTypeID = 3
            .ActionRemarks = Remarks
          End With
          ackItm = SIS.DMS.UI.apiItem.InsertData(ackItm)
          'Link Approved By Me to Me
          Dim LItm As New SIS.DMS.dmsMultiItems
          With LItm
            .ItemID = usr.ItemID
            .MultiTypeID = enumMultiTypes.Created
            .MultiItemTypeID = ackItm.ItemTypeID
            .MultiItemID = ackItm.ItemID
            .MultiSequence = 0
            .CreatedBy = usr.ItemID
            .CreatedOn = Now
          End With
          LItm = SIS.DMS.dmsMultiItems.InsertData(LItm)
          SIS.DMS.UI.dmsEMails.SendEMail(wf, ackItm, usr)
        End If
        Return itm
      End Function

      Public Shared Function ApproveWFFiles(itm As SIS.DMS.UI.apiItem, usr As SIS.DMS.UI.apiItem, nextWF As SIS.DMS.UI.apiItem, Remarks As String) As SIS.DMS.UI.apiItem
        With itm
          .StatusID = nextWF.ConvertedStatusID
          .Approved = True
          .Rejected = False
          .CreatedBy = usr.UserID
          .CreatedOn = Now
          .ForwardLinkedItemID = nextWF.ItemID
          .ForwardLinkedItemTypeID = nextWF.ItemTypeID
          .ActionRemarks = Remarks
        End With
        itm = SIS.DMS.UI.apiItem.UpdateData(itm)
        '===========Create History===========
        If itm.ItemTypeID = enumItemTypes.File Then
          SIS.DMS.dmsHistory.InsertData(itm)
        End If
        '====================================
        Dim cItms As List(Of SIS.DMS.UI.apiItem) = SIS.DMS.UI.Core.GetChildItems(itm.ItemID, itm.ItemTypeID)
        For Each xItm As SIS.DMS.UI.apiItem In cItms
          ApproveWFFiles(xItm, usr, nextWF, Remarks)
        Next
        Return itm
      End Function

      Public Shared Function ApproveWF(itm As SIS.DMS.UI.apiItem, usr As SIS.DMS.UI.apiItem, Remarks As String) As SIS.DMS.UI.apiItem
        Dim wf As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(itm.ForwardLinkedItemID)
        If itm.StatusID <> wf.ConvertedStatusID Then
          Throw New Exception("Item status does not allow this action.")
        Else
          'Get Next wf step
          Dim nextWF As SIS.DMS.UI.apiItem = GetTopOneChild(wf)
          If nextWF IsNot Nothing Then
            With itm
              .StatusID = nextWF.ConvertedStatusID
              .Approved = True
              .Rejected = False
              .CreatedBy = usr.UserID
              .CreatedOn = Now
              .ForwardLinkedItemID = nextWF.ItemID
              .ForwardLinkedItemTypeID = nextWF.ItemTypeID
              .ActionRemarks = Remarks
            End With
            itm = SIS.DMS.UI.apiItem.UpdateData(itm)
            '===========Create History===========
            If itm.ItemTypeID = enumItemTypes.File Then
              SIS.DMS.dmsHistory.InsertData(itm)
            End If
            '====================================
            'update status of Linked files
            Dim xFile As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(itm.LinkedItemID)
            ApproveWFFiles(xFile, usr, nextWF, Remarks)
            'Create Approved By Me Record
            Dim ackItm As New SIS.DMS.UI.apiItem
            With ackItm
              .ItemTypeID = enumItemTypes.ApprovedByMe
              .Description = itm.Description & "=>" & SIS.DMS.UI.apiStatus.GetByID(itm.StatusID).Description
              .FullDescription = ackItm.Description
              .RevisionNo = "00"
              .StatusID = enumDMSStates.Published
              .CreatedBy = usr.UserID
              .CreatedOn = Now
              .ForwardLinkedItemTypeID = nextWF.ItemTypeID
              .ForwardLinkedItemID = nextWF.ItemID
              .LinkedItemID = itm.LinkedItemID
              .LinkedItemTypeID = 3
              .ActionRemarks = Remarks
            End With
            ackItm = SIS.DMS.UI.apiItem.InsertData(ackItm)
            'Link Approved By Me to Me
            Dim LItm As New SIS.DMS.dmsMultiItems
            With LItm
              .ItemID = usr.ItemID
              .MultiTypeID = enumMultiTypes.Created
              .MultiItemTypeID = ackItm.ItemTypeID
              .MultiItemID = ackItm.ItemID
              .MultiSequence = 0
              .CreatedBy = usr.ItemID
              .CreatedOn = Now
            End With
            LItm = SIS.DMS.dmsMultiItems.InsertData(LItm)
            SIS.DMS.UI.dmsEMails.SendEMail(wf, ackItm, usr)
          End If
        End If
        Return itm
      End Function
      Public Shared Function GetTopOneChild(itm As SIS.DMS.UI.apiItem) As SIS.DMS.UI.apiItem
        Dim mRet As SIS.DMS.UI.apiItem = Nothing
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Con.Open()
          Dim Sql As String = " select top 1 * from dms_items "
          Sql &= "   WHERE "
          Sql &= "   ParentItemID=" & itm.ItemID
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            Dim Reader As SqlDataReader = Cmd.ExecuteReader()
            If Reader.Read() Then
              mRet = New SIS.DMS.UI.apiItem(Reader)
            End If
            Reader.Close()
          End Using
        End Using
        Return mRet

      End Function

      Public Shared Function GetRootWF(itm As SIS.DMS.UI.apiItem) As SIS.DMS.UI.apiItem
        Dim mRet As SIS.DMS.UI.apiItem = Nothing
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Con.Open()
          Dim Sql As String = " select * from dms_items "
          Sql &= "   WHERE "
          Sql &= "   ItemID=" & itm.ParentItemID
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            Dim Reader As SqlDataReader = Cmd.ExecuteReader()
            If Reader.Read() Then
              mRet = New SIS.DMS.UI.apiItem(Reader)
            End If
            Reader.Close()
          End Using
        End Using
        If mRet.ParentItemID <> "" Then
          mRet = GetRootWF(mRet)
        End If
        Return mRet
      End Function

      Public Shared Function InitiateWFFiles(itm As SIS.DMS.UI.apiItem, usr As SIS.DMS.UI.apiItem, nextWF As SIS.DMS.UI.apiItem) As SIS.DMS.UI.apiItem
        With itm
          .StatusID = nextWF.ConvertedStatusID
          .CreatedBy = usr.UserID
          .CreatedOn = Now
          .ForwardLinkedItemID = nextWF.ItemID
          .ForwardLinkedItemTypeID = nextWF.ItemTypeID
        End With
        itm = SIS.DMS.UI.apiItem.UpdateData(itm)
        '===========Create History===========
        If itm.ItemTypeID = enumItemTypes.File Then
          SIS.DMS.dmsHistory.InsertData(itm)
        End If
        '====================================
        Dim cItms As List(Of SIS.DMS.UI.apiItem) = SIS.DMS.UI.Core.GetChildItems(itm.ItemID, itm.ItemTypeID)
        For Each cItm As SIS.DMS.UI.apiItem In cItms
          InitiateWFFiles(cItm, usr, nextWF)
        Next
        Return itm
      End Function

      Public Shared Function InitiateWF(itm As SIS.DMS.UI.apiItem, usr As SIS.DMS.UI.apiItem, ByVal WF As SIS.DMS.UI.apiItem) As SIS.DMS.UI.apiItem
        If itm.StatusID <> WF.ConvertedStatusID Then
          Return itm
        Else
          'Get Next wf step
          Dim nextWF As SIS.DMS.UI.apiItem = GetTopOneChild(WF)
          If nextWF IsNot Nothing Then
            Dim xItm As New SIS.DMS.UI.apiItem
            With xItm
              .ItemTypeID = enumItemTypes.UnderApprovalToMe
              .Description = itm.Description
              .FullDescription = itm.Description
              .RevisionNo = "00"
              .CreatedBy = usr.UserID
              .CreatedOn = Now
              .StatusID = nextWF.ConvertedStatusID
              .ForwardLinkedItemTypeID = enumItemTypes.Workflow
              .ForwardLinkedItemID = nextWF.ItemID
              .LinkedItemTypeID = itm.ItemTypeID
              .LinkedItemID = itm.ItemID
            End With
            SIS.DMS.UI.apiItem.InsertData(xItm)
            With itm
              .StatusID = nextWF.ConvertedStatusID
              .CreatedBy = usr.UserID
              .CreatedOn = Now
              .ForwardLinkedItemID = nextWF.ItemID
              .ForwardLinkedItemTypeID = nextWF.ItemTypeID
            End With
            itm = SIS.DMS.UI.apiItem.UpdateData(itm)
            '===========Create History===========
            If itm.ItemTypeID = enumItemTypes.File Then
              SIS.DMS.dmsHistory.InsertData(itm)
            End If
            '====================================
            Dim cItms As List(Of SIS.DMS.UI.apiItem) = SIS.DMS.UI.Core.GetChildItems(itm.ItemID, itm.ItemTypeID)
            For Each cItm As SIS.DMS.UI.apiItem In cItms
              InitiateWFFiles(cItm, usr, nextWF)
            Next
          End If
        End If
        Return itm
      End Function

      Public Shared Function PublishItem(itm As SIS.DMS.UI.apiItem, usr As SIS.DMS.UI.apiItem) As SIS.DMS.UI.apiItem
        With itm
          .StatusID = enumDMSStates.Published
          .CreatedBy = usr.UserID
          .CreatedOn = Now
        End With
        itm = SIS.DMS.UI.apiItem.UpdateData(itm)
        '===========Create History===========
        If itm.ItemTypeID = enumItemTypes.File Then
          SIS.DMS.dmsHistory.InsertData(itm)
        End If
        '====================================
        Return itm
      End Function

    End Class
    Public Class apiStatus
      Public Property StatusID As Int32 = 0
      Public Property Description As String = ""
      Public Property OpenType As Boolean = False
      Public Property WorkflowType As Boolean = False
      Public Property RevisionTrigger As Boolean = False
      Public Property Active As Boolean = False
      Public Property IsStart As Boolean = False
      Public Property IsFinish As Boolean = False
      Public Property AlertTrigger As Boolean = False
      Public Property AlertWithDownloadLink As Boolean = False
      Public Property Locked As Boolean = False
      Public Property Revisable As Boolean = False

      Public Shared Function GetByID(ID As Integer) As apiStatus
        Dim tmp As apiStatus = Nothing
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Dim Sql As String = ""
          Sql &= " select * from dms_States where StatusID=" & ID
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = Sql
            Con.Open()
            Dim rd As SqlDataReader = Cmd.ExecuteReader
            While rd.Read
              tmp = New apiStatus(rd)
            End While
          End Using
        End Using
        Return tmp
      End Function

      Sub New(rd As SqlDataReader)
        SIS.DMS.UI.NewObj(Me, rd)
      End Sub
      Sub New()
        'dummy
      End Sub

    End Class

    <DataObject()>
    Partial Public Class apiItem
      Public Function Status() As SIS.DMS.UI.apiStatus
        Return SIS.DMS.UI.apiStatus.GetByID(StatusID)
      End Function
      Public Property IsAdmin As Boolean = False
      Public Property strIsAdmin As String
        Get
          If IsAdmin Then Return "True" Else Return "False"
        End Get
        Set(value As String)
          If value = "" Or value = "False" Then
            IsAdmin = False
          Else
            IsAdmin = True
          End If
        End Set
      End Property
      Public Property ChildCount As Integer = 0
      Private _UserGroupID As String = ""
      Public Property UserGroupID As String
        Get
          Select Case ItemTypeID
            Case enumItemTypes.Workflow
              Return LinkedItemID
            Case Else
              Return _UserGroupID
          End Select
        End Get
        Set(value As String)
          Select Case ItemTypeID
            Case enumItemTypes.Workflow
              LinkedItemID = value
            Case Else
              _UserGroupID = value
          End Select

        End Set
      End Property
      Public Property WorkflowID As Integer = 0
      Public Property SearchSerialNo As Integer = 0

#Region "********GetICON***********"
      Public ReadOnly Property GetIconHTML() As String
        Get
          Return SIS.DMS.UI.Core.GetIconHTML(ItemTypeID, Description)
        End Get
      End Property
#End Region

#Region "********Properties**********"
      Public Property ItemID As Int32 = 0
      Public Property InheritFromParent As Boolean = False
      Public Property UserID As String = ""
      Public Property Description As String = ""
      Public Property RevisionNo As String = ""
      Public Property ItemTypeID As Int32 = 0
      Public Property StatusID As Int32 = 0
      Public Property StatusID_Description As String = ""
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
      Public Shared Function GetItem(data As String) As SIS.DMS.UI.apiItem
        Dim mRet As SIS.DMS.UI.apiItem = Nothing
        If data <> "" Then
          mRet = New SIS.DMS.UI.apiItem
          mRet = (New JavaScriptSerializer).Deserialize(data, GetType(SIS.DMS.UI.apiItem))
        End If
        Return mRet
      End Function
      <DataObjectMethod(DataObjectMethodType.Select)>
      Public Shared Function apiItemSelectList(ByVal ItemTypeID As String) As List(Of SIS.DMS.UI.apiItem)
        Dim Results As New List(Of SIS.DMS.UI.apiItem)
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = "select ItemID, Description from dms_items where ItemTypeID=" & ItemTypeID & " and parentItemID is NULL order by Description"
            Con.Open()
            Dim Reader As SqlDataReader = Cmd.ExecuteReader()
            While (Reader.Read())
              Results.Add(New SIS.DMS.UI.apiItem(Reader))
            End While
            Reader.Close()
          End Using
        End Using
        Return Results
      End Function

#Region "********* INSERT / UPDATE ***********"
      Public Shared Function InsertData(ByVal Record As SIS.DMS.UI.apiItem) As SIS.DMS.UI.apiItem
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
      Public Shared Function UpdateData(ByVal Record As SIS.DMS.UI.apiItem) As SIS.DMS.UI.apiItem
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
            Con.Open()
            Cmd.ExecuteNonQuery()
          End Using
        End Using
        Return Record
      End Function

#End Region
      Sub New(rd As SqlDataReader)
        SIS.DMS.UI.NewObj(Me, rd)
      End Sub
      Sub New()
        'dummy
      End Sub
    End Class

    Public Class apiType
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
      Sub New(rd As SqlDataReader)
        API.NewObj(Me, rd)
      End Sub
      Sub New()
      End Sub
      Public Shared Function getUsersAllowedTypes(ID As Integer) As List(Of SIS.DMS.UI.apiType)
        Dim mRet As New List(Of apiType)
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
              mRet.Add(New apiType(rd))
            End While
          End Using
        End Using
        '2. Folder should be allowed by default
        If mRet.Find(Function(x) x.ItemTypeID = enumItemTypes.Folder) Is Nothing Then
          mRet.Add(apiType.GetByID(enumItemTypes.Folder))
        End If
        If mRet.Find(Function(x) x.ItemTypeID = enumItemTypes.Projects) Is Nothing Then
          mRet.Add(apiType.GetByID(enumItemTypes.Projects))
        End If
        If mRet.Find(Function(x) x.ItemTypeID = enumItemTypes.UnderApprovalToMe) Is Nothing Then
          mRet.Add(apiType.GetByID(enumItemTypes.UnderApprovalToMe))
        End If
        If mRet.Find(Function(x) x.ItemTypeID = enumItemTypes.ApprovedByMe) Is Nothing Then
          mRet.Add(apiType.GetByID(enumItemTypes.ApprovedByMe))
        End If
        If mRet.Find(Function(x) x.ItemTypeID = enumItemTypes.Searches) Is Nothing Then
          mRet.Add(apiType.GetByID(enumItemTypes.Searches))
        End If
        mRet.Sort(Function(x, y) x.Sequence.CompareTo(y.Sequence))
        Return mRet
      End Function

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

    End Class

    Public Shared Function GetSessionUser() As String
      Return HttpContext.Current.Session("LoginID")
    End Function
    Public Shared Function GetSessionUserObject() As SIS.DMS.UI.apiItem
      Return SIS.DMS.UI.GetUserByUserID(HttpContext.Current.Session("LoginID"))
    End Function

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
    Public Shared Function Stringify(ctl As Control) As String
      Dim sb As StringBuilder = New StringBuilder()
      Dim sw As IO.StringWriter = New IO.StringWriter(sb)
      Dim hw As System.Web.UI.HtmlTextWriter = New System.Web.UI.HtmlTextWriter(sw)
      ctl.RenderControl(hw)
      Return sb.ToString
    End Function
    Public Shared Function GetItem(ByVal ItemID As String) As SIS.DMS.UI.apiItem
      Dim Results As apiItem = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= "   select dms_items.*,  "
        Sql &= "   dms_states.Description as StatusID_Description "
        Sql &= "   from dms_items "
        Sql &= "   inner join dms_states on dms_states.statusid = dms_items.statusid "
        Sql &= " WHERE "
        Sql &= " [DMS_Items].[ItemID]=" & ItemID
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim rd As SqlDataReader = Cmd.ExecuteReader()
          If rd.Read() Then
            Results = New apiItem(rd)
          End If
          rd.Close()
        End Using
      End Using
      If Results IsNot Nothing Then
        Select Case Results.ItemTypeID
          Case enumItemTypes.User
            Results.IsAdmin = GetIsAdmin(Results.ItemID)
        End Select
      End If
      Return Results
    End Function
    Public Shared Function GetUserByUserID(ByVal UserID As String) As SIS.DMS.UI.apiItem
      Dim Results As SIS.DMS.UI.apiItem = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = " "
        Sql &= "   select dms_items.*,  "
        Sql &= "   dms_states.Description as StatusID_Description "
        Sql &= "   from dms_items "
        Sql &= "   inner join dms_states on dms_states.statusid = dms_items.statusid "
        Sql &= "   WHERE "
        Sql &= "   [DMS_Items].[ItemTypeID]=" & enumItemTypes.User
        Sql &= "   AND [DMS_Items].[UserID] = '" & UserID & "'"
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.DMS.UI.apiItem(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      If Results IsNot Nothing Then
        Results.IsAdmin = GetIsAdmin(Results.ItemID)
      End If
      Return Results
    End Function
    Public Shared Function FileExists(ParentID As Integer, FileName As String) As SIS.DMS.UI.apiItem
      Dim Results As apiItem = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= "   select dms_items.*,  "
        Sql &= "   dms_states.Description as StatusID_Description "
        Sql &= "   from dms_items "
        Sql &= "   inner join dms_states on dms_states.statusid = dms_items.statusid "
        Sql &= " WHERE "
        Sql &= " LOWER([DMS_Items].[Description])= LOWER('" & FileName & "')"
        Sql &= " and dms_Items.ParentItemID=" & ParentID
        Sql &= " and dms_Items.ItemTypeID=" & enumItemTypes.File
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim rd As SqlDataReader = Cmd.ExecuteReader()
          If rd.Read() Then
            Results = New apiItem(rd)
          End If
          rd.Close()
        End Using
      End Using
      Return Results
    End Function

    Public Shared Function DuplicateFound(ByVal Description As String, ByVal ItemTypeID As Integer, ByVal ParentID As Integer) As Boolean
      Dim mRet As Boolean = False
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= "   select isnull(count(ItemID),0) as cnt  "
        Sql &= "   from dms_items "
        Sql &= "   WHERE "
        Sql &= "   lower(description) = lower('" & Description & "')"
        If ParentID > 0 Then
          Sql &= " and parentItemid=" & ParentID
        Else
          Sql &= " and parentItemid is null "
        End If
        Sql &= " and ItemTypeID=" & ItemTypeID
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim tmp As Integer = Cmd.ExecuteScalar
          If tmp > 0 Then mRet = True
        End Using
      End Using
      Return mRet
    End Function
    Public Shared Function GetParentItem(ByVal ItemID As String) As SIS.DMS.UI.apiItem
      Dim Results As apiItem = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= "   select dms_items.*,  "
        Sql &= "   dms_states.Description as StatusID_Description "
        Sql &= "   from dms_items "
        Sql &= "   inner join dms_states on dms_states.statusid = dms_items.statusid "
        Sql &= " WHERE "
        Sql &= " [DMS_Items].[ItemID]=(select ParentItemID from dms_items where ItemID=" & ItemID & ")"
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim rd As SqlDataReader = Cmd.ExecuteReader()
          If rd.Read() Then
            Results = New apiItem(rd)
          End If
          rd.Close()
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function DeleteItem(ItemTypeID As Integer, ItemID As Integer) As Boolean
      Dim mRet As Boolean = True
      Select Case ItemTypeID
        Case enumItemTypes.File
          DirectDeleteItem(ItemID)
        Case Else
          'Checked for Folder, UserGroup
          Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
            Con.Open()
            Try
              Using Cmd As SqlCommand = Con.CreateCommand()
                Cmd.CommandType = CommandType.Text
                Cmd.CommandText = "delete dms_multiItems where MultiItemID=" & ItemID
                Cmd.ExecuteNonQuery()
              End Using
              Using Cmd As SqlCommand = Con.CreateCommand()
                Cmd.CommandType = CommandType.Text
                Cmd.CommandText = "delete dms_multiItems where ItemID=" & ItemID
                Cmd.ExecuteNonQuery()
              End Using
              Using Cmd As SqlCommand = Con.CreateCommand()
                Cmd.CommandType = CommandType.Text
                Cmd.CommandText = "delete dms_Items where ItemID=" & ItemID
                Cmd.ExecuteNonQuery()
              End Using
            Catch ex As Exception
              Throw New Exception("Item is used in Auth...etc.")
            End Try
          End Using
      End Select
      Return mRet
    End Function
    Public Shared Function GetAllTypeLinkedTypeLinkedItem(ItemTypeID As Integer, LinkedItemTypeID As Integer, LinkedItemID As Integer) As List(Of SIS.DMS.UI.apiItem)
      Dim Results As New List(Of SIS.DMS.UI.apiItem)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= "   select *  "
        Sql &= "   from dms_items "
        Sql &= "   where ItemTypeID=" & ItemTypeID
        Sql &= "   and LinkedItemTypeID=" & LinkedItemTypeID
        Sql &= "   and LinkedItemID=" & LinkedItemID
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.UI.apiItem(Reader))
          End While
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function

    Public Shared Function GetParentFolder(ByVal ItemID As String) As SIS.DMS.UI.apiItem
      Dim Results As apiItem = GetItem(ItemID)
      If Results IsNot Nothing Then
        If Results.ItemTypeID <> enumItemTypes.Folder Then
          Results = GetParentFolder(Results.ParentItemID)
        End If
      End If
      Return Results
    End Function
    Public Shared Function IsSelfRef(pItmID As String, toMatch As String) As Boolean
      Dim mRet As Boolean = False
      Dim pItm As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(pItmID)
      If pItm IsNot Nothing Then
        If pItm.ItemTypeID = enumItemTypes.File Then
          If pItm.Description.ToLower = toMatch.ToLower Then
            mRet = True
          Else
            mRet = IsSelfRef(pItm.ParentItemID, toMatch)
          End If
        End If
      End If
      Return mRet
    End Function

    Public Shared Function GetAssociatedWF(ItemID As String) As List(Of SIS.DMS.UI.apiItem)
      Dim tmp As New List(Of SIS.DMS.UI.apiItem)
      Dim mRet As New ArrayList
      mRet = getWorkFlow(ItemID, mRet)
      For Each str As String In mRet
        If str <> "" Then
          tmp.Add(SIS.DMS.UI.GetItem(str))
        End If
      Next
      Return tmp
    End Function
    Private Shared Function getWorkFlow(ItemID As Integer, mRet As ArrayList) As ArrayList
      Dim Parent As String = ""
      Dim Sql As String = ""
      Sql &= " select isnull(MultiItemID,'') as fld  from dms_multiItems where itemid=" & ItemID
      Sql &= " and multitypeID=" & enumMultiTypes.Associated
      Sql &= " and multiItemtypeID=" & enumItemTypes.Workflow
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Dim rd As SqlDataReader = Cmd.ExecuteReader()
          While rd.Read
            mRet.Add(rd("fld"))
          End While
          rd.Close()
        End Using
        Sql = " select isnull(ParentItemID,'') from dms_Items where itemid=" & ItemID
        Parent = ""
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Parent = Cmd.ExecuteScalar()
        End Using
      End Using
      If Parent <> "" Then
        mRet = getWorkFlow(Parent, mRet)
      End If
      Return mRet
    End Function

    Public Shared Function DirectDeleteItem(ItemID As Integer) As Boolean
      Dim mRet As Boolean = True
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "delete dms_Items where ItemID=" & ItemID
          Cmd.ExecuteNonQuery()
        End Using
      End Using
      Return mRet
    End Function
    Private Shared Function GetGroupUsers(grpID As String, gUsers As List(Of SIS.DMS.UI.apiItem)) As List(Of SIS.DMS.UI.apiItem)
      If grpID = "" Then Return gUsers
      Dim Sql As String = ""
      Sql &= " select aa.* from dms_items as aa "
      Sql &= " where aa.itemid in (select bb.linkeditemid from dms_items bb where bb.parentItemid=" & grpID & ") "
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Dim rd As SqlDataReader = Cmd.ExecuteReader()
          While rd.Read
            gUsers.Add(New SIS.DMS.UI.apiItem(rd))
          End While
          rd.Close()
        End Using
      End Using
      Return gUsers
    End Function

    Public Shared Function ItemExistsInGroup(groupID As Integer, ItemID As Integer) As Boolean
      Dim mRet As Boolean = False
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= " select isnull(count(*),0) as cnt from dms_items "
        Sql &= " left outer join DMS_MultiItems on DMS_MultiItems.multiitemid=dms_items.itemid and DMS_MultiItems.MultiTypeID=" & enumMultiTypes.Child
        Sql &= " WHERE "
        Sql &= " dms_items.LinkedItemID='" & ItemID & "'"
        Sql &= " and ( "
        Sql &= " dms_items.ParentItemID =" & groupID
        Sql &= " or dms_multiitems.ItemID=" & groupID
        Sql &= " ) "
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Dim tmp As Integer = Cmd.ExecuteScalar
          If tmp > 0 Then mRet = True
        End Using
      End Using
      Return mRet
    End Function

    Public Shared Function DeleteUserAuthByGroup(groupItemID As Integer, UserID As Integer) As SIS.DMS.UI.apiResponse
      'ReqItem is UserGroupUser Recod where
      '  -ParentItemID => User Group
      '  -LinkedItemID => User
      'Therefore =>First Select all authorization record ItemTypeID=enumItemTypes.Authorization 
      'AND LinkedItemTypeID=enumItemType.UserGroup
      'AND LinkedItemID = ReqItm.ParentItemID
      'In Each Auth Record
      '  -ForwardLinkedItemID => Target on which auth is given
      '  -ForwardLinkedItemTypeID => Type of Target
      '====>Delete from Users MultiItem where MultiTypeID = Authorized
      '     AND MultiItemTypeID = Type Of Target
      '     AND MultiItemID = Target on which auth is given
      Dim mRet As New SIS.DMS.UI.apiResponse
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= "   delete dms_multiItems  "
        Sql &= "   where ItemID=" & UserID
        Sql &= "   AND MultiTypeID=" & enumMultiTypes.Authorized
        Sql &= "   AND MultiItemID in (select ForwardLinkedItemID from dms_Items "
        Sql &= "      where ItemTypeID=" & enumItemTypes.Authorization
        Sql &= "        and LinkedItemTypeID=" & enumItemTypes.UserGroup
        Sql &= "        and LinkedItemID=" & groupItemID
        Sql &= "      )"
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Try
            Cmd.ExecuteNonQuery()
          Catch ex As Exception
            mRet.err = True
            mRet.msg = ex.Message
          End Try
        End Using
      End Using
      Return mRet
    End Function
    Public Shared Function DeleteGroupAuth(folderID As Integer, groupID As Integer) As SIS.DMS.UI.apiResponse
      Dim mRet As New SIS.DMS.UI.apiResponse
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= "   delete dms_multiItems  "
        Sql &= "   where MultiItemID=" & folderID
        Sql &= "   AND MultiTypeID=" & enumMultiTypes.Authorized
        Sql &= "   AND ItemID in (select LinkedItemID from dms_Items "
        Sql &= "      where ItemTypeID=" & enumItemTypes.UserGroupUser
        Sql &= "        and LinkedItemTypeID=" & enumItemTypes.User
        Sql &= "        and ParentItemID=" & groupID
        Sql &= "      )"
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Try
            Cmd.ExecuteNonQuery()
          Catch ex As Exception
            mRet.err = True
            mRet.msg = ex.Message
          End Try
        End Using
      End Using
      Return mRet
    End Function
    Public Shared Function DeleteUserAuth(folderID As Integer, userID As Integer) As SIS.DMS.UI.apiResponse
      Dim mRet As New SIS.DMS.UI.apiResponse
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= "   delete dms_multiItems  "
        Sql &= "   where MultiItemID=" & folderID
        Sql &= "   AND MultiTypeID=" & enumMultiTypes.Authorized
        Sql &= "   AND ItemID =" & userID
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Con.Open()
          Try
            Cmd.ExecuteNonQuery()
          Catch ex As Exception
            mRet.err = True
            mRet.msg = ex.Message
          End Try
        End Using
      End Using
      Return mRet
    End Function
    Public Shared Function DeleteMultiItems(Optional ByVal ItemID As String = "", Optional ByVal MultiTypeID As String = "", Optional ByVal MultiItemTypeID As String = "", Optional ByVal MultiItemID As String = "") As SIS.DMS.UI.apiResponse
      Dim mRet As New SIS.DMS.UI.apiResponse
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Dim Sql As String = ""
        Sql &= " DELETE DMS_MultiItems "
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
          Try
            Cmd.ExecuteNonQuery()
          Catch ex As Exception
            mRet.err = True
            mRet.msg = ex.Message
          End Try
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

    Public Class dmsEMails
      Public Shared Shadows Function SendEMail(wf As SIS.DMS.UI.apiItem, itm As SIS.DMS.UI.apiItem, usr As SIS.DMS.UI.apiItem) As String
        If Convert.ToBoolean(ConfigurationManager.AppSettings("Testing")) Then Return ""
        Dim mRet As String = ""
        Dim First As Boolean = True
        Dim Cnt As Integer = 0
        Dim mRecipients As New StringBuilder
        Dim maySend As Boolean = False

        Dim oClient As SmtpClient = New SmtpClient()
        Dim oMsg As System.Net.Mail.MailMessage = New System.Net.Mail.MailMessage()
        Dim ad As MailAddress = Nothing
        Dim ErrAD As New MailAddress("lalit@isgec.co.in", "Lalit Gupta")
        'From EMail
        If usr.EMailID = "" Then
          oMsg.From = ErrAD
        Else
          ad = New MailAddress(usr.EMailID, usr.Description)
          oMsg.From = ad
        End If
        'CC E-Mail IDs
        Dim gUsers As New List(Of SIS.DMS.UI.apiItem)
        gUsers = SIS.DMS.UI.GetGroupUsers(wf.LinkedItemID, gUsers)
        For Each gUsr As SIS.DMS.UI.apiItem In gUsers
          ad = Nothing
          Try
            ad = New MailAddress(gUsr.EMailID, gUsr.Description)
          Catch ex As Exception
          End Try
          If ad IsNot Nothing Then
            If Not oMsg.CC.Contains(ad) Then
              oMsg.CC.Add(ad)
            End If
          End If
        Next
        'To E-Mail ID
        'Get File from Itm
        Dim ItemProperty As SIS.DMS.UI.ItemProperty = SIS.DMS.UI.ItemProperty.GetByID(itm.LinkedItemID)
        Dim CreatedBy As SIS.QCM.qcmUsers = SIS.QCM.qcmUsers.qcmUsersGetByID(ItemProperty.CreatedBy)
        If CreatedBy.EMailID <> "" Then
          ad = New MailAddress(CreatedBy.EMailID, CreatedBy.UserFullName)
          oMsg.To.Add(ad)
        Else
          oMsg.To.Add(ErrAD)
        End If
        Dim ApprovedBy As SIS.QCM.qcmUsers = SIS.QCM.qcmUsers.qcmUsersGetByID(ItemProperty.ApprovedBy)
        If ApprovedBy IsNot Nothing Then
          If ApprovedBy.EMailID <> "" Then
            ad = New MailAddress(ApprovedBy.EMailID, ApprovedBy.UserFullName)
            oMsg.To.Add(ad)
          End If
        End If
        Dim IssuedBy As SIS.QCM.qcmUsers = SIS.QCM.qcmUsers.qcmUsersGetByID(ItemProperty.IssuedBy)
        If IssuedBy IsNot Nothing Then
          If IssuedBy.EMailID <> "" Then
            ad = New MailAddress(IssuedBy.EMailID, IssuedBy.UserFullName)
            oMsg.To.Add(ad)
          End If
        End If

        oMsg.IsBodyHtml = True
        oMsg.Subject = itm.Description

        Dim sb As New StringBuilder
        With sb
          .AppendLine("<table style=""width:900px"" border=""1"" cellspacing=""1"" cellpadding=""1"">")
          .AppendLine("<tr><td colspan=""2"" align=""center""><h3><b>" & itm.Description & "</b></h2></td></tr>")
          .AppendLine("<tr><td colspan=""2"">" & itm.ActionRemarks & "</td></tr>")
          .AppendLine("<tr><td colspan=""2"">" & usr.Description & "</td></tr>")
          .AppendLine("<tr><td colspan=""2"">" & itm.CreatedOn & "</td></tr>")
          .AppendLine("</table>")
        End With
        Try
          Dim Header As String = ""
          Header = Header & "<html xmlns=""http://www.w3.org/1999/xhtml"">"
          Header = Header & "<head>"
          Header = Header & "<title></title>"
          Header = Header & "</head>"
          Header = Header & "<body>"
          Header = Header & sb.ToString
          Header = Header & "</body></html>"
          oMsg.Body = Header
          oClient.Send(oMsg)
        Catch ex As Exception
          mRet = ex.Message
        End Try
        Return mRet
      End Function
    End Class

  End Class

End Namespace
