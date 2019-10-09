var var_search = {
  searching: false,
  stop: false,
  searchMode: false,
  fetching: false,
  searchid: '',
  timer: '',
  lastitem: '',
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
    var gb = $get('gridBody')
    gb.innerHTML = '';
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
    var sUrl = 'App_Services/dmsClientRequest.asmx/InitiateSearch';
    $.ajax({
      type: 'POST',
      url: sUrl,
      cache: false,
      context: that,
      dataType: 'json',
      data: "{context:'" + x + "',searchid:'" + searchid + "'}",
      contentType: 'application/json; charset=utf-8',
      success: function (ret, sStatus, xXHR) {
        var y = JSON.parse(ret.d);
        if (y.isErr) {
          var_search.stopped();
          var_search.failed(y.errMsg);
        } else {
          var_search.searchid = y.SearchID;
          var_search.timer = setTimeout(var_search.triggerSearch, 200);
        }
      },
      error: function (xXHR, sStatus, err) {
        var_search.stopped();
        var_search.failed(err);
      },
    });
  },
  triggerSearch: function () {
    var searchid = var_search.searchid;
    var sUrl = 'App_Services/dmsClientRequest.asmx/TriggerSearch';
    var url = "test.aspx?sid=" + searchid;
    var that = this;
    $.ajax({
      type: 'POST',
      url: sUrl,
      cache: false,
      context: that,
      dataType: 'json',
      data: "{searchid:'" + searchid + "'}",
      contentType: 'application/json; charset=utf-8',
      success: function (ret, sStatus, xXHR) {
        var y = JSON.parse(ret.d);
        if (y.isErr) {
          var_search.stopped();
          var_search.failed(y.errMsg);
        }
      },
      error: function (xXHR, sStatus, err) {
        var_search.stopped();
        var_search.failed(err);
      },
    });
    var_search.fetching = true;
    var_search.lastitem = '';
    var_search.timer = setTimeout(var_search.fetchResults, 500);
  },
  fetchResults: function () {
    var searchid = var_search.searchid;
    var stop = var_search.stop;
    var that = this;
    var lastitem = var_search.lastitem;
    var sUrl = 'App_Services/dmsClientRequest.asmx/FetchResults';
    $.ajax({
      type: 'POST',
      url: sUrl,
      cache: false,
      context: that,
      dataType: 'json',
      data: "{searchid:'" + searchid + "',stopit:" + stop + ",lastitem:'" + lastitem + "'}",
      contentType: 'application/json; charset=utf-8',
      success: function (ret, sStatus, xXHR) {
        var y = JSON.parse(ret.d);
        if (y.isErr) {
          var_search.stopped();
          var_search.failed(y.errMsg);
        } else {
          if (y.recordCount > "0") {
            var_search.lastitem = y.lastItem;
            //y.strResults
            //Show Data In Grid
            var_search.appendData(y.strResults);
            var_search.timer = setTimeout(var_search.fetchResults, 500);
          } else {
            if (y.serverStopped) {
              var_search.stopped();
            } else {
              var_search.timer = setTimeout(var_search.fetchResults, 500);
            }
          }
        }
      },
      error: function (xXHR, sStatus, err) {
        var_search.stopped();
        var_search.failed(err);
      }
    });
  },
  stopResults: function () {
    var searchid = var_search.searchid;
    var stop = var_search.stop;
    var that = this;
    var lastitem = var_search.lastitem;
    var sUrl = 'App_Services/dmsClientRequest.asmx/FetchResults';
    $.ajax({
      type: 'POST',
      url: sUrl,
      cache: false,
      dataType: 'json',
      data: "{searchid:'" + searchid + "',stopit:" + stop + ",lastitem:'" + lastitem + "'}",
      contentType: 'application/json; charset=utf-8',
      success: function (ret, sStatus, xXHR) {
        var y = JSON.parse(ret.d);
        var_search.stopped();
      },
      error: function (xXHR, sStatus, err) {
        var_search.stopped();
        var_search.failed(err);
      }
    });
  },
  failed: function (x) {
    alert(x);
  },
  appendData: function (x) {
    var oItm = JSON.parse(x);
    var str = getGridstr(oItm);
    var gb = $get('gridBody')
    gb.innerHTML = str + gb.innerHTML;

  }
}
