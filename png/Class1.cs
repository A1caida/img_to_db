using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace img_to_db
{
    class db
    {
        MySqlConnection Connection;

        public db(string server, string user, string pass, string database)
        {
            MySqlConnectionStringBuilder Connect = new MySqlConnectionStringBuilder
            {
                Server = server,
                UserID = user,
                Password = pass,
                Port = 3306,
                Database = database,
                CharacterSet = "utf8"
            };
            Connection = new MySqlConnection(Connect.ConnectionString);
        }
        public DataTable getTableInfoo(string query)
        {
            MySqlCommand queryExecute = new MySqlCommand(query, Connection);
            DataTable ass = new DataTable();
            Connection.Open();
            ass.Load(queryExecute.ExecuteReader());
            Connection.Close();
            return ass;

        }

        public int up(string flop)
        {
            MainWindow imgg = new MainWindow();
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();           
            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();
            string filename = "";
            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                filename = dlg.FileName;
            }


            var userImage = imgg.ImageToByteArray(Image.FromFile(@filename));
            
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "INSERT INTO img_kek(name,img,filesize) VALUES(?name, @img, ?filesize)"; 
            command.Parameters.AddWithValue("@img", userImage);
            command.Parameters.Add("?filesize", MySqlDbType.Int32).Value = userImage.Length;

            try
            {
                Connection.Open();
                command.ExecuteNonQuery();

                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return -1;

        }


        public System.Drawing.Image upp(int id)
        {
            MemoryStream ms;
            
            int floppa = 0;
            MySqlCommand command = Connection.CreateCommand();
            command.CommandText = "SELECT * FROM img_kek WHERE id = "+ id;
            //MySqlDataReader myData;
            byte[] rawData;
            try
            {
                Connection.Open();
                using (MySqlDataReader myData = command.ExecuteReader())
                {
                    myData.Read();
                    floppa = myData.GetInt32(myData.GetOrdinal("filesize"));
                    rawData = new byte[floppa];
                    myData.GetBytes(myData.GetOrdinal("img"), 0, rawData, 0, (Int32)floppa);
                    ms = new MemoryStream(rawData);
                    System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
                    Connection.Close();
                    return returnImage;
                }
               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Connection.Close();
            return null;
        }
    }
}