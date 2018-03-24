using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsteroidRunner.Utilities.Sound
{
    public class SoundEffectEngine
    {
        private static SoundEffectEngine _instance;

        private SoundEffect _airThrust;
        private Queue<SoundEffectInstance> _airThrustInsances;

        private SoundEffect _rocket;
        private Queue<SoundEffectInstance> _rocketInstances;

        private SoundEffect _laserShot;

        private SoundEffectEngine(ContentManager content)
        {
            _airThrust = content.Load<SoundEffect>("Sound/AirThrust");
            _airThrustInsances = new Queue<SoundEffectInstance>();
            
            _rocket = content.Load<SoundEffect>("Sound/Rocket");
            _rocketInstances = new Queue<SoundEffectInstance>();

            _laserShot = content.Load<SoundEffect>("Sound/LaserShot");
        }

        public static SoundEffectEngine Initiate(ContentManager content)
        {
            if (_instance == null)
                _instance = new SoundEffectEngine(content);
            return _instance;
        }

        public static SoundEffectEngine GetInstance()
        {
            return _instance;
        }

        public void LoopAirThrust()
        {
            var effect = _airThrust.CreateInstance();
            effect.IsLooped = true;
            effect.Play();
            _airThrustInsances.Enqueue(effect);
        }

        public void StopAirThrust()
        {
            if (_airThrustInsances.Count > 0)
                _airThrustInsances.Dequeue().Stop();
        }

        public void LoopRocket()
        {
            var effect = _rocket.CreateInstance();
            effect.IsLooped = true;
            effect.Play();
            _rocketInstances.Enqueue(effect);
        }

        public void StopRocket()
        {
            if (_rocketInstances.Count > 0)
                _rocketInstances.Dequeue().Stop();
        }

        public void PlayLaserShot()
        {
            _laserShot.Play();
        }
    }
}
