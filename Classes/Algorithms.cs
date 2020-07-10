using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeminarCTDLGT.Classes
{
    public class Algorithms
    {
        public static int LinearSearch(int[] a, int x)
        {
            var i = 0;
            while (i < a.Length && a[i] != x)
                i++;
            if (i == a.Length)
                return -1;
            return i;
        }

        public static int BinarySearch(int[] a, int x)
        {
            int l = 0, r = a.Length - 1;
            while (l <= r)
            {
                var mid = l + (r - l) / 2;
                if (a[mid] == x)
                    return mid;
                if (x < a[mid])
                    r = mid - 1;
                else
                    l = mid + 1;
            }

            return -1;
        }

        public static int InterpolationSearch(int[] a, int x)
        {
            var l = 0;
            var m = -1;
            var h = a.Length - 1;
            while (l <= h && x >= a[l] && x <= a[h])
            {
                m = (int)(l + (((double)(h - l) / (a[h] - a[l])) * (x - a[l])));

                if (a[m] == x)
                    return m;
                if (a[m] < x)
                    l = m + 1;
                else
                    h = m - 1;
            }
            return -1;
        }

        public static void Swap(ref int a, ref int b)
        {
            var t = a;
            a = b;
            b = t;
        }

        public static void InterchangeSort(int[] a)
        {
            for (var i = 0; i < a.Length - 1; i++)
                for (var j = i + 1; j < a.Length; j++)
                    if (a[j] < a[i])
                        Swap(ref a[i], ref a[j]);
        }

        public static void BubbleSort(int[] a)
        {
            for (var i = 0; i < a.Length - 1; i++)
                for (var j = 0; j < a.Length - 1 - i; j++)
                    if (a[j + 1] < a[j])
                        Swap(ref a[j], ref a[j + 1]);
        }

        public static void SelectionSort(int[] a)
        {
            for (var i = 0; i < a.Length - 1; i++)
            {
                var min = i;
                for (var j = i + 1; j < a.Length; j++)
                    if (a[j] < a[min])
                        min = j;
                if (min != i)
                    Swap(ref a[i], ref a[min]);
            }
        }

        public static void InsertionSort(int[] a)
        {
            for (var i = 0; i < a.Length - 1; i++)
            {
                var x = a[i];
                var pos = i - 1;
                while (pos >= 0 && x < a[pos])
                {
                    a[pos + 1] = a[pos];
                    pos--;
                }

                a[pos + 1] = x;
            }
        }

        public static void QuickSort(int[] a, int l, int r)
        {
            if (l >= r)
                return;
            var x = a[l + (r - l) / 2];
            int i = l, j = r;
            while (i < j)
            {
                while (a[i] < x)
                    i++;
                while (a[j] > x)
                    j--;
                if (i <= j)
                {
                    if (i < j)
                        Swap(ref a[i], ref a[j]);
                    i++;
                    j--;
                }
            }

            QuickSort(a, l, j);
            QuickSort(a, i, r);
        }

        public static void Merge(int[] a, int l, int m, int r)
        {
            var n1 = m - l + 1;
            var n2 = r - m;

            var lArr = new int[n1];
            var rArr = new int[n2];

            int i;
            for (i = 0; i < n1; i++)
                lArr[i] = a[l + i];
            for (i = 0; i < n2; i++)
                rArr[i] = a[m + 1 + i];

            i = 0;
            var j = 0;
            var current = l;
            while (i < n1 && j < n2)
                if (lArr[i] <= rArr[j])
                    a[current++] = lArr[i++];
                else
                    a[current++] = rArr[j++];

            while (i < n1)
                a[current++] = lArr[i++];
            while (j < n2)
                a[current++] = rArr[j++];
        }

        public static void MergeSort(int[] a, int l, int r)
        {
            if (l < r)
            {
                var m = l + (r - l) / 2;

                MergeSort(a, l, m);
                MergeSort(a, m + 1, r);
                Merge(a, l, m, r);
            }
        }

        public static void Shift(int[] a, int l, int r)
        {
            var i = l;
            var j = 2 * i + 1;
            var x = a[i];
            while (j <= r)
            {
                if (j < r)
                    if (a[j] < a[j + 1])
                        j++;

                if (a[j] < x)
                    return;

                a[i] = a[j];
                a[j] = x;
                i = j;
                j = 2 * i + 1;
                x = a[i];
            }
        }

        public static void CreateHeap(int[] a, int n)
        {
            var l = n / 2;
            while (l >= 0)
            {
                Shift(a, l, n - 1);
                l--;
            }
        }

        public static void HeapSort(int[] a, int n)
        {
            var r = n - 1;
            CreateHeap(a, n);
            while (r > 0)
            {
                Swap(ref a[0], ref a[r]);
                r--;
                Shift(a, 0, r);
            }
        }
    }
}