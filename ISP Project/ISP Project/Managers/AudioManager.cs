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
        private static Song previousSong;
        private static Song currentSong;
        // private static Song currentSong;
        /*private static Dictionary<Song, string> songPathDictionary;*/
        public static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();
        private static float fadeTimer = 0f;
        private static float fadeDuration = 3f;
        private static bool isFadingOut = false;
        private static bool isFadingIn = true;
        private static bool transitionLock = false;
        private static float currentSongTime = 0f;
        private static float maximumVolume = 1f;
        private static float volumeLow = 0;
        private static float volumeHigh = maximumVolume;

        public static void LoadAudio()
        {
            songs.Add("Hub Theme", Globals.ContentManager.Load<Song>("Songs/Hub Theme"));
            songs.Add("Title Theme", Globals.ContentManager.Load<Song>("Songs/Title Theme"));

            soundEffects.Add("Envelope", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Envelope"));
            soundEffects.Add("Button Press", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Button Press"));

            MediaPlayer.Volume = 0f;
        }
        public static void Update(GameTime gameTime)
        {
            previousMediaState = currentMediaState;
            currentMediaState = MediaPlayer.State;

            if (songStack.Count > 0)
            {
                previousSong = currentSong;
                currentSong = songStack[0];

                currentSongTime += Globals.Time;

                // play new song
                if (currentMediaState == MediaState.Stopped || currentSong != previousSong)
                {
                    MediaPlayer.Play(currentSong);
                    currentSongTime = 0f;
                }
                // signal fade out when nearing the end of a song
                if (Math.Abs(currentSong.Duration.TotalSeconds - currentSongTime) <= fadeDuration && !isFadingOut)
                {
                    volumeHigh = MediaPlayer.Volume;
                    isFadingOut = true;
                }
                
            }
            

            if (isFadingIn)
            {
                Fade(volumeLow, maximumVolume); // fade in
            }
            if (isFadingOut)
            {
                Fade(volumeHigh, volumeLow); // fade out
            }

            Debug.WriteLine(songStack.Count);
            foreach (Song _song in songStack)
            {
                Debug.WriteLine(_song.Name);
            }
            /*Debug.WriteLine(currentSong.Duration.TotalSeconds - currentSongTime);*/
            // Debug.WriteLine(isFadingOut);
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
                /*song = songs[songName];
                if (songStack.Count == 0)
                    songStack.Insert(0, song);
                else if (songStack[songStack.Count - 1] != song)
                    songStack.Insert(songStack.Count, song); // adds song to the back of the stack*/
                song = songs[songName];
                if (songStack.Count > 0)
                {
                    if (songStack[songStack.Count - 1] != song)
                    {
                        songStack.Insert(songStack.Count, song);
                    }
                }
                else
                {
                    songStack.Insert(0, song);
                }
                
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
                if (songStack.Count > 0)
                {
                    if (currentSong != song)
                    {
                        songStack.Insert(1, song);
                        volumeHigh = MediaPlayer.Volume;
                        isFadingOut = true;
                    }
                }
                else
                {
                    songStack.Insert(0, song);
                }
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
        public static void PlayNextSong()
        {
            isFadingIn = true;
            if (songStack.Count > 1)
            {
                // MediaPlayer.Stop();
                songStack.RemoveAt(0);
                // MediaPlayer.Play(songStack[0]);
                MediaPlayer.Volume = 0f;
            }

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
        
        private static void Fade(float initialVolume, float targetVolume)
        {
            if (fadeTimer > fadeDuration)
            {
                fadeTimer = 0f;

                if (isFadingIn)
                {
                    isFadingIn = false;
                }
                if (isFadingOut)
                {
                    PlayNextSong();
                    isFadingOut = false;
                }
            }
            else
            {
                fadeTimer += Globals.Time;

                var fadeIncrement = ((targetVolume - initialVolume) / fadeDuration) * Globals.Time;
                MediaPlayer.Volume += fadeIncrement;
                // Debug.WriteLine(fadeTimer + " : " + fadeIncrement + " : " + MediaPlayer.Volume);
            }
        }
    }
}
