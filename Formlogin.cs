using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace _13253065P_
{
    public partial class Formlogin : Form
    {
        public Formlogin()
        {
            InitializeComponent();
        }

        SqlConnection baglan = new SqlConnection();

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" | textBox2.Text == "")
            {
                MessageBox.Show("Eksiksiz Bilgi Giriniz");
                return;
            }//eğer gerekli alanalr doldurumdıysa uyarı verecek

            string baglanti = "Data Source=.; Initial Catalog=bankaotomasyon; Integrated Security=true;";
            SqlConnection conn = new SqlConnection(baglanti);
            if (conn.State == ConnectionState.Closed)
            conn.Open();
            SqlCommand sorgula = new SqlCommand("select * from kullanici where k_adi='" + textBox1.Text.ToString() + "' and sifre='" + textBox2.Text.ToString()+"'", conn);
            SqlDataReader dr = sorgula.ExecuteReader();//alanlar doldurulduysa bağlantı kurulup kulanici tablosudan k_adi ve şifre ye göre veri çekilecek
            if (dr.Read())
            {
                Program.girisyapilan = textBox1.Text.ToString();
                //MessageBox.Show("Giriş başarılı");
                MainForm frm = new MainForm();
                frm.Show();
                this.Hide();
            }//eğer çekilen veri varsa ise main ekraını yönlendirecek ve giriş ekranı saklanacak
            else
            {
            MessageBox.Show("Kullanıcı Adı veya Şifre yanlış…");
            }//veri yoksa hatalı giriş uyarısı verilecel
        }

    }
}
