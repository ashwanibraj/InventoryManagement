using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace InvMgmt_ASPNET
{
	public class InvEventHandler
	{
		string connectionString = ConfigurationManager.AppSettings["connectionString"].ToString();
		MySqlConnection dbcon;
		MySqlCommand cmd;

		public InvEventHandler ()
		{
		}

		public String removeFromInventory(String trxType, DataTable dataTable, String itemName, int noOfItems){
			int length = dataTable.Rows.Count;
			dbcon = new MySqlConnection (connectionString);
			bool delFlag = false;
			if (trxType == "Delete")
				delFlag = true;

			for (int i = 0; i < length; i++) {
				if ( Convert.ToString(dataTable.Rows [i] [0]) == itemName) {
					if (Convert.ToInt32(dataTable.Rows [i] [3]) >= noOfItems || delFlag) {
						dbcon.Open ();
						if (Convert.ToInt32(dataTable.Rows [i] [3]) == noOfItems || delFlag) {
							cmd = new MySqlCommand ("DELETE FROM Inventory_Headers" +
								" WHERE Item_Name = @name", 
								dbcon);									
						} else {
							cmd = new MySqlCommand ("insert into Transaction_Details" +
								" (Item_Name, No_of_Units, Transaction_Type) values(@name,@units,@trans)", 
								dbcon);

							cmd.Parameters.AddWithValue ("@name", itemName);
							cmd.Parameters.AddWithValue ("@units", noOfItems);
							cmd.Parameters.AddWithValue ("@trans", "Remove");
							cmd.ExecuteNonQuery ();

							cmd = new MySqlCommand ("UPDATE Inventory_Headers" +
								" SET Available_Units = Available_Units - @units WHERE Item_Name = @name", 
								dbcon);									
							cmd.Parameters.AddWithValue ("@units", noOfItems);
						}


						cmd.Parameters.AddWithValue ("@name", itemName);

						cmd.ExecuteNonQuery ();



						dbcon.Close ();

					} else {
						return "InsufficientUnits";
					}
					return "Success";
				}
			}
			return "ItemNotFound";
		}

		public bool addToInventory(String trxType, String itemName, int noOfItems, int areaPerUnit, int unitPrice){
			try{
				dbcon = new MySqlConnection (connectionString);

				if (trxType == "New") {
					cmd = new MySqlCommand ("insert into Inventory_Headers" +
						"(Item_Name, Area_Per_Unit, Unit_Price, Available_Units) values(@name,@area, @price, @units)", 
						dbcon);
					cmd.Parameters.AddWithValue ("@area", areaPerUnit);
					cmd.Parameters.AddWithValue ("@price", unitPrice);
				} else {
					cmd = new MySqlCommand ("UPDATE Inventory_Headers" +
						" SET Available_Units = Available_Units + @units WHERE Item_Name = @name", 
						dbcon);							
				}
				dbcon.Open ();

				cmd.Parameters.AddWithValue ("@name", itemName);
				cmd.Parameters.AddWithValue ("@units", noOfItems);

				cmd.ExecuteNonQuery ();

				cmd = new MySqlCommand ("insert into Transaction_Details" +
					"(Item_Name, No_of_Units, Transaction_Type) values(@name,@units,@trans)", 
					dbcon);

				cmd.Parameters.AddWithValue ("@name", itemName);
				cmd.Parameters.AddWithValue ("@units", noOfItems);
				cmd.Parameters.AddWithValue ("@trans", "Add");
				cmd.ExecuteNonQuery ();

				dbcon.Close ();

				return true;

			} catch (Exception){
				//throw;
				return false;	
			}
		}
	}
}

