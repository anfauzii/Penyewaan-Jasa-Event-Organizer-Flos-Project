using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;

namespace FlosProjects
{
    public partial class DataPesanan : Form
    {
        private MySqlCommand cmd;
        public DataPesanan()
        {
            InitializeComponent();
        }

        void ShowData()
        {
            try
            {
                Koneksi.conn.Open();
                MySqlDataAdapter Tampil = new MySqlDataAdapter("SELECT * FROM pesanan", Koneksi.conn);
                DataTable data = new DataTable();
                Tampil.SelectCommand.ExecuteNonQuery();
                Tampil.Fill(data);
                dgvPesanan.DataSource = data;
                Koneksi.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failed \n Error" + ex.Message,
               "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void Search()
        {
            try
            {
                Koneksi.conn.Open();
                MySqlDataAdapter Tampil = new MySqlDataAdapter("SELECT  * FROM pesanan where  id_pesanan like '%" + txtSearch.Text + "%' or id_barang like '%" + txtSearch.Text + "%' or id_pembayaran like '%" + txtSearch.Text + "%' or id_user like '%" + txtSearch.Text + "%' or pemilik_hajat like '%" + txtSearch.Text + "%' or lokasi_acara like '%" + txtSearch.Text + "%' or kota like '%" + txtSearch.Text + "%' ", Koneksi.conn);
                DataTable data = new DataTable();
                Tampil.SelectCommand.ExecuteNonQuery();
                Tampil.Fill(data);
                dgvPesanan.DataSource = data;
                Koneksi.conn.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Connection Failed \n Error" + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void ClearData()
        {
            txtIdPesanan.Text = "";
            cbUser.Text = null;

            pbarang.Text = "";
            txtPemilikHajat.Text = "";
            txtTgl.Text = "";
            txtPemilikHajat.Text = "";
            txtLokasi.Text = "";
            txtKota.Text = "";
            cbPembayaran.Text = null;
            cbStatus.Text = null;
        }

        void IdOtomatis()
        {
            long itung;
            string urut;
            MySqlDataReader dr;
            Koneksi.conn.Open();

            cmd = new MySqlCommand("select id_pesanan from pesanan where id_pesanan in(select max(id_pesanan) from pesanan) order by id_pesanan desc", Koneksi.conn);
            dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                itung = Convert.ToInt64(dr[0].ToString().Substring(dr["id_pesanan"].ToString().Length - 4, 4)) + 1;
                string idurut = "0000" + itung;
                urut = "BOOK" + idurut.Substring(idurut.Length - 4, 4);
            }
            else
            {
                urut = "BOOK0001";
            }
            dr.Close();
            txtIdPesanan.Text = urut;
            Koneksi.conn.Close();
        }

        private void DataPesanan_Load(object sender, EventArgs e)
        {
            ShowData();
            ClearData();
            IdOtomatis();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            DateTime  dt = new DateTime();
            dt = Convert.ToDateTime(txtTgl.Text);
            string tanggal = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString(); 

            if (txtIdPesanan.Text.Trim() == "" || cbUser.Text.Trim() == "" || pbarang.Text.Trim() == "" ||
                txtPemilikHajat.Text.Trim() == "" || txtTgl.Text.Trim() == "" || txtLokasi.Text.Trim() == "" || 
                txtKota.Text.Trim() == "" || cbPembayaran.Text.Trim() == "" || cbStatus.Text.Trim() == "")
            {
                MessageBox.Show("Data Tidak Lengkap", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    cmd = new MySqlCommand("insert into pesanan(id_pesanan,id_barang,id_user,pemilik_hajat,tgl_acara,lokasi_acara,kota,id_pembayaran,status) values('" + txtIdPesanan.Text + "', '" + pbarang.Text + "', '" + cbUser.Text + "', '" + txtPemilikHajat.Text + "', '" + tanggal + "', '" + txtLokasi.Text + "', '" + txtKota.Text + "', '" + cbPembayaran.Text + "', '" + cbStatus.Text + "')", Koneksi.conn);
                    Koneksi.conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Insert Data Berhasil", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Koneksi.conn.Close();
                    ShowData();
                    ClearData();
                     //IdOtomatis();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                finally
                {
                    Koneksi.conn.Close();
                }
            }
            
        }

        private void dgvPesanan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = this.dgvPesanan.Rows[e.RowIndex];
                txtIdPesanan.Text = row.Cells["id_pesanan"].Value.ToString();
                pbarang.Text = row.Cells["id_barang"].Value.ToString();
                cbUser.Text = row.Cells["id_user"].Value.ToString();
                txtPemilikHajat.Text = row.Cells["pemilik_hajat"].Value.ToString();
                txtTgl.Text = row.Cells["tgl_acara"].Value.ToString();
                txtLokasi.Text = row.Cells["lokasi_acara"].Value.ToString();
                txtKota.Text = row.Cells["kota"].Value.ToString();
                cbPembayaran.Text = row.Cells["id_pembayaran"].Value.ToString();
                cbStatus.Text = row.Cells["status"].Value.ToString();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            Search();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ShowData();
            ClearData();
            IdOtomatis();
        }


        private void dgvPesanan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = this.dgvPesanan.Rows[e.RowIndex];
                txtIdPesanan.Text = row.Cells["id_pesanan"].Value.ToString();
                pbarang.Text = row.Cells["id_barang"].Value.ToString();
                cbUser.Text = row.Cells["id_user"].Value.ToString();
                
                txtPemilikHajat.Text = row.Cells["pemilik_hajat"].Value.ToString();
                txtTgl.Text = row.Cells["tgl_acara"].Value.ToString();
                txtLokasi.Text = row.Cells["lokasi_acara"].Value.ToString();
                txtKota.Text = row.Cells["kota"].Value.ToString();
                cbPembayaran.Text = row.Cells["id_pembayaran"].Value.ToString();
                cbStatus.Text = row.Cells["status"].Value.ToString();
            }
            catch (Exception x)
            {
                MessageBox.Show(x.ToString());
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            DateTime dt = new DateTime();
            dt = Convert.ToDateTime(txtTgl.Text);
            string tanggal = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString();

            if (txtIdPesanan.Text.Trim() == "" || pbarang.Text.Trim() == "" || cbUser.Text.Trim() == "" || txtPemilikHajat.Text.Trim() == "" || txtTgl.Text.Trim() == "" || txtLokasi.Text.Trim() == "" || txtKota.Text.Trim() == "" || cbPembayaran.Text.Trim() == "" || cbStatus.Text.Trim() == "")
            {
                MessageBox.Show("Data Tidak Lengkap", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    Koneksi.conn.Open();
                    cmd = new MySqlCommand("update pesanan set id_barang='" + pbarang.Text + "', id_user='" + cbUser.Text + "', pemilik_hajat='" + txtPemilikHajat.Text + "', tgl_acara='" + tanggal + "', lokasi_acara='" + txtLokasi.Text + "', kota='" + txtKota.Text + "', id_pembayaran='" + cbPembayaran.Text + "', status='" + cbStatus.Text + "' where id_pesanan='" + txtIdPesanan.Text + "'", Koneksi.conn);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Update Data Berhasil", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Koneksi.conn.Close();
                    ShowData();
                    ClearData();
                    IdOtomatis();
                }
                catch (Exception x)
                {
                    MessageBox.Show(x.Message);
                }
                finally
                {
                    Koneksi.conn.Close();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Apakah Anda Yakin Booking Data : " + txtPemilikHajat.Text + " ?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cmd = new MySqlCommand("delete from pesanan where id_pesanan='" + txtIdPesanan.Text + "'", Koneksi.conn);
                    Koneksi.conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Delete Data " + txtIdPesanan.Text + " Berhasil", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Koneksi.conn.Close();
                    ShowData();
                    ClearData();
                    IdOtomatis();
                }
            }
            catch (Exception x)
            {
                MessageBox.Show(x.Message);
            }
            finally
            {
                Koneksi.conn.Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void cbUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtIdPesanan_TextChanged(object sender, EventArgs e)
        {

        }

        private void pbarang_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
