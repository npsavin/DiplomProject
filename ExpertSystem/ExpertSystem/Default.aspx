<%@ Page Title="Экспертная система" Language="C#" MasterPageFile="~/Site.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ExpertSystem.Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

   

    <div>
       
       
        <br />
&nbsp;<asp:Button ID="Button1" runat="server" OnClick="Translate" Text="Информативные признаки" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
        <br />
       
       
        <asp:TextBox ID="TextBox1" runat="server" Height="602px" Width="741px"></asp:TextBox>
&nbsp;<asp:TextBox ID="TextBox7" runat="server" Height="300px" Width="200px" ></asp:TextBox>

        &nbsp;<br />
        <br />
        &nbsp;<asp:FileUpload ID="FileUpload1" runat="server" />
        <br />
       
        <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Загрузить" />
        <asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="Сохранить" />
        <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="Сохранить в базу знаний" />
       
        <br />
        <asp:Button ID="Button5" runat="server" OnClick="Button5_Click" Text="Добавить стоп-слово" />
       
        <asp:TextBox ID="TextBox20" runat="server"></asp:TextBox>
       
    </div>

</asp:Content>
