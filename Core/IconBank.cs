using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace FocalPointer
{
    public static class IconBank
    {
        private static readonly Icon[] _icons = new Icon[]
        {
            Properties.Resources.idle,
            Properties.Resources.idle,
            Properties.Resources.idle,
            Properties.Resources.idle,
            Properties.Resources.focusing_off,
            Properties.Resources.focusing_on,
            Properties.Resources.focusing_paused,
            Properties.Resources.focusing_paused,
            Properties.Resources.idle,
            Properties.Resources.idle,
            Properties.Resources.idle,
            Properties.Resources.idle,
            Properties.Resources.resting_off,
            Properties.Resources.resting_on,
            Properties.Resources.resting_paused,
            Properties.Resources.resting_paused
        };

        private static Dictionary<int, Dictionary<Icon, Bitmap>> _bitmapCache = new Dictionary<int, Dictionary<Icon, Bitmap>>();

        public static Bitmap GetBitmapForIcon(Icon icon, int size)
        {
            if (!_bitmapCache.ContainsKey(size))
            {
                _bitmapCache.Add(size, new Dictionary<Icon, Bitmap>());
            }

            var cache = _bitmapCache[size];
            if (!cache.ContainsKey(icon))
            {
                var bitmap = new Icon(icon, size, size).ToBitmap();
                cache[icon] = bitmap;
            }

            return cache[icon];
        }

        public static Bitmap GetBitmapForState(int size, PluginBase.IntervalType type, PluginBase.StateChangeType state, bool heartbeat)
        {
            return GetBitmapForIcon(GetIconForState(type, state, heartbeat), size);
        }

        public static Icon GetIconForState(PluginBase.IntervalType type, PluginBase.StateChangeType state, bool heartbeat)
        {
            int frame = 0;

            if (type == PluginBase.IntervalType.Rest)
                frame = 8;

            if (state == PluginBase.StateChangeType.Start)
                frame |= 4;

            if (state == PluginBase.StateChangeType.Pause)
                frame |= 2;

            if (heartbeat)
                frame |= 1;

            return _icons[frame];
        }
    }
}
