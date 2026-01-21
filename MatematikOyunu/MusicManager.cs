using System.IO;
using WMPLib;

namespace MatematikOyunu
{
    public static class MusicManager
    {
        private static WindowsMediaPlayer bgPlayer;

        public static void Start()
        {
            if (bgPlayer != null) return;

            bgPlayer = new WindowsMediaPlayer();
            bgPlayer.URL = GetTempFile(Properties.Resources.bg);
            bgPlayer.settings.setMode("loop", true);
            bgPlayer.settings.volume = 10;
            bgPlayer.controls.play();
        }

        private static string GetTempFile(Stream stream)
        {
            string path = Path.Combine(Path.GetTempPath(), "bg.wav");

            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }

            return path;
        }
    }
}
