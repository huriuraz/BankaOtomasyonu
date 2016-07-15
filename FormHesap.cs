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
    public partial class FormHesap : Form
    {
        public FormHesap()
        {
            InitializeComponent();
        }
        SqlConnection con;
        SqlCommand cmd;

        //müşteri ekle butonu
        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var item in groupBox1.Controls)
              {
                  if (item is TextBox)
                  {
                     TextBox txt = (TextBox)item;
                     if (txt.Text == "")
                     {
                         MessageBox.Show("Lütfen Tüm Alanları Doldurun");
                         return;
                     }
                  }
              }//foreach ile tüm groupbox1 içideki tüm nesneleri dolaşır eğer textbox ise boş olup olmadığını kntrol eder
            //boş alan varsa işlemi iptal eder yoksa aşağı geçer
            
            con = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
            cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "insert into musteriler values('" + textBox2.Text.ToString() + "','" + textBox3.Text.ToString() + "','" + textBox4.Text.ToString() + "','" + textBox5.Text.ToString() + "','" + textBox6.Text.ToString() + "')";          
            cmd.ExecuteNonQuery();                    
            con.Close();
            MessageBox.Show("Müşteri Eklendi");//eğer tüm alanalr doldurulduysa bağlantı kurulup musteriler tablosuna değerler eklenir değer sırası(ad,soyad,adres,telefon,email)
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var item in groupBox1.Controls)
            {
                if (item is TextBox)
                {
                    TextBox txt = (TextBox)item;
                    if (txt.Enabled == false)
                    {
                        txt.Enabled = true;
                    }
                    else
                    {
                        txt.Enabled = false;
                    }                   
                }
            }//eğer müşterinin hesabı var kutuc uğu seçilirse müşteri ekle alanındaki textboxların yazı girme özelliği kapanacak


            if (textBox1.Enabled == false)
            {
                textBox1.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
            }//eğer müşteri no alanı aktif ise pasif yapcak pasif ise aktif yapacak
        }

        //hesap aç butonu
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox7.Text == "" | textBox8.Text == "" | textBox1.Text == "")
            {
                MessageBox.Show("Lütfen Tüm Bilgileri Giriniz");
                return;
            }//eğer hesap aç butonuna tıklandığında müşteri no bakiye ve ekbakiye girilmediyse uyarı verilecek işlem iptal edileck

            con = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
            cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            int toplam = int.Parse(textBox7.Text) + int.Parse(textBox8.Text);
            cmd.CommandText = "insert into hesaplar values("+textBox1.Text+","+textBox8.Text+","+textBox7.Text+","+toplam+")";
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Hesap Açıldı");//eğer girildiyse bağlantı kurulup hesaplar tablosuna veri eklenecek veri sırası(müşteri no,ekhesap,bakiye,kullanılabilir baiye)
        }

        //güncelle butonu
        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var item in groupBox3.Controls)
            {
                if (item is TextBox)
                {
                    TextBox txt = (TextBox)item;
                    if (txt.Enabled == false)
                    {
                        txt.Enabled = true;
                    }
                    else
                    {
                        txt.Enabled = false;
                    }
                }
            }//eğer bilgiler girilmediyse işlem ipal edilip uatı verecek

            con = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
            cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "update musteriler set ad='"+textBox12.Text.ToString()+"', soyad='"+textBox11.Text.ToString()+"', adres='"+textBox13.Text.ToString()+"', telefon='"+textBox10.Text.ToString()+"', email='"+textBox9.Text.ToString()+"' where musteri_id="+textBox14.Text;
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Müşteri Bilgileri Güncellendi");//güncelle butonuna basıldığında bağlantı kurulup musteriler tablsounda update şlem iyapacak
            //musteriler tablosundan müşteri bilgileri güncellenecek girilen müşteri_id ye göre;
        }

        //müşteri bul butonu
        private void button4_Click(object sender, EventArgs e)
        {
            if (textBox15.Text == "" | textBox16.Text == "")
            {
                MessageBox.Show("Lütfen Tüm alanları Doldurun");
                return;
            }//eğer gerekli alanlar doldurulmdıysa ilem iptl edilip uyarı verilecek.

            try
            {
                SqlConnection CN = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
                DataTable dt = new DataTable();
                CN.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select * from musteriler where ad='" + textBox16.Text.ToString() + "' and soyad='" + textBox15.Text.ToString() + "'", CN);
                sda.Fill(dt);
                MessageBox.Show(dt.Rows[0][0].ToString(), "Müşteri Bilgi Ekranı");
            }//bağlantı kurulup musteriler tablosundan giirlen ad ve soyad verisine göre sorug yollayım sonuç alacak eğer varsa messagebox'da üşgteri numarasını verecek
            catch (Exception)
            {
                MessageBox.Show("Müşteri Bulunamadı", "Müşteri Bilgi Ekranı");               
            }//eğer müşteri yoksa uyarı verecek

            
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

        private void textBox14_KeyPress(object sender, KeyPressEventArgs e)
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
    }
}
