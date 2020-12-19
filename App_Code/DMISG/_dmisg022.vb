Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.DMISG
  <DataObject()>
  Public Class BOM
    Public Property DocWeight As Decimal = 0.00
    Public Property DocumentID As String = ""
    Public Property RevisionNo As String = ""
    Public Property Title As String = ""
    Public Property SrNo As String = ""
    Public Property ItemID As String = ""
    Public Property ItemDescription As String = ""
    Public Property ItemQuantity As Decimal = 0.00
    Public Property ItemWeight As Decimal = 0.00
    Public Property ItemUnit As String = ""
    Public Property PartItemID As String = ""
    Public Property PartDescription As String = ""
    Public Property PartSpecification As String = ""
    Public Property PartSize As String = ""
    Public Property PartQuantity As Decimal = 0.00
    Public Property PartWeight As Decimal = 0.00
    Public Property PartRemarks As String = ""
    Public Shared Function GetDoc(ByVal docn As String, ByVal revn As String, Optional Comp As String = "200") As SIS.DMISG.BOM
      Dim Sql As String = ""
      Sql &= " select top 1  "
      Sql &= "   dm.t_wght as DocWeight, "
      Sql &= "   dm.t_docn as DocumentID, "
      Sql &= "   dm.t_revn as RevisionNo, "
      Sql &= "   dm.t_dttl as Title "
      Sql &= " from tdmisg001" & Comp & " as dm  "
      Sql &= " where dm.t_docn = '" & docn & "' and dm.t_revn='" & revn & "' "
      Dim tmp As SIS.DMISG.BOM = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Dim rd As SqlDataReader = Cmd.ExecuteReader
          If rd.Read Then
            tmp = New SIS.DMISG.BOM(rd)
          End If
        End Using
      End Using
      Return tmp
    End Function
    Public Shared Function GetBOM(ByVal docn As String, ByVal revn As String, Optional Comp As String = "200") As List(Of SIS.DMISG.BOM)
      Dim Sql As String = ""
      Sql &= " select  "
      Sql &= "   it.t_srno as SrNo, "
      Sql &= "   it.t_item as ItemID, "
      Sql &= "   it.t_dsca as ItemDescription, "
      Sql &= "   it.t_qnty as ItemQuantity, "
      Sql &= "   it.t_wght as ItemWeight, "
      Sql &= "   it.t_cuni as ItemUnit "
      Sql &= " from tdmisg002" & Comp & " as it  "
      Sql &= " where it.t_docn = '" & docn & "' and it.t_revn='" & revn & "' "
      Dim tmp As New List(Of SIS.DMISG.BOM)

      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Dim rd As SqlDataReader = Cmd.ExecuteReader
          While rd.Read
            tmp.Add(New SIS.DMISG.BOM(rd))
          End While
        End Using
      End Using
      Return tmp
    End Function
    Public Shared Function GetPBOM(ByVal docn As String, ByVal revn As String, ByVal srno As String, Optional Comp As String = "200") As List(Of SIS.DMISG.BOM)
      Dim Sql As String = ""
      Sql &= " select  "
      Sql &= "   pt.t_prtn as PartItemID, "
      Sql &= "   pt.t_prtd as PartDescription, "
      Sql &= "   pt.t_spec as PartSpecification, "
      Sql &= "   pt.t_size as PartSize, "
      Sql &= "   pt.t_qnty as PartQuantity, "
      Sql &= "   pt.t_wght as PartWeight,  "
      Sql &= "   pt.t_rmrk as PartRemarks  "
      Sql &= " from tdmisg004" & Comp & " as pt  "
      Sql &= " where pt.t_docn = '" & docn & "' and pt.t_revn='" & revn & "' and pt.t_srno='" & srno & "' "
      Dim tmp As New List(Of SIS.DMISG.BOM)

      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Dim rd As SqlDataReader = Cmd.ExecuteReader
          While rd.Read
            tmp.Add(New SIS.DMISG.BOM(rd))
          End While
        End Using
      End Using
      Return tmp
    End Function

    Public Shared Function GetBOM_Old(ByVal docn As String, ByVal revn As String, Optional Comp As String = "200") As List(Of SIS.DMISG.BOM)
      Dim Sql As String = ""
      Sql &= " select  "
      Sql &= "   dm.t_docn as DocumentID, "
      Sql &= "   dm.t_revn as RevisionNo, "
      Sql &= "   dm.t_dttl as Title, "
      Sql &= "   it.t_item as ItemID, "
      Sql &= "   it.t_dsca as ItemDescription, "
      Sql &= "   it.t_qnty as ItemQuantity, "
      Sql &= "   it.t_wght as ItemWeight, "
      Sql &= "   it.t_cuni as ItemUnit, "
      Sql &= "   pt.t_prtn as PartItemID, "
      Sql &= "   pt.t_prtd as PartDescription, "
      Sql &= "   pt.t_spec as PartSpecification, "
      Sql &= "   pt.t_size as PartSize, "
      Sql &= "   pt.t_qnty as PartQuantity, "
      Sql &= "   pt.t_wght as PartWeight,  "
      Sql &= "   pt.t_rmrk as PartRemarks  "
      Sql &= " from tdmisg001" & Comp & " as dm  "
      Sql &= " left outer join tdmisg002" & Comp & " as it on it.t_docn=dm.t_docn and it.t_revn=dm.t_revn "
      Sql &= " left outer join tdmisg004" & Comp & " as pt on pt.t_docn=it.t_docn and pt.t_revn=it.t_revn and pt.t_srno=it.t_srno "
      Sql &= " where dm.t_docn = '" & docn & "' and dm.t_revn='" & revn & "' "
      Dim tmp As New List(Of SIS.DMISG.BOM)

      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Dim rd As SqlDataReader = Cmd.ExecuteReader
          While rd.Read
            tmp.Add(New SIS.DMISG.BOM(rd))
          End While
        End Using
      End Using
      Return tmp
    End Function
    Public Shared Function GetRefBOM(ByVal docn As String, ByVal revn As String, Optional Comp As String = "200") As List(Of SIS.DMISG.BOM)
      Dim Sql As String = ""
      Sql &= " select  "
      Sql &= "   it.t_srno as SrNo, "
      Sql &= "   it.t_item as ItemID, "
      Sql &= "   it.t_dsca as ItemDescription, "
      Sql &= "   it.t_qnty as ItemQuantity, "
      Sql &= "   it.t_wght as ItemWeight, "
      Sql &= "   it.t_cuni as ItemUnit  "
      Sql &= " from tdmisg021" & Comp & " as it  "
      Sql &= " where it.t_docn = '" & docn & "' and it.t_revn='" & revn & "' "
      Dim tmp As New List(Of SIS.DMISG.BOM)

      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Dim rd As SqlDataReader = Cmd.ExecuteReader
          While rd.Read
            tmp.Add(New SIS.DMISG.BOM(rd))
          End While
        End Using
      End Using
      Return tmp
    End Function
    Public Shared Function GetRefPBOM(ByVal docn As String, ByVal revn As String, ByVal srno As String, Optional Comp As String = "200") As List(Of SIS.DMISG.BOM)
      Dim Sql As String = ""
      Sql &= " select  "
      Sql &= "   pt.t_prtn as PartItemID, "
      Sql &= "   pt.t_prtd as PartDescription, "
      Sql &= "   pt.t_spec as PartSpecification, "
      Sql &= "   pt.t_size as PartSize, "
      Sql &= "   pt.t_qnty as PartQuantity, "
      Sql &= "   pt.t_wght as PartWeight,  "
      Sql &= "   pt.t_rmrk as PartRemarks  "
      Sql &= " from tdmisg022" & Comp & " as pt  "
      Sql &= " where pt.t_docn = '" & docn & "' and pt.t_revn='" & revn & "' and pt.t_srno='" & srno & "' "
      Dim tmp As New List(Of SIS.DMISG.BOM)

      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Dim rd As SqlDataReader = Cmd.ExecuteReader
          While rd.Read
            tmp.Add(New SIS.DMISG.BOM(rd))
          End While
        End Using
      End Using
      Return tmp
    End Function
    Public Shared Function GetRefBOM_old(ByVal docn As String, ByVal revn As String, Optional Comp As String = "200") As List(Of SIS.DMISG.BOM)
      Dim Sql As String = ""
      Sql &= " select  "
      Sql &= "   dm.t_docn as DocumentID, "
      Sql &= "   dm.t_revn as RevisionNo, "
      Sql &= "   dm.t_dttl as Title, "
      Sql &= "   it.t_item as ItemID, "
      Sql &= "   it.t_dsca as ItemDescription, "
      Sql &= "   it.t_qnty as ItemQuantity, "
      Sql &= "   it.t_wght as ItemWeight, "
      Sql &= "   it.t_cuni as ItemUnit, "
      Sql &= "   pt.t_prtn as PartItemID, "
      Sql &= "   pt.t_prtd as PartDescription, "
      Sql &= "   pt.t_spec as PartSpecification, "
      Sql &= "   pt.t_size as PartSize, "
      Sql &= "   pt.t_qnty as PartQuantity, "
      Sql &= "   pt.t_wght as PartWeight,  "
      Sql &= "   pt.t_rmrk as PartRemarks  "
      Sql &= " from tdmisg001" & Comp & " as dm  "
      Sql &= " inner join tdmisg021" & Comp & " as it on it.t_docn=dm.t_docn and it.t_revn=dm.t_revn "
      Sql &= " left outer join tdmisg022" & Comp & " as pt on pt.t_docn=it.t_docn and pt.t_revn=it.t_revn and pt.t_srno=it.t_srno "
      Sql &= " where dm.t_docn = '" & docn & "' and dm.t_revn='" & revn & "' "
      Dim tmp As New List(Of SIS.DMISG.BOM)

      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Dim rd As SqlDataReader = Cmd.ExecuteReader
          While rd.Read
            tmp.Add(New SIS.DMISG.BOM(rd))
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
  <DataObject()>
  Partial Public Class dmisg022
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
    Public Class PKdmisg022
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
    Public Shared Function Getdmisg022(ByVal docn As String, ByVal revn As String, ByVal srno As String, Optional Comp As String = "200") As List(Of SIS.DMISG.dmisg022)
      Dim tmp As New List(Of SIS.DMISG.dmisg022)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "Select * from tdmisg022" & Comp & " where t_docn = '" & docn & "' and t_revn='" & revn & "' and t_srno=" & srno & " order by t_pisn"
          Dim rd As SqlDataReader = Cmd.ExecuteReader
          While rd.Read
            tmp.Add(New SIS.DMISG.dmisg022(rd))
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
  <DataObject()>
  Public Class TDocs
    Public Property DocumentID As String = ""
    Public Property RevisionNo As String = ""
    Public Property EnggFunc As String = ""
    Public Shared Function GetDocs(ByVal prj As String, ByVal eFun As String, Optional Comp As String = "200") As List(Of SIS.DMISG.TDocs)
      Dim Sql As String = ""

      Sql &= " Select distinct td.t_docn As DocumentID,td.t_revn as RevisionNo, th.t_tfld As EnggFunc "
      Sql &= " From tdmisg132200 As td "
      Sql &= " inner Join tdmisg131200 as th on td.t_tran=th.t_tran "
      Sql &= " where "
      Sql &= "     th.t_issu = '007' "
      Sql &= " And th.t_type = 4 "
      Sql &= " And th.t_ofbp='SUPI00002' "
      Sql &= " And th.t_stat = 5 "
      Sql &= " And td.t_revn = (select max(xx.t_revn) from tdmisg132200 as xx where xx.t_docn=td.t_docn) "
      If eFun <> "" Then
        Sql &= " And th.t_tfld = (select t_srno as srno from tdmisg015200 where t_type=4 And t_cprj='" & prj & "' and t_tfid=" & eFun & ") "
      End If
      Sql &= " And th.t_dprj='" & prj & "' "
      Sql &= " Order By th.t_tfld "

      Dim tmp As New List(Of SIS.DMISG.TDocs)

      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Con.Open()
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = Sql
          Dim rd As SqlDataReader = Cmd.ExecuteReader
          While rd.Read
            tmp.Add(New SIS.DMISG.TDocs(rd))
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
