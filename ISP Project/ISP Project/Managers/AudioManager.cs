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
        // create dictionaries to store audio
        public static Dictionary<string, Song> songs = new Dictionary<string, Song>();
        public static Dictionary<string, SoundEffect> soundEffects = new Dictionary<string, SoundEffect>();

        // create an empty song stack
        public static List<Song> songStack = new List<Song>(); 

        // create variables to track song states
        private static MediaState previousMediaState;
        private static MediaState currentMediaState;
        private static Song previousSong;
        private static Song currentSong;
        private static float currentSongTime = 0f;

        // create song fade variables
        private static float fadeTimer = 0f;
        private static float fadeDuration = 3f;
        private static bool isFadingOut = false;
        private static bool isFadingIn = true;
        
        // create volume control variables
        private static float maximumVolume = 0.5f;
        private static float volumeLow = 0;
        private static float volumeHigh = maximumVolume;

        /// <summary>
        /// Loads all of the Audio.
        /// </summary>
        public static void LoadAudio()
        {
            // add every song
            songs.Add("Hub Theme", Globals.ContentManager.Load<Song>("Songs/Hub Theme"));
            songs.Add("Title Theme", Globals.ContentManager.Load<Song>("Songs/Title Theme"));

            // add every sound effect
            soundEffects.Add("Envelope", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Envelope"));
            soundEffects.Add("Button Press", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Button Press"));
            soundEffects.Add("Player Movement", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Player Movement"));
            soundEffects.Add("Box Splash", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Box Splash"));
            soundEffects.Add("Victory Jingle", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Victory Jingle"));
            soundEffects.Add("Door Opening", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Door Opening"));

            // start from the lowest volume
            MediaPlayer.Volume = volumeLow;
        }

        /// <summary>
        /// Updates the audio.
        /// </summary>
        public static void Update()
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

            /*Debug.WriteLine(songStack.Count);
            foreach (Song _song in songStack)
            {
                Debug.WriteLine(_song.Name);
            }*/
            /*Debug.WriteLine(currentSong.Duration.TotalSeconds - currentSongTime);*/
            // Debug.WriteLine(isFadingOut);
        }

        /// <summary>
        /// Gets the current song that is playing.
        /// </summary>
        /// <returns></returns>
        public static Song GetCurrentSong()
        {
            return songStack[0];
        }

        /// <summary>
        /// Plays a song after the current song stops.
        /// </summary>
        /// <param name="songName"></param>
        public static void PlaySong(string songName)
        {
            Song song;
            try
            {
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

        /// <summary>
        /// Fades out the song that is playing and plays another song.
        /// </summary>
        /// <param name="songName"></param>
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
                        // force a fade out
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

        /// <summary>
        /// Pauses the current song.
        /// </summary>
        public static void PauseSong()
        {
            if (songStack.Count > 0)
                MediaPlayer.Pause();
        }

        /// <summary>
        /// Resumes the current song.
        /// </summary>
        public static void ResumeSong()
        {
            if (songStack.Count > 0)
                MediaPlayer.Resume();
        }

        /// <summary>
        /// Skips the song abruptly.
        /// </summary>
        public static void SkipSong()
        {
            if (songStack.Count > 0)
                songStack.RemoveAt(0);
        }

        /// <summary>
        /// Clears the song stack.
        /// </summary>
        public static void ClearSongStack()
        {
            songStack.Clear();
        }

        /// <summary>
        /// Plays the next song by updating the song stack.
        /// </summary>
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

        /// <summary>
        /// Plays the given sound effect.
        /// </summary>
        /// <param name="soundEffectKey"></param>
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

        /// <summary>
        /// Sets the master song volume.
        /// </summary>
        /// <param name="volume"></param>
        public static void SetSongVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        /// <summary>
        /// Sets the master sound effect volume.
        /// </summary>
        /// <param name="volume"></param>
        public static void SetSoundEffectVolume(float volume)
        {
            SoundEffect.MasterVolume = volume;
        }
        
        /// <summary>
        /// Fades between two songs as a transition.
        /// </summary>
        /// <param name="initialVolume"></param>
        /// <param name="targetVolume"></param>
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
