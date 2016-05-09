using System;
using MySql.Data.MySqlClient;
using System.Data;
using System.Configuration;

namespace InvMgmt_ASPNET
{
	public class DataGatherer
	{
		MySqlConnection dbcon;

		string connectionString = ConfigurationManager.AppSettings["connectionString"].ToString();

		public DataGatherer ()
		{			
		}

		public void initializeDB(){
			dbcon = new MySqlConnection(connectionString);

			//IDbCommand dbcmd = dbcon.CreateCommand();
			string strCreate = "CREATE TABLE IF NOT EXISTS Inventory_Headers"+
				"(Item_Name CHAR(20) NOT NULL, " +
				"Area_Per_Unit INT NOT NULL, "+
				"Unit_Price INT NOT NULL, "+
				"Available_Units INT NOT NULL, "+
				"PRIMARY KEY (Item_Name))";
			
			MySqlCommand cmdDatabase = new MySqlCommand(strCreate, dbcon);

			dbcon.Open();

			cmdDatabase.ExecuteNonQuery();

			strCreate = "CREATE TABLE IF NOT EXISTS Transaction_Details"+
				"(TransactionID INT NOT NULL AUTO_INCREMENT, "+
				"Item_Name CHAR(20) NOT NULL, " +
				"No_of_Units INT NOT NULL, "+
				"Transaction_Type CHAR(10) NOT NULL, "+
				"PRIMARY KEY (TransactionID), "+
				"FOREIGN KEY (Item_Name) "+
				"REFERENCES Inventory_Headers(Item_Name) ON DELETE CASCADE)";

			cmdDatabase = new MySqlCommand(strCreate, dbcon);
			cmdDatabase.ExecuteNonQuery();

			dbcon.Close();
		}

		public DataTable getInventoryData(){
			dbcon = new MySqlConnection(connectionString);
			dbcon.Open();

			DataTable dt = new DataTable();

			MySqlDataAdapter adapt = new MySqlDataAdapter("select * from Inventory_Headers", dbcon);
			adapt.Fill(dt);

			dbcon.Close();
			return dt;
		}
	}
}

