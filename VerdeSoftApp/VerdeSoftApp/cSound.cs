//using Android.Media; //ref mono.android
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace VerdeSoftApp
{
    public static class cSound
    {
        //    public static void beepSound()
        //    {
        //        //ToneGenerator generator = new ToneGenerator(Android.Media.Stream.Alarm, 100);
        //        //generator.StartTone(Tone.CdmaAlertCallGuard);
        //        //Android.OS.SystemClock.Sleep(1000);
        //        //generator.Release();
        //    }

        public static void beepSound()
        {
            var stream = GetStreamFromFile("myBeepShort.wav");
            var audio = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current; //Xam.Plugin.SimpleAudioPlayer
            audio.Load(stream);
            audio.Play();
        }

        public static System.IO.Stream GetStreamFromFile(string filename)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;

            var stream = assembly.GetManifestResourceStream("VerdeSoftApp." + filename);

            return stream;
        }
    }
}
