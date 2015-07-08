using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeospaceEntity.Helper
{
    public class Time
    {
        public Limits HH;
        public Limits MI;

        public Time()
        {
            HH = new Limits(0, 23);
            MI = new Limits(0, 59);
        }

        public Time(int hh_, int mi_)
        {
            HH = new Limits(hh_, 0, 23);
            MI = new Limits(mi_, 0, 59);

            //if (mi_ > 59) HH++;
        }

        public Time(Time obj)
        {
            HH = obj.HH;
            MI = obj.MI;
        }

        public void init(int hh_, int mi_)
        {
            HH = new Limits(hh_, 0, 23);
            MI = new Limits(mi_, 0, 59);

            //if (mi_ > 59) HH++;
        }
        public void init(Time obj)
        {
            HH = obj.HH;
            MI = obj.MI;
        }

        //метод для проверки ЧЧ и МИ
        public bool Check_Format()
        {
            if (HH.start > HH.val || HH.val > HH.end) return false;
            if (MI.start > MI.val || MI.val > MI.end) return false;

            return true;
        }

        public static Time operator *(int a, Time b)
        {
            Time c = new Time(b);
            c.HH = a * b.HH;
            c.MI = a * b.MI;
            return c;
        }

        public static Time operator +(Time a, Time b)
        {
            Time c = new Time();
            c.MI = a.MI + b.MI;
            c.HH = a.HH + b.HH;
            if (a.MI.val + b.MI.val > a.MI.end) c.HH = c.HH + new Limits((a.MI.val + b.MI.val) / (a.MI.end + 1), a.MI.start, a.MI.end);

            return c;
        }

        public static Time operator -(Time a, Time b)
        {
            Time c = new Time();
            c.MI = a.MI - b.MI;
            c.HH = a.HH - b.HH;
            if ((a.MI - b.MI).val > a.MI.val) c.HH--;

            return c;
        }

        public static bool operator >(Time a, Time b)
        {
            if (a.HH > b.HH) return true;
            if (a.HH == b.HH && a.MI > b.MI) return true;
            return false;
        }

        public static bool operator >=(Time a, Time b)
        {
            if (a.HH > b.HH) return true;
            if (a.HH == b.HH && a.MI >= b.MI) return true;
            return false;
        }

        public static bool operator <(Time a, Time b)
        {
            if (a.HH < b.HH) return true;
            if (a.HH == b.HH && a.MI < b.MI) return true;
            return false;
        }

        public static bool operator <=(Time a, Time b)
        {
            if (a.HH < b.HH) return true;
            if (a.HH == b.HH && a.MI <= b.MI) return true;
            return false;
        }

        public static bool operator ==(Time a, Time b)
        {
            if (a.HH == b.HH && a.MI == b.MI) return true;
            return false;
        }

        public static bool operator !=(Time a, Time b)
        {
            if (a.HH != b.HH && a.MI != b.MI) return true;
            return false;
        }
    }

    public class Limits
    {
        public int val = 0;
        public int start = 0;
        public int end = 0;

        public Limits(int val_) { val = val_; }

        public Limits(Limits obj) { val = obj.val; }

        public Limits(int val_, int start_, int end_) { init(val_, start_, end_); }

        public Limits(int start_, int end_) { start = start_; end = end_; }

        public void init(int start_, int end_) { start = start_; end = end_; }

        public void init(int val_, int start_, int end_)
        {
            val = val_;
            start = start_;
            end = end_;

            //if (val < start) val = start;
            //if (val > end) val = start + val - end - 1;
        }

        public static Limits operator *(int a, Limits b)
        {
            return new Limits(a * b.val, b.start, b.end);
        }

        public static Limits operator +(Limits a, Limits b)
        {
            if (a.val + b.val <= a.end) return new Limits(a.val + b.val, a.start, a.end);
            else return new Limits((a.val + b.val) % (a.end + 1), a.start, a.end);
        }

        public static Limits operator ++(Limits a)
        {
            if (a.val + 1 <= a.end) return new Limits(a.val + 1, a.start, a.end);
            else return new Limits(a.start, a.start, a.end);
        }

        public static Limits operator -(Limits a, Limits b)
        {
            if (a.val - b.val >= a.start) return new Limits(a.val - b.val, a.start, a.end);
            else return new Limits(a.end - Math.Abs(a.val - b.val) + 1, a.start, a.end);
        }

        public static Limits operator --(Limits a)
        {
            if (a.val - 1 >= a.start) return new Limits(a.val - 1, a.start, a.end);
            else return new Limits(a.end, a.start, a.end);
        }

        public static bool operator >(Limits a, Limits b)
        {
            if (a.val > b.val) return true;

            return false;
        }

        public static bool operator >=(Limits a, Limits b)
        {
            if (a.val >= b.val) return true;

            return false;
        }

        public static bool operator <(Limits a, Limits b)
        {
            if (a.val < b.val) return true;

            return false;
        }
        public static bool operator <=(Limits a, Limits b)
        {
            if (a.val <= b.val) return true;

            return false;
        }

        public static bool operator ==(Limits a, Limits b)
        {
            if (a.val == b.val) return true;

            return false;
        }

        public static bool operator !=(Limits a, Limits b)
        {
            if (a.val != b.val) return true;

            return false;
        }
    }
}
