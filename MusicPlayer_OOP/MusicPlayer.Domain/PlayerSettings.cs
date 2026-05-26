using System;

namespace MusicPlayer.Domain
{
    public sealed class PlayerSettings
    {
        private static PlayerSettings? _instance;
        private static readonly object _lock = new object();

        public bool IsMuted { get; set; }
        public int Volume { get; set; }

        private PlayerSettings()
        {
            IsMuted = false;
            Volume = 50;
        }

        public static PlayerSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new PlayerSettings();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
