<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IvnMgmt_ASPNET._Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
	    function DisableEnable() {
	        var trxTypeList = document.getElementById("<%= trxTypeList.ClientID %>")
	        var txtAreaPerUnit = document.getElementById("<%= txtAreaPerUnit.ClientID %>")
	        var txtUnitPrice = document.getElementById("<%= txtUnitPrice.ClientID %>")
	        var txtNoOfItems = document.getElementById("<%= txtNoOfItems.ClientID %>")

	        if (trxTypeList.options[trxTypeList.selectedIndex].text != "Insert New Item") {
	        	document.getElementById("<%= txtAreaPerUnit.ClientID %>").value= "";
	        	document.getElementById("<%= txtUnitPrice.ClientID %>").value = "";
	        	txtAreaPerUnit.disabled = true;
	            txtUnitPrice.disabled = true;
	        	if (trxTypeList.options[trxTypeList.selectedIndex].text == "Delete Item") {
	        		document.getElementById("<%= txtNoOfItems.ClientID %>").value = "";
	        		txtNoOfItems.disabled = true;	
	        	}
	        	else {
	        		txtNoOfItems.disabled = false;	
	        	}	            
	        }

	        else {
	            txtAreaPerUnit.disabled = false;
	            txtUnitPrice.disabled = false;
	            txtNoOfItems.disabled = false;	
	        }
	    }
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div style="text-align: center">
            <asp:Label ID="inventoryApp" Text="Inventory Management System" runat="server" Font-Bold="true"
                Width="100%" BackColor="LightBlue"></asp:Label></div>
        <div style="width: 100%">
            <br />
            <br />
            <div style="float: left; width: 40%">
                <table style="width: 100%; border-style: solid; border-color: Gray">
                    <tr style="text-align: center">
                        <td>
                            <asp:Label ID="label4" Text="Total Area" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label1" Text="Warning Limit" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label2" Text="Error Limit" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label3" Text="Total Used Area" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                    <tr style="text-align: center">
                        <td>
                            <asp:Label ID="lblTotalArea" Text="5000" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblWarningLimit" Text="4500" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblErrorLimit" Text="4700" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblTotalUsedArea" Text="0" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <table style="width: 75%">
                    <tr align="center">
                        <td>
                            <asp:Label ID="Label5" Text="Warning Limit" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtWarningLimit" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <asp:Label ID="Label6" Text="Error Limit" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtErrorLimit" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div style="margin-left: 34.5%; margin-top: 1%">
                    <asp:Button ID="btnUpdateSpaceDetails" runat="server" OnClick="btnUpdateSpaceDetails_Click"
                        Width="175px" Text="Update Space Details" /></div>
                <br />
                <br />
                <table style="width: 70%">
                    <tr align="center">
                        <td>
                            <asp:Label ID="Label7" Text="Item Name" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemName" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <asp:Label ID="Label8" Text="No. of Items" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoOfItems" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <asp:Label ID="Label9" Text="Transaction Type" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="trxTypeList" runat="server" onchange="DisableEnable();" Width="170px">
			                <asp:ListItem Text="Select:" Value="0"></asp:ListItem>
			                <asp:ListItem Text="Insert New Item" Value="New"></asp:ListItem>
			                <asp:ListItem Text="Add units" Value="Added"></asp:ListItem>
			                <asp:ListItem Text="Remove units" Value="Removed"></asp:ListItem>
			                <asp:ListItem Text="Delete Item" Value="Delete"></asp:ListItem>
				            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <asp:Label ID="Label10" Text="Area Per Unit" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAreaPerUnit" runat="server" Enabled=false></asp:TextBox>
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            <asp:Label ID="Label11" Text="Unit Price" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtUnitPrice" runat="server" Enabled=false></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div style="margin-left: 43%; margin-top: 1%">
                    <asp:Button ID="btnInsert" runat="server" OnClick="btnInsert_Click" Text="Submit" />
                </div>
            </div>
            <div style="float: right; width: 50%">
                <asp:DataGrid ID="dg_InventoryManagement" runat="server" PageSize="5" AllowPaging="False" DataKeyField="Item_Name"
                    AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" style="margin-left:10%"
                    OnSelectedIndexChanged = "dg_InventoryManagement_SelectionIndexChanged">
                    <Columns>
                        <asp:BoundColumn HeaderText="Item Name" DataField="Item_Name"></asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Area Per Unit" DataField="Area_Per_Unit"></asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Unit Price" DataField="Unit_Price"></asp:BoundColumn>
                        <asp:BoundColumn HeaderText="Available Units" DataField="Available_Units"></asp:BoundColumn>                       
                    </Columns>
                    <FooterStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                    <SelectedItemStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                    <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" Mode="NumericPages" />
                    <AlternatingItemStyle BackColor="White" />
                    <ItemStyle BackColor="#FFFBD6" ForeColor="#333333" />
                    <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" />
                </asp:DataGrid>
            </div>
        </div>
        <br />
        <br />
        <div style="margin-top: 24%">
            <asp:Label ID="Label12" Text="Inverntory Management System" runat="server" ForeColor="LightBlue"
                Width="100%" BackColor="LightBlue"></asp:Label></div>
    </div>
    </form>
</body>
</html>
