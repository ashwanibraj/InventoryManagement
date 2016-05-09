using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using DataGatherer = InvMgmt_ASPNET.DataGatherer;
using InvEventHandler = InvMgmt_ASPNET.InvEventHandler;

namespace IvnMgmt_ASPNET
{
	public partial class _Default : System.Web.UI.Page
	{
		int warningLimit = 0;
		int errorLimit = 0;

		protected void Page_Load(object sender, EventArgs e)
		{
			//InitializeComponent();
			DataGatherer db_obj = new DataGatherer();
			db_obj.initializeDB();
			DisplayData();
		}

		//Display Data in DataGridView  
		private void DisplayData()
		{
			warningLimit = Convert.ToInt32(lblWarningLimit.Text);
			errorLimit = Convert.ToInt32(lblErrorLimit.Text);

			DataGatherer db_obj = new DataGatherer();
			dg_InventoryManagement.DataSource = db_obj.getInventoryData();
			dg_InventoryManagement.DataBind();

			lblTotalUsedArea.Text = calculateCurrentTotalArea("isInitialLoad").ToString();
		}

		public void btnInsert_Click(object sender, EventArgs e)
		{
			int currentTotalArea = 0;

			DataTable dataTable = (DataTable)dg_InventoryManagement.DataSource;
			int length = dataTable.Rows.Count;

			String trxType = trxTypeList.SelectedValue;

			if (txtItemName.Text.Trim() != "" && 
				(
					(trxType == "New" && txtNoOfItems.Text.Trim() != "" &&	txtAreaPerUnit.Text.Trim() != "" && txtUnitPrice.Text.Trim() != "") ||
					(trxType == "Added" && txtNoOfItems.Text.Trim() != "") ||
					(trxType == "Removed" && txtNoOfItems.Text.Trim() != "") ||
					(trxType == "Delete")
				)
			)
			{
				int areaPerUnit = 0;
				int noOfItems = 0;
				int unitPrice = 0;

				if (!String.IsNullOrEmpty(txtAreaPerUnit.Text.Trim()))
					areaPerUnit = Convert.ToInt32(txtAreaPerUnit.Text.Trim());

				if (!String.IsNullOrEmpty(txtNoOfItems.Text.Trim()))
					noOfItems = Convert.ToInt32(txtNoOfItems.Text.Trim());

				if (!String.IsNullOrEmpty(txtUnitPrice.Text.Trim()))
					unitPrice = Convert.ToInt32(txtUnitPrice.Text.Trim());

				if (trxType == "Removed" || trxType == "Delete") {	
					
					InvEventHandler ev = new InvEventHandler ();
					String status = ev.removeFromInventory (trxType, dataTable, txtItemName.Text.Trim(), noOfItems);

					if (status == "InsufficientUnits") {
						MessageBox ("Not enough Available units for mentioned item.");
					} else if (status == "ItemNotFound") {
						MessageBox ("Requested item not in Inventory.");
					} else if (status == "Success") {
						DisplayData ();

						currentTotalArea = calculateCurrentTotalArea ("isDeleted");
						lblTotalUsedArea.Text = currentTotalArea.ToString ();

						MessageBox ("Units removed for selected item.");
					} else {
						MessageBox ("Unexpected error while removing units.");
					}

					ClearFields ();

				} else if (trxType == "Added" || trxType == "New") {

					bool isItemFound = false;
						
					for (int i = 0; i < length; i++) {
						if (Convert.ToString (dataTable.Rows [i] [0]) == txtItemName.Text.Trim()) {
							isItemFound = true;
							areaPerUnit = Convert.ToInt32 (dataTable.Rows [i] [1]);
						}
					}

					if (isItemFound && trxType == "New") {	
						MessageBox ("An item with same name exists. Enter a different item name.");
						ClearFields ();
						return;
					} 

					if (!isItemFound && trxType == "Added")	{						
						MessageBox ("Entered item does not exist. Please insert new item before adding units.");
						ClearFields ();
						return; 
					}

					currentTotalArea = Convert.ToInt32(lblTotalUsedArea.Text) + areaPerUnit*noOfItems;

					if (currentTotalArea >= errorLimit) {
						MessageBox("The total square footage occupied is exceeding the Error Limit! Please do the necessary changes.");
						ClearFields ();
						return;
					} else {
						InvEventHandler ev = new InvEventHandler ();
						bool isInserted = ev.addToInventory(trxType, txtItemName.Text.Trim(), noOfItems, areaPerUnit, unitPrice);

						if (isInserted) {
							lblTotalUsedArea.Text = currentTotalArea.ToString ();
							dg_InventoryManagement.DataSource = dataTable;
							if (currentTotalArea > warningLimit) {
								MessageBox ("Record Inserted Successfully with Warning: The total square footage occupied is exceeding the Warning Limit!");
							} else {
								MessageBox ("Record Inserted Successfully.");
							}
						} else {
							MessageBox ("Unexpected error while inserting records.");
						}
						DisplayData ();
						ClearFields ();
					}
				}
			}
			else
			{
				trxTypeList.ClearSelection();
				MessageBox("Please Provide Details!");
			}
		}


		public void btnUpdateSpaceDetails_Click(object sender, EventArgs e)
		{
			int tempErr = errorLimit;
			int tempWarn = warningLimit;
			int totalArea = Convert.ToInt32 (lblTotalArea.Text);

			if (txtErrorLimit.Text.Trim() != "" || txtWarningLimit.Text.Trim() != "")
			{
				if (txtErrorLimit.Text.Trim() != "")
				{
					tempErr = Convert.ToInt32 (txtErrorLimit.Text.Trim());
				}
				if (txtWarningLimit.Text.Trim() != "")
				{
					tempWarn = Convert.ToInt32 (txtWarningLimit.Text.Trim());
				}

				if (tempErr < tempWarn) {
					ClearSpaceDetails ();
					MessageBox ("Error Limit cannot be less than warning limit.");
				} else if (tempErr > totalArea || tempWarn > totalArea) {
					ClearSpaceDetails ();
					MessageBox ("Error or warning Limits cannot be greater than total area.");
				} else if (tempErr < Convert.ToInt32 (lblTotalUsedArea.Text)) {
					ClearSpaceDetails ();
					MessageBox ("Error Limit cannot be less than current area.");
				} else {
					lblErrorLimit.Text = tempErr.ToString();
					lblWarningLimit.Text = tempWarn.ToString();
					warningLimit = tempWarn;
					errorLimit = tempErr;
					ClearSpaceDetails ();
					if (tempWarn < Convert.ToInt32 (lblTotalUsedArea.Text)) {
						MessageBox ("Updated with Warning: Current area is more than set warning limit.");
					}
				}
			}
			else
			{
				MessageBox("Please enter details to Update");
			}
		}

		public void dg_InventoryManagement_SelectionIndexChanged(object sender, EventArgs e)
		{
		}

		private int calculateCurrentTotalArea(string actionFlag)
		{
			String trxType = trxTypeList.SelectedValue;
			int result = 0;
			DataTable dataTable = (DataTable)dg_InventoryManagement.DataSource;
			int length = dataTable.Rows.Count;
			int areaPerUnit = 0;
			int noOfItems = 0;
			if (!String.IsNullOrEmpty(txtAreaPerUnit.Text.Trim()))
				areaPerUnit = Convert.ToInt32(txtAreaPerUnit.Text.Trim());

			if (!String.IsNullOrEmpty(txtNoOfItems.Text.Trim()))
				noOfItems = Convert.ToInt32(txtNoOfItems.Text.Trim());
			
			int prev_TotalUsedArea = Convert.ToInt32(lblTotalUsedArea.Text);
			if (actionFlag == "isInserted")
			{
				result = prev_TotalUsedArea + (areaPerUnit * noOfItems);
			}
			else if (actionFlag == "isInitialLoad" || actionFlag == "isDeleted")
			{
				for (int i = 0; i < length; i++)
				{
					result = result + (Convert.ToInt32(dataTable.Rows[i][1]) * Convert.ToInt32(dataTable.Rows[i][3]));
				}
			}
			return result;
		}

		//Clear Data  
		private void ClearFields()
		{
			txtItemName.Text = "";
			txtNoOfItems.Text = "";
			trxTypeList.ClearSelection();
			txtAreaPerUnit.Text = "";
			txtErrorLimit.Text = "";
			txtUnitPrice.Text = "";
			txtWarningLimit.Text = "";
		}

		private void ClearSpaceDetails()
		{
			txtWarningLimit.Text = "";
			txtErrorLimit.Text = "";
		}

		public void MessageBox(string message)
		{
			Page.ClientScript.RegisterStartupScript(this.GetType(), "ClientScript", "javascript: alert('" + message + "');", true);
		}

	}
}
