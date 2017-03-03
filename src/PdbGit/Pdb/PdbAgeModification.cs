// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdbAgeModification.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2016 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PdbGit.Pdb
{
    using System;
    using System.IO;
    using System.Linq;

    internal static class PdbAgeModification
    {
        internal static void RestoreAge(string pdbPath, int currentAge, int age, Guid signature)
        {
            var replacements = 0;
            using (var stream = File.Open(pdbPath, FileMode.Open, FileAccess.ReadWrite, FileShare.Delete | FileShare.Read))
            {
                var buffer = new byte[stream.Length];
                var r = 0;
                while (r < buffer.Length)
                {
                    var z = stream.Read(buffer, r, buffer.Length - r);
                    if (z == 0)
                    {
                        throw new EndOfStreamException();
                    }

                    r += z;
                }

                var oldvalue = BitConverter.GetBytes(currentAge).Concat(signature.ToByteArray()).ToArray();
                var newvalue = BitConverter.GetBytes(age).Concat(signature.ToByteArray()).ToArray();
                var idx = 0;
                while (true)
                {
                    var pos = IndexOf(buffer, oldvalue, idx);
                    if (pos == -1)
                    {
                        break;
                    }

                    stream.Seek(pos, SeekOrigin.Begin);
                    stream.Write(newvalue, 0, newvalue.Length);
                    idx = pos + newvalue.Length;
                    replacements++;
                }
            }

            if (replacements < 2)
            {
                throw new FormatException();
            }
        }

        public static int IndexOf(byte[] haystack, byte[] needle, int startIndex)
        {
            for (int i = startIndex; i < haystack.Length - needle.Length + 1; ++i)
            {
                var found = true;
                for (int j = 0; j < needle.Length; ++j)
                {
                    if (haystack[i + j] != needle[j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
