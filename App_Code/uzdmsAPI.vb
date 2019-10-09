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
  Partial Public Class UI
    Public Class ItemProperty
      Public Property ItemID As String = ""
      Public Property TransmittalID As String = ""
      Public Property ProjectID As String = ""
      Public Property ProjectName As String = ""
      Public Property TransmittalType As String = ""
      Public Property TransmittalSubject As String = ""
      Public Property Remarks As String = ""
      Public Property CreatedBy As String = ""
      Public Property CreatedName As String = ""
      Public Property CreatedOn As String = ""
      Public Property ApprovedBy As String = ""
      Public Property ApproverName As String = ""
      Public Property ApprovedOn As String = ""
      Public Property IssuedBy As String = ""
      Public Property IssuerName As String = ""
      Public Property IssuedOn As String = ""
      Public Shared Function GetByID(ByVal ItemID As Int32) As SIS.DMS.UI.ItemProperty
        Dim Results As SIS.DMS.UI.ItemProperty = Nothing
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = "select * from DMS_ItemProperty where itemid=" & ItemID
            Con.Open()
            Dim Reader As SqlDataReader = Cmd.ExecuteReader()
            If Reader.Read() Then
              Results = New SIS.DMS.UI.ItemProperty(Reader)
            End If
            Reader.Close()
          End Using
        End Using
        Return Results
      End Function
      Sub New(rd As SqlDataReader)
        API.NewObj(Me, rd)
      End Sub
      Sub New()
      End Sub

    End Class

    Public Class apiTags
      Public Property tagID As String = ""
      Public Property tagType As String = ""
      Public Property tagDescription As String = ""
      Public Property active As Boolean = False
      Public Property parentTagID As String = ""

      Public Shared Function selectAutoComplete(prefix As String, Optional count As Integer = 10) As String
        Dim mret As New SIS.DMS.UI.apiResponse
        Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetConnectionString())
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = "select top " & count & " * from DMS_Tags where upper(tagID) like '" & prefix.ToUpper & "%'"
            Con.Open()
            Dim Reader As SqlDataReader = Cmd.ExecuteReader()
            While Reader.Read()
              mret.strHTML.Add(Reader("tagID") & ", " & Reader("tagDescription"))
            End While
            Reader.Close()
          End Using
        End Using
        Return New JavaScriptSerializer().Serialize(mret)
      End Function

      Sub New(rd As SqlDataReader)
        API.NewObj(Me, rd)
      End Sub
      Sub New()
      End Sub
    End Class

  End Class

End Namespace
