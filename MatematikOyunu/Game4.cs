using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace MatematikOyunu
{
    public partial class Game4 : BaseGameForm
    {
        Label[] anaKareler = new Label[5];      // orta satır [6][+][y][=][9]
        Label[] dikeyKareler = new Label[4];    // 6, -, =, [z]
        Label[] sayiKutulari = new Label[6];    // alt sayı kutuları
        Label ustKareLabel;

        // ✅ Yeni doğru cevaplar (uçmadan)
        int dogruY = 3;   // 6 + y = 9
        int dogruZ = 3;   // a - 6 = 4
        int dogruX = 3;   // x * 9 = 27
        int dogruA = 9;  // a - 6 = 4 => a = 10

        SoundPlayer fail;
        SoundPlayer succes;

        public Game4()
        {
            this.Text = "Matematik Oyunu - Level 4";
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
            // 1) ORTA SATIR: 6 + [y] = 9
            // ================================
            string[] ortaMetin = { "6", "+", "", "=", "9" };

            for (int i = 0; i < 5; i++)
            {
                bool surukle = (i == 2);    // y kare sürükle-bırak alacak
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
            // 2) DİKEY KARELER + SOLDA a - 6 = (ek)
            // Amaç: a - 6 = 4  (z kare = 4)
            // ================================
            int baslangicX = 100 + 2 * 80;   // 260
            int yKonumBaslangic = 60;

            // aKare (boş) -> sürükle bırak
            Label aKareLabel = KareEkle(
                baslangicX - 2 * 80, // 100
                yKonumBaslangic,     // 60
                80,
                80,
                "",
                true,
                "aKare"
            );

            // "-" karesi (a ile 6 arasında)
            Label yeniKare2 = KareEkle(
                baslangicX - 80,     // 180
                yKonumBaslangic,     // 60
                80,
                80,
                "-"
            );

            // Dikey: 6, -, =, [z]
            string[] dikeyMetinler = { "6", "-", "=", "" };
            int[] yKonum = { 60, 140, 300, 380 };

            for (int i = 0; i < 4; i++)
            {
                bool surukle = (i == 3); // z kare sürükle-bırak alacak
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

            // 6'nın sağına "=" karesi (dekor)
            Label altininSagEsitKaresi = KareEkle(
                dikeyKareler[0].Location.X + dikeyKareler[0].Width,
                dikeyKareler[0].Location.Y,
                80,
                80,
                "="
            );

            // ================================
            // 3) 9'un ÜSTÜNE "*"
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
            // 5) 9'un ALTINA "="
            // ================================
            Label altKare1 = KareEkle(
                anaKareler[4].Location.X,
                anaKareler[4].Location.Y + anaKareler[4].Height,
                80,
                80,
                "="
            );

            // ================================
            // 6) Altına sonuç: 27 (x * 9 = 27)
            // ================================
            KareEkle(
                altKare1.Location.X,
                altKare1.Location.Y + altKare1.Height,
                80,
                80,
                "27"
            );

            // ================================
            // 7) ALT SAYI KUTULARI (güncellendi)
            // Doğrular: y=3, z=4, a=10, x=3 (3 zaten var)
            // ================================
            int[] sayilar = { 3, 4, 10, 2, 5, 9 };

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

            if (hedef.Name == "yKare")
                dogruMu = (suruklenen == dogruY.ToString());
            else if (hedef.Name == "zKare")
                dogruMu = (suruklenen == dogruZ.ToString());
            else if (hedef.Name == "ustKare")
                dogruMu = (suruklenen == dogruX.ToString());
            else if (hedef.Name == "aKare")
                dogruMu = (suruklenen == dogruA.ToString());

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

            if (aKare.Text == dogruA.ToString() &&
                anaKareler[2].Text == dogruY.ToString() &&
                dikeyKareler[3].Text == dogruZ.ToString() &&
                ustKareLabel.Text == dogruX.ToString())
            {
                succes.Play();
                MessageBox.Show("Tebrikler! Cevaplar doğru 🎉", "Başarılı");

                Game5 yeniForm = new Game5();
                yeniForm.Show();
                this.Hide();
            }
        }
    }
}
