using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarpWriter.Model.Extra
{
    public static class BasicTools
    {
        public static T[][] Fill<T>(T item, int width, int height)
        {
            T[][] stuff = new T[width][];
            for (int x = 0; x < width; x++)
            {
                stuff[x] = new T[height];
                for (int y = 0; y < height; y++)
                {
                    stuff[x][y] = item;
                }

            }
            return stuff;
        }
        public static T[][][] Fill<T>(T item, int width, int height, int depth)
        {
            T[][][] stuff = new T[width][][];
            for (int x = 0; x < width; x++)
            {
                stuff[x] = new T[height][];
                for (int y = 0; y < height; y++)
                {
                    stuff[x][y] = new T[depth];
                    for (int z = 0; z < depth; z++)
                    {
                        stuff[x][y][z] = item;
                    }
                }
            }
            return stuff;
        }
        public static T[][][][] Fill<T>(T item, int duration, int width, int height, int depth)
        {
            T[][][][] stuff = new T[duration][][][];
            for (int w = 0; w < duration; w++)
            {
                stuff[w] = new T[width][][];
                for (int x = 0; x < width; x++)
                {
                    stuff[w][x] = new T[height][];
                    for (int y = 0; y < height; y++)
                    {
                        stuff[w][x][y] = new T[depth];
                        for(int z = 0; z < depth; z++)
                        {
                            stuff[w][x][y][z] = item;
                        }
                    }
                }
            }
            return stuff;
        }

        public static int Clamp(int n, int lower, int upper)
        {
            return Math.Max(lower, Math.Min(n, upper));
        }

        public static uint Clamp(uint n, uint lower, uint upper)
        {
            return Math.Max(lower, Math.Min(n, upper));
        }


        public static long Clamp(long n, long lower, long upper)
        {
            return Math.Max(lower, Math.Min(n, upper));
        }


        public static ulong Clamp(ulong n, ulong lower, ulong upper)
        {
            return Math.Max(lower, Math.Min(n, upper));
        }

        public static float Clamp(float n, float lower, float upper)
        {
            return Math.Max(lower, Math.Min(n, upper));
        }

        public static double Clamp(double n, double lower, double upper)
        {
            return Math.Max(lower, Math.Min(n, upper));
        }
    }
}
