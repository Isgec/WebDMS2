Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.ComponentModel
Namespace SIS.EDI
  <DataObject()>
  Partial Public Class ediAFile
    Private Shared _RecordCount As Integer
    Public Shared Property Comp As String = "200"
    Private _t_drid As String = ""
    Private _t_dcid As String = ""
    Private _t_hndl As String = ""
    Private _t_indx As String = ""
    Private _t_prcd As String = ""
    Private _t_fnam As String = ""
    Private _t_lbcd As String = ""
    Private _t_atby As String = ""
    Private _t_aton As String = ""
    Private _t_Refcntd As Int32 = 0
    Private _t_Refcntu As Int32 = 0
    Public ReadOnly Property GetDownloadLink() As String
      Get
        Return "javascript:window.open('" & HttpContext.Current.Request.Url.Scheme & Uri.SchemeDelimiter & HttpContext.Current.Request.Url.Authority & HttpContext.Current.Request.ApplicationPath & "/App_Downloads/athDownload.aspx?ath=" & t_drid & "', 'win" & t_drid & "', 'left=20,top=20,width=100,height=100,toolbar=1,resizable=1,scrollbars=1'); return false;"
      End Get
    End Property

    Public ReadOnly Property ForeColor() As System.Drawing.Color
      Get
        Dim mRet As System.Drawing.Color = Drawing.Color.Blue
        Try
          mRet = GetColor()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public ReadOnly Property Visible() As Boolean
      Get
        Dim mRet As Boolean = True
        Try
          mRet = GetVisible()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public ReadOnly Property Enable() As Boolean
      Get
        Dim mRet As Boolean = True
        Try
          mRet = GetEnable()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property

    Public Property t_drid() As String
      Get
        Return _t_drid
      End Get
      Set(ByVal value As String)
        _t_drid = value
      End Set
    End Property
    Public Property t_dcid() As String
      Get
        Return _t_dcid
      End Get
      Set(ByVal value As String)
        _t_dcid = value
      End Set
    End Property
    Public Property t_hndl() As String
      Get
        Return _t_hndl
      End Get
      Set(ByVal value As String)
        _t_hndl = value
      End Set
    End Property
    Public Property t_indx() As String
      Get
        Return _t_indx
      End Get
      Set(ByVal value As String)
        _t_indx = value
      End Set
    End Property
    Public Property t_prcd() As String
      Get
        Return _t_prcd
      End Get
      Set(ByVal value As String)
        _t_prcd = value
      End Set
    End Property
    Public Property t_fnam() As String
      Get
        Return _t_fnam
      End Get
      Set(ByVal value As String)
        _t_fnam = value
      End Set
    End Property
    Public Property t_lbcd() As String
      Get
        Return _t_lbcd
      End Get
      Set(ByVal value As String)
        _t_lbcd = value
      End Set
    End Property
    Public Property t_atby() As String
      Get
        Return _t_atby
      End Get
      Set(ByVal value As String)
        _t_atby = value
      End Set
    End Property
    Public Property t_aton() As String
      Get
        If Not _t_aton = String.Empty Then
          Return Convert.ToDateTime(_t_aton).ToString("dd/MM/yyyy")
        End If
        Return _t_aton
      End Get
      Set(ByVal value As String)
        _t_aton = value
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
    Public Shared Function GetNextFileName(Optional ByVal Comp As String = "200") As String
      Dim UniqueFound As Boolean = False
      Dim tmpNextNo As String = (New Random(Guid.NewGuid().GetHashCode())).Next()
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Con.Open()
        Do While Not UniqueFound
          Using Cmd As SqlCommand = Con.CreateCommand()
            Cmd.CommandType = CommandType.Text
            Cmd.CommandText = "select isnull(count(*),0) from ttcisg132" & Comp & " where t_drid = '" & tmpNextNo & "'"
            Dim cnt As Integer = Cmd.ExecuteScalar()
            If cnt = 0 Then
              UniqueFound = True
              Exit Do
            End If
            tmpNextNo = (New Random(Guid.NewGuid().GetHashCode())).Next()
          End Using
        Loop
      End Using
      Return tmpNextNo
    End Function
    Public Shared Function ediAFileGetByHandleIndex(ByVal t_hndl As String, ByVal t_indx As String, Optional ByVal Comp As String = "200") As SIS.EDI.ediAFile
      Dim Results As SIS.EDI.ediAFile = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "Select top 1 * from ttcisg132" & Comp & " where t_hndl='" & t_hndl & "' and t_indx='" & t_indx & "'"
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.EDI.ediAFile(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function ediAFileGetAllByHandleIndex(ByVal t_hndl As String, ByVal t_indx As String, Optional ByVal Comp As String = "200") As List(Of SIS.EDI.ediAFile)
      Dim Results As New List(Of SIS.EDI.ediAFile)
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "Select top 1 * from ttcisg132" & Comp & " where t_hndl='" & t_hndl & "' and t_indx='" & t_indx & "'"
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While Reader.Read()
            Results.Add(New SIS.EDI.ediAFile(Reader))
          End While
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function InsertData(ByVal Record As SIS.EDI.ediAFile, Optional ByVal Comp As String = "200") As SIS.EDI.ediAFile
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.StoredProcedure
          Cmd.CommandText = "spediAFileInsert"
          If Comp <> "200" Then Cmd.CommandText = "spediAFileInsert" & Comp
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_dcid", SqlDbType.VarChar, 201, Record.t_dcid)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_hndl", SqlDbType.VarChar, 201, Record.t_hndl)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_indx", SqlDbType.VarChar, 51, Record.t_indx)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_prcd", SqlDbType.VarChar, 51, Record.t_prcd)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_fnam", SqlDbType.VarChar, 251, Record.t_fnam)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_lbcd", SqlDbType.VarChar, 51, Record.t_lbcd)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_atby", SqlDbType.VarChar, 51, Record.t_atby)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_aton", SqlDbType.DateTime, 21, Record.t_aton)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_Refcntd", SqlDbType.Int, 11, Record.t_Refcntd)
          SIS.SYS.SQLDatabase.DBCommon.AddDBParameter(Cmd, "@t_Refcntu", SqlDbType.Int, 11, Record.t_Refcntu)
          Cmd.Parameters.Add("@Return_t_drid", SqlDbType.VarChar, 51)
          Cmd.Parameters("@Return_t_drid").Direction = ParameterDirection.Output
          Con.Open()
          Cmd.ExecuteNonQuery()
          Record.t_drid = Cmd.Parameters("@Return_t_drid").Value
        End Using
      End Using
      Return Record
    End Function
    Public Shared Function ediAFileSelectList(ByVal StartRowIndex As Integer, ByVal MaximumRows As Integer, ByVal OrderBy As String, ByVal SearchState As Boolean, ByVal SearchText As String, ByVal t_hndl As String, ByVal t_indx As String) As List(Of SIS.EDI.ediAFile)
      If CType(ConfigurationManager.AppSettings("RunLocally"), Boolean) Then Return New List(Of SIS.EDI.ediAFile)()
      Dim Results As List(Of SIS.EDI.ediAFile) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "Select * from ttcisg132" & Comp & " where t_hndl='" & t_hndl & "' and t_indx='" & t_indx & "'"
          _RecordCount = -1
          Results = New List(Of SIS.EDI.ediAFile)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.EDI.ediAFile(Reader))
          End While
          Reader.Close()
          _RecordCount = Results.Count
        End Using
      End Using
      Return Results
    End Function
    Public Shared Function ediAFileSelectCount(ByVal SearchState As Boolean, ByVal SearchText As String, ByVal t_hndl As String, ByVal t_indx As String) As Integer
      Return _RecordCount
    End Function
    Public Shared Function ediAFileGetDCID(ByVal t_dcid As String) As SIS.EDI.ediAFile
      Dim Results As SIS.EDI.ediAFile = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "Select top 1 * from ttcisg132" & Comp & " where t_dcid='" & t_dcid & "'"
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.EDI.ediAFile(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    <DataObjectMethod(DataObjectMethodType.Select)>
    Public Shared Function ediAFileGetByID(ByVal t_drid As String) As SIS.EDI.ediAFile
      Dim Results As SIS.EDI.ediAFile = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "Select top 1 * from ttcisg132" & Comp & " where t_drid='" & t_drid & "'"
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          If Reader.Read() Then
            Results = New SIS.EDI.ediAFile(Reader)
          End If
          Reader.Close()
        End Using
      End Using
      Return Results
    End Function
    'Select By ID One Record Filtered Overloaded GetByID
    <DataObjectMethod(DataObjectMethodType.Select)>
    Public Shared Function ediAFileGetByID(ByVal t_drid As String, ByVal Filter_t_hndl As String, ByVal Filter_t_indx As String) As SIS.EDI.ediAFile
      Return ediAFileGetByID(t_drid)
    End Function
    Public Shared Function ediAFileCopy(ByVal s_t_hndl As String, ByVal s_t_indx As String, ByVal t_t_hndl As String, ByVal t_t_indx As String) As List(Of SIS.EDI.ediAFile)
      If CType(ConfigurationManager.AppSettings("RunLocally"), Boolean) Then Return New List(Of SIS.EDI.ediAFile)()
      Dim Results As List(Of SIS.EDI.ediAFile) = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "Select * from ttcisg132" & Comp & " where t_hndl='" & s_t_hndl & "' and t_indx='" & s_t_indx & "'"
          _RecordCount = -1
          Results = New List(Of SIS.EDI.ediAFile)()
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results.Add(New SIS.EDI.ediAFile(Reader))
          End While
          Reader.Close()
          _RecordCount = Results.Count
        End Using
      End Using
      For Each tmp As SIS.EDI.ediAFile In Results
        With tmp
          .t_drid = GetNextFileName()
          .t_hndl = t_t_hndl
          .t_indx = t_t_indx
        End With
        tmp = SIS.EDI.ediAFile.InsertData(tmp)
      Next
      Return Results
    End Function
    Public Shared Function ediAFileCopy(ByVal t_drid As String, ByVal t_t_hndl As String, ByVal t_t_indx As String) As SIS.EDI.ediAFile
      If CType(ConfigurationManager.AppSettings("RunLocally"), Boolean) Then Return New SIS.EDI.ediAFile
      Dim Results As SIS.EDI.ediAFile = Nothing
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "Select * from ttcisg132" & Comp & " where t_drid='" & t_drid & "'"
          _RecordCount = -1
          Con.Open()
          Dim Reader As SqlDataReader = Cmd.ExecuteReader()
          While (Reader.Read())
            Results = New SIS.EDI.ediAFile(Reader)
          End While
          Reader.Close()
        End Using
      End Using
      With Results
        .t_drid = GetNextFileName()
        .t_hndl = t_t_hndl
        .t_indx = t_t_indx
      End With
      Results = SIS.EDI.ediAFile.InsertData(Results)
      Return Results
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
    Public ReadOnly Property DeleteWFVisible() As Boolean
      Get
        Dim mRet As Boolean = True
        Try
          mRet = GetVisible()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public ReadOnly Property DeleteWFEnable() As Boolean
      Get
        Dim mRet As Boolean = True
        Try
          mRet = GetEnable()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public Shared Function DeleteWF(ByVal t_drid As String, Optional ByVal Process As String = "") As SIS.EDI.ediAFile
      Dim Results As SIS.EDI.ediAFile = ediAFileGetByID(t_drid)
      If CType(ConfigurationManager.AppSettings("RunLocally"), Boolean) Then Return New SIS.EDI.ediAFile
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "delete ttcisg132" & Comp & " where t_drid='" & t_drid & "'"
          Con.Open()
          Cmd.ExecuteNonQuery()
        End Using
      End Using
      If Process = Results.t_prcd Then
        Try
          Dim LibFolder As String = "attachmentlibrary1"
          Dim libPath As String = ""
          Dim NeedsMapping As Boolean = False
          Dim Mapped As Boolean = False
          Dim LibraryID As String = ""
          Dim tmp As SIS.EDI.ediALib = SIS.EDI.ediALib.GetActiveLibrary
          LibFolder = tmp.t_path
          LibraryID = tmp.t_lbcd
          Dim UrlAuthority As String = HttpContext.Current.Request.Url.Authority
          If UrlAuthority.ToLower <> "cloud.isgec.co.in" Then
            UrlAuthority = "192.9.200.146"
            NeedsMapping = True
          End If
          libPath = "D:\" & LibFolder
          If NeedsMapping Then
            libPath = "\\" & UrlAuthority & "\" & LibFolder
            If ConnectToNetworkFunctions.connectToNetwork(libPath, "X:", "administrator", "Indian@12345") Then
              Mapped = True
            End If
          End If
          If IO.File.Exists(libPath & "\" & Results.t_dcid) Then
            IO.File.Delete(libPath & "\" & Results.t_dcid)
          End If
          If Mapped Then
            ConnectToNetworkFunctions.disconnectFromNetwork("X:")
          End If
        Catch ex As Exception
        End Try
      End If
      Return Results
    End Function
    Public ReadOnly Property DownloadWFVisible() As Boolean
      Get
        Dim mRet As Boolean = True
        Try
          mRet = GetVisible()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public ReadOnly Property DownloadWFEnable() As Boolean
      Get
        Dim mRet As Boolean = True
        Try
          mRet = GetEnable()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public Shared Function DownloadWF(ByVal t_drid As String) As SIS.EDI.ediAFile
      Dim Results As SIS.EDI.ediAFile = ediAFileGetByID(t_drid)
      Return Results
    End Function
    Public ReadOnly Property InitiateWFVisible() As Boolean
      Get
        Dim mRet As Boolean = True
        Try
          mRet = GetVisible()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public ReadOnly Property InitiateWFEnable() As Boolean
      Get
        Dim mRet As Boolean = True
        Try
          mRet = GetEnable()
        Catch ex As Exception
        End Try
        Return mRet
      End Get
    End Property
    Public ReadOnly Property PrimaryKey As String
      Get
        Return t_drid
      End Get
    End Property
    Public Shared Function DeleteByHandleIndex(ByVal t_hndl As String, ByVal t_indx As String, Optional ByVal Comp As String = "200") As Integer
      If CType(ConfigurationManager.AppSettings("RunLocally"), Boolean) Then Return 0
      Using Con As SqlConnection = New SqlConnection(SIS.SYS.SQLDatabase.DBCommon.GetBaaNConnectionString())
        Using Cmd As SqlCommand = Con.CreateCommand()
          Cmd.CommandType = CommandType.Text
          Cmd.CommandText = "delete ttcisg132" & Comp & " where t_hndl='" & t_hndl & "' and t_indx='" & t_indx & "'"
          Con.Open()
          Cmd.ExecuteNonQuery()
        End Using
      End Using
      Return 1
    End Function

    Public Shared Function InitiateWF(ByVal t_drid As String) As SIS.EDI.ediAFile
      Dim Results As SIS.EDI.ediAFile = ediAFileGetByID(t_drid)
      Return Results
    End Function
    Public Shared Function UploadFiles(ByVal oRequest As HttpRequest, ByVal AthHandle As String, ByVal AthIndex As String, Optional ByVal CreatedBy As String = "", Optional ByVal AthProcess As String = "") As List(Of SIS.EDI.ediAFile)
      Dim mRet As New List(Of SIS.EDI.ediAFile)
      If oRequest.Files.Count > 0 Then
        Dim LibFolder As String = "attachmentlibrary1"
        Dim libPath As String = ""
        Dim NeedsMapping As Boolean = False
        Dim Mapped As Boolean = False
        Dim LibraryID As String = ""
        Dim tmp As SIS.EDI.ediALib = SIS.EDI.ediALib.GetActiveLibrary
        LibFolder = tmp.t_path
        LibraryID = tmp.t_lbcd
        Dim UrlAuthority As String = HttpContext.Current.Request.Url.Authority
        If UrlAuthority.ToLower <> "cloud.isgec.co.in" Then
          UrlAuthority = "192.9.200.146"
          NeedsMapping = True
        End If
        libPath = "D:\" & LibFolder
        If NeedsMapping Then
          libPath = "\\" & UrlAuthority & "\" & LibFolder
          If ConnectToNetworkFunctions.connectToNetwork(libPath, "X:", "administrator", "Indian@12345") Then
            Mapped = True
          End If
        End If
        Dim tmpFilesToDelete As New ArrayList

        For I As Integer = 0 To oRequest.Files.Count - 1
          Dim fu As HttpPostedFile = oRequest.Files(I)
          If fu.ContentLength <= 0 Then Continue For
          If fu.FileName = "" Then Continue For
          Dim tmpPath As String = HttpContext.Current.Server.MapPath("~/../App_Temp")
          Dim tmpName As String = IO.Path.GetRandomFileName()
          Dim tmpFile As String = tmpPath & "\\" & tmpName
          fu.SaveAs(tmpFile)
          tmpFilesToDelete.Add(tmpFile)

          Dim tmpVault As SIS.EDI.ediAFile = New SIS.EDI.ediAFile
          Dim LibPDFFile As String = ""
          With tmpVault
            LibPDFFile = SIS.EDI.ediASeries.GetNextFileName
            .t_dcid = LibPDFFile
            .t_hndl = AthHandle
            .t_indx = AthIndex
            .t_prcd = AthProcess
            .t_fnam = IO.Path.GetFileName(fu.FileName)
            .t_lbcd = LibraryID
            .t_atby = IIf(CreatedBy = "", HttpContext.Current.Session("LoginID"), CreatedBy)
            .t_aton = Now
            .t_Refcntd = 0
            .t_Refcntu = 0
          End With
          tmpVault = SIS.EDI.ediAFile.InsertData(tmpVault)
          Try
            If IO.File.Exists(libPath & "\" & LibPDFFile) Then
              IO.File.Delete(libPath & "\" & LibPDFFile)
            End If
            IO.File.Move(tmpFile, libPath & "\" & LibPDFFile)
          Catch ex As Exception
          End Try
          mRet.Add(tmpVault)
        Next
        'Move command is used, Below code not required
        'For Each str As String In tmpFilesToDelete
        '  IO.File.Delete(str)
        'Next
        If Mapped Then
          ConnectToNetworkFunctions.disconnectFromNetwork("X:")
        End If
      End If
      Return mRet
    End Function
  End Class
End Namespace
