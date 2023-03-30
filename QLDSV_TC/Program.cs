using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using System.Data;

namespace QLDSV_TC
{

    static class Program
    {
        public static frmMain formMain;
        public static SqlConnection conn = new SqlConnection();
        public static string connstr = "";
        public static string connstr_publisher = "Data Source=DESKTOP-EQ3HK6B; Initial Catalog=QLDSV_TC; Integrated Security=True";
        public static SqlDataReader myReader;
        public static BindingSource bds_dspm = new BindingSource();

        public static string servername;
        public static string database;


        public static string username;
        public static string mHoTen;
        public static string mChucVu;

        public static string mloginDN;
        public static string passwordDN;

        public static int mKhoa = 0;

        public static int KetNoi()
        {
            if (Program.conn != null && Program.conn.State == ConnectionState.Open)
                // đóng đối tượng kết nối
                Program.conn.Close();
            try
            {
                Program.connstr = "Data Source=" + Program.servername + ";Initial Catalog=" +
                      Program.database + ";User ID=" +
                      Program.mloginDN + ";Password=" + Program.passwordDN;
                Program.conn.ConnectionString = Program.connstr;

                // mở đối tượng kết nối
                Program.conn.Open();
                return 1;
            }

            catch (Exception e)
            {
                XtraMessageBox.Show("---> Lỗi kết nối cơ sở dữ liệu.\n---> Bạn xem lại Username và Password.\n " + e.Message, string.Empty, MessageBoxButtons.OK);
                return 0;
            }
        }

        public static SqlDataReader ExecSqlDataReader(String strLenh)
        {
            SqlDataReader myReader;
            SqlCommand sqlcmd = new SqlCommand("exec dbo.SP_DANGNHAP 'GV02'", Program.conn);

            //xác định kiểu lệnh cho sqlcmd là kiểu text.
            sqlcmd.CommandType = CommandType.Text;
            sqlcmd.CommandTimeout = 600;
            if (Program.conn.State == ConnectionState.Closed) Program.conn.Open();
            try
            {
                myReader = sqlcmd.ExecuteReader();
                return myReader;
            }
            catch (SqlException ex)
            {
                Program.conn.Close();
                XtraMessageBox.Show(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            formMain = new frmMain();
            Application.Run(formMain);
        }
    }
}
