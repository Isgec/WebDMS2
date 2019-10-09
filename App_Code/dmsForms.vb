Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Xml
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports System.Web.Script.Serialization

Namespace SIS.DMS
  Public Class dmsForms
    Public Shared Function frmCreateFolder() As String
      Dim mRet As String = ""
      Dim tblMain As New Table
      Dim tr As TableRow = Nothing
      Dim td As TableCell = Nothing

      tr = New TableRow
      td = New TableCell


      Return mRet
    End Function
    Private Shared Function GetTRItemID() As TableRow
      Dim tr As TableRow = Nothing
      Dim td As TableCell = Nothing

      tr = New TableRow
      td = New TableCell
      td.Text = "Item ID"
      tr.Cells.Add(td)
      td = New TableCell
      Dim lbl As New Label
      lbl.ID = "F_ItemID"

      Return tr
    End Function
  End Class

End Namespace
