using log4net;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace OWASP.WebGoat.NET.App_Code.DB
{
    public class SqlServerDbProvider : IDbProvider
    {
        private string _connectionString;
        private readonly string _host;
        private readonly string _port;
        private readonly string _pwd;
        private readonly string _uid;
        private readonly string _database;
        private readonly string _clientExec;

        private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SqlServerDbProvider(ConfigFile configFile)
        {
            if (configFile == null)
                _connectionString = string.Empty;

            if (!string.IsNullOrEmpty(configFile.Get(DbConstants.KEY_PWD)))
            {
                // TODO: Set up server connection string
                _connectionString = string.Format("SERVER={0};PORT={1};DATABASE={2};UID={3};PWD={4}",
                                                  configFile.Get(DbConstants.KEY_HOST),
                                                  configFile.Get(DbConstants.KEY_PORT),
                                                  configFile.Get(DbConstants.KEY_DATABASE),
                                                  configFile.Get(DbConstants.KEY_UID),
                                                  configFile.Get(DbConstants.KEY_PWD));
            }
            else
            {
                _connectionString = string.Format("Server={0};Trusted_Connection=True;MultipleActiveResultSets=true;",
                                                 configFile.Get(DbConstants.KEY_HOST));
            }

            _uid = configFile.Get(DbConstants.KEY_UID);
            _pwd = configFile.Get(DbConstants.KEY_PWD);
            _database = configFile.Get(DbConstants.KEY_DATABASE);
            _host = configFile.Get(DbConstants.KEY_HOST);
            _clientExec = configFile.Get(DbConstants.KEY_CLIENT_EXEC);
            _port = configFile.Get(DbConstants.KEY_PORT);
        }

        public string Name
        {
            get
            {
                return DbConstants.DB_TYPE_SQLSERVER;
            }
        }

        public string AddComment(string productCode, string email, string comment)
        {
            string sql = "insert into Comments(productCode, email, comment) values ('" + productCode + "','" + email + "','" + comment + "');";
            string output = null;

            try
            {

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.Error("Error adding comment", ex);
                output = ex.Message;
            }

            return output;
        }

        public DataSet GetCatalogData()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter("select * from Products", connection);
                DataSet ds = new DataSet();

                da.Fill(ds);

                return ds;
            }
        }

        public DataSet GetComments(string productCode)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "select * from Comments where productCode = @productCode";
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                da.SelectCommand.Parameters.AddWithValue("@productCode", productCode);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public DataSet GetCustomerDetails(string customerNumber)
        {
            string sql = "select Customers.customerNumber, Customers.customerName, Customers.logoFileName, Customers.contactLastName, Customers.contactFirstName, " +
                "Customers.phone, Customers.addressLine1, Customers.addressLine2, Customers.city, Customers.state, Customers.postalCode, Customers.country, " +
                "Customers.salesRepEmployeeNumber, Customers.creditLimit, CustomerLogin.email, CustomerLogin.password, CustomerLogin.questionid, CustomerLogin.answer " +
                "From Customers, CustomerLogin where Customers.customerNumber = CustomerLogin.customerNumber and Customers.customerNumber = " + customerNumber;

            DataSet ds = new DataSet();
            try
            {

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                    da.Fill(ds);
                }

            }
            catch (Exception ex)
            {
                log.Error("Error getting customer details", ex);

                throw new ApplicationException("Error getting customer details", ex);
            }
            return ds;
        }

        public string GetCustomerEmail(string customerNumber)
        {
            string output = null;
            try
            {

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sql = "select email from CustomerLogin where customerNumber = " + customerNumber;
                    SqlCommand command = new SqlCommand(sql, connection);
                    output = command.ExecuteScalar().ToString();
                }
            }
            catch (Exception ex)
            {
                output = ex.Message;
            }
            return output;
        }

        public DataSet GetCustomerEmails(string email)
        {
            string sql = "select email from CustomerLogin where email like '" + email + "%'";


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count == 0)
                    return null;
                else
                    return ds;
            }
        }

        public string GetEmailByCustomerNumber(string num)
        {
            string output = "";
            try
            {

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string sql = "select email from CustomerLogin where customerNumber = " + num;
                    SqlCommand cmd = new SqlCommand(sql, connection);
                    output = (string)cmd.ExecuteScalar();
                }

            }
            catch (Exception ex)
            {
                log.Error("Error getting email by customer number", ex);
                output = ex.Message;
            }

            return output;
        }

        public DataSet GetEmailByName(string name)
        {
            string sql = "select firstName, lastName, email from Employees where firstName like '" + name + "%' or lastName like '" + name + "%'";


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count == 0)
                    return null;
                else
                    return ds;
            }
        }

        public DataSet GetOffice(string city)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "select * from Offices where city = @city";
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                da.SelectCommand.Parameters.AddWithValue("@city", city);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public DataSet GetOrderDetails(int orderNumber)
        {
            string sql = "select Customers.customerName, Orders.customerNumber, Orders.orderNumber, Products.productName, " +
                "OrderDetails.quantityOrdered, OrderDetails.priceEach, Products.productImage " +
                "from OrderDetails, Products, Orders, Customers where " +
                "Customers.customerNumber = Orders.customerNumber " +
                "and OrderDetails.productCode = Products.productCode " +
                "and Orders.orderNumber = OrderDetails.orderNumber " +
                "and OrderDetails.orderNumber = " + orderNumber;


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count == 0)
                    return null;
                else
                    return ds;
            }
        }

        public DataSet GetOrders(int customerID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "select * from Orders where customerNumber = " + customerID;
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count == 0)
                    return null;
                else
                    return ds;
            }
        }

        public string GetPasswordByEmail(string email)
        {
            string result = string.Empty;
            try
            {

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    //get data
                    string sql = "select * from CustomerLogin where email = '" + email + "';";
                    SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                    DataSet ds = new DataSet();
                    da.Fill(ds);

                    //check if email address exists
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        result = "Email Address Not Found!";
                    }

                    string encoded_password = ds.Tables[0].Rows[0]["Password"].ToString();
                    string decoded_password = Encoder.Decode(encoded_password);
                    result = decoded_password;
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return result;
        }

        public DataSet GetPayments(int customerNumber)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "select * from Payments where customerNumber = " + customerNumber;
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count == 0)
                    return null;
                else
                    return ds;
            }
        }

        public DataSet GetProductDetails(string productCode)
        {
            string sql = string.Empty;
            SqlDataAdapter da;
            DataSet ds = new DataSet();


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                sql = "select * from Products where productCode = '" + productCode + "'";
                da = new SqlDataAdapter(sql, connection);
                da.Fill(ds, "products");

                sql = "select * from Comments where productCode = '" + productCode + "'";
                da = new SqlDataAdapter(sql, connection);
                da.Fill(ds, "comments");

                DataRelation dr = new DataRelation("prod_comments",
                ds.Tables["products"].Columns["productCode"], //category table
                ds.Tables["comments"].Columns["productCode"], //product table
                false);

                ds.Relations.Add(dr);
                return ds;
            }
        }

        public DataSet GetProductsAndCategories()
        {
            return GetProductsAndCategories(0);
        }

        public DataSet GetProductsAndCategories(int catNumber)
        {
            //TODO: Rerun the database script.
            string sql = string.Empty;
            SqlDataAdapter da;
            DataSet ds = new DataSet();

            //catNumber is optional.  If it is greater than 0, add the clause to both statements.
            string catClause = string.Empty;
            if (catNumber >= 1)
                catClause += " where catNumber = " + catNumber;


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {

                sql = "select * from Categories" + catClause;
                da = new SqlDataAdapter(sql, connection);
                da.Fill(ds, "categories");

                sql = "select * from Products" + catClause;
                da = new SqlDataAdapter(sql, connection);
                da.Fill(ds, "products");


                //category / products relationship
                DataRelation dr = new DataRelation("cat_prods",
                ds.Tables["categories"].Columns["catNumber"], //category table
                ds.Tables["products"].Columns["catNumber"], //product table
                false);

                ds.Relations.Add(dr);
                return ds;
            }
        }

        public string[] GetSecurityQuestionAndAnswer(string email)
        {
            string sql = "select SecurityQuestions.questiontext, CustomerLogin.answer from CustomerLogin, " +
                "SecurityQuestions where CustomerLogin.email = '" + email + "' and CustomerLogin.questionid = " +
                "SecurityQuestions.questionid;";

            string[] qAndA = new string[2];

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);

                DataSet ds = new DataSet();
                da.Fill(ds);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    qAndA[0] = row[0].ToString();
                    qAndA[1] = row[1].ToString();
                }
            }

            return qAndA;
        }

        public DataSet GetUsers()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string sql = "select * from CustomerLogin;";
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public bool IsValidCustomerLogin(string email, string password)
        {
            //encode password
            string encoded_password = Encoder.Encode(password);

            //check email/password
            string sql = "select * from CustomerLogin where email = '" + email +
                "' and password = '" + encoded_password + "';";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, connection);

                //TODO: User reader instead (for all calls)
                DataSet ds = new DataSet();

                da.Fill(ds);

                try
                {
                    return ds.Tables[0].Rows.Count != 0;

                }
                catch (Exception ex)
                {
                    //Log this and pass the ball along.
                    log.Error("Error checking login", ex);

                    throw new Exception("Error checking login", ex);
                }
            }
        }

        public bool RecreateGoatDb()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Check if database exists
                    string script = "SELECT database_id FROM sys.databases WHERE Name = '" + _database + "'";
                    SqlCommand cmd = new SqlCommand(script, connection);
                    object result = cmd.ExecuteScalar();

                    if (result == null)
                    {
                        // Create database if not existing
                        script = "CREATE DATABASE [" + _database + "]";
                        cmd = new SqlCommand(script, connection);
                        cmd.ExecuteNonQuery();
                    }

                    // Drop and create database tables
                    string scriptPath = Path.Combine(Settings.RootDir, DbConstants.DB_CREATE_SQLSERVER_SCRIPT);
                    script = File.ReadAllText(scriptPath);
                    cmd = new SqlCommand(script, connection);
                    cmd.ExecuteNonQuery();

                    // Delete and insert seed data
                    scriptPath = Path.Combine(Settings.RootDir, DbConstants.DB_LOAD_SQLSERVER_SCRIPT);
                    script = File.ReadAllText(scriptPath);
                    cmd = new SqlCommand(script, connection);
                    cmd.ExecuteNonQuery();

                    connection.Close();
                }

                _connectionString = string.Format("Server={0};Database={1};Trusted_Connection=True;MultipleActiveResultSets=true;",
                                                     _host,
                                                     _database);

                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error recreating DB", ex);
                return false;
            }
        }

        public bool TestConnection()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("select * from information_schema.TABLES", connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                log.Error("Error testing DB", ex);
                return false;
            }
        }

        public string UpdateCustomerPassword(int customerNumber, string password)
        {
            string sql = "update CustomerLogin set password = '" + Encoder.Encode(password) + "' where customerNumber = " + customerNumber;
            string output = null;
            try
            {

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand(sql, connection);

                    int rows_added = command.ExecuteNonQuery();

                    log.Info("Rows Added: " + rows_added + " to comment table");
                }
            }
            catch (Exception ex)
            {
                log.Error("Error updating customer password", ex);
                output = ex.Message;
            }
            return output;
        }
    }
}