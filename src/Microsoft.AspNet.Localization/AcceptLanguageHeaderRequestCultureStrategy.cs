﻿// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

using System.Globalization;
using System.Linq;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Localization.Internal;
using Microsoft.Framework.Internal;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNet.Localization
{
    /// <summary>
    /// Determines the culture information for a request via the value of the Accept-Language header.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class AcceptLanguageHeaderRequestCultureStrategy : IRequestCultureStrategy
    {
        /// <summary>
        /// The maximum number of values in the Accept-Language header to attempt to create a <see cref="CultureInfo"/>
        /// from for the current request.
        /// Defaults to <c>3</c>.
        /// </summary>
        public int MaximumAcceptLanguageHeaderValuesToTry { get; set; } = 3;

        /// <inheritdoc />
        public RequestCulture DetermineRequestCulture([NotNull] HttpContext httpContext)
        {
            var acceptLanguageHeader = httpContext.Request.GetTypedHeaders().AcceptLanguage;

            if (acceptLanguageHeader == null || acceptLanguageHeader.Count == 0)
            {
                return null;
            }

            var languages = acceptLanguageHeader.AsEnumerable();

            if (MaximumAcceptLanguageHeaderValuesToTry > 0)
            {
                // We take only the first configured number of languages from the header and then order those that we
                // attempt to parse as a CultureInfo to mitigate potentially spinning CPU on lots of parse attempts.
                languages = languages.Take(MaximumAcceptLanguageHeaderValuesToTry);
            }
            
            var orderedLanguages = languages.OrderByDescending(h => h, StringWithQualityHeaderValueComparer.QualityComparer)
                .ToList();

            foreach (var language in orderedLanguages)
            {
                // Allow empty string values as they map to InvariantCulture, whereas null culture values will throw in
                // the CultureInfo ctor
                if (language.Value != null)
                {
                    try
                    {
                        return new RequestCulture(CultureUtilities.GetCultureFromName(language.Value));
                    }
                    catch (CultureNotFoundException) { }
                }
            }

            return null;
        }
    }
}