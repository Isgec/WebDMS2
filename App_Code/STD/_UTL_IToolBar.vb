Imports Microsoft.VisualBasic
Public MustInherit Class ToolBar0
	Inherits System.Web.UI.UserControl
	Public MustOverride Property CurrentPage() As Integer
	Public MustOverride Property TotalPages() As Integer
	Public MustOverride Property RecordsPerPage() As Integer
	Public MustOverride Property AfterInsertURL() As String
	Public MustOverride Property AfterUpdateURL() As String
	Public MustOverride Property InsertAndStay() As Boolean
	Public MustOverride Property UpdateAndStay() As Boolean
  Public MustOverride Property SearchValidationGroup As String


	Public Event PageChanged(ByVal NewPageNo As Integer, ByVal PageSize As Integer)
	Public Event SearchClicked(ByVal SearchText As String, ByVal SearchState As Boolean)
  Public Event SaveClicked(ByVal sender As Object, ByVal e As System.EventArgs)
  Public Event DeleteClicked(ByVal sender As Object, ByVal e As System.EventArgs)
  Public Event AddClicked(ByVal sender As Object, ByVal e As System.EventArgs)
  Public Event CancelClicked(ByVal sender As Object, ByVal e As System.EventArgs)
  Public Event ForwardClicked(ByVal sender As Object, ByVal e As System.EventArgs)
  Public Event ReturnClicked(ByVal sender As Object, ByVal e As System.EventArgs)
  Public Sub RaisePageChanged(ByVal NewPageNo As Integer, ByVal PageSize As Integer)
    RaiseEvent PageChanged(NewPageNo, PageSize)
  End Sub
  Public Sub RaiseSearchClicked(ByVal SearchText As String, ByVal SearchState As Boolean)
		RaiseEvent SearchClicked(SearchText, SearchState)
	End Sub
  Public Sub RaiseSaveClicked(ByVal sender As Object, ByVal e As System.EventArgs)
    RaiseEvent SaveClicked(sender, e)
  End Sub
  Public Sub RaiseDeleteClicked(ByVal sender As Object, ByVal e As System.EventArgs)
    RaiseEvent DeleteClicked(sender, e)
  End Sub
  Public Sub RaiseAddClicked(ByVal sender As Object, ByVal e As System.EventArgs)
    RaiseEvent AddClicked(sender, e)
  End Sub
  Public Sub RaiseCancelClicked(ByVal sender As Object, ByVal e As System.EventArgs)
    RaiseEvent CancelClicked(sender, e)
  End Sub
  Public Sub RaiseForwardClicked(ByVal sender As Object, ByVal e As System.EventArgs)
    RaiseEvent ForwardClicked(sender, e)
  End Sub
  Public Sub RaiseReturnClicked(ByVal sender As Object, ByVal e As System.EventArgs)
    RaiseEvent ReturnClicked(sender, e)
  End Sub
End Class

Public MustInherit Class IpsBar
  Inherits System.Web.UI.UserControl
  Public MustOverride Property CurrentPage() As Integer
  Public MustOverride Property TotalPages() As Integer
  Public MustOverride Property RecordsPerPage() As Integer


  Public Event PageChanged(ByVal NewPageNo As Integer, ByVal PageSize As Integer)
  Public Event SearchClicked(ByVal SearchText As String, ByVal SearchState As Boolean)
  Public Sub RaisePageChanged(ByVal NewPageNo As Integer, ByVal PageSize As Integer)
    RaiseEvent PageChanged(NewPageNo, PageSize)
  End Sub
  Public Sub RaiseSearchClicked(ByVal SearchText As String, ByVal SearchState As Boolean)
    RaiseEvent SearchClicked(SearchText, SearchState)
  End Sub
End Class
Public MustInherit Class IattachBar
	Inherits System.Web.UI.UserControl
	Public MustOverride Property CurrentPage() As Integer
	Public MustOverride Property TotalPages() As Integer
	Public MustOverride Property RecordsPerPage() As Integer


	Public Event PageChanged(ByVal NewPageNo As Integer, ByVal PageSize As Integer)
	Public Event SearchClicked(ByVal SearchText As String, ByVal SearchState As Boolean)
	Public Sub RaisePageChanged(ByVal NewPageNo As Integer, ByVal PageSize As Integer)
		RaiseEvent PageChanged(NewPageNo, PageSize)
	End Sub
	Public Sub RaiseSearchClicked(ByVal SearchText As String, ByVal SearchState As Boolean)
		RaiseEvent SearchClicked(SearchText, SearchState)
	End Sub

End Class
