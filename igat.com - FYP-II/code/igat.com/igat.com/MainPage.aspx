<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master"  CodeBehind="MainPage.aspx.cs" Inherits="igat.com.MainPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
			<!-- Default-JavaScript --> 
    <script type="text/javascript" src="Resources//js//jquery-2.1.4.min.js"></script>

			<!-- Slider-JavaScript -->
				<script src="Resources//js//responsiveslides.min.js"></script>
				<script>
					$(function () {
						$("#slider, #slider2").responsiveSlides({
							auto: true,
							nav: true,
							speed: 1500,
							namespace: "callbacks",
							pager: true,
						});
					});
				</script>
			<!-- //Slider-JavaScript -->

    			<!-- Horizontal-Tabs-JavaScript -->
				<script src="Resources//js//easyResponsiveTabs.js" type="text/javascript"></script>
				<script type="text/javascript">
				$(document).ready(function () {
					$('#horizontalTab').easyResponsiveTabs({
						type: 'default',
						width: 'auto',
						fit: true,
					});
				});
				</script>
			<!-- //Horizontal-Tabs-JavaScript -->


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Slider -->
    <div class="slider agileits w3layouts" style="width: 74% !important; height: auto; float: left;">

        <ul class="rslides agileits w3layouts" id="slider">
            <li>
                <img src="Resources//images//slide-1.jpg" alt="Image" />
            </li>
            <li>
                <img src="Resources//images//slide-2.jpg" alt="Image" />
            </li>
            <li>
                <img src="Resources//images//slide-3.jpg" alt="Image" />
            </li>
            <li>
                <img src="Resources/images/slide-4.jpg" alt="Image" />
            </li>
            <li>
                <img src="Resources//images/slide-5.jpg" alt="Image" />
            </li>
        </ul>

    </div>
    <!-- //Slider -->

    <!-- Scrolling-Articles -->
    <div class="activity_box agileits w3layouts">
        <h3>Trending</h3>
        <div class="scrollbar agileits w3layouts" id="style-2" style="    height: auto !important; max-height: 600px !important;">
            <div class="activity-row agileits w3layouts">
                <asp:Repeater ID="rptPhtots" runat="server">
            <ItemTemplate>
                <div>
                <div class="activity-img agileits w3layouts">
     
                <div >
                    <asp:HiddenField ID="hdnId" Value='<%# Eval("Id") %>' runat="server"/>

                    <asp:Image ID="imgGameImage" runat="server" ImageUrl='<%# Eval("ImageAdress") %>' AlternateText="Image" ></asp:Image>

                </div>
                </div>
                <div class="activity-desc agileits w3layouts">
                        <h5> <asp:HyperLink  ID="lblTopGame" runat="server" Text='<%# Eval("Name") %>'  NavigateUrl = '<%#"http://localhost:49211/Details.aspx?ID=" +  Eval("Id") %>' >   </asp:HyperLink></h5>
                    
                    
                </div>
                 </div>
                <div class="clear"></div></br>
              </ItemTemplate>
        </asp:Repeater>
                
            
                
            </div>
        </div>
    </div>
    <!-- //Scrolling-Articles -->

</asp:Content>
