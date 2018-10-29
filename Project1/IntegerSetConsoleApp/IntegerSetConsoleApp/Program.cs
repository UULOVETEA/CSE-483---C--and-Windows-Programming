using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegerSetConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            "Demostrating IntegerSet Class".title('=');
            "\n  Constructing Set 1".title();
            IntegerSet s1 = new IntegerSet();
            "\n  Constructing Set 2".title();
            IntegerSet s2 = new IntegerSet();

            bool[] data = new bool[200];
            data[10] = true;
            data[20] = true;
            data[99] = true;
            data[100] = true;
            data[150] = true;
            "\n  Constructing Set 3 from bool[]".title();
            IntegerSet s3 = new IntegerSet(data);

            //Testing
            "\n  Demostrating Overwritten ToString()".title('=');
            testString("Set 3", s3);
            "\n  Demonstrating Insert()".title('=');
            testInsert("Set 1", s1, 5);
            testInsert("Set 1", s1, 6);
            testInsert("Set 2", s2, 6);
            testInsert("Set 2", s2, 7);
            PrintSet("Set 1", s1);
            PrintSet("set 2", s2);
            "\n  Demonstrating Delete()".title('=');
            testDelete("Set 3", s3, 100);
            "\n  Demonstrating isEqual()".title('=');
            testEqual("Set 1", "Set 2", s1, s2);
            "\n  Demonstrating Union()".title('=');
            testUnion("Set 1", "Set 2", s1, s2);
            "\n  Demonstrating Intersection()".title('=');
            testIntersection("Set 1", "Set 2", s1, s2);
            "\n  Demonstrating Clear()".title('=');
            testClear("Set 3", s3);
            Console.WriteLine();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static void PrintSet(string setName, IntegerSet set)
        {
            Console.Write("\n  " + setName + " = " + set.ToString());
        }
        static void testString(string setName, IntegerSet s3)
        {
            Console.Write("\n  Contents of " + setName + ": ");
            Console.Write(" " + s3.ToString());
        }
        static void testInsert(string setName, IntegerSet s1, int num)
        {
            Console.Write("\n  Inserting " + num + " into " + setName);
            s1.InsertElement(num);
        }
        static void testDelete(string setName, IntegerSet s3, int num)
        {
            Console.Write("\n  Contents of " + setName + ": " + s3.ToString());
            Console.Write("\n  Deleting element 100");
            s3.DeleteElement(num);
            Console.Write("\n  Contents of Set 3:");
            Console.Write(" " + s3.ToString());
        }
        static void testEqual(string setName1, string setName2, IntegerSet s1, IntegerSet s2)
        {
            Console.Write("\n  Comparing " + setName1 + " and " + setName2);
            Console.Write("\n  Comparison returned: " + s1.IsEqualTo(s2));
        }
        static void testUnion(string setName1, string setName2, IntegerSet s1, IntegerSet s2)
        {
            IntegerSet s4 = s1.Union(s2);
            Console.Write("\n  s4 = " + setName1 + " Union " + setName2);
            Console.Write("\n  Contents of Set 4: " + s4.ToString());

        }
        static void testIntersection(string setName1, string setName2, IntegerSet s1, IntegerSet s2)
        {
            IntegerSet s5 = s2.Intersection(s1);
            Console.Write("\n  s5 = " + setName1 + " Intersection " + setName2);
            Console.Write("\n  Contents of Set 5: " + s5.ToString());
        }
        static void testClear(string setName, IntegerSet s3)
        {
            Console.Write("\n  Contents of " + setName + ":");
            Console.Write(" " + s3.ToString());
            Console.Write("\n  Clearing " + setName);
            s3.Clear();
            Console.Write("\n  Contents of " + setName + ":");
            Console.Write(" " + s3.ToString());
        }


    }
    public static class UtilityExtensions
    {
        public static void title(this string aString, char underline = '-')
        {
            Console.Write("\n  {0}", aString);
            Console.Write("\n {0}", new string(underline, aString.Length + 2));
        }
    }
}
