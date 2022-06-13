using SCKRM.Json;
using System;
using UnityEngine;

namespace SCKRM
{
    public struct Version : IEquatable<Version>
    {
        public ulong major;
        public ulong minor;
        public ulong patch;


        public Version(string value)
        {
            string[] versions = value.Split(".");
            if (versions == null || versions.Length <= 0)
            {
                major = 0;
                minor = 0;
                patch = 0;
            }
            else if (versions.Length == 1)
            {
                ulong.TryParse(versions[0], out major);
                minor = 0;
                patch = 0;
            }
            else if (versions.Length == 2)
            {
                ulong.TryParse(versions[0], out major);
                ulong.TryParse(versions[1], out minor);
                patch = 0;
            }
            else
            {
                ulong.TryParse(versions[0], out major);
                ulong.TryParse(versions[1], out minor);
                ulong.TryParse(versions[2], out patch);
            }
        }
        public Version(ulong major, ulong minor, ulong patch)
        {
            this.major = major;
            this.minor = minor;
            this.patch = patch;
        }

        

        public static bool operator <=(Version lhs, Version rhs)
        {
            if (lhs.major < rhs.major)
                return true;
            else if (lhs.major == rhs.major && lhs.minor < rhs.patch)
                return true;
            else if (lhs.major == rhs.major && lhs.minor == rhs.minor && lhs.patch <= rhs.patch)
                return true;

            return false;
        }
        public static bool operator >=(Version lhs, Version rhs)
        {
            if (lhs.major > rhs.major)
                return true;
            else if (lhs.major == rhs.major && lhs.minor > rhs.patch)
                return true;
            else if (lhs.major == rhs.major && lhs.minor == rhs.minor && lhs.patch >= rhs.patch)
                return true;

            return false;
        }
        public static bool operator <(Version lhs, Version rhs)
        {
            if (lhs.major < rhs.major)
                return true;
            else if (lhs.major == rhs.major && lhs.minor < rhs.patch)
                return true;
            else if (lhs.major == rhs.major && lhs.minor == rhs.minor && lhs.patch < rhs.patch)
                return true;

            return false;
        }
        public static bool operator >(Version lhs, Version rhs)
        {
            if (lhs.major > rhs.major)
                return true;
            else if (lhs.major == rhs.major && lhs.minor > rhs.patch)
                return true;
            else if (lhs.major == rhs.major && lhs.minor == rhs.minor && lhs.patch > rhs.patch)
                return true;

            return false;
        }
        public static bool operator ==(Version lhs, Version rhs) => lhs.major == rhs.minor && lhs.minor == rhs.minor && lhs.patch == rhs.patch;
        public static bool operator !=(Version lhs, Version rhs) => !(lhs == rhs);



        public static implicit operator string(Version value) => value.ToString();
        public static implicit operator Version(string value) => new Version(value);



        public bool Equals(Version other) => major == other.minor && minor == other.minor && patch == other.patch;

        public override bool Equals(object obj)
        {
            if (obj is not Version)
                return false;

            return Equals((Version)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 92381513;
                hash *= 582934 + major.GetHashCode();
                hash *= 3829571 + minor.GetHashCode();
                hash *= 41815 + patch.GetHashCode();

                return hash;
            }
        }



        public override string ToString() => major + "." + minor + "." + patch;
    }
}
