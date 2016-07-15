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
    public partial class FormOzet : Form
    {
        public FormOzet()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            DateTime dp1=dateTimePicker1.Value;
            DateTime dp2=dateTimePicker2.Value;
            TimeSpan ts = new TimeSpan();
            ts = dp2.Subtract(dp1);//girilen tarihler arası fark ts adlı timesppan aktarılacak
            int fark = Convert.ToInt32(ts.Days);//ardından int değere cevriliyor

            if (fark< 0)
            {
                MessageBox.Show("Lütfen Tariheri Düzgün Giriniz");
                return;
            }//eğer baş. tarihi bitiş tarihinden sonra geliyorsa uarı verecek işlem bitirilecek

            string tarih;
            tarih = dateTimePicker1.Value.Year.ToString() +
                "-0" + dateTimePicker1.Value.Month.ToString() +
                "-" + dateTimePicker1.Value.Day.ToString();

            string tarih2;
            tarih2 = dateTimePicker2.Value.Year.ToString() +
                "-0" + dateTimePicker2.Value.Month.ToString() +
                "-" + dateTimePicker2.Value.Day.ToString();

            //tarihler dopru girildiyse öncelkle tarih adlı string değere datetimepicker'dan yıl ay ve gün çeklip sorguya uygun hala getirilecek yyyy-aa-gg gibi şekile koyacak
            
            SqlConnection CN = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT count(*) FROM islemler where hesap_no=" + textBox1.Text + "and tarih between '" + tarih + "' and '" + tarih2 + "'", CN);
            sda.Fill(dt);//ardından bağlantı kurulup islemler tablosundan hesap_noya göre ve iki tarih arasında verilere göre kaç adet işlem yapılmış o sayılacak

            int dongu = Convert.ToInt32(dt.Rows[0][0]);//döngü için sayılan işlem sayısı dongu değişkenine atıldı
           
            DataTable dt1 = new DataTable();
            SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM islemler where hesap_no=" + textBox1.Text + "and tarih between '" + tarih + "' and '" + tarih2 + "'", CN);
            sda1.Fill(dt1);//ardından islemler tablosundan hesap_noya göre ve iki tarih arasında veriler çekilecek

            for (int i = 0; i < dongu; i++)
            {
                
                string tarihaktar = dt1.Rows[i][4].ToString();//tarih değişkenine islemler tablosundan tarihler alınıyor
                string[] satirVerisi = tarihaktar.Split(' ');//ardından tarih saat şekilndeki formatı tarih şekline çevrilecek
                
                if (dt1.Rows[i][2].ToString() == "1")
                {
                    richTextBox1.Text += satirVerisi[0] + " tarihinde para çekme işlemi yapıldı Miktar : " + dt1.Rows[i][3].ToString() + Environment.NewLine;
                }//eğer işlem türü para çekme ise richtextboxa çekildi şeklinde yazı yazılacak
                else if (dt1.Rows[i][2].ToString() == "2")
                {
                    richTextBox1.Text += satirVerisi[0] + " tarihinde para yatırma işlemi yapıldı. Miktar : " + dt1.Rows[i][3].ToString() + Environment.NewLine;
                }//eğer işlem türü para yatırma ise richtextboxa yatırıldı şeklinde yazı yazılacak

            }


            DataTable dt2 = new DataTable();
            SqlDataAdapter sda2 = new SqlDataAdapter("SELECT count(*) FROM havale where (gonderen_no="+textBox1.Text+"or alan_no=" + textBox1.Text + ") and tarih between '" + tarih + "' and '" + tarih2 + "'", CN);
            sda2.Fill(dt2);//ardından yukarıdaki gibi havale tablosunda iki tarih arası ve gonderen yada alana göre veriler sayıldı

            int dongu2 = Convert.ToInt32(dt2.Rows[0][0]);//döngüye aktarıldı işelm sayısı

            DataTable dt3 = new DataTable();
            SqlDataAdapter sda3 = new SqlDataAdapter("SELECT * FROM havale where (gonderen_no=" + textBox1.Text + "or alan_no=" + textBox1.Text + ") and tarih between '" + tarih + "' and '" + tarih2 + "'", CN);
            sda3.Fill(dt3);//ardından yukarıdaki gibi havale tablosunda iki tarih arası ve gonderen yada alana göre veriler çekildi

            for (int i = 0; i < dongu2; i++)
            {
                string tarihaktar = dt3.Rows[i][3].ToString();
                string[] satirVerisi = tarihaktar.Split(' ');
                //yine aynı şekilde tarih saat formatındaki yazı sadece tarihe formatına çevirildi split komutu ile
                if (dt3.Rows[i][2].ToString() == textBox1.Text.ToString())
                {                   
                    richTextBox1.Text += satirVerisi[0]+ " tarihinde hesabınıza " + dt3.Rows[i][1].ToString() + " hesap numaralı müşteri havale işlemi yapıldı Miktar : " + dt3.Rows[i][4].ToString() + Environment.NewLine;
                }//eğer havale yapılan ise ayarı yazı
                else if (dt3.Rows[i][1].ToString() == textBox1.Text.ToString())
                {
                    richTextBox1.Text += satirVerisi[0] + " tarihinde hesabınızdan " + dt3.Rows[i][2].ToString() + " hesap numaralı müşteriye havale işlemi yapıldı Miktar : " + dt3.Rows[i][4].ToString() + Environment.NewLine;
                }//eğer havale yapan ise ayrı yazı yazıldı
            }
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

        private void FormOzet_Load(object sender, EventArgs e)
        {

        }
    }
}
