<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeBehind="Genre.aspx.cs" Inherits="igat.com.Genre" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
        <div class="col-articles">    
        <asp:Repeater ID="rptGenres" runat="server" OnItemCommand="rptGames_ItemCommand">
            <HeaderTemplate>
             <div>   <!--<table border="1" width="100%">-->
            </HeaderTemplate>
            <ItemTemplate>
                
                
                    
                    
                     <div style="display:inline-block;float:left; margin-right:20px;border:solid 2px #000;height:70px;"> 
                     <asp:HiddenField ID="hdnId" Value='<%# Eval("Id") %>' runat="server"/>
                     <asp:HyperLink  style="color:lightgray; font-size:25px" ID="HyperLink1" runat="server" Text='<%# Eval("Genre") %>'  NavigateUrl='<%#"http://localhost:49211/Games.aspx?ID=" +  Eval("ID") %>' >  </asp:HyperLink>
                         <asp:Image ID="imgGameImage" runat="server" ImageUrl='<%#@"Resources\GenreImages\" + Eval("Genre") + ".jpeg"%>' AlternateText="This Image"></asp:Image>  
                    </div>
                <div class="clear"></br></div>
                 
                        
                    
            </ItemTemplate>
            <FooterTemplate><!--</table>--></div></FooterTemplate>
        </asp:Repeater>
            </div>
   
</asp:Content>