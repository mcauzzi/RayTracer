namespace Globals
{
    public class HelperMethods
    {
        public static double Max(double xt, double yt, double zt)
        {
            return xt < yt ? yt < zt ? zt : yt :
                xt < zt ? zt : xt;
        }

        public static double Min(double xt, double yt, double zt)
        {
            return xt < yt ? xt < zt ? xt : zt :
                yt < zt ? yt : zt;
        }
    }
}