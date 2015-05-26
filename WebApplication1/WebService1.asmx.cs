using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebApplication1
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService1 : System.Web.Services.WebService
    {
        public SqlConnection connection = new SqlConnection("Data Source=vart.nu;Uid=sysa14;pwd=mats;Initial Catalog=Cronus");
        public WebService1()
        {

            //Uncomment the following line if using designed components 
            //InitializeComponent(); 
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        //[WebMethod]
        //List<ParentRow> GetData();

        //public class ParentRow
        //{
        //    public int Id { get; set; }
        //    public string Name { get; set; }
        // other parent columns
        //    public List<ChildRow> Children { get; set; }
        //}


        //public class ChildRow
        //{
        //    public int Id { get; set; }
        //    public int ParentId { get; set; }
        //    public string Name { get; set; }
        // other child columns
        //}

        [WebMethod]
        public Employee[] getEmployees()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select [No_], [First Name], [Last Name] from [CRONUS Sverige AB$Employee]", connection);
                DataSet employeeDS = new DataSet();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(employeeDS, "Employees");
                int count = employeeDS.Tables[0].Rows.Count;
                Employee[] empz = new Employee[count];
                int pos = 0;
                foreach (DataRow row in employeeDS.Tables[0].Rows)
                {
                    Employee e = new Employee() { Number = (String)row.ItemArray.GetValue(0), FirstName = (String)row.ItemArray.GetValue(1), LastName = (String)row.ItemArray.GetValue(2) };
                    empz[pos++] = e;
                }
                return empz;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        [WebMethod]
        public void updateEployee(DataTable employeeDT)
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select [No_], [First Name], [Last Name] from [CRONUS Sverige AB$Employee]", connection);
                SqlCommandBuilder cmdB = new SqlCommandBuilder(adapter);
                adapter.Update(employeeDT);
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        [WebMethod]
        public DataSet getEmployeeRelatives()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT [Employee No_], [Relative Code], [First Name], [Last Name], [Birth Date] FROM [CRONUS Sverige AB$Employee Relative] WHERE [Employee No_] IN (SELECT No_ FROM [CRONUS Sverige AB$Employee])", connection);
                DataSet erbDS = new DataSet();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(erbDS, "Employee Relatives");
                return erbDS;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        [WebMethod]
        public DataSet getEmployeeAbscence()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT [Employee No_], [From Date], [To Date], [Cause of Absence Code], [Description], [Unit of Measure Code] FROM [CRONUS Sverige AB$Employee Absence]", connection);
                DataSet abDS = new DataSet();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(abDS, "Employee Abscence");
                return abDS;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        [WebMethod]
        public DataSet getEmployeeAbscence2004()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT [No_], [First Name], [Last Name] FROM [dbo].[CRONUS Sverige AB$Employee] WHERE [No_] IN (SELECT [No_] FROM [CRONUS Sverige AB$Employee Absence] WHERE (DATEPART(yy, [From Date]) = 2004) AND [Cause of Absence Code] = 'SJUK')", connection);
                DataSet abDS = new DataSet();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(abDS, "Employee Abscence In 2004");
                return abDS;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        [WebMethod]
        public DataSet getMostSick()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT [First Name] FROM [dbo].[CRONUS Sverige AB$Employee] WHERE [No_] IN (SELECT TOP 1 WITH TIES [Employee No_] FROM [dbo].[CRONUS Sverige AB$Employee Absence] GROUP BY [Employee No_] ORDER BY COUNT([Employee No_]) DESC)", connection);
                DataSet sickDS = new DataSet();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(sickDS, "The Most Sick Employee(s)");
                return sickDS;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        [WebMethod]
        public DataSet getQualafications()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT [Employee No_], [Qualification Code], [Description] FROM [CRONUS Sverige AB$Employee Qualification]", connection);
                DataSet qDS = new DataSet();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(qDS, "The Most Sick Employee(s)");
                return qDS;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        [WebMethod]
        public DataSet getKeys()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE", connection);
                DataSet kDS = new DataSet();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(kDS, "Keys");
                return kDS;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        [WebMethod]
        public DataSet getIndexes()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM sys.indexes", connection);
                DataSet iDS = new DataSet();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(iDS, "Indexes");
                return iDS;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        [WebMethod]
        public DataSet getTableConstraints()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT CONSTRAINT_NAME, TABLE_NAME, CONSTRAINT_TYPE FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS", connection);
                DataSet tcDS = new DataSet();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(tcDS, "Table Constraints");
                return tcDS;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        [WebMethod]
        public DataSet getTables()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'", connection);
                //Lösning nummer 2.
                //SqlDataAdapter adapter = new SqlDataAdapter("SELECT name FROM sys.tables", connection);
                DataSet tDS = new DataSet();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(tDS, "Tables");
                return tDS;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }

        [WebMethod]
        public DataSet getColumns()
        {
            try
            {
                SqlDataAdapter adapter = new SqlDataAdapter("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'CRONUS Sverige AB$Employee'", connection);
                //Lösning nummer 2.
                //SqlDataAdapter adapter = new SqlDataAdapter("SELECT name FROM sys.columns WHERE object_id = OBJECT_ID('[dbo].[CRONUS Sverige AB$Employee]')", connection);
                DataSet cDS = new DataSet();
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(cDS, "Columns");
                return cDS;
            }
            catch (SqlException e)
            {
                throw e;
            }
        }
    }
}