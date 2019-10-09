<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="LGDefault" title="Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" ClientIDMode="Static" runat="Server">
  <link href="App_Scripts/dms.css" rel="stylesheet" />
  <script src="App_Scripts/splitMaster/src/split.min.js"></script>
  <script src="App_Scripts/dmsMain.js"></script>
  <script src="App_Scripts/dmsItems.js"></script>
  <script src="App_Scripts/filedownload/jquery.fileDownload.js"></script>
  <script src="App_Scripts/Search.js"></script>
  <link href="App_Scripts/menu/lgMenu.css" rel="stylesheet" />
  <%--Menu Section--%>
  <div class="container-fluid">
    <div class="row">
      <div class="col-sm-9" style="background-color: #808080;">
        <div class="btn-group">
        <div class="dropdown">
          <button type="button" class="btn btn-sm btn-success dropdown-toggle" data-toggle="dropdown">
            File
          </button>
          <div class="dropdown-menu shadow" style="background-color:#8ac78a">
            <a class="dropdown-item" href="~/ChangePassword.aspx" runat="server">Change Password</a>
            <a href="#">
              <asp:LoginStatus ID="LoginStatus1" CssClass="dropdown-item" runat="server" LoginText="Sign In" LogoutAction="Redirect" LogoutPageUrl="~/Login.aspx" LogoutText="Sign Out" />
            </a>
          </div>
        </div>
          &nbsp;&nbsp;
        <div class="dropdown">
          <button type="button" class="btn btn-sm btn-info dropdown-toggle" data-toggle="dropdown">
            View
          </button>
          <div class="dropdown-menu shadow" style="background-color:#b3d5ed">
            <a class="dropdown-item" href="#" runat="server">History</a>
            <a class="dropdown-item" href="#" runat="server">Property</a>
          </div>
        </div>
        </div>
      </div>
      <div class="col-sm-3 search" style="background-color: #808080;">
        <div class="input-group input-group-sm">
          <div class="input-group-prepend">
            <div class="input-group-text">
              <input type="checkbox" disabled="disabled" onclick="var_search.exitSearchMode(this);" id="chkSearch">
            </div>
          </div>
          <input type="text" class="form-control" placeholder="search..." id="txtSearch" onkeyup="return var_search.textEntered(this);" />
          <div class="input-group-append">
            <button class="btn btn-primary" value="Start" disabled="disabled" id="cmdStartSearch" onclick="var_search.startSearch(this);"><i id="icmdStartSearch" class="fas fa-cog"></i>&nbsp;Start</button>
            <input class="btn btn-danger" disabled="disabled" type="button" value="Stop" id="cmdStopSearch" onclick="var_search.stopSearch(this);" />
          </div>
        </div>
      </div>
    </div>
  </div>
  <%--Split Section--%>
  <div id="splitSection" style="position: fixed; height: 90%; width: 100%; top: auto; left: 0; bottom: auto;">
    <div id="dmsRoot" runat="server" class="split content split-horizontal">
    </div>
    <div id="dmsGrid" class="split content split-horizontal">
      <div id="a" runat="server" class="split content" style="overflow: hidden;">
        <%--Grid View--%>
        <div class="container-fluid">
          <div class="row">
            <div class="col-sm-1 btn-info">ID</div>
            <div class="col-sm-1 btn-info">
              <div class="custom-control custom-checkbox">
                <input type="checkbox" class="custom-control-input" id="checkAll">
                <label class="custom-control-label" for="checkAll"></label>
              </div>
            </div>
            <div class="col-sm-1 btn-info">
              Type
            </div>
            <div class="col-sm-5 btn-info">
              Description
            </div>
            <div class="col-sm-2 btn-info">
              Status
            </div>
            <div class="col-sm-2 btn-info">
              Date
            </div>
          </div>
        </div>
        <div class="container-fluid" id="gridBody" style="height: 85%; overflow-y: scroll;"></div>
        <div class="container-fluid" style="margin-bottom: 0px;">
          <div class="row">
            <div class="col-sm-12 btn-info">Options</div>
          </div>
        </div>
        <%--End Grid View--%>
      </div>
      <div id="c" class="split content">
        <div id="p" runat="server" class="split content split-horizontal" style="overflow: hidden;">
          <%--History View--%>
          <div class="container-fluid">
            <div class="row">
              <div class="col-sm-1 btn-dark">
                SN
              </div>
              <div class="col-sm-4 btn-dark">
                Remarks
              </div>
              <div class="col-sm-2 btn-dark">
                Status
              </div>
              <div class="col-sm-2 btn-dark">
                Date
              </div>
              <div class="col-sm-3 btn-dark">
                User
              </div>
            </div>
          </div>
          <div class="container-fluid" id="historyBody" style="height: 85%; overflow-y: scroll;">
          </div>
          <div class="container-fluid" style="margin-bottom: 0px;">
            <div class="row">
              <div class="col-sm-12 btn-info"></div>
            </div>
          </div>
          <%--End History View--%>
        </div>
        <div id="q" runat="server" class="split content split-horizontal">
        </div>
      </div>
    </div>
  </div>

  <%--Script to Create Split View--%>
  <script>
    Split(['#a', '#c'], {
      sizes: [55, 45],
      gutterSize: 20,
      cursor: 'pointer',
      direction: 'vertical',
    });
    Split(['#dmsRoot', '#dmsGrid'], {
      sizes: [30, 70],
      gutterSize: 20,
      cursor: 'row-resize',
    });
    Split(['#p', '#q'], {
      sizes: [60, 40],
      gutterSize: 20,
      cursor: 'row-resize',
    });
  </script>

  <%--Status Bar--%>
  <div class="statusbar">
    <div class="btn-group-sm">
      <span id="SelectedNode" class="btn btn-primary"></span>
      <span id="SelectedOption" class="btn btn-warning"></span>
      <span id="ReloadingNode" class="btn btn-danger"></span>
    </div>
  </div>

  <%--Context Menu--%>
  <div id="dmsContextMenu" class="dmsCmenuHide" style="box-shadow: 10px 10px 5px #C0C0C0;">
    <div class="container" style="padding: 0px;">
      <div class="btn-group-vertical">
        <button class="btn btn-sm btn-outline-primary text-left" data-dmsname="Create" onclick="return dms_cmMenu.menu_click(this);"><i class="fas fa-file"></i>&nbsp;&nbsp;Create</button>
        <button class="btn btn-sm btn-outline-danger text-left" data-dmsname="Publish" onclick="return dms_cmMenu.menu_click(this);"><i class="fas fa-torii-gate" style="color: goldenrod;"></i>&nbsp;&nbsp;Publish</button>
        <button class="btn btn-sm btn-outline-success text-left" data-dmsname="Upload" onclick="return dms_cmMenu.menu_click(this);"><i class="fas fa-upload"></i>&nbsp;&nbsp;Upload Files</button>
        <button class="btn btn-sm btn-outline-warning text-left" data-dmsname="Authorize" onclick="return dms_cmMenu.menu_click(this);"><i class="fas fa-user-cog"></i>&nbsp;&nbsp;Authorize</button>
        <button class="btn btn-sm btn-outline-info text-left" data-dmsname="Edit" onclick="return dms_cmMenu.menu_click(this);"><i class="far fa-edit"></i>&nbsp;&nbsp;Edit/Update TAG</button>
        <button class="btn btn-sm btn-outline-secondary text-left" data-dmsname="Download" onclick="return dms_cmMenu.menu_click(this);"><i class="fa fa-download"></i>&nbsp;&nbsp;Download</button>
        <button class="btn btn-sm btn-outline-primary text-left" data-dmsname="Execute" onclick="return dms_cmMenu.menu_click(this);"><i class="fas fa-space-shuttle"></i>&nbsp;&nbsp;Execute</button>
        <button class="btn btn-sm btn-outline-success text-left" data-dmsname="Associate" onclick="return dms_cmMenu.menu_click(this);"><i class="far fa-edit"></i>&nbsp;&nbsp;Associate</button>
        <button class="btn btn-sm btn-outline-danger text-left" data-dmsname="Delete" onclick="return dms_cmMenu.menu_click(this);"><i class="fas fa-cut"></i>&nbsp;&nbsp;Delete</button>
      </div>
    </div>
  </div>
  <%--Entry Form Dialog--%>
  <div class="container-fluid">
    <div class="modal fade" id="myModal">
      <div class="modal-dialog modal-lg h-100 d-flex flex-column justify-content-center my-0 modal-dialog-centered modal-dialog-scrollable">
        <div class="modal-content">
          <div class="modal-header">
            <h4 class="modal-title" id="modalTitle">Property</h4>
            <button type="button" class="close" data-dismiss="modal">&times;</button>
          </div>
          <div id="uiItem" class="modal-body">
            <%--Start of Modal Body--%>
            <div class="container-fluid">
              <div class='row' id="R_ItemID">
                <div class='col'>
                  <b>
                    <asp:Label ID="L_ItemID" ForeColor="#CC6633" runat="server" Text="Item :" /></b>
                </div>
                <div class='col'>
                  <asp:TextBox ID="F_ItemID" Enabled="False" CssClass="form-control" runat="server" Text="0" />
                </div>
              </div>
              <div class='row' id="R_ItemTypeID">
                <div class='col'>
                  <asp:Label ID="L_ItemTypeID" runat="server" Text="Item Type :" /><span style="color: red">*</span>
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_ItemTypeID"
                    DataSourceID="OdsDdldmsItemTypes"
                    OrderBy="DisplayField"
                    DataTextField="DisplayField"
                    DataValueField="PrimaryKey"
                    AppendDataBoundItems="true"
                    CssClass="form-control"
                    Required="required"
                    Enabled="false"
                    runat="server" />
                  <asp:ObjectDataSource
                    ID="OdsDdldmsItemTypes"
                    TypeName="SIS.DMS.dmsItemTypes"
                    SortParameterName="OrderBy"
                    SelectMethod="dmsItemTypesSelectList"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_UserID">
                <div class='col'>
                  <asp:Label ID="L_UserID" runat="server" Text="User :" /><span style="color: red">*</span>
                </div>
                <div class='col'>
                  <div class="input-group-sm">
                    <asp:TextBox
                      ID="F_UserID"
                      CssClass="form-control"
                      AutoCompleteType="None"
                      onfocus="return this.select();"
                      onblur="script_dmsItems.validate_UserID(this);"
                      runat="Server" />
                    <asp:Label
                      ID="F_UserID_Display"
                      CssClass="form-control"
                      disabled="disabled"
                      BackColor="#cccccc"
                      runat="Server" />
                    <AJX:AutoCompleteExtender
                      ID="ACEUserID"
                      BehaviorID="B_ACEUserID"
                      ContextKey=""
                      UseContextKey="true"
                      ServiceMethod="UserIDCompletionList"
                      TargetControlID="F_UserID"
                      EnableCaching="false"
                      CompletionInterval="100"
                      FirstRowSelected="true"
                      MinimumPrefixLength="1"
                      OnClientItemSelected="script_dmsItems.ACEUserID_Selected"
                      OnClientPopulating="script_dmsItems.ACEUserID_Populating"
                      OnClientPopulated="script_dmsItems.ACEUserID_Populated"
                      CompletionSetCount="10"
                      CompletionListCssClass="autocomplete_completionListElement"
                      CompletionListItemCssClass="autocomplete_listItem"
                      CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                      runat="Server" />
                  </div>
                </div>
              </div>
              <div class='row' id="R_FullDescription">
                <div class='col'>
                  <asp:Label ID="L_FullDescription" runat="server" Text="Description :" /><span style="color: red">*</span>
                </div>
                <div class='col'>
                  <asp:TextBox
                    ID="F_FullDescription"
                    CssClass="form-control"
                    onfocus="return this.select();"
                    onblur="this.value=this.value.replace(/\'/g,'');"
                    Required="required"
                    MaxLength="1000"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_Description">
                <div class='col'>
                  <asp:Label ID="L_Description" runat="server" Text="Description :" />
                </div>
                <div class='col'>
                  <asp:TextBox
                    ID="F_Description"
                    CssClass="form-control"
                    disabled="disabled"
                    BackColor="#cccccc"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_KeyWords">
                <div class='col'>
                  <asp:Label ID="L_KeyWords" runat="server" Text="Key Words :" />
                </div>
                <div class='col'>
                  <asp:TextBox ID="F_KeyWords"
                    CssClass="form-control"
                    onfocus="return this.select();"
                    onblur="this.value=this.value.replace(/\'/g,'');"
                    MaxLength="250"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_DeleteFile">
                <div class='col'>
                  <asp:Label ID="L_DeleteFile" runat="server" Text="Delete File :" />
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_DeleteFile"
                    DataSourceID="ODSQualities"
                    AppendDataBoundItems="true"
                    OrderBy="DisplayField"
                    DataTextField="DisplayField"
                    DataValueField="PrimaryKey"
                    CssClass="form-control"
                    runat="server" />
                  <asp:ObjectDataSource
                    ID="ODSQualities"
                    TypeName="SIS.DMS.dmsQualities"
                    SortParameterName="OrderBy"
                    SelectMethod="dmsQualitiesSelectList"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_CreateFile">
                <div class='col'>
                  <asp:Label ID="L_CreateFile" runat="server" Text="Create File :" />
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_CreateFile"
                    DataSourceID="ODSQualities"
                    AppendDataBoundItems="true"
                    OrderBy="DisplayField"
                    DataTextField="DisplayField"
                    DataValueField="PrimaryKey"
                    CssClass="form-control"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_BrowseList">
                <div class='col'>
                  <asp:Label ID="L_BrowseList" runat="server" Text="Browse List :" />&nbsp;
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_BrowseList"
                    DataSourceID="ODSQualities"
                    AppendDataBoundItems="true"
                    OrderBy="DisplayField"
                    DataTextField="DisplayField"
                    DataValueField="PrimaryKey"
                    CssClass="form-control"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_GrantAuthorization">
                <div class='col'>
                  <asp:Label ID="L_GrantAuthorization" runat="server" Text="Grant Authorization :" />&nbsp;
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_GrantAuthorization"
                    DataSourceID="ODSQualities"
                    AppendDataBoundItems="true"
                    OrderBy="DisplayField"
                    DataTextField="DisplayField"
                    DataValueField="PrimaryKey"
                    CssClass="form-control"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_CreateFolder">
                <div class='col'>
                  <asp:Label ID="L_CreateFolder" runat="server" Text="Create Folder :" />&nbsp;
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_CreateFolder"
                    DataSourceID="ODSQualities"
                    AppendDataBoundItems="true"
                    OrderBy="DisplayField"
                    DataTextField="DisplayField"
                    DataValueField="PrimaryKey"
                    CssClass="form-control"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_DeleteFolder">
                <div class='col'>
                  <asp:Label ID="L_DeleteFolder" runat="server" Text="Delete Folder :" />&nbsp;
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_DeleteFolder"
                    DataSourceID="ODSQualities"
                    AppendDataBoundItems="true"
                    OrderBy="DisplayField"
                    DataTextField="DisplayField"
                    DataValueField="PrimaryKey"
                    CssClass="form-control"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_RenameFolder">
                <div class='col'>
                  <asp:Label ID="L_RenameFolder" runat="server" Text="Rename Folder :" />&nbsp;
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_RenameFolder"
                    DataSourceID="ODSQualities"
                    AppendDataBoundItems="true"
                    OrderBy="DisplayField"
                    DataTextField="DisplayField"
                    DataValueField="PrimaryKey"
                    CssClass="form-control"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_ShowInList">
                <div class='col'>
                  <asp:Label ID="L_ShowInList" runat="server" Text="Show In List :" />&nbsp;
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_ShowInList"
                    DataSourceID="ODSQualities"
                    AppendDataBoundItems="true"
                    OrderBy="DisplayField"
                    DataTextField="DisplayField"
                    DataValueField="PrimaryKey"
                    CssClass="form-control"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_Publish">
                <div class='col'>
                  <asp:Label ID="L_Publish" runat="server" Text="Publish :" />&nbsp;
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_Publish"
                    DataSourceID="ODSQualities"
                    AppendDataBoundItems="true"
                    OrderBy="DisplayField"
                    DataTextField="DisplayField"
                    DataValueField="PrimaryKey"
                    CssClass="form-control"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_ConvertedStatusID">
                <div class='col'>
                  <asp:Label ID="L_ConvertedStatusID" runat="server" Text="Associated Status :" />&nbsp;
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_ConvertedStatusID"
                    DataSourceID="ODSStatus"
                    AppendDataBoundItems="true"
                    OrderBy="DisplayField"
                    DataTextField="DisplayField"
                    DataValueField="PrimaryKey"
                    CssClass="form-control"
                    runat="server" />
                  <asp:ObjectDataSource
                    ID="ODSStatus"
                    TypeName="SIS.DMS.dmsStates"
                    SortParameterName="OrderBy"
                    SelectMethod="dmsStatesSelectList"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_CompanyID">
                <div class='col'>
                  <asp:Label ID="L_CompanyID" runat="server" Text="Company :" />
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_CompanyID"
                    DataSourceID="OdsDdlqcmCompanies"
                    AppendDataBoundItems="true"
                    OrderBy="DisplayField"
                    DataTextField="DisplayField"
                    DataValueField="PrimaryKey"
                    CssClass="form-control"
                    runat="server" />
                  <asp:ObjectDataSource
                    ID="OdsDdlqcmCompanies"
                    TypeName="SIS.QCM.qcmCompanies"
                    SortParameterName="OrderBy"
                    SelectMethod="qcmCompaniesSelectList"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_DivisionID">
                <div class='col'>
                  <asp:Label ID="L_DivisionID" runat="server" Text="Division :" />
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_DivisionID"
                    OrderBy="DisplayField"
                    DataTextField="DisplayField"
                    DataValueField="PrimaryKey"
                    DataSourceID="OdsDdlqcmDivisions"
                    AppendDataBoundItems="true"
                    CssClass="form-control"
                    runat="server" />
                  <asp:ObjectDataSource
                    ID="OdsDdlqcmDivisions"
                    TypeName="SIS.QCM.qcmDivisions"
                    SortParameterName="OrderBy"
                    SelectMethod="qcmDivisionsSelectList"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_DepartmentID">
                <div class='col'>
                  <asp:Label ID="L_DepartmentID" runat="server" Text="Department :" />
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_DepartmentID"
                    DataSourceID="OdsDdlqcmDepartments"
                    AppendDataBoundItems="true"
                    OrderBy="DisplayField"
                    DataTextField="DisplayField"
                    DataValueField="PrimaryKey"
                    CssClass="form-control"
                    runat="server" />
                  <asp:ObjectDataSource
                    ID="OdsDdlqcmDepartments"
                    TypeName="SIS.QCM.qcmDepartments"
                    SortParameterName="OrderBy"
                    SelectMethod="qcmDepartmentsSelectList"
                    runat="server" />
                </div>
              </div>
              <div class='row' id="R_ProjectID">
                <div class='col'>
                  <asp:Label ID="L_ProjectID" runat="server" Text="Project :" />
                </div>
                <div class='col'>
                  <div class="input-group-sm">
                    <asp:TextBox
                      ID="F_ProjectID"
                      CssClass="form-control"
                      AutoCompleteType="None"
                      onfocus="return this.select();"
                      onblur="script_dmsItems.validate_ProjectID(this);"
                      runat="Server" />
                    <asp:Label
                      ID="F_ProjectID_Display"
                      CssClass="form-control"
                      disabled="disabled"
                      BackColor="#cccccc"
                      runat="Server" />
                    <AJX:AutoCompleteExtender
                      ID="ACEProjectID"
                      BehaviorID="B_ACEProjectID"
                      ContextKey=""
                      UseContextKey="true"
                      ServiceMethod="ProjectIDCompletionList"
                      TargetControlID="F_ProjectID"
                      EnableCaching="false"
                      CompletionInterval="100"
                      FirstRowSelected="true"
                      MinimumPrefixLength="1"
                      OnClientItemSelected="script_dmsItems.ACEProjectID_Selected"
                      OnClientPopulating="script_dmsItems.ACEProjectID_Populating"
                      OnClientPopulated="script_dmsItems.ACEProjectID_Populated"
                      CompletionSetCount="10"
                      CompletionListCssClass="autocomplete_completionListElement"
                      CompletionListItemCssClass="autocomplete_listItem"
                      CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                      runat="Server" />
                  </div>
                </div>
              </div>
              <div class='row' id="R_WBSID">
                <div class='col'>
                  <asp:Label ID="L_WBSID" runat="server" Text="Element :" />
                </div>
                <div class='col'>
                  <div class="input-group-sm">
                    <asp:TextBox
                      ID="F_WBSID"
                      CssClass="form-control"
                      AutoCompleteType="None"
                      onfocus="return this.select();"
                      onblur="script_dmsItems.validate_WBSID(this);"
                      runat="Server" />
                    <asp:Label
                      ID="F_WBSID_Display"
                      CssClass="form-control"
                      disabled="disabled"
                      BackColor="#cccccc"
                      runat="Server" />
                    <AJX:AutoCompleteExtender
                      ID="ACEWBSID"
                      BehaviorID="B_ACEWBSID"
                      ContextKey=""
                      UseContextKey="true"
                      ServiceMethod="WBSIDCompletionList"
                      TargetControlID="F_WBSID"
                      EnableCaching="false"
                      CompletionInterval="100"
                      FirstRowSelected="true"
                      MinimumPrefixLength="1"
                      OnClientItemSelected="script_dmsItems.ACEWBSID_Selected"
                      OnClientPopulating="script_dmsItems.ACEWBSID_Populating"
                      OnClientPopulated="script_dmsItems.ACEWBSID_Populated"
                      CompletionSetCount="10"
                      CompletionListCssClass="autocomplete_completionListElement"
                      CompletionListItemCssClass="autocomplete_listItem"
                      CompletionListHighlightedItemCssClass="autocomplete_highlightedListItem"
                      runat="Server" />
                  </div>
                </div>
              </div>
              <div class='row' id="R_Approved">
                <div class='col'>
                  <asp:Label ID="L_Approved" runat="server" Text="Approved :" />
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_Approved"
                    CssClass="form-control"
                    runat="server">
                    <asp:ListItem Selected="True" Value="True" Text="YES"></asp:ListItem>
                    <asp:ListItem Value="False" Text="NO"></asp:ListItem>
                  </asp:DropDownList>
                </div>
              </div>
              <div class='row' id="R_ActionRemarks">
                <div class='col'>
                  <asp:Label ID="L_ActionRemarks" runat="server" Text="Remarks :" />
                </div>
                <div class='col'>
                  <asp:TextBox ID="F_ActionRemarks"
                    CssClass="form-control"
                    onfocus="return this.select();"
                    onblur="this.value=this.value.replace(/\'/g,'');"
                    MaxLength="250"
                    runat="server" />
                </div>
              </div>

            </div>
            <%--End of Modal Body--%>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-warning" onclick="return dms_cmMenu.submited('1');" id="cmdPublishOnSave" style="display: none;">Save & Publish</button>
            <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
            <button type="button" class="btn btn-success" onclick="return dms_cmMenu.submited();">Submit</button>
          </div>
        </div>
      </div>
    </div>
  </div>
  <%--File Upload Dialog--%>
  <asp:TextBox ID="dmsTarget" runat="server" Style="display: none;"></asp:TextBox>
  <div class="container-fluid">
    <div class="modal fade" id="myFileModal">
      <div class="modal-dialog modal-lg h-100 d-flex flex-column justify-content-center my-0 modal-dialog-centered modal-dialog-scrollable">
        <div class="modal-content">
          <div class="modal-header">
            <h4 class="modal-title">Upload File</h4>
            <button type="button" class="close" data-dismiss="modal">&times;</button>
          </div>
          <div class="modal-body">
            <div class="custom-file">
              <input type="file" class="custom-file-input" id="f_Uploads" name="f_Uploads[]" multiple="multiple" onchange="return dms_cmMenu.filesSelected(event);">
              <label class="custom-file-label" for="f_Uploads">Choose file</label>
            </div>
            <div class='container-fluid' id='UploadFileList'></div>
            <div class="progress" style="display: none; height: 20px;">
              <div id="FileProgress" class="progress-bar progress-bar-striped progress-bar-animated bg-info" style="width: 0%">0 %</div>
            </div>
            <hr />
            <span id="lblMessage" style="color: Green"></span>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-danger" data-dismiss="modal">Cancle</button>
            <button type="button" class="btn btn-success" onclick="return dms_cmMenu.startUpload();">Upload</button>
          </div>
        </div>
      </div>
    </div>
  </div>
  <%--File Download Dialog--%>
  <div class="container-fluid">
    <div class="modal fade" id="myFileDownload">
      <div class="modal-dialog modal-lg h-100 d-flex flex-column justify-content-center my-0 modal-dialog-centered modal-dialog-scrollable">
        <div class="modal-content">
          <div class="modal-header">
            <h4 class="modal-title">Downloading. . .</h4>
            <button type="button" class="close" data-dismiss="modal">&times;</button>
          </div>
          <div class="modal-body">
            <div class="progress" style="height: 20px;">
              <div id="DownloadFileProgress" class="progress-bar progress-bar-striped progress-bar-animated bg-danger" style="width: 100%"></div>
            </div>
            <hr />
            <span id="downloadingMessage" style="color: Green"></span>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-danger" data-dismiss="modal">Cancle</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>

