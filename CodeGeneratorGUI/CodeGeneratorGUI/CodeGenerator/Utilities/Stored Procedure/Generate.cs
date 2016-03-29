using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;



namespace CodeGeneratorGUI.CodeGenerator.Utilities.Stored_Procedure
{
    class Generate
    {
        private static string AddSpace(int count)
        {
            return "".PadLeft(count);
        }


        #region METHOD

        public void AddProcedure(string table, string connectionString,string filePath)
        {
           

            string query = "Select * from " + table + " where 1 = 2";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader(CommandBehavior.KeyInfo))
                    {
                        reader.Read();
                        DataTable tableSchema = reader.GetSchemaTable();


                        StringBuilder main = new StringBuilder();
                        StringBuilder pr = new StringBuilder();
                        StringBuilder insertValue = new StringBuilder();
                        StringBuilder MainValues = new StringBuilder();
                        StringBuilder ForeignValue = new StringBuilder();

                        pr.AppendLine(string.Format("CREATE PROCEDURE [dbo].[{0}_Add]", table));
                        pr.AppendLine(string.Format("("));

                        GetDataType(tableSchema, pr);


                        main.AppendLine(pr.ToString().Trim().TrimEnd(','));

                        insertValue.AppendLine(string.Format(")"));
                        insertValue.AppendLine(string.Format("AS"));
                        insertValue.AppendLine(string.Format("{0}INSERT INTO[{1}]", AddSpace(4), table));
                        insertValue.AppendLine(string.Format(" ("));

                        foreach (System.Data.DataRow row in tableSchema.Rows)
                        {
                            insertValue.AppendLine(string.Format("{1}{0},", row["ColumnName"], AddSpace(10)));
                        }

                        main.AppendLine(insertValue.ToString().Trim().TrimEnd(','));
                        MainValues.AppendLine(string.Format(" )"));
                        MainValues.AppendLine(string.Format("{0}VALUES", AddSpace(4)));
                        MainValues.AppendLine(string.Format(" ("));
                        foreach (System.Data.DataRow row in tableSchema.Rows)
                        {
                            MainValues.AppendLine(string.Format("{1} @{0},", row["ColumnName"], AddSpace(10)));
                        }
                        main.AppendLine(MainValues.ToString().Trim().TrimEnd(','));
                        ForeignValue.AppendLine(string.Format(") "));
                        foreach (System.Data.DataRow row in tableSchema.Rows)
                        {
                            if ((bool)row["IsKey"])
                            {
                                ForeignValue.AppendLine(string.Format("{1} SET @{0} = SCOPE_IDENTITY()", row["ColumnName"], AddSpace(4)));
                            }
                        }
                        main.Append(ForeignValue.ToString().Trim().TrimEnd(','));
                        main.AppendLine(string.Format("\r\n  GO"));
                        System.IO.File.AppendAllText(filePath + table + "Sql" + ".sql", main.ToString());



                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public void DeleteProcedure(string table, string connectionStr, string filePath)
        {
            
            string query = "Select * from " + table + " where 1 = 2";

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(query, conn);
                try
                {
                    conn.Open();

                    using (var reader = command.ExecuteReader(CommandBehavior.KeyInfo))
                    {
                        reader.Read();

                        DataTable tableSchema = reader.GetSchemaTable();
                        StringBuilder main = new StringBuilder();
                        StringBuilder pr = new StringBuilder();
                        StringBuilder insertValue = new StringBuilder();
                        StringBuilder MainValues = new StringBuilder();
                        StringBuilder ForeignValue = new StringBuilder();

                        pr.AppendLine(string.Format(" CREATE PROCEDURE [dbo].[{0}_Delete]", table));
                        pr.AppendLine("(");

                        GetKey(tableSchema, pr);
                        main.AppendLine(pr.ToString().Trim().TrimEnd(','));
                        main.AppendLine(")");
                        insertValue.AppendLine("AS");
                        insertValue.AppendLine(string.Format(" {1}DELETE FROM [{0}]", table, AddSpace(6)));
                        foreach (System.Data.DataRow rows in tableSchema.Rows)
                        {
                            if ((bool)rows["IsKey"])
                            {
                                insertValue.AppendLine(string.Format("{1}WHERE ID = @{0}", rows["ColumnName"], AddSpace(6)));
                            }

                        }
                        main.AppendLine(insertValue.ToString());
                        main.AppendLine("GO");
                        System.IO.File.AppendAllText(filePath + table + "Sql" + ".sql", main.ToString());

                    }



                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void GetKey(DataTable tableSchema, StringBuilder pr)
        {
            foreach (System.Data.DataRow row in tableSchema.Rows)
            {
                if ((bool)row["IsKey"])
                {
                    switch (row["DataType"].ToString())
                    {

                        case "System.String":

                            pr.AppendLine(string.Format("{1} @{0} NVARCHAR,", row["ColumnName"], AddSpace(10)));
                            break;
                        case "System.DateTime":

                            pr.AppendLine(string.Format("{1} @{0} DATETIME,", row["ColumnName"], AddSpace(10)));

                            break;
                        case "System.Decimal":

                            pr.AppendLine(string.Format("{1} @{0}  DECIMAL,", row["ColumnName"], AddSpace(10)));
                            break;
                        case "System.Boolean":

                            pr.AppendLine(string.Format("{1} @{0}  BIT,", row["ColumnName"], AddSpace(10)));
                            break;
                        case "System.Int16":
                            pr.AppendLine(string.Format("{1} @{0} SMALLINT,", row["ColumnName"], AddSpace(10)));
                            break;
                        case "System.Byte[]":
                            pr.AppendLine(string.Format("{1} @{0}  VARBINARY,", row["ColumnName"], AddSpace(10)));
                            break;
                        case "System.Int32":
                            pr.AppendLine(string.Format("{1} @{0}  INT,", row["ColumnName"], AddSpace(10)));
                            break;
                        case "System.Single":
                            pr.AppendLine(string.Format("{1} @{0}  real,", row["ColumnName"], AddSpace(10)));
                            break;
                        case "System.Money":

                            pr.AppendLine(string.Format("{1} @{0}  money,", row["ColumnName"], AddSpace(10)));
                            break;
                        default:

                            pr.AppendLine(string.Format("{2} @{1} {0}, ", row["DataType"], row["ColumnName"], AddSpace(10)));
                            break;
                    }
                }

            }
        }
        public void UpdateProcedure(string table, string connectionStr,string filePath)
        {
           
            string query = "Select * from " + table + " where 1 = 2";

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(query, conn);
                try
                {
                    conn.Open();

                    using (var reader = command.ExecuteReader(CommandBehavior.KeyInfo))
                    {
                        reader.Read();

                        DataTable tableSchema = reader.GetSchemaTable();
                        StringBuilder main = new StringBuilder();
                        StringBuilder pr = new StringBuilder();
                        StringBuilder insertValue = new StringBuilder();
                        StringBuilder MainValues = new StringBuilder();
                        StringBuilder ForeignValue = new StringBuilder();
                        pr.AppendLine(string.Format("CREATE PROCEDURE [dbo].[{0}_Update]", table));
                        pr.AppendLine("(");

                        GetDataType(tableSchema, pr);
                        main.AppendLine(pr.ToString().Trim().TrimEnd(','));
                        main.AppendLine("AS");
                        main.AppendLine(string.Format("{0} UPDATE  [{1}]", AddSpace(4), table));
                        main.AppendLine(string.Format("{0} SET ", AddSpace(4)));
                        foreach (System.Data.DataRow rows in tableSchema.Rows)
                        {
                            insertValue.AppendLine(string.Format("{1} {0} = @{0},", rows["ColumnName"], AddSpace(6)));

                        }
                        main.AppendLine(AddSpace(7) + insertValue.ToString().Trim().TrimEnd(','));
                        main.AppendLine(String.Format("{0} WHERE", AddSpace(4)));

                        foreach (System.Data.DataRow rows in tableSchema.Rows)
                        {
                            if ((bool)rows["IsKey"])
                            {

                                MainValues.AppendLine(string.Format("{1} {0} = @{0},", rows["ColumnName"], AddSpace(6)));
                            }
                        }
                        main.AppendLine(AddSpace(7) + MainValues.ToString().Trim().TrimEnd(','));
                        main.AppendLine("GO");

                        System.IO.File.AppendAllText(filePath + table + "Sql" + ".sql", main.ToString());


                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static void GetDataType(DataTable tableSchema, StringBuilder pr)
        {
            foreach (System.Data.DataRow row in tableSchema.Rows)
            {
                switch (row["DataType"].ToString())
                {

                    case "System.String":

                        pr.AppendLine(string.Format("{1}@{0} NVARCHAR,", row["ColumnName"], AddSpace(10)));
                        break;
                    case "System.DateTime":

                        pr.AppendLine(string.Format("{1}@{0} DATETIME,", row["ColumnName"], AddSpace(10)));

                        break;
                    case "System.Decimal":

                        pr.AppendLine(string.Format("{1}@{0}  DECIMAL,", row["ColumnName"], AddSpace(10)));
                        break;
                    case "System.Boolean":

                        pr.AppendLine(string.Format("{1}@{0}  BIT,", row["ColumnName"], AddSpace(10)));
                        break;
                    case "System.Int16":
                        pr.AppendLine(string.Format("{1}@{0} SMALLINT,", row["ColumnName"], AddSpace(10)));
                        break;
                    case "System.Byte[]":
                        pr.AppendLine(string.Format("{1}@{0}  VARBINARY,", row["ColumnName"], AddSpace(10)));
                        break;
                    case "System.Int32":

                        pr.AppendLine(string.Format("{1}@{0}  INT,", row["ColumnName"], AddSpace(10)));
                        break;
                    case "System.Single":

                        pr.AppendLine(string.Format("{1} @{0}  real,", row["ColumnName"], AddSpace(10)));
                        break;
                    case "System.Money":

                        pr.AppendLine(string.Format("{1} @{0}  money,", row["ColumnName"], AddSpace(10)));
                        break;
                    default:

                        pr.AppendLine(string.Format("{2}@{1} {0}, ", row["DataType"], row["ColumnName"], AddSpace(10)));
                        break;
                }
            }
        }

        public void GetProcedure(string table, string connectionStr, string filePath)
        {
            
            string query = "Select * from " + table + " where 1 = 2";

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(query, conn);
                try
                {
                    conn.Open();

                    using (var reader = command.ExecuteReader(CommandBehavior.KeyInfo))
                    {
                        reader.Read();

                        DataTable tableSchema = reader.GetSchemaTable();
                        StringBuilder main = new StringBuilder();
                        StringBuilder pr = new StringBuilder();
                        StringBuilder insertValue = new StringBuilder();
                        StringBuilder MainValues = new StringBuilder();
                        StringBuilder ForeignValue = new StringBuilder();

                        pr.AppendLine(string.Format("CREATE PROCEDURE [dbo].[{0}_Get]", table));
                        pr.AppendLine("(");
                        GetDataType(tableSchema, pr);
                        main.AppendLine(AddSpace(6) + pr.ToString().Trim().TrimEnd(','));
                        main.AppendLine(")");
                        main.AppendLine("AS");
                        main.AppendLine(AddSpace(6) + "SET NOCOUNT ON");
                        main.AppendLine(AddSpace(6) + "SELECT");
                        foreach (System.Data.DataRow rows in tableSchema.Rows)
                        {
                            insertValue.AppendLine(string.Format("{0}[{1}].{2},", AddSpace(8), table, rows["ColumnName"]));
                        }
                        main.AppendLine(AddSpace(8) + insertValue.ToString().Trim().TrimEnd(','));
                        main.AppendLine(AddSpace(6) + "FROM");
                        main.AppendLine(string.Format("{0} [{1}]", AddSpace(10), table));
                        System.IO.File.AppendAllText(filePath + table + "Sql" + ".sql", main.ToString());


                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        public void GetAllProcedure(string table, string connectionStr, string filePath)
        {

            string query = "Select * from " + table + " where 1 = 2";

            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(query, conn);
                try
                {
                    conn.Open();

                    using (var reader = command.ExecuteReader(CommandBehavior.KeyInfo))
                    {
                        reader.Read();

                        DataTable tableSchema = reader.GetSchemaTable();
                        StringBuilder main = new StringBuilder();
                        StringBuilder pr = new StringBuilder();
                        StringBuilder insertValue = new StringBuilder();
                        StringBuilder MainValues = new StringBuilder();
                        StringBuilder ForeignValue = new StringBuilder();

                        pr.AppendLine(string.Format("CREATE PROCEDURE [dbo].[{0}_GetAll]", table));
                        pr.AppendLine("(");
                        //GetDataType(tableSchema, pr);
                        main.AppendLine(AddSpace(6) + pr.ToString().Trim().TrimEnd(','));
                        main.AppendLine(")");
                        main.AppendLine("AS");
                        main.AppendLine(AddSpace(6) + "SET NOCOUNT ON");
                        main.AppendLine(AddSpace(6) + "SELECT");
                        foreach (System.Data.DataRow rows in tableSchema.Rows)
                        {
                            insertValue.AppendLine(string.Format("{0}[{1}].{2},", AddSpace(8), table, rows["ColumnName"]));
                        }
                        main.AppendLine(AddSpace(8) + insertValue.ToString().Trim().TrimEnd(','));
                        main.AppendLine(AddSpace(6) + "FROM");
                        main.AppendLine(string.Format("{0} [{1}]", AddSpace(10), table));
                        System.IO.File.AppendAllText(filePath + table + "Sql" + ".sql", main.ToString());


                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        #endregion
    }
}
