using ISP_Project.Game_States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;

namespace ISP_Project.Managers
{
    public class AudioManager
    {
        public static List<Song> songs = new List<Song>();
        public static List<Song> songStack = new List<Song>(); // create an empty queue of songs
        private static MediaState previousMediaState;
        private static MediaState currentMediaState;
        // private static Song currentSong;
        /*private static Dictionary<Song, string> songPathDictionary;*/
        public static List<SoundEffect> soundEffects = new List<SoundEffect>();
        
        public static void LoadAudio()
        {
            songs.Add(Globals.ContentManager.Load<Song>("Songs/Sample Song 2"));
            songs.Add(Globals.ContentManager.Load<Song>("Songs/Sample Song"));

            soundEffects.Add(Globals.ContentManager.Load<SoundEffect>("Sound Effects/Envelope"));
            /*foreach (Song song in songs)
            {
                Globals.ContentManager.Load<Song>("Songs/"+songPathDictionary[song]);
            }*/
            MediaPlayer.IsRepeating = false;
        }
        public static void Update(GameTime gameTime)
        {
            previousMediaState = currentMediaState;
            currentMediaState = MediaPlayer.State;

            // play next song once the current song is done playing
            if (previousMediaState == MediaState.Playing && currentMediaState == MediaState.Stopped && songStack.Count > 0)
            {
                songStack.RemoveAt(0);
            }
            // play current song when no other song is playing
            if (MediaPlayer.State == MediaState.Stopped && songStack.Count > 0)
            {
                MediaPlayer.Play(songStack[0]);
            }

            if (InputManager.isKey(InputManager.Inputs.PAUSE, InputManager.isTriggered))
                AudioManager.ForcePlaySong(AudioManager.songs[0]);
            /*if (InputManager.isKey(InputManager.Inputs.UP, InputManager.isTriggered))
                AudioManager.PauseSong();
            if (InputManager.isKey(InputManager.Inputs.DOWN, InputManager.isTriggered))
                AudioManager.ResumeSong();*/

            // Debug.WriteLine(songStack.Count);
        }
        public static Song GetCurrentSong()
        {
            return songStack[0];
        }
        public static void PlaySong(Song song)
        {
            if (songStack.Count == 0)
                songStack.Insert(0, song);
            else if (songStack[songStack.Count - 1] != song)
                songStack.Insert(songStack.Count - 1, song); // adds song to the back of the stack
        }
        public static void ForcePlaySong(Song song)
        {
            if (songStack.Count == 0)
                songStack.Insert(0, song);
            else if (songStack[0] != song)
                songStack.Insert(0, song); // adds song to the front of the stack
            // MediaPlayer.Play(songStack[0]);
        }
        public static void PauseSong()
        {
            if (songStack.Count > 0)
                MediaPlayer.Pause();
        }
        public static void ResumeSong()
        {
            if (songStack.Count > 0)
                MediaPlayer.Resume();
        }
        public static void SkipSong()
        {
            if (songStack.Count > 0)
                songStack.RemoveAt(0);
        }
        public static void ClearSongStack()
        {
            songStack.Clear();
        }
        public static void PlaySoundEffect(SoundEffect soundEffect)
        {
            var instance = soundEffect.CreateInstance();
            instance.Play();
        }
        public static void SetSongVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        public static void SetSoundEffectVolume(float volume)
        {
            SoundEffect.MasterVolume = volume;
        }
    }
}
