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
        public static Dictionary<string, Song> songs = new Dictionary<string, Song>();
        public static List<Song> songStack = new List<Song>(); // create an empty queue of songs
        private static MediaState previousMediaState;
        private static MediaState currentMediaState;
        // private static Song currentSong;
        /*private static Dictionary<Song, string> songPathDictionary;*/
        public static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();
        private static float fadeTimer = 0f;
        private static float fadeInDuration = 4f;
        private static float fadeOutDuration = 4f;
        private static bool isFadingOut = false;
        private static bool isFadingIn = true;
        private static float currentSongTime = 0f;
        private static float initialFadeInVolume;
        private static float initialFadeOutVolume;

        public static void LoadAudio()
        {
            songs.Add("Hub Theme", Globals.ContentManager.Load<Song>("Songs/Hub Theme"));
            songs.Add("Title Theme", Globals.ContentManager.Load<Song>("Songs/Title Theme"));

            soundEffects.Add("Envelope", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Envelope"));
            soundEffects.Add("Button Press", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Button Press"));

            songStack.Insert(0, songs["Title Theme"]);
            songStack.Insert(0, songs["Title Theme"]);
            /*var titleTheme = Globals.ContentManager.Load<SoundEffect>("Songs/Title Theme");
            var instance = titleTheme.CreateInstance();
            instance.IsLooped = true;
            instance.Play();*/
            // MediaPlayer.Volume = 0f;
        }
        public static void Update(GameTime gameTime)
        {
            previousMediaState = currentMediaState;
            currentMediaState = MediaPlayer.State;

            // play next song once the current song is done playing
            if (previousMediaState == MediaState.Playing && currentMediaState == MediaState.Stopped && songStack.Count > 0)
            {
                songStack.RemoveAt(0);
                currentSongTime = 0f;
                MediaPlayer.Volume = 0f;
                initialFadeInVolume = MediaPlayer.Volume;
            }
            // play current song when no other song is playing
            if (MediaPlayer.State == MediaState.Stopped && songStack.Count > 0)
            {
                isFadingIn = true;
                MediaPlayer.Play(songStack[0]);
                initialFadeOutVolume = MediaPlayer.Volume;
            }
            // track how much of the song has elapsed
            if (MediaPlayer.State == MediaState.Playing)
            {
                currentSongTime += Globals.Time;
            }

            // fade in
            if (isFadingIn)
            {
                Fade(initialFadeInVolume, 1f);
            }
            // fade out when nearing the end of a song
            if (songStack.Count > 0)
            {
                if (songStack[0].Duration.Seconds - currentSongTime <= fadeOutDuration)
                {
                    isFadingOut = true;
                    Fade(initialFadeOutVolume, 0f);
                }
            }
            
            // Debug.WriteLine(currentSongTime);

            // Debug.WriteLine(songStack.Count);
            // Debug.WriteLine(songStack[0].Duration);
        }
        public static Song GetCurrentSong()
        {
            return songStack[0];
        }
        public static void PlaySong(string songName)
        {
            Song song;
            try
            {
                song = songs[songName];
                if (songStack.Count == 0)
                    songStack.Insert(0, song);
                else if (songStack[songStack.Count - 1] != song && songStack[0] != song)
                    songStack.Insert(songStack.Count - 1, song); // adds song to the back of the stack
            }
            catch
            {
                Debug.WriteLine("Song does not exist.");
            }
            
        }
        public static void ForcePlaySong(string songName)
        {
            Song song;
            try
            {
                song = songs[songName];
                MediaPlayer.Stop();
                if (songStack.Count == 0)
                    songStack.Insert(0, song);
                else if (songStack[0] != song)
                {
                    songStack.Insert(0, song); // adds song to the front of the stack
                }
                MediaPlayer.Play(songStack[0]);
            }
            catch
            {
                Debug.WriteLine("Song does not exist.");
            }
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
        public static void PlaySoundEffect(string soundEffectKey)
        {
            try
            {
                var instance = soundEffects[soundEffectKey].CreateInstance();
                instance.Play();
            }
            catch
            {
                Debug.WriteLine("Sound Effect does not exist.");
            }
        }
        public static void SetSongVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        public static void SetSoundEffectVolume(float volume)
        {
            SoundEffect.MasterVolume = volume;
        }

        /*private static void FadeOut(float fadeDuration, float targetVolume)
        {
            var fadeTimer = 0f;
            var fadeSlope = (targetVolume - MediaPlayer.Volume) / fadeDuration;

            while (fadeTimer <= fadeDuration)
            {
                var newVolume = MediaPlayer.Volume + fadeSlope;
                MediaPlayer.Volume = newVolume;
            }
        }*/
        
        private static void Fade(float initialVolume, float targetVolume)
        {
            if (fadeTimer > fadeInDuration)
            {
                fadeTimer = 0f;
                isFadingIn = false;
            }
            else
            {
                fadeTimer += Globals.Time;
            }

            var fadeIncrement = ((targetVolume - initialVolume) / fadeInDuration) * Globals.Time;
            MediaPlayer.Volume += fadeIncrement;
            // Debug.WriteLine(fadeTimer + " : " + fadeIncrement + " : " + MediaPlayer.Volume);
        }
    }
}
