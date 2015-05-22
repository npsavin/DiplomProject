<%@ Page Title="Добавление в базу" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ontology.aspx.cs" Inherits="ExpertSystem.About" %>
<%@ PreviousPageType VirtualPath="~/Default.aspx" %> 

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <asp:TextBox ID="TextBox2" runat="server" Height="65px" Width="386px"></asp:TextBox>
    <br />
    <asp:Button ID="Button2" runat="server" OnClick="Yes_click" Text="Верно" />
    <br />
    
    <asp:TextBox ID="TextBox1" runat="server" Height="79px" Width="385px"></asp:TextBox>
    
    <br />
    <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Создать новую" />
    
    </asp:Content>
