<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Stars.aspx.cs" Inherits="igat.com.Stars" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
     span.rating {
     background: url("stars.png") top left;
     /* background: url("stars-blue.png") top left; */
     display: inline-block;
     height: 35px;
     width: 370px;
     overflow: hidden;
     text-indent: -9999px;
    }
   span.r0 {background-position:  -360px 0px;}
   span.r1 {background-position:  -324px 0px;}
   span.r2 {background-position:  -288px 0px;width:368px;}
   span.r3 {background-position:  -252px 0px;width:368px;}
   span.r4 {background-position:  -216px 0px;width:367px;}
   span.r5 {background-position:  -180px 0px;width:366px;}
   span.r6 {background-position:  -144px 0px;width:366px;}
   span.r7 {background-position:  -108px 0px;width:364px;}
   span.r8 {background-position:  -72px 0px;width:364px;}
   span.r9 {background-position:  -36px 0px;width:362px;}
   span.r10 {background-position:  0px 0px;width:362px;}
   span.r05 {background-position:  -360px 38px;}
   span.r15 {background-position:  -324px 38px;}
   span.r25 {background-position:  -288px 38px;width:368px;}
   span.r35 {background-position:  -252px 38px;width:368px;}
   span.r45 {background-position:  -216px 38px;width:367px;}
   span.r55 {background-position:  -180px 38px;width:366px;}
   span.r65 {background-position:  -144px 38px;width:366px;}
   span.r75 {background-position:  -108px 38px;width:364px;}
   span.r85 {background-position:  -72px 38px;width:364px;}
   span.r95 {background-position:  -36px 38px;width:362px;}
  </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
          <span class="rating r0">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r1">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r2">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r3">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r4">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r5">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r6">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r7">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r8">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r9">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r10">Rating : 0.5 star out of 5</span><br/>
    
         <span class="rating r05">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r15">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r25">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r35">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r45">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r55">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r65">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r75">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r85">Rating : 0.5 star out of 5</span><br/>
        <span class="rating r95">Rating : 0.5 star out of 5</span><br/>

    </div>
    </form>
</body>
</html>
