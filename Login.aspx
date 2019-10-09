<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>DMS: Login</title>
  <meta charset="utf-8"/>
  <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
  <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate"/>
  <meta http-equiv="Pragma" content="no-cache"/>
  <meta http-equiv="Expires" content="0"/>
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <link rel="SHORTCUT ICON" type="image/x-icon" runat="server" href="~/isgec.ico" />
  <script src="/../UserRights/jq3.3/jquery-3.3.1.min.js"></script>
  <link href="App_Resources/fa-5.9.0/css/all.css" rel="stylesheet" />
  <link rel="stylesheet" href="/../UserRights/bs4.0/css/bootstrap.min.css" />
  <script src="/../UserRights/Popper1.0/Popper.min.js"></script>
  <script src="/../UserRights/bs4.0/js/bootstrap.min.js"></script>
<script>
 $(document).ready(function(){
 
 var imgArr = new Array( // relative paths of images
 'Banners/new_banner_home1.jpg',
 'Banners/new_banner_home2.jpg',
 'Banners/new_banner_home3.jpg',
 'Banners/new_banner_home4.jpg',
 'Banners/new_banner_home5.jpg',
 'Banners/new_banner_home6.jpg',
 'Banners/new_banner_home7.jpg',
 'Banners/new_banner_home8.jpg',
 'Banners/new_banner_home9.jpg'
 );
 
 var preloadArr = new Array();
 var i;
 
 /* preload images */
 for(i=0; i < imgArr.length; i++){
 preloadArr[i] = new Image();
 preloadArr[i].src = imgArr[i];
 }
 
 var currImg = 1;
 var intID = setInterval(changeImg, 10000);
 
 /* image rotator */
 function changeImg(){
 $('#topp').animate({opacity: 0}, 3000, function(){
   $(this).css({ 'background': 'url(' + imgArr[currImg++ % imgArr.length] + ')','background-size':'contain', 'background-repeat':'no-repeat', 'max-height':'600px', 'max-width':'1350px', 'height':'600px', 'width':'1350px'});
 }).animate({opacity: 1}, 3000);
 }
 
 });
</script>

</head>
<body>
  <form id="form1" runat="server">
          <div style="width:300px;margin:50px auto auto 65%;z-index:1;position:absolute;" class="btn btn-warning">
            <div class="row">
              <div class="col-sm-12 text-center" style="height:70px;">
                <img alt="ISGEC" height="100" src="Banners/logo.png" />
              </div>
            </div>
            <div class="row">
              <div class="col-sm-12">
                <asp:Login ID="Login0" OnLoggedIn="LoggedIn" Width="100%" runat="server">
                  <LayoutTemplate>
                    <asp:Panel ID="pnlLogin" runat="server" DefaultButton="cmdLogin">
                      <div class="form-group">
                        <label for="UserName">Login ID:</label>
                        <asp:TextBox CssClass="form-control" ID="UserName" ClientIDMode="Static" runat="server" required="required" MaxLength="8" ValidationGroup="login" />
                      </div>
                      <div class="form-group">
                        <label for="Password">Password:</label>
                        <asp:TextBox TextMode="Password" class="form-control" ID="Password" ClientIDMode="Static" runat="server" required="required" MaxLength="20" ValidationGroup="login" />
                      </div>
                      <div class="row text-center">
                        <div class="col">
                          <asp:Button ID="cmdLogin" CommandName="Login" ClientIDMode="Static" CssClass="btn btn-sm btn-primary" runat="server" Text="Sign In" ValidationGroup="login" />
                        </div>
                      </div>
                    </asp:Panel>
                  </LayoutTemplate>
                </asp:Login>
              </div>
            </div>
          </div>

    <style>
      @keyframes cf3FadeInOut {
        0% {
          opacity: 1;
        }

        45% {
          opacity: 1;
        }

        55% {
          opacity: 0;
        }

        100% {
          opacity: 0;
        }
      }

      #cf3 img.top {
        animation-name: cf3FadeInOut;
        animation-timing-function: ease-in-out;
        animation-iteration-count: infinite;
        animation-duration: 6s;
        animation-direction: alternate;
      }

      #cf3 img {
        position: absolute;
        max-width: 1350px;
        max-height: 600px;
      }
    </style>
    <div class="container" id="cf3">
      <div class="row">
        <div class="col-sm-12">
          <div style="background: url('Banners/new_banner_home1.jpg'); background-size: contain; background-repeat: no-repeat; max-height: 600px; max-width: 1350px; height: 600px; width: 1350px;">
            <div id="topp" style="background: url('Banners/new_banner_home1.jpg'); background-size: contain; background-repeat: no-repeat; max-height: 600px; max-width: 1350px; height: 600px; width: 1350px;">
              <div style="margin: 20px auto auto 5%; color: white; float: left;">
                <strong style="color:gold;text-shadow:3px 3px 3px orangered;">
                  <h2>ISGEC-Document Management System</h2>
                </strong>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

  </form>
</body>
</html>
