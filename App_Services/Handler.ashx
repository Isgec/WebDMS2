﻿
<%@ WebHandler Language="VB" Class="Handler" %>

Imports System
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Web.Script.Serialization
Imports ejiVault
Public Class Handler : Implements IHttpHandler, IRequiresSessionState

  Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
    If Convert.ToBoolean(ConfigurationManager.AppSettings("JoomlaLive")) Then
      UploadLive(context)
    Else
      UploadTest(context)
    End If
  End Sub
  Private Sub UploadLive(ByVal context As HttpContext)
    Dim isErr As Boolean = False
    Dim errMsg As String = ""
    Dim I As Integer = 0
    Dim fCount As Integer = 0
    Dim Req As SIS.DMS.Handlers.dmsTarget = Nothing
    Dim Mapped As Boolean = False
    Try
      'Check if Request is to Upload the File.
      If context.Request.Files.Count > 0 Then
        Dim dmsTarget As String = ""
        For Each key As String In context.Request.Form.AllKeys
          If key.IndexOf("dmsTarget") >= 0 Then
            dmsTarget = context.Request.Form(key)
            Exit For
          End If
        Next
        Dim JS As New System.Web.Script.Serialization.JavaScriptSerializer()
        Req = JS.Deserialize(dmsTarget, GetType(SIS.DMS.Handlers.dmsTarget))
        If Req.Item = "" Then
          isErr = True
          errMsg = "ERROR: Can NOT Upload Files at Root."
          GoTo done
        End If
        Select Case Convert.ToInt32(Req.Type)
          Case enumItemTypes.Folder, enumItemTypes.FolderGroup, enumItemTypes.File, enumItemTypes.FileGroup
          Case Else
            isErr = True
            errMsg = "ERROR: Files can only be uploaded in Folder."
            GoTo done
        End Select
        Dim src As SIS.DMS.dmsItems = SIS.DMS.dmsItems.dmsItemsGetByID(Req.Item)
        Dim ItemID As Integer = Req.dmsID.Split("_".ToCharArray)(1)
        Dim usr As SIS.DMS.dmsItems = SIS.DMS.dmsItems.dmsItemsGetByID(ItemID)

        Dim libPath As String = ""
        Dim LibraryID As String = ""
        Dim tmp As EJI.ediALib = EJI.ediALib.GetActiveLibrary
        libPath = tmp.LibraryPath
        LibraryID = tmp.t_lbcd
        If Not EJI.DBCommon.IsLocalISGECVault Then
          EJI.ediALib.ConnectISGECVault(tmp)
        End If
        Dim tmpPath As String = HttpContext.Current.Server.MapPath("~/../App_Temp")
        Dim tmpFilesToDelete As New ArrayList


        Dim folderPath As String = context.Server.MapPath("~/../App_Temp/")
        For I = 0 To context.Request.Files.Count - 1
          Dim fu As HttpPostedFile = context.Request.Files(I)
          If fu.ContentLength <= 0 Then Continue For
          If fu.FileName = "" Then Continue For
          Dim fileName As String = Path.GetFileName(fu.FileName)
          Dim tmpName As String = IO.Path.GetRandomFileName()
          Dim tmpFile As String = tmpPath & "\\" & tmpName
          If SIS.DMS.Handlers.dmsData.FileAlreadyExists(Req.Item, fileName) Then
            errMsg = "File(s) already exist."
            Continue For
          End If
          fu.SaveAs(tmpFile)
          tmpFilesToDelete.Add(tmpFile)
          fCount = fCount + 1

          Dim LibPDFFile As String = ""
          LibPDFFile = EJI.ediASeries.GetNextFileID

          Dim tmpItm As New SIS.DMS.dmsItems
          With tmpItm
            .ParentItemID = src.ItemID
            .ItemTypeID = enumItemTypes.File
            .Description = fileName
            .FullDescription = fileName
            .CreatedBy = usr.UserID
            .CreatedOn = Now
            .RevisionNo = "00"
            .StatusID = SIS.DMS.Handlers.Core.GetInitialStatus(src.ItemID, "")
            .ShowInList = src.ShowInList
            .BrowseList = src.BrowseList
            .DeleteFile = src.DeleteFile
            .CreateFile = src.CreateFile
            .CreateFolder = src.CreateFolder
            .DeleteFolder = src.DeleteFolder
            .RenameFolder = src.RenameFolder
            .GrantAuthorization = src.GrantAuthorization
            .KeyWords = src.KeyWords
            .CompanyID = src.CompanyID
            .DivisionID = src.DivisionID
            .DepartmentID = src.DepartmentID
            .ProjectID = src.ProjectID
            .WBSID = src.WBSID
            .IsgecVaultID = LibPDFFile
          End With
          tmpItm = SIS.DMS.dmsItems.InsertData(tmpItm)


          Dim tmpVault As EJI.ediAFile = New EJI.ediAFile
          With tmpVault
            .t_drid = EJI.ediASeries.GetNextRecordID
            .t_dcid = LibPDFFile
            .t_hndl = tmpItm.AthHandle
            .t_indx = tmpItm.AthIndex
            .t_prcd = "EJIMAIN"
            .t_fnam = IO.Path.GetFileName(fu.FileName)
            .t_lbcd = LibraryID
            .t_atby = usr.UserID
            .t_aton = Now
            .t_Refcntd = 0
            .t_Refcntu = 0
          End With
          tmpVault = EJI.ediAFile.InsertData(tmpVault)
          Try
            If IO.File.Exists(libPath & "\" & LibPDFFile) Then
              IO.File.Delete(libPath & "\" & LibPDFFile)
            End If
            IO.File.Move(tmpFile, libPath & "\" & LibPDFFile)
          Catch ex As Exception
          End Try
        Next
        'Move command is used, Below code not required
        'For Each str As String In tmpFilesToDelete
        '  IO.File.Delete(str)
        'Next
        If Not EJI.DBCommon.IsLocalISGECVault Then
          EJI.ediALib.DisconnectISGECVault()
        End If
      End If
    Catch ex As Exception
      If Not EJI.DBCommon.IsLocalISGECVault Then
        EJI.ediALib.DisconnectISGECVault()
      End If
      isErr = True
      errMsg = ex.Message
    End Try
done:
    Dim mStr As String = ""
    If Not isErr Then
      mStr = New JavaScriptSerializer().Serialize(New With {
          .iserr = False,
          .main = fCount & " File(s) Uploaded",
          .dmsbase = Req.dmsID,
          .errmsg = errMsg
      })
    Else
      mStr = New JavaScriptSerializer().Serialize(New With {
          .iserr = True,
          .main = errMsg,
          .dmsbase = "",
          .errmsg = errMsg
      })

    End If
    context.Response.StatusCode = CInt(HttpStatusCode.OK)
    context.Response.ContentType = "text/json"
    context.Response.Write(mStr)
    context.Response.End()
  End Sub
  Private Sub UploadTest(ByVal context As HttpContext)
    Dim isErr As Boolean = False
    Dim errMsg As String = ""
    Dim I As Integer = 0
    Dim fCount As Integer = 0
    Dim Req As SIS.DMS.Handlers.dmsTarget = Nothing
    Try
      'Check if Request is to Upload the File.
      If context.Request.Files.Count > 0 Then
        Dim dmsTarget As String = ""
        For Each key As String In context.Request.Form.AllKeys
          If key.IndexOf("dmsTarget") >= 0 Then
            dmsTarget = context.Request.Form(key)
            Exit For
          End If
        Next
        Dim JS As New System.Web.Script.Serialization.JavaScriptSerializer()
        Req = JS.Deserialize(dmsTarget, GetType(SIS.DMS.Handlers.dmsTarget))
        If Req.Item = "" Then
          isErr = True
          errMsg = "ERROR: Can NOT Upload Files at Root."
          GoTo done
        End If
        Select Case Convert.ToInt32(Req.Type)
          Case enumItemTypes.Folder, enumItemTypes.FolderGroup, enumItemTypes.File, enumItemTypes.FileGroup
          Case Else
            isErr = True
            errMsg = "ERROR: Files can only be uploaded in Folder."
            GoTo done
        End Select
        Dim src As SIS.DMS.dmsItems = SIS.DMS.dmsItems.dmsItemsGetByID(Req.Item)
        Dim usrID As Integer = Req.dmsID.Split("_".ToCharArray)(1)
        Dim usr As SIS.DMS.dmsItems = SIS.DMS.dmsItems.dmsItemsGetByID(usrID)

        Dim folderPath As String = context.Server.MapPath("~/../App_Temp/")
        For I = 0 To context.Request.Files.Count - 1
          Dim postedFile As HttpPostedFile = context.Request.Files(I)
          Dim fileName As String = Path.GetFileName(postedFile.FileName)
          If postedFile.ContentLength > 0 AndAlso fileName <> "" Then
            'File Already Exists in Folder
            If SIS.DMS.Handlers.dmsData.FileAlreadyExists(Req.Item, fileName) Then
              errMsg = "File(s) already exist."
              Continue For
            End If
            fCount = fCount + 1
            postedFile.SaveAs(folderPath + fileName)
            Dim tmp As New SIS.DMS.dmsItems
            With tmp
              .ParentItemID = src.ItemID
              .ItemTypeID = enumItemTypes.File
              .Description = fileName
              .FullDescription = fileName
              .CreatedBy = usr.UserID
              .CreatedOn = Now
              .RevisionNo = "00"
              .StatusID = SIS.DMS.Handlers.Core.GetInitialStatus(src.ItemID, "")
              .ShowInList = src.ShowInList
              .BrowseList = src.BrowseList
              .DeleteFile = src.DeleteFile
              .CreateFile = src.CreateFile
              .CreateFolder = src.CreateFolder
              .DeleteFolder = src.DeleteFolder
              .RenameFolder = src.RenameFolder
              .GrantAuthorization = src.GrantAuthorization
              .KeyWords = src.KeyWords
              .CompanyID = src.CompanyID
              .DivisionID = src.DivisionID
              .DepartmentID = src.DepartmentID
              .ProjectID = src.ProjectID
              .WBSID = src.WBSID
            End With
            tmp = SIS.DMS.dmsItems.InsertData(tmp)
            'Files Need not Linked to User
            'Dim lItm As New SIS.DMS.dmsMultiItems
            'With lItm
            '  .ItemID = usr.ItemID
            '  .MultiTypeID = enumMultiTypes.Linked
            '  .MultiItemID = tmp.ItemID
            '  .MultiItemTypeID = enumItemTypes.File
            '  .MultiSequence = 0
            'End With
            'lItm = SIS.DMS.dmsMultiItems.InsertData(lItm)
          End If
        Next
      End If
    Catch ex As Exception
      isErr = True
      errMsg = ex.Message
    End Try
done:
    Dim mStr As String = ""
    If Not isErr Then
      mStr = New JavaScriptSerializer().Serialize(New With {
          .iserr = False,
          .main = fCount & " File(s) Uploaded",
          .dmsbase = Req.dmsID,
          .errmsg = errMsg
      })
    Else
      mStr = New JavaScriptSerializer().Serialize(New With {
          .iserr = True,
          .main = errMsg,
          .dmsbase = "",
          .errmsg = errMsg
      })

    End If
    context.Response.StatusCode = CInt(HttpStatusCode.OK)
    context.Response.ContentType = "text/json"
    context.Response.Write(mStr)
    context.Response.End()

  End Sub
  Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
    Get
      Return False
    End Get
  End Property

End Class
