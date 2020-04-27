Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Script.Serialization
Imports Ionic
Imports Ionic.Zip
Imports ejiVault
Partial Class dmsDownload
  Inherits System.Web.UI.Page
  Private st As Long = HttpContext.Current.Server.ScriptTimeout
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    HttpContext.Current.Server.ScriptTimeout = Integer.MaxValue
    Dim mt As String = ""
    Dim mo As String = ""
    If Request.QueryString("mt") IsNot Nothing Then
      mt = Request.QueryString("mt")
      If Convert.ToBoolean(ConfigurationManager.AppSettings("JoomlaLive")) Then
        DownloadLive(mt)
      Else
        DownloadTest(mt)
      End If

    End If
  End Sub

  Private Sub DownloadLive(ByVal mt As String)
    Dim aMt() As String = mt.Split("|".ToCharArray)
    Dim dmsType As String = aMt(0)
    Dim dmsItem As String = aMt(1)
    Dim folderPath As String = Context.Server.MapPath("~/../App_Temp/")
    Dim fileName As String = ""
    Dim filePath As String = ""
    Dim libPath As String = ""
    If dmsItem = "NaN" Then dmsItem = 0
    If Not IsNumeric(dmsItem) Then dmsItem = 0

    Dim tmpItem As SIS.DMS.dmsItems = Nothing
    Dim tmpItems As New List(Of SIS.DMS.dmsItems)

    Threading.Thread.Sleep(2000)
    '================Under Approval To Me / Approved By Me ================
    Select Case Convert.ToInt32(dmsType)
      Case enumItemTypes.UnderApprovalToMe, enumItemTypes.ApprovedByMe
        Dim itm As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(dmsItem)
        dmsType = itm.LinkedItemTypeID
        dmsItem = itm.LinkedItemID
    End Select
    '======================================================================
    Select Case Convert.ToInt32(dmsType)
      Case enumItemTypes.File, enumItemTypes.FileGroup
        tmpItem = SIS.DMS.dmsItems.dmsItemsGetByID(dmsItem)
    End Select
    tmpItems = SIS.DMS.Handlers.dmsData.GetChildFiles(dmsItem)
    If tmpItem IsNot Nothing Then tmpItems.Add(tmpItem)
    Dim FileCount As Integer = tmpItems.Count
    'For Each tmp As SIS.DMS.dmsItems In tmpItems
    '  'Copy file from isgec vault to app temp
    '  filePath = folderPath & tmp.Description
    '  If IO.File.Exists(filePath) Then
    '    FileCount += 1
    '    tmpItem = tmp
    '  End If
    'Next
    If FileCount = 0 Then
      Response.Clear()
      Dim x As New System.Web.HttpCookie("fileDownload", "true")
      x.Expires = DateTime.Now.AddYears(-1)
      x.Path = "/"
      Response.AppendCookie(x)
      Response.End()
    ElseIf FileCount = 1 Then
      Response.Clear()
      If SIS.DMS.Handlers.Core.IsDownloadable(tmpItem) Then
        Dim rDoc As EJI.ediAFile = EJI.ediAFile.GetFileByFileID(tmpItem.IsgecVaultID)
        If rDoc IsNot Nothing Then
          Dim rLib As EJI.ediALib = EJI.ediALib.GetLibraryByID(rDoc.t_lbcd)
          If rLib IsNot Nothing Then
            If Not EJI.DBCommon.IsLocalISGECVault Then
              EJI.ediALib.ConnectISGECVault(rLib)
            End If
            libPath = rLib.LibraryPath
            filePath = libPath & "\" & rDoc.t_dcid
            fileName = rDoc.t_fnam
            If IO.File.Exists(filePath) Then
              Response.AppendHeader("content-disposition", "attachment; filename=" & fileName)
              Response.ContentType = SIS.SYS.Utilities.ApplicationSpacific.ContentType(fileName)
              Response.WriteFile(filePath)
            End If
            If Not EJI.DBCommon.IsLocalISGECVault Then
              EJI.ediALib.DisconnectISGECVault()
            End If
          End If
        End If
      End If
      Dim x As New System.Web.HttpCookie("fileDownload", "true")
      x.Path = "/"
      Response.AppendCookie(x)
      Response.End()
    Else
      Dim FilesAtATime As Integer = 9999
      Dim tmpFilesToDelete As New ArrayList
      fileName = "Download.zip"
      Response.Clear()
      Response.AppendHeader("content-disposition", "attachment; filename=" & fileName)
      Response.ContentType = SIS.SYS.Utilities.ApplicationSpacific.ContentType(fileName)
      Using zip As New ZipFile
        zip.CompressionLevel = Zlib.CompressionLevel.Level5
        Dim cnt As Integer = 0
        Dim LibraryID As String = ""
        For Each tmp As SIS.DMS.dmsItems In tmpItems
          If Not SIS.DMS.Handlers.Core.IsDownloadable(tmp) Then Continue For
          Dim rDoc As EJI.ediAFile = EJI.ediAFile.GetFileByFileID(tmp.IsgecVaultID)
          If rDoc IsNot Nothing Then
            Dim rLib As EJI.ediALib = EJI.ediALib.GetLibraryByID(rDoc.t_lbcd)
            If rLib IsNot Nothing Then
              If LibraryID <> rDoc.t_lbcd Then
                If Not EJI.DBCommon.IsLocalISGECVault Then
                  EJI.ediALib.ConnectISGECVault(rLib)
                End If
                LibraryID = rDoc.t_lbcd
              End If
              libPath = rLib.LibraryPath
              filePath = libPath & "\" & rDoc.t_dcid
              fileName = rDoc.t_fnam
              If IO.File.Exists(filePath) Then
                Try
                  zip.AddFile(filePath, "Files").FileName = rDoc.t_fnam
                  tmpFilesToDelete.Add(filePath)
                  cnt += 1
                Catch ex As Exception
                End Try
                If cnt >= FilesAtATime Then
                  Exit For
                End If
              End If
            End If
          End If
        Next
        zip.Save(Response.OutputStream)
        If Not EJI.DBCommon.IsLocalISGECVault Then
          EJI.ediALib.DisconnectISGECVault()
        End If
      End Using
      'For Each str As String In tmpFilesToDelete
      '  Try
      '    Dim fInfo As New FileInfo(str)
      '    fInfo.IsReadOnly = False
      '    IO.File.Delete(str)
      '  Catch ex As Exception
      '  End Try
      'Next
      Dim x As New System.Web.HttpCookie("fileDownload", "true")
      x.Path = "/"
      Response.AppendCookie(x)
      Response.End()
    End If
  End Sub
  Private Sub DownloadTest(ByVal mt As String)
    Dim aMt() As String = mt.Split("|".ToCharArray)
    Dim dmsType As String = aMt(0)
    Dim dmsItem As String = aMt(1)
    Dim folderPath As String = Context.Server.MapPath("~/../App_Temp/")
    Dim fileName As String = ""
    Dim filePath As String = ""
    If dmsItem = "NaN" Then dmsItem = 0
    If Not IsNumeric(dmsItem) Then dmsItem = 0

    Dim tmpItem As SIS.DMS.dmsItems = Nothing
    Dim tmpItems As New List(Of SIS.DMS.dmsItems)

    Threading.Thread.Sleep(2000)
    '================Under Approval To Me / Approved By Me ================
    Select Case Convert.ToInt32(dmsType)
      Case enumItemTypes.UnderApprovalToMe, enumItemTypes.ApprovedByMe
        Dim itm As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(dmsItem)
        dmsType = itm.LinkedItemTypeID
        dmsItem = itm.LinkedItemID
    End Select
    '======================================================================
    Select Case Convert.ToInt32(dmsType)
      Case enumItemTypes.File, enumItemTypes.FileGroup
        tmpItem = SIS.DMS.dmsItems.dmsItemsGetByID(dmsItem)
    End Select
    tmpItems = SIS.DMS.Handlers.dmsData.GetChildFiles(dmsItem)
    If tmpItem IsNot Nothing Then tmpItems.Add(tmpItem)
    Dim FileCount As Integer = tmpItems.Count
    'For Each tmp As SIS.DMS.dmsItems In tmpItems
    '  'Copy file from isgec vault to app temp
    '  filePath = folderPath & tmp.Description
    '  If IO.File.Exists(filePath) Then
    '    FileCount += 1
    '    tmpItem = tmp
    '  End If
    'Next
    If FileCount = 0 Then
      Response.Clear()
      Dim x As New System.Web.HttpCookie("fileDownload", "true")
      x.Expires = DateTime.Now.AddYears(-1)
      x.Path = "/"
      Response.AppendCookie(x)
      Response.End()
    ElseIf FileCount = 1 Then
      fileName = tmpItem.Description
      filePath = folderPath & fileName
      Response.Clear()
      If SIS.DMS.Handlers.Core.IsDownloadable(tmpItem) Then
        Response.AppendHeader("content-disposition", "attachment; filename=" & fileName)
        Response.ContentType = SIS.SYS.Utilities.ApplicationSpacific.ContentType(fileName)
        Response.WriteFile(filePath)
      End If
      Dim x As New System.Web.HttpCookie("fileDownload", "true")
      x.Path = "/"
      Response.AppendCookie(x)
      Response.End()
    Else
      Dim FilesAtATime As Integer = 10
      Dim tmpFilesToDelete As New ArrayList
      fileName = "Download.zip"
      Response.Clear()
      Response.AppendHeader("content-disposition", "attachment; filename=" & fileName)
      Response.ContentType = SIS.SYS.Utilities.ApplicationSpacific.ContentType(fileName)
      Using zip As New ZipFile
        zip.CompressionLevel = Zlib.CompressionLevel.Level5
        Dim cnt As Integer = 0
        For Each tmp As SIS.DMS.dmsItems In tmpItems
          If Not SIS.DMS.Handlers.Core.IsDownloadable(tmp) Then Continue For
          fileName = tmp.Description
          filePath = folderPath & fileName
          If IO.File.Exists(filePath) Then
            Try
              zip.AddFile(filePath, "Files")
              tmpFilesToDelete.Add(filePath)
              cnt += 1
            Catch ex As Exception
            End Try
            If cnt >= FilesAtATime Then
              Exit For
            End If
          End If
        Next
        zip.Save(Response.OutputStream)
      End Using
      'For Each str As String In tmpFilesToDelete
      '  Try
      '    Dim fInfo As New FileInfo(str)
      '    fInfo.IsReadOnly = False
      '    IO.File.Delete(str)
      '  Catch ex As Exception
      '  End Try
      'Next
      Dim x As New System.Web.HttpCookie("fileDownload", "true")
      x.Path = "/"
      Response.AppendCookie(x)
      Response.End()
    End If
  End Sub

End Class
