using System;

namespace BazisGUI.Win.Animation
{
    public class CreateAnimationEventArgs : EventArgs
    {
        public bool DeleteTempImages { get; }

        public int DelayTime { get; }

        public CreateAnimationEventArgs(bool deleteTempImages, int delayTime)
        {

            DeleteTempImages = deleteTempImages;
            DelayTime = delayTime;
        }
    }
}