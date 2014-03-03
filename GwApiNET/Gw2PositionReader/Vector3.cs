using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GwApiNET.Gw2PositionReader
{
    /// <summary>
    /// 3D vector used to store position/location information
    /// </summary>
    public struct Vector3
    {
        /// <summary>
        /// X-Axis value
        /// </summary>
        public double X;
        /// <summary>
        /// Y-Axis value
        /// </summary>
        public double Y;
        /// <summary>
        /// Z-Axis value
        /// </summary>
        public double Z;

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="X">X-Axis value</param>
        /// <param name="Y">Y-Axis value</param>
        /// <param name="Z">Z-Axis value</param>
        public Vector3(double X, double Y, double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        public override string ToString()
        {
            return string.Format("X:{0},Y:{1},Z:{2}", X, Y, Z);
        }
        /// <summary>
        /// Provides multiplication operator to the point using a scaler.
        /// </summary>
        /// <param name="point">value to multiply</param>
        /// <param name="scaler">scaler to multiply the X and Y value by.</param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 point, double scaler)
        {
            return new Vector3(point.X * scaler, point.Y * scaler, point.Z * scaler);
        }

        /// <summary>
        /// Provides multiplication operator to the point using a scaler.
        /// </summary>
        /// <param name="point">value to multiply</param>
        /// <param name="scaler">scaler to multiply the X and Y value by.</param>
        /// <returns></returns>
        public static Vector3 operator *(double scaler, Vector3 point)
        {
            return point * scaler;
        }
    }
}
