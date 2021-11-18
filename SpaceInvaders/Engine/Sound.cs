using System.Windows.Media;

namespace SpaceInvaders
{
    internal static class Sound
    {
        internal static MediaPlayer Theme
        {
            get
            {
                var media = new MediaPlayer();
                media.Open(new System.Uri(@"..\..\Resources\theme.wav", System.UriKind.Relative));
                return media;
            }
        }

        internal static MediaPlayer Shoot
        {
            get
            {
                var media = new MediaPlayer();
                media.Open(new System.Uri(@"..\..\Resources\shoot.wav", System.UriKind.Relative));
                return media;
            }
        }
    }
}