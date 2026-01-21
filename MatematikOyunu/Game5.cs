using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace MatematikOyunu
{
    public partial class Game5 : BaseGameForm
    {
        Label[] anaKareler = new Label[5];
        Label[] dikeyKareler = new Label[4];
        Label[] sayiKutulari = new Label[6];
        Label ustKareLabel;

        // ✅ Yeni doğru cevaplar (uçmadan)
        int dogruY = 5;   // 4 + y = 9
        int dogruZ = 2;   // a - 7 = 3
        int dogruX = 3;   // x * 9 = 27
        int dogruA = 10;  // a - 7 = 3  => a=10
        int dogruV = 14;   // 4'ün altındaki boş kare

        Label vKareLabel;

        SoundPlayer fail;
        SoundPlayer succes;

        public Game5()
        {
            this.Text = "Matematik Oyunu - Level 5";
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
            // 1) ORTA SATIR: 4 + [y] = 9
            // ================================
            string[] ortaMetin = { "4", "+", "", "=", "9" };

            for (int i = 0; i < 5; i++)
            {
                bool surukle = (i == 2);
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
            // YENİ KARELER (4'ün Çevresi)
            // ================================
            int dortKaresininX = anaKareler[0].Location.X;
            int dortKaresininY = anaKareler[0].Location.Y;

            // 4'ün üstüne "+"
            KareEkle(
                dortKaresininX,
                dortKaresininY - 80,
                80,
                80,
                "+"
            );

            // 4'ün altına "="
            Label altKareEsit = KareEkle(
                dortKaresininX,
                dortKaresininY + 80,
                80,
                80,
                "="
            );

            // "=" altına boş kare (vKare) -> doğru: 9
            vKareLabel = KareEkle(
                altKareEsit.Location.X,
                altKareEsit.Location.Y + 80,
                80,
                80,
                "",
                true,
                "vKare"
            );

            // ================================
            // 2) DİKEY KARELER: aKare - 7 = [z]
            // ================================
            int baslangicX = 100 + 2 * 80; // 260
            int yKonumBaslangic = 60;

            Label aKareLabel = KareEkle(
                baslangicX - 2 * 80, // 100
                yKonumBaslangic,     // 60
                80,
                80,
                "",
                true,
                "aKare"
            );

            KareEkle(
                baslangicX - 80,     // 180
                yKonumBaslangic,     // 60
                80,
                80,
                "-"
            );

            // Dikey: 7, -, =, [z]
            string[] dikeyMetinler = { "7", "-", "=", "" };
            int[] yKonum = { 60, 140, 300, 380 };

            for (int i = 0; i < 4; i++)
            {
                bool surukle = (i == 3);
                string isim = (i == 3) ? "zKare" : "";

                dikeyKareler[i] = KareEkle(
                    baslangicX,
                    yKonum[i],
                    80,
                    80,
                    dikeyMetinler[i],
                    surukle,
                    isim
                );
            }

            // 7'nin sağındaki "=" karesi (dekor)
            KareEkle(
                dikeyKareler[0].Location.X + dikeyKareler[0].Width,
                dikeyKareler[0].Location.Y,
                80,
                80,
                "="
            );

            // ================================
            // 3) 9'un ÜSTÜNE "*"
            // ================================
            int dokuzKaresininX = anaKareler[4].Location.X;
            int dokuzKaresininY = anaKareler[4].Location.Y;

            Label yildizKare = KareEkle(
                dokuzKaresininX,
                dokuzKaresininY - 80,
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
            // 5) 9'un ALTINA "=" ve sonuç 27
            // ================================
            Label altKare1 = KareEkle(
                dokuzKaresininX,
                dokuzKaresininY + anaKareler[4].Height,
                80,
                80,
                "="
            );

            KareEkle(
                altKare1.Location.X,
                altKare1.Location.Y + altKare1.Height,
                80,
                80,
                "27"
            );

            // ================================
            // 6) ALT SAYI KUTULARI (güncellendi)
            // Doğrular: 5, 3, 10, 9 (+ 2, 7 dikkat dağıtıcı)
            // ================================
            int[] sayilar = { 5, 3, 10, 9, 2, 14 };

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
            bool dogruMu = false;

            switch (hedef.Name)
            {
                case "yKare":
                    dogruMu = (suruklenen == dogruY.ToString());
                    break;
                case "zKare":
                    dogruMu = (suruklenen == dogruZ.ToString());
                    break;
                case "ustKare":
                    dogruMu = (suruklenen == dogruX.ToString());
                    break;
                case "aKare":
                    dogruMu = (suruklenen == dogruA.ToString());
                    break;
                case "vKare":
                    dogruMu = (suruklenen == dogruV.ToString());
                    break;
            }

            if (dogruMu)
            {
                hedef.Text = suruklenen;
                hedef.BackColor = Color.LightGreen;
            }
            else
            {
                fail.Play();
                hedef.BackColor = Color.LightCoral;
            }

            Label aKare = (Label)this.Controls["aKare"];
            Label vKare = (Label)this.Controls["vKare"];

            if (vKare.Text == dogruV.ToString() &&
                aKare.Text == dogruA.ToString() &&
                anaKareler[2].Text == dogruY.ToString() &&
                dikeyKareler[3].Text == dogruZ.ToString() &&
                ustKareLabel.Text == dogruX.ToString())
            {
                succes.Play();
                MessageBox.Show("Tebrikler! Tüm cevaplar doğru 🎉", "Başarılı");

                Game7 yeniForm = new Game7();
                yeniForm.Show();
                this.Hide();
            }
        }
    }
}
