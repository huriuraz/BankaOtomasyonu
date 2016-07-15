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
    public partial class FormRapor : Form
    {
        public FormRapor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string tarih;
            tarih=dateTimePicker1.Value.Year.ToString()+
                "-0"+dateTimePicker1.Value.Month.ToString()+
                "-"+dateTimePicker1.Value.Day.ToString();
            //string tüürndeki tarih değişkenine datetimepickere göre yyyy-aa-gg şeklinde atandı
            int geneltoplam = 0;

            SqlConnection CN = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT count(*) FROM islemler where tarih='" + tarih+"'", CN);
            sda.Fill(dt);//bağlantı kurulup  islemler tablosundan tarihe göre veri cekip veri sayısını saydı

            int dongu = Convert.ToInt32(dt.Rows[0][0]);//işlem sayısı dongu değilkenine aktarıldı
           
            DataTable dt1 = new DataTable();
            SqlDataAdapter sda1 = new SqlDataAdapter("SELECT * FROM islemler where tarih='" + tarih + "'", CN);
            sda1.Fill(dt1);//arından islemler talosunda tarihe göre verierin hepsi çekildi
          
            for (int i = 0; i < dongu; i++)
            {
                if (comboBox1.SelectedIndex == 0)//eğer sadece para çekme bakılacaksa
                {
                    if (dt1.Rows[i][2].ToString() == "1")
                    {
                        richTextBox1.Text += dt1.Rows[i][1].ToString() + " nolu Hesap numarası para çekme işlemi yaptı. Miktar : " + dt1.Rows[i][3].ToString() + Environment.NewLine;
                        geneltoplam -= Convert.ToInt32(dt1.Rows[i][3]);
                    }//eğer işlem türü 1 ise yanı para çekme ise richtextbox a paraçekildi yazılıp geneltoplamdan çıkarıldı
                }
                else if (comboBox1.SelectedIndex == 1)//eğer sadece para yatırma bakılacaksa
                {
                    if (dt1.Rows[i][2].ToString() == "2")
                    {
                        richTextBox1.Text += dt1.Rows[i][1].ToString() + " nolu Hesap numarası para yatırma işlemi yaptı. Miktar : " + dt1.Rows[i][3].ToString() + Environment.NewLine;
                        geneltoplam += Convert.ToInt32(dt1.Rows[i][3]);
                    }//eğer işlem türü 2 ise yanı para yatırma ise richtextbox a para yatırıldı yazılıp geneltoplam toplandıı..
                }
                else if (comboBox1.SelectedIndex == 2)//eğer sadece havale bakılacaksa
                {
                    if (dt1.Rows[i][2].ToString() == "3")
                    {
                        richTextBox1.Text += dt1.Rows[i][1].ToString() + " nolu Hesap numarası havale işlemi yaptı. Miktar : " + dt1.Rows[i][3].ToString() + Environment.NewLine;
                    }//eğer işlem türü 3 ise yanı havale ise richtextbox a havale yapıldı yazıldı.
                }
                else if (comboBox1.SelectedIndex == 3)//eğer tüm işlemlere bakılacaksa
                {
                    if (dt1.Rows[i][2].ToString() == "1")
                    {
                        richTextBox1.Text += dt1.Rows[i][1].ToString() + " nolu Hesap numarası para çekme işlemi yaptı. Miktar : " + dt1.Rows[i][3].ToString() + Environment.NewLine;
                        geneltoplam -= Convert.ToInt32(dt1.Rows[i][3]);
                    }//eğer işlem türü 1 ise yanı para çekme ise richtextbox a paraçekildi yazılıp geneltoplamdan çıkarıldı
                    else if (dt1.Rows[i][2].ToString() == "2")
                    {
                        richTextBox1.Text += dt1.Rows[i][1].ToString() + " nolu Hesap numarası para yatırma işlemi yaptı. Miktar : " + dt1.Rows[i][3].ToString() + Environment.NewLine;
                        geneltoplam -= Convert.ToInt32(dt1.Rows[i][3]);
                    }//eğer işlem türü 2 ise yanı para yatırma ise richtextbox a para yatırıldı yazılıp geneltoplam toplandıı..
                    else if (dt1.Rows[i][2].ToString() == "3")
                    {
                        richTextBox1.Text += dt1.Rows[i][1].ToString() + " nolu Hesap numarası havale işlemi yaptı. Miktar : " + dt1.Rows[i][3].ToString() + Environment.NewLine;
                    }//eğer işlem türü 3 ise yanı havale ise richtextbox a havale yapıldı yazıldı.
                }          
            }
            label4.Text = geneltoplam.ToString()+" TL";// ve bankada bulanan para : 'geneltoplam'  tl şeklinde yazıldı..
        }

        private void FormRapor_Load(object sender, EventArgs e)
        {

        }
    }
}
