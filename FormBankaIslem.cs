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
    public partial class FormBankaIslem : Form
    {
        public FormBankaIslem()
        {
            InitializeComponent();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Combobox içerisinden İşlem Türü seçiliyor..
            if (comboBox1.SelectedIndex == 0)
            {
                textBox3.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                textBox3.Enabled = false;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                textBox3.Enabled = true;
            }
        }
        
        //paracek methodu ile hesapta para olup olmadığı kontrol edilecek varsa çekilip hesaptan düşecek
        void paracek()
        {
            SqlConnection con, con2;//sql bağlantısı için 
            SqlCommand cmd, cmd2;//sql komutlarının yollanacağı komut
            //SqlConnetion CN ile bankaotomasyon adlı veritabanını bağlanıldı// CN yerine başka birşey yazabilirsiniz..
            SqlConnection CN = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
            DataTable dt = new DataTable();//verilerin çekildeği tablo yani sql sorgusunun verileri geldi
            CN.Open();//bağlatı açılıyor
            //dataatapter ile komut CN adlı bağlantıya yollanıyor
            SqlDataAdapter sda = new SqlDataAdapter("select * from hesaplar where hesap_no=" + textBox1.Text, CN);
            sda.Fill(dt);//sql sorgusu datatable aktarılıyor 
            //MessageBox.Show(dt.Rows[0][0].ToString(), "Müşteri Bilgi Ekranı");

            //çekilen veride dt.Rows[0][3] ve dt.Rows[0][2] alanındaki veri sayısala çevirilip girilen miktardan küçükmü diye kontrol ediliyor
            //çünkü eğer hesapta yada ekbakiyede o kadar para varsa işlem yapılacaktır
            //Bu if'in adına kontrol ifi diyelim
            if (int.Parse(textBox2.Text) <= Convert.ToInt32(dt.Rows[0][3]) || int.Parse(textBox2.Text) <= Convert.ToInt32(dt.Rows[0][4]))
            {
                DataTable dt2 = new DataTable();//yeni bir tablo alanı oluşturuldu sql sorgularının cevabı için
                //yine aynı şekilde CN bağlantısına sql sorgusu yollandı ve dt2'ye aktarıldı
                //Bu sorgu tablodaki bugn yapılan işlemleri gösterir
                SqlDataAdapter sda2 = new SqlDataAdapter("SELECT hesap_no,DATEDIFF(day,tarih,GETDATE()),miktar FROM islemler where DATEDIFF(day,tarih,GETDATE())=0", CN);
                sda2.Fill(dt2);

                
                DataTable dt3 = new DataTable();
                SqlDataAdapter sda3 = new SqlDataAdapter("SELECT count(DATEDIFF(day,tarih,GETDATE())) FROM islemler where DATEDIFF(day,tarih,GETDATE())=0", CN);
                sda3.Fill(dt3);
                //yine aynı şekilde işlem yapıldı ama bu sefersorguda count yollandı yani kaç işlem yapılmış o öğrenildi.. 


                int dongu = Convert.ToInt32(dt3.Rows[0][0]);//kaç işlem yapıldıysa donguye aktarıldı for işleminde kaç kere yapılacağı anlaşılması için
                int toplam = 0;//toplam yapılan işlem 750 tlden büyükmü diye kontrol etmek için

                for (int i = 0; i < dongu; i++)
                {
                    if ((dt2.Rows[i][0].ToString()).Equals(textBox1.Text.ToString()))
                    {
                        toplam += Convert.ToInt32(dt2.Rows[i][2]);
                    }
                }//for ile verileri çekip yapılan işlem tutarları toplama eklendi

                if (toplam == 750)
                {
                    MessageBox.Show("günlük Para Çekme Limiti Bitmiştir");
                    return;//eğer if içerisine girerse başka işlem yapmayıp işlemi sonlandırmak için kullanıldı return komuytu
                }//eğer toplam 750 tlden büyük ise başka çekme işlemi yapmaması için

                if (int.Parse(textBox2.Text) + toplam > 750)
                {
                    MessageBox.Show("Günlük Limit Yetersiz");
                    return;
                }//yada şuanki miktar ile yapılacak cekme işlemi tutarı 750den büyükmü kontrolü

                //eğer 750'den büyük değil ise aşağıdaki işlemlere geçecektir.

                con = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
                cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "insert into islemler values ('" + textBox1.Text + "','1','" + textBox2.Text + "',getdate())";
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Para Çekildi");
                //aynı şekilde yine bağlantı kuruldu ama bu sefer con adıyla ardından cmd(SQLCOMMAND) ile sorgu yollandı
                //sorguda islemler adlı tabloya verileri ekledik (girilen hesap no, işlem türü 1 yani para çekme, çekilen miktar, çektiği tarih)

                DataTable dt4 = new DataTable();
                SqlDataAdapter sda4 = new SqlDataAdapter("SELECT * FROM hesaplar where hesap_no=" + textBox1.Text, CN);
                sda4.Fill(dt4);//girilen hesap numarasına göre hesaplar adlı tablodan tüm verileri çekildi

                con2 = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
                cmd2 = new SqlCommand();
                con2.Open();
                cmd2.Connection = con2;//güncelleme yapmak için tekrardan bağlantı kuruldu

                int fark = Convert.ToInt32(dt4.Rows[0][3]) - int.Parse(textBox2.Text);                
                //dt4.rows[][]  ile bakiye ve ekhesap bilgileri çekildi güncelleme yapmak için
                //ve şuan girilen miktardan çıkarıldı

                    cmd2.CommandText = "update hesaplar set bakiye="+fark+" where hesap_no=" + textBox1.Text;
                    cmd2.ExecuteNonQuery();
                    con2.Close();// yeni bakiye miktarı update komutu ile güncellendi
    
                con = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
                cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "update hesaplar set k_bakiye=ekhesap+bakiye where hesap_no=" + textBox1.Text;
                cmd.ExecuteNonQuery();
                con.Close();             
                return;// yeni bağlantı oluşturulup k_bakiye adlı veri bakiye+ekhesap verileri ile güncellendi
            }
            else
            {
                MessageBox.Show("Hesabınızda Para Bulunamadı");
            }// en yukarıdaki kontrol if'inin elsesi eğer bakiye veya kullanılabilir bakiye 0'dan büyük değil ise para çekme ilemi yapılmayacaktır.

        }

        void parayatir()
        {
            SqlConnection con,con2;
            SqlCommand cmd,cmd2;//aynı şekilde bağlantı ve komut yollamak için tanımlamalar yapıldı

            SqlConnection CN = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM hesaplar where hesap_no="+textBox1.Text, CN);
            sda.Fill(dt);
            //cn ile bankaotomasyon veritabanına bağlandı ve hesaplar tablosundan hesap_no ya göre verileri çekti
            
            int toplam = Convert.ToInt32(dt.Rows[0][3]) + int.Parse(textBox2.Text);
            //dt.Rows[0][3] ile bakiye verisi çekildi ve girilen miktar ile toplandı
            
            con = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
            cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "update hesaplar set bakiye=" + toplam + ",k_bakiye=ekhesap+"+toplam+" where hesap_no=" + textBox1.Text;
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Para Yatırıldı");
            //tekrardan bağlantı kuruldu ve cmd.commandtext ile update komutu çalıştırıldı 
            //bakiye toplam değişkenine atandı k_bakiye verisi ise ekhesap+bakiye değerini aldı
            
            con2 = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
            cmd2 = new SqlCommand();
            con2.Open();
            cmd2.Connection = con2;
            cmd2.CommandText = "insert into islemler values(" + textBox1.Text + ",2,+" + textBox2.Text + ",getdate())";
            cmd2.ExecuteNonQuery();
            con2.Close();
            return;//ardından islemler adlı tabloya değerler eklendi
        }

        void havaleyap()
        {
            SqlConnection con, con2,con3;
            SqlCommand cmd, cmd2,cmd3;
            SqlConnection CN = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
            DataTable dt = new DataTable();
            CN.Open();
            SqlDataAdapter sda = new SqlDataAdapter("select * from hesaplar where hesap_no=" + textBox1.Text, CN);
            sda.Fill(dt);
            //yukarıdaki gibi tekrar bağlantı kurulup hesaplar tablosundan veriler çekildi.

            //paracek methodundaki kontrol if işleminin aynısı eğer bakiye veya ekhesapta para yoksa işlemi iptal etmek için
            if (int.Parse(textBox2.Text) <= Convert.ToInt32(dt.Rows[0][3]) || int.Parse(textBox2.Text) <= Convert.ToInt32(dt.Rows[0][2]))
            {
                DataTable dt2 = new DataTable();
                SqlDataAdapter sda2 = new SqlDataAdapter("SELECT hesap_no,DATEDIFF(day,tarih,GETDATE()),miktar FROM islemler where DATEDIFF(day,tarih,GETDATE())=0", CN);
                sda2.Fill(dt2);//paracek methodundaki gibi bugün yapılan ilemleri listelemek için kullanılıyor

                con = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
                cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "insert into islemler values ('" + textBox1.Text + "','3','" + textBox2.Text + "',getdate())";
                cmd.ExecuteNonQuery();
                con.Close();//ardından islemler tablosuna değer ekleniyor değerler sırası(gönderen hesap_no,işlem türü havale olduğu için 3, miktar, işlem yapılan tarih)
                

                DataTable dt3 = new DataTable();
                SqlDataAdapter sda3 = new SqlDataAdapter("SELECT * FROM hesaplar where hesap_no=" + textBox3.Text, CN);
                sda3.Fill(dt3);//hesap_noya göre hesaplar adlı tablodan veriler çekildi

                int toplam = Convert.ToInt32(dt3.Rows[0][3]) + int.Parse(textBox2.Text);//gönderilen bakiye ile hesaptaki bakiye toplandı
                int toplambakiyehavale = toplam + Convert.ToInt32(dt3.Rows[0][2]);//toplam değişkenine ekhesapta eklendi k_bakiy değeri güncellemesi için

                DataTable dt4 = new DataTable();
                SqlDataAdapter sda4 = new SqlDataAdapter("SELECT * FROM hesaplar where hesap_no=" + textBox1.Text, CN);
                sda4.Fill(dt4);//hesap_noya göre tekrardan veriler çekildi

                int fark = Convert.ToInt32(dt4.Rows[0][3]) - int.Parse(textBox2.Text);
                int ekfark = Convert.ToInt32(dt4.Rows[0][2]) - int.Parse(textBox2.Text) + Convert.ToInt32(dt4.Rows[0][3]);
                int toplambakiye = fark + Convert.ToInt32(dt4.Rows[0][2]) ;//gönderen hesap numarasına göre bakiyeden gönderilecek miktar azaltıldı

                con2 = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
                cmd2 = new SqlCommand();
                con2.Open();
                cmd2.Connection = con2;

                con3 = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
                cmd3 = new SqlCommand();
                con3.Open();
                cmd3.Connection = con3;
                //bakiye ve k_bakiye adlı verileri güncellemek için 2 ayrı bağlantı oluşturuldu

              
                if (fark < 0)
                {
                    cmd2.CommandText = "update hesaplar set bakiye="+fark+",k_bakiye="+toplambakiye+" where hesap_no=" + textBox1.Text;
                    cmd2.ExecuteNonQuery();
                    con2.Close();
                    cmd3.CommandText = "update hesaplar set bakiye="+toplam+",k_bakiye=" + toplambakiyehavale +" where hesap_no=" + textBox3.Text;
                    cmd3.ExecuteNonQuery();
                    con3.Close();
                }//eğer fark eksiye düştüyse ekhesaptan para çekildi..
                else
                {
                    cmd2.CommandText = "update hesaplar set k_bakiye="+toplambakiye+",bakiye=" + fark + " where hesap_no=" + textBox1.Text;
                    cmd2.ExecuteNonQuery();
                    con2.Close();
                    cmd3.CommandText = "update hesaplar set bakiye=" + toplam + ",k_bakiye=" + toplambakiyehavale + " where hesap_no=" + textBox3.Text;
                    cmd3.ExecuteNonQuery();
                    con3.Close();
                }//değilse normal bakiyeden para çekildi.

                con = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
                cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "insert into havale values(" + textBox1.Text + "," + textBox3.Text + ",getdate(),"+textBox2.Text+")";
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Para Çekildi");
                return;//yani bağlantı oluştuurlup havale tablosuna veriler eklendi veriler sırası(gönderen,alan,şlem tarihi,miktar)
            }
            else
            {
                MessageBox.Show("Hesabınızda Para Bulunamadı");
            }//kontrol ifi gibi olan işlem hesapta para bulunmazsa işlemi iptal eder.
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                paracek();//eğer combobox'tan para cek seçildiysi paracek methodu çağrıldı
                return;
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                parayatir();//eğer combobox'tan para yatır seçildiysi parayatır methodu çağrıldı
                return;
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                havaleyap();//eğer combobox'tan havale seçildiysi havale methodu çağrıldı
                return;
            }
            MessageBox.Show("Lütfen Bir işlem Seçin");// eğer hiçbiri seçilmediyse uyarı verir.
        }

        //hesap numarası öğrenmek için kullanılır
        private void button4_Click(object sender, EventArgs e)
        {
            int dongu;
            SqlConnection CN;
            DataTable dt;
            SqlDataAdapter sda;
            if (textBox15.Text == "" | textBox16.Text == "")
            {
                MessageBox.Show("Lütfen Tüm alanları Doldurun");
                return;
            }// eğer alanlar boş bırakıldıysa uyarı verir

            try
            {
                CN = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
                dt = new DataTable();
                CN.Open();
                sda = new SqlDataAdapter("select * from musteriler where ad='" + textBox16.Text.ToString() + "' and soyad='" + textBox15.Text.ToString() + "'", CN);
                sda.Fill(dt);
                MessageBox.Show(" Müşteri Numarası : "+dt.Rows[0][0].ToString(), "Müşteri Bilgi Ekranı");
                //bağlantı kurulup musteriler tablosunda ad ve soyad alanına göre veri çeker vebunu datatableya atar
                //eğer veri varsa [0][0] sütununda müşteri numarası bulunur onu messagebox'd gösterir
                sda = new SqlDataAdapter("select count(*) from hesaplar where musteri_no='" + dt.Rows[0][0].ToString() + "'", CN);
                DataTable dt2 = new DataTable();
                sda.Fill(dt2);

                dongu=Convert.ToInt32(dt2.Rows[0][0]);
                
                
            }
            catch (Exception)
            {
                MessageBox.Show("Müşteri Bulunamadı", "Müşteri Bilgi Ekranı");//eğer müşteri yoksa..
                return;
            }
            //try-catch yapma sebebi eğer müşteri yoksa proğramın çökmemesi için
            sda = new SqlDataAdapter("select * from hesaplar where musteri_no='" + dt.Rows[0][0].ToString() + "'", CN);
            DataTable dt3 = new DataTable();
            sda.Fill(dt3);
            for (int i = 0; i < dongu; i++)
            {
                MessageBox.Show("Hesap Numarası : " + dt3.Rows[i][0].ToString() + Environment.NewLine + "Bakiyesi : " + dt3.Rows[i][3].ToString() + Environment.NewLine + "Kullanılabilir Bakiye :" + dt3.Rows[i][4].ToString());
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection CN = new SqlConnection("Data Source=.; Initial Catalog=bankaotomasyon;Integrated Security=SSPI");
                DataTable dt = new DataTable();
                CN.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select * from hesaplar where hesap_no='" + textBox5.Text.ToString() + "'", CN);
                sda.Fill(dt);
                MessageBox.Show("Müşterinin Bakiyesi :" + dt.Rows[0][3].ToString() + " TL" + Environment.NewLine + " Kullanılabilir Bakiye :" + dt.Rows[0][4].ToString() + " TL", "Müşteri Bilgi Ekranı");
       
            }
            catch (Exception)
            {
                MessageBox.Show("Hesap Bulunamadı");
                return;
                throw;
            }
             }

      
    }
}
