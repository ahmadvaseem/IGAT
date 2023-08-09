<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AllLinks.aspx.cs" Inherits="igat.com.AllLinks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>IGAT</title>
<meta http-equiv="Content-type" content="text/html; charset=utf-8" />
<link rel="stylesheet" href="Resources/css/style.css" type="text/css" media="all" />
<!--[if IE 6]><link rel="stylesheet" href="css/ie6-style.css" type="text/css" media="all" /><![endif]-->
<script src="js/jquery-1.3.2.min.js" type="text/javascript"></script>
<script src="js/fns.js" type="text/javascript"></script>
</head>
<body>
    <form runat="server">
    <div id="page" class="shell">
  <!-- Header -->
  <div id="header">
    <div id="top-nav">
      <ul>
        <li class="home"><a href="">home</a></li>
        <li><a href="">pc</a></li>
        <li><a href="">xbox</a></li>
        <li><a href="">360</a></li>
        <li><a href="">wii</a></li>
        <li><a href="">ps3</a></li>
        <li><a href="">ps2</a></li>
        <li><a href="">psp</a></li>
        
      </ul>
    </div>
    <!-- / Top Navigation -->
    <div class="cl">&nbsp;</div>
    <!-- Logo -->
    <div id="logo">
      <h1><a href="">IG<span>AT</span></a></h1>
      <p class="description">Game Ratings</p>
    </div>
       <div id="main-nav">
      <div class="bg-right">
        <div class="bg-left">
          <ul>
            <li><a href="">community</a></li>
            <li><a href="">forum</a></li>
            <li><a href="">video</a></li>
            <li><a href="">features</a></li>
            <li><a href="">downloads</a></li>
          </ul>
        </div>
      </div>
    </div>
      <!-- / Main Navigation -->
    <div class="cl">&nbsp;</div>
    <!-- Sort Navigation -->
    <div id="sort-nav">
      <div class="bg-right">
        <div class="bg-left">
          <div class="cl">&nbsp;</div>
          <ul>
            <li class="first active first-active"><a href="">Home</a><span class="sep">&nbsp;</span></li>
            <li><a href="ReviewPage.aspx">Reviews</a><span class="sep">&nbsp;</span></li>
             <li><a href="AllLinks.aspx">All Links </a><span class="sep">&nbsp;</span></li>
            <li><a href="AllGames.aspx">All Games</a><span class="sep">&nbsp;</span></li>
          </ul>
          <div class="cl">&nbsp;</div>
        </div>
      </div>
    </div>
    <!-- / Sort Navigation -->
  </div>
        <div class="block">
        <div class="block-bot">
        <div class="head">
            <div class="head-cnt">
                <div class="head-cnt"> <asp:Button class="view-all" id="Button1" Text="View All" runat="server" OnClick="Button1_Click" />
            
              <h3>All Links</h3>
              <div class="cl">&nbsp;</div>
            </div>
          </div>
        </div>
          </div>

        <div>
    
    <asp:Table id="Table1" 
        HorizontalAlign="left" 
        Font-Names="Verdana" 
        Font-Size="10pt" 
        CellPadding="15" 
        CellSpacing="5" 
        Runat="server"
        Width="55%"/>
    </div>





        <!-- / Sidebar -->
    <div class="cl">&nbsp;</div>
        <!-- Footer -->
    <div id="footer">
      <div class="navs">
        <div class="navs-bot">
          <div class="cl">&nbsp;</div>
          <ul>
            <li><a href="">community</a></li>
            <li><a href="">forum</a></li>
            <li><a href="">video</a></li>
            <li><a href="">features</a></li>
            <li><a href="">downloads</a></li>
          </ul>
          <ul>
            <li><a href="">pc</a></li>
            <li><a href="">xbox</a></li>
            <li><a href="">360</a></li>
            <li><a href="">wii</a></li>
            <li><a href="">ps3</a></li>
            <li><a href="">ps2</a></li>
            <li><a href="">psp</a></li>
            <li><a href="">ds</a></li>
          </ul>
          <div class="cl">&nbsp;</div>
        </div>
      </div>
      <p class="copy">&copy; igat.com. Design by <a href="">MAK Developers</a></p>
    </div>
    <!-- / Footer -->
  </div>
</div>
<!-- / Main -->
</div>
        </form>
</body>
</html>

