using MySql.Data;
using MySql.Data.MySqlClient;

namespace Utility{
    public class DBConnection{
       
        private MySqlConnection connection = null;
        private string databaseName = string.Empty;
        private string server = "localhost";
        private string database = "Achilles";
        private string username = "root";
        private string password = "L2J3J5A8()";

        private DBConnection(){}

        public MySqlConnection Connection{ get {return connection;}}
        
        private static DBConnection _instance = null;
        public static DBConnection Instance(){
            if(_instance == null) _instance = new DBConnection();
            return _instance;
        }
        public void closeConnection(){
            connection.Close();
        }
        public bool isConnected(){
            if (connection == null){
                if(string.IsNullOrEmpty(databaseName))return false;
                else{
                    string connectionString = string.Format("Server={0}; database={1}; UID={2};password={3}", server,database,username,password);
                    connection = new MySqlConnection(connectionString);
                    connection.Open();
                }
            }
            return true;
       }
    }
}