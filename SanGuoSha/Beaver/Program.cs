using System;

namespace BeaverMarkupLanguage
{
    class Program
    {
        static void Main(string[] args)
        {
            Beaver t = new Beaver("head",new int[]{1,3,5,7,9});
            string s = t.ToString();
            Console.WriteLine(s);
            t = BeaverDecoder.Decoding(s);

            Console.WriteLine(t);
            Console.Read();
        }
    }
}
