<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="igat.com.Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
      /*  span.rating {
            background: url("Resources/images/stars.png") top left;
        }*/

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    					<!-- Appointment-Widget -->
					<div class="appiontment-widget agileits w3layouts" style="margin-left:0%; width:70%;">

						<div class="header agileits w3layouts" >
      <asp:Repeater ID="rptGameName" runat="server" OnItemCommand="rptGameName_ItemCommand">
            <ItemTemplate>
                <div >
                    <asp:HiddenField ID="hdnId" Value='<%# Eval("Id") %>' runat="server"/>
                    
                    <asp:Label style='font-size:40px;color:white; padding: 4%;margin: 0px !important;' ID="lblGameName" Text='<%# Eval("Name") %>' runat="server"></asp:Label> 
                    
                    <div class="cl">&nbsp;</div>
                </div>
              </ItemTemplate>
        </asp:Repeater>
 

                <div >
                    
                    <asp:Image ID="imgGameImage" runat="server" ImageUrl='' AlternateText="This Image"></asp:Image>
						       
                    <div class="cl">&nbsp;</div>
                </div>
                             
                            	
						
                        </div>

                                


						<div class="reception agileits w3layouts">
                          
                            <div class="rating">
                                        
                              <h2 class="text-left">Rating :</h2> 
                              <div style="margin-left:175px;"><span class="" id="spnRating" runat="server"></span></div> <asp:Label style='font-size:30px;font-weight:bold;color:#fff;' ID="lblRate" runat="server" Text='Rating:'></asp:Label><br/>

        
                <div>
                    
                    
                    <asp:Label style='font-size:20px;color:coral;' ID="lblRating" runat="server" Text='<%# Eval("Rating") %>'></asp:Label>
                    <div class="cl">&nbsp;</div>
                </div>
   
                               
        <div>
                                    </div>
                            </div>
						</div>
                        <div class="clear"></div>

						<div class="links agileits w3layouts">
							<div class="appt agileits w3layouts">
								<p class="word agileits w3layouts"><asp:Label style='font-size:20px;' ID="lblGraphic" runat="server"  Text='Graphics'></asp:Label>
                            </p>
								<p class="number agileits w3layouts"><asp:Label style='font-size:20px;color:coral;' ID="lblGraphics" runat="server"  Text=""></asp:Label></p>
							</div>
							<div class="due agileits w3layouts" >
								<p class="word agileits w3layouts" "><asp:Label style='font-size:20px;' ID="lblGp" runat="server" Text='Gameplay'></asp:Label>
                            </p>
								<p class="number agileits w3layouts"><asp:Label style='font-size:20px;color:coral;' ID="lblGameplay" runat="server" Text=''></asp:Label></p>
							</div>
							<div class="task agileits w3layouts">
								<p class="word agileits w3layouts" ><asp:Label style='font-size:20px;' ID="lblPr" runat="server"  Text='Performance'></asp:Label>
                            </p>
								<p class="number agileits w3layouts"><asp:Label style='font-size:20px;color:coral;' ID="lblPerformance"  runat="server" Text=''></asp:Label></p>
							</div>
							<div class="clear"></div>
						</div>
                        <div class="menu agileits w3layouts">
							
						</div>

                        <div class="links agileits w3layouts">
							<div class="appt agileits w3layouts">
								<p class="word agileits w3layouts">Platform</p>
								<p class="number agileits w3layouts">
                                    <asp:Label style='color:coral;' ID="lblPlatform" runat="server" Text=''></asp:Label>
								</p>
							</div>
							<div class="due agileits w3layouts" >
								<p class="word agileits w3layouts" ">Developer</p>
								<p class="number agileits w3layouts">
                                    <asp:Label style='color:coral;' ID="lblDeveloper" runat="server" Text=''></asp:Label>
								</p>
							</div>
							<div class="task agileits w3layouts">
								<p class="word agileits w3layouts" >Genre</p>
								<p class="number agileits w3layouts">
                                    <asp:Label style='color:coral;' ID="lblGenre" runat="server" Text=''></asp:Label>
								</p>
							</div>
                            <div class="task agileits w3layouts">
								<p class="word agileits w3layouts" >Release Date</p>
								<p class="number agileits w3layouts">
                                    <asp:Label style='color:coral;' ID="lblReleaseDate" runat="server" Text=''></asp:Label>
								</p>
							</div>
							<div class="clear"></div>
						</div>
    					<div class="menu agileits w3layouts">
							
							<form>

                                <p> 
                                    <asp:Label style='color:coral;' ID="lblDescription" runat="server" Text=''></asp:Label>
                                </p>
							</form>
						</div>

					</div>
					<!-- //Appointment-Widget -->

    					<!-- Chat-Widget -->
					<div class="chat-widget agileits w3layouts" style="display:inline;float:right; width:28%; margin-left:0%;">
						<div id="chatbox">

							<div id="friendslist">

								<div id="topmenu">
									<span class="friends agileits w3layouts">Suggestions</span>
								</div>

								<div id="friends" >

									<div class="friend agileits w3layouts">
  <asp:Repeater ID="rptRecommond" runat="server">
            <ItemTemplate>
                <div  style="border:solid 1px #fff;">
                <div class="activity-img agileits w3layouts">
     
                <div >
                    <asp:HiddenField ID="hdnId" Value='<%# Eval("Id") %>' runat="server"/>

                    <asp:Image ID="imgGameImage" runat="server" ImageUrl='<%# Eval("ImageAdress") %>' AlternateText="This Image"  style="width: 50px;height: 40px;" alt="Collective UI Kit"></asp:Image>
                    
                </div>
                </div>
                <div class="activity-desc agileits w3layouts">
                        <h5 style="margin-top:15px;"> <asp:HyperLink  ID="lblTopGame" runat="server" Text='<%# Eval("Name") %>'  style="color:white" NavigateUrl = '<%#"http://localhost:49211/Details.aspx?ID=" +  Eval("Id") %>' >   </asp:HyperLink></h5>
                    
                    
                </div>
                    </div>
                <div class="clear"></div>
              </ItemTemplate>
        </asp:Repeater>
								</div>

							</div>


						</div>
					</div>
					<!-- //Chat-Widget -->


</asp:Content>