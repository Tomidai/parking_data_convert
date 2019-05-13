using MySql.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommLog_Convert {
    class comm22 {
        public void convert22(string str) {
            string readLine = str.Substring(23,str.Length - 23);

            //正常フラグ
            string fs_flag = "T";

            //取集日時
            string str_fs_time = "20" + str.Substring(0,2) + readLine.Substring(20,8);
            DateTime fs_time = DateTime.ParseExact(str_fs_time,"yyyyMMddHHmm",null);

            //端末No.
            string terminal_number = readLine.Substring(6,2);

            //コマンドコード
            string command = "22";


            //券種判定
            string ticket_type = "00";
            if(readLine.Substring(30,4) == "0000") {
                ticket_type = "1";
            } else {
                ticket_type = "2";
            }

            //車種判定
            string car_type = "00";
            switch(ticket_type) {
                case "1":
                    car_type = readLine.Substring(50,2);
                    break;
                case "2":
                    car_type = readLine.Substring(28,2);
                    break;
            }

            //出庫日時
            string str_leaving_shedcol_time = "20" + str.Substring(0,2) + readLine.Substring(20,8);
            DateTime leaving_shedcol_time = DateTime.ParseExact(str_leaving_shedcol_time,"yyyyMMddHHmm",null);

            //定期オーバー車種
            string commuter_pass_over = "0";
            //データ長が72以下の場合、現金売上が0だからオーバー車種にならない
            if(readLine.Length <= 72) {

            } else {
                if((readLine.Substring(30,4) != "0000") && (readLine.Substring(66,6) != "000000")) {
                    commuter_pass_over = "1";
                }
            }

            //出庫券No.
            string leaving_shedcol_ticket;
            //定期チェックをする
            if(readLine.Substring(28,6) == "000000") {
                leaving_shedcol_ticket = readLine.Substring(54,4);
            } else {
                leaving_shedcol_ticket = readLine.Substring(30,4);
            }

            //現金請求金額
            string cash_unpaid = "000000";
            if(readLine.Length <= 72) {

            } else {
                cash_unpaid = readLine.Substring(66,6);
            }

            //未集金額
            string accrued_payable = "000000";

            //回数券使用金額
            string coupon_ticket_used = "000000";

            int coupon_1, coupon_2, coupon_3, coupon_4;
            switch(readLine.Length) {
                case 128:
                    if(readLine.Substring(114,2) == "86") {
                        accrued_payable = readLine.Substring(122,6);
                    } else {
                        coupon_ticket_used = readLine.Substring(122,6);
                    }
                    break;
                case 144:
                    coupon_1 = int.Parse(readLine.Substring(122,6));
                    coupon_2 = int.Parse(readLine.Substring(136,6));
                    coupon_ticket_used = (coupon_1 + coupon_2).ToString();
                    break;
                case 160:
                    coupon_1 = int.Parse(readLine.Substring(122,6));
                    coupon_2 = int.Parse(readLine.Substring(136,6));
                    coupon_3 = int.Parse(readLine.Substring(150,6));
                    coupon_ticket_used = (coupon_1 + coupon_2 + coupon_3).ToString();
                    break;
                case 176:
                    coupon_1 = int.Parse(readLine.Substring(122,6));
                    coupon_2 = int.Parse(readLine.Substring(136,6));
                    coupon_3 = int.Parse(readLine.Substring(150,6));
                    coupon_4 = int.Parse(readLine.Substring(164,6));
                    coupon_ticket_used = (coupon_1 + coupon_2 + coupon_3 + coupon_4).ToString();
                    break;
                default:
                    //
                    break;
            }

            //未払い金額
            string accounts_payable = "000000";

            //駐車時間
            string parking_time = "0";
            if(readLine.Length >= 66) {
                string strReceipt_time = "20" + str.Substring(0,2) + readLine.Substring(58,8);
                DateTime dt_receipt_time = DateTime.ParseExact(strReceipt_time,"yyyyMMddHHmm",null);
                TimeSpan ts = leaving_shedcol_time - dt_receipt_time;
                int hour = int.Parse(ts.ToString(@"hh"));
                hour = hour * 60;
                int min = int.Parse(ts.ToString(@"mm"));
                int time = hour + min - 5;
                if(time < 0) {
                    time = 0;
                }
                parking_time = time.ToString();

            } else if(readLine.Length == 64) {
                string strReceipt_time = "20" + str.Substring(0,2) + readLine.Substring(58,6) + "00";
                DateTime dt_receipt_time = DateTime.ParseExact(strReceipt_time,"yyyyMMddHHmm",null);
                TimeSpan ts = leaving_shedcol_time - dt_receipt_time;
                int hour = int.Parse(ts.ToString(@"hh"));
                hour = hour * 60;
                int min = int.Parse(ts.ToString(@"mm"));
                int time = hour + min - 5;
                if(time < 0) {
                    time = 0;
                }
                parking_time = time.ToString();

            } else {
                //
            }

            //発券機番号
            string ticketing_device = readLine.Substring(48,2);

            //テナントNo
            string tenant = "00";
            if(readLine.Length >= 128) {
                if(readLine.Substring(114,2) == "86") {
                    tenant = readLine.Substring(119,3);
                }
            }

            //入庫日時
            DateTime receipt_time;
            if(readLine.Length >= 66) {
                string str_receipt_time = "20" + str.Substring(0,2) + readLine.Substring(58,8);
                receipt_time = DateTime.ParseExact(str_receipt_time,"yyyyMMddHHmm",null);
            } else if(readLine.Length == 64) {
                string str_receipt_time = "20" + str.Substring(0,2) + readLine.Substring(58,6) + "00";
                receipt_time = DateTime.ParseExact(str_receipt_time,"yyyyMMddHHmm",null);
            } else {
                //ここをどうするか
                receipt_time = DateTime.MaxValue;
            }

            //宿泊割引金額
            string accommondation_discount = "000000";
            if(readLine.Length >= 78) {
                accommondation_discount = readLine.Substring(72,6);
            }

            //zNo
            string z_2 = "0";

            //1連No
            string series_2 = "0";

            //回数券使用明細
            string coupon_ticket_used_details_1 = "000000";
            string coupon_ticket_used_details_2 = "000000";
            string coupon_ticket_used_details_3 = "000000";
            string coupon_ticket_used_details_4 = "000000";
            if(readLine.Length >= 128) {
                switch(readLine.Substring(118,4)) {
                    case "1000":
                        coupon_ticket_used_details_1 = readLine.Substring(122,6);
                        break;
                    case "2000":
                        coupon_ticket_used_details_2 = readLine.Substring(122,6);
                        break;
                    case "3000":
                        coupon_ticket_used_details_3 = readLine.Substring(122,6);
                        break;
                    case "4000":
                        coupon_ticket_used_details_4 = readLine.Substring(122,6);
                        break;
                }
            }
            if(readLine.Length >= 144) {
                switch(readLine.Substring(132,4)) {
                    case "1000":
                        coupon_ticket_used_details_1 = readLine.Substring(136,6);
                        break;
                    case "2000":
                        coupon_ticket_used_details_2 = readLine.Substring(136,6);
                        break;
                    case "3000":
                        coupon_ticket_used_details_3 = readLine.Substring(136,6);
                        break;
                    case "4000":
                        coupon_ticket_used_details_4 = readLine.Substring(136,6);
                        break;
                }
            }
            if(readLine.Length >= 160) {
                switch(readLine.Substring(146,6)) {
                    case "1000":
                        coupon_ticket_used_details_1 = readLine.Substring(150,6);
                        break;
                    case "2000":
                        coupon_ticket_used_details_2 = readLine.Substring(150,6);
                        break;
                    case "3000":
                        coupon_ticket_used_details_3 = readLine.Substring(150,6);
                        break;
                    case "4000":
                        coupon_ticket_used_details_4 = readLine.Substring(150,6);
                        break;
                }
            }
            if(readLine.Length >= 176) {
                switch(readLine.Substring(160,6)) {
                    case "1000":
                        coupon_ticket_used_details_1 = readLine.Substring(164,6);
                        break;
                    case "2000":
                        coupon_ticket_used_details_2 = readLine.Substring(164,6);
                        break;
                    case "3000":
                        coupon_ticket_used_details_3 = readLine.Substring(164,6);
                        break;
                    case "4000":
                        coupon_ticket_used_details_4 = readLine.Substring(164,6);
                        break;
                }
            }

            MySqlConnection con = new MySqlConnection(
                string.Format("Data Source={0};Database={1};User ID={2};password={3}",
                               AppSet.Default.DataSource,
                               AppSet.Default.Database,
                               AppSet.Default.UserID,
                               AppSet.Default.password));
            MySqlCommand cmd = new MySqlCommand(
                string.Format(SQL.COMMAND22_INSERT,
                                fs_flag,fs_time,terminal_number,command,ticket_type,
                                car_type,leaving_shedcol_time,commuter_pass_over,leaving_shedcol_ticket,cash_unpaid,
                                accrued_payable,coupon_ticket_used,accounts_payable,parking_time,ticketing_device,
                                tenant,receipt_time,accommondation_discount,z_2,series_2,
                                coupon_ticket_used_details_1,coupon_ticket_used_details_2,coupon_ticket_used_details_3,coupon_ticket_used_details_4),con);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public void convert21(string str) {
            string readLine = str.Substring(23,str.Length - 23);

            //正常フラグ
            string fs_flag = "T";

            //取集日時
            string str_fs_time = "20" + str.Substring(0,2) + readLine.Substring(20,8);
            DateTime fs_time = DateTime.ParseExact(str_fs_time,"yyyyMMddHHmm",null);

            //端末No
            string terminal = readLine.Substring(6,2);

            //コマンドコード
            string command = "21";

            //券種
            string ticket_type;
            if(readLine.Substring(16,4) == "0000") {
                ticket_type = "2";
            } else {
                ticket_type = "1";
            }

            //車種(1固定？)
            string car_type = "1";

            //入庫日時
            string str_receipt_time = "20" + str.Substring(0,2) + readLine.Substring(20,8);
            DateTime receipt_time = DateTime.ParseExact(str_receipt_time,"yyyyMMddHHmm",null);

            //駐車券No
            string parking_ticket = readLine.Substring(16,4);

            //定期券No
            string commuter_pass = readLine.Substring(30,4);

            MySqlConnection con = new MySqlConnection(
                string.Format("Data Source={0};Database={1};User ID={2};password={3}",
                               AppSet.Default.DataSource,
                               AppSet.Default.Database,
                               AppSet.Default.UserID,
                               AppSet.Default.password));
            MySqlCommand cmd = new MySqlCommand(
                string.Format(SQL.COMMAND21_INSERT,
                            fs_flag,fs_time,terminal,command,ticket_type,
                            car_type,receipt_time,parking_ticket,commuter_pass),con);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
        }

        public void convert27(string str) {
            string readLine = str.Substring(23,str.Length - 23);

            if(readLine.Substring(6,2) == "02" || readLine.Substring(6,2) == "03") {
                //正常フラグ
                string fs_flag = "T";

                //取集日時
                string str_fs_time = "20" + str.Substring(0,2) + readLine.Substring(12,8);
                DateTime fs_time = DateTime.ParseExact(str_fs_time,"yyyyMMddHHmm",null);

                //端末No
                string terminal_number = readLine.Substring(6,2);

                //コマンドコード
                string command = "27";

                //今回締め切り日時
                DateTime now_closing_date = fs_time;

                MySqlConnection con = new MySqlConnection(
                string.Format("Data Source={0};Database={1};User ID={2};password={3}",
                               AppSet.Default.DataSource,
                               AppSet.Default.Database,
                               AppSet.Default.UserID,
                               AppSet.Default.password));
                MySqlCommand cmd = new MySqlCommand(
                    string.Format(SQL.COMMSND27_INSERT,
                                fs_flag,fs_time,terminal_number,command,now_closing_date),con);
                cmd.Connection.Open();
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }

        }
    }
}
