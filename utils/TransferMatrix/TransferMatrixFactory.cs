using System;
using System.Numerics;
using Godot;

namespace protonic.utils.TransferMatrix
{
    public static class TransferMatrix2x2Factory
    {
        public static Matrix2x2 Drift(float length)
        {
            return new Matrix2x2(
                1, length,
                0, 1
            );
        }

        /*public static Matrix2x2 EdgeFocusing(float phi, float radius)
        {
            return new Matrix4x4(
                1, 0, 0, 0,
                Mathf.Tan(phi) / radius, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, -Mathf.Tan(phi) / radius, 1
            );
        }*/

        // Dipole transfer matrix
        public static Matrix2x2 Dipole(float length, float radius)
        {
            float angle = length / radius; // Calculate bending angle
            return new Matrix2x2(
                Mathf.Cos(angle), radius * Mathf.Sin(angle),
                (-1 / radius) * Mathf.Sin(angle), Mathf.Cos(angle)
            );
        }

        // Focusing quadrupole transfer matrix
        public static Matrix2x2 FocusingQuadrupole(float k, float length)
        {
            float omega = Mathf.Sqrt(Mathf.Abs(k)) * length;
            return new Matrix2x2(
                Mathf.Cos(omega), (1 / Mathf.Sqrt(Mathf.Abs(k))) * Mathf.Sin(omega), 
                -Mathf.Sqrt(Mathf.Abs(k)) * Mathf.Sin(omega), Mathf.Cos(omega)
            );
        }

        // Defocusing quadrupole transfer matrix
        public static Matrix2x2 DefocusingQuadrupole(float k, float length)
        {
            float omega = Mathf.Sqrt(Mathf.Abs(k)) * length;
            return new Matrix2x2(
                Mathf.Cosh(omega), (1 / Mathf.Sqrt(Mathf.Abs(k))) * Mathf.Sinh(omega), 
                Mathf.Sqrt(Mathf.Abs(k)) * Mathf.Sinh(omega), Mathf.Cosh(omega)
            );
        }
    }

    
    public static class TransferMatrix4x4Factory
    {
        public static Matrix4x4 Drift(float length)
        {
            return new Matrix4x4(
                1, length, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, length,
                0, 0, 0, 1);
        }

        public static Matrix4x4 EdgeFocusing(float phi, float radius)
        {
            return new Matrix4x4(
                1, 0, 0, 0,
                Mathf.Tan(phi) / radius, 1, 0, 0,
                0, 0, 1, 0,
                0, 0, -Mathf.Tan(phi) / radius, 1
            );
        }

        // Dipole transfer matrix
        public static Matrix4x4 Dipole(float length, float radius)
        {
            float angle = length / radius; // Calculate bending angle
            return new Matrix4x4(
                Mathf.Cos(angle), radius * Mathf.Sin(angle), 0, 0,
                (-1 / radius) * Mathf.Sin(angle), Mathf.Cos(angle), 0, 0,
                0, 0, 1, length,
                0, 0, 0, 1
            );
        }

        // Focusing quadrupole transfer matrix
        public static Matrix4x4 FocusingQuadrupole(float k, float length)
        {
            float omega = Mathf.Sqrt(Mathf.Abs(k)) * length;
            return new Matrix4x4(
                Mathf.Cos(omega), (1 / Mathf.Sqrt(Mathf.Abs(k))) * Mathf.Sin(omega), 0, 0,
                -Mathf.Sqrt(Mathf.Abs(k)) * Mathf.Sin(omega), Mathf.Cos(omega), 0, 0,
                0, 0, Mathf.Cosh(omega), (1 / Mathf.Sqrt(Mathf.Abs(k))) * Mathf.Sinh(omega),
                0, 0, Mathf.Sqrt(Mathf.Abs(k)) * Mathf.Sinh(omega), Mathf.Cosh(omega)
            );
        }

        // Defocusing quadrupole transfer matrix
        public static Matrix4x4 DefocusingQuadrupole(float k, float length)
        {
            float omega = Mathf.Sqrt(Mathf.Abs(k)) * length;
            return new Matrix4x4(
                Mathf.Cosh(omega), (1 / Mathf.Sqrt(k)) * Mathf.Sinh(omega), 0, 0,
                Mathf.Sqrt(Mathf.Abs(k)) * Mathf.Sinh(omega), Mathf.Cosh(omega), 0, 0,
                0, 0, Mathf.Cos(omega), (1 / Mathf.Sqrt(Mathf.Abs(k))) * Mathf.Sin(omega),
                0, 0, -Mathf.Sqrt(Mathf.Abs(k)) * Mathf.Sin(omega), Mathf.Cos(omega)
            );
        }
    }

}