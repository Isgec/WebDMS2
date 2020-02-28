Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.DMISG
  <DataObject()>
  Partial Public Class dmisg002
    Private Shared _RecordCount As Integer
    Private _t_docn As String = ""
    Private _t_revn As String = ""
    Private _t_srno As Int32 = 0
    Private _t_item As String = ""
    Private _t_dsca As String = ""
    Private _t_citg As String = ""
    Private _t_qnty As Double = 0
    Private _t_wght As Double = 0
    Private _t_itmt As String = ""
    Private _t_txta As Int32 = 0
    Private _t_txtb As Int32 = 0
    Private _t_cuni As String = ""
    Private _t_oitm As String = ""
    Private _t_elem As String = ""
    Private _t_Refcntd As Int32 = 0
    Private _t_Refcntu As Int32 = 0
    Public Property Remarks As String = ""
    Public Property t_docn() As String
      Get
        Return _t_docn
      End Get
      Set(ByVal value As String)
        _t_docn = value
      End Set
    End Property
    Public Property t_revn() As String
      Get
        Return _t_revn
      End Get
      Set(ByVal value As String)
        _t_revn = value
      End Set
    End Property
    Public Property t_srno() As Int32
      Get
        Return _t_srno
      End Get
      Set(ByVal value As Int32)
        _t_srno = value
      End Set
    End Property
    Public Property t_item() As String
      Get
        Return _t_item
      End Get
      Set(ByVal value As String)
        _t_item = value
      End Set
    End Property
    Public Property t_dsca() As String
      Get
        Return _t_dsca
      End Get
      Set(ByVal value As String)
        _t_dsca = value
      End Set
    End Property
    Public Property t_citg() As String
      Get
        Return _t_citg
      End Get
      Set(ByVal value As String)
        _t_citg = value
      End Set
    End Property
    Public Property t_qnty() As Double
      Get
        Return _t_qnty
      End Get
      Set(ByVal value As Double)
        _t_qnty = value
      End Set
    End Property
    Public Property t_wght() As Double
      Get
        Return _t_wght
      End Get
      Set(ByVal value As Double)
        _t_wght = value
      End Set
    End Property
    Public Property t_itmt() As String
      Get
        Return _t_itmt
      End Get
      Set(ByVal value As String)
        _t_itmt = value
      End Set
    End Property
    Public Property t_txta() As Int32
      Get
        Return _t_txta
      End Get
      Set(ByVal value As Int32)
        _t_txta = value
      End Set
    End Property
    Public Property t_txtb() As Int32
      Get
        Return _t_txtb
      End Get
      Set(ByVal value As Int32)
        _t_txtb = value
      End Set
    End Property
    Public Property t_cuni() As String
      Get
        Return _t_cuni
      End Get
      Set(ByVal value As String)
        _t_cuni = value
      End Set
    End Property
    Public Property t_oitm() As String
      Get
        Return _t_oitm
      End Get
      Set(ByVal value As String)
        _t_oitm = value
      End Set
    End Property
    Public Property t_elem() As String
      Get
        Return _t_elem
      End Get
      Set(ByVal value As String)
        _t_elem = value
      End Set
    End Property
    Public Property t_Refcntd() As Int32
      Get
        Return _t_Refcntd
      End Get
      Set(ByVal value As Int32)
        _t_Refcntd = value
      End Set
    End Property
    Public Property t_Refcntu() As Int32
      Get
        Return _t_Refcntu
      End Get
      Set(ByVal value As Int32)
        _t_Refcntu = value
      End Set
    End Property
    Public ReadOnly Property DisplayField() As String
      Get
        Return ""
      End Get
    End Property
    Public ReadOnly Property PrimaryKey() As String
      Get
        Return _t_docn & "|" & _t_revn & "|" & _t_srno
      End Get
    End Property
    Public Shared Property RecordCount() As Integer
      Get
        Return _RecordCount
      End Get
      Set(ByVal value As Integer)
        _RecordCount = value
      End Set
    End Property
    Public Class PKdmisg002
      Private _t_docn As String = ""
      Private _t_revn As String = ""
      Private _t_srno As Int32 = 0
      Public Property t_docn() As String
        Get
          Return _t_docn
        End Get
        Set(ByVal value As String)
          _t_docn = value
        End Set
      End Property
      Public Property t_revn() As String
        Get
          Return _t_revn
        End Get
        Set(ByVal value As String)
          _t_revn = value
        End Set
      End Property
      Public Property t_srno() As Int32
        Get
          Return _t_srno
        End Get
        Set(ByVal value As Int32)
          _t_srno = value
        End Set
      End Property
    End Class
    '1. Used
    Public Shared Function Getdmisg002(ByVal docn As String, ByVal revn As String, Optional Comp As String = "200") As List(Of SIS.DMISG.dmisg002)
      Dim tmp As New List(Of SIS.DMISG.dmisg002)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "Select * from tdmisg002" & Comp & " where t_docn = '" & docn & "' and t_revn='" & revn & "' order by t_srno"
          Dim rd As SqlDataReader = Cmd.ExecuteReader
          While rd.Read
            tmp.Add(New SIS.DMISG.dmisg002(rd))
          End While
        End Using
      End Using
      Return tmp
    End Function

    Public Sub New(ByVal Reader As SqlDataReader)
      Try
        For Each pi As System.Reflection.PropertyInfo In Me.GetType.GetProperties
          If pi.MemberType = Reflection.MemberTypes.Property Then
            Try
              Dim Found As Boolean = False
              For I As Integer = 0 To Reader.FieldCount - 1
                If Reader.GetName(I).ToLower = pi.Name.ToLower Then
                  Found = True
                  Exit For
                End If
              Next
              If Found Then
                If Convert.IsDBNull(Reader(pi.Name)) Then
                  Select Case Reader.GetDataTypeName(Reader.GetOrdinal(pi.Name))
                    Case "decimal"
                      CallByName(Me, pi.Name, CallType.Let, "0.00")
                    Case "bit"
                      CallByName(Me, pi.Name, CallType.Let, Boolean.FalseString)
                    Case Else
                      CallByName(Me, pi.Name, CallType.Let, String.Empty)
                  End Select
                Else
                  CallByName(Me, pi.Name, CallType.Let, Reader(pi.Name))
                End If
              End If
            Catch ex As Exception
            End Try
          End If
        Next
      Catch ex As Exception
      End Try
    End Sub
    Public Sub New()
    End Sub
  End Class
End Namespace
