var loading = false;
var loadingImg;
var loadingCog;
var lastLoadedGrid;
var lastLoadedHistory;
function show_row(x, tbl) {
  var indent = parseInt(x.getAttribute('data-indent'));
  var expended = parseInt(x.getAttribute('data-expended'));
  var bottom = parseInt(x.getAttribute('data-bottom'));

  x.style.display = 'table-row';

  if (bottom == '0')
    return;
  if (expended == '0')
    return;

  for (var i = 0, row; row = tbl.rows[i]; i++) {
    if (row.id.substr(0, x.id.length) == x.id) {
      if (parseInt(row.getAttribute('data-indent')) == indent + 1) {
        show_row(row, tbl);
      }
    }
  }

}
function hide_row(x, tbl) {
  var indent = parseInt(x.getAttribute('data-indent'));
  var expended = parseInt(x.getAttribute('data-expended'));
  var bottom = parseInt(x.getAttribute('data-bottom'));

  x.style.display = 'none';

  if (bottom == '0')
    return;
  if (expended == '0')
    return;

  for (var i = 0, row; row = tbl.rows[i]; i++) {
    if (row.id.substr(0, x.id.length) == x.id) {
      if (parseInt(row.getAttribute('data-indent')) > indent) {
        hide_row(row, tbl);
      }
    }
  }


}
function tree_chkreload(o) {
  if (loading)
    return;
  var x = $get(o.id.replace('img_', ''));
  var aID = x.id.split('_');
  var bottom = parseInt(x.getAttribute('data-bottom'));
  var loaded = parseInt(x.getAttribute('data-loaded'));

  load_grid(x);


  if (bottom == 0)
    return;
  if (loaded == 1) {
    //Commented to Reload always
    //if (!confirm('Reload  Data ?'))
    //  return;
  }
  load_activity(x);
}
function load_activity(x) {
  $get('ReloadingNode').innerHTML = x.id;
  var aID = x.id.split('_');
  var tbl = document.getElementById(aID[0]);
  var base = x.getAttribute('data-base');
  var indent = parseInt(x.getAttribute('data-indent'));
  var expended = parseInt(x.getAttribute('data-expended'));
  var bottom = parseInt(x.getAttribute('data-bottom'));
  var loaded = parseInt(x.getAttribute('data-loaded'));
  var item = x.getAttribute('data-item');
  var type = x.getAttribute('data-type');
  //remove all child rows
  loading = true;
  loadingImg = $get('img_' + x.id);
  loadingImg.src = '/WebDMS2/TreeImgs/Loading.gif';
  if (loaded == 1)
    for (var i = tbl.rows.length - 1; i > -1; i--) {
      var row = tbl.rows[i];
      if (row.id.substr(0, x.id.length) == x.id) {
        if (parseInt(row.getAttribute('data-indent')) > indent) {
          tbl.deleteRow(i);
        }
      }
    }
  //Reload
  $.ajax({
    url: '/WebDMS2/App_Services/dmsMain.ashx',
    dataType: 'json',
    cache: false,
    data: {
      'Base': base,
      'Item': item,
      'Type': type,
      'ID': x.id,
      'Loaded': loaded,
      'Expended': expended,
      'Bottom': bottom,
      'Indent': indent
    },
    contentType: "application/json; charset=utf-8"
  }).done(function (ret) {
    if (ret.Request.dmsType== 'Root') {
      successTbl(ret);
    } else {
    success(ret);
    }
  }).fail(function (err) {
    failed(err);
    //alert('Err: ' + err);
  });
}
function successTbl(r) {
  $get(r.dmsTarget).innerHTML = r.strHTML;
  var tbl = $get(r.dmsTarget).childNodes[0];
  var x = tbl.rows[0];
  x.setAttribute('data-loaded', '1');
  //Expand It
  var aID = x.id.split('_');
  var tbl = document.getElementById(aID[0]);
  var indent = parseInt(x.getAttribute('data-indent'));
  x.setAttribute('data-expended', '1');
  for (var i = 0, row; row = tbl.rows[i]; i++) {
    if (row.id.substr(0, x.id.length) == x.id) {
      if (parseInt(row.getAttribute('data-indent')) == indent + 1) {
        show_row(row, tbl);
      }
    }
  }
  $get('img_' + x.id).src = '/WebDMS2/TreeImgs/Minus.gif';
  loading = false;
}
function success(r) {
  //var aR = r.split('|');
  var rowID = r.dmsTarget;
  var rows = r.strHTML.split('##');
  var x = $get(rowID);
  for (var i = 0; i < rows.length; i++) {
    x.insertAdjacentHTML('afterend', rows[i]);
  }
  x.setAttribute('data-loaded', '1');
  //Expand It
  var aID = x.id.split('_');
  var tbl = document.getElementById(aID[0]);
  var indent = parseInt(x.getAttribute('data-indent'));
  x.setAttribute('data-expended', '1');
  for (var i = 0, row; row = tbl.rows[i]; i++) {
    if (row.id.substr(0, x.id.length) == x.id) {
      if (parseInt(row.getAttribute('data-indent')) == indent + 1) {
        show_row(row, tbl);
      }
    }
  }
  $get('img_' + x.id).src = '/WebDMS2/TreeImgs/Minus.gif';
  loading = false;
}
function failed(e) {
  loadingImg.src = '/WebDMS2/TreeImgs/Plus.gif';
  loading = false;
  alert(e.get_message);
}

function load_history(x) {
  if (lastLoadedHistory) {
    if (lastLoadedHistory.id == x.id) {
      return;
    }
  }
  lastLoadedHistory = x;
  var grID = x.id.replace('gridRow_', '');
  $.ajax({
    type: 'POST',
    url: 'App_Services/dmsClientRequest.asmx/LoadHistory',
    dataType: 'json',
    cache: false,
    data: "{context:'" + grID + "'}",
    contentType: 'application/json; charset=utf-8',
    success: function (result) {
      var oItm = JSON.parse(result.d);
      var str = getHistorystr(oItm);
      var hb = $get('historyBody')
      hb.innerHTML = str;

    },
    error: function (err) {
      alert(err);
      //failed(err);
    },
    failure: function (err) {
      //failed(err);
      alert('Err: ' + err);
    }
  });

}
function getHistorystr(t) {
  var str = '';
  for (i = 0; i < t.length; i++) {
    var o = t[i];
    str = str + "          <div class='row btn-outline-dark'>";
    str = str + "            <div class='col-sm-1'>" + o.HistoryID + "</div>";
    str = str + "            <div class='col-sm-4'>"
    str = str + "              " + o.ActionRemarks;
    str = str + "            </div>"
    str = str + "            <div class='col-sm-2'>"
    str = str + "              " + o.DMS_States12_Description;
    str = str + "            </div>"
    str = str + "            <div class='col-sm-2'>"
    str = str + "              " + o.ActionOn;
    str = str + "            </div>"
    str = str + "            <div class='col-sm-3'>"
    str = str + "              " + o.aspnet_Users19_UserFullName;
    str = str + "            </div>"
    str = str + "          </div>"
  }
  return str;
}
function load_grid(x) {
  if (lastLoadedGrid) {
    if (lastLoadedGrid.id == x.id) {
      return;
    }
  }
  lastLoadedGrid = x;
  var y = new DmsNode(x);
  var mt = x.id + '|' + y.type + '|' + y.item;

  $.ajax({
    type: 'POST',
    url: 'App_Services/dmsClientRequest.asmx/LoadGrid',
    dataType: 'json',
    cache:false,
    data: "{context:'" + mt + "'}",
    contentType: 'application/json; charset=utf-8',
    success: function (result) {
      var oItm = JSON.parse(result.d);
      var str=getGridstr(oItm);
      var gb = $get('gridBody')
      gb.innerHTML = str;

    },
    error: function (err) {
      alert(err);
      //failed(err);
    },
    failure: function (err) {
      //failed(err);
      alert('Err: ' + err);
    }
  });

}
function getGridstr(t) {
  var str = '';
  for (i = 0; i < t.length; i++) {
    var o = t[i];
    str = str + "          <div class='row btn-outline-dark' onclick='load_history(this);' id='gridRow_" + o.ItemID + "' style='cursor:pointer;'>";
    str = str + "            <div class='col-sm-1'>" + o.ItemID + "</div>";
    str = str + "            <div class='col-sm-1'>";
    str = str + "              <div class='custom-control custom-checkbox'>";
    str = str + "                <input type='checkbox' class='custom-control-input' id='checkAll" + o.ItemID + "'>";
    str = str + "                <label class='custom-control-label' for='checkAll" + o.ItemID + "'></label>";
    str = str + "              </div>";
    str = str + "            </div>";
    str = str + "            <div class='col-sm-1'>"; //o.DMS_ItemTypes8_Description;
    str = str + "              " + getIconHTML(o.ItemTypeID, o.Description);
    str = str + "            </div>"
    str = str + "            <div class='col-sm-5'>"
    str = str + "              " + o.Description;
    str = str + "            </div>"
    str = str + "            <div class='col-sm-2'>"
    str = str + "              " + o.DMS_States12_Description;
    str = str + "            </div>"
    str = str + "            <div class='col-sm-2'>"
    str = str + "              " + o.CreatedOn.substring(0,10);
    str = str + "            </div>"
    str = str + "          </div>"
  }
  return str;
}
function tree_toggle(x) {
  if (loading)
    return;
  var aID = x.id.split('_');
  var tbl = document.getElementById(aID[0]);
  var indent = parseInt(x.getAttribute('data-indent'));
  var expended = parseInt(x.getAttribute('data-expended'));
  var bottom = parseInt(x.getAttribute('data-bottom'));
  var loaded = parseInt(x.getAttribute('data-loaded'));

  load_grid(x);

  if (bottom == 0)
    return;

  if (expended == 0) {
    if (loaded == 0) {
      load_activity(x);
    } else {
      x.setAttribute('data-expended', '1');
      $get('img_' + x.id).src = '/WebDMS2/TreeImgs/Minus.gif';
      for (var i = 0, row; row = tbl.rows[i]; i++) {
        if (row.id.substr(0, x.id.length) == x.id) {
          if (parseInt(row.getAttribute('data-indent')) == indent + 1) {
            show_row(row, tbl);
          }
        }
      }
    }
    return;
  }

  if (expended == '1') {
    x.setAttribute('data-expended', '0');
    $get('img_' + x.id).src = '/WebDMS2/TreeImgs/Plus.gif';
    for (var i = 0, row; row = tbl.rows[i]; i++) {
      if (row.id.substr(0, x.id.length) == x.id) {
        if (parseInt(row.getAttribute('data-indent')) > indent) {
          hide_row(row, tbl);
        }
      }
    }
  }

}
/*
============================
DMS Menu related scripts
============================
*/
function DmsNode(t) {
  this.base = t.dataset.base;
  this.expended = parseInt(t.dataset.expended);
  this.indent = parseInt(t.dataset.indent);
  this.bottom = parseInt(t.dataset.bottom);
  this.loaded = parseInt(t.dataset.loaded);
  this.item = parseInt(t.dataset.item);
  this.type = parseInt(t.dataset.type);
}
function DmsMenu(t) {
  this.action = t.dataset.dmsname;
}
var dms_cmMenu = {
  menuOption: '',
  menuTarget: '',
  ctrlUpload:'',
  getFileType: function(x){
    if(x.indexOf('sheet')>=0) return 'MS-Excel';
    if (x.indexOf('word') >= 0) return 'MS-Word';
    if (x.indexOf('presentation') >= 0) return 'MS-PowerPoint';
    return x || 'Other';
  },
  getFileSize: function(x){
    var y = parseInt(x);
    if (y > 1073741824) return (y / 1073741824).toFixed(2) + ' GB';
    if (y > 1048576) return (y / 1048576).toFixed(2) + ' MB';
    if (y > 1024) return (y / 1024).toFixed(2) + ' KB';
    return y + ' Bytes';
  },
  filesSelected: function (evt) {
    var EventTarget = evt.target; //File Upload Control
    this.ctrlUpload = evt.target;
    var files = evt.target.files; // FileList object

    // files is a FileList of File objects. List some properties.
    var output = [];
    for (var i = 0, f; f = files[i]; i++) {
      output.push(
        '<div class="row">',
        '<div class="col-sm-6"><strong>', f.name, '</strong></div>',
        '<div class="col-sm-3">', this.getFileType(f.type), '</div>',
        '<div class="col-sm-3">', this.getFileSize(f.size), '</div>',
        '</div>'
        );
    }
    $get('UploadFileList').innerHTML = output.join('');
    $(evt.target).siblings(".custom-file-label").addClass("selected").html('Choose File');
  },
  startUpload: function () {
    $("#lblMessage").html('');
    var y = new DmsNode(this.menuTarget);
    y.dmsID = this.menuTarget.id;
    var x = JSON.stringify(y);
    $get('dmsTarget').value = x;
    var fd = $('form')[0];
    var ctrlUpload = this.ctrlUpload;
    $("#FileProgress").width("0%");
    $("#FileProgress").text("0 %");
    $.ajax({
      url: '/WebDMS2/App_Services/Handler.ashx',
      type: 'POST',
      data: new FormData($('form')[0]),
      cache: false,
      contentType: false,
      processData: false,
      success: function (file) {
        $(".progress").hide();
        $("#lblMessage").html("<b>" + file.main + "</b><i>&nbsp;[ " + file.errmsg + " ]</i>" );
        ctrlUpload.value = null;
        if (!file.iserr) {
          var x = $get(file.dmsbase);
          load_activity(x);
        }
      },
      xhr: function () {
        var fileXhr = $.ajaxSettings.xhr();
        if (fileXhr.upload) {
          $('.progress').show();
          $("#FileProgress").width("0%");
          $("#FileProgress").text("0 %");
          fileXhr.upload.addEventListener("progress", function (e) {
            if (e.lengthComputable) {
              var s = parseInt((e.loaded / e.total) * 100);
              $("#FileProgress").width(s + "%");
              $("#FileProgress").text(s + " %");
            }
          }, false);
        }
        return fileXhr;
      }
    });
  },
  submited: function (publish) {
    $("#myModal").modal('hide');

    var y = new GetDMSItemEntry();
    var x = JSON.stringify(y);
    var mt = this.menuTarget.id + '|' + this.menuTarget.dataset.type + '|' + this.menuTarget.dataset.item;
    var mo = this.menuOption.dataset.dmsname;
    $.ajax({
      type: 'POST',
      url: 'App_Services/dmsClientRequest.asmx/MyDmsSubmit',
      cache: false,
      dataType: 'json',
      data: "{context:'" + x + "',dmstarget:'" + mt + "',dmsoption:'" + mo + "'}",
      contentType: 'application/json; charset=utf-8',
      success: function (ret) {
        var y = JSON.parse(ret.d);
        if (y.isError) {
          alert(y.ErrorMessage);
          return false;
        }
        if (y.dmsTarget != '') {
          var x = $get(y.dmsTarget);
          load_activity(x);
          if (publish) {
            dms_cmMenu.publishItem(mt, mo);
          }

        }
      },
      error: function (err) {
        //failed(err);
      },
      failure: function (err) {
        //failed(err);
        //alert('Err: ' + err);
      }
    });


  },
  publishItem:function(mt,mo){
    $.ajax({
      type: 'POST',
      url: 'App_Services/dmsClientRequest.asmx/PublishItem',
      cache: false,
      dataType: 'json',
      data: "{context:'" + mt + "'}",
      contentType: 'application/json; charset=utf-8',
      success: function (ret) {
        var y = JSON.parse(ret.d);
        if (y.isErr) {
          alert(y.errMsg);
        } else {
          if (y.dmsTarget != '') {
            var x = $get(y.dmsTarget);
            load_activity(x);
          }
        }
      },
      error: function (err) {
        //failed(err);
      },
    });
  },
  menu_click: function (t) {
    this.menuOption = t;
    var nd = new DmsNode(this.menuTarget);
    var nu = new DmsMenu(t);
    $get('SelectedOption').innerHTML = nu.action;
    var dd = $get('F_ItemTypeID');
    dd.value = nd.type;

    switch (nd.type) {
      case 1: // folder
        break;
      case 2: //folder group
        break;
      case 3: //file
        break;
      case 4: //file group
        break;
      case 5: //user
        break;
      case 6: //user group
        break;
      case 7: //tag
        break;
      case 8: //link
        break;
      case 9: //u-value
        break;
      case 10: //workflow
        break;
      case 11: //granted by me
        break;
      case 12: //granted to me
        break;
      case 13: //under approval
        break;
      case 14: //approved by me
        break;
    }
    switch (nu.action) {
      case'Execute':
      case 'Edit':
        $.ajax({
          type: 'POST',
          url: 'App_Services/dmsClientRequest.asmx/GetItem',
          cache: false,
          dataType: 'json',
          data: "{context:'" + nd.item + "', dmsoption:'" + nu.action + "'}",
          contentType: 'application/json; charset=utf-8',
          success: function (ret) {
            var y = JSON.parse(ret.d);
            if (y.isErr) {
              alert(y.errMsg);
            } else {
              setFrmProperty(nd, nu);
              setFrmValue(y.strValue);
              $("#myModal").modal('show');
            }
          },
          error: function (err) {
            //failed(err);
          }
        });

        break;
      case 'Create':
      case 'Authorize':
        setFrmProperty(nd, nu);
        $("#myModal").modal('show');
        break;
      case 'Upload':
        $get('UploadFileList').innerHTML = '';
        $get('lblMessage').innerHTML = '';
        $("#myFileModal").modal('show');
        $('#f_Uploads').value = null;
        break;
      case 'Download':
        if (nd.type != 1 && nd.type != 2 && nd.type != 3 && nd.type != 4) {
          alert('Only File or Folder can be downloaded');
        } else {
          var mt = nd.type + '|' + nd.item;
          var mo = nu.action;
          var fd = $("#myFileDownload");

          fd.modal('show');
          var url = 'App_Downloads/dmsDownload.aspx?mt=' + mt + '&mo=' + mo;
          $.fileDownload(url, {
            successCallback: function (url) {
              fd.modal('hide');
            },
            failCallback: function (responseHtml, url) {
              fd.modal('hide');
            }
          });
        }
        break;
      case 'Delete':
        var mt = this.menuTarget.id + '|' + this.menuTarget.dataset.type + '|' + this.menuTarget.dataset.item;
        var mo = this.menuOption.dataset.dmsname;

        $.ajax({
          type: 'POST',
          url: 'App_Services/dmsClientRequest.asmx/DeleteItem',
          cache: false,
          dataType: 'json',
          data: "{context:'" + mt + "'}",
          contentType: 'application/json; charset=utf-8',
          success: function (ret) {
            var y = JSON.parse(ret.d);
            if (y.isErr) {
              alert(y.errMsg);
            } else {
              if (y.dmsTarget != '') {
                var x = $get(y.dmsTarget);
                load_activity(x);
              }
            }
          },
          error: function (err) {
            //failed(err);
          },
          failure: function (err) {
            //failed(err);
            //alert('Err: ' + err);
          }
        });
        break;
      case 'Publish':
        var mt = this.menuTarget.id + '|' + this.menuTarget.dataset.type + '|' + this.menuTarget.dataset.item;
        var mo = this.menuOption.dataset.dmsname;
        this.publishItem(mt, mo);
        break;
    }
    return false;
  },
  enableMenu: function () {
    window.addEventListener("contextmenu", function (e) {
      e.preventDefault()
    });
  },
  dmsCmenu: function (t) {
    $get('SelectedNode').innerHTML = t.id;
    this.menuTarget = t;
    var menu = $get("dmsContextMenu");

    menu.className = "dmsCmenuShow";
    menu.style.top = parseFloat(this.mouseY(event)) - 25 + 'px';
    menu.style.left = this.mouseX(event) + 'px';

    $(document).bind("click", function (event) {
      $get("dmsContextMenu").className = "dmsCmenuHide";
    });
    window.event.returnValue = false;
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
dms_cmMenu.enableMenu();

//======================//
  //                   0              1                2             3                 4                 5      
  //                   6              7                8             9                 10                11      
  //                   12             13               14            15                16                17      
  //                   18             19               20            21                22
  var dms_Fields = ['ItemID', 'FullDescription',   'UserID',    'ItemTypeID', 'ConvertedStatusID',   'CompanyID',
                  'DivisionID', 'DepartmentID',   'ProjectID',    'WBSID',        'KeyWords',       'CreateFolder',
                  'RenameFolder', 'DeleteFolder', 'CreateFile', 'DeleteFile',  'GrantAuthorization', 'BrowseList',
                  'ShowInList',     'Publish',    'Description', 'Approved',     'ActionRemarks'];
function GetDMSItemEntry() {
  this.ItemID = $get('F_ItemID').value;
  this.UserID = $get('F_UserID').value;
  this.FullDescription = $get('F_FullDescription').value;
  if ($get('F_ItemTypeID').value != '')
    this.ItemTypeID = $get('F_ItemTypeID').value;
  this.CompanyID = $get('F_CompanyID').value;
  this.DivisionID = $get('F_DivisionID').value;
  this.DepartmentID = $get('F_DepartmentID').value;
  this.ProjectID = $get('F_ProjectID').value;
  this.WBSID = $get('F_WBSID').value;
  this.KeyWords = $get('F_KeyWords').value;
  if ($get('F_CreateFolder').value != '')
    this.CreateFolder = $get('F_CreateFolder').value;
  if ($get('F_RenameFolder').value != '')
    this.RenameFolder = $get('F_RenameFolder').value;
  if ($get('F_DeleteFolder').value != '')
    this.DeleteFolder = $get('F_DeleteFolder').value;
  if ($get('F_CreateFile').value != '')
    this.CreateFile = $get('F_CreateFile').value;
  if ($get('F_DeleteFile').value != '')
    this.DeleteFile = $get('F_DeleteFile').value;
  if ($get('F_GrantAuthorization').value != '')
    this.GrantAuthorization = $get('F_GrantAuthorization').value;
  if ($get('F_ShowInList').value != '')
    this.ShowInList = $get('F_ShowInList').value;
  if ($get('F_BrowseList').value != '')
    this.BrowseList = $get('F_BrowseList').value;
  if ($get('F_Publish').value != '')
    this.Publish = $get('F_Publish').value;
  if ($get('F_ConvertedStatusID').value != '')
    this.ConvertedStatusID = $get('F_ConvertedStatusID').value;
  if ($get('F_Approved').value != '')
    this.Approved = $get('F_Approved').value;
  this.ActionRemarks = $get('F_ActionRemarks').value;
}
function setFrmValue(strItm) {
  var oItm = JSON.parse(strItm);
  var aProp = Object.keys(oItm);
  for (i = 0; i < aProp.length; i++) {
    try {
      $get('F_' + aProp[i]).value = oItm[aProp[i]];
    } catch (e) { }
  }
  $get('F_UserID_Display').value = '';
  $get('F_ProjectID_Display').value = '';
  $get('F_WBSID_Display').value = '';
}
function setFrmProperty(nd, nu) {
  var title = $('#modalTitle')[0];
  for (i = 1; i < dms_Fields.length; i++) {
    $('#R_' + dms_Fields[i]).hide();
  }
  $get('cmdPublishOnSave').style.display = 'none';
  var toShow = [0, 1];
  switch (nd.type) {
    case 1:
    case 2:
      switch (nu.action) {
        case 'Edit':
        case 'Create':
          toShow = [0, 1, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19]
          title.textContent = "Create/Edit Folder";
          break;
        case 'Authorize':
          toShow = [0, 2, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19]
          title.textContent = "Folder Authorization to User";
      }
      break;
    case 3: //File
    case 4: //File Group
      switch (nu.action) {
        case 'Edit':
        case 'Create':
          toShow = [0, 5, 6, 7, 8, 9, 10, 20]
          title.textContent = "Update File Tags";
          $get('cmdPublishOnSave').style.display = 'block';
          break;
        case 'Execute':
          toShow = [0, 20, 21, 22]
          title.textContent = "Approve/Reject";
          break;
      }

      break;
    case 5: //user
      switch (nu.action) {
        case 'Edit':
        case 'Create':
          toShow = [0, 2, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19]
          title.textContent = "Create User";
      }
      break;
    case 6: //user group
      switch (nu.action) {
        case 'Edit':
          toShow = [0, 1, 3, 5, 6, 7, 8, 9]
          title.textContent = "Edit User Group";
          break;
        case 'Create':
          toShow = [0, 1, 3, 5, 6, 7, 8, 9]
          title.textContent = "Create User Group";
      }
      break;
    case 10: //WorkFlow
      switch (nu.action) {
        case 'Edit':
        case 'Create':
          toShow = [0, 1, 2, 4, 5, 6, 7, 8, 9]
          title.textContent = "Create/Edit Workflow";
      }
      break;
    default:
      switch (nu.action) {
        case 'Edit':
        case 'Create':
          toShow = [0, 1, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19]
          title.textContent = "Create/Edit";
          break;
        case 'Execute':
          toShow = [0, 20, 21, 22]
          title.textContent = "Approve/Reject";
          break;
      }
  }
  for (i = 1; i < toShow.length; i++) {
    $('#R_' + dms_Fields[toShow[i]]).show();
  }

}
