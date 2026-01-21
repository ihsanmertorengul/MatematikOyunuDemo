using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace MatematikOyunu
{
    public partial class Game6 : BaseGameForm
    {
        // Orta satır: 7 + y = 11
        Label[] anaKareler = new Label[5];

        // Dikey sütun: 6 - y = z  (y orta satırdaki boş kare)
        Label zKareLabel;

        // Sağ dikey: x * 11 = 22
        Label ustKareLabel;

        // Alt satır: v + z = 22
        Label vKareLabel;

        // ✅ Doğru cevaplar (tam tutarlı)
        int dogruY = 4;   // 7 + 4 = 11
        int dogruZ = 2;   // 6 - 4 = 2
        int dogruX = 2;   // 2 * 11 = 22
        int dogruV = 20;  // 20 + 2 = 22

        SoundPlayer fail;
        SoundPlayer succes;

        public Game6()
        {
            this.Text = "Matematik Oyunu - Level 6";
            this.Size = new Size(600, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            fail = new SoundPlayer(Properties.Resources.fail);
            succes = new SoundPlayer(Properties.Resources.succes);

            OyunKur();
        }

        private void OyunKur()
        {
            // Grid ayarları (ekrandaki görsele uysun)
            int cell = 80;
            int baseX = 140;
            int mainY = 240;            // orta satır y
            int topY = mainY - 160;     // en üst satır y (x ve 6)
            int midTopY = mainY - 80;   // 6'nın altındaki "-" ve "*" satırı
            int eqY = mainY + 80;       // "=" satırı
            int bottomY = mainY + 160;  // z, 22, v satırı

            // =========================
            // 1) ORTA SATIR: 7 + y = 11
            // =========================
            string[] orta = { "7", "+", "", "=", "11" };
            for (int i = 0; i < 5; i++)
            {
                bool drop = (i == 2);
                string name = drop ? "yKare" : "";

                anaKareler[i] = KareEkle(
                    baseX + i * cell,
                    mainY,
                    cell, cell,
                    orta[i],
                    drop,
                    name
                );
            }

            // Orta sütun x koordinatı (y'nin sütunu)
            int midX = anaKareler[2].Location.X;

            // =========================
            // 2) DİKEY: 6 - y = z   (6'nın altındaki '-' EKSİKSİZ!)
            // =========================
            KareEkle(midX, topY, cell, cell, "6");             // 6
            KareEkle(midX, midTopY, cell, cell, "-");          // -  (6'nın altındaki)
            // y zaten anaKareler[2] (boş drop)
            KareEkle(midX, eqY, cell, cell, "=");              // =
            zKareLabel = KareEkle(midX, bottomY, cell, cell, "", true, "zKare"); // z

            // =========================
            // 3) SAĞ DİKEY: x * 11 = 22
            // =========================
            int rightX = anaKareler[4].Location.X;  // 11'in sütunu

            ustKareLabel = KareEkle(rightX, topY, cell, cell, "", true, "ustKare"); // x
            KareEkle(rightX, midTopY, cell, cell, "*");                             // *
            // 11 zaten anaKareler[4]
            KareEkle(rightX, eqY, cell, cell, "=");                                 // =
            KareEkle(rightX, bottomY, cell, cell, "22");                            // 22

            // =========================
            // 4) ALT SATIR: v + z = 22   (zKare burada denklem parçası!)
            // =========================
            vKareLabel = KareEkle(baseX, bottomY, cell, cell, "", true, "vKare");   // v (7'nin altında)
            KareEkle(baseX + cell, bottomY, cell, cell, "+");                       // +
            // z zaten midX,bottomY konumunda (zKare)
            KareEkle(baseX + 3 * cell, bottomY, cell, cell, "=");                   // =
            KareEkle(baseX + 4 * cell, bottomY, cell, cell, "22");                  // 22 (aynı sonuç, sorun değil)

            // =========================
            // 5) ALT SAYILAR (ŞIKLAR)
            // Doğrular: 4, 2, 2, 20 (2 tek kutu yeter, kopyalanabiliyor)
            // =========================
            int[] sayilar = { 4, 2, 20, 5, 6, 3 };

            for (int i = 0; i < sayilar.Length; i++)
            {
                Label kutu = KareEkle(60 + i * 90, 500, 80, 60, sayilar[i].ToString());
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

            if (hedef.Name == "yKare") dogruMu = (suruklenen == dogruY.ToString());
            else if (hedef.Name == "zKare") dogruMu = (suruklenen == dogruZ.ToString());
            else if (hedef.Name == "ustKare") dogruMu = (suruklenen == dogruX.ToString());
            else if (hedef.Name == "vKare") dogruMu = (suruklenen == dogruV.ToString());

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

            // Kazanma kontrolü
            if (anaKareler[2].Text == dogruY.ToString() &&
                zKareLabel.Text == dogruZ.ToString() &&
                ustKareLabel.Text == dogruX.ToString() &&
                vKareLabel.Text == dogruV.ToString())
            {
                succes.Play();
                MessageBox.Show("Tebrikler! Seviye tamamlandı 🎉", "Başarılı");
                new Game7().Show();
                this.Hide();
            }
        }
    }
}
