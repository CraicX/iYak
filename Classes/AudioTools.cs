//
//  ██╗██╗   ██╗ █████╗ ██╗  ██╗
//  ██║╚██╗ ██╔╝██╔══██╗██║ ██╔╝
//  ██║ ╚████╔╝ ███████║█████╔╝     AudioTools.cs
//  ██║  ╚██╔╝  ██╔══██║██╔═██╗ 
//  ██║   ██║   ██║  ██║██║  ██╗
//  ╚═╝   ╚═╝   ╚═╝  ╚═╝╚═╝  ╚═╝
//
//  NAudio Tools
//
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using NAudio.SoundFont;



namespace iYak.Classes
{
    public static class AudioTools
    {

        private static WaveOutEvent outputDevice;
        private static AudioFileReader audioFile;

        public static void PlayAudio(string FileName)
        {

            if (outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += OnPlaybackStopped;
            }

            if (audioFile == null)
            {
                //var soundCloudOrangeBlocks = new SoundCloudBlockWaveFormSettings(Color.FromArgb(255, 76, 0), Color.FromArgb(255, 52, 2), Color.FromArgb(255, 171, 141),
                //    Color.FromArgb(255, 213, 199))
                //{ Name = "SoundCloud Orange Blocks" };

                audioFile = new AudioFileReader(@FileName);
                outputDevice.Init(audioFile);
            }
            outputDevice.Play();


        }

        public static void StopAudio()
        {
            outputDevice?.Stop();
        }

        private static void OnPlaybackStopped(object sender, StoppedEventArgs args)
        {
            outputDevice.Dispose();
            outputDevice = null;
            audioFile.Dispose();
            audioFile = null;
        }

    }

    //public class SoundCloudBlockWaveFormSettings : WaveFormRendererSettings
    //{
    //    private readonly Color topSpacerStartColor;
    //    private Pen topPen;
    //    private Pen topSpacerPen;
    //    private Pen bottomPen;
    //    private Pen bottomSpacerPen;

    //    private int lastTopHeight;
    //    private int lastBottomHeight;

    //    public SoundCloudBlockWaveFormSettings(Color topPeakColor, Color topSpacerStartColor, Color bottomPeakColor, Color bottomSpacerColor)
    //    {
    //        this.topSpacerStartColor = topSpacerStartColor;
    //        topPen = new Pen(topPeakColor);
    //        bottomPen = new Pen(bottomPeakColor);
    //        bottomSpacerPen = new Pen(bottomSpacerColor);
    //        PixelsPerPeak = 4;
    //        SpacerPixels = 2;
    //        BackgroundColor = Color.White;
    //        TopSpacerGradientStartColor = Color.White;
    //    }

    //    public override Pen TopPeakPen
    //    {
    //        get { return topPen; }
    //        set { topPen = value; }
    //    }

    //    public Color TopSpacerGradientStartColor { get; set; }

    //    public override Pen TopSpacerPen
    //    {
    //        get
    //        {
    //            if (topSpacerPen == null || lastBottomHeight != BottomHeight || lastTopHeight != TopHeight)
    //            {
    //                topSpacerPen = CreateGradientPen(TopHeight, TopSpacerGradientStartColor, topSpacerStartColor);
    //                lastBottomHeight = BottomHeight;
    //                lastTopHeight = TopHeight;
    //            }
    //            return topSpacerPen;
    //        }
    //        set { topSpacerPen = value; }
    //    }


    //    public override Pen BottomPeakPen
    //    {
    //        get { return bottomPen; }
    //        set { bottomPen = value; }
    //    }


    //    public override Pen BottomSpacerPen
    //    {
    //        get { return bottomSpacerPen; }
    //        set { bottomSpacerPen = value; }
    //    }

    //}

}
