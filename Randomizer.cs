﻿/// <summary> OWASP Enterprise Security API .NET (Esapi.NET)
/// 
/// This file is part of the Open Web Application Security Project (OWASP)
/// Enterprise Security API (Esapi) project. For details, please see
/// http://www.owasp.org/Esapi.
/// 
/// Copyright (c) 2008 - The OWASP Foundation
/// 
/// The Esapi is published by OWASP under the LGPL. You should read and accept the
/// LICENSE before you use, modify, and/or redistribute this software.
/// 
/// </summary>
/// <author>  Alex Smolen <a href="http://www.foundstone.com">Foundstone</a>
/// </author>
/// <created>  2008 </created>

using System;
using System.ComponentModel;
using Owasp.Esapi.Interfaces;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Net;
using Owasp.Esapi.Errors;
using System.Runtime.InteropServices;

namespace Owasp.Esapi
{
    /// <summary> Reference implemenation of the IRandomizer interface. This implementation builds on the JCE provider to provide a
    /// cryptographically strong source of entropy. The specific algorithm used is configurable in Esapi.properties.
    /// 
    /// </summary>
    /// <author>  Alex Smolen [a href="http://www.foundstone.com"]Foundstone[/a]
    /// </author>
    /// <since> February 20, 2008
    /// </since>
    /// <seealso cref="Owasp.Esapi.Interfaces.IRandomizer">
    /// </seealso>
    /// 
  [ClassInterface(ClassInterfaceType.None)]
  [ProgId("Owasp_Esapi.Randomizer")]
    public class Randomizer : Component, IRandomizer
    {
        private RandomNumberGenerator randomNumberGenerator = null;

        private static readonly Logger logger;

        /// <summary> Hide the constructor for the Singleton pattern.</summary>
        public Randomizer()
        {
            string algorithm = Esapi.SecurityConfiguration().RandomAlgorithm;
            try
            {
                //Todo: Right now algorithm is ignored
                randomNumberGenerator = RNGCryptoServiceProvider.Create();
            }
            catch (Exception e)
            {
                // Can't throw an exception from the constructor, but this will get
                // it logged and tracked
                new EncryptionException("Error creating randomizer", "Can't find random algorithm " + algorithm, e);
            }
        }

        /// <summary> Returns a random boolean.</summary>
        /// <seealso cref="Owasp.Esapi.Interfaces.IRandomizer.RandomBoolean">
        /// </seealso>
        public bool RandomBoolean
        {        
            get
            {                
                byte[] randomByte = new byte[1];
                randomNumberGenerator.GetBytes(randomByte);
                return (randomByte[0] >= 128);
            }
        }
        /// <summary> Generates a random GUID.</summary>
        /// <seealso cref="Owasp.Esapi.Interfaces.IRandomizer.RandomGUID">
        /// </seealso>
        public string RandomGUID
        {
            get
            {
                // create random string to seed the GUID
                StringBuilder sb = new StringBuilder();
                try
                {
                    IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
                    
                    sb.Append(ipHostEntry.AddressList[0].ToString());
                }
                catch (System.Exception e)
                {
                    sb.Append("0.0.0.0");
                }
                sb.Append(":");
                sb.Append(DateTime.Now.Ticks);
                sb.Append(":");
                sb.Append(this.GetRandomString(20, Encoder.CHAR_ALPHANUMERICS));

                // hash the random string to get some random bytes
                string hash = Esapi.Encryptor().Hash(sb.ToString(), "salt");
                byte[] array = null;
                try
                {
                    array = Esapi.Encoder().DecodeFromBase64(hash);
                }
                catch (IOException e)
                {
                    logger.LogCritical(ILogger_Fields.SECURITY, "Problem decoding hash while creating GUID: " + hash);
                }

                // convert to printable hexadecimal characters 
                StringBuilder hex = new StringBuilder();
                for (int j = 0; j < array.Length; ++j)
                {
                    int b = array[j] & 0xFF;
                    if (b < 0x10)
                        hex.Append('0');
                    hex.Append(Convert.ToString(b, 16));
                }
                string raw = hex.ToString().ToUpper();

                // convert to standard GUID format
                StringBuilder result = new StringBuilder();
                result.Append(raw.Substring(0, (8) - (0)));
                result.Append("-");
                result.Append(raw.Substring(8, (12) - (8)));
                result.Append("-");
                result.Append(raw.Substring(12, (16) - (12)));
                result.Append("-");
                result.Append(raw.Substring(16, (20) - (16)));
                result.Append("-");
                result.Append(raw.Substring(20));
                return result.ToString();
            }

        }

        /// <summary> 
        /// Gets a random string.
        /// </summary>
        /// <param name="length">
        /// The desired length.
        /// </param>
        /// <param name="characterSet">
        /// The desired character set.
        /// </param>
        /// <returns> The random string.
        /// </returns>
        /// <seealso cref="Owasp.Esapi.Interfaces.IRandomizer.GetRandomString(int, char[])">
        /// </seealso>
        public string GetRandomString(int length, char[] characterSet)
        {
            StringBuilder sb = new StringBuilder();

            for (int loop = 0; loop < length; loop++)
            {                
                int index = GetRandomInteger(0, characterSet.GetLength(0) - 1);                    
                sb.Append(characterSet[index]);                
            }
            string nonce = sb.ToString();
            return nonce;
        }

        /// <summary> 
        /// Gets a random string.
        /// </summary>
        /// <param name="length">
        /// The desired length.
        /// </param>
        /// <param name="characterSet">
        /// The desired character in a string.
        /// </param>
        /// <returns> The random string.
        /// </returns>
        /// <seealso cref="Owasp.Esapi.Interfaces.IRandomizer.GetRandomStringUsingString(int, string)">
        /// </seealso>
        public string GetRandomStringUsingString(int length, string characterSet)
        {
            StringBuilder sb = new StringBuilder();

            for (int loop = 0; loop < length; loop++)
            {
                int index = GetRandomInteger(0, characterSet.Length - 1);
                sb.Append(characterSet.Substring(index, 1));
            }
            string nonce = sb.ToString();
            return nonce;
        }


        /// <summary> 
        /// Gets a random integer.        
        /// </summary>
        /// <param name="min">
        /// The minimum value.
        /// </param>
        /// <param name="max">
        /// The maximum value.        
        /// </param>
        /// <returns> 
        /// The random integer
        /// </returns>
        /// <seealso cref="Owasp.Esapi.Interfaces.IRandomizer.GetRandomInteger(int, int)">
        /// </seealso>
        public int GetRandomInteger(int min, int max)
        {           
            int range = max - min;
            byte[] randomBytes = new byte[4];
            randomNumberGenerator.GetBytes(randomBytes);
            uint randomFactor = BitConverter.ToUInt32(randomBytes, 0);
            double divisor =  randomFactor / (UInt32.MaxValue + 0.0);
            int randomNumber = Convert.ToInt32(Math.Round(range * divisor)) + min;

            return randomNumber;
        }

        // Note:  Use float or double here?
        /// <summary> 
        /// Gets a random real.
        /// 
        /// </summary>
        /// <param name="min">
        /// The minimum value.
        /// </param>
        /// <param name="max">
        /// The maximum value.        
        /// </param>
        /// <returns>
        /// The random real.
        /// </returns>
        /// <seealso cref="Owasp.Esapi.Interfaces.IRandomizer.GetRandomReal(float, float)">
        /// </seealso>
        public float GetRandomReal(float min, float max)
        {
            float factor = max - min;
            float random = GetRandomInteger(0, Int32.MaxValue) / Int32.MaxValue;
            return (float) random * factor + min;
        }

        /// <summary>
        /// Returns an unguessable filename.
        /// </summary>
        /// <param name="extension">The extension for the filename</param>
        /// <returns>The unguessable filename</returns>
        /// <seealso cref="Owasp.Esapi.Interfaces.IRandomizer.GetRandomFilename(string)">
        /// </seealso>
        public string GetRandomFilename(string extension)
        {
            return this.GetRandomString(12, Encoder.CHAR_ALPHANUMERICS) + "." + extension;
        }

        /// <summary> Union two character arrays.
        /// </summary>
        /// <param name="c1">The first character array.
        /// </param>
        /// <param name="c2">The second character array.
        /// </param>
        /// <returns> The union of the two charater arrays.
        /// </returns>
        public static char[] Union(char[] c1, char[] c2)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < c1.Length; i++)
            {
                if (!Contains(sb, c1[i]))
                    sb.Append(c1[i]);
            }
            for (int i = 0; i < c2.Length; i++)
            {
                if (!Contains(sb, c2[i]))
                    sb.Append(c2[i]);
            }
            char[] c3 = new char[sb.Length];
            int i2;
            int j;
            i2 = 0;
            j = 0;
            while (i2 < sb.Length)
            {
                c3[j] = sb[i2];
                i2++;
                j++;
            }
            Array.Sort(c3);
            return c3;
        }

        /// <summary> Determines if a string buffer contains a char
        /// 
        /// </summary>
        /// <param name="sb">The string buffer.
        /// </param>
        /// <param name="c">The char.
        /// </param>
        /// <returns> true, if found.
        /// </returns>
        public static bool Contains(StringBuilder sb, char c)
        {            
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] == c)
                    return true;
            }
            return false;
        }
        
        /// <summary>
        /// Static constuctor
        /// </summary>
        static Randomizer()
        {
            logger = Logger.GetLogger("Esapi", "Randomizer");
        }
    }
}
