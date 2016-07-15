using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _13253065P_

{
    public partial class FormHesapKapat : Form
    {
        public FormHesapKapat()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con;
            SqlCommand cmd;

            SqlConnection CN = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM hesaplar where hesap_no=" + textBox1.Text, CN);
            sda.Fill(dt);//bağlnatı kurulup hesaplar tablosundan hesap_noya gre veriler çekileek

            int bakiye = Convert.ToInt32(dt.Rows[0][3]);//bakiye adlı değişkene dt.Rows[0][3] sütunu ekelencek

            if (bakiye > 0)
            {
                MessageBox.Show("Hesabınızda para vardır");
                return;//eğer hesapta para varsa hesabı silmeyecek ve uyarı verecek 
            }
            else if (bakiye < 0)
            {
                MessageBox.Show("Bankaya Borcunuz vardır");
                return;//eğer öüşterinin borcu varsa tekrardan işlem iptal edilip uyarı verilece
            }

            con = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
            cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = " delete from islemler where hesap_no= " + textBox1.Text;
            cmd.ExecuteNonQuery(); //eğer bakiye 0 ise bağlantı kurulup önce islmeler adlı tablodan müşterinin yapmış olduğu veriler silinecek

            cmd.CommandText = " delete from havale where gonderen_no= " + textBox1.Text+" or alan_no="+textBox1.Text;
            cmd.ExecuteNonQuery(); //eğer bakiye 0 ise bağlantı kurulup önce islmeler adlı tablodan müşterinin yapmış olduğu veriler silinecek
            cmd.CommandText = " delete from hesaplar where hesap_no= "+textBox1.Text;
            cmd.ExecuteNonQuery(); //ardından hesapalr adlı tablodan hesap silinecek                   
            con.Close();
            MessageBox.Show("Müşteri Hesabı Kapatıldı");//ve silindiğine dair uyarı verecek
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar >= 48 && (int)e.KeyChar <= 57)
            {
                e.Handled = false;//eğer rakamsa  yazdır.
            }
            else if ((int)e.KeyChar == 8)
            {
                e.Handled = false;//eğer basılan tuş backspace ise yazdır.
            }
            else
            {
                e.Handled = true;//bunların dışındaysa hiçbirisini yazdırma
            }
        }

        private void FormHesapKapat_Load(object sender, EventArgs e)
        {

        }
    }
}
