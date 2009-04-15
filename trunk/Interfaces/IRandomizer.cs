﻿/// <summary> OWASP Enterprise Security API .NET (ESAPI.NET)
/// 
/// This file is part of the Open Web Application Security Project (OWASP)
/// Enterprise Security API (ESAPI) project. For details, please see
/// http://www.owasp.org/esapi.
/// 
/// Copyright (c) 2008 - The OWASP Foundation
/// 
/// The ESAPI is published by OWASP under the LGPL. You should read and accept the
/// LICENSE before you use, modify, and/or redistribute this software.
/// 
/// </summary>
/// <author>  Alex Smolen <a href="http://www.foundstone.com">Foundstone[/a]
/// </author>
/// <created>  2008 </created>

using System;
using System.Runtime.InteropServices;

namespace Owasp.Esapi.Interfaces
{
    /// <summary> The IRandomizer interface defines a set of methods for creating
    /// cryptographically random numbers and strings. Implementers should be sure to
    /// use a strong cryptographic implementation, such as the JCE or BouncyCastle.
    /// Weak sources of randomness can undermine a wide variety of security
    /// mechanisms.
    /// [P]
    /// [img src="doc-files/Randomizer.jpg" height="600"]
    /// [P]
    /// </summary>
    /// <author>  Alex Smolen (alex.smolen@foundstone.com)
    /// </author>
    /// <since> February 20, 2008
    /// </since>
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IRandomizer
    {
        /// <summary> Returns a random boolean.</summary>
        bool RandomBoolean
        {
            get;

        }
        /// <summary> Generates a random GUID.</summary>
        string RandomGUID
        {
            get;

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
        string GetRandomString(int length, char[] characterSet);

        /// <summary> 
        /// Gets a random string.
        /// </summary>
        /// <param name="length">
        /// The desired length.
        /// </param>
        /// <param name="characterSet">
        /// The desired character set in a String.
        /// </param>
        /// <returns> The random string.
        /// </returns>
        string GetRandomStringUsingString(int length, string characterSet);

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
        int GetRandomInteger(int min, int max);

        /// <summary>
        /// Returns an unguessable filename.
        /// </summary>
        /// <param name="extension">The extension for the filename</param>
        /// <returns>The unguessable filename</returns>
        string GetRandomFilename(string extension);


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
        float GetRandomReal(float min, float max);

    }
}
