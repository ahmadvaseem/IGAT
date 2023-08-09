<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Games.aspx.cs" Inherits="igat.com.Games" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="appiontment-widget agileits w3layouts" style="margin-left:0%; width:70%;">

						<div class="header agileits w3layouts" >
    <asp:Repeater ID="rptGenreGame" runat="server">
            <HeaderTemplate></HeaderTemplate>
            <ItemTemplate>
                <div>
                    <asp:HiddenField ID="hdnId" Value='<%# Eval("Id") %>' runat="server"/>
                     <asp:HyperLink style='font-size:40px;color:white; padding: 4%;margin: 0px !important;' ID="HyperLink1" runat="server" Text='<%# Eval("Name") %>'  NavigateUrl='<%#"http://localhost:49211/Details.aspx?ID=" +  Eval("ID") %>' >  </asp:HyperLink>
                       <asp:Image ID="imgGameImage" runat="server" ImageUrl='<%# Eval("ImageAdress") %>' AlternateText="This Image"  alt="Collective UI Kit"></asp:Image>
                   
                </div>
                </br>
                 <div class="clear"></div>
            </ItemTemplate>
        </asp:Repeater>
                            </div>
        </div>
    </asp:Content>
