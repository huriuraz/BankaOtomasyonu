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
    public partial class FormUser : Form
    {
        public FormUser()
        {
            InitializeComponent();
        }
        SqlConnection con;    
        SqlCommand cmd;
        SqlConnection baglan = new SqlConnection();

        //kullanıcı ekle butonu
        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Lütfen Tüm alanalrı doldurun..");
                return;
            }
            string baglanti = "Data Source=.; Initial Catalog=bankaotomasyon; Integrated Security=true;";
            SqlConnection conn = new SqlConnection(baglanti);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlCommand sorgula = new SqlCommand("select * from kullanici where k_adi='" + textBox1.Text.ToString() + "'", conn);
            SqlDataReader dr = sorgula.ExecuteReader();
            //bağlantı kurulup kullanıcı tablosundan k_adi na göre arama yapıldı.
            if (dr.Read())
            {
                MessageBox.Show("Bu kullanci adi var");
            }//eğer o kullanıcı adı var ise işlem iptal edildi.
            else
            {
                con = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
                cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "insert into kullanici values ('" + textBox1.Text + "','" + textBox2.Text + "')";
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Kullanıcı Eklendi");
            }//eğer yoksa tekrandan bağlantı kurulup kuıllanici tablosuna kullanıcı eklen veri sırası(k_adi,şifre)
            textBox1.Text = "";
            textBox2.Text = "";//textboxtlar temizlendi.   
        }

        //sil butonu
        private void button2_Click(object sender, EventArgs e)
        {
            con = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
            cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "delete from kullanici where k_adi='"+textBox3.Text.ToString()+"'";
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Kullanıcı Silindi");//bağlantı kurulup kullanıcı taablosundan k_adina göre arama yapıldı eğer varise kullanıcı silindi ve uyarı verildş
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox4.Text!=textBox5.Text)
            {
                MessageBox.Show("Lütfen Şifreleri Aynı Giriniz");
                return;
            }//eğer alanlar birbiriyle eşit değilse işlem yapılmayıp uyarı verecek

            con = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
            cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "update kullanici set sifre='"+textBox4.Text.ToString()+"' where k_adi='" + Program.girisyapilan + "'";
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Şifreniz güncellendi");//eğer şifreler eşitse bağlantı kurulup kullanici tablosundan update ile güncelleme yapıldı
            //şifre alanı k_adina göre değiştiriliyor kullanıcı adı ise giriş yapılan kullanıcı adı program.cs dosyasında tutulyor
        }

    }
}
