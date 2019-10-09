Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.DMS
  Partial Public Class dmsItemTypes
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
        CType(.FindControl("F_ItemTypeID"), TextBox).Text = 0
        CType(.FindControl("F_Description"), TextBox).Text = ""
        CType(.FindControl("F_Active"), CheckBox).Checked = False
        CType(.FindControl("F_UQuality"), CheckBox).Checked = False
        CType(.FindControl("F_IQuality"), CheckBox).Checked = False
        CType(.FindControl("F_InheritFromParent"), CheckBox).Checked = False
        CType(.FindControl("F_SearchInParent"), CheckBox).Checked = False
        CType(.FindControl("F_ShowInList"), CheckBox).Checked = False
        CType(.FindControl("F_Sequence"), TextBox).Text = 0
        Catch ex As Exception
        End Try
      End With
      Return sender
    End Function
  End Class
End Namespace
