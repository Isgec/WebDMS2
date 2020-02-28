Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.DMISG
  <DataObject()>
  Partial Public Class dmisg004
    Private Shared _RecordCount As Integer
    Private _t_docn As String = ""
    Private _t_revn As String = ""
    Private _t_srno As Int32 = 0
    Private _t_item As String = ""
    Private _t_pisn As String = ""
    Private _t_prtn As String = ""
    Private _t_prtd As String = ""
    Private _t_wght As Double = 0
    Private _t_qnty As Double = 0
    Private _t_spec As String = ""
    Private _t_size As String = ""
    Private _t_rmrk As String = ""
    Private _t_Refcntd As Int32 = 0
    Private _t_Refcntu As Int32 = 0
    Private _t_mcod As String = ""
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
    Public Property t_pisn() As String
      Get
        Return _t_pisn
      End Get
      Set(ByVal value As String)
        _t_pisn = value
      End Set
    End Property
    Public Property t_prtn() As String
      Get
        Return _t_prtn
      End Get
      Set(ByVal value As String)
        _t_prtn = value
      End Set
    End Property
    Public Property t_prtd() As String
      Get
        Return _t_prtd
      End Get
      Set(ByVal value As String)
        _t_prtd = value
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
    Public Property t_qnty() As Double
      Get
        Return _t_qnty
      End Get
      Set(ByVal value As Double)
        _t_qnty = value
      End Set
    End Property
    Public Property t_spec() As String
      Get
        Return _t_spec
      End Get
      Set(ByVal value As String)
        _t_spec = value
      End Set
    End Property
    Public Property t_size() As String
      Get
        Return _t_size
      End Get
      Set(ByVal value As String)
        _t_size = value
      End Set
    End Property
    Public Property t_rmrk() As String
      Get
        Return _t_rmrk
      End Get
      Set(ByVal value As String)
        _t_rmrk = value
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
    Public Property t_mcod() As String
      Get
        Return _t_mcod
      End Get
      Set(ByVal value As String)
        _t_mcod = value
      End Set
    End Property
    Public ReadOnly Property DisplayField() As String
      Get
        Return ""
      End Get
    End Property
    Public ReadOnly Property PrimaryKey() As String
      Get
        Return _t_docn & "|" & _t_revn & "|" & _t_srno & "|" & _t_item & "|" & _t_pisn
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
    Public Class PKdmisg004
      Private _t_docn As String = ""
      Private _t_revn As String = ""
      Private _t_srno As Int32 = 0
      Private _t_item As String = ""
      Private _t_pisn As String = ""
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
      Public Property t_pisn() As String
        Get
          Return _t_pisn
        End Get
        Set(ByVal value As String)
          _t_pisn = value
        End Set
      End Property
    End Class

    '1 Used
    Public Shared Function Getdmisg004(ByVal docn As String, ByVal revn As String, ByVal srno As String, Optional Comp As String = "200") As List(Of SIS.DMISG.dmisg004)
      Dim tmp As New List(Of SIS.DMISG.dmisg004)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "Select * from tdmisg004" & Comp & " where t_docn = '" & docn & "' and t_revn='" & revn & "' and t_srno=" & srno & " order by t_pisn"
          Dim rd As SqlDataReader = Cmd.ExecuteReader
          While rd.Read
            tmp.Add(New SIS.DMISG.dmisg004(rd))
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
