<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="dmsMain.aspx.vb" Inherits="dmsMain" title="Home" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" ClientIDMode="Static" runat="Server">
  <script src="App_Scripts/splitMaster/src/split.min.js"></script>
  <script src="App_Scripts/resize/resize.js"></script>
  <script src="App_Scripts/dmsMain.js"></script>
  <script src="App_Scripts/dmsItems.js"></script>
  <script src="App_Scripts/filedownload/jquery.fileDownload.js"></script>
  <script src="App_Scripts/Search.js"></script>
  <link href="App_Scripts/menu/lgMenu.css" rel="stylesheet" />
  <style>

  .dms-page-header{
    display:flex;
    flex-direction: row;
    justify-content:space-between;
    background:linear-gradient(#efeded, #f1eeee, #f1eeee, #f1eeee, #f1eeee, #f1eeee, #f1eeee, #f1eeee, #bab9b9);
    border:1pt solid #bab9b9;
  }
  .dms-page-menu{
    display:flex;
    flex-direction:row;
    justify-content:flex-start;
  }
  .dms-page-menu div{
    margin:1px 0px 1px 0px;
    font-size: 11px;
  }
  .dms-menu-head:hover{
    background:linear-gradient(#C0C0C0 5%, #cbd0e0 90%, #C0C0C0 5%);
  }
  .dms-page-search{
    flex-direction:row;
    justify-content: flex-end;
  }
  .dms-page-search div{
    font-size:11px;
  }
    .dms-tab-search {
      border: 1px solid gray;
      padding:4px;
      min-width: 60px;
      cursor: pointer;
      background:linear-gradient(#C0C0C0 5%, #f1eeee 90%, #C0C0C0 5%);
    }
      .dms-tab-search:hover {
        background:linear-gradient(#C0C0C0 5%, #cbd0e0 90%, #C0C0C0 5%);
      }

  .dms-page-footer{
    margin-bottom:0px!important;
    display:flex;
    flex-direction: row;
    justify-content:space-between;
    background:linear-gradient(#bab9b9 5%, #f1eeee 90%, #bab9b9 5%);
    border:1pt solid #bab9b9;
  }
</style>

  <style>
    /*Splitter*/
    .split {-webkit-box-sizing: border-box;-moz-box-sizing: border-box;box-sizing: border-box;overflow-y: auto;overflow-x: auto;}
    .content {border: 1px solid #C0C0C0;box-shadow: inset 0 1px 2px #e4e4e4;}
    .gutter {background-color: lavender;background-repeat: no-repeat;background-position: 50%;}
    .gutter.gutter-horizontal {cursor: col-resize;background-image: url('App_Scripts/splitMaster/grips/vertical.png');}
    .gutter.gutter-vertical {cursor: row-resize;background-image: url('App_Scripts/splitMaster/grips/horizontal.png');}
    .split.split-horizontal,.gutter.gutter-horizontal {height: 100%;float: left;}

    /*dms Tab*/
    .dms-tabs {
      display:flex;
      flex-direction: row;
      background-color:#f1eeee;
      border:1pt solid #bab9b9;
      font-size: 11px;
    }

    .dms-tab-head {
      border: 1px solid gray;
      padding:4px;
      min-width: 80px;
      cursor: pointer;
      background:linear-gradient(#C0C0C0 5%, #f1eeee 90%, #C0C0C0 5%);
    }

      .dms-tab-head:hover {
        background:linear-gradient(#C0C0C0 5%, #cbd0e0 90%, #C0C0C0 5%);
      }

    .dms-menu-head {
      border: 1px solid #f2f1f1;
      padding: 4px 6px 2px 2px;
      text-align:center;
      min-width: 60px;
      cursor: pointer;
    }
    

    .dms-tab-selected {
      font-weight: bold;
    }
    .dms-tab-property{
      display:flex;
      flex-direction:row;
    }
    .dms-tab-property:hover{
      background-image:linear-gradient(#faeeac,#faf5dc,#faeeac);
    }
    /*dms Tree*/
    #tblTree {
      width: 100%;
    }

    .dms-table {
      width: 100%;
      position: relative;
    }

      .dms-table th {
        font-family: Tahoma;
        font-size: 11px;
        /*background-color:#f1eeee;*/
        background:linear-gradient(#bab9b9 5%, #f1eeee 90%, #bab9b9 5%);
        border:1pt solid #bab9b9;
        position: -webkit-sticky;
        position: sticky;
        top: 0;
        height:24px;
        vertical-align:middle;
      }

    .dms-selected {
      background-color: antiquewhite;
    }

    .dms-row {
      font-family: Tahoma;
      font-size: 11px;
      cursor: pointer;
    }

      .dms-row:hover {
        background-color: #ffd800;
        color: #0026ff;
      }

    .dms-icon {
      font-size: 20px;
    }

    .dms-tree-icon {
      font-size: 18px;
    }
    /*dms context menu*/
    .dms-menu-table {
      z-index: 1000;
      position: absolute;
      box-shadow: 10px 10px 10px #a8a7a7;
      border: 2px solid #000000;
      display: none;
      /*border-radius: 10px;*/
    }

    .dms-menu-row {
      cursor: pointer;
    }

      .dms-menu-row:hover {
        color: red;
      }

      .dms-menu-text {
        min-width: 200px !important;
        background-color:#faf7f7;
        padding: 8px;
      }
      .dms-menu-text:hover {
        background-color:#edecec;
      }
      .dms-menu-icon {
        min-width: 30px !important;
        background-color:#f1eeee;
        border-right:black;
        padding: 8px;
      }
      .dms-disabled{
        color:burlywood;
      }

    /*keygen Word*/
    .dms-word-container {
      display: flex;
      flex-wrap: wrap;
      background-color: aquamarine;
      border: 1pt solid #7d8ff7;
    }

    .dms-word {
      display: flex;
      flex-direction: row;
      background-color: #e8e3e3;
      border: 1pt solid #9b9898;
      margin: 2px;
      padding: 2px;
      border-radius: 5px;
    }

    .dms-word-remove {
      padding-left: 6px;
      color: #9b9898;
      cursor: pointer;
      vertical-align: top;
    }

      .dms-word-remove:hover {
        color: orangered;
      }

    .dms-popup-container {
      display: none;
      flex-direction: column;
      background-color: #eed54d;
      border: 1pt solid #cbad06;
      position: absolute;
      z-index: 10005;
    }

    .dms-popup-value {
      background-color: #fbe778;
      border: 1pt solid #ffd800;
      margin: 1pt;
      cursor: pointer;
    }

      .dms-popup-value:hover {
        background-color: #fff5c0;
      }
.treeRow[data-state='none']::after {
}
.modal-my {
  overflow-y: scroll;
  height: 350px;
}
/*DMS Menu*/
.dms-menu-show {
  z-index: 1000;
  position: absolute;
  background-color: white;
  padding: 2px;
  display: block;
  margin: 0;
}

.dms-menu-hide {
  display: none;
}

  </style>
  <%--Header Section--%>
  <div class="dms-page-header" style="min-height:28px!important">
    <div style="font-size:18px; color:#dd163e;">
      <strong>ISGEC</strong> -<strong>D</strong>ocument <strong>M</strong>anagement <strong>S</strong>ystem
    </div>
    <div>
      <div class="dropdown">
        <div class="dropdown-toggle dms-menu-head" data-toggle="dropdown">
          <i class="fas fa-power-off" style="font-size:18px;color:#dd163e;"></i>
        </div>
        <div class="dropdown-menu dropdown-menu-right shadow">
          <a class="dropdown-item" href="~/ChangePassword.aspx" runat="server">Change Password</a>
          <a href="#">
            <asp:LoginStatus CssClass="dropdown-item" runat="server" LoginText="Sign In" LogoutAction="Redirect" LogoutPageUrl="~/Login.aspx" LogoutText="Sign Out" />
          </a>
        </div>
      </div>

    </div>
  </div>
  <div class="dms-page-header">
    <div class="dms-page-menu">
      <div class="dropdown">
        <div class="dropdown-toggle dms-menu-head" data-toggle="dropdown">
          File
        </div>
        <div class="dropdown-menu shadow">
          <a class="dropdown-item" href="~/ChangePassword.aspx" runat="server">Change Password</a>
          <a href="#">
            <asp:LoginStatus CssClass="dropdown-item" runat="server" LoginText="Sign In" LogoutAction="Redirect" LogoutPageUrl="~/Login.aspx" LogoutText="Sign Out" />
          </a>
        </div>
      </div>
      <div class="dropdown">
        <div class="dropdown-toggle dms-menu-head" data-toggle="dropdown">
          View
        </div>
        <div class="dropdown-menu shadow" >
          <a class="dropdown-item" href="#" runat="server">History</a>
          <a class="dropdown-item" href="#" runat="server">Property</a>
        </div>
      </div>
    </div>
    <div class="dms-page-search">
      <div class="input-group input-group-sm">
        <div class="input-group-prepend">
          <div class="input-group-text">
            <input  type="checkbox" disabled="disabled" onclick="dmsScript.exitSearchMode(this);" id="chkSearch">
          </div>
        </div>
        <input type="text" class="dms-tab-search"  placeholder="search..." id="txtSearch" onkeyup="return dmsScript.textEntered(this);" />
        <div class="input-group-append">
          <button class="dms-tab-search" value="Start" disabled="disabled" id="cmdStartSearch" onclick="dmsScript.startSearch(this);"><i id="icmdStartSearch" class="fas fa-cog"></i>&nbsp;Start</button>
          <button class="dms-tab-search" value="Stop" disabled="disabled" id="cmdStopSearch" onclick="dmsScript.stopSearch(this);" >&nbsp;Stop</button>
        </div>
      </div> 
    </div>
  </div> 
  <%--Body Section--%>
  <div id="splitSection" style="position: fixed; height: 90%; width: 100%; top: auto; left: 0; bottom: auto;">
    <div id="dmsRoot" runat="server" class="split content split-horizontal">
      <table id="tblTree" runat="server">
        <tr id="trRoot">
          <td style="width:100%;"><input id="startImg" type="image" alt="img" src="TreeImgs/Loading.gif" /></td>
        </tr>
      </table>
    </div>
    <div id="dmsGrid" class="split content split-horizontal">
      <div id="a" runat="server" class="split content" style="overflow: hidden;">
        <%--Grid View--%>
        <div style="height: 95%; overflow-y: scroll;width:100%;">
          <table class="dms-table">
            <thead>
              <tr>
                <th style="width:5%">ID</th>
                <th style="width:4%">
                  <input type="checkbox" id="checkAll"/>
                </th>
                <th style="width:6%">Type</th>
                <th style="width:45%">Description</th>
                <th style="width:5%">REV</th>
                <th style="width:10%">Status</th>
                <th style="width:10%">DATE</th>
              </tr>
            </thead>
            <tbody id="gridBody">
            </tbody>
          </table>
        </div>
        <%--End Grid View--%>
      </div>
      <div id="c" class="split content" style="overflow:unset;">
          <div class="dms-tabs">
            <div id="dmsProperty" class="dms-tab-head" onclick="dmsScript.showDmsTab(this);">Property</div>
            <div id="dmsHistory" class="dms-tab-head dms-tab-selected"  onclick="dmsScript.showDmsTab(this);">History</div>
            <div id="dmsMisc" class="dms-tab-head" onclick="dmsScript.showDmsTab(this);">Misc</div>
          </div>
          <div style="height:87%; width: 100%;border:1pt solid black;">
            <div id="tabHistory" class="dms-tab-body" style="height: 100%; width: 100%;display:block;">
              <%--History View--%>
              <div style="height: 100%; overflow: scroll; width: 100%;">
                <table class="dms-table">
                  <thead>
                    <tr class="green">
                      <th style="width: 5%">SN</th>
                      <th style="width: 6%">Type</th>
                      <th style="width: 30%">Description</th>
                      <th style="width: 5%">REV</th>
                      <th style="width: 10%">Status</th>
                      <th style="width: 20%">Remarks</th>
                      <th style="width: 10%">DATE</th>
                      <th style="width: 10%">User</th>
                    </tr>
                  </thead>
                  <tbody id="histBody">
                  </tbody>
                </table>
              </div>
              <%--End History View--%>
            </div>
            <div id="tabProperty" class="dms-tab-body" style="height:100%;width:100%;font-size:11px;display:none;padding:4px;overflow-y:scroll;overflow-x:hidden;">
            </div>
            <div id="tabMisc" class="dms-tab-body" style="height: 100%; width: 100%;display:none;">
            </div>
          </div>
      </div>
    </div>
  </div>
  <%--Footer Bar--%>
  <div class="statusbar dms-page-footer">
    <div class="btn-group-sm">
      <span id="SelectedNode" class="btn btn-primary"></span>
      <span id="SelectedOption" class="btn btn-warning"></span>
      <span id="ReloadingNode" class="btn btn-danger"></span>
    </div>
  </div>
  <%--Script to Create Split View--%>
  <script>
    Split(['#a', '#c'], {
      sizes: [55, 45],
      gutterSize: 10,
      cursor: 'pointer',
      direction: 'vertical',
    });
    Split(['#dmsRoot', '#dmsGrid'], {
      sizes: [30, 70],
      gutterSize: 10,
      cursor: 'row-resize',
    });
    //Split(['#p', '#q'], {
    //  sizes: [60, 40],
    //  gutterSize: 10,
    //  cursor: 'row-resize',
    //});
  </script>

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
            <div style="display:flex;flex-direction:column;min-height:800px;">
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
              <div class='row' id="R_EMailID">
                <div class='col'>
                  <asp:Label ID="L_EMailID" runat="server" Text="E Mail ID :" />
                </div>
                <div class='col'>
                  <asp:TextBox
                    ID="F_EMailID"
                    CssClass="form-control"
                    onfocus="return this.select();"
                    onblur="this.value=this.value.replace(/\'/g,'');"
                    MaxLength="250"
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
              <div class='row' id="R_UserGroupID">
                <div class='col'>
                  <asp:Label ID="L_UserGroupID" runat="server" Text="User Group :" /><span style="color: red">*</span>
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_UserGroupID"
                    DataSourceID="ODS_UserGroupID"
                    DataTextField="Description"
                    DataValueField="ItemID"
                    AppendDataBoundItems="true"
                    CssClass="form-control"
                    Required="required"
                    runat="server" />
                  <asp:ObjectDataSource
                    ID="ODS_UserGroupID"
                    TypeName="SIS.DMS.UI+apiItem"
                    SelectMethod="apiItemSelectList"
                    runat="server">
                      <SelectParameters>
                        <asp:Parameter Name="ItemTypeID" DefaultValue="6" Type="String" Direction="Input" />
                      </SelectParameters>
                  </asp:ObjectDataSource>
                </div>
              </div>
              <div class='row' id="R_WorkflowID">
                <div class='col'>
                  <asp:Label ID="L_WorkflowID" runat="server" Text="Workflow :" /><span style="color: red">*</span>
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_WorkflowID"
                    DataSourceID="ODS_WorkflowID"
                    DataTextField="Description"
                    DataValueField="ItemID"
                    AppendDataBoundItems="true"
                    CssClass="form-control"
                    Required="required"
                    runat="server" />
                  <asp:ObjectDataSource
                    ID="ODS_WorkflowID"
                    TypeName="SIS.DMS.UI+apiItem"
                    SelectMethod="apiItemSelectList"
                    runat="server">
                      <SelectParameters>
                        <asp:Parameter Name="ItemTypeID" DefaultValue="10" Type="String" Direction="Input" />
                      </SelectParameters>
                  </asp:ObjectDataSource>
                </div>
              </div>

              <div class='row' id="R_KeyWords">
                <div class='col'>
                  <asp:Label ID="L_KeyWords" runat="server" Text="Key Words :" />
                </div>
                <div class='col'>
                  <div id="divWords" class="dms-word-container"></div>
                  <input id="dmsKeyWordInput" type="text" class="form-control" onkeydown="dmsScript.dmsKey(event);" />
                  <div class="dms-popup-container"></div>
                  <asp:TextBox ID="F_KeyWords"
                    style="display:none;"
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
              <div class='row' id="R_strIsAdmin">
                <div class='col'>
                  <asp:Label ID="L_strIsAdmin" runat="server" Text="Is Admin :" />
                </div>
                <div class='col'>
                  <asp:DropDownList
                    ID="F_strIsAdmin"
                    CssClass="form-control"
                    runat="server">
                    <asp:ListItem Selected="True" Value="True" Text="YES"></asp:ListItem>
                    <asp:ListItem Value="False" Text="NO"></asp:ListItem>
                  </asp:DropDownList>
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
            <button type="button" class="btn btn-warning" onclick="return dmsScript.frmSubmit('1');" id="cmdPublishOnSave" style="display: none;">Save & Publish</button>
            <button type="button" class="btn btn-danger" data-dismiss="modal">Cancel</button>
            <button type="button" class="btn btn-success" onclick="return dmsScript.frmSubmit();">Submit</button>
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
            <div class="container-fluid">
              <div class="row">
                <div class="col-sm-11">
                  <h4 class="modal-title">Upload File</h4>
                </div>
                <div class="col-sm-1">
                  <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
              </div>
              <div class="row" style="margin-top: 5px;">
                <div class="col-sm-12">
                  <div class="custom-file">
                    <input type="file" class="custom-file-input" id="f_Uploads" name="f_Uploads[]" multiple="multiple" onchange="return dmsScript.filesSelected(event);">
                    <label class="custom-file-label" for="f_Uploads">Choose file</label>
                  </div>
                </div>
              </div>
              <div class="row btn-info" style="margin-top: 5px;">
                <div class="col-sm-1 text-center"><b>S.N.</b></div>
                <div class="col-sm-6"><strong>File Name</strong></div>
                <div class="col-sm-2">Type</div>
                <div class="col-sm-2">Size</div>
                <div class="col-sm-1">Status</div>
              </div>
            </div>
          </div>
          <div class="modal-body">
            <div class='container-fluid' id='UploadFileList'></div>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-warning" data-dismiss="modal">Close</button>
            <button type="button" class="btn btn-primary" onclick="return dmsScript.startUpload();">Upload</button>
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
  <%--Context Menu--%>
  <table id="dmsContextMenu" class="dms-menu-table">
  </table>

  <div class="modal fade" id="dmsWinLoad">
    <div class="modal-dialog modal-sm h-100 d-flex flex-column justify-content-center my-0 modal-dialog-centered modal-dialog-scrollable">
      <div class="modal-content">
        <div class="modal-body text-center">
          <input type="image" alt="Loading" src="loading.gif" style="height:64px;width:64px;" /> 
        </div>
      </div>
    </div>
  </div>
  <div id="dmsAlert" style="display:none;padding:20px;border:1pt solid black; background-color:lightgray; border-radius:30px; box-shadow:10px 10px 10px 10px darkgray;color:black; font-weight:bold;font-size:14px;">

  </div>
  <script>
    function DmsNode(t) {
      this.base = t.dataset.base;
      this.expended = parseInt(t.dataset.expended);
      this.indent = parseInt(t.dataset.indent);
      this.childs = parseInt(t.dataset.childs);
      this.loaded = parseInt(t.dataset.loaded);
      this.item = parseInt(t.dataset.item);
      this.type = parseInt(t.dataset.type);
      this.target = t.id;
      var x = t.id.split('_');
      var y = x.pop();
      this.parent = x.join('_');
      //this.data = JSON.parse(t.dataset.data);
    }
    function SelectedMenuOption(t) {
      this.source = t.dataset.source;
      this.options = t.dataset.options;
      this.publish = false;
    }

    var dms_Fields = [];
    dms_Fields[0] = 'ItemID';
    dms_Fields[1] = 'FullDescription';
    dms_Fields[2] = 'UserID';
    dms_Fields[3] = 'ItemTypeID';
    dms_Fields[4] = 'ConvertedStatusID';
    dms_Fields[5] = 'CompanyID';
    dms_Fields[6] = 'DivisionID';
    dms_Fields[7] = 'DepartmentID';
    dms_Fields[8] = 'ProjectID';
    dms_Fields[9] = 'WBSID';
    dms_Fields[10] = 'KeyWords';
    dms_Fields[11] = 'CreateFolder';
    dms_Fields[12] = 'RenameFolder';
    dms_Fields[13] = 'DeleteFolder';
    dms_Fields[14] = 'CreateFile';
    dms_Fields[15] = 'DeleteFile';
    dms_Fields[16] = 'GrantAuthorization';
    dms_Fields[17] = 'BrowseList';
    dms_Fields[18] = 'ShowInList';
    dms_Fields[19] = 'Publish';
    dms_Fields[20] = 'Description';
    dms_Fields[21] = 'Approved';
    dms_Fields[22] = 'ActionRemarks';
    dms_Fields[23] = 'UserGroupID';
    dms_Fields[24] = 'WorkflowID';
    dms_Fields[25] = 'strIsAdmin'
    dms_Fields[26] = 'EMailID'

    var dms_menuOption = [];
    dms_menuOption[0] = 'Create';
    dms_menuOption[1] = 'Edit';
    dms_menuOption[2] = 'Delete';
    dms_menuOption[3] = 'Authorize to User';
    dms_menuOption[4] = 'Authorize to User Group';
    dms_menuOption[5] = 'Upload Files';
    dms_menuOption[6] = 'Download Files';
    dms_menuOption[7] = 'Add Folder to Group';
    dms_menuOption[8] = 'Upload Ref. Files';
    dms_menuOption[9] = 'Edit and Publish';
    dms_menuOption[10] = '#Check-Out';
    dms_menuOption[11] = '#Check-In';
    dms_menuOption[12] = 'Download';
    dms_menuOption[13] = 'Publish';
    dms_menuOption[14] = '#Revise';
    dms_menuOption[15] = 'Add File to Group';
    dms_menuOption[16] = 'Add User to Group';
    dms_menuOption[17] = 'Approve / Acknowledge';
    dms_menuOption[18] = '#Attach Tag';
    dms_menuOption[19] = 'Edit User of Group'; //Not Used, Not Required
    dms_menuOption[20] = 'Delete User from Group';
    dms_menuOption[21] = 'Delete Authorization';
    dms_menuOption[22] = 'Attach Workflow';
    dms_menuOption[23] = 'Remove Workflow';
    dms_menuOption[24] = 'Remove';
    dms_menuOption[25] = 'Remove All';
    dms_menuOption[26] = 'Extract BOM';

    //Context Menu Options at ItemType
    //dms_type_menu[ItemType]=[dms_menuOption[<<Number>>],dms_menuOption[<<Number>>],....];
    var dms_type_menu = [];
    dms_type_menu[1] = [0,1,2,3,4,5,6,18,22,23];
    dms_type_menu[2] = [0,1,2,3,4,7,18];
    dms_type_menu[3] = [8,9,2,10,11,12,13,14,18,26];
    dms_type_menu[4] = [0,1,2,15,18,26];
    dms_type_menu[5] = [1,2,18];
    dms_type_menu[6] = [0,1,2,16,18];
    dms_type_menu[7] = [0,1,2];
    dms_type_menu[8] = [0, 1, 2];
    dms_type_menu[9] = [0, 1, 2];
    dms_type_menu[10] = [0, 1, 2];
    dms_type_menu[11] = [21];
    dms_type_menu[12] = [0, 1, 2];
    dms_type_menu[13] = [17,6];
    dms_type_menu[14] = [0, 1, 2];
    dms_type_menu[15] = [0, 1, 2];
    dms_type_menu[16] = [0, 1, 2];
    dms_type_menu[17] = [20];
    dms_type_menu[18] = [25,12];
    dms_type_menu[19] = [25];
    dms_type_menu[20] = [24];

    var dms_hist_menu = [];
    dms_hist_menu[3] = [12];

    function DmsFormValues(t) {
      $get('F_KeyWords').value = dmsScript.getKeyWords($('.dms-word-value'));
      for (i = 0; i < t.length; i++) {
        this[dms_Fields[t[i]]]=$get('F_'+dms_Fields[t[i]]).value;
      }
    }
  //==================
    var dmsScript = {
      //==================
      site: '/WebDMS2/',
      tbl: '',
      grid: '',
      hist: '',
      selected_any:'',
      get selectedAny(){
        return this.selected_any;
      },
      set selectedAny(x){
        this.selected_any = x;
        this.readProperty();
      },
      selected_tree: '',
      get selectedTree() {
        return this.selected_tree;
      },
      set selectedTree(x){
        this.selected_tree = x;
        this.selectedAny = x;
      },
      selected_grid:'',
      get selectedGrid() {
        return this.selected_grid;
      },
      set selectedGrid(x){
        this.selected_grid = x;
        this.selectedAny = x;
      },
      selected_hist:'',
      get selectedHist(){
        return this.selected_hist;
      },
      set selectedHist(x){
        this.selected_hist = x;
        this.selectedAny = x;
      },
      loadingImg: '',
      lastSrc:'',
      loading: false,
      url:function(){
        return this.site + 'App_Services/dmsService.asmx/';
      },
      whenFailed: '',
      treeSelectionChanged: false,
      gridSelectionChanged: false,
      histSelectionChanged: false,
      readProperty: function () {
        if (this.selectedAny == '') return;
        var x = new DmsNode(this.selectedAny);
        var y = JSON.parse(this.selectedAny.dataset.data);
        var yp = Object.keys(y);
        var z = '';
        var zp = ''
        if (typeof (this.selectedAny.dataset.property) != 'undefined') {
          z = JSON.parse(this.selectedAny.dataset.property);
          zp = Object.keys(z);
        }
        var output = [];
        for (var i = 0; i < zp.length; i++) {
          output.push(
                     '<div class="dms-tab-property">',
                       '<div style="width:35%;"><b>' + zp[i] + '</b></div>',
                       '<div style="width:15%;"><b>:</b></div>',
                       '<div style="width:50%;">', z[zp[i]], '</div>',
                     '</div>'
                     )
        }
        for (var i = 0; i < yp.length; i++) {
          var val = '';
          if (typeof (dmsFieldType[yp[i]]) != 'undefined') {
            switch (dmsFieldType[yp[i]]){
              case 'dmsType':
                val = dmsType(y[yp[i]]);
                break;
              case 'dmsQuality':
                val = dmsQuality(y[yp[i]]);
                break;
              case 'dmsState':
                val = dmsState(y[yp[i]]);
                break;
            }
          } else {
            val = y[yp[i]];
          }

          output.push(
                     '<div class="dms-tab-property">',
                       '<div style="width:35%;"><b>' + yp[i] + '</b></div>',
                       '<div style="width:15%;"><b>:</b></div>',
                       '<div style="width:50%;">', val, '</div>',
                     '</div>'
                     )
        }
        output.push(
          '<div class="dms-tab-property">',
            '<div style="width:35%;"><b>ID</b></div>',
            '<div style="width:15%;"><b>:</b></div>',
            '<div style="width:50%;">', x.target, '</div>',
          '</div>',
          '<div class="dms-tab-property">',
            '<div style="width:35%;"><b>Base</b></div>',
            '<div style="width:15%;"><b>:</b></div>',
            '<div style="width:50%;">', x.base, '</div>',
          '</div>',
          '<div class="dms-tab-property">',
            '<div style="width:35%;"><b>Childs</b></div>',
            '<div style="width:15%;"><b>:</b></div>',
            '<div style="width:50%;">', x.childs, '</div>',
          '</div>',
          '<div class="dms-tab-property">',
            '<div style="width:35%;"><b>Type</b></div>',
            '<div style="width:15%;"><b>:</b></div>',
            '<div style="width:50%;">', dmsType(x.type), '</div>',
          '</div>',
          '<div class="dms-tab-property">',
            '<div style="width:35%;"><b>Item</b></div>',
            '<div style="width:15%;"><b>:</b></div>',
            '<div style="width:50%;">', x.item, '</div>',
          '</div>',
          '<div class="dms-tab-property">',
            '<div style="width:35%;"><b>Parent</b></div>',
            '<div style="width:15%;"><b>:</b></div>',
            '<div style="width:50%;">', x.parent, '</div>',
          '</div>',
          '<div class="dms-tab-property">',
            '<div style="width:35%;"><b>Status</b></div>',
            '<div style="width:15%;"><b>:</b></div>',
            '<div style="width:50%;">', y.StatusID, '</div>',
          '</div>',
          '<div class="dms-tab-property">',
            '<div style="width:35%;"><b>WF-Status</b></div>',
            '<div style="width:15%;"><b>:</b></div>',
            '<div style="width:50%;">', y.ConvertedStatusID, '</div>',
          '</div>',
          '<div class="dms-tab-property">',
            '<div style="width:35%;"><b>Forward Type</b></div>',
            '<div style="width:15%;"><b>:</b></div>',
            '<div style="width:50%;">', y.ForwardLinkedItemTypeID, '</div>',
          '</div>',
          '<div class="dms-tab-property">',
            '<div style="width:35%;"><b>Forward Item</b></div>',
            '<div style="width:15%;"><b>:</b></div>',
            '<div style="width:50%;">', y.ForwardLinkedItemID, '</div>',
          '</div>',
          '<div class="dms-tab-property">',
            '<div style="width:35%;"><b>Linked Type</b></div>',
            '<div style="width:15%;"><b>:</b></div>',
            '<div style="width:50%;">', y.LinkedItemTypeID, '</div>',
          '</div>',
          '<div class="dms-tab-property">',
            '<div style="width:35%;"><b>Linked Item</b></div>',
            '<div style="width:15%;"><b>:</b></div>',
            '<div style="width:50%;">', y.LinkedItemID, '</div>',
          '</div>'
          );
        $get('tabProperty').innerHTML = output.join('');

      },
      showDmsTab: function (t) {
        $('.dms-tab-body').hide();
        $('.dms-tab-head').removeClass('dms-tab-selected');
        $get(t.id.replace('dms', 'tab')).style.display = 'block';
        t.classList.add('dms-tab-selected');
      },
      resetLoading:function(){
        this.loadingImg.src = this.lastSrc;
        this.loading = false;
      },
      failed: function (z) {
        this.loading = false;
        eval(this.whenFailed);
        if (z != '') {
          //alert(z);
          $get('dmsAlert').innerHTML = z;
          $('#dmsAlert').center();
          $("#dmsAlert").show().delay(3000).fadeOut();
        }

      },
      showAlert: function (z) {
        if (z != '') {
          $get('dmsAlert').innerHTML = z;
          $('#dmsAlert').center();
          $("#dmsAlert").show().delay(3000).fadeOut();
          //$("#myElem").fadeIn('slow').animate({opacity: 1.0}, 1500).effect("pulsate", { times: 2 }, 800).fadeOut('slow');
        }
      },
      started: function (z) {
        $("#dmsWinLoad").modal('hide');
        //================
        $("#dmsWinLoad").removeClass("in");
        $(".modal-backdrop").remove();
        $('body').removeClass('modal-open');
        $('body').css('padding-right', '');
        $("#dmsWinLoad").hide();
        //================
        var tbl = this.tbl;
        var y = $get(z.Target);
        var itm = new DmsNode(y);
        if (itm.loaded == 1) {
          for (var i = tbl.rows.length - 1; i > 0; i--) {
            var row = tbl.rows[i];
            var rowItm = new DmsNode(row);
            if (rowItm.parent == itm.target) {
              tbl.deleteRow(i);
              if (rowItm.childs > 0 && rowItm.loaded > 0)
                this.delete_row(rowItm, tbl);
            }
          }
        }

        this.loading = false;
        if (z.strHTML.length > 0) {
          var y = $get(z.Target);
          y.setAttribute('data-loaded', '1');
          for (i = z.strHTML.length - 1; i > -1; i--) {
            y.insertAdjacentHTML('afterend', z.strHTML[i]);
          }
          this.toggleTree(y, 0);
        }
      },
      start: function () {
        //Add Function To JQuery
        jQuery.fn.center = function () {
          this.css("position", "absolute");
          this.css("top", Math.max(0, (($(window).height() - $(this).outerHeight()) / 2) +
                                                      $(window).scrollTop()) + "px");
          this.css("left", Math.max(0, (($(window).width() - $(this).outerWidth()) / 2) +
                                                      $(window).scrollLeft()) + "px");
          return this;
        }
        //=================
        this.tbl = $get('tblTree');
        this.grid = $get('gridBody');
        this.hist = $get('histBody');
        var that = this;
        this.whenFailed = '';
        $.ajax({
          type: 'POST',
          url: that.url() + 'LoadRoot',
          context: that,
          dataType: 'json',
          cache: false,
          data: "{context:'{\"Target\":\"trRoot\"}'}",
          contentType: "application/json; charset=utf-8"
        }).done(function (data,status,xhr) {
          var y = JSON.parse(data.d);
          if (y.err) {
            this.failed(y.msg)
          } else {
            $get('startImg').style.display = 'none';
            this.started(y);
          }
        }).fail(function (xhr,status,err) {
          this.failed(err);
        });

      },
      //treeRowClicked
      tRC: function (t, e) {
        if (t != this.selectedTree){
          this.treeSelectionChanged = true;
          try { this.selectedTree.classList.remove('dms-selected'); } catch (e) { }
          this.selectedTree = t;
          this.selectedTree.classList.add('dms-selected');
          this.loadHistory(t);
        }
        this.toggleTree(t);
      },
      gridRowClicked: function (t, e) {
        if (t != this.selectedGrid) {
          this.gridSelectionChanged = true; 
          try { this.selectedGrid.classList.remove('dms-selected'); } catch (e) { }
          this.selectedGrid = t;
          this.selectedGrid.classList.add('dms-selected');
          this.loadHistory(t);
        }
      },
      histRowClicked: function (t, e) {
        if (t != this.selectedHist) {
          this.histSelectionChanged = true;
          try { this.selectedHist.classList.remove('dms-selected'); } catch (e) { }
          this.selectedHist = t;
          this.selectedHist.classList.add('dms-selected');
        }
      },
      loadingHistory: false,
      showHist: '',
      getHiststr: function (o, id, p) {
        var str = '';
        str = str + " <tr class='dms-row' onclick='return dmsScript.histRowClicked(this,event);' oncontextmenu='dmsScript.showHistMenu(this,event);' id='" + id + '_' + o.ItemID + "' ";
        str = str + "   data-base='" + p.base + "' ";
        str = str + "   data-expended='" + p.expended + "' ";
        str = str + "   data-indent='" + p.indent + "' ";
        str = str + "   data-childs='" + p.childs + "' ";
        str = str + "   data-loaded='" + p.loaded + "' ";
        str = str + "   data-item='" + p.item + "' ";
        str = str + "   data-type='" + p.type + "' ";
        str = str + "   data-data='" + JSON.stringify(o) + "' >";
        str = str + "   <td>";
        str = str + "     <input type='checkbox' id='chkH_" + o.ItemID + "'/>";
        str = str + "   </td>";
        str = str + "   <td>"; 
        str = str + "      " + getIconHTML(o.ItemTypeID, o.Description);
        str = str + "   </td>"
        str = str + "   <td>"
        str = str + "     " + o.Description;
        str = str + "   </td>"
        str = str + "   <td>"
        str = str + "     " + o.RevisionNo;
        str = str + "   </td>"
        str = str + "   <td>"
        str = str + "     " + o.DMS_States12_Description;
        str = str + "   </td>"
        str = str + "   <td>"
        str = str + "    " + o.ActionRemarks
        str = str + "   </td>"
        str = str + "   <td>"
        str = str + "    " + o.CreatedOn   //.substring(0, 10);
        str = str + "   </td>"
        str = str + "   <td>"
        str = str + "    " + o.aspnet_Users2_UserFullName;
        str = str + "   </td>"
        str = str + " </tr>"
        return str;
      },
      displayHistory: function (y) {
        var hist = this.hist;
        var h = JSON.parse(y.strHTML[0]);
        hist.innerHTML = '';
        for (var i = 0, hi; hi = h[i]; i++) {
          var rowItm = new DmsNode(this.showHist);
          var str = this.getHiststr(hi, rowItm.target.replace('grRoot', 'hrRoot').replace('trRoot','hrRoot'), rowItm);
          hist.innerHTML +=  str;
        }
        this.gridSelectionChanged = false;
        this.loadingHistory = false;
      },
      loadHistory: function(t){
        if (this.loadingHistory)
          return;
        this.loadingHistory = true;
        this.showHist = t;
        var itm = new DmsNode(t);
        var y = JSON.stringify(itm);
        var that = this;
        var y = JSON.stringify(itm);
        $.ajax({
          type: 'POST',
          url: that.url() + 'LoadHistory',
          context: that,
          dataType: 'json',
          cache: false,
          data: "{context:'" + y + "'}",
          contentType: "application/json; charset=utf-8"
        }).done(function (data, status, xhr) {
          var y = JSON.parse(data.d);
          if (y.err) {
            this.loadingHistory = false;
            this.failed(y.msg)
          } else {
            this.displayHistory(y);
          }
        }).fail(function (xhr, status, err) {
          this.gridSelectionChanged = false;
          this.loadingHistory = false;
          this.failed(err);
        });
      },
      treeImgClicked: function (t, e) {
        if (this.loading)
          return;
        else
          this.loading = true;
        var x = $get(t.id.replace('img_', ''));
        if (x != this.selectedTree) {
          this.treeSelectionChanged = true;
          try { this.selectedTree.classList.remove('dms-selected'); } catch (e) { }
          this.selectedTree = x;
          try { this.selectedTree.classList.add('dms-selected'); } catch (e) { }
        }
        this.loadTree(x);
      },
      refreshTree:function(t){
        var x = $get('img_' + t.replace('grRoot','trRoot').replace('hrRoot','trRoot'));
        this.selectedTree = '';
        this.treeImgClicked(x);
      },
      toggleTree: function (t,o) {
        var itm = new DmsNode(t);
        if (itm.childs > 0 && itm.loaded == 0) {
          this.loadTree(t);
          return;
        }
        if (typeof (o) != 'undefined') {
          itm.expended = o;
        }
        var tbl = this.tbl;
        if (itm.expended == 0) {
          if (itm.childs > 0 || itm.type == 3) {
            for (var i = 1, row; row = tbl.rows[i]; i++) {
              var rowItm = new DmsNode(row);
              if (rowItm.parent == itm.target) {
                 this.show_row(rowItm, tbl);
              }
            }
            if (itm.childs > 0) {
              t.setAttribute('data-expended', '1');
              $get('img_' + t.id).src = this.site + 'TreeImgs/Minus.gif';
            }
          }
        }else {
          if (itm.childs > 0 || itm.type == 3) {
            for (var i = 1, row; row = tbl.rows[i]; i++) {
              var rowItm = new DmsNode(row);
              if (rowItm.parent == itm.target) {
                 this.hide_row(rowItm, tbl);
              }
            }
            if (itm.childs > 0) {
              t.setAttribute('data-expended', '0');
              $get('img_' + t.id).src = this.site + 'TreeImgs/Plus.gif';
            }
          }
        }
        this.loadGrid(t);
      },
      show_row: function (x, tbl) {
        $get(x.target).style.display = 'table-row';
        if (x.childs == 0)
          return;
        if (x.expended == 0)
          return;
        for (var i = 1, row; row = tbl.rows[i]; i++) {
          var rowItm = new DmsNode(row);
          if (rowItm.parent == x.target) {
             this.show_row(rowItm, tbl);
          }
        }
      },
      hide_row: function (x, tbl) {
        $get(x.target).style.display = 'none';
        if (x.childs == 0)
          return;
        if (x.expended == 0)
          return;
        for (var i = 1, row; row = tbl.rows[i]; i++) {
          var rowItm = new DmsNode(row);
          if (rowItm.parent == x.target) {
            this.hide_row(rowItm, tbl);
          }
        }
      },
      loadGrid: function (t) {
        if (!this.treeSelectionChanged) return;
        this.treeSelectionChanged = false;
        var itm = new DmsNode(t);
        var tbl = this.tbl;
        var grid = this.grid;
        grid.innerHTML = '';
        for (var i = 1, row; row = tbl.rows[i]; i++) {
          var rowItm = new DmsNode(row);
          if(rowItm.parent==itm.target || (itm.type==3 && rowItm.target==itm.target)){
            var itmJN = JSON.parse(row.getAttribute('data-data'));
            var str = this.getGridstr(itmJN, rowItm.target.replace('trRoot','grRoot'),rowItm);
            grid.innerHTML += str;
          }
        }
      },
      getGridstr: function (o, id, p) {
        var str = '';
        str = str + " <tr class='dms-row' onclick='return dmsScript.gridRowClicked(this,event);' oncontextmenu='dmsScript.showGridMenu(this,event);' id='" + id + "' ";
        str = str + "   data-base='" + p.base + "' ";
        str = str + "   data-expended='" + p.expended + "' ";
        str = str + "   data-indent='" + p.indent + "' ";
        str = str + "   data-childs='" + p.childs + "' ";
        str = str + "   data-loaded='" + p.loaded + "' ";
        str = str + "   data-item='" + p.item + "' ";
        str = str + "   data-type='" + p.type + "' ";
        str = str + "   data-data='" + JSON.stringify(o) + "' >";
        str = str + "   <td>" + o.ItemID + "</td>";
        str = str + "   <td>";
        str = str + "     <input type='checkbox' id='chk_" + o.ItemID + "'/>";
        str = str + "   </td>";
        str = str + "   <td>"; //o.DMS_ItemTypes8_Description;
        str = str + "      " + getIconHTML(o.ItemTypeID, o.Description);
        str = str + "   </td>"
        str = str + "   <td>"
        str = str + "     " + o.Description;
        str = str + "   </td>"
        str = str + "   <td>"
        str = str + "     " + o.RevisionNo;
        str = str + "   </td>"
        str = str + "   <td>"
        str = str + "     " + o.StatusID_Description;
        str = str + "   </td>"
        str = str + "   <td>"
        str = str + "    " + o.CreatedOn   //.substring(0, 10);
        str = str + "   </td>"
        //str = str + "   <td>"
        //str = str + "    " + id;
        //str = str + "   </td>"
        str = str + " </tr>"
        return str;
      },
      treeLoaded: function (z) {
        this.started(z);
      },
      delete_row: function (x, tbl) {
        for (var i = tbl.rows.length - 1; i > 0; i--) {
          var row = tbl.rows[i];
          var rowItm = new DmsNode(row);
          if (rowItm.parent == x.target) {
            tbl.deleteRow(i);
            if (rowItm.childs > 0 && rowItm.loaded > 0)
              this.delete_row(rowItm, tbl);
          }
        }
      },
      loadTree: function (t) {
        if (!this.loading)
          this.loading = true;
        //t=>tr object
        this.loadingImg = $get('img_' + t.id);
        this.lastSrc = this.loadingImg.src;
        this.loadingImg.src = this.site + 'TreeImgs/Loading.gif';
        var itm = new DmsNode(t);
        //Reload-tree
        this.whenFailed = this.resetLoading();
        var that = this;
        var y = JSON.stringify(itm);
        $.ajax({
          type: 'POST',
          url: that.url() + 'LoadAny',
          context: that,
          dataType: 'json',
          cache: false,
          data: "{context:'" + y + "'}",
          contentType: "application/json; charset=utf-8"
        }).done(function (data, status, xhr) {
          var y = JSON.parse(data.d);
          if (y.err) {
            this.failed(y.msg)
          } else {
            this.treeLoaded(y);
          }
        }).fail(function (xhr, status, err) {
          this.failed(err);
        });
      },
      showGridMenu: function (t, e) {
        this.gridRowClicked(t, e);
        mnuScript.showMenu(t);
      },
      //showTreeMenu
      sTM: function (t, e) {
        var x = new DmsNode(t);
        if (x.base == 'User' && x.parent == 'trRoot') {
          //No context menu
          return;
        }
        //this.treeRowClicked(t, e);
        if (t != this.selectedTree) {
          this.treeSelectionChanged = true;
          try { this.selectedTree.classList.remove('dms-selected'); } catch (e) { }
          this.selectedTree = t;
          try { this.selectedTree.classList.add('dms-selected'); } catch (e) { }
        }

        mnuScript.showMenu(t);
      },
      showHistMenu: function (t, e) {
        this.histRowClicked(t, e);
        mnuScript.showMenu(t);
      },
      frmSubmit: function (z) {
        var x = new SelectedMenuOption(this.selectedMenu);
        if (typeof (z) != 'undefined')
          x.publish = true;
        var y = JSON.stringify(x);
        var p;
        if (x.source == 'trRoot')
          p = this.selectedTree;
        if (x.source == 'grRoot')
          p = this.selectedGrid;
        if (x.source == 'hrRoot')
          p = this.selectedHist;
        var q = new DmsNode(p);
        var r = JSON.stringify(q);
        var u = new DmsFormValues(this.frmInput);
        var v = JSON.stringify(u);
        var that = this;
        $.ajax({
          type: 'POST',
          url: that.url() + 'frmSubmitted',
          context: that,
          dataType: 'json',
          cache: false,
          data: "{context:'" + r + "',options:'" + y + "',data:'" + v + "'}",
          contentType: "application/json; charset=utf-8"
        }).done(function (data, status, xhr) {
          var y = JSON.parse(data.d);
          if (y.err) {
            this.failed(y.msg)
          } else {
            if (y.Target != '') {
              this.refreshTree(y.Target);
              if (z) {
                //Publish File
                //dms_cmMenu.publishItem(mt, mo);
              }
            }
            $("#myModal").modal('hide');
          }
        }).fail(function (xhr, status, err) {
          this.failed(err);
        });
      },
      frmInput: [],
      frmCaption: '',
      cmdPublishOnSave: 'none',
      selectedMenu: '',
      CTRLUpload: '',
      ctProgress:-1,
      menuExecute: function (t) {
        //t is returned server ResponseObject
        var x = new SelectedMenuOption(this.selectedMenu);
        this.cmdPublishOnSave = 'none';
        var y;
        if (x.source == 'trRoot') 
          y = this.selectedTree;
        if (x.source == 'grRoot') 
          y = this.selectedGrid;
        if (x.source == 'hrRoot') 
          y = this.selectedHist;
        var z = new DmsNode(y);
        switch (x.options) {
          case 'Extract BOM':
            var mt = z.type + '|' + z.item;
            var fd = $("#myFileDownload");

            fd.modal('show');
            var url = 'App_Downloads/dmsExtractBOM.aspx?mt=' + mt;
            $.fileDownload(url, {
              successCallback: function (url) {
                fd.modal('hide');
              },
              failCallback: function (responseHtml, url) {
                fd.modal('hide');
              }
            });
            break;
          case 'Download Files':
          case 'Download':
            var mt = z.type + '|' + z.item;
            var fd = $("#myFileDownload");

            fd.modal('show');
            var url = 'App_Downloads/dmsDownload.aspx?mt=' + mt;
            $.fileDownload(url, {
              successCallback: function (url) {
                fd.modal('hide');
              },
              failCallback: function (responseHtml, url) {
                fd.modal('hide');
              }
            });

            break;
          case 'Upload Ref. Files':
          case 'Upload Files':
            $get('UploadFileList').innerHTML = '';
            $('#f_Uploads').value = null;
            $("#myFileModal").modal('show');
            break;
          default:
            if (t.strHTML.length>0)
              this.initFrmInput(t.strHTML[0]);
            else
              this.initFrmInput();
            break;
        } //(x.options)
      },
      menuClicked: function (t) {
        this.selectedMenu = t;
        var x = new SelectedMenuOption(t);
        var y = JSON.stringify(x);
        var p;
        if(x.source == 'trRoot') 
          p = this.selectedTree;
        if(x.source=='grRoot')
          p = this.selectedGrid;
        if(x.source == 'hrRoot')
          p = this.selectedHist;
        var q = new DmsNode(p);
        var r = JSON.stringify(q);
        var that = this;
        $.ajax({
          type: 'POST',
          url: that.url() + 'menuClicked',
          context: that,
          dataType: 'json',
          cache: false,
          data: "{context:'" + r + "',options:'" + y + "'}",
          contentType: "application/json; charset=utf-8"
        }).done(function (data, status, xhr) {
          var y = JSON.parse(data.d);
          if (y.err) {
            this.failed(y.msg)
          } else {
            if (y.nofrm) {
              if (y.Target != '') this.refreshTree(y.Target);
              if (y.okmsg != '') this.showAlert(y.okmsg);
              if (y.warnmsg != '') this.showAlert(y.warnmsg);
            } else {
              this.menuExecute(y);
            }
          }
        }).fail(function (xhr, status, err) {
          this.failed(err);
        });
      },
      initFrmInput: function (val) {
        //Hide All Fields
        var title = $('#modalTitle')[0];
        for (i = 1; i < dms_Fields.length; i++) {
          $('#R_' + dms_Fields[i]).hide();
        }
        //Show Fields Accordingly
        var x = new SelectedMenuOption(this.selectedMenu);
        var y;
        if (x.source == 'trRoot') 
          y = this.selectedTree;
        if (x.source == 'grRoot') 
          y = this.selectedGrid;
        if (x.source == 'hrRoot') 
          y = this.selectedHist;
        var z = new DmsNode(y);

        this.frmInput = dmsMatrix[z.type][x.options].frmInput;
        this.frmCaption = dmsMatrix[z.type][x.options].frmCaption;
        this.cmdPublishOnSave = dmsMatrix[z.type][x.options].cmdPublish;
        
        $get('F_ItemTypeID').value = z.type;
        $get('cmdPublishOnSave').style.display = this.cmdPublishOnSave;
        title.textContent = '[' + this.frmCaption + ']';
        for (i = 1; i < this.frmInput.length; i++) {
          $get('R_' + dms_Fields[this.frmInput[i]]).style.order = i;
          $('#R_' + dms_Fields[this.frmInput[i]]).show();
        }
        //Set Passed Value for editing
        var objVal;
        if (typeof(val) != 'undefined') {
          objVal = JSON.parse(val);
        }
        if (typeof(objVal) == 'object') {
          var aProp = Object.keys(objVal);
          for (i = 0; i < aProp.length; i++) {
            try {
              $get('F_' + aProp[i]).value = objVal[aProp[i]];
            } catch (e) { }
          }
          $get('F_UserID_Display').value = '';
          $get('F_ProjectID_Display').value = '';
          $get('F_WBSID_Display').value = '';
          this.setKeyWords();
        }
        //Editing Values Assigned
        $("#myModal").modal('show');
      },
      setKeyWords:function(){
        var fv = $get('F_KeyWords').value.split(',');
        if (fv.length > 0) {
          var dv = $get('divWords');
          dv.innerHTML='';
          for(var i=0;i<fv.length;i++){
            dv.innerHTML += this.DmsWord(fv[i]);
          }
        }
      },
      getKeyWords: function (x) {
        var y = '';
        for (i = 0;i<x.length;i++){
          if(y=='')
            y = x[i].innerText;
          else
            y = y + ',' + x[i].innerText;
        }
        return y;
      },
      getFileType: function (x) {
        if (x.indexOf('sheet') >= 0) return 'MS-Excel';
        if (x.indexOf('word') >= 0) return 'MS-Word';
        if (x.indexOf('presentation') >= 0) return 'MS-PowerPoint';
        return x || 'Other';
      },
      getFileSize: function (x) {
        var y = parseInt(x);
        if (y > 1073741824) return (y / 1073741824).toFixed(2) + ' GB';
        if (y > 1048576) return (y / 1048576).toFixed(2) + ' MB';
        if (y > 1024) return (y / 1024).toFixed(2) + ' KB';
        return y + ' Bytes';
      },
      filesSelected: function (evt) {
        this.CTRLUpload = evt.target;
        var files = evt.target.files; // FileList object
        var filedata = [];
        for (var i = 0, f; f = files[i]; i++) {
          filedata.push(new DmsSelectedFiles(f));
        }
        var s = JSON.stringify(filedata);
        //Validate FileData
        var x = new SelectedMenuOption(this.selectedMenu);
        var y = JSON.stringify(x);
        var p;
        if (x.source == 'trRoot')
          p = this.selectedTree;
        if (x.source == 'grRoot')
          p = this.selectedGrid;
        if (x.source == 'hrRoot')
          p = this.selectedHist;
        var q = new DmsNode(p);
        var r = JSON.stringify(q);
        var that = this;
        //Temp Place for next Line
        $(evt.target).siblings(".custom-file-label").addClass("selected").html('Choose File');
        $.ajax({
          type: 'POST',
          url: that.url() + 'validateFiles',
          context: that,
          dataType: 'json',
          cache: false,
          data: "{context:'" + r + "',options:'" + y + "',filedata:'" + s + "'}",
          contentType: "application/json; charset=utf-8"
        }).done(function (data, status, xhr) {
          var y = JSON.parse(data.d);
          if (y.err) {
            this.failed(y.msg)
          } else {
            var z = [];
            for (var i = 0, f; f = y.strHTML[i]; i++) {
              var x = JSON.parse(f);
              x.id = i;
              z.push(x);
            }
            this.FilesToUpload = z;
            this.filesValidated();
          }
        }).fail(function (xhr, status, err) {
          this.failed(err);
        });
      },
      getUploadFileIcon: function(x){
        if(x.err && x.cancelled)
          return '<input type="button" class="btn btn-sm btn-danger" id="uploadFile_' + x.id + '" value="NOT" />';
        if(x.warn && !x.cancelled && !x.uploaded)
            return '<input type="button" class="btn btn-sm btn-warning" id="uploadFile_' + x.id + '" onclick="dmsScript.toggleUploadFile(this);" value="YES" />';
        if(x.warn && x.cancelled && !x.uploaded)
          return '<input type="button" class="btn btn-sm btn-warning" id="uploadFile_' + x.id + '" onclick="dmsScript.toggleUploadFile(this);" value="NO" />';
        if(!x.err && !x.warn && x.cancelled && !x.uploaded)
          return '<input type="button" class="btn btn-sm btn-danger" id="uploadFile_' + x.id + '" onclick="dmsScript.toggleUploadFile(this);" value="NO" />';
        if(!x.err && !x.warn && !x.cancelled && !x.uploaded)
          return '<input type="button" class="btn btn-sm btn-success" id="uploadFile_' + x.id + '" onclick="dmsScript.toggleUploadFile(this);" value="YES" />';
        if (x.uploaded)
          return '<input type="button" class="btn btn-sm btn-info" id="uploadFile_' + x.id + '" value="DONE" />';
      },
      toggleUploadFile: function(z){
        var zId = z.id.replace('uploadFile_', '');
        this.FilesToUpload[zId].cancelled = !this.FilesToUpload[zId].cancelled;
        this.filesValidated();
      },
      FilesToUpload:'',
      filesValidated: function () {
        var z = this.FilesToUpload;
        for (var i = 0; i < z.length; i++) {
          z[i].icon = this.getUploadFileIcon(z[i]);
        }
        var output = [];
        for (var i = 0, f; f = z[i]; i++) {
          output.push(
            '<div class="row">',
              '<div class="col-sm-1 text-center"><b>', i+1, '</b></div>',
              '<div class="col-sm-6"><strong>', f.fileName, '</strong></div>',
              '<div class="col-sm-2">', f.fileType, '</div>',
              '<div class="col-sm-2">', f.fileSize, '</div>',
              '<div class="col-sm-1"><span id="ctMsg_' + i + '" style="font-size:10px;color:pink;">', f.icon, '</span></div>',
            '</div>',
            '<div class="row">',
              '<div class="col-sm-12 btn-outline-danger" style="font-size:10px;"><i>', f.msg, '</i></div>',
            '</div>',
            '<div class="row">',
              '<div class="col-sm-12">',
                '<div style="height:16px;">',
                  '<div id="ctPBar_' + i + '" class="progress-bar progress-bar-striped progress-bar-animated bg-info" style="width:0%">0 %</div>',
                '</div>',
              '</div>',
            '</div>'
            );
        }
        $get('UploadFileList').innerHTML = output.join('');
        //Original Place
        //$(evt.target).siblings(".custom-file-label").addClass("selected").html('Choose File');
      },
      startUpload: function () {
        var x = new SelectedMenuOption(this.selectedMenu);
        var y = JSON.stringify(x);
        var p;
        if (x.source == 'trRoot')
          p = this.selectedTree;
        if (x.source == 'grRoot')
          p = this.selectedGrid;
        if (x.source == 'hrRoot')
          p = this.selectedHist;
        var q = new DmsNode(p);
        var r = JSON.stringify(q);
        var that = this;
        var done = true;
        $('.progress').show();
        for (var i = 0, f; f = this.FilesToUpload[i]; i++) {
          if (!f.cancelled && !f.uploaded) {
            done = false;
            this.ctProgress = i;
            $("#ctMsg_"+i).html('');
            $("#ctPBar_"+i).width("0%");
            $("#ctPBar_"+i).text("0 %");
            var fd = new FormData();
            fd.append('file_target', r);
            fd.append('file_id', f.id);
            fd.append('file_data', this.CTRLUpload.files[i], f.fileName);
            $.ajax({
              url: that.site + 'App_Services/dmsUploader.ashx',
              context: that,
              type: 'POST',
              data: fd,
              cache: false,
              contentType: false,
              processData: false,
              success: function (data, status, xhr) {
                if (data.err) {
                  this.failed(data.msg)
                } else {
                  this.fileUploaded(data);
                }
              },
              xhr: function () {
                var fileXhr = $.ajaxSettings.xhr();
                if (fileXhr.upload) {
                  fileXhr.upload.addEventListener("progress", dmsScript.fileUploading, false);
                }
                return fileXhr;
              }
            });
            break;
          }
        } // Next For
        if (done) {
          //$(".progress").hide();
          //this.CTRLUpload.value = null;
          //this.loadTree(q.childs > 0 ? $get(q.target) : $get(q.parent));
          this.loadTree($get(q.parent.replace('grRoot','trRoot')));
        }
      },
      fileUploading:function(e){
        if (e.lengthComputable) {
          var s = parseInt((e.loaded / e.total) * 100);
          $("#ctPBar_"+dmsScript.ctProgress).width(s + "%");
          $("#ctPBar_"+dmsScript.ctProgress).text(s + " %");
        }
      },
      fileUploaded: function (y) {
        $("#ctMsg_" + this.ctProgress).html("<b>" + y.msg + "</b>");
        this.FilesToUpload[this.ctProgress].uploaded = true;
        this.startUpload();
      },
      //===============Key Word Related===================
      DmsWord: function (x) {
        var output = [];
        output.push(
          '<div class="dms-word">',
            '<div class="dms-word-value">',
               x,
            '</div>',
            '<div class="dms-word-remove" onclick="$(this).parent().fadeOut(300) && dmsScript.removeKey(this);">&times;</div>',
          '</div>'
          );
        return output.join('');
      },
      removeKey: function (x) {
        x.parentNode.parentNode.removeChild(x.parentNode);
      },
      dmsKey: function (e) {
        var y = e.char || e.key;
        if (y == ',') {
          var t = e.target;
          var z = this.DmsWord(t.value.replace(',', ''));
          $get('divWords').innerHTML += z;
          t.value = '';
          e.preventDefault();
          return false;
        }
      },
      dmsHelpNode: function (x) {
        var output = [];
        output.push(
          '<div class="dms-popup-value" onclick="dmsScript.dmsHelpNodeSelected(this);">',
               x,
          '</div>'
          );
        return output.join('');
      },
      dmsHelpNodeSelected: function (x) {
        var ib = $get('dmsKeyWordInput');
        var p = x.innerHTML.split(',');
        for (i = 0; i < p.length; i++) {
          var z = this.DmsWord(p[i]);
          $get('divWords').innerHTML += z;
        }
        ib.value = '';
        ib.focus();
      },
      dmsKeyWordHelp: function (event) {
        var e = event;
        var tgt = e.target;
        var str = tgt.value;
        var pc = $('.dms-popup-container')[0];
        if (event.char == ',') {
          return;
        }
        if (event.keyCode == 27) {
          pc.style.display = 'none';
        }
        pc.style.top = parseFloat(tgt.style.top) + parseFloat(tgt.style.height) + 25 + 'px';
        pc.style.left = parseFloat(tgt.style.left) + 'px';
        pc.style.width = (parseFloat(tgt.style.width) || tgt.clientWidth) + 'px';
        pc.style.display = 'flex';
        $(document).bind("click", function (event) {
          $('.dms-popup-container')[0].style.display = 'none';
        });
        window.event.returnValue = false;
        pc.innerHTML = '';
        var that = dmsScript;
        $.ajax({
          type: 'POST',
          url: that.url() + 'getTags',
          context: that,
          dataType: 'json',
          cache: false,
          data: "{context:'" + str + "',cnt:'" + 10 + "'}",
          contentType: "application/json; charset=utf-8"
        }).done(function (data, status, xhr) {
          var y = JSON.parse(data.d);
          if (y.err) {
            this.failed(y.msg)
          } else {
            var pc = $('.dms-popup-container')[0];
            for (var i = 0, f; f = y.strHTML[i]; i++) {
              pc.innerHTML += dmsScript.dmsHelpNode(f);
            }
          }
        }).fail(function (xhr, status, err) {
          this.failed(err);
        });
      },
      //===============
      serverStopped: false,
      stop: false,
      searchMode: false,
      fetching: false,
      searchid: '',
      timer: '',
      lastitem: 0,
      exitSearchMode: function (x) {
        $get('txtSearch').value = '';
        $get('cmdStartSearch').disabled = true;
        $get('cmdStopSearch').disabled = true;
        $get('chkSearch').disabled = true;
        $get('chkSearch').checked = false;
        this.searchMode = false;
        this.searchid = '';
        this.stop = false;
        this.fetching = false;
      },
      textEntered: function (x) {
        if (x.value.length > 0) {
          $get('cmdStartSearch').disabled = false;
        } else {
          $get('cmdStartSearch').disabled = true;
        }
      },
      startSearch: function (x) {
        $get('icmdStartSearch').classList.add('fa-spin');
        $get('txtSearch').disabled = true;
        $get('cmdStartSearch').disabled = true;
        $get('cmdStopSearch').disabled = false;
        $get('chkSearch').disabled = true;
        $get('chkSearch').checked = true;
        this.searchMode = true;
        this.stop = false;
        this.fetching = false;
        //var gb = $get('gridBody')
        //gb.innerHTML = '';
        this.initiateSearch();
      },
      stopSearch: function (x) {
        if (this.searchid == '') {
          this.stopped();
        } else {
          if (this.fetching) {
            this.stop = true;
          } else {
            this.stop = true;
            this.stopResults();
          }
        }
      },
      stopped: function () {
        this.stop = false;
        this.fetching = false;
        $get('txtSearch').disabled = false;
        $get('cmdStartSearch').disabled = false;
        $get('cmdStopSearch').disabled = true;
        $get('icmdStartSearch').classList.remove('fa-spin');
        var x = $get('icmdStartSearch').style.height;
        $get('chkSearch').disabled = false;
        if (this.searchMode) {
          $get('chkSearch').disabled = false;
        }
      },
      initiateSearch: function () {
        var x = $get('txtSearch').value;
        var searchid = this.searchid;
        var that = this;
        $.ajax({
          type: 'POST',
          url: that.url() + 'InitiateSearch',
          cache: false,
          context: that,
          dataType: 'json',
          data: "{context:'" + x + "',searchid:'" + searchid + "'}",
          contentType: 'application/json; charset=utf-8',
          success: function (ret, sStatus, xXHR) {
            var y = JSON.parse(ret.d);
            if (y.err) {
              this.stopped();
              this.failed(y.msg)
            } else {
              this.searchid = y.CurrentSearch;
              this.started(y);
              this.timer = setTimeout(this.triggerSearch, 200);
            }
          },
          error: function (xXHR, sStatus, err) {
            this.stopped();
            this.failed(err);
          },
        });
      },
      triggerSearch: function () {
        var that = dmsScript;
        that.lastitem = 0;
        that.timer = setTimeout(that.fetchResults, 1000);
        var searchid = that.searchid;
        $.ajax({
          type: 'POST',
          url: that.url() + 'TriggerSearch',
          cache: false,
          context: that,
          dataType: 'json',
          data: "{searchid:'" + searchid + "'}",
          contentType: 'application/json; charset=utf-8',
          success: function (ret, sStatus, xXHR) {
            var y = JSON.parse(ret.d);
            if (y.err) {
              this.stopped();
              this.failed(y.msg)
            } else {
              if (y.ServerStopped) {
                this.serverStopped = true;
                this.failed(y.msg);
              }
            }
          },
          error: function (xXHR, sStatus, err) {
            this.stopped();
            this.failed(err);
          },
        });
      },
      fetchResults: function () {
        var that = dmsScript;
        that.fetching = true;
        var searchid = that.searchid;
        var stop = that.stop;
        var lastitem = that.lastitem;
        $.ajax({
          type: 'POST',
          url: that.url() + 'FetchResults',
          cache: false,
          context: that,
          dataType: 'json',
          data: "{searchid:'" + searchid + "',stopit:" + stop + ",lastitem:'" + lastitem + "'}",
          contentType: 'application/json; charset=utf-8',
          success: function (ret, sStatus, xXHR) {
            var y = JSON.parse(ret.d);
            if (y.err) {
              this.stopped();
              this.failed(y.msg)
            } else {
              if (y.strHTML.length > 0) {
                this.lastitem = y.LastItem;
                this.appendData(y);
                this.timer = setTimeout(this.fetchResults, 500);
              } else {
                if (y.ServerStopped) {
                  this.stopped();
                } else {
                  this.timer = setTimeout(this.fetchResults, 500);
                }
              }
            }
          },
          error: function (xXHR, sStatus, err) {
            this.stopped();
            this.failed(err);
          }
        });
      },
      stopResults: function () {
        var searchid = this.searchid;
        var stop = this.stop;
        var that = this;
        var lastitem = this.lastitem;
        $.ajax({
          type: 'POST',
          url: that.url() + 'FetchResults',
          cache: false,
          context: that,
          dataType: 'json',
          data: "{searchid:'" + searchid + "',stopit:" + stop + ",lastitem:'" + lastitem + "'}",
          contentType: 'application/json; charset=utf-8',
          success: function (ret, sStatus, xXHR) {
            var y = JSON.parse(ret.d);
            if (y.err) {
              this.stopped();
              this.failed(y.msg)
            } 
          },
          error: function (xXHR, sStatus, err) {
            this.stopped();
            this.failed(err);
          }
        });
      },
      appendData: function (z) {
        var tbl = this.tbl;
        var y = $get(z.Target);
        var itm = new DmsNode(y);
        if (z.strHTML.length > 0) {
          var y = $get(z.Target);
          y.setAttribute('data-loaded', '1');
          for (i = z.strHTML.length - 1; i > -1; i--) {
            y.insertAdjacentHTML('afterend', z.strHTML[i]);
          }
          this.toggleTree(y, 0);
        }
      }
    } // dmsScript
    $get('dmsKeyWordInput').addEventListener('keyup', dmsScript.dmsKeyWordHelp);
    $("#dmsWinLoad").modal('show');
    dmsScript.start();

    function DmsSelectedFiles(f) {
      this.fileName = f.name;
      this.fileType = dmsScript.getFileType(f.type);
      this.fileSize = dmsScript.getFileSize(f.size);
    }
    function DmsFRM() {
      this.frmInput = [];
      this.frmCaption = '';
      this.cmdPublish = 'none';
    }
    function DmsOpt() {
      for (var i = 0; i < dms_menuOption.length; i++) {
        this[dms_menuOption[i]] = new DmsFRM();
      }
    }
    function DmsTgt() {
      for (var i = 1; i < 18; i++) {
        this[i] = new DmsOpt();
      }
    }
    var dmsMatrix = new DmsTgt();
    //Default Value
    for (var i = 1; i < 18; i++) {
      for (var k = 0; k < dms_type_menu[i].length; k++) {
        dmsMatrix[i][dms_menuOption[dms_type_menu[i][k]]].frmInput = [0, 1, 3, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19];
        dmsMatrix[i][dms_menuOption[dms_type_menu[i][k]]].frmCaption = dms_menuOption[dms_type_menu[i][k]];
      }
    }

    //OverRide Default Form Fields
    dmsMatrix[1]['Create'].frmInput = [0, 1, 3, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19];
    dmsMatrix[1]['Edit'].frmInput = [0, 1, 3, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19];
    dmsMatrix[1]['Authorize to User'].frmInput = [0, 20, 2, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19];
    dmsMatrix[1]['Authorize to User Group'].frmInput = [0, 20, 23, 10];
    dmsMatrix[1]['Attach Workflow'].frmInput = [0, 20, 24];
    dmsMatrix[1]['Remove Workflow'].frmInput = [0, 20, 24];

    dmsMatrix[3]['Edit'].frmInput = [0, 5, 6, 7, 8, 9, 10, 20];
    dmsMatrix[3]['Edit'].cmdPublish = 'block';
    dmsMatrix[3]['Publish'] = dmsMatrix[3]['Edit'];
    dmsMatrix[3]['Edit and Publish'] = dmsMatrix[3]['Edit'];

    dmsMatrix[17]['Edit User of Group'].frmInput = [0, 3, 20, 10, 5, 6, 7, 8, 9, 11, 12, 13, 14, 15, 16, 19];

    dmsMatrix[5]['Edit'].frmInput = [0, 3, 20, 26, 25, 5, 6, 7, 8, 9, 11, 12, 13, 14, 15, 16, 19];

    dmsMatrix[6]['Create'].frmInput = [0, 3, 1, 10, 5, 6, 7, 8, 9, 11, 12, 13, 14, 15, 16, 19];
    dmsMatrix[6]['Create'].frmCaption = 'Create User Group';
    dmsMatrix[6]['Edit'].frmInput = [0, 3, 1, 10, 5, 6, 7, 8, 9, 11, 12, 13, 14, 15, 16, 19];
    dmsMatrix[6]['Edit'].frmCaption = 'Edit User Groupr';
    dmsMatrix[6]['Add User to Group'].frmInput = [0, 3, 2, 10];

    dmsMatrix[7]['Create'].frmInput = [0, 3, 1];

    dmsMatrix[10]['Create'].frmInput = [0, 3, 20, 1, 4, 23, 10];
    dmsMatrix[10]['Edit'].frmInput = [0, 3, 20, 1, 4, 23, 10];


    dmsMatrix[11]['Edit'].frmInput = [0, 2, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19];
    dmsMatrix[11]['Edit'].frmCaption = 'Edit Folder Authorization';

    dmsMatrix[13]['Approve / Acknowledge'].frmInput = [0, 3, 20, 21, 22];
    dmsMatrix[13]['Approve / Acknowledge'].frmCaption = 'Approve / Acknowledge';

  </script>
  <script>
    function DmsMenuOption(p) {
      var t = new DmsNode(p);
      t.source = t.target.split('_')[0];
      t.options = [];
      if (t.source == 'hrRoot') {
        for (var i = 0; i < dms_hist_menu[t.type].length; i++) {
          t.options[i] = dms_menuOption[dms_hist_menu[t.type][i]];
        }
      } else {
        if (t.base == 'Type') {
          switch (t.type) {
            case 11:
            case 13:
            case 14:
            case 15:
              break;
            case 18:
            case 19:
              for (var i = 0; i < dms_type_menu[t.type].length; i++) {
                t.options[i] = dms_menuOption[dms_type_menu[t.type][i]];
              }
              break;
            default:
              t.options = ['Create'];
              break;
          }
        }
        if (t.base == 'Item' || t.base == 'User') {
          for (var i = 0; i < dms_type_menu[t.type].length; i++) {
            t.options[i] = dms_menuOption[dms_type_menu[t.type][i]];
          }
        }
      }
      return t;
    }

    var mnuScript = {
      showMenu: function (t) {
        var p = new DmsMenuOption(t);
        var menu = $get("dmsContextMenu");
        menu.innerHTML = this.getMenuStr(p);
        menu.style.top = parseFloat(this.mouseY(event)) - 25 + 'px';
        menu.style.left = this.mouseX(event) + 'px';
        menu.style.display='block';
        $(document).bind("click", function (event) {
          $get("dmsContextMenu").style.display = 'none';
        });
        window.event.returnValue = false;
      },
      getMenuStr: function (o) {
        var str = '';
        for (i = 0; i < o.options.length; i++) {
          var x = o.options[i].split('|');
          var y = x[0];
          var disabled = false;
          if (x[0].substring(0, 1) == '#') {
            y = x[0].replace('#', '');
            disabled = true;
          }
          if (!disabled)
            str = str + "<tr class='dms-menu-row' data-source='" + o.source + "' data-options='" + y + "' onclick='return dmsScript.menuClicked(this);'>";
          else
            str = str + "<tr class='dms-menu-row dms-disabled' data-source='" + o.source + "' data-options='" + y + "'>";

          if (x.length == 2) {
            str = str + "   <td class='dms-menu-icon'>XX</td><td class='dms-menu-text' title='" + x[1] + "'>" + x[0] + "</td>";
          } else {
            str = str + "   <td class='dms-menu-icon'>XX</td><td class='dms-menu-text'>" + x[0] + "</td>";
          }
          str = str + "</tr>"
        }
        return str;
      },
      enableMenu: function () {
        window.addEventListener("contextmenu", function (e) {
          e.preventDefault()
        });
      },
      mouseX: function (evt) {
        if (evt.pageX) {
          return evt.pageX;
        } else if (evt.clientX) {
          return evt.clientX + (document.documentElement.scrollLeft ?
              document.documentElement.scrollLeft :
              document.body.scrollLeft);
        } else {
          return null;
        }
      },
      mouseY: function (evt) {
        if (evt.pageY) {
          return evt.pageY;
        } else if (evt.clientY) {
          return evt.clientY + (document.documentElement.scrollTop ?
          document.documentElement.scrollTop :
          document.body.scrollTop);
        } else {
          return null;
        }
      }
    }
    mnuScript.enableMenu();
    function getIconHTML(t, f) {
      var ret = "";
      if (t == "3" || t == "4" || t == '20') {
        var ext = f.split(".").pop();
        switch (ext.toUpperCase()) {
          case "DOC":
          case "DOCX":
            ret = '<i class="far fa-file-word dms-icon" style="color:SteelBlue;"></i>'
            break;
          case "XLS":
          case "XLSX":
          case "XLSM":
          case "CSV":
            ret = '<i class="far fa-file-excel dms-icon" style="color:LimeGreen;"></i>'
            break;
          case "PPT":
          case "PPTX":
            ret = '<i class="far fa-file-powerpoint dms-icon" style="color:Crimson;"></i>'
            break;
          case "JPG":
          case "JPEG":
          case "PNG":
          case "BMP":
          case "GIF":
            ret = '<i class="far fa-file-image dms-icon" style="color:Plum;"></i>'
            break;
          case "MP4":
          case "AVI":
          case "MPG":
          case "MPEG":
          case "MKA":
            ret = '<i class="far fa-file-video dms-icon" style="color:Magenta;"></i>'
            break;
          case "MP3":
            ret = '<i class="far fa-file-audio dms-icon" style="color:LightCoral;"></i>'
            break;
          case "TXT":
            ret = '<i class="far fa-file-alt dms-icon" style="color:Teal;"></i>'
            break;
          case "DWG":
            ret = '<i class="fas fa-file-code dms-icon" style="color:PaleVioletRed;"></i>'
            break;
          case "PDF":
            ret = '<i class="far fa-file-pdf dms-icon" style="color:red;"></i>'
            break;
          case "ZIP":
          case "RAR":
            ret = '<i class="far fa-file-archive dms-icon" style="color:gold;"></i>'
            break;
          default:
            ret = '<i class="far fa-file dms-icon" style="color:gray;"></i>'
        }
      } else {
        switch (t) {
          case 1: // folder
            ret = '<i class="fas fa-folder dms-icon" style="color:Gold;"></i>'
            break;
          case 2: //folder group
            ret = '<i class="far fa-folder dms-icon" style="color:orange;"></i>'
            break;
          case 3: //file
            ret = '<i class="far fa-file dms-icon" style="color:green;"></i>'
            break;
          case 4: //file group
            ret = '<i class="fas fa-file dms-icon" style="color:green;"></i>'
            break;
          case 5: //user
            ret = '<i class="fas fa-user-tie dms-icon" style="color:DarkMagenta;"></i>'
            break;
          case 6: //user group
            ret = '<i class="fas fa-users dms-icon" style="color:Chartreuse;"></i>'
            break;
          case 7: //tag
            ret = '<i class="fas fa-tag dms-icon" style="color:blue;"></i>'
            break;
          case 8: //link
            ret = '<i class="fas fa-link dms-icon" style="color:darkgray;"></i>'
            break;
          case 9: //u-value
            ret = '<i class="fab fa-playstation dms-icon" style="color:Crimson;"></i>'
            break;
          case 10: //workflow
            ret = '<i class="fas fa-network-wired dms-icon" style="color:BlueViolet;"></i>'
            break;
          case 11: //Authorization
            ret = '<i class="fas fa-user-tag dms-icon" style="color:DeepPink;"></i>'
            break;
          case 12: //granted to me
            ret = '<i class="fas fa-highlighter dms-icon" style="color:DarkTurquoise;"></i>'
            break;
          case 13: //under approval
            ret = '<i class="fas fa-envelope-open-text dms-icon" style="color:DarkOrange;"></i>'
            break;
          case 14: //approved by me
            ret = '<i class="fas fa-pen-nib dms-icon" style="color:FireBrick;"></i>'
            break;
          case 15: //enumItemTypes.Searches
            ret = '<i class="fab fa-searchengin dms-icon" style="color:#00bfff;"></i>'
            break;
          case 16: // enumItemTypes.Projects
            ret = '<i class="fas fa-atom dms-icon" style="color:#bf00ff;"></i>'
            break;
          case 17: // enumItemTypes.Projects
            ret = '<i class="fas fa-user-friends dms-icon" style="color:coral;"></i>'
            break;
            case 18: //enumItemTypes.Selected
            ret = '<i class="fas fa-globe dms-tree-icon" style="color:blue;"></i>'
            break;
            case 19: // enumItemTypes.Fevorites
            ret = '<i class="fab fa-galactic-republic dms-tree-icon" style="color:darkgoldenrod;"></i>'
            break;
        }

      }
      return ret;
    }

  </script>
  <script>
    function dmsState(p) {
      switch (parseInt(p)) {
        case 1: return 'Created';
        case 2: return 'Checked In';
        case 3: return 'Checked Out';
        case 4: return 'Under Verification';
        case 5: return 'Under Approval';
        case 6: return 'Published';
        case 7: return 'Under Revision';
        case 8: return 'Superseded';
        case 9: return 'Expired';
        case 10: return 'Closed';
        case 11: return 'Approved';
        case 12: return 'Rejected';
        case 13: return 'DMSError';
        case 14: return 'Submitted';
        case 15: return 'Acknowledged';
        case 16: return 'Issued';
      }
      return '';
    }
    function dmsQuality(p) {
      switch (parseInt(p)) {
        case 1: return 'No Restriction';
        case 2: return 'Not Allowed';
        case 3: return 'Creator Only';
        case 4: return 'All Authorized';
        case 5: return 'All Authorized Same Company';
        case 6: return 'All Authorized Same Division';
        case 7: return 'All Authorized Same Department';
        case 8: return 'All Authorized Same Project';
        case 9: return 'All Authorized Same WBS';
        case 10: return 'Having Keyword';
        case 11: return 'Use Workflow';
        case 12: return 'All Items';
        case 13: return 'Authorized Items';
      }
      return '';
    }

    function dmsType(p) {
      switch (parseInt(p)) {
        case 1: return 'Folder';
        case 2: return 'Folder Group';
        case 3: return 'File';
        case 4: return 'File Group';
        case 5: return 'User';
        case 6: return 'User Group';
        case 7: return 'Tag';
        case 8: return 'Link';
        case 9: return 'U-Value';
        case 10: return 'Workflow';
        case 11: return 'Authorizations';
        case 12: return 'Granted To Me';
        case 13: return 'Under Approval-Pending';
        case 14: return 'Approved By Me';
        case 15: return 'Searches';
        case 16: return 'Projects';
        case 17: return 'UserGroup User';
        case 18: return 'Selected';
        case 19: return 'Fevorites';
        case 20: return 'Selected File';
      }
      return '';
    }
    var dmsFieldType = [];
    dmsFieldType['ItemTypeID'] = 'dmsType';
    dmsFieldType['StatusID'] = 'dmsState';
    dmsFieldType['ConvertedStatusID'] = 'dmsState';
    dmsFieldType['ForwardLinkedItemTypeID'] = 'dmsType';
    dmsFieldType['LinkedItemTypeID'] = 'dmsType';
    dmsFieldType['BackwardLinkedItemTypeID'] = 'dmsType';
    dmsFieldType['Publish'] = 'dmsQuality';
    dmsFieldType['CreateFolder'] = 'dmsQuality';
    dmsFieldType['RenameFolder'] = 'dmsQuality';
    dmsFieldType['DeleteFolder'] = 'dmsQuality';
    dmsFieldType['CreateFile'] = 'dmsQuality';
    dmsFieldType['DeleteFile'] = 'dmsQuality';
    dmsFieldType['GrantAuthorization'] = 'dmsQuality';
    dmsFieldType['BrowseList'] = 'dmsQuality';
    dmsFieldType['ShowInList'] = 'dmsQuality';

  </script>
</asp:Content>

