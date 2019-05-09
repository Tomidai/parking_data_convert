using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommLog_Convert {
    class Program {
        static void Main(string[] args) {
            /*DateTime closeDateTime = new DateTime();
            DateTime addCloseDateTime = closeDateTime.AddDays(+1);

            MySqlConnection con = new MySqlConnection(
                string.Format("Data Source={0};Database={1};User ID={2};password={3}",
                               AppSet.Default.DataSource,
                               AppSet.Default.Database,
                               AppSet.Default.UserID,
                               AppSet.Default.password));
            con.Open();
            MySqlCommand cmd = new MySqlCommand(SQL.GET_CLOSE_DATETIME,con);
            MySqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read()) {
                closeDateTime = reader.GetDateTime(1);
            }
            con.Close();
            

            string[] fileName = Directory.GetFiles(
                AppSet.Default.folderPath,"CommunicationLog*",SearchOption.TopDirectoryOnly);
            foreach(string file in fileName) {
                DateTime nowDateTime = DateTime.Now;
                nowDateTime = nowDateTime.Date;

                string fileDate = Path.GetExtension(file);
                fileDate = fileDate.Substring(1,fileDate.Length - 1);
                string month = fileDate.Substring(0,1);
                string day = fileDate.Substring(1,2);
                switch(month) {
                    case "a":
                        month = "10";
                        break;
                    case "b":
                        month = "11";
                        break;
                    case "c":
                        month = "12";
                        break;
                    default:
                        month = "0" + month;
                        break;
                }
                DateTime fdt = File.GetCreationTime(file);
                DateTime fileDateTime = DateTime.Parse(fdt.Year + "/" + month + "/" + day);

                if(fileDateTime == closeDateTime || fileDateTime == addCloseDateTime) {
                    Console.WriteLine(fileDateTime);

                    comm22 comm = new comm22();
                    using(StreamReader sr = new StreamReader(file)) {
                        while(!sr.EndOfStream) {
                            string readLine = sr.ReadLine();
                            if(readLine.Substring(21,1) == "R" && readLine.Substring(27,2) == "13") {
                                int length = readLine.Length - 23;
                                comm.convert22(readLine);
                            } else if(readLine.Substring(21,1) == "R" && readLine.Substring(27,2) == "10") {
                                int length = readLine.Length - 23;
                                comm.convert21(readLine);
                            } else if(readLine.Substring(21,1) == "R" && readLine.Substring(27,2) == "23") {
                                comm.convert27(readLine);
                            }
                        }
                    }
                }
            }*/

            //参照するファイル名宣言
            string[] files = Directory.GetFiles(
                AppSet.Default.folderPath,"CommunicationLog*",SearchOption.TopDirectoryOnly);
            comm22 comm = new comm22();
            foreach(string file in files) {
                Console.WriteLine(file);
                using(StreamReader sr = new StreamReader(file)) {
                    while(!sr.EndOfStream) {
                        string readLine = sr.ReadLine();
                        if(readLine.Substring(21,1) == "R" && readLine.Substring(27,2) == "13") {
                            int length = readLine.Length - 23;
                            comm.convert22(readLine);
                        }else if(readLine.Substring(21,1) == "R" && readLine.Substring(27,2) == "10") {
                            int length = readLine.Length - 23;
                            comm.convert21(readLine);
                        }else if(readLine.Substring(21,1) == "R" && readLine.Substring(27,2) == "23") {
                            comm.convert27(readLine);
                        }
                    }
                }
            }
            Console.WriteLine("何かキーを押して終了してください。");
            Console.ReadLine();
        }
    }
}
