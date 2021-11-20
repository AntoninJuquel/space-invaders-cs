using System.Windows.Media;

namespace SpaceInvaders.Managers
{
    internal static class Sound
    {
        /// <summary>
        /// Find the theme.wav ressource of type System.Windows.Media.MediaPlayer.
        /// </summary>
        internal static MediaPlayer Theme
        {
            get
            {
                var media = new MediaPlayer();
                media.Open(new System.Uri(@"..\..\Resources\theme.wav", System.UriKind.Relative));
                return media;
            }
        }

        /// <summary>
        /// Find the shoot.wav ressource of type System.Windows.Media.MediaPlayer.
        /// </summary>
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