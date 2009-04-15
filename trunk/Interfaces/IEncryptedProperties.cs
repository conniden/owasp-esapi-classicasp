/// <summary> OWASP Enterprise Security API .NET (ESAPI.NET)
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
/// <author>  Alex Smolen <a href="http://www.foundstone.com">Foundstone</a>
/// </author>
/// <created>  2008 </created>

using System;
using System.Runtime.InteropServices;

namespace Owasp.Esapi.Interfaces
{
    /// <summary> The IEncryptedProperties interface is a properties file where all the data is
    /// encrypted before it is added, and decrypted when it retrieved.
    /// [P]
    /// [img src="doc-files/EncryptedProperties.jpg" height="600"]
    /// [P]
    /// 
    /// </summary>
    /// <author>  Alex Smolen (alex.smolen@foundstone.com)
    /// </author>
    /// <since> February 20, 2008
    /// </since>
    

  [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IEncryptedProperties
    {

        /// <summary> Gets the property value from the encrypted store, decrypts it, and returns the 
        /// plaintext value to the caller.
        /// </summary>
        /// <param name="key">The key for the property key/value pair.
        /// </param>
        /// <returns> The property (decrypted).
        /// </returns>
        string GetProperty(string key);

        /// <summary> Encrypts the plaintext property value and stores the ciphertext value in the encrypted store.        
        /// </summary>
        /// <param name="key">The key for the property key/value pair.
        /// </param>
        /// <param name="value">The value to set the property to.
        /// </param>
        /// <returns> The value the property was set to.
        /// </returns>
        string SetProperty(string key, string value);

        /// <summary> Load encrypted settings from specific file
        /// </summary>
        /// <param name="filePath">encrypted properties file.
        /// </param>
        void LoadFromFile(string filePath);

        /// <summary> Save encrypted settings to specified file
        /// </summary>
        /// <param name="filePath">encrypted properties file.
        /// </param>
        /// <param name="comments">Comments to save with the properties file.
        /// </param>
        void StoreInFile(string filePath, string comments);
    }
}
