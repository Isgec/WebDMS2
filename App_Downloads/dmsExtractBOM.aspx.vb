Imports OfficeOpenXml
Imports System.Web.Script.Serialization
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports Ionic
Imports Ionic.Zip
Imports System.Net
Partial Class dmsExtractBOM
  Inherits System.Web.UI.Page
  Private st As Long = HttpContext.Current.Server.ScriptTimeout
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    HttpContext.Current.Server.ScriptTimeout = Integer.MaxValue
    Dim mt As String = ""
    Dim mo As String = ""
    If Request.QueryString("mt") IsNot Nothing Then
      mt = Request.QueryString("mt")
      DownloadLive(mt)
    End If
  End Sub

  Private Sub DownloadLive(ByVal mt As String)
    Dim aMt() As String = mt.Split("|".ToCharArray)
    Dim dmsType As String = aMt(0)
    Dim dmsItem As String = aMt(1)
    If dmsItem = "NaN" Then dmsItem = 0
    If Not IsNumeric(dmsItem) Then dmsItem = 0

    Dim tmpItem As SIS.DMS.dmsItems = Nothing
    Dim tmpItems As New List(Of SIS.DMS.dmsItems)

    Dim EnggFunc As String = ""
    Dim Project As String = ""
    Dim IsTransmittal As Boolean = False
    Threading.Thread.Sleep(2000)
    '===========Under Approval To Me / Approved By Me =================
    Select Case Convert.ToInt32(dmsType)
      Case enumItemTypes.UnderApprovalToMe, enumItemTypes.ApprovedByMe
        Dim itm As SIS.DMS.UI.apiItem = SIS.DMS.UI.GetItem(dmsItem)
        dmsType = itm.LinkedItemTypeID
        dmsItem = itm.LinkedItemID
    End Select
    '==================================================================
    Select Case Convert.ToInt32(dmsType)
      Case enumItemTypes.File, enumItemTypes.FileGroup
        IsTransmittal = True
        tmpItem = SIS.DMS.dmsItems.dmsItemsGetByID(dmsItem)
      Case enumItemTypes.Folder, enumItemTypes.FolderGroup
        tmpItem = SIS.DMS.dmsItems.dmsItemsGetByID(dmsItem)
        Project = tmpItem.ProjectID
        If tmpItem.DMS_Items6_Description <> "YNR" Then
          EnggFunc = tmpItem.ItemID
        End If
    End Select
    Try
      If IsTransmittal Then
        tmpItems = SIS.DMS.Handlers.dmsData.GetChildFiles(dmsItem)
        If tmpItem IsNot Nothing Then tmpItems.Add(tmpItem)
        Dim FileCount As Integer = tmpItems.Count
        If FileCount = 0 Then
          Response.Clear()
          Response.Cache.SetCacheability(HttpCacheability.NoCache)
          Dim x As New System.Web.HttpCookie("fileDownload", "true")
          x.Expires = DateTime.Now.AddYears(-1)
          x.Path = "/"
          Response.AppendCookie(x)
          HttpContext.Current.Server.ScriptTimeout = st
          Response.End()
        Else
          Dim TemplateName As String = "BOM_Template.xlsx"
          Dim tmpFile As String = Server.MapPath("~/App_Templates/" & TemplateName)
          Dim FileName As String = Server.MapPath("~/..") & "App_Temp/" & Guid.NewGuid().ToString()
          IO.File.Copy(tmpFile, FileName)
          Dim FileInfo As IO.FileInfo = New IO.FileInfo(FileName)
          Dim xlPk As ExcelPackage = New ExcelPackage(FileInfo)

          Dim xlWS As ExcelWorksheet = xlPk.Workbook.Worksheets("Data")
          Dim r As Integer = 3
          Dim c As Integer = 1
          Dim cnt As Integer = 1
          Dim DownloadName As String = ""
          'Set Download File Name
          For Each dmtmp As SIS.DMS.dmsItems In tmpItems
            If dmtmp.Description.IndexOf("Transmittal") >= 0 Then
              DownloadName = IO.Path.GetFileNameWithoutExtension(dmtmp.Description) & ".xlsx"
              Exit For
            End If
          Next
          For Each dmtmp As SIS.DMS.dmsItems In tmpItems
            'If Not SIS.DMS.Handlers.Core.IsDownloadable(tmp) Then Continue For
            If dmtmp.Description.IndexOf("Transmittal") >= 0 Then
              Continue For
            End If
            Dim DocumentID As String = IO.Path.GetFileNameWithoutExtension(dmtmp.Description)
            If DownloadName = "" Then
              DownloadName = DocumentID & ".xlsx"
            End If

            Dim tmpDoc As SIS.DMISG.BOM = SIS.DMISG.BOM.GetDoc(DocumentID, dmtmp.RevisionNo)
            With xlWS
              c = 1
              .Cells(r, c).Value = tmpDoc.DocumentID
              c += 1
              .Cells(r, c).Value = tmpDoc.RevisionNo
              c += 1
              .Cells(r, c).Value = tmpDoc.Title
              c += 1
              .Cells(r, c).Value = tmpDoc.DocWeight
              c += 1
              .Cells(r, c).Value = "Kg"
              r += 1
            End With
            Dim tmpBoms As List(Of SIS.DMISG.BOM) = SIS.DMISG.BOM.GetBOM(DocumentID, dmtmp.RevisionNo)
            For Each tmp As SIS.DMISG.BOM In tmpBoms
              With xlWS
                c = 6
                .Cells(r, c).Value = tmpDoc.ProjectID & "-" & tmp.ItemID.Trim
                c += 1
                .Cells(r, c).Value = tmp.ItemID
                c += 1
                .Cells(r, c).Value = tmp.ItemDescription
                c += 1
                .Cells(r, c).Value = tmp.ItemQuantity
                c += 1
                .Cells(r, c).Value = tmp.ItemWeight
                c += 1
                .Cells(r, c).Value = tmp.ItemUnit
                c += 1
                .Cells(r, c).Value = ""
                r += 1
              End With
              Dim tmpPBoms As List(Of SIS.DMISG.BOM) = SIS.DMISG.BOM.GetPBOM(DocumentID, dmtmp.RevisionNo, tmp.SrNo)
              For Each ptmp As SIS.DMISG.BOM In tmpPBoms
                With xlWS
                  c = 13
                  .Cells(r, c).Value = ptmp.PartItemID
                  c += 1
                  .Cells(r, c).Value = ptmp.PartDescription
                  c += 1
                  .Cells(r, c).Value = ptmp.PartSpecification
                  c += 1
                  .Cells(r, c).Value = ptmp.PartSize
                  c += 1
                  .Cells(r, c).Value = ptmp.PartQuantity
                  c += 1
                  .Cells(r, c).Value = ptmp.PartWeight
                  c += 1
                  .Cells(r, c).Value = ptmp.PartRemarks
                  r += 1
                End With
              Next
            Next
            Dim tmpRefBoms As List(Of SIS.DMISG.BOM) = SIS.DMISG.BOM.GetRefBOM(DocumentID, dmtmp.RevisionNo)
            For Each tmp As SIS.DMISG.BOM In tmpRefBoms
              With xlWS
                c = 6
                .Cells(r, c).Value = tmpDoc.ProjectID & "-" & tmp.ItemID.Trim
                c += 1
                .Cells(r, c).Value = tmp.ItemID
                c += 1
                .Cells(r, c).Value = tmp.ItemDescription
                c += 1
                .Cells(r, c).Value = tmp.ItemQuantity
                c += 1
                .Cells(r, c).Value = tmp.ItemWeight
                c += 1
                .Cells(r, c).Value = tmp.ItemUnit
                c += 1
                .Cells(r, c).Value = ""
                r += 1
              End With
              Dim tmpRefPBoms As List(Of SIS.DMISG.BOM) = SIS.DMISG.BOM.GetRefPBOM(DocumentID, dmtmp.RevisionNo, tmp.SrNo)
              For Each ptmp As SIS.DMISG.BOM In tmpRefPBoms
                With xlWS
                  c = 13
                  .Cells(r, c).Value = ptmp.PartItemID
                  c += 1
                  .Cells(r, c).Value = ptmp.PartDescription
                  c += 1
                  .Cells(r, c).Value = ptmp.PartSpecification
                  c += 1
                  .Cells(r, c).Value = ptmp.PartSize
                  c += 1
                  .Cells(r, c).Value = ptmp.PartQuantity
                  c += 1
                  .Cells(r, c).Value = ptmp.PartWeight
                  c += 1
                  .Cells(r, c).Value = ptmp.PartRemarks
                End With
                r += 1
              Next
            Next
          Next

          xlPk.Save()
          xlPk.Dispose()

          Response.Clear()
          Response.Cache.SetCacheability(HttpCacheability.NoCache)
          Response.AppendHeader("content-disposition", "attachment; filename=" & DownloadName)
          Response.ContentType = SIS.SYS.Utilities.ApplicationSpacific.ContentType(DownloadName)
          Response.WriteFile(FileName)
          Dim x As New System.Web.HttpCookie("fileDownload", "true")
          x.Path = "/"
          Response.AppendCookie(x)
          Response.Flush()
          HttpContext.Current.Server.ScriptTimeout = st
          Response.End()
        End If
      Else 'Project or EnggFunctionWise
        Dim TemplateName As String = "BOM_Template.xlsx"
        Dim tmpFile As String = Server.MapPath("~/App_Templates/" & TemplateName)
        Dim FileName As String = Server.MapPath("~/..") & "App_Temp/" & Guid.NewGuid().ToString()
        IO.File.Copy(tmpFile, FileName)
        Dim FileInfo As IO.FileInfo = New IO.FileInfo(FileName)
        Dim xlPk As ExcelPackage = New ExcelPackage(FileInfo)

        Dim xlWS As ExcelWorksheet = xlPk.Workbook.Worksheets("Data")
        Dim r As Integer = 3
        Dim c As Integer = 1
        Dim cnt As Integer = 1
        Dim DownloadName As String = Project & IIf(EnggFunc <> "", "_" & EnggFunc, "") & ".xlsx"

        Dim tmpDocs As List(Of SIS.DMISG.TDocs) = SIS.DMISG.TDocs.GetDocs(Project, EnggFunc)

        For Each doc As SIS.DMISG.TDocs In tmpDocs
          Dim tmpDoc As SIS.DMISG.BOM = SIS.DMISG.BOM.GetDoc(doc.DocumentID, doc.RevisionNo)
          With xlWS
            c = 1
            .Cells(r, c).Value = tmpDoc.DocumentID
            c += 1
            .Cells(r, c).Value = tmpDoc.RevisionNo
            c += 1
            .Cells(r, c).Value = tmpDoc.Title
            c += 1
            .Cells(r, c).Value = tmpDoc.DocWeight
            c += 1
            .Cells(r, c).Value = "Kg"
            r += 1
          End With
          Dim tmpBoms As List(Of SIS.DMISG.BOM) = SIS.DMISG.BOM.GetBOM(doc.DocumentID, doc.RevisionNo)
          For Each tmp As SIS.DMISG.BOM In tmpBoms
            With xlWS
              c = 6
              .Cells(r, c).Value = tmpDoc.ProjectID & "-" & tmp.ItemID.Trim
              c += 1
              .Cells(r, c).Value = tmp.ItemID
              c += 1
              .Cells(r, c).Value = tmp.ItemDescription
              c += 1
              .Cells(r, c).Value = tmp.ItemQuantity
              c += 1
              .Cells(r, c).Value = tmp.ItemWeight
              c += 1
              .Cells(r, c).Value = tmp.ItemUnit
              c += 1
              .Cells(r, c).Value = ""
              r += 1
            End With
            Dim tmpPBoms As List(Of SIS.DMISG.BOM) = SIS.DMISG.BOM.GetPBOM(doc.DocumentID, doc.RevisionNo, tmp.SrNo)
            For Each ptmp As SIS.DMISG.BOM In tmpPBoms
              With xlWS
                c = 13
                .Cells(r, c).Value = ptmp.PartItemID
                c += 1
                .Cells(r, c).Value = ptmp.PartDescription
                c += 1
                .Cells(r, c).Value = ptmp.PartSpecification
                c += 1
                .Cells(r, c).Value = ptmp.PartSize
                c += 1
                .Cells(r, c).Value = ptmp.PartQuantity
                c += 1
                .Cells(r, c).Value = ptmp.PartWeight
                c += 1
                .Cells(r, c).Value = ptmp.PartRemarks
                r += 1
              End With
            Next
          Next
          Dim tmpRefBoms As List(Of SIS.DMISG.BOM) = SIS.DMISG.BOM.GetRefBOM(doc.DocumentID, doc.RevisionNo)
          For Each tmp As SIS.DMISG.BOM In tmpRefBoms
            With xlWS
              c = 6
              .Cells(r, c).Value = tmpDoc.ProjectID & "-" & tmp.ItemID.Trim
              c += 1
              .Cells(r, c).Value = tmp.ItemID
              c += 1
              .Cells(r, c).Value = tmp.ItemDescription
              c += 1
              .Cells(r, c).Value = tmp.ItemQuantity
              c += 1
              .Cells(r, c).Value = tmp.ItemWeight
              c += 1
              .Cells(r, c).Value = tmp.ItemUnit
              c += 1
              .Cells(r, c).Value = ""
              r += 1
            End With
            Dim tmpRefPBoms As List(Of SIS.DMISG.BOM) = SIS.DMISG.BOM.GetRefPBOM(doc.DocumentID, doc.RevisionNo, tmp.SrNo)
            For Each ptmp As SIS.DMISG.BOM In tmpRefPBoms
              With xlWS
                c = 13
                .Cells(r, c).Value = ptmp.PartItemID
                c += 1
                .Cells(r, c).Value = ptmp.PartDescription
                c += 1
                .Cells(r, c).Value = ptmp.PartSpecification
                c += 1
                .Cells(r, c).Value = ptmp.PartSize
                c += 1
                .Cells(r, c).Value = ptmp.PartQuantity
                c += 1
                .Cells(r, c).Value = ptmp.PartWeight
                c += 1
                .Cells(r, c).Value = ptmp.PartRemarks
              End With
              r += 1
            Next
          Next
        Next
        xlPk.Save()
        xlPk.Dispose()

        Response.Clear()
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.AppendHeader("content-disposition", "attachment; filename=" & DownloadName)
        Response.ContentType = SIS.SYS.Utilities.ApplicationSpacific.ContentType(DownloadName)
        Response.WriteFile(FileName)
        Dim x As New System.Web.HttpCookie("fileDownload", "true")
        x.Path = "/"
        Response.AppendCookie(x)
        Response.Flush()
        HttpContext.Current.Server.ScriptTimeout = st
        Response.End()
      End If
    Catch ex As Exception
      Response.Clear()
      Response.Cache.SetCacheability(HttpCacheability.NoCache)
      Dim x As New System.Web.HttpCookie("fileDownload", "true")
      x.Expires = DateTime.Now.AddYears(-1)
      x.Path = "/"
      Response.AppendCookie(x)
      HttpContext.Current.Server.ScriptTimeout = st
      Response.End()
    End Try
  End Sub

End Class
'For Each doc As SIS.DMISG.TDocs In tmpDocs
'Dim tmpBoms As List(Of SIS.DMISG.BOM) = SIS.DMISG.BOM.GetBOM(doc.DocumentID, doc.RevisionNo)
'For Each tmp As SIS.DMISG.BOM In tmpBoms
'With xlWS
'c = 1
'.Cells(r, c).Value = tmp.DocumentID
'c += 1
'.Cells(r, c).Value = tmp.RevisionNo
'c += 1
'.Cells(r, c).Value = tmp.Title
'c += 1
'.Cells(r, c).Value = tmp.ItemID
'c += 1
'.Cells(r, c).Value = tmp.ItemDescription
'c += 1
'.Cells(r, c).Value = tmp.ItemQuantity
'c += 1
'.Cells(r, c).Value = tmp.ItemWeight
'c += 1
'.Cells(r, c).Value = tmp.ItemUnit
'c += 1
'.Cells(r, c).Value = ""
'c += 1
'.Cells(r, c).Value = tmp.PartItemID
'c += 1
'.Cells(r, c).Value = tmp.PartDescription
'c += 1
'.Cells(r, c).Value = tmp.PartSpecification
'c += 1
'.Cells(r, c).Value = tmp.PartSize
'c += 1
'.Cells(r, c).Value = tmp.PartQuantity
'c += 1
'.Cells(r, c).Value = tmp.PartWeight
'c += 1
'.Cells(r, c).Value = tmp.PartRemarks
'End With
'r += 1
'Next
'Dim tmpRefBoms As List(Of SIS.DMISG.BOM) = SIS.DMISG.BOM.GetRefBOM(doc.DocumentID, doc.RevisionNo)
'For Each tmp As SIS.DMISG.BOM In tmpRefBoms
'With xlWS
'c = 1
'.Cells(r, c).Value = tmp.DocumentID
'c += 1
'.Cells(r, c).Value = tmp.RevisionNo
'c += 1
'.Cells(r, c).Value = tmp.Title
'c += 1
'.Cells(r, c).Value = tmp.ItemID
'c += 1
'.Cells(r, c).Value = tmp.ItemDescription
'c += 1
'.Cells(r, c).Value = tmp.ItemQuantity
'c += 1
'.Cells(r, c).Value = tmp.ItemWeight
'c += 1
'.Cells(r, c).Value = tmp.ItemUnit
'c += 1
'.Cells(r, c).Value = ""
'c += 1
'.Cells(r, c).Value = tmp.PartItemID
'c += 1
'.Cells(r, c).Value = tmp.PartDescription
'c += 1
'.Cells(r, c).Value = tmp.PartSpecification
'c += 1
'.Cells(r, c).Value = tmp.PartSize
'c += 1
'.Cells(r, c).Value = tmp.PartQuantity
'c += 1
'.Cells(r, c).Value = tmp.PartWeight
'c += 1
'.Cells(r, c).Value = tmp.PartRemarks
'End With
'r += 1
'Next

'Next
