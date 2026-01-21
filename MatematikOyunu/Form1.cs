using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;

namespace MatematikOyunu
{
    public partial class Form1 : BaseGameForm
    {
        Label[] kareler = new Label[5];
        Label[] sayiKutulari = new Label[4];
        int dogruCevap;
        Random rnd = new Random();
        SoundPlayer fail;
        SoundPlayer succes;

        public Form1()
        {
            this.Text = "Matematik Oyunu - Level 1";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            fail = new SoundPlayer(Properties.Resources.fail);
            succes = new SoundPlayer(Properties.Resources.succes);

            MusicManager.Start();

            OyunKur();
        }

        private void OyunKur()
        {
            // Basit toplama işlemi oluştur
            int a = rnd.Next(1, 20);
            int b = rnd.Next(1, 20);
            dogruCevap = b;
            int sonuc = a + b;

            // Karelerin metinleri
            string[] metinler = { a.ToString(), "+", "[ ? ]", "=", sonuc.ToString() };

            // 5 kare oluştur
            for (int i = 0; i < 5; i++)
            {
                Label lbl = new Label();
                lbl.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.Size = new Size(80, 80);
                lbl.Location = new Point(50 + i * 100, 80);
                lbl.BorderStyle = BorderStyle.FixedSingle;
                lbl.BackColor = Color.LightGray;
                lbl.Text = metinler[i];

                // Ortadaki boş kareye sürükleme izni
                if (i == 2)
                {
                    lbl.AllowDrop = true;
                    lbl.Text = "";
                    lbl.DragEnter += Lbl_DragEnter;
                    lbl.DragDrop += Lbl_DragDrop;
                }

                kareler[i] = lbl;
                this.Controls.Add(lbl);
            }

            // Alttaki sayı kutuları
            int[] sayilar = new int[4];
            sayilar[0] = dogruCevap;
            for (int i = 1; i < 4; i++)
                sayilar[i] = rnd.Next(1, 20);

            sayilar = sayilar.OrderBy(x => rnd.Next()).ToArray();

            for (int i = 0; i < 4; i++)
            {
                Label lblSayi = new Label();
                lblSayi.Font = new Font("Segoe UI", 16, FontStyle.Bold);
                lblSayi.TextAlign = ContentAlignment.MiddleCenter;
                lblSayi.Size = new Size(80, 60);
                lblSayi.Location = new Point(80 + i * 120, 250);
                lblSayi.BorderStyle = BorderStyle.FixedSingle;
                lblSayi.BackColor = Color.LightBlue;
                lblSayi.Text = sayilar[i].ToString();

                lblSayi.MouseDown += LblSayi_MouseDown;

                sayiKutulari[i] = lblSayi;
                this.Controls.Add(lblSayi);
            }
        }

        private void LblSayi_MouseDown(object sender, MouseEventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null)
                lbl.DoDragDrop(lbl.Text, DragDropEffects.Move);
        }

        private void Lbl_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effect = DragDropEffects.Move;
        }

        private void Lbl_DragDrop(object sender, DragEventArgs e)
        {
            Label lbl = sender as Label;
            string suruklenen = e.Data.GetData(DataFormats.Text).ToString();

            if (suruklenen == dogruCevap.ToString())
            {
                succes.Play();

                lbl.Text = suruklenen;
                lbl.BackColor = Color.LightGreen;
                MessageBox.Show("Tebrikler! Doğru cevap 🎉", "Başarılı");

                // ✅ Yeni formu aç
                Game2 yeniForm = new Game2();
                yeniForm.Show();

                // ❌ Bu formu kapat
                this.Hide();
            }
            else
            {
                fail.Play();
                lbl.BackColor = Color.LightCoral;
                MessageBox.Show("Yanlış cevap 😢", "Hata");
            }
        }


    }
}
