Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.DMS
  Partial Public Class dmsMultiItems
    Public Function GetColor() As System.Drawing.Color
      Dim mRet As System.Drawing.Color = Drawing.Color.Blue
      Return mRet
    End Function
    Public Function GetVisible() As Boolean
      Dim mRet As Boolean = True
      Return mRet
    End Function
    Public Function GetEnable() As Boolean
      Dim mRet As Boolean = True
      Return mRet
    End Function
    Public Function GetEditable() As Boolean
      Dim mRet As Boolean = True
      Return mRet
    End Function
    Public Function GetDeleteable() As Boolean
      Dim mRet As Boolean = True
      Return mRet
    End Function
    Public ReadOnly Property Editable() As Boolean
      Get
        Dim mRet As Boolean = True
        Try
          mRet = GetEditable()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public ReadOnly Property Deleteable() As Boolean
      Get
        Dim mRet As Boolean = True
        Try
          mRet = GetDeleteable()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public Shared Function SetDefaultValues(ByVal sender As System.Web.UI.WebControls.FormView, ByVal e As System.EventArgs) As System.Web.UI.WebControls.FormView
      With sender
        Try
        CType(.FindControl("F_SerialNo"), TextBox).Text = ""
        CType(.FindControl("F_ItemID"), TextBox).Text = ""
        CType(.FindControl("F_ItemID_Display"), Label).Text = ""
        CType(.FindControl("F_MultiTypeID"),Object).SelectedValue = ""
        CType(.FindControl("F_MultiItemTypeID"),Object).SelectedValue = ""
        CType(.FindControl("F_MultiItemID"), TextBox).Text = ""
        CType(.FindControl("F_MultiItemID_Display"), Label).Text = ""
        CType(.FindControl("F_MultiSequence"), TextBox).Text = 0
        Catch ex As Exception
        End Try
      End With
      Return sender
    End Function
  End Class
End Namespace
