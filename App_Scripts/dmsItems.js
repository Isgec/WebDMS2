var script_dmsItems = {
    ACEUserID_Selected: function(sender, e) {
      var Prefix = sender._element.id.replace('UserID','');
      var F_UserID = $get(sender._element.id);
      var F_UserID_Display = $get(sender._element.id + '_Display');
      var retval = e.get_value();
      var p = retval.split('|');
      F_UserID.value = p[0];
      F_UserID_Display.innerHTML = e.get_text();
    },
    ACEUserID_Populating: function(sender,e) {
      var p = sender.get_element();
      var Prefix = sender._element.id.replace('UserID','');
      p.style.backgroundImage  = 'url(../../images/loader.gif)';
      p.style.backgroundRepeat= 'no-repeat';
      p.style.backgroundPosition = 'right';
      sender._contextKey = '';
    },
    ACEUserID_Populated: function(sender,e) {
      var p = sender.get_element();
      p.style.backgroundImage  = 'none';
    },
    ACEStatusID_Selected: function(sender, e) {
      var Prefix = sender._element.id.replace('StatusID','');
      var F_StatusID = $get(sender._element.id);
      var F_StatusID_Display = $get(sender._element.id + '_Display');
      var retval = e.get_value();
      var p = retval.split('|');
      F_StatusID.value = p[0];
      F_StatusID_Display.innerHTML = e.get_text();
    },
    ACEStatusID_Populating: function(sender,e) {
      var p = sender.get_element();
      var Prefix = sender._element.id.replace('StatusID','');
      p.style.backgroundImage  = 'url(../../images/loader.gif)';
      p.style.backgroundRepeat= 'no-repeat';
      p.style.backgroundPosition = 'right';
      sender._contextKey = '';
    },
    ACEStatusID_Populated: function(sender,e) {
      var p = sender.get_element();
      p.style.backgroundImage  = 'none';
    },
    ACECreatedBy_Selected: function(sender, e) {
      var Prefix = sender._element.id.replace('CreatedBy','');
      var F_CreatedBy = $get(sender._element.id);
      var F_CreatedBy_Display = $get(sender._element.id + '_Display');
      var retval = e.get_value();
      var p = retval.split('|');
      F_CreatedBy.value = p[0];
      F_CreatedBy_Display.innerHTML = e.get_text();
    },
    ACECreatedBy_Populating: function(sender,e) {
      var p = sender.get_element();
      var Prefix = sender._element.id.replace('CreatedBy','');
      p.style.backgroundImage  = 'url(../../images/loader.gif)';
      p.style.backgroundRepeat= 'no-repeat';
      p.style.backgroundPosition = 'right';
      sender._contextKey = '';
    },
    ACECreatedBy_Populated: function(sender,e) {
      var p = sender.get_element();
      p.style.backgroundImage  = 'none';
    },
    ACEBackwardLinkedItemID_Selected: function(sender, e) {
      var Prefix = sender._element.id.replace('BackwardLinkedItemID','');
      var F_BackwardLinkedItemID = $get(sender._element.id);
      var F_BackwardLinkedItemID_Display = $get(sender._element.id + '_Display');
      var retval = e.get_value();
      var p = retval.split('|');
      F_BackwardLinkedItemID.value = p[0];
      F_BackwardLinkedItemID_Display.innerHTML = e.get_text();
    },
    ACEBackwardLinkedItemID_Populating: function(sender,e) {
      var p = sender.get_element();
      var Prefix = sender._element.id.replace('BackwardLinkedItemID','');
      p.style.backgroundImage  = 'url(../../images/loader.gif)';
      p.style.backgroundRepeat= 'no-repeat';
      p.style.backgroundPosition = 'right';
      sender._contextKey = '';
    },
    ACEBackwardLinkedItemID_Populated: function(sender,e) {
      var p = sender.get_element();
      p.style.backgroundImage  = 'none';
    },
    ACELinkedItemID_Selected: function(sender, e) {
      var Prefix = sender._element.id.replace('LinkedItemID','');
      var F_LinkedItemID = $get(sender._element.id);
      var F_LinkedItemID_Display = $get(sender._element.id + '_Display');
      var retval = e.get_value();
      var p = retval.split('|');
      F_LinkedItemID.value = p[0];
      F_LinkedItemID_Display.innerHTML = e.get_text();
    },
    ACELinkedItemID_Populating: function(sender,e) {
      var p = sender.get_element();
      var Prefix = sender._element.id.replace('LinkedItemID','');
      p.style.backgroundImage  = 'url(../../images/loader.gif)';
      p.style.backgroundRepeat= 'no-repeat';
      p.style.backgroundPosition = 'right';
      sender._contextKey = '';
    },
    ACELinkedItemID_Populated: function(sender,e) {
      var p = sender.get_element();
      p.style.backgroundImage  = 'none';
    },
    ACEChildItemID_Selected: function(sender, e) {
      var Prefix = sender._element.id.replace('ChildItemID','');
      var F_ChildItemID = $get(sender._element.id);
      var F_ChildItemID_Display = $get(sender._element.id + '_Display');
      var retval = e.get_value();
      var p = retval.split('|');
      F_ChildItemID.value = p[0];
      F_ChildItemID_Display.innerHTML = e.get_text();
    },
    ACEChildItemID_Populating: function(sender,e) {
      var p = sender.get_element();
      var Prefix = sender._element.id.replace('ChildItemID','');
      p.style.backgroundImage  = 'url(../../images/loader.gif)';
      p.style.backgroundRepeat= 'no-repeat';
      p.style.backgroundPosition = 'right';
      sender._contextKey = '';
    },
    ACEChildItemID_Populated: function(sender,e) {
      var p = sender.get_element();
      p.style.backgroundImage  = 'none';
    },
    ACEParentItemID_Selected: function(sender, e) {
      var Prefix = sender._element.id.replace('ParentItemID','');
      var F_ParentItemID = $get(sender._element.id);
      var F_ParentItemID_Display = $get(sender._element.id + '_Display');
      var retval = e.get_value();
      var p = retval.split('|');
      F_ParentItemID.value = p[0];
      F_ParentItemID_Display.innerHTML = e.get_text();
    },
    ACEParentItemID_Populating: function(sender,e) {
      var p = sender.get_element();
      var Prefix = sender._element.id.replace('ParentItemID','');
      p.style.backgroundImage  = 'url(../../images/loader.gif)';
      p.style.backgroundRepeat= 'no-repeat';
      p.style.backgroundPosition = 'right';
      sender._contextKey = '';
    },
    ACEParentItemID_Populated: function(sender,e) {
      var p = sender.get_element();
      p.style.backgroundImage  = 'none';
    },
    ACEProjectID_Selected: function(sender, e) {
      var Prefix = sender._element.id.replace('ProjectID','');
      var F_ProjectID = $get(sender._element.id);
      var F_ProjectID_Display = $get(sender._element.id + '_Display');
      var retval = e.get_value();
      var p = retval.split('|');
      F_ProjectID.value = p[0];
      F_ProjectID_Display.innerHTML = e.get_text();
    },
    ACEProjectID_Populating: function(sender,e) {
      var p = sender.get_element();
      var Prefix = sender._element.id.replace('ProjectID','');
      p.style.backgroundImage  = 'url(../../images/loader.gif)';
      p.style.backgroundRepeat= 'no-repeat';
      p.style.backgroundPosition = 'right';
      sender._contextKey = '';
    },
    ACEProjectID_Populated: function(sender,e) {
      var p = sender.get_element();
      p.style.backgroundImage  = 'none';
    },
    ACEForwardLinkedItemID_Selected: function(sender, e) {
      var Prefix = sender._element.id.replace('ForwardLinkedItemID','');
      var F_ForwardLinkedItemID = $get(sender._element.id);
      var F_ForwardLinkedItemID_Display = $get(sender._element.id + '_Display');
      var retval = e.get_value();
      var p = retval.split('|');
      F_ForwardLinkedItemID.value = p[0];
      F_ForwardLinkedItemID_Display.innerHTML = e.get_text();
    },
    ACEForwardLinkedItemID_Populating: function(sender,e) {
      var p = sender.get_element();
      var Prefix = sender._element.id.replace('ForwardLinkedItemID','');
      p.style.backgroundImage  = 'url(../../images/loader.gif)';
      p.style.backgroundRepeat= 'no-repeat';
      p.style.backgroundPosition = 'right';
      sender._contextKey = '';
    },
    ACEForwardLinkedItemID_Populated: function(sender,e) {
      var p = sender.get_element();
      p.style.backgroundImage  = 'none';
    },
    ACEWBSID_Selected: function(sender, e) {
      var Prefix = sender._element.id.replace('WBSID','');
      var F_WBSID = $get(sender._element.id);
      var F_WBSID_Display = $get(sender._element.id + '_Display');
      var retval = e.get_value();
      var p = retval.split('|');
      F_WBSID.value = p[0];
      F_WBSID_Display.innerHTML = e.get_text();
    },
    ACEWBSID_Populating: function(sender,e) {
      var p = sender.get_element();
      var Prefix = sender._element.id.replace('WBSID','');
      p.style.backgroundImage  = 'url(../../images/loader.gif)';
      p.style.backgroundRepeat= 'no-repeat';
      p.style.backgroundPosition = 'right';
      sender._contextKey = '';
    },
    ACEWBSID_Populated: function(sender,e) {
      var p = sender.get_element();
      p.style.backgroundImage  = 'none';
    },
    ACEActionBy_Selected: function(sender, e) {
      var Prefix = sender._element.id.replace('ActionBy','');
      var F_ActionBy = $get(sender._element.id);
      var F_ActionBy_Display = $get(sender._element.id + '_Display');
      var retval = e.get_value();
      var p = retval.split('|');
      F_ActionBy.value = p[0];
      F_ActionBy_Display.innerHTML = e.get_text();
    },
    ACEActionBy_Populating: function(sender,e) {
      var p = sender.get_element();
      var Prefix = sender._element.id.replace('ActionBy','');
      p.style.backgroundImage  = 'url(../../images/loader.gif)';
      p.style.backgroundRepeat= 'no-repeat';
      p.style.backgroundPosition = 'right';
      sender._contextKey = '';
    },
    ACEActionBy_Populated: function(sender,e) {
      var p = sender.get_element();
      p.style.backgroundImage  = 'none';
    },
    validate_UserID: function(sender) {
      var Prefix = sender.id.replace('UserID','');
      this.validated_FK_DMS_Items_UserID_main = true;
      this.validate_FK_DMS_Items_UserID(sender,Prefix);
      },
    validate_StatusID: function(sender) {
      var Prefix = sender.id.replace('StatusID','');
      this.validated_FK_DMS_Items_StatusID_main = true;
      this.validate_FK_DMS_Items_StatusID(sender,Prefix);
      },
    validate_CreatedBy: function(sender) {
      var Prefix = sender.id.replace('CreatedBy','');
      this.validated_FK_DMS_Items_CreatedBy_main = true;
      this.validate_FK_DMS_Items_CreatedBy(sender,Prefix);
      },
    validate_BackwardLinkedItemID: function(sender) {
      var Prefix = sender.id.replace('BackwardLinkedItemID','');
      this.validated_FK_DMS_Items_BackwardLinkedItemID_main = true;
      this.validate_FK_DMS_Items_BackwardLinkedItemID(sender,Prefix);
      },
    validate_LinkedItemID: function(sender) {
      var Prefix = sender.id.replace('LinkedItemID','');
      this.validated_FK_DMS_Items_LinkedItemID_main = true;
      this.validate_FK_DMS_Items_LinkedItemID(sender,Prefix);
      },
    validate_ChildItemID: function(sender) {
      var Prefix = sender.id.replace('ChildItemID','');
      this.validated_FK_DMS_Items_ChildItemID_main = true;
      this.validate_FK_DMS_Items_ChildItemID(sender,Prefix);
      },
    validate_ParentItemID: function(sender) {
      var Prefix = sender.id.replace('ParentItemID','');
      this.validated_FK_DMS_Items_ParentItemID_main = true;
      this.validate_FK_DMS_Items_ParentItemID(sender,Prefix);
      },
    validate_ProjectID: function(sender) {
      var Prefix = sender.id.replace('ProjectID','');
      this.validated_FK_DMS_Items_ProjectID_main = true;
      this.validate_FK_DMS_Items_ProjectID(sender,Prefix);
      },
    validate_ForwardLinkedItemID: function(sender) {
      var Prefix = sender.id.replace('ForwardLinkedItemID','');
      this.validated_FK_DMS_Items_ForwardLinkedItemID_main = true;
      this.validate_FK_DMS_Items_ForwardLinkedItemID(sender,Prefix);
      },
    validate_WBSID: function(sender) {
      var Prefix = sender.id.replace('WBSID','');
      this.validated_FK_DMS_Items_WBSID_main = true;
      this.validate_FK_DMS_Items_WBSID(sender,Prefix);
      },
    validate_ActionBy: function(sender) {
      var Prefix = sender.id.replace('ActionBy','');
      this.validated_FK_DMS_Items_ActionBy_main = true;
      this.validate_FK_DMS_Items_ActionBy(sender,Prefix);
      },
    validate_FK_DMS_Items_UserID: function(o,Prefix) {
      var value = o.id;
      var UserID = $get(Prefix + 'UserID');
      if(UserID.value==''){
        if(this.validated_FK_DMS_Items_UserID_main){
          var o_d = $get(Prefix + 'UserID' + '_Display');
          try{o_d.innerHTML = '';}catch(ex){}
        }
        return true;
      }
      value = value + ',' + UserID.value ;
        o.style.backgroundImage  = 'url(../../images/pkloader.gif)';
        o.style.backgroundRepeat= 'no-repeat';
        o.style.backgroundPosition = 'right';
        PageMethods.validate_FK_DMS_Items_UserID(value, this.validated_FK_DMS_Items_UserID);
      },
    validated_FK_DMS_Items_UserID_main: false,
    validated_FK_DMS_Items_UserID: function(result) {
      var p = result.split('|');
      var o = $get(p[1]);
      if(script_dmsItems.validated_FK_DMS_Items_UserID_main){
        var o_d = $get(p[1]+'_Display');
        try{o_d.innerHTML = p[2];}catch(ex){}
      }
      o.style.backgroundImage  = 'none';
      if(p[0]=='1'){
        o.value='';
        o.focus();
      }
    },
    validate_FK_DMS_Items_CreatedBy: function(o,Prefix) {
      var value = o.id;
      var CreatedBy = $get(Prefix + 'CreatedBy');
      if(CreatedBy.value==''){
        if(this.validated_FK_DMS_Items_CreatedBy_main){
          var o_d = $get(Prefix + 'CreatedBy' + '_Display');
          try{o_d.innerHTML = '';}catch(ex){}
        }
        return true;
      }
      value = value + ',' + CreatedBy.value ;
        o.style.backgroundImage  = 'url(../../images/pkloader.gif)';
        o.style.backgroundRepeat= 'no-repeat';
        o.style.backgroundPosition = 'right';
        PageMethods.validate_FK_DMS_Items_CreatedBy(value, this.validated_FK_DMS_Items_CreatedBy);
      },
    validated_FK_DMS_Items_CreatedBy_main: false,
    validated_FK_DMS_Items_CreatedBy: function(result) {
      var p = result.split('|');
      var o = $get(p[1]);
      if(script_dmsItems.validated_FK_DMS_Items_CreatedBy_main){
        var o_d = $get(p[1]+'_Display');
        try{o_d.innerHTML = p[2];}catch(ex){}
      }
      o.style.backgroundImage  = 'none';
      if(p[0]=='1'){
        o.value='';
        o.focus();
      }
    },
    validate_FK_DMS_Items_ForwardLinkedItemID: function(o,Prefix) {
      var value = o.id;
      var ForwardLinkedItemID = $get(Prefix + 'ForwardLinkedItemID');
      if(ForwardLinkedItemID.value==''){
        if(this.validated_FK_DMS_Items_ForwardLinkedItemID_main){
          var o_d = $get(Prefix + 'ForwardLinkedItemID' + '_Display');
          try{o_d.innerHTML = '';}catch(ex){}
        }
        return true;
      }
      value = value + ',' + ForwardLinkedItemID.value ;
        o.style.backgroundImage  = 'url(../../images/pkloader.gif)';
        o.style.backgroundRepeat= 'no-repeat';
        o.style.backgroundPosition = 'right';
        PageMethods.validate_FK_DMS_Items_ForwardLinkedItemID(value, this.validated_FK_DMS_Items_ForwardLinkedItemID);
      },
    validated_FK_DMS_Items_ForwardLinkedItemID_main: false,
    validated_FK_DMS_Items_ForwardLinkedItemID: function(result) {
      var p = result.split('|');
      var o = $get(p[1]);
      if(script_dmsItems.validated_FK_DMS_Items_ForwardLinkedItemID_main){
        var o_d = $get(p[1]+'_Display');
        try{o_d.innerHTML = p[2];}catch(ex){}
      }
      o.style.backgroundImage  = 'none';
      if(p[0]=='1'){
        o.value='';
        o.focus();
      }
    },
    validate_FK_DMS_Items_LinkedItemID: function(o,Prefix) {
      var value = o.id;
      var LinkedItemID = $get(Prefix + 'LinkedItemID');
      if(LinkedItemID.value==''){
        if(this.validated_FK_DMS_Items_LinkedItemID_main){
          var o_d = $get(Prefix + 'LinkedItemID' + '_Display');
          try{o_d.innerHTML = '';}catch(ex){}
        }
        return true;
      }
      value = value + ',' + LinkedItemID.value ;
        o.style.backgroundImage  = 'url(../../images/pkloader.gif)';
        o.style.backgroundRepeat= 'no-repeat';
        o.style.backgroundPosition = 'right';
        PageMethods.validate_FK_DMS_Items_LinkedItemID(value, this.validated_FK_DMS_Items_LinkedItemID);
      },
    validated_FK_DMS_Items_LinkedItemID_main: false,
    validated_FK_DMS_Items_LinkedItemID: function(result) {
      var p = result.split('|');
      var o = $get(p[1]);
      if(script_dmsItems.validated_FK_DMS_Items_LinkedItemID_main){
        var o_d = $get(p[1]+'_Display');
        try{o_d.innerHTML = p[2];}catch(ex){}
      }
      o.style.backgroundImage  = 'none';
      if(p[0]=='1'){
        o.value='';
        o.focus();
      }
    },
    validate_FK_DMS_Items_BackwardLinkedItemID: function(o,Prefix) {
      var value = o.id;
      var BackwardLinkedItemID = $get(Prefix + 'BackwardLinkedItemID');
      if(BackwardLinkedItemID.value==''){
        if(this.validated_FK_DMS_Items_BackwardLinkedItemID_main){
          var o_d = $get(Prefix + 'BackwardLinkedItemID' + '_Display');
          try{o_d.innerHTML = '';}catch(ex){}
        }
        return true;
      }
      value = value + ',' + BackwardLinkedItemID.value ;
        o.style.backgroundImage  = 'url(../../images/pkloader.gif)';
        o.style.backgroundRepeat= 'no-repeat';
        o.style.backgroundPosition = 'right';
        PageMethods.validate_FK_DMS_Items_BackwardLinkedItemID(value, this.validated_FK_DMS_Items_BackwardLinkedItemID);
      },
    validated_FK_DMS_Items_BackwardLinkedItemID_main: false,
    validated_FK_DMS_Items_BackwardLinkedItemID: function(result) {
      var p = result.split('|');
      var o = $get(p[1]);
      if(script_dmsItems.validated_FK_DMS_Items_BackwardLinkedItemID_main){
        var o_d = $get(p[1]+'_Display');
        try{o_d.innerHTML = p[2];}catch(ex){}
      }
      o.style.backgroundImage  = 'none';
      if(p[0]=='1'){
        o.value='';
        o.focus();
      }
    },
    validate_FK_DMS_Items_ParentItemID: function(o,Prefix) {
      var value = o.id;
      var ParentItemID = $get(Prefix + 'ParentItemID');
      if(ParentItemID.value==''){
        if(this.validated_FK_DMS_Items_ParentItemID_main){
          var o_d = $get(Prefix + 'ParentItemID' + '_Display');
          try{o_d.innerHTML = '';}catch(ex){}
        }
        return true;
      }
      value = value + ',' + ParentItemID.value ;
        o.style.backgroundImage  = 'url(../../images/pkloader.gif)';
        o.style.backgroundRepeat= 'no-repeat';
        o.style.backgroundPosition = 'right';
        PageMethods.validate_FK_DMS_Items_ParentItemID(value, this.validated_FK_DMS_Items_ParentItemID);
      },
    validated_FK_DMS_Items_ParentItemID_main: false,
    validated_FK_DMS_Items_ParentItemID: function(result) {
      var p = result.split('|');
      var o = $get(p[1]);
      if(script_dmsItems.validated_FK_DMS_Items_ParentItemID_main){
        var o_d = $get(p[1]+'_Display');
        try{o_d.innerHTML = p[2];}catch(ex){}
      }
      o.style.backgroundImage  = 'none';
      if(p[0]=='1'){
        o.value='';
        o.focus();
      }
    },
    validate_FK_DMS_Items_ChildItemID: function(o,Prefix) {
      var value = o.id;
      var ChildItemID = $get(Prefix + 'ChildItemID');
      if(ChildItemID.value==''){
        if(this.validated_FK_DMS_Items_ChildItemID_main){
          var o_d = $get(Prefix + 'ChildItemID' + '_Display');
          try{o_d.innerHTML = '';}catch(ex){}
        }
        return true;
      }
      value = value + ',' + ChildItemID.value ;
        o.style.backgroundImage  = 'url(../../images/pkloader.gif)';
        o.style.backgroundRepeat= 'no-repeat';
        o.style.backgroundPosition = 'right';
        PageMethods.validate_FK_DMS_Items_ChildItemID(value, this.validated_FK_DMS_Items_ChildItemID);
      },
    validated_FK_DMS_Items_ChildItemID_main: false,
    validated_FK_DMS_Items_ChildItemID: function(result) {
      var p = result.split('|');
      var o = $get(p[1]);
      if(script_dmsItems.validated_FK_DMS_Items_ChildItemID_main){
        var o_d = $get(p[1]+'_Display');
        try{o_d.innerHTML = p[2];}catch(ex){}
      }
      o.style.backgroundImage  = 'none';
      if(p[0]=='1'){
        o.value='';
        o.focus();
      }
    },
    validate_FK_DMS_Items_StatusID: function(o,Prefix) {
      var value = o.id;
      var StatusID = $get(Prefix + 'StatusID');
      if(StatusID.value==''){
        if(this.validated_FK_DMS_Items_StatusID_main){
          var o_d = $get(Prefix + 'StatusID' + '_Display');
          try{o_d.innerHTML = '';}catch(ex){}
        }
        return true;
      }
      value = value + ',' + StatusID.value ;
        o.style.backgroundImage  = 'url(../../images/pkloader.gif)';
        o.style.backgroundRepeat= 'no-repeat';
        o.style.backgroundPosition = 'right';
        PageMethods.validate_FK_DMS_Items_StatusID(value, this.validated_FK_DMS_Items_StatusID);
      },
    validated_FK_DMS_Items_StatusID_main: false,
    validated_FK_DMS_Items_StatusID: function(result) {
      var p = result.split('|');
      var o = $get(p[1]);
      if(script_dmsItems.validated_FK_DMS_Items_StatusID_main){
        var o_d = $get(p[1]+'_Display');
        try{o_d.innerHTML = p[2];}catch(ex){}
      }
      o.style.backgroundImage  = 'none';
      if(p[0]=='1'){
        o.value='';
        o.focus();
      }
    },
    validate_FK_DMS_Items_ProjectID: function(o,Prefix) {
      var value = o.id;
      var ProjectID = $get(Prefix + 'ProjectID');
      if(ProjectID.value==''){
        if(this.validated_FK_DMS_Items_ProjectID_main){
          var o_d = $get(Prefix + 'ProjectID' + '_Display');
          try{o_d.innerHTML = '';}catch(ex){}
        }
        return true;
      }
      value = value + ',' + ProjectID.value ;
        o.style.backgroundImage  = 'url(../../images/pkloader.gif)';
        o.style.backgroundRepeat= 'no-repeat';
        o.style.backgroundPosition = 'right';
        PageMethods.validate_FK_DMS_Items_ProjectID(value, this.validated_FK_DMS_Items_ProjectID);
      },
    validated_FK_DMS_Items_ProjectID_main: false,
    validated_FK_DMS_Items_ProjectID: function(result) {
      var p = result.split('|');
      var o = $get(p[1]);
      if(script_dmsItems.validated_FK_DMS_Items_ProjectID_main){
        var o_d = $get(p[1]+'_Display');
        try{o_d.innerHTML = p[2];}catch(ex){}
      }
      o.style.backgroundImage  = 'none';
      if(p[0]=='1'){
        o.value='';
        o.focus();
      }
    },
    validate_FK_DMS_Items_WBSID: function(o,Prefix) {
      var value = o.id;
      var WBSID = $get(Prefix + 'WBSID');
      if(WBSID.value==''){
        if(this.validated_FK_DMS_Items_WBSID_main){
          var o_d = $get(Prefix + 'WBSID' + '_Display');
          try{o_d.innerHTML = '';}catch(ex){}
        }
        return true;
      }
      value = value + ',' + WBSID.value ;
        o.style.backgroundImage  = 'url(../../images/pkloader.gif)';
        o.style.backgroundRepeat= 'no-repeat';
        o.style.backgroundPosition = 'right';
        PageMethods.validate_FK_DMS_Items_WBSID(value, this.validated_FK_DMS_Items_WBSID);
      },
    validated_FK_DMS_Items_WBSID_main: false,
    validated_FK_DMS_Items_WBSID: function(result) {
      var p = result.split('|');
      var o = $get(p[1]);
      if(script_dmsItems.validated_FK_DMS_Items_WBSID_main){
        var o_d = $get(p[1]+'_Display');
        try{o_d.innerHTML = p[2];}catch(ex){}
      }
      o.style.backgroundImage  = 'none';
      if(p[0]=='1'){
        o.value='';
        o.focus();
      }
    },
    validate_FK_DMS_Items_ActionBy: function(o,Prefix) {
      var value = o.id;
      var ActionBy = $get(Prefix + 'ActionBy');
      if(ActionBy.value==''){
        if(this.validated_FK_DMS_Items_ActionBy_main){
          var o_d = $get(Prefix + 'ActionBy' + '_Display');
          try{o_d.innerHTML = '';}catch(ex){}
        }
        return true;
      }
      value = value + ',' + ActionBy.value ;
        o.style.backgroundImage  = 'url(../../images/pkloader.gif)';
        o.style.backgroundRepeat= 'no-repeat';
        o.style.backgroundPosition = 'right';
        PageMethods.validate_FK_DMS_Items_ActionBy(value, this.validated_FK_DMS_Items_ActionBy);
      },
    validated_FK_DMS_Items_ActionBy_main: false,
    validated_FK_DMS_Items_ActionBy: function(result) {
      var p = result.split('|');
      var o = $get(p[1]);
      if(script_dmsItems.validated_FK_DMS_Items_ActionBy_main){
        var o_d = $get(p[1]+'_Display');
        try{o_d.innerHTML = p[2];}catch(ex){}
      }
      o.style.backgroundImage  = 'none';
      if(p[0]=='1'){
        o.value='';
        o.focus();
      }
    },
    temp: function() {
    }
    }
