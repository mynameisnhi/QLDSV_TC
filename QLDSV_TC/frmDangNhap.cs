using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLDSV_TC
{
    public partial class frmDangNhap : DevExpress.XtraEditors.XtraForm
    {
        private SqlConnection conn_publisher = new SqlConnection();
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(txtLogin.Text.Trim() == "" || txtPass.Text.Trim() == "")
            {
                MessageBox.Show("Tên đăng nhập và mật khẩu không được để trống");
                return;
            }

            Program.mloginDN = txtLogin.Text;
            Program.passwordDN = txtPass.Text;
            Program.servername = cmbChucVu.SelectedValue.ToString();

            if (Program.KetNoi() == 0) return;
            Program.mKhoa = cmbChucVu.SelectedIndex;

            string strlenh = "SP_DANGNHAP '" + Program.mloginDN + "'";

            Program.myReader = Program.ExecSqlDataReader(strlenh);
            MessageBox.Show(strlenh);

            if (Program.myReader == null) return;
            Program.myReader.Read();

            Program.username = Program.myReader.GetString(0);     // Lay user name
            if (Convert.IsDBNull(Program.username))
            {
                XtraMessageBox.Show("Login bạn nhập không có quyền truy cập dữ liệu\nBạn xem lại username, password", "", MessageBoxButtons.OK);
                return;
            }

            try
            {
                Program.mHoTen = Program.myReader.GetString(1);
                Program.mChucVu = Program.myReader.GetString(2);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("---> Lỗi: " + ex.ToString());
                XtraMessageBox.Show("Login bạn nhập không có quyền truy cập vào chương trình", "", MessageBoxButtons.OK);
                return;
            }

            Program.formMain.labelHoTen.Text = Program.mHoTen;
            Program.formMain.labelMa.Text = Program.username;
            Program.formMain.labelChucVu.Text = Program.mChucVu;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void LayDSPM(String cmd)
        {
            DataTable dt = new DataTable();
            if (conn_publisher.State == ConnectionState.Closed) conn_publisher.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn_publisher);
            da.Fill(dt);
            conn_publisher.Close();

            Program.bds_dspm.DataSource = dt;
            cmbChucVu.DataSource = Program.bds_dspm;
            cmbChucVu.DisplayMember = "TENKHOA"; cmbChucVu.ValueMember = "TENSEVER";
        }
        private int KetNoi_CSDLGOC()
        {
            if (conn_publisher != null && conn_publisher.State == ConnectionState.Open)
                conn_publisher.Close();
            try
            {
                conn_publisher.ConnectionString = Program.connstr_publisher;
                conn_publisher.Open();
                return 1;
            }
            catch
            {
                MessageBox.Show("Loi keet noi ve co so du lieu goc");
                return 0;
            }
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            if (KetNoi_CSDLGOC() == 0) return;
            LayDSPM("select * from Get_Subscribes");
            cmbChucVu.SelectedIndex = 0;
        }

        private void cmbChucVu_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Program.servername = cmbChucVu.SelectedValue.ToString();
            }
            catch { }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (label2.Text == "Tài khoản")
                label2.Text = "Mã sinh viên";
            else label2.Text = "Tài khoản";
        }
    }
}