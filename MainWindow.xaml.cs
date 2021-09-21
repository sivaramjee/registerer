using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Management;
using System.Security.Cryptography;
using Microsoft.Win32;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace registerer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // Insert code required on object creation below this point.
        }

        private static string key = "ABCDEFGHIJKLMNOPQRSTUVWXYZSDERFD";
        private static string IV = "QWESDERFVBGTYHNJ";
        private static string key2 = "!@#$%^&*()_+qwertyuiopasdfghjkl;";
        private static string IV2 = "'$.,mnbvcxza<>?:";



        public static string encrypttext1(string text)
        {
            byte[] ptbyte = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encrypted = crypto.TransformFinalBlock(ptbyte, 0, ptbyte.Length);
            crypto.Dispose();
            return Convert.ToBase64String(encrypted);
        }
        public static string encrypttext2(string text)
        {
            byte[] ptbyte = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(key2);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV2);
            aes.Padding = PaddingMode.ISO10126;
            aes.Mode = CipherMode.ECB;
            ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encrypted = crypto.TransformFinalBlock(ptbyte, 0, ptbyte.Length);
            crypto.Dispose();
            return Convert.ToBase64String(encrypted);
        }

        public static string decrypttext1(string encrypted)
        {
            byte[] ebyte = Convert.FromBase64String(encrypted);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] secret = crypto.TransformFinalBlock(ebyte, 0, ebyte.Length);
            crypto.Dispose();
            return System.Text.ASCIIEncoding.ASCII.GetString(secret);
        }
        public static string decrypttext2(string encrypted)
        {
            byte[] ebyte = Convert.FromBase64String(encrypted);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(key2);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(IV2);
            aes.Padding = PaddingMode.ISO10126;
            aes.Mode = CipherMode.ECB;
            ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] secret = crypto.TransformFinalBlock(ebyte, 0, ebyte.Length);
            crypto.Dispose();
            return System.Text.ASCIIEncoding.ASCII.GetString(secret);
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                dfs.Clear();
                string tomull = t1.Text.ToString().Trim();
                //MessageBox.Show(tomull.ToString());
                string s = encrypttext2(encrypttext1(tomull));

                foreach (char item in t1.Text.ToString().Trim())
                {
                    dfs.Add(item.ToString());
                }

                //List<string> dd = dfs.GetRange(10, 16);

                //string dd1 = t1.Text.Remove(0, 10);


                //string bb = "";

                //foreach (var item in dd)
                //{
                //    bb += item;
                //}


                for (int i = 0; i < 10; i++)
                {
                    dfs.RemoveAt(0);
                }

                for (int i = 0; i < 10; i++)
                {

                    dfs.RemoveAt(dfs.Count - 1);
                }

                string bb = "";

                foreach (var item in dfs)
                {
                    bb += item;
                }

                exist = false;
                t2.Text = s;
                UID.Text = "";
                textBox.Text = "";
                textBox_Copy.Text = "";
                textBox_Copy1.Text = "";
                textBox_Copy2.Text = "";
                textBox_Copy3.Text = "";
                textBox_Copy4.Text = "";
                textBox_Copy5.Text = "";
                try
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select * from Key_Service where (ProcessorID = @ProcessorID)", con);
                    cmd.Parameters.AddWithValue("@ProcessorID", bb);
                    // cmd.Parameters.AddWithValue("@Auth_Code", Key_Code);
                    //cmd.Parameters.AddWithValue("@password", password);

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    System.Data.DataTable dt = new System.Data.DataTable();
                    sda.Fill(ds);
                    sda.Fill(dt);
                    dataGrid.ItemsSource = dt.DefaultView;
                    // sda.Fill(ds);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    Key_Service ff = ds.Tables[0].AsEnumerable().Select(a => new Key_Service()
                    {
                        Count = a.Field<int>("Count"),
                        ProcessorID = a.Field<string>("ProcessorID"),
                        PCName = a.Field<string>("PCName"),
                        UserName = a.Field<string>("UserName"),
                        Location = a.Field<string>("Location"),
                        Key_Code = a.Field<string>("Key_Code"),
                        Authentication_Code = a.Field<string>("Authentication_Code"),
                        Permission_Level = a.Field<int>("Permission_Level"),
                        Active = a.Field<int>("Active")

                    }).FirstOrDefault();
                    if (ff.Count != 0 || ff != null)
                    {


                        UID.Text = ff.ProcessorID;
                        textBox.Text = ff.PCName;
                        textBox_Copy.Text = ff.UserName;
                        textBox_Copy1.Text = ff.Location;
                        textBox_Copy2.Text = ff.Key_Code;
                        textBox_Copy3.Text = ff.Authentication_Code;
                        textBox_Copy4.Text = ff.Permission_Level.ToString();
                        textBox_Copy5.Text = ff.Active.ToString();
                    }
                    if (ff.Count != 0 && ff != null)
                    {
                        exist = true;
                        MessageBox.Show("ID Exist");
                        //return ff.Permission_Level;
                    }
                    else
                    {
                        //return 0;
                    }

                }
                catch
                {
                    //  return 0;

                }
                //MessageBox.Show(s);
                textBox_Copy2.Text = t1.Text;
                textBox_Copy3.Text = t2.Text;
                //t2.Text = decrypttext1(decrypttext2(s));
            }
            catch
            {
                MessageBox.Show("Error");
            } 
        }
        bool exist = false;
        List<string> dfs = new List<string>();
        SqlConnection con = new SqlConnection(@"Data Source=52.74.124.33;Initial Catalog=Brimhast_Key_Service;User ID=followon;Password=Techfront@TF");
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                dfs.Clear();
                foreach (char item in t1.Text.ToString().Trim())
                {
                    dfs.Add(item.ToString());
                }

             //   List<string> dd = dfs.GetRange(10, 16);

                for (int i = 0; i < 10; i++)
                {
                    dfs.RemoveAt(0);
                }

                for (int i = 0; i < 10; i++)
                {

                    dfs.RemoveAt(dfs.Count-1);
                }

                string bb = "";

                foreach (var item in dfs)
                {
                    bb += item;
                }

                con.Open();
                SqlCommand cmd;
                if (exist)
                {
                    cmd = new SqlCommand("update Key_Service set Key_Code =@Key_Code,Authentication_Code =@Authentication_Code,PCName =@PCName,UserName =@UserName,Location =@Location,Permission_Level =@Permission_Level,Active =@Active where ProcessorID = @ProcessorID", con);
                }
                else
                {


                    cmd = new SqlCommand("insert into Key_Service (ProcessorID,Key_Code,Authentication_Code,PCName,UserName,Location,Permission_Level,Active) values ( @processorid,@Key_Code,@Authentication_Code,@PCName,@UserName,@Location,@Permission_Level,@Active)", con);
                }
                // SqlCommand cmd = con.CreateCommand();
                // cmd.CommandText = "Execute Key_LogService @processorid,@Username,@Localtime";

                cmd.Parameters.AddWithValue("@processorid", bb);
                cmd.Parameters.AddWithValue("@Key_Code", t1.Text);
                cmd.Parameters.AddWithValue("@Authentication_Code", t2.Text);
                cmd.Parameters.AddWithValue("@PCName", textBox.Text);
                cmd.Parameters.AddWithValue("@UserName", textBox_Copy.Text);
                cmd.Parameters.AddWithValue("@Location", textBox_Copy1.Text);
                cmd.Parameters.AddWithValue("@Permission_Level", textBox_Copy4.Text);
                cmd.Parameters.AddWithValue("@Active", textBox_Copy5.Text);




                //cmd.Parameters.AddWithValue("@Localtime", Localtime);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                //DataSet ds = new DataSet();

                //sda.Fill(ds);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("select * from Key_Service", con);
                //cmd.Parameters.AddWithValue("@ProcessorID", bb);
                // cmd.Parameters.AddWithValue("@Auth_Code", Key_Code);
                //cmd.Parameters.AddWithValue("@password", password);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                System.Data.DataTable dt = new System.Data.DataTable();
                sda.Fill(ds);
                sda.Fill(dt);
                dataGrid.ItemsSource = dt.DefaultView;
                // sda.Fill(ds);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch
            {

            }

            }
    }

    class Key_Service
    {
        private int _count;

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        private string _processorID;

        public string ProcessorID
        {
            get { return _processorID; }
            set { _processorID = value; }
        }

        private string _pcname;

        public string PCName
        {
            get { return _pcname; }
            set { _pcname = value; }
        }

        private string _username;

        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        private string _location;

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        private string _key_code;

        public string Key_Code
        {
            get { return _key_code; }
            set { _key_code = value; }
        }

        private string _authentication_code;

        public string Authentication_Code
        {
            get { return _authentication_code; }
            set { _authentication_code = value; }
        }

        private int _permission_level;

        public int Permission_Level
        {
            get { return _permission_level; }
            set { _permission_level = value; }
        }

        private int _active;

        public int Active
        {
            get { return _active; }
            set { _active = value; }
        }


        class Key_Service_Log
        {
            private int _count;

            public int Count
            {
                get { return _count; }
                set { _count = value; }
            }

            private string _processor_ID;

            public string Processor_ID
            {
                get { return _processor_ID; }
                set { _processor_ID = value; }
            }

            private string _Log_DateTime_Server;

            public string Log_DateTime_Server
            {
                get { return _Log_DateTime_Server; }
                set { _Log_DateTime_Server = value; }
            }

            private string _username;

            public string UserName
            {
                get { return _username; }
                set { _username = value; }
            }

            private string _Log_DateTime_Local;

            public string Log_DateTime_Local
            {
                get { return _Log_DateTime_Local; }
                set { _Log_DateTime_Local = value; }
            }
        }
    }

}