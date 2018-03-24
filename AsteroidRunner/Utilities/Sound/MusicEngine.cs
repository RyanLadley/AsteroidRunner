using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidRunner.Utilities.Sound
{
    public class MusicEngine
    {
        private static MusicEngine _instance;

        private Song _meditation;

        private MusicEngine(ContentManager content)
        {

            _meditation = content.Load<Song>("Sound/Meditation");
        }

        public static MusicEngine Initiate(ContentManager content)
        {
            if (_instance == null)
                _instance = new MusicEngine(content);
            return _instance;
        }

        public static MusicEngine GetInstance()
        {
            return _instance;
        }

        public void PlayMeditation()
        {
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume =0.2f;
            MediaPlayer.Play(_meditation);
        }
    }
}
