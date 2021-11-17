using System.Windows.Media;

namespace SpaceInvaders
{
    class Sound
    {
        internal static MediaPlayer theme
        {
            get
            {
                var media = new MediaPlayer();
                media.Open(new System.Uri(@"..\..\Resources\theme.wav", System.UriKind.Relative));
                return media;
            }
        }

        internal static MediaPlayer shoot
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
