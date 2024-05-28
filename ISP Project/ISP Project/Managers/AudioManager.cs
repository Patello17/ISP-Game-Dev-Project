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
        private static float maximumSongVolume = 1f;
        private static float maximumSFXVolume = 1f;
        private static float volumeLow = 0f;
        private static float volumeHigh = maximumSongVolume;

        /// <summary>
        /// Loads all of the Audio.
        /// </summary>
        public static void LoadAudio()
        {
            // add every song
            songs.Add("Hub Theme", Globals.ContentManager.Load<Song>("Songs/Hub Theme"));
            songs.Add("Hub Theme 1", Globals.ContentManager.Load<Song>("Songs/Hub Theme 1"));
            songs.Add("Hub Theme 2", Globals.ContentManager.Load<Song>("Songs/Hub Theme 2"));
            songs.Add("Hub Theme 3", Globals.ContentManager.Load<Song>("Songs/Hub Theme 3"));
            songs.Add("Title Theme", Globals.ContentManager.Load<Song>("Songs/Title Theme"));
            songs.Add("Level 1 Theme", Globals.ContentManager.Load<Song>("Songs/Level 1"));
            songs.Add("Level 2 Theme", Globals.ContentManager.Load<Song>("Songs/Level 2"));
            songs.Add("Level 3 Theme", Globals.ContentManager.Load<Song>("Songs/Level 3"));

            // add every sound effect
            soundEffects.Add("Envelope", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Envelope"));
            soundEffects.Add("Button Press", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Button Press"));
            soundEffects.Add("Player Movement", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Player Movement"));
            soundEffects.Add("Box Splash", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Box Splash"));
            soundEffects.Add("Victory Jingle", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Victory Jingle"));
            soundEffects.Add("Door Opening", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Door Opening"));
            soundEffects.Add("Collision Not Permitted", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Collision Not Permitted"));
            soundEffects.Add("Reset", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Reset"));
            soundEffects.Add("Scroll", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Scroll"));
            soundEffects.Add("Water Step", Globals.ContentManager.Load<SoundEffect>("Sound Effects/Water Step"));

            // start from the lowest volume
            MediaPlayer.Volume = volumeLow;

            // set sound effect volume
            // SetSoundEffectVolume(0.2f);
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
                if (currentSong != previousSong || currentMediaState == MediaState.Stopped)
                {
                    // MediaPlayer.Volume = maximumSongVolume;
                    MediaPlayer.Play(currentSong);
                    currentSongTime = 0f;
                    isFadingIn = true;
                    isFadingOut = false;
                }
                // signal fade out when nearing the end of a song
                if (Math.Abs(currentSong.Duration.TotalSeconds - currentSongTime) <= fadeDuration)
                {
                    // volumeHigh = MediaPlayer.Volume;
                    isFadingIn = false;
                    isFadingOut = true;
                }
                if (songStack.Count > 1)
                {
                    // prepare to fade in
                    isFadingIn = false;
                    isFadingOut = true;
                    // MediaPlayer.Volume = volumeLow;
                }
            }

            if (isFadingIn)
            {
                // Debug.WriteLine("Fade IN");
                Fade(volumeLow, maximumSongVolume); // fade in
            }
            else if (isFadingOut)
            {
                // Debug.WriteLine("Fade OUT");
                Fade(maximumSongVolume, volumeLow); // fade out
            }

            Debug.WriteLine(maximumSongVolume + " : " + MediaPlayer.Volume);
            // Debug.WriteLine("IN: " + isFadingIn + " | " + "OUT: " + isFadingOut + " || " + "Current Time: " + currentSongTime + " | " + " Remaining Time: " + Math.Abs(currentSong.Duration.TotalSeconds - currentSongTime));

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
        /// Gets the maximum song volume.
        /// </summary>
        /// <returns></returns>
        public static float GetMaximumSongVolume()
        {
            return maximumSongVolume;
        }

        /// <summary>
        /// Gets the maximum sound effects volume.
        /// </summary>
        /// <returns></returns>
        public static float GetMaximumSFXVolume()
        {
            return maximumSFXVolume;
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
                    var isSongInStack = false;
                    foreach (Song _song in songStack)
                    {
                        if (_song == song)
                            isSongInStack = true;
                    }

                    if (!isSongInStack)
                    {
                        songStack.Insert(1, song);
                        // force a fade out
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
            // isFadingIn = true;
            if (songStack.Count > 1)
            {
                // MediaPlayer.Stop();
                songStack.RemoveAt(0);
                // MediaPlayer.Play(songStack[0]);
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
                instance.Volume = maximumSFXVolume;
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
            maximumSongVolume = volume;
            MediaPlayer.Volume = volume;
        }

        /// <summary>
        /// Sets the master sound effect volume.
        /// </summary>
        /// <param name="volume"></param>
        public static void SetSoundEffectVolume(float volume)
        {
            maximumSFXVolume = volume;
            // SoundEffect.MasterVolume = (int)volume;
        }
        
        /// <summary>
        /// Fades between two songs.
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
                    MediaPlayer.Volume = maximumSongVolume;
                    // isFadingOut = false;
                }
                if (isFadingOut)
                {
                    isFadingOut = false;
                    // prepare to fade back in
                    isFadingIn = true;
                    MediaPlayer.Volume = volumeLow;
                    PlayNextSong();
                }
            }
            else
            {
                fadeTimer += Globals.Time;

                var fadeIncrement = ((targetVolume - initialVolume) / fadeDuration) * Globals.Time;
                MediaPlayer.Volume += fadeIncrement;
                Debug.WriteLine(fadeTimer + " : " + fadeIncrement + " : " + MediaPlayer.Volume);
            }
        }
    }
}
