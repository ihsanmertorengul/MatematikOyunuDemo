using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace MatematikOyunu
{
    public partial class Game11 : BaseGameForm
    {
        Label[] anaKareler = new Label[5]; // [8][+][y][=][12]
        Label yKareLabel;
        Label zKareLabel;
        Label xKareLabel;
        Label vKareLabel;

        // ✅ Doğru cevaplar (tam sayı, tutarlı)
        int dogruY = 4;   // 8 + 4 = 12
        int dogruZ = 5;   // 9 - 4 = 5
        int dogruX = 2;   // 2 * 12 = 24
        int dogruV = 12;  // 12 / 2 = 6

        SoundPlayer fail;
        SoundPlayer succes;

        public Game11()
        {
            this.Text = "Matematik Oyunu - Level 11";
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            fail = new SoundPlayer(Properties.Resources.fail);
            succes = new SoundPlayer(Properties.Resources.succes);

            OyunKur();
        }

        private void OyunKur()
        {
            int cell = 80;

            // Daha kompakt yerleşim
            int baseX = 260;
            int baseY = 220;

            // =========================
            // 1) ORTA: 8 + y = 12
            // =========================
            string[] orta = { "8", "+", "", "=", "12" };

            for (int i = 0; i < 5; i++)
            {
                bool drop = (i == 2);
                string name = drop ? "yKare" : "";
                anaKareler[i] = KareEkle(baseX + i * cell, baseY, cell, cell, orta[i], drop, name);
            }

            yKareLabel = anaKareler[2];
            int yX = yKareLabel.Location.X;
            int yY = yKareLabel.Location.Y;

            // =========================
            // 2) DİKEY: 9 - y = z
            // (9 üstte, '-' arada, y ortada, '=' altta, z en altta)
            // =========================
            KareEkle(yX, yY - 2 * cell, cell, cell, "9");
            KareEkle(yX, yY - 1 * cell, cell, cell, "-");
            KareEkle(yX, yY + 1 * cell, cell, cell, "=");
            zKareLabel = KareEkle(yX, yY + 2 * cell, cell, cell, "", true, "zKare");

            // =========================
            // 3) SAĞ DİKEY: x * 12 = 24
            // =========================
            int onIkiX = anaKareler[4].Location.X;
            int onIkiY = anaKareler[4].Location.Y;

            xKareLabel = KareEkle(onIkiX, onIkiY - 2 * cell, cell, cell, "", true, "xKare");
            KareEkle(onIkiX, onIkiY - 1 * cell, cell, cell, "*");
            KareEkle(onIkiX, onIkiY + 1 * cell, cell, cell, "=");
            KareEkle(onIkiX, onIkiY + 2 * cell, cell, cell, "24");

            // =========================
            // 4) SOL BLOK: v / 2 = 6
            // =========================
            int solX = 80;
            int solY = 180;

            vKareLabel = KareEkle(solX, solY, cell, cell, "", true, "vKare");
            KareEkle(solX, solY + cell, cell, cell, "/");
            KareEkle(solX, solY + 2 * cell, cell, cell, "2");
            KareEkle(solX, solY + 3 * cell, cell, cell, "=");
            KareEkle(solX, solY + 4 * cell, cell, cell, "6");

            // =========================
            // 5) ŞIKLAR (az)
            // doğrular: 4,5,2,12 + 2 dikkat dağıtıcı
            // =========================
            int[] sayilar = { 4, 5, 2, 12, 6, 3 };

            for (int i = 0; i < sayilar.Length; i++)
            {
                Label kutu = KareEkle(120 + i * 110, 580, 95, 60, sayilar[i].ToString());
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
            if (lbl != null) lbl.DoDragDrop(lbl.Text, DragDropEffects.Move);
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
                case "yKare": dogruMu = (suruklenen == dogruY.ToString()); break;
                case "zKare": dogruMu = (suruklenen == dogruZ.ToString()); break;
                case "xKare": dogruMu = (suruklenen == dogruX.ToString()); break;
                case "vKare": dogruMu = (suruklenen == dogruV.ToString()); break;
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
                return;
            }

            // ✅ Kazanma
            if (yKareLabel.Text == dogruY.ToString() &&
                zKareLabel.Text == dogruZ.ToString() &&
                xKareLabel.Text == dogruX.ToString() &&
                vKareLabel.Text == dogruV.ToString())
            {
                succes.Play();
                MessageBox.Show("Tebrikler! Seviye 11 Tamamlandı 🎉", "Harika!");
                new Game12().Show();
                this.Hide();
            }
        }
    }
}
