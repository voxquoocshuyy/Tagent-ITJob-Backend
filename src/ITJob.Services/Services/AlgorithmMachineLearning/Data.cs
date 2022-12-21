// using System.Globalization;
// using System.Numerics;
// using Microsoft.EntityFrameworkCore.Metadata.Internal;
//
// namespace ITJob.Services.Services.AlgorithmMachineLearning;
//
// public class Data
// {
//     // private vector = new Vector<Key, ValueType>();
//     public readonly struct Vector<T> : IEquatable<System.Numerics.Vector<T>>, IFormattable where T : struct, IEquatable<T>, IFormattable
//     {
//         public Vector(T x, T y, T z)
//         {
//             X = x;
//             Y = y;
//             Z = z;
//         }
//
//         public T X { get; }
//         public T Y { get; }
//         public T Z { get; }
//
//         public bool Equals(Vector<T> other)
//         {
//             return X.Equals(other.X) && Y.Equals(other.Y) && Z.Equals(other.Z);
//         }
//
//         public bool Equals(System.Numerics.Vector<T> other)
//         {
//             throw new NotImplementedException();
//         }
//
//         public override bool Equals(object obj)
//         {
//             return obj is Vector<T> other && Equals(other);
//         }
//
//         public override int GetHashCode()
//         {
//             unchecked
//             {
//                 var hashCode = X.GetHashCode();
//                 hashCode = (hashCode * 397) ^ Y.GetHashCode();
//                 hashCode = (hashCode * 397) ^ Z.GetHashCode();
//                 return hashCode;
//             }
//         }
//
//         public override string ToString()
//         {
//             return ToString("G", CultureInfo.CurrentCulture);
//         }
//
//         public string ToString(string format, IFormatProvider formatProvider)
//         {
//             return string.Format(formatProvider, "({0}, {1}, {2})", X.ToString(format, formatProvider), Y.ToString(format, formatProvider), Z.ToString(format, formatProvider));
//         }
//     }
// }