using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace GwApiNET
{
    /// <summary>
    /// Extensions for structs
    /// </summary>
    public static class StructExtensions
    {
        /// <summary>
        /// Convert a struct to a byte[]
        /// <remarks>Inverse of <see cref="ToStruct{T}"/>
        /// Structures should be decorated with the [StructLayout(LayoutKind.Sequential)] or [StructLayout(LayoutKind.Explicit)] attribute</remarks>
        /// </summary>
        /// <typeparam name="T">struct type implicitly determined</typeparam>
        /// <param name="obj">struct to convert</param>
        /// <returns>byte[], <seealso cref="ToStruct{T}"/></returns>
        public static Byte[] ToByteArray<T>(this T obj) where T : struct
        {
            int size = Marshal.SizeOf(obj);
            var arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);

            return arr;
        }

        /// <summary>
        /// Convert a struct array to a byte[]
        /// <remarks>Inverse of <see cref="ToStructArray{T}"/>
        /// Structures should be decorated with the [StructLayout(LayoutKind.Sequential)] or [StructLayout(LayoutKind.Explicit)] attribute</remarks>
        /// </summary>
        /// <typeparam name="T">struct type implicitly determined</typeparam>
        /// <param name="obj">struct to convert</param>
        /// <returns>byte[], <seealso cref="ToStruct{T}"/></returns>
        public static Byte[] ToByteArray<T>(this T[] obj) where T : struct
        {
            List<byte> listOfBytes = new List<byte>();
            int size = Marshal.SizeOf(typeof(T));
            var arr = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            for (int i = 0; i < obj.Length; i++)
            {
                Marshal.StructureToPtr(obj[i], ptr, false);
                Marshal.Copy(ptr, arr, 0, size);
                listOfBytes.AddRange(arr);
            }
            Marshal.FreeHGlobal(ptr);
            return listOfBytes.ToArray();
        }

        /// <summary>
        /// Convert the byte[] to a struct
        /// <remarks>Inverse of <see cref="ToByteArray{T}"/>
        /// Structures should be decorated with the [StructLayout(LayoutKind.Sequential)] or [StructLayout(LayoutKind.Explicit)] attribute</remarks>
        /// </summary>
        /// <typeparam name="T">struct type</typeparam>
        /// <param name="array">array to convert</param>
        /// <returns>struct of type T, <seealso cref="ToByteArray{T}"/></returns>
        public static T ToStruct<T>(this byte[] array) where T : struct
        {
            return array.ToStruct<T>(0);
        }

        /// <summary>
        /// Convert the byte[] to a struct
        /// <remarks>Inverse of <see cref="ToByteArray{T}"/>
        /// Structures should be decorated with the [StructLayout(LayoutKind.Sequential)] or [StructLayout(LayoutKind.Explicit)] attribute</remarks>
        /// </summary>
        /// <typeparam name="T">struct type</typeparam>
        /// <param name="array">array to convert</param>
        /// <returns>struct of type T, <seealso cref="ToByteArray{T}"/></returns>
        public static T ToStruct<T>(this byte[] array, int start) where T : struct
        {
            int length = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(length);
            Marshal.Copy(array, start, ptr, length);
            var obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
            Marshal.FreeHGlobal(ptr);
            return obj;
        }

        /// <summary>
        /// Convert the byte[] to a struct array
        /// <remarks>Inverse of <see cref="ToByteArray{T}"/>
        /// Structures should be decorated with the [StructLayout(LayoutKind.Sequential)] or [StructLayout(LayoutKind.Explicit)] attribute</remarks>
        /// </summary>
        /// <typeparam name="T">struct type</typeparam>
        /// <param name="array">array to convert</param>
        /// <returns>struct of type T, <seealso cref="ToByteArray{T}"/></returns>
        public static T[] ToStructArray<T>(this byte[] array) where T : struct
        {
            int length = Marshal.SizeOf(typeof(T));
            IntPtr ptr = Marshal.AllocHGlobal(length);
            List<T> list = new List<T>();
            for (int i = 0; i < array.Length / length; i++)
            {
                Marshal.Copy(array, i * length, ptr, length);
                var obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
                list.Add(obj);
            }
            Marshal.FreeHGlobal(ptr);
            return list.ToArray();
        }
    }
}
