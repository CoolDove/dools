namespace dove.math
{
public class Mathd
{
    public static int PadTo(int _from, int _to) {
        int remainder = _from % _to;
        if (remainder != 0) return _from + (_to - remainder);
        return _from;
    }
}
}
