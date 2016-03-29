using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CodeGeneratorGUI.CodeGenerator.Utilities.Class
{
   public class Generate
    {
        private  string AddSpace(int count)
        {
            return "".PadLeft(count);
        }

        #region METHOD
        public void GenerateTableColumns(string table, string connectionString, string filePath)
        {
          

            string query = "Select * from " + table + " where 1 = 2";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                try
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        reader.Read();
                        DataTable tableSchema = reader.GetSchemaTable();
                        StringBuilder sb = new StringBuilder();

                        StringBuilder pr = new StringBuilder();
                      //  sb.AppendLine(UsingReference);
                      //  sb.AppendLine("namespace " +namespaces);

                        sb.AppendLine("public class " + table);
                        sb.AppendLine(string.Format("{{\r\n"));
                       
                        GetDataType(tableSchema, sb);

                        sb.AppendLine("}");

                        try
                        {
                            string dirPath = filePath;
                            string fileName = table + ".txt";
                          
                                System.IO.File.AppendAllText(filePath + table + ".cs", sb.ToString());
                            
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
        }

        private void GetDataType(DataTable tableSchema, StringBuilder sb)
        {
            foreach (System.Data.DataRow row in tableSchema.Rows)
            {
                
                Console.WriteLine(row["ColumnName"] + "\n" + row["ColumnSize"] + "\n" + row["DataType"]);

                switch (row["DataType"].ToString())
                {
                    case "System.Int32":
                        sb.AppendLine(AddSpace(4) + "public" + " int " + row["ColumnName"] + " { get; set; }");
                        
                        break;
                    case "System.String":
                        sb.AppendLine(AddSpace(4) + "public" + " string " + row["ColumnName"] + " { get; set; }");
                        break;
                    case "System.DateTime":
                        sb.AppendLine(AddSpace(4) + "public" + " DateTime " + row["ColumnName"] + " { get; set;}");
                        break;
                    case "System.Decimal":
                        sb.AppendLine(AddSpace(4) + "public" + " decimal " + row["ColumnName"] + " { get; set; }");
                        break;
                    case "System.Boolean":
                        sb.AppendLine(AddSpace(4) + "public" + " bool " + row["ColumnName"] + " { get; set; }");
                        break;
                    case "System.Int16":
                        sb.AppendLine(AddSpace(4) + "public" + " int " + row["ColumnName"] + " { get; set; }");
                        break;
                    case "System.Byte[]":
                        sb.AppendLine(AddSpace(4) + "public" + " binary " + row["ColumnName"] + " { get; set; }");
                        break;
                    case "System.Single":
                        sb.AppendLine(AddSpace(4) + "public" + " real  " + row["ColumnName"] + " { get; set; }");
                        
                        break;
                    case "System.Money":

                        sb.AppendLine(AddSpace(4) + "public" + " money  " + row["ColumnName"] + " { get; set; }");
                        break;
                    default:
                        sb.AppendLine("\tpublic" + row["DataType"] + row["ColumnName"] + " { get; set; }");
                        break;
                }
            }
        }
        #endregion
    }
}
