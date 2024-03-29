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
/// <author>  Alex Smolen <a href="http://www.foundstone.com">Foundstone</a>
/// </author>
/// <created>  2008 </created>

using System;
using System.Runtime.InteropServices;

namespace Owasp.Esapi.Interfaces
{
    /// <summary> The IAccessController interface defines a set of methods that can be used in a wide variety of applications to
    /// enforce access control. In most applications, access control must be performed in multiple different locations across
    /// the various applicaton layers. This class provides access control for URLs, business functions, data, services, and
    /// files.   
    /// 
    /// The implementation of this interface will need to access some sort of user information repository to determine what
    /// roles or permissions are assigned to the accountName passed into the various methods. In addition, the implementation
    /// will also need information about the resources that are being accessed. Using the user information and the resource
    /// information, the implementation should return an access control decision.
    /// 
    /// Implementers are encouraged to build on existing access control mechanisms, such as methods like isUserInRole() or
    /// hasPrivilege(). While powerful, these methods can be confusing, as users may be in multiple roles or possess multiple
    /// overlapping privileges. These methods encourage the use of complex boolean tests throughout the code. The point of
    /// this interface is to centralize access control logic so that it is easy to use and easy to verify.
    /// 
    /// 
    /// if (Esapi.AccessController.().IsAuthorizedForFunction( BUSINESS_FUNCTION ) ) 
    /// {
    /// ... access is allowed
    /// } else {
    /// ... attack in progress
    /// }
    /// 
    /// 
    /// Note that in the user interface layer, access control checks can be used to control whether particular controls are
    /// rendered or not. These checks are supposed to fail when an unauthorized user is logged in, and do not represent
    /// attacks. Remember that regardless of how the user interface appears, an attacker can attempt to invoke any business
    /// function or access any data in your application. Therefore, access control checks in the user interface should be
    /// repeated in both the business logic and data layers.
    /// 
    /// 
    /// &lt;% if ( Esapi.AccessController().IsAuthorizedForFunction( ADMIN_FUNCTION ) ) 
    /// { %&gt;
    /// &lt;a href=&quot;/doAdminFunction&quot;&gt;ADMIN&lt;/a&gt;
    /// &lt;% } else { %&gt;
    /// &lt;a href=&quot;/doNormalFunction&quot;&gt;NORMAL&lt;/a&gt;
    /// &lt;% } %&gt;
    /// 
    /// Attibutes added by Juan C Calderon to support COM Interop for Classic ASPs
    /// </summary>
    /// <author>  Alex Smolen (alex.smolen@foundstone.com)
    /// </author>
    
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface IAccessController
    {
        /// <summary> Checks if an account is authorized to access the referenced URL. The implementation should allow
        /// access to be granted to any part of the URI.
        /// 
        /// </summary>
        /// <param name="url">The url.
        /// </param>
        /// <returns> true, if the user is authorized for the URL.
        /// </returns>
        bool IsAuthorizedForUrl(string url);

        /// <summary> Checks if an account is authorized to access the referenced function. The implementation should define the
        /// function "namespace" to be enforced. Choosing something simple like the classname of action classes or menu item
        /// names will make this implementation easier to use.
        /// 
        /// </summary>
        /// <param name="functionName">The function name.
        /// </param>
        /// <returns> true, if the user is authorized for the function.
        /// </returns>
        bool IsAuthorizedForFunction(string functionName);

        /// <summary> Checks if an account is authorized to access the referenced data. The implementation should define the data
        /// "namespace" to be enforced.
        /// 
        /// </summary>
        /// <param name="key">The key.
        /// </param>
        /// <returns> true, if the user is authorized for the data.
        /// </returns>
        bool IsAuthorizedForData(string key);

        /// <summary> Checks if an account is authorized to access the referenced file. The implementation should be extremely careful
        /// about canonicalization.
        /// 
        /// </summary>
        /// <param name="filepath">The filepath.
        /// </param>
        /// <returns> true, if the user is authorized for the file.
        /// </returns>
        bool IsAuthorizedForFile(string filepath);

        /// <summary> Checks if an account is authorized to access the referenced service. This can be used in applications that
        /// provide access to a variety of backend services.
        /// 
        /// </summary>
        /// <param name="serviceName">The service name.
        /// </param>
        /// <returns> true, if the user is authorized for the service.
        /// </returns>
        bool IsAuthorizedForService(string serviceName);
        
        
        
        // FIXME: these should log! isAuthorizedForXXXshould not log (for use in UI)
        void AssertAuthorizedForUrl(String url);
        void AssertAuthorizedForFunction(String functionName);
        void AssertAuthorizedForData(String key);
        void AssertAuthorizedForFile(String filepath);
        void AssertAuthorizedForService(String serviceName);
        
    }
}
