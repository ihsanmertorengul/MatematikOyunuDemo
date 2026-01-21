using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace MatematikOyunu
{
    public partial class Game3 : BaseGameForm
    {
        Label[] anaKareler = new Label[5];      // orta satır: 9 + [y] = 14
        Label[] dikeyKareler = new Label[4];    // dikey: 12 - y = [z]
        Label[] sayiKutulari = new Label[6];    // alt sayı kutuları
        Label ustKareLabel;

        // Yeni doğru cevaplar
        int dogruY = 5;  // 9 + y = 14
        int dogruZ = 7;  // 12 - y = z  => 12 - 5 = 7
        int dogruX = 4;  // x * 14 = 56

        SoundPlayer fail;
        SoundPlayer succes;

        public Game3()
        {
            this.Text = "Matematik Oyunu - Level 3";
            this.Size = new Size(600, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            fail = new SoundPlayer(Properties.Resources.fail);
            succes = new SoundPlayer(Properties.Resources.succes);

            OyunKur();
        }

        private void OyunKur()
        {
            // ================================
            // 1) ORTA SATIR: 9 + [y] = 14
            // ================================
            string[] ortaMetin = { "9", "+", "", "=", "14" };

            for (int i = 0; i < 5; i++)
            {
                bool surukle = (i == 2);   // y kare sürükle-bırak alacak
                string isim = (i == 2) ? "yKare" : "";

                anaKareler[i] = KareEkle(
                    100 + i * 80,
                    220,
                    80,
                    80,
                    ortaMetin[i],
                    surukle,
                    isim
                );
            }

            // ================================
            // 2) DİKEY KARELER: 12, -, =, [z]
            //    (Bu düzen: 12 - y = z)
            // ================================
            string[] dikeyMetinler = { "12", "-", "=", "" };
            int[] yKonum = { 60, 140, 300, 380 };

            for (int i = 0; i < 4; i++)
            {
                bool surukle = (i == 3); // z kare sürükle-bırak alacak
                string isim = (i == 3) ? "zKare" : "";

                dikeyKareler[i] = KareEkle(
                    100 + 2 * 80,
                    yKonum[i],
                    80,
                    80,
                    dikeyMetinler[i],
                    surukle,
                    isim
                );
            }

            // ================================
            // 3) 14'ün ÜSTÜNE * kare
            // ================================
            Label yildizKare = KareEkle(
                anaKareler[4].Location.X,
                anaKareler[4].Location.Y - 80,
                80,
                80,
                "*"
            );

            // ================================
            // 4) *'ın ÜSTÜNE sürükle-bırak kare (x)
            // ================================
            ustKareLabel = KareEkle(
                yildizKare.Location.X,
                yildizKare.Location.Y - 80,
                80,
                80,
                "",
                true,
                "ustKare"
            );

            // ================================
            // 5) 14'ün ALTINA = kare
            // ================================
            Label altKare1 = KareEkle(
                anaKareler[4].Location.X,
                anaKareler[4].Location.Y + anaKareler[4].Height,
                80,
                80,
                "="
            );

            // ================================
            // 6) Altına sonuç: 56 (x * 14 = 56)
            // ================================
            KareEkle(
                altKare1.Location.X,
                altKare1.Location.Y + altKare1.Height,
                80,
                80,
                "56"
            );

            // ================================
            // 7) ALT SAYI KUTULARI (güncellendi)
            // Doğrular: 5 (y), 7 (z), 4 (x)
            // ================================
            int[] sayilar = { 5, 7, 4, 2, 9, 11 };

            for (int i = 0; i < sayilar.Length; i++)
            {
                Label kutu = KareEkle(
                    40 + i * 90,
                    500,
                    80,
                    60,
                    sayilar[i].ToString()
                );

                kutu.BackColor = Color.LightBlue;
                kutu.MouseDown += LblSayi_MouseDown;

                sayiKutulari[i] = kutu;
            }
        }

        private Label KareEkle(int x, int y, int w, int h, string text = "", bool surukle = false, string name = "")
        {
            Label lbl = new Label();
            lbl.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Size = new Size(w, h);
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.BackColor = Color.LightGray;
            lbl.Location = new Point(x, y);
            lbl.Text = text;

            if (surukle)
            {
                lbl.Name = name;
                lbl.AllowDrop = true;
                lbl.DragEnter += Lbl_DragEnter;
                lbl.DragDrop += Lbl_DragDrop;
            }

            this.Controls.Add(lbl);
            return lbl;
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
            Label hedef = sender as Label;
            string suruklenen = e.Data.GetData(DataFormats.Text).ToString();

            if (hedef.Name == "yKare")
            {
                if (suruklenen == dogruY.ToString())
                {
                    hedef.Text = suruklenen;
                    hedef.BackColor = Color.LightGreen;
                }
                else
                {
                    fail.Play();
                    hedef.BackColor = Color.LightCoral;
                }
            }
            else if (hedef.Name == "zKare")
            {
                if (suruklenen == dogruZ.ToString())
                {
                    hedef.Text = suruklenen;
                    hedef.BackColor = Color.LightGreen;
                }
                else
                {
                    fail.Play();
                    hedef.BackColor = Color.LightCoral;
                }
            }
            else if (hedef.Name == "ustKare")
            {
                if (suruklenen == dogruX.ToString())
                {
                    hedef.Text = suruklenen;
                    hedef.BackColor = Color.LightGreen;
                }
                else
                {
                    fail.Play();
                    hedef.BackColor = Color.LightCoral;
                }
            }

            if (anaKareler[2].Text == dogruY.ToString() &&
                dikeyKareler[3].Text == dogruZ.ToString() &&
                ustKareLabel.Text == dogruX.ToString())
            {
                succes.Play();
                MessageBox.Show("Tebrikler! Cevaplar doğru 🎉", "Başarılı");

                Game4 yeniForm = new Game4();
                yeniForm.Show();
                this.Hide();
            }
        }
    }
}
