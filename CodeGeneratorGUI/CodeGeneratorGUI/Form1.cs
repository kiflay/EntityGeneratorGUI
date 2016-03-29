using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using CodeGeneratorGUI.CodeGenerator;
using System.IO;






namespace CodeGeneratorGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();   
        }

        string InputConnectionString;
        string filePath;

        private static string AddSpace(int count)
        {
            return "".PadLeft(count);
        }
        public void button1_Click(object sender, EventArgs e)
        {

            InputConnectionString = textBox1.Text.ToString();
            //textBox2.Text = InputConnectionString;
            List<string> Tables = new List<string>();
            // string query = "Select * from " + table + " where 1 = 2";
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.Enabled = true;


            using (SqlConnection conn = new SqlConnection(InputConnectionString))
            {
                try
                {
                    conn.Open();
                    System.Data.DataTable tables = conn.GetSchema("Tables");
                    foreach (System.Data.DataRow rows in tables.Rows)
                    {
                        if (rows["TABLE_TYPE"].ToString() == "BASE TABLE")
                        {
                            string tableNames = rows["TABLE_NAME"].ToString();
                            Tables.Add(tableNames);

                            checkedListBox1.Items.Add(tableNames);



                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }



        private void button9_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            string folder = folderBrowserDialog1.SelectedPath + "\\";
            textBox3.Text = folder;
        }
        public void button2_Click(object sender, EventArgs e)
        {
            string namespaces = textBox5.Text.ToString();
            filePath = textBox3.Text.ToString();
            string ClassUsingReference = UsingReferenceClassText.Text;
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                //  listBox1.Items.Add(checkedListBox1.CheckedItems[i]);
                CodeGenerator.Utilities.Class.Generate generate = new CodeGenerator.Utilities.Class.Generate();


                if (System.IO.Directory.Exists(filePath))
                {
                    generate.GenerateTableColumns(checkedListBox1.CheckedItems[i].ToString(), InputConnectionString, filePath);
                }
                else
                {
                    //  listBox2.Items.Add("file does not exist");
                }

            }



        }




        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }

        }

        private void button10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
            }
        }

        private void btn_DeselectAll_Click(object sender, EventArgs e)
        {

        }



        private void classBrowse_Click(object sender, EventArgs e)
        {
            DialogResult folderShow = folderBrowserDialog2.ShowDialog();
            string folder = folderBrowserDialog2.SelectedPath + "\\";
            classDirectory.Text = folder;
        }

        private void crudBrowse_Click(object sender, EventArgs e)
        {
            DialogResult folderShow = folderBrowserDialog3.ShowDialog();
            string folder = folderBrowserDialog3.SelectedPath + "\\";
            crudDirectory.Text = folder;
        }

        private void spBrowse_Click(object sender, EventArgs e)
        {
            DialogResult folderShow = folderBrowserDialog3.ShowDialog();
            string folder = folderBrowserDialog3.SelectedPath + "\\";
            spDirectory.Text = folder;
        }

        public String GetPrimaryKey(string table)
        {
            string connectionStr = "Data Source=.\\SQLEXPRESS1; Initial Catalog=Northwind;Integrated Security=true";
            string query = "Select * from " + table + " where 1 = 2";
            StringBuilder PrimaryKey = new StringBuilder();
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                SqlCommand command = new SqlCommand(query, conn);
                try
                {
                    conn.Open();

                    using (var reader = command.ExecuteReader(CommandBehavior.KeyInfo))
                    {
                        reader.Read();

                        DataTable table1 = reader.GetSchemaTable();
                        //  StringBuilder PrimaryKey = new StringBuilder();

                        foreach (System.Data.DataRow rows in table1.Rows)
                        {
                            if ((bool)rows["IsKey"])
                            {
                                Console.WriteLine(rows["ColumnName"]);
                                PrimaryKey.AppendLine(rows["ColumnName"].ToString());

                            }
                        }
                        System.IO.File.WriteAllText(@"C:\Users\gnc\Desktop\class\procedure\foreignKey\" + table + "Repository" + ".txt", PrimaryKey.ToString());

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return PrimaryKey.ToString();
        }

        private void generate_Click(object sender, EventArgs e)
        {


            if (generateService.Checked)
            {

                string namespaces = ServiceNamespace.Text;
                string directory = ServiceDirectory.Text;
                StringBuilder service = new StringBuilder();
                service.AppendLine(string.Format("namespace {0}.Service",namespaces));
                service.AppendLine("{");
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    string TableName = FirstCharToLower(checkedListBox1.CheckedItems[i].ToString());
                    service.AppendLine(string.Format("{1}public class {0}Service : ServiceBase<{0}>,I{0}Service", checkedListBox1.CheckedItems[i].ToString(),AddSpace(4)));
                    service.AppendLine(AddSpace(4)+"{");
                   // service.AppendLine(string.Format("private I{}Repository _{0}Repository",checkedListBox1.CheckedItems[i].ToString()));
                    service.AppendLine(AddSpace(6)+"private" + "I" + checkedListBox1.CheckedItems[i].ToString() + "Repository " + "_" + TableName + "Repository;");
                    List<ForeignKeyMap> keys = GetForeignKeyMapping(checkedListBox1.CheckedItems[i].ToString(),InputConnectionString);
                    foreach (ForeignKeyMap foreignKeys in keys)
                    {
                        string foreignKey = FirstCharToLower(foreignKeys.PKTable);
                        service.AppendLine(AddSpace(6)+"private" + "I" + foreignKeys.PKTable + "Repository  " + "_" + foreignKey + "Repository;");
                   
                    }
                    service.AppendLine("\t");
                    service.AppendLine(string.Format("{1}public {0}Service(I{0}Repository repository): base(repository)", checkedListBox1.CheckedItems[i].ToString(), AddSpace(6)));
                    service.AppendLine(AddSpace(6)+"{");
                    service.AppendLine("\t");
                    service.AppendLine(AddSpace(8)+"_"+TableName + "Repository " + " = " + "repository;");
                    foreach (ForeignKeyMap foreignKeys in keys)
                    {
                        string foreignKey = FirstCharToLower(foreignKeys.PKTable);
                        service.AppendLine(AddSpace(8)+"_" + foreignKey + "Repository " + " = " + " repository." + foreignKeys.PKTable +"Repository;");
                    }
                    service.AppendLine(AddSpace(6)+"}");
                    service.AppendLine("\t");

                    service.AppendLine(string.Format("{1}public override int Add({0} entity)", checkedListBox1.CheckedItems[i].ToString(), AddSpace(6)));
                    service.AppendLine(AddSpace(6)+"{");
                    service.AppendLine(AddSpace(8)+"return " + " _" + TableName + "Repository.Add(entity);");
                    service.AppendLine(AddSpace(6)+"}");
                    service.AppendLine("\t");

                    service.AppendLine(string.Format("{1}public override bool Update ({0} entity)", checkedListBox1.CheckedItems[i].ToString(), AddSpace(6)));
                    service.AppendLine(AddSpace(6)+"{");
                    service.AppendLine(string.Format("{1}return  _{0}Repository.Update(entity);", TableName, AddSpace(8)));
                     service.AppendLine(AddSpace(6)+"}");
                      service.AppendLine("\t");

                      service.AppendLine(string.Format("{1}public override bool Delete ({0} entity)", checkedListBox1.CheckedItems[i].ToString(), AddSpace(6)));
                     service.AppendLine(AddSpace(6)+"{");
                     service.AppendLine(string.Format("{1}return  _{0}Repository.Delete(entity);", TableName, AddSpace(8)));
                     service.AppendLine(AddSpace(6)+"}");
                     service.AppendLine("\t");
                    string primaryKey = GetPrimaryKey(checkedListBox1.CheckedItems[i].ToString());
                    //string PrimaryKeyValue = primaryKey.Replace("\r\n");

                    service.AppendLine(string.Format("{2}public {0} Get (int {1})", checkedListBox1.CheckedItems[i].ToString(), primaryKey.Replace("\r\n", ""), AddSpace(6)));
                    service.AppendLine(AddSpace(6)+"{");
                    service.AppendLine(string.Format("{2}return  _{0}Repository.Get({1});", TableName, primaryKey.Replace("\r\n", ""), AddSpace(8)));
                     service.AppendLine(AddSpace(6)+"}");
                     service.AppendLine("\t");

                     service.AppendLine(string.Format("{1}public List<{0}> GetAll ()", checkedListBox1.CheckedItems[i].ToString(), AddSpace(6)));
                     service.AppendLine(AddSpace(6)+"{");
                     service.AppendLine(string.Format("{1}return _{0}Repository.GetAll();", TableName, AddSpace(8)));
                     service.AppendLine(AddSpace(6)+"}");
                     service.AppendLine("\t");

                    service.AppendLine(AddSpace(4)+"}");
                    service.AppendLine("}");
                    




               System.IO.File.AppendAllText(directory + checkedListBox1.CheckedItems[i].ToString() +"Service" +".cs",service.ToString());

                }



            }
            if (generateIRepository.Checked)
            {

                string namespaces = IRepositoryNamespace.Text;
                string directory = IRepositoryDirectory.Text;
                StringBuilder IRepository = new StringBuilder();
                IRepository.AppendLine(string.Format("namespace {0}", namespaces));

                IRepository.AppendLine("{");
               // IRepository.AppendLine("\t");

                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    String primaryKey = GetPrimaryKey(checkedListBox1.CheckedItems[i].ToString());
                    string lowerPrimaryKey = FirstCharToLower(primaryKey);
                    IRepository.AppendLine("\t");
                    IRepository.AppendLine(string.Format("{1}public interface  I{0}Repository : I{0}<{0}>", checkedListBox1.CheckedItems[i].ToString(),AddSpace(4)));
                    IRepository.AppendLine(AddSpace(4)+"{");
                    IRepository.AppendLine("\t");
                    List<ForeignKeyMap> keys = GetForeignKeyMapping(checkedListBox1.CheckedItems[i].ToString(),InputConnectionString);

                    foreach (ForeignKeyMap foreinKeys in keys)
                    {

                        string foreignKeyTableName = foreinKeys.PKTable;
                        IRepository.AppendLine(AddSpace(6)+"I" + foreignKeyTableName + "Repository    "   + foreignKeyTableName + "Repository" +"{ get;}");
                    }

                    IRepository.AppendLine(string.Format("{2}{0} Get(int {1});", checkedListBox1.CheckedItems[i].ToString(), lowerPrimaryKey.Replace("\r\n", ""), AddSpace(6)));
                    IRepository.AppendLine(string.Format("{1}List<{0}>  GetAll();", checkedListBox1.CheckedItems[i].ToString(), AddSpace(6)));
                    IRepository.AppendLine("\t");
                    IRepository.AppendLine(AddSpace(4)+"}");
                    IRepository.AppendLine("\t");
                    IRepository.AppendLine("}");
                    System.IO.File.AppendAllText(directory + "I" + checkedListBox1.CheckedItems[i].ToString() +"Repository" +".cs",IRepository.ToString());

                }


            }





            if (generateIService.Checked)
            {
                string nameSpaces = IServiceNamespaces.Text;
                string directory = IServiceDirectory.Text;
                StringBuilder Iservice = new StringBuilder();
                StringBuilder Iservices = new StringBuilder();
                CodeGenerator.Utilities.Crud.Generate generate = new CodeGenerator.Utilities.Crud.Generate();

                Iservice.AppendLine(string.Format("namespace {0}", nameSpaces));
                Iservice.AppendLine("{");
                for(int i= 0; i < checkedListBox1.CheckedItems.Count; i++)
                    
                {
                     String primaryKey = GetPrimaryKey(checkedListBox1.CheckedItems[i].ToString());
                     Iservice.AppendLine(string.Format("{1}public interface I{0}Service : IEntityService<{0}>", checkedListBox1.CheckedItems[i].ToString(), AddSpace(4)));
                    Iservice.AppendLine(AddSpace(4)+"{");
                    Iservice.AppendLine("\t");
                    Iservice.AppendLine(string.Format("{2}{0} Get( int {1});", checkedListBox1.CheckedItems[i].ToString(), primaryKey.Replace("\r\n", ""), AddSpace(6)));
                    Iservice.AppendLine(string.Format("{1}List<{0}>  GetAll();", checkedListBox1.CheckedItems[i].ToString(), AddSpace(6)));
                    Iservice.AppendLine("\t");
                    Iservice.AppendLine(AddSpace(4) + "}");
                    Iservice.AppendLine("}");
                    

                   System.IO.File.AppendAllText(directory +"I"+ checkedListBox1.CheckedItems[i].ToString() +"Service"+ ".cs", Iservice.ToString());
                }

                

                //System.IO.File.AppendAllText(directory + "I" + checkedListBox1.CheckedItems[i].ToString() + "Service" + ".cs", Iservices.ToString());


            }
           
            if (generateClass.Checked)
            {
                string classNamespaces = classNamespace.Text.ToString();
                string classFilePath = classDirectory.Text.ToString();
                string ClassUsingReference = UsingReferenceClassText.Text;
                StringBuilder classReference = new StringBuilder();
                StringBuilder usingReferenceClass = new StringBuilder();
                for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                {
                    classReference.AppendLine(ClassUsingReference );
                    classReference.AppendLine("namespace " + classNamespaces);
                    classReference.AppendLine("{");

                    CodeGenerator.Utilities.Class.Generate generate = new CodeGenerator.Utilities.Class.Generate();

                    System.IO.File.AppendAllText(classFilePath + checkedListBox1.CheckedItems[i].ToString()  + ".cs", classReference.ToString());
                    if (System.IO.Directory.Exists(classFilePath))
                    {
                        generate.GenerateTableColumns(checkedListBox1.CheckedItems[i].ToString(), InputConnectionString, classFilePath);
                    }

                    usingReferenceClass.AppendLine("}");
                    System.IO.File.AppendAllText(classFilePath + checkedListBox1.CheckedItems[i].ToString() + ".cs", usingReferenceClass.ToString());


                }
            }

            if (generateCRUD.Checked)
            {
                string crudFilePath = crudDirectory.Text.ToString();
                string namespacesText = crudNamespace.Text.ToString();
                string UsingRefrenceCrud = UsingReferenceTextBox.Text;
                CodeGenerator.Utilities.Crud.Generate crud = new CodeGenerator.Utilities.Crud.Generate();
                saveCRUD(crudFilePath, namespacesText, crud, UsingRefrenceCrud);

                
            }

            if (generateStoredProcedure.Checked)
            {
                
                string storedProcedurePath = spDirectory.Text.ToString();
                CodeGenerator.Utilities.Stored_Procedure.Generate storedProcedure = new CodeGenerator.Utilities.Stored_Procedure.Generate();
                saveProcedure(storedProcedurePath, storedProcedure);

            }
        }
        private void generateCRUD_Click(object sender, EventArgs e)
        {
            if (generateCRUD.Checked)
            {
                groupCRUD.Enabled = true;
            }
            else
            {
                groupCRUD.Enabled = false;
            }

        }
        private void BrowseService_Click(object sender, EventArgs e)
        {
            DialogResult folder = ServicefolderBrowserDialog4.ShowDialog();
            string text = ServicefolderBrowserDialog4.SelectedPath + "\\";
            ServiceDirectory.Text = text;


        }

        private void IServiceBrowse_Click(object sender, EventArgs e)
        {
            DialogResult folder = IServicefolderBrowserDialog4.ShowDialog();
            string Iservice = IServicefolderBrowserDialog4.SelectedPath + "\\";
            IServiceDirectory.Text = Iservice;
        }

        private void IRepositoryBrowse_Click(object sender, EventArgs e)
        {
            DialogResult folder = IRepositoryfolderBrowserDialog4.ShowDialog();
            string text = IRepositoryfolderBrowserDialog4.SelectedPath + "\\";
            IRepositoryDirectory.Text = text.ToString();
        }
        private void generateStoredProcedure_Click(object sender, EventArgs e)
        {
            if (generateStoredProcedure.Checked)
            {
                groupProcedure.Enabled = true;
            }
            else
            {
                groupProcedure.Enabled = false;
            }

        }



        #region METHOD

        private void saveProcedure(string storedProcedurePath, CodeGenerator.Utilities.Stored_Procedure.Generate storedProcedure)
        {
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {

                if (addStoredProcedure.Checked)
                {
                    storedProcedure.AddProcedure(checkedListBox1.CheckedItems[i].ToString(), InputConnectionString, storedProcedurePath);
                }
                if (updateStoredProcedure.Checked)
                {
                    storedProcedure.UpdateProcedure(checkedListBox1.CheckedItems[i].ToString(), InputConnectionString, storedProcedurePath);

                }
                if (deleteStoredProcedure.Checked)
                {
                    storedProcedure.DeleteProcedure(checkedListBox1.CheckedItems[i].ToString(), InputConnectionString, storedProcedurePath);

                }
                if (getStoredProcedure.Checked)
                {
                    storedProcedure.GetProcedure(checkedListBox1.CheckedItems[i].ToString(), InputConnectionString, storedProcedurePath);

                }
                if (getAllStored.Checked)
                {
                    storedProcedure.GetAllProcedure(checkedListBox1.CheckedItems[i].ToString(), InputConnectionString, storedProcedurePath);

                }
            }
        }

        private static string FirstCharToLower(string data)
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

        private void saveCRUD(string crudFilePath, string namespacesText, CodeGenerator.Utilities.Crud.Generate crud, string UsingRefrenceCrud)
        {
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                StringBuilder pr = new StringBuilder();
                StringBuilder pb = new StringBuilder();
                pr.AppendLine(UsingRefrenceCrud);
                pr.AppendLine(string.Format("namespace {0}", namespacesText));
                pr.AppendLine(string.Format("{{ \r\n"));
                pr.AppendLine(string.Format("{0}public class UserRepository", AddSpace(1)));

                pr.AppendLine(string.Format("{0} {{", AddSpace(2)));
                pr.AppendLine(string.Format("\t"));
                pr.AppendLine(string.Format("{0}private string _connectionString;", AddSpace(4)));
                pr.AppendLine(string.Format("{0}private readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);", AddSpace(4)));
                pr.AppendLine(string.Format("\t"));
                pr.AppendLine(string.Format("{0}public UserRepository (string  ConnectionString)", AddSpace(4)));
                pr.AppendLine(string.Format("{0}{{", AddSpace(4)));
                pr.AppendLine(string.Format("{1}this._connectionString = @\"{0}\";", InputConnectionString, AddSpace(6)));
                pr.AppendLine(AddSpace(4) + "}");
                List<ForeignKeyMap> keys = GetForeignKeyMapping(checkedListBox1.CheckedItems[i].ToString(), InputConnectionString);
                foreach (ForeignKeyMap foreignKeys in keys)
                {
                    string foreignKey = FirstCharToLower(foreignKeys.PKTable);
                    pr.AppendLine(AddSpace(6) + "private " + "I" + foreignKeys.PKTable + "Repository  " + "_" + foreignKey + "Repository;");
                    List<ForeignKeyMap> KeySecondLevel = GetForeignKeyMapping(foreignKeys.PKTable, InputConnectionString);
                    foreach (ForeignKeyMap foreignKeyLevelTwo in KeySecondLevel)
                    {
                        string foreignKeyLevelTwoName = FirstCharToLower(foreignKeyLevelTwo.PKTable);
                        pr.AppendLine(AddSpace(6) + "private " + "I" + foreignKeyLevelTwo.PKTable + "Repository  " + "_" + foreignKeyLevelTwoName + "Repository;");

                    }
                    List<ForeignKeyMap> KeyThirdLevel = GetForeignKeyMapping(foreignKeys.PKTable, InputConnectionString);
                    foreach (ForeignKeyMap foreignKeyLevelThree in KeyThirdLevel)
                    {
                        string foreignKeyLevelThreeName = FirstCharToLower(foreignKeyLevelThree.PKTable);
                        pr.AppendLine(AddSpace(6) + "private " + "I" + foreignKeyLevelThree.PKTable + "Repository  " + "_" + foreignKeyLevelThreeName + "Repository;");

                    }



                   
     
                }
                foreach (ForeignKeyMap foreignKeys in keys)
                {
                    string foreignKey = FirstCharToLower(foreignKeys.PKTable);
                    pr.AppendLine(AddSpace(6) + "public " + "I" + foreignKeys.PKTable + "Repository  " + foreignKeys.PKTable + "Repository");

                    pr.AppendLine(string.Format("{0} {{", AddSpace(6)));
                    pr.AppendLine(string.Format("\t"));
                    pr.AppendLine(AddSpace(8) + "get");
                    pr.AppendLine(string.Format("{0} {{", AddSpace(8)));
                    pr.AppendLine(string.Format("\t"));
                    pr.AppendLine(AddSpace(10) + "if" + "(" + "_" + foreignKey + "Repository" + " == " + "null" + ")");
                    pr.AppendLine(AddSpace(10) + "_" + foreignKey + "Repository" + " = " + "new" + foreignKeys.PKTable + "(" + "_connectionString" + ")"+";");
                    pr.AppendLine(AddSpace(10) + "return " + "_" + foreignKey + "Repository;");
                    pr.AppendLine(string.Format("\t"));
                    pr.AppendLine(string.Format("{0} }}", AddSpace(8)));
                    pr.AppendLine(string.Format("\t"));
                    pr.AppendLine(string.Format("{0} }}", AddSpace(6)));
                    pr.AppendLine(string.Format("\t"));
                    
                }
                System.IO.File.AppendAllText(crudFilePath + checkedListBox1.CheckedItems[i].ToString() + "Repository" + ".cs", pr.ToString());
                if (add.Checked)
                {

                    crud.Add(checkedListBox1.CheckedItems[i].ToString(), InputConnectionString, crudFilePath);

                }
                if (update.Checked)
                {

                    crud.Update(checkedListBox1.CheckedItems[i].ToString(), InputConnectionString, crudFilePath);


                }
                if (delete.Checked)
                {

                    crud.Delete(checkedListBox1.CheckedItems[i].ToString(), InputConnectionString, crudFilePath);

                }
                if (get.Checked)
                {

                    crud.Get(checkedListBox1.CheckedItems[i].ToString(), InputConnectionString, crudFilePath);

                }
                if (getAll.Checked)
                {

                    crud.GetAll(checkedListBox1.CheckedItems[i].ToString(), InputConnectionString, crudFilePath);


                }

                pb.AppendLine(string.Format("\t"));
                pb.AppendLine(string.Format("\t"));
                pb.AppendLine(string.Format("\t"));

                pb.AppendLine(AddSpace(1) + "}");
                pb.AppendLine(AddSpace(1) + "}");

                System.IO.File.AppendAllText(crudFilePath + checkedListBox1.CheckedItems[i].ToString() + "Repository" + ".cs", pb.ToString());
            }
        }
                
       
        public  List<ForeignKeyMap> GetForeignKeyMapping(string tableName,string connectionString)
        {
            List<ForeignKeyMap> mappings = new List<ForeignKeyMap>();

            //string connectionString = "Data Source=.\\SQLEXPRESS1; Initial Catalog=Northwind;Integrated Security=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
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

        #endregion


    }
}
