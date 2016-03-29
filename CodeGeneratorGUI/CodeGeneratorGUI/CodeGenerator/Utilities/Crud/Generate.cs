using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using CodeGeneratorGUI.CodeGenerator.Utilities.foreignKey;

namespace CodeGeneratorGUI.CodeGenerator.Utilities.Crud
{
    public class Generate
    {
        private string AddSpace(int count)
        {
            return "".PadLeft(count);
        }

        public void Add(string table, string connectionString, string filePath)
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
                        DataTable table1Schema = reader.GetSchemaTable();


                        StringBuilder pr = new StringBuilder();
                        

                        pr.AppendLine(string.Format("public int Add({0} entity) \t", table));
                        pr.AppendLine(string.Format("{{\r\n"));
                        pr.AppendLine(string.Format("{0} int id = 0;\r\n", AddSpace(2)));


                        pr.AppendLine(string.Format("{0} try", AddSpace(2)));
                        pr.AppendLine(string.Format("{0} {{", AddSpace(2)));

                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(string.Format("{0} string connectionString = @\"{1}\" ", AddSpace(5),connectionString));

                        pr.AppendLine(AddSpace(6) + "SqlConnection connection = new SqlConnection(connectionString);");
                        pr.AppendLine(AddSpace(6) + "SqlCommand cmd = new SqlCommand();");
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(AddSpace(6) + "cmd.Connection = connection;");
                        pr.AppendLine(string.Format("{1} cmd.CommandText =  \"{0}_Add\";", table, AddSpace(5)));
                        pr.AppendLine(AddSpace(6) + "cmd.CommandTimeout = 99999;");
                        pr.AppendLine(AddSpace(6) + "cmd.CommandType = CommandType.StoredProcedure;");
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));


                        string outparam = "";

                        foreach (System.Data.DataRow row in table1Schema.Rows)
                        {



                            pr.AppendLine(string.Format("{2} cmd.Parameters.AddWithValue(\"@{0}\", entity.{0});",
                                   row["ColumnName"], table, AddSpace(5)
                                ));

                            if ((bool)row["IsKey"])
                            {
                                outparam = string.Format("SqlParameter outputIdParam = new SqlParameter(\"@{0}\", SqlDbType.Int)", row["ColumnName"]);

                            }
                        }


                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(AddSpace(6) + outparam);
                        pr.AppendLine(AddSpace(6) + "{");
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(AddSpace(9) + "Direction = ParameterDirection.Output");
                        pr.AppendLine(AddSpace(6) + "};");
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(string.Format("{0} cmd.Parameters.Add(outputIdParam);", AddSpace(6)));
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(6) + "using(cmd)");
                        pr.AppendLine(AddSpace(6) + "{");
                        pr.AppendLine(AddSpace(8) + "connection.Open();");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(8) + "using(connection)");
                        pr.AppendLine(AddSpace(8) + "{");
                        pr.AppendLine(AddSpace(9) + "int rowsAffected = cmd.ExecuteNonQuery();");
                        pr.AppendLine(AddSpace(9) + "if(rowsAffected > 0 )");
                        pr.AppendLine(AddSpace(9) + "{");
                        pr.AppendLine(string.Format("{0} id = outputIdParam.Value as int? ?? default (int);", AddSpace(10)));
                        pr.AppendLine(string.Format("{0}\t", AddSpace(9)));
                        pr.AppendLine(AddSpace(9) + "}");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(8) + "}");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(6) + "}");
                        pr.AppendLine(AddSpace(2) + "}");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(2) + "catch(Exception ex)");
                        pr.AppendLine(AddSpace(2) + "{");
                        pr.AppendLine(string.Format("{0} this.log.Error(\"{1}Repository.Add\",ex);", AddSpace(4), table));
                        pr.AppendLine(AddSpace(2) + "}");
                        pr.AppendLine(AddSpace(2) + "return id;");
                        pr.AppendLine("}");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));
                        System.IO.File.AppendAllText(filePath + table + "Repository" + ".cs", pr.ToString());
                    }

                }


                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }


            }

        }

        public void Update(string table, string connectionString, string filePath)
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


                        StringBuilder pr = new StringBuilder();
                        
                        pr.AppendLine(string.Format("public bool Update({0} entity) \t", table));
                        pr.AppendLine(string.Format("{{\r\n"));
                        pr.AppendLine(string.Format("bool updateResult = false ;\n"));
                        pr.AppendLine(string.Format("\n"));

                        pr.AppendLine(string.Format("{0} try", AddSpace(2)));
                        pr.AppendLine(string.Format("{0} {{", AddSpace(2)));

                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(string.Format("{0} string connectionStr = @\"{1}\"", AddSpace(5),connectionString));
                        pr.AppendLine(AddSpace(6) + "SqlConnection connection = new SqlConnection(connectionStr);");
                        pr.AppendLine(AddSpace(6) + "SqlCommand cmd = new SqlCommand();");
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(AddSpace(6) + "cmd.Connection = connection;");
                        pr.AppendLine(string.Format("{1} cmd.CommandText =\"{0}_Update\";", table, AddSpace(5)));
                        pr.AppendLine(AddSpace(6) + "cmd.CommandTimeout=  99999;");
                        pr.AppendLine(AddSpace(6) + "cmd.CommandType = CommandType.StoredProcedure;");
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));




                        foreach (System.Data.DataRow row in tableSchema.Rows)
                        {



                            pr.AppendLine(string.Format("{2} cmd.Parameters.AddWithValue(\"@{0}\", entity.{0});",
                                   row["ColumnName"], table, AddSpace(5)
                                ));


                        }


                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));
                        // pr.AppendLine(outparam);
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(6) + "using(cmd)");
                        pr.AppendLine(AddSpace(6) + "{");
                        pr.AppendLine(AddSpace(8) + "connection.Open();");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(8) + "using(connection)");
                        pr.AppendLine(AddSpace(8) + "{");
                        pr.AppendLine(AddSpace(9) + "int rowsAffected = cmd.ExecuteNonQuery();");
                        pr.AppendLine(AddSpace(9) + "if(rowsAffected > 0 )");
                        pr.AppendLine(string.Format("{0}updateResult = true ;", AddSpace(10)));
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(8) + "}");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(6) + "}");
                        pr.AppendLine(AddSpace(2) + "}");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(2) + "catch(Exception ex)");
                        pr.AppendLine(AddSpace(2) + "{");
                        pr.AppendLine(string.Format("{0} this.log.Error(\"{1}Repository.Update\",ex);", AddSpace(4), table));
                        // pr.AppendLine(AddSpace(4) + "this.log.Error(\" table Repository.Add\", ex);");
                        pr.AppendLine(AddSpace(2) + "}");
                        pr.AppendLine(AddSpace(2) + "return updateResult;");
                        pr.AppendLine("}");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));

                        System.IO.File.AppendAllText(filePath + table + "Repository" + ".cs", pr.ToString());
                    }

                }


                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }


            }

        }

        public void Delete(string table, string connectionString,string filePath)
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
                      


                        StringBuilder pr = new StringBuilder();
                       
                        pr.AppendLine(string.Format("public bool Delete({0} entity) \t", table));
                        pr.AppendLine(string.Format("{{\r\n"));
                        pr.AppendLine(string.Format("bool deleteResult = false ;\n"));
                        pr.AppendLine(string.Format("\n"));

                        pr.AppendLine(string.Format("{0} try", AddSpace(2)));
                        pr.AppendLine(string.Format("{0} {{", AddSpace(2)));
                       
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(string.Format("{0} string connectionStr = @\"{1}\";", AddSpace(5),connectionString));
                        pr.AppendLine(AddSpace(6) + "SqlConnection connection = new SqlConnection(connectionStr);");
                        pr.AppendLine(AddSpace(6) + "SqlCommand cmd = new SqlCommand();");
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(AddSpace(6) + "cmd.Connection = connection;");
                        pr.AppendLine(string.Format("{1} cmd.CommandText = \"{0}_Delete\";", table, AddSpace(5)));
                        pr.AppendLine(AddSpace(6) + "cmd.CommandTimeout = 99999;");
                        pr.AppendLine(AddSpace(6) + "cmd.CommandType = CommandType.StoredProcedure;");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));


                        

                        foreach (System.Data.DataRow row in tableSchema.Rows)
                        {
                           
                            if ((bool)row["IsKey"])
                            {
                                pr.AppendLine(string.Format("{2}cmd.Parameters.AddWithValue(\"@{0}\",entity.{0});", row["ColumnName"], table, AddSpace(6)));
                            }


                        }



                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));
                        // pr.AppendLine(outparam);
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(6) + "using(cmd)");
                        pr.AppendLine(AddSpace(6) + "{");
                        pr.AppendLine(AddSpace(8) + "connection.Open();");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(8) + "using(connection)");
                        pr.AppendLine(AddSpace(8) + "{");
                        pr.AppendLine(AddSpace(9) + "int rowsAffected = cmd.ExecuteNonQuery();\r\n");
                        pr.AppendLine(AddSpace(9) + "if(rowsAffected > 0 )\t");
                        pr.AppendLine(string.Format("{0}deleteResult = true ;", AddSpace(10)));
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(8) + "}");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(6) + "}");
                        pr.AppendLine(AddSpace(2) + "}");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(2) + "catch(Exception ex)");
                        pr.AppendLine(AddSpace(2) + "{");
                        pr.AppendLine(string.Format("{0} this.log.Error(\"{1}Repository.Delete\",ex);", AddSpace(4), table));
                        pr.AppendLine(AddSpace(2) + "}");
                        pr.AppendLine(AddSpace(2) + "return deleteResult;");
                        pr.AppendLine("}");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));

                        System.IO.File.AppendAllText(filePath + table + "Repository" + ".cs", pr.ToString());
                    }

                }


                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }


            }

        }

        public void Get(string table, string connectionString, string filePath)
        {
            //string connectionString = "Data Source=.\\SQLEXPRESS1; Initial Catalog=Northwind;Integrated Security=true";

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
                        DataTable table1 = reader.GetSchemaTable();
                        //StringBuilder sb = new StringBuilder();

                        StringBuilder pr = new StringBuilder();

                        pr.AppendLine(string.Format("public {0} Get(int id)", table));
                        pr.AppendLine(string.Format("{{\r\n"));
                        pr.AppendLine(string.Format("{0} {1}Item = null;", table, table.ToString().ToLower()));
                        pr.AppendLine(string.Format("\n"));

                        pr.AppendLine(string.Format("{0} try", AddSpace(2)));
                        pr.AppendLine(string.Format("{0} {{", AddSpace(2)));
                        //pr.AppendLine(string.Format("\b")); 
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(string.Format("{0} string connectionStr = @\"{1}\";", AddSpace(5), connectionString));
                        pr.AppendLine(AddSpace(6) + "SqlConnection connection = new SqlConnection(connectionStr);");
                        pr.AppendLine(AddSpace(6) + "SqlCommand cmd = new SqlCommand();");
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(AddSpace(6) + "cmd.Connection = connection;");
                        pr.AppendLine(string.Format("{1} cmd.CommandText =\"{0}_Get\";", table, AddSpace(5)));
                        pr.AppendLine(AddSpace(6) + "cmd.CommandTimeout = 99999;");
                        pr.AppendLine(AddSpace(6) + "cmd.CommandType = CommandType.StoredProcedure;");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));


                        //  string outparam = "";

                        foreach (System.Data.DataRow row in table1.Rows)
                        {
                            //pr.AppendLine("cmd.Parameters.AddWithValue(" + "@" + row["ColumnName"] + "," + table + "." + row["ColumnName"]);



                            if ((bool)row["IsKey"])
                            {

                                pr.AppendLine(string.Format("{2}cmd.Parameters.AddWithValue(\"@{0}\",id);", row["ColumnName"], table, AddSpace(6)));
                            }


                        }


                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));
                        // pr.AppendLine(outparam);
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(6) + "using(cmd)");

                        pr.AppendLine(AddSpace(7) + "using(connection)");
                        pr.AppendLine(AddSpace(7) + "{");
                        pr.AppendLine(AddSpace(8) + "connection.Open();");
                        pr.AppendLine(string.Format("\t"));


                        pr.AppendLine(string.Format(" {0} using(SqlDataReader reader = cmd.ExecuteReader())", AddSpace(8)));
                        pr.AppendLine(AddSpace(8) + "{");
                        pr.AppendLine(string.Format("{0} reader.Read();", AddSpace(9)));
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("{0}if(reader.HasRows)", AddSpace(9)));
                        pr.AppendLine(AddSpace(9) + "{");

                        List<ForeignKeyMap> key = GetForeignKeyMapping(table, connectionString);

                        StringBuilder schemaTable = GetArgumentDeclarations(table1, "");
                        pr.Append(schemaTable);

                        foreach (ForeignKeyMap foreignKey in key)
                        {
                            StringBuilder argumentDecalaration = GetSchemaTableWithTableName(foreignKey.PKTable);
                            pr.Append(argumentDecalaration);
                            List<ForeignKeyMap> keys = GetForeignKeyMapping(foreignKey.PKTable, connectionString);
                            foreach (ForeignKeyMap foreignKeys in keys)
                            {
                                StringBuilder argumentDecalarationn = GetSchemaTableWithTableName(foreignKeys.PKTable);
                                pr.Append(argumentDecalarationn);
                                List<ForeignKeyMap> keyss = GetForeignKeyMapping(foreignKeys.PKTable, connectionString);
                                foreach (ForeignKeyMap foreignKeyss in keyss)
                                {
                                    StringBuilder argumentDecalarationnn = GetSchemaTableWithTableName(foreignKeyss.PKTable);
                                    pr.Append(argumentDecalarationnn);
                                }
                            }

                        }



                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));

                        StringBuilder DataTypeValue = DataTypes(table1, "", table);
                        pr.Append(DataTypeValue);
                        foreach (ForeignKeyMap dataType in key)
                        {
                            StringBuilder dataTypes = RetrieveSchemaTable(dataType.PKTable);
                            pr.Append(dataTypes);
                            List<ForeignKeyMap> keys = GetForeignKeyMapping(dataType.PKTable, connectionString);
                            foreach (ForeignKeyMap foreignKeys in keys)
                            {
                                StringBuilder dataTypess = RetrieveSchemaTable(foreignKeys.PKTable);
                                pr.Append(dataTypess);
                                List<ForeignKeyMap> keyss = GetForeignKeyMapping(foreignKeys.PKTable, connectionString);
                                foreach (ForeignKeyMap foreignKeyss in keyss)
                                {
                                    StringBuilder argumentDecalarationnn = RetrieveSchemaTable(foreignKeyss.PKTable);
                                    pr.Append(argumentDecalarationnn);
                                }
                            }

                        }

                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));
                        StringBuilder buildTableFields = FieldValuesForGet(table, table1, "");
                        pr.Append(buildTableFields);
                        foreach (ForeignKeyMap foreignKey in key)
                        {
                            StringBuilder foreignKeyArgumentsField = BuildTableFields(foreignKey.PKTable);
                            pr.Append(foreignKeyArgumentsField);
                            List<ForeignKeyMap> keys = GetForeignKeyMapping(foreignKey.PKTable, connectionString);
                            foreach (ForeignKeyMap foreignKeys in keys)
                            {
                                StringBuilder dataTypess = BuildTableFields(foreignKeys.PKTable);
                                pr.Append(dataTypess);
                                List<ForeignKeyMap> keyss = GetForeignKeyMapping(foreignKeys.PKTable, connectionString);
                                foreach (ForeignKeyMap foreignKeyss in keyss)
                                {
                                    StringBuilder argumentDecalarationnn = BuildTableFields(foreignKeyss.PKTable);
                                    pr.Append(argumentDecalarationnn);
                                }
                            }


                        }


                        foreach (ForeignKeyMap foreignKey in key)
                        {

                            pr.AppendLine(string.Format("{3}{0}.{1}= {2};", table, foreignKey.PKTable, foreignKey.PKTable.ToString().ToLower(), AddSpace(10)));
                            List<ForeignKeyMap> keys = GetForeignKeyMapping(foreignKey.PKTable, connectionString);
                            foreach (ForeignKeyMap foreignKeys in keys)
                            {
                                pr.AppendLine(string.Format("{3}{0}.{1}= {2};", table, foreignKeys.PKTable, foreignKeys.PKTable.ToString().ToLower(), AddSpace(10)));
                                List<ForeignKeyMap> keyss = GetForeignKeyMapping(foreignKeys.PKTable, connectionString);
                                foreach (ForeignKeyMap foreignKeyss in keyss)
                                {
                                    pr.AppendLine(string.Format("{3}{0}.{1}= {2};", table, foreignKeyss.PKTable, foreignKeyss.PKTable.ToString().ToLower(), AddSpace(10)));
                                }
                            }

                        }


                        pr.AppendLine(AddSpace(9) + "}");
                        pr.AppendLine(AddSpace(8) + "}");
                        pr.AppendLine(AddSpace(7) + "}");
                        pr.AppendLine(AddSpace(6) + "}");


                        pr.AppendLine(AddSpace(2) + "catch(Exception ex)");
                        pr.AppendLine(AddSpace(2) + "{");
                        pr.AppendLine(string.Format("{0} this.log.Error(\"{1}Repository.Get\",ex);", AddSpace(4), table));
                        pr.AppendLine(AddSpace(2) + "}");
                        pr.AppendLine(AddSpace(2) + "return" + AddSpace(2) + table.ToString().ToLower() + "Item;");
                        pr.AppendLine("}");


                        System.IO.File.AppendAllText(filePath + table + "Repository" + ".cs", pr.ToString());

                    }

                }


                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }


            }

        }
        
        public void GetAll(string table, string connectionString, string filePath)
        {
            // string connectionString = "Data Source=.\\SQLEXPRESS1; Initial Catalog=Northwind;Integrated Security=true";

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
                        DataTable table1 = reader.GetSchemaTable();
                        //StringBuilder sb = new StringBuilder();

                        StringBuilder pr = new StringBuilder();
                        StringBuilder foreignKeyValues = new StringBuilder();
                        StringBuilder sb = new StringBuilder();
                        StringBuilder result = new StringBuilder();


                        pr.AppendLine(string.Format("public List<{0}> GetAll(int id)", table));
                        pr.AppendLine(string.Format("{{\r\n"));
                        pr.AppendLine(string.Format("List<{0}> {1}List = new List<{0}>();", table, table.ToString().ToLower()));
                        pr.AppendLine(string.Format("\n"));

                        pr.AppendLine(string.Format("{0} try", AddSpace(2)));
                        pr.AppendLine(string.Format("{0} {{", AddSpace(2)));
                        //pr.AppendLine(string.Format("\b")); 
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(string.Format("{0} string connectionStr = @\"{1}\";", AddSpace(5), connectionString));
                        pr.AppendLine(AddSpace(6) + "SqlConnection connection = new SqlConnection(connectionStr);");
                        pr.AppendLine(AddSpace(6) + "SqlCommand cmd = new SqlCommand();");
                        pr.AppendLine(string.Format("{0} \t", AddSpace(6)));
                        pr.AppendLine(AddSpace(6) + "cmd.Connection = connection;");
                        pr.AppendLine(string.Format("{1} cmd.CommandText = \"{0}_GetAll\";", table, AddSpace(5)));
                        pr.AppendLine(AddSpace(6) + "cmd.CommandTimeout = 99999;");
                        pr.AppendLine(AddSpace(6) + "cmd.CommandType = CommandType.StoredProcedure;");
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));


                        //  string outparam = "";

                        foreach (System.Data.DataRow row in table1.Rows)
                        {
                            //pr.AppendLine("cmd.Parameters.AddWithValue(" + "@" + row["ColumnName"] + "," + table + "." + row["ColumnName"]);



                            if ((bool)row["IsKey"])
                            {

                                pr.AppendLine(string.Format("{2}cmd.Parameters.AddWithValue(\"@{0}\",{1}.{0});", row["ColumnName"], table, AddSpace(6)));
                            }


                        }


                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));
                        // pr.AppendLine(outparam);
                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(AddSpace(6) + "using(cmd)");

                        pr.AppendLine(AddSpace(7) + "using(connection)");
                        pr.AppendLine(AddSpace(7) + "{");
                        pr.AppendLine(AddSpace(8) + "connection.Open();");
                        pr.AppendLine(string.Format("\t"));


                        pr.AppendLine(string.Format(" {0} using(SqlDataReader = reader = cmd.ExecuteReader())", AddSpace(8)));
                        pr.AppendLine(AddSpace(8) + "{");

                        pr.AppendLine(string.Format("{0}while(reader.Read())", AddSpace(9)));
                        pr.AppendLine(AddSpace(9) + "{");


                        //tableColumns(table1, pr);

                        List<ForeignKeyMap> keys = GetForeignKeyMapping(table, connectionString);

                        // building arguments

                        StringBuilder arguments = GetArgumentDeclarations(table1, "");
                        pr.Append(arguments);

                        foreach (ForeignKeyMap foreignKey in keys)
                        {
                            StringBuilder foreignKeyArguments = GetSchemaTableWithTableName(foreignKey.PKTable);
                            pr.Append(foreignKeyArguments);
                            List<ForeignKeyMap> keyss = GetForeignKeyMapping(foreignKey.PKTable, connectionString);
                            foreach (ForeignKeyMap foreignKeys in keyss)
                            {
                                StringBuilder argumentDecalarationn = GetSchemaTableWithTableName(foreignKeys.PKTable);
                                pr.Append(argumentDecalarationn);
                                List<ForeignKeyMap> keysss = GetForeignKeyMapping(foreignKeys.PKTable, connectionString);
                                foreach (ForeignKeyMap foreignKeyss in keysss)
                                {
                                    StringBuilder argumentDecalarationnn = GetSchemaTableWithTableName(foreignKeyss.PKTable);
                                    pr.Append(argumentDecalarationnn);
                                }
                            }

                        }

                        // build retrievers/settters

                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));

                        StringBuilder retrieves = DataTypes(table1, "", table);
                        pr.Append(retrieves);
                        foreach (ForeignKeyMap foreignKey in keys)
                        {
                            StringBuilder dataTypeLevelOne = RetrieveSchemaTable(foreignKey.PKTable);
                            pr.Append(dataTypeLevelOne);
                            List<ForeignKeyMap> keyLevelTwo = GetForeignKeyMapping(foreignKey.PKTable, connectionString);
                            foreach (ForeignKeyMap foreignKeys in keyLevelTwo)
                            {
                                StringBuilder dataTypeLevelTwo = RetrieveSchemaTable(foreignKeys.PKTable);
                                pr.Append(dataTypeLevelTwo);
                                List<ForeignKeyMap> keyLevelThree = GetForeignKeyMapping(foreignKeys.PKTable, connectionString);
                                foreach (ForeignKeyMap foreignKeyss in keyLevelThree)
                                {
                                    StringBuilder dataTypeLevelThree = RetrieveSchemaTable(foreignKeyss.PKTable);
                                    pr.Append(dataTypeLevelThree);
                                }
                            }

                        }


                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));
                        // build tablenames with fields


                        StringBuilder buildTableFields = FieldValues(table, table1, "");
                        pr.Append(buildTableFields);
                        foreach (ForeignKeyMap foreignKey in keys)
                        {
                            StringBuilder foreignKeyArgumentsField = (BuildTableFields(foreignKey.PKTable));
                            pr.Append(foreignKeyArgumentsField);
                            List<ForeignKeyMap> keyLevelTwo = GetForeignKeyMapping(foreignKey.PKTable, connectionString);
                            foreach (ForeignKeyMap foreignKeys in keyLevelTwo)
                            {
                                StringBuilder dataTypeLevelTwo = BuildTableFields(foreignKeys.PKTable);
                                pr.Append(dataTypeLevelTwo);
                                List<ForeignKeyMap> keyLevelThree = GetForeignKeyMapping(foreignKeys.PKTable, connectionString);
                                foreach (ForeignKeyMap foreignKeyss in keyLevelThree)
                                {
                                    StringBuilder dataTypeLevelThree = BuildTableFields(foreignKeyss.PKTable);
                                    pr.Append(dataTypeLevelThree);
                                }
                            }

                        }

                        pr.AppendLine(string.Format("\t"));
                        pr.AppendLine(string.Format("\t"));

                        foreach (ForeignKeyMap foreignKey in keys)
                        {

                            pr.AppendLine(string.Format("{3}{0}.{1}= {2};", table, foreignKey.PKTable, foreignKey.PKTable.ToString().ToLower(), AddSpace(10)));
                            List<ForeignKeyMap> keyLevelTwo = GetForeignKeyMapping(foreignKey.PKTable, connectionString);
                            foreach (ForeignKeyMap foreignKeys in keyLevelTwo)
                            {
                                pr.AppendLine(string.Format("{3}{0}.{1}= {2};", table, foreignKeys.PKTable, foreignKeys.PKTable.ToString().ToLower(), AddSpace(10)));
                                List<ForeignKeyMap> keyLevelThree = GetForeignKeyMapping(foreignKeys.PKTable, connectionString);
                                foreach (ForeignKeyMap foreignKeyss in keyLevelThree)
                                {
                                    pr.AppendLine(string.Format("{3}{0}.{1}= {2};", table, foreignKeyss.PKTable, foreignKeyss.PKTable.ToString().ToLower(), AddSpace(10)));
                                }
                            }

                        }
                        //FieldValues(table, table1, pr);
                        ////string result1;
                        ////List<ForeignKeyMap> keys = GetForeignKeyMapping(table);
                        ////foreach (ForeignKeyMap foreignKey in keys)
                        ////{
                        //// result1 =   GetTableColumns(foreignKey.PKTable);
                        //// pr.AppendLine(result1);

                        ////}

                        // pr.AppendLine(string.Format("{0}user.UserType = new Model.Core.Entities.Type();", AddSpace(10)));
                        //pr.AppendLine(string.Format("{0}user.UserType.Id = typeId;", AddSpace(10)));
                        //pr.AppendLine(string.Format("{0}user.UserType.Name =typeName;", AddSpace(10)));
                        pr.AppendLine(string.Format("{0}{1}List.Add({1});", AddSpace(10), table.ToString().ToLower()));

                        pr.AppendLine(AddSpace(9) + "}");
                        pr.AppendLine(AddSpace(8) + "}");
                        pr.AppendLine(AddSpace(7) + "}");
                        pr.AppendLine(AddSpace(6) + "}");
                        //  pr.AppendLine(AddSpace(2) + "}");

                        pr.AppendLine(AddSpace(2) + "catch(Exception ex)");
                        pr.AppendLine(AddSpace(2) + "{");
                        pr.AppendLine(string.Format("{0} this.log.Error(\"{1}Repository.GetAll\",ex);", AddSpace(4), table));
                        pr.AppendLine(AddSpace(2) + "}");
                        pr.AppendLine(AddSpace(2) + "return" + AddSpace(2) + table.ToString().ToLower() + "List;");
                        pr.AppendLine("}");


                        System.IO.File.AppendAllText(filePath + table + "Repository" + ".cs", pr.ToString());

                    }

                }


                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                }


            }

        }





        public  List<ForeignKeyMap> GetForeignKeyMapping(string tableName,string inputConnectionString)
        {
            List<ForeignKeyMap> mappings = new List<ForeignKeyMap>();

            //string connectionString = "Data Source=.\\SQLEXPRESS1; Initial Catalog=Northwind;Integrated Security=true";


            // Create stored procedure name (ForeignKeyGet) in your database
            // code for stored procedure


            using (SqlConnection connection = new SqlConnection(inputConnectionString))
            {
                SqlCommand cmd = new SqlCommand();

                cmd.CommandText = "ForeignKeyGet";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = connection;

                cmd.Parameters.AddWithValue("@TableName", tableName);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    //reader.Read();
                    while (reader.Read())
                    {
                        string fkTable = "";
                        string fkColumn = "";
                        string pkTable = "";
                        string pkColumn = "";

                        if (reader["FK_Table"] != DBNull.Value)
                            fkTable = (string)reader["FK_Table"];

                        if (reader["FK_Column"] != DBNull.Value)
                            fkColumn = (string)reader["FK_Column"];

                        if (reader["PK_Table"] != DBNull.Value)
                            pkTable = (string)reader["PK_Table"];


                        if (reader["PK_Column"] != DBNull.Value)
                            pkColumn = (string)reader["PK_Column"];

                        ForeignKeyMap foreignKey = new ForeignKeyMap();
                        foreignKey.FKTable = fkTable;
                        foreignKey.FKColumn = fkColumn;
                        foreignKey.PKTable = pkTable;
                        foreignKey.PKColumn = pkColumn;

                        mappings.Add(foreignKey);
                    }
                }


            }

            return mappings;
        }
        public StringBuilder FieldValues(string table, DataTable table1, string prefix)
        {
            StringBuilder pr = new StringBuilder();
            pr.AppendLine(string.Format("\t"));

            pr.AppendLine(string.Format("{0}{2}  {1} = new {2}();", AddSpace(10), table.ToString().ToLower(), table));
            foreach (System.Data.DataRow row1 in table1.Rows)
            {
                string lower = row1["ColumnName"].ToString().ToLower();
                pr.AppendLine(string.Format("{2}{4}.{0} = {3}{1}; ", row1["ColumnName"], CaptalizeFirstLetter(lower, prefix), AddSpace(10), FirstCharToLower(prefix), table.ToString().ToLower()));
            }
            pr.AppendLine(string.Format("\t"));
            return pr;
        }
        public  StringBuilder FieldValuesForGet(string table, DataTable table1, string prefix)
        {
            StringBuilder pr = new StringBuilder();
            pr.AppendLine(string.Format("\t"));

            pr.AppendLine(string.Format("{0}{2}  {1}Item = new {2}();", AddSpace(10), table.ToString().ToLower(), table));
            foreach (System.Data.DataRow row1 in table1.Rows)
            {
                string lower = row1["ColumnName"].ToString().ToLower();
                pr.AppendLine(string.Format("{2}{4}Item.{0} = {3}{1}; ", row1["ColumnName"], CaptalizeFirstLetter(lower, prefix), AddSpace(10), FirstCharToLower(prefix), table.ToString().ToLower()));
            }
            pr.AppendLine(string.Format("\t"));
            return pr;
        }

        public  StringBuilder GetSchemaTableWithTableName(string table)
        {
            string connectionString = "Data Source=.\\SQLEXPRESS1; Initial Catalog=Northwind;Integrated Security=true";

            string query = "Select * from " + table + " where 1 = 2";

            StringBuilder sb = new StringBuilder();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader(CommandBehavior.KeyInfo))
                    {
                        reader.Read();
                        DataTable schemaDataTable = reader.GetSchemaTable();

                        sb.Append(GetArgumentDeclarations(schemaDataTable, table));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return sb;
        }
        public  StringBuilder BuildTableFields(string table)
        {
            string connectionString = "Data Source=.\\SQLEXPRESS1; Initial Catalog=Northwind;Integrated Security=true";

            string query = "Select * from " + table + " where 1 = 2";

            StringBuilder sb = new StringBuilder();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader(CommandBehavior.KeyInfo))
                    {
                        reader.Read();
                        DataTable schemaDataTable = reader.GetSchemaTable();
                        sb.Append(FieldValues(table, schemaDataTable, table));

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return sb;
        }
        public  StringBuilder RetrieveSchemaTable(string table)
        {
            string connectionString = "Data Source=.\\SQLEXPRESS1; Initial Catalog=Northwind;Integrated Security=true";

            string query = "Select * from " + table + " where 1 = 2";

            StringBuilder sb = new StringBuilder();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();

                    using (var reader = command.ExecuteReader(CommandBehavior.KeyInfo))
                    {
                        reader.Read();
                        DataTable schemaDataTable = reader.GetSchemaTable();
                        sb.Append(DataTypes(schemaDataTable, table,table));

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return sb;
        }

        public  StringBuilder DataTypes(DataTable table1, string prefix, string table)
        {
            StringBuilder pr = new StringBuilder();
            foreach (System.Data.DataRow row in table1.Rows)
            {
                switch (row["DataType"].ToString())
                {
                    case "System.Int32":
                        string field = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{2}if(reader[\"{4}{1}\"] != DBNull.Value) \r\n {2}{3}{0} = (int)reader[\"{4}{1}\"];", CaptalizeFirstLetter(field, prefix), row["ColumnName"], AddSpace(10), FirstCharToLower(prefix), table));

                        break;
                    case "System.String":
                        string b = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{2}if(reader[\"{4}{1}\"] != DBNull.Value) \r\n{2}{3}{0} = (string)reader[\"{4}{1}\"];", CaptalizeFirstLetter(b, prefix), row["ColumnName"], AddSpace(10), FirstCharToLower(prefix), table));

                        break;
                    case "System.DateTime":
                        string date = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{2}if(reader[\"{4}{1}\"] != DBNull.Value) \r\n{2}{3}{0} = (DateTime)reader[\"{4}{1}\"];", CaptalizeFirstLetter(date, prefix), row["ColumnName"], AddSpace(10), FirstCharToLower(prefix), table));

                        // pr.AppendLine(string.Format("if(reader[\"{1}\"] != DBNull.Value) \r\n {0}= (DateTime)reader[\"{1}\"];",date = row["ColumnName"].ToString().ToLower(), row["ColumnName"]));

                        break;
                    case "System.Decimal":
                        string decimalValue = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{2}if(reader[\"{4}{1}\"] != DBNull.Value) \r\n{2}{3}{0} = (float)reader[\"{4}{1}\"];", CaptalizeFirstLetter(decimalValue, prefix), row["ColumnName"], AddSpace(10), FirstCharToLower(prefix), table));

                        break;
                    case "System.Boolean":
                        string bValue = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{2}if(reader[\"{4}{1}\"] != DBNull.Value) \r\n{2}{3}{0}= (bool)reader[\"{4}{1}\"];", CaptalizeFirstLetter(bValue, prefix), row["ColumnName"], AddSpace(10), FirstCharToLower(prefix), table));
                        break;
                    case "System.Int16":
                        string IValue = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{2}if(reader[\"{4}{1}\"] != DBNull.Value) \r\n {2}{3}{0} = (int)reader[\"{4}{1}\"];", CaptalizeFirstLetter(IValue, prefix), row["ColumnName"], AddSpace(10), FirstCharToLower(prefix), table));

                        break;
                    case "System.Byte[]":
                        string binaryValue = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{2}if(reader[\"{4}{1}\"] != DBNull.Value) \r\n {2}{3}{0}= (VarBinary)reader[\"{4}{1}\"];", CaptalizeFirstLetter(binaryValue, prefix), row["ColumnName"], AddSpace(10), FirstCharToLower(prefix), table));
                        pr.AppendLine(string.Format("\t"));
                        break;
                    default:
                        string any = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{3}if(reader[\"{4}{1}\"] != DBNull.Value) \r\n{3}{5}{0} = {2}reader[\"{4}{1}\"];", CaptalizeFirstLetter(any, prefix), row["ColumnName"], row["DataType"], AddSpace(10), table, FirstCharToLower(prefix)));

                        break;
                }

                pr.AppendLine(string.Format("\t"));
                pr.AppendLine(string.Format("\t"));





            }
            return pr;
        }

        public  string FirstCharToLower(string data)
        {


            if (data.Length == 0)
                return data;

            var chars = data.ToCharArray();

            // Find the Index of the first letter
            //var charac = data.First(char.IsLetter);
            //var i = data.IndexOf(charac);

            // capitalize that letter
            chars[0] = char.ToLower(chars[0]);

            return new string(chars);

        }
        public  StringBuilder GetArgumentDeclarations(DataTable table1, string prefix)
        {
            StringBuilder pr = new StringBuilder();

            pr.AppendLine(string.Format("\t"));
            foreach (System.Data.DataRow row in table1.Rows)
            {
                switch (row["DataType"].ToString())
                {
                    case "System.Int32":
                        string name = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{1}int {2}{0} = 0;", CaptalizeFirstLetter(name, prefix), AddSpace(10), FirstCharToLower(prefix)));
                        break;
                    case "System.String":
                        string name1 = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{1}string {2}{0} = \"\";", CaptalizeFirstLetter(name1, prefix), AddSpace(10), FirstCharToLower(prefix)));
                        break;
                    case "System.DateTime":
                        string name2 = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{1}DateTime {2}{0} = DateTime.MinValue;", CaptalizeFirstLetter(name2, prefix), AddSpace(10), FirstCharToLower(prefix)));
                        // pr.AppendLine(string.Format("DateTime {0}= DateTime.MinValue;", name2));
                        break;
                    case "System.Decimal":
                        string name3 = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{1}Float {2}{0} = 0;", CaptalizeFirstLetter(name3, prefix), AddSpace(10), FirstCharToLower(prefix)));
                        break;
                    case "System.Boolean":
                        string bb = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{1}bool {2}{0} = false;", CaptalizeFirstLetter(bb, prefix), AddSpace(10), FirstCharToLower(prefix)));
                        break;
                    case "System.Int16":
                        string name4 = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{1}int {2}{0} = 0;", CaptalizeFirstLetter(name4, prefix), AddSpace(10), FirstCharToLower(prefix)));
                        break;
                    case "System.Byte[]":
                        string name5 = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{1}varbinary {2}{0} = \"\";", CaptalizeFirstLetter(name5, prefix), AddSpace(10), FirstCharToLower(prefix)));
                        break;
                    default:
                        string name6 = row["ColumnName"].ToString().ToLower();
                        pr.AppendLine(string.Format("{2}{0} {3}{1} = \"\";", row["DataType"], CaptalizeFirstLetter(name6, prefix), AddSpace(10), FirstCharToLower(prefix)));
                        break;
                }
            }
            pr.AppendLine(string.Format("\t"));


            return pr;
        }
        private static string CaptalizeFirstLetter(string data, string prefix)
        {
            if (prefix.Length == 0)
                return data;

            var chars = data.ToCharArray();

            // Find the Index of the first letter
            //var charac = data.First(char.IsLetter);
            //var i = data.IndexOf(charac);

            // capitalize that letter
            chars[0] = char.ToUpper(chars[0]);

            return new string(chars);
        }


       


    }
}
