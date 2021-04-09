using System;

namespace DIY.Time
{
    public static class  TimeUtil
    {
        public static long GetTimestamp() {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
            //return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
    }
}
