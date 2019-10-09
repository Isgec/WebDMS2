Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.DMS
  Partial Public Class dmsItemVersions
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
    Public Shared Function UZ_dmsItemVersionsSelectList(ByVal StartRowIndex As Integer, ByVal MaximumRows As Integer, ByVal OrderBy As String, ByVal SearchState As Boolean, ByVal SearchText As String, ByVal ItemID As Int32) As List(Of SIS.DMS.dmsItemVersions)
      Dim Results As List(Of SIS.DMS.dmsItemVersions) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          If SearchState Then
            Cmd.CommandText = "spdms_LG_ItemVersionsSelectListSearch"
            Cmd.CommandText = "spdmsItemVersionsSelectListSearch"
            SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@KeyWord", SqlDbType.NVarChar, 250, SearchText)
          Else
            Cmd.CommandText = "spdms_LG_ItemVersionsSelectListFilteres"
            Cmd.CommandText = "spdmsItemVersionsSelectListFilteres"
            SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@Filter_ItemID",SqlDbType.Int,10, IIf(ItemID = Nothing, 0,ItemID))
          End If
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@StartRowIndex", SqlDbType.Int, -1, StartRowIndex)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@MaximumRows", SqlDbType.Int, -1, MaximumRows)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@LoginID", SqlDbType.NvarChar, 9, HttpContext.Current.Session("LoginID"))
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@OrderBy", SqlDbType.NVarChar, 50, OrderBy)
          Cmd.Parameters.Add("@RecordCount", SqlDbType.Int)
          Cmd.Parameters("@RecordCount").Direction = ParameterDirection.Output
          _RecordCount = -1
          Results = New List(Of SIS.DMS.dmsItemVersions)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.DMS.dmsItemVersions(Reader))
          End While
          Reader.Close()
          _RecordCount = Cmd.Parameters("@RecordCount").Value
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function UZ_dmsItemVersionsInsert(ByVal Record As SIS.DMS.dmsItemVersions) As SIS.DMS.dmsItemVersions
      Dim _Result As SIS.DMS.dmsItemVersions = dmsItemVersionsInsert(Record)
      Return _Result
    End Function
    Public Shared Function UZ_dmsItemVersionsUpdate(ByVal Record As SIS.DMS.dmsItemVersions) As SIS.DMS.dmsItemVersions
      Dim _Result As SIS.DMS.dmsItemVersions = dmsItemVersionsUpdate(Record)
      Return _Result
    End Function
    Public Shared Function UZ_dmsItemVersionsDelete(ByVal Record As SIS.DMS.dmsItemVersions) As Integer
      Dim _Result as Integer = dmsItemVersionsDelete(Record)
      Return _Result
    End Function
    Public Shared Function SetDefaultValues(ByVal sender As System.Web.UI.WebControls.FormView, ByVal e As System.EventArgs) As System.Web.UI.WebControls.FormView
      With sender
        Try
        CType(.FindControl("F_ItemID"), TextBox).Text = ""
        CType(.FindControl("F_ItemID_Display"), Label).Text = ""
        CType(.FindControl("F_SerialNo"), TextBox).Text = ""
        CType(.FindControl("F_UserID"), TextBox).Text = ""
        CType(.FindControl("F_UserID_Display"), Label).Text = ""
        CType(.FindControl("F_Description"), TextBox).Text = ""
        CType(.FindControl("F_FullDescription"), TextBox).Text = ""
        CType(.FindControl("F_RevisionNo"), TextBox).Text = ""
        CType(.FindControl("F_EMailID"), TextBox).Text = ""
        CType(.FindControl("F_ItemTypeID"), TextBox).Text = ""
        CType(.FindControl("F_ItemTypeID_Display"), Label).Text = ""
        CType(.FindControl("F_StatusID"), TextBox).Text = ""
        CType(.FindControl("F_StatusID_Display"), Label).Text = ""
        CType(.FindControl("F_ConvertedStatusID"), TextBox).Text = ""
        CType(.FindControl("F_ConvertedStatusID_Display"), Label).Text = ""
        CType(.FindControl("F_IsMultiParent"), CheckBox).Checked = False
        CType(.FindControl("F_ParentItemID"), TextBox).Text = 0
        CType(.FindControl("F_IsMultiChild"), CheckBox).Checked = False
        CType(.FindControl("F_ChildItemID"), TextBox).Text = 0
        CType(.FindControl("F_CompanyID"), TextBox).Text = ""
        CType(.FindControl("F_CompanyID_Display"), Label).Text = ""
        CType(.FindControl("F_DivisionID"), TextBox).Text = ""
        CType(.FindControl("F_DivisionID_Display"), Label).Text = ""
        CType(.FindControl("F_DepartmentID"), TextBox).Text = ""
        CType(.FindControl("F_DepartmentID_Display"), Label).Text = ""
        CType(.FindControl("F_ProjectID"), TextBox).Text = ""
        CType(.FindControl("F_ProjectID_Display"), Label).Text = ""
        CType(.FindControl("F_WBSID"), TextBox).Text = ""
        CType(.FindControl("F_WBSID_Display"), Label).Text = ""
        CType(.FindControl("F_KeyWords"), TextBox).Text = ""
        CType(.FindControl("F_CreatedBy"), TextBox).Text = ""
        CType(.FindControl("F_CreatedBy_Display"), Label).Text = ""
        CType(.FindControl("F_CreatedOn"), TextBox).Text = ""
        CType(.FindControl("F_IsMultiForward"), CheckBox).Checked = False
        CType(.FindControl("F_ForwardLinkedItemTypeID"), TextBox).Text = ""
        CType(.FindControl("F_ForwardLinkedItemTypeID_Display"), Label).Text = ""
        CType(.FindControl("F_ForwardLinkedItemID"), TextBox).Text = 0
        CType(.FindControl("F_IsMultiLinked"), CheckBox).Checked = False
        CType(.FindControl("F_LinkedItemTypeID"), TextBox).Text = ""
        CType(.FindControl("F_LinkedItemTypeID_Display"), Label).Text = ""
        CType(.FindControl("F_LinkedItemID"), TextBox).Text = 0
        CType(.FindControl("F_IsMultiBackward"), CheckBox).Checked = False
        CType(.FindControl("F_BackwardLinkedItemTypeID"), TextBox).Text = ""
        CType(.FindControl("F_BackwardLinkedItemTypeID_Display"), Label).Text = ""
        CType(.FindControl("F_BackwardLinkedItemID"), TextBox).Text = 0
        CType(.FindControl("F_MaintainAllLog"), CheckBox).Checked = False
        CType(.FindControl("F_MaintainStatusLog"), CheckBox).Checked = False
        CType(.FindControl("F_MaintainVersions"), CheckBox).Checked = False
        CType(.FindControl("F_IsgecVaultID"), TextBox).Text = ""
        CType(.FindControl("F_Publish"), CheckBox).Checked = False
        CType(.FindControl("F_CreateFolder"), CheckBox).Checked = False
        CType(.FindControl("F_RenameFolder"), CheckBox).Checked = False
        CType(.FindControl("F_DeleteFolder"), CheckBox).Checked = False
        CType(.FindControl("F_CreateFile"), CheckBox).Checked = False
        CType(.FindControl("F_DeleteFile"), CheckBox).Checked = False
        CType(.FindControl("F_GrantAuthorization"), CheckBox).Checked = False
        CType(.FindControl("F_BrowseList"), CheckBox).Checked = False
        CType(.FindControl("F_InheritFromParent"), CheckBox).Checked = False
        CType(.FindControl("F_SearchInParent"), CheckBox).Checked = False
        CType(.FindControl("F_ShowInList"), CheckBox).Checked = False
        CType(.FindControl("F_Approved"), CheckBox).Checked = False
        CType(.FindControl("F_Rejected"), CheckBox).Checked = False
        CType(.FindControl("F_ActionRemarks"), TextBox).Text = ""
        CType(.FindControl("F_ActionBy"), TextBox).Text = ""
        CType(.FindControl("F_ActionBy_Display"), Label).Text = ""
        CType(.FindControl("F_ActionOn"), TextBox).Text = ""
        CType(.FindControl("F_IsError"), CheckBox).Checked = False
        CType(.FindControl("F_ErrorMessage"), TextBox).Text = ""
        Catch ex As Exception
        End Try
      End With
      Return sender
    End Function
  End Class
End Namespace
