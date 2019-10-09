Imports System
Imports System.Reflection
Imports System.Web.UI
Imports System.Web.UI.UserControl
Namespace SIS.SYS
	Public MustInherit Class UpdateBase
		Inherits System.Web.UI.Page
		Protected WithEvents _ToolBar1 As ToolBar0
		Protected WithEvents _FormView1 As FormView
		Private _dataClassName As String = ""
		Private _fvOK As Boolean = False
		Private _dcOK As Boolean = False
		Public ReadOnly Property FileName() As String
			Get
				Return _dataClassName & "." & _FormView1.ID
			End Get
		End Property
		Public Property DataClassName() As String
			Get
				Return _dataClassName
			End Get
			Set(ByVal value As String)
				_dataClassName = value
				_dcOK = True
				If _fvOK Then
					fvInit()
				End If
			End Set
		End Property
		Public Property SetToolBar() As ToolBar0
			Get
				Return _ToolBar1
			End Get
			Set(ByVal value As ToolBar0)
				_ToolBar1 = value
			End Set
		End Property
		Public Property SetFormView() As FormView
			Get
				Return _FormView1
			End Get
			Set(ByVal value As FormView)
				_FormView1 = value
				_fvOK = True
				If _dcOK Then
					fvInit()
				End If
			End Set
		End Property
		Protected Sub fvInit()
			Try
				If Session("PageNoProvider") = True Then
					_FormView1.PageIndex = SIS.SYS.Utilities.GlobalVariables.PageNo(FileName, HttpContext.Current.Session("LoginID"))
				Else
					_FormView1.PageIndex = Session("PageNo_" & FileName)
				End If
			Catch ex As Exception
				_FormView1.PageIndex = 0
			End Try
		End Sub
		Private Sub ExceptionHandling(ByVal InnerException As String, ByVal Message As String)
			If Not InnerException Is Nothing Then
				Session("myError") = InnerException.ToString & vbCrLf & Message
			Else
				Session("myError") = Message
			End If
			Response.Redirect("~/ErrorPage.aspx")
		End Sub
    Private Sub _ToolBar1_CancelClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ToolBar1.CancelClicked
      Response.Redirect(SIS.SYS.Utilities.SessionManager.PopNavBar(True))
    End Sub
    Private Sub _ToolBar1_DeleteClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ToolBar1.DeleteClicked
      _FormView1.DeleteItem()
    End Sub
    Private Sub _ToolBar1_PageChanged(ByVal NewPageNo As Integer, ByVal PageSize As Integer) Handles _ToolBar1.PageChanged
			_FormView1.PageIndex = NewPageNo
			If Session("PageNoProvider") = True Then
				SIS.SYS.Utilities.GlobalVariables.PageNo(FileName, HttpContext.Current.Session("LoginID"), NewPageNo)
			Else
				Session("PageNo_" & FileName) = NewPageNo
			End If
		End Sub
    Private Sub _ToolBar1_SaveClicked(ByVal sender As Object, ByVal e As System.EventArgs) Handles _ToolBar1.SaveClicked
      _FormView1.UpdateItem(True)
    End Sub
    Private Sub _ToolBar1_SearchClicked(ByVal SearchText As String, ByVal SearchState As Boolean) Handles _ToolBar1.SearchClicked
			_FormView1.PageIndex = 0
			CType(_FormView1.DataSourceObject, ObjectDataSource).SelectParameters("SearchState").DefaultValue = SearchState
			CType(_FormView1.DataSourceObject, ObjectDataSource).SelectParameters("SearchText").DefaultValue = SearchText
		End Sub
		Private Sub _FormView1_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles _FormView1.DataBound
			With _ToolBar1
				.CurrentPage = _FormView1.PageIndex
				.TotalPages = _FormView1.PageCount
				.RecordsPerPage = 1
			End With
		End Sub
		Private Sub _FormView1_ItemDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewDeletedEventArgs) Handles _FormView1.ItemDeleted
			If e.Exception Is Nothing Then
				Response.Redirect(SIS.SYS.Utilities.SessionManager.PopNavBar())
			Else
				e.ExceptionHandled = True
				ExceptionHandling(e.Exception.InnerException.ToString, e.Exception.Message)
			End If
		End Sub
		Private Sub _FormView1_ItemUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.FormViewUpdatedEventArgs) Handles _FormView1.ItemUpdated
			If e.Exception Is Nothing Then
				If Not _ToolBar1.UpdateAndStay Then
					If _ToolBar1.AfterUpdateURL = String.Empty Then
						Response.Redirect(SIS.SYS.Utilities.SessionManager.PopNavBar())
					Else
						Response.Redirect(_ToolBar1.AfterUpdateURL)
					End If
				End If
			Else
				e.ExceptionHandled = True
				ExceptionHandling(e.Exception.InnerException.ToString, e.Exception.Message)
			End If
		End Sub
		Private Sub _FormView1_PageIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _FormView1.PageIndexChanged
			If Session("PageNoProvider") = True Then
				SIS.SYS.Utilities.GlobalVariables.PageNo(FileName, HttpContext.Current.Session("LoginID"), _FormView1.PageIndex)
			Else
				Session("PageNo_" & FileName) = _FormView1.PageIndex
			End If
		End Sub

    Private Sub UpdateBase_Load(sender As Object, e As EventArgs) Handles Me.Load
      If Not Page.IsPostBack And Not Page.IsCallback Then
        Try
          SIS.SYS.Utilities.SessionManager.PushNavBar(System.Web.HttpContext.Current.Request.Url.ToString)
        Catch ex As Exception
        End Try
      End If

    End Sub
  End Class
End Namespace


