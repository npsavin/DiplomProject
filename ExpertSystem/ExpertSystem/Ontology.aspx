<%@ Page Title="Добавление в базу" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Ontology.aspx.cs" Inherits="ExpertSystem.About" %>
<%@ PreviousPageType VirtualPath="~/Default.aspx" %> 

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <asp:TextBox ID="TextBox2" runat="server" Height="108px" Width="448px"></asp:TextBox>
    <br />
    <asp:Button ID="Button2" runat="server" OnClick="Yes_click" Text="Верно" />
    <br />
    <asp:TextBox ID="TextBox3" runat="server" Height="84px" Width="447px"></asp:TextBox>
    <br />
    <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="Выбрать" />
    <br />
    
    <asp:TextBox ID="TextBox1" runat="server" Height="105px" Width="446px"></asp:TextBox>
    
    <br />
    <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Создать новую" />
    
    </asp:Content>
