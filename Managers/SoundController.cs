using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace App05MonoGame.Managers
{
    public static class SoundController
    {
        public static Dictionary<string, Song> Songs =
            new Dictionary<string, Song>();
        public static Dictionary<string, SoundEffect> SoundEffects = 
            new Dictionary<string, SoundEffect>();
        
        public static void LoadContent(ContentManager content)
        {
            Songs.Add("Adventure",content.Load<Song>("Sounds/Adventures"));            

            SoundEffects.Add("Coin", content.Load<SoundEffect>("Sounds/Coins"));
            SoundEffects.Add("Flame", content.Load<SoundEffect>("Sounds/flame"));
        }

        public static SoundEffect GetSoundEffect(string effect)
        {
            return SoundEffects[effect];
        }

        public static void PlaySong(string song)
        {
            MediaPlayer.IsRepeating = true;

            MediaPlayer.Play(Songs[song]);
        }
    }
}
