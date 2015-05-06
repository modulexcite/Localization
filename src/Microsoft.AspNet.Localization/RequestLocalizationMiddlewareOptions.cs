﻿// Copyright (c) .NET Foundation. All rights reserved. 
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Microsoft.AspNet.Localization
{
    /// <summary>
    /// Specifies options for the <see cref="RequestLocalizationMiddleware"/>.
    /// </summary>
    public class RequestLocalizationMiddlewareOptions
    {
        /// <summary>
        /// Creates a new <see cref="RequestLocalizationMiddlewareOptions"/> with default values.
        /// </summary>
        public RequestLocalizationMiddlewareOptions()
        {
            DefaultRequestCulture = new RequestCulture(CultureInfo.DefaultThreadCurrentCulture, CultureInfo.DefaultThreadCurrentUICulture);

            RequestCultureStrategies = new List<IRequestCultureStrategy>
            {
                new QueryStringRequestCultureStrategy(),
                new CookieRequestCultureStrategy(),
                new AcceptLanguageHeaderRequestCultureStrategy { MaximumAcceptLanguageHeaderValuesToTry = MaximumAcceptLanguageHeaderValuesToTry }
            };
        }

        /// <summary>
        /// The maximum number of values in the Accept-Language header to attempt to create a <see cref="CultureInfo"/>
        /// from for the current request.
        /// Defaults to <c>3</c>.
        /// </summary>
        public int MaximumAcceptLanguageHeaderValuesToTry { get; set; } = 3;

        /// <summary>
        /// The default <see cref="RequestCulture"/> to use. This value will be used if none of the configured
        /// <see cref="IRequestCultureStrategy"/> options result in a non-<c>null</c> result.
        /// Defaults to <see cref="RequestCulture.Culture"/> set to <see cref="CultureInfo.DefaultThreadCurrentCulture"/>
        /// and <see cref="RequestCulture.UICulture"/> set to <see cref="CultureInfo.DefaultThreadCurrentUICulture"/>.
        /// </summary>
        public RequestCulture DefaultRequestCulture { get; set; }

        /// <summary>
        /// The cultures supported by the application. If this value is non-<c>null</c>, the
        /// <see cref="RequestLocalizationMiddleware"/> will only set the current request culture to an entry in this
        /// list. A value of <c>null</c> means all cultures are supported.
        /// Defaults to <c>null</c>.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Improves usability")]
        public IList<CultureInfo> SupportedCultures { get; set; }

        /// <summary>
        /// The UI cultures supported by the application. If this value is non-<c>null</c>, the
        /// <see cref="RequestLocalizationMiddleware"/> will only set the current request culture to an entry in this
        /// list. A value of <c>null</c> means all cultures are supported.
        /// Defaults to <c>null</c>.
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Improves usability")]
        public IList<CultureInfo> SupportedUICultures { get; set; }

        /// <summary>
        /// An ordered list of strategies used to determine a request's culture information. The first strategy that
        /// returns a non-<c>null</c> result for a given request will be used.
        /// Defaults to the following:
        /// <list type="number">
        ///     <item><description><see cref="QueryStringRequestCultureStrategy"/></description></item>
        ///     <item><description><see cref="CookieRequestCultureStrategy"/></description></item>
        ///     <item><description><see cref="AcceptLanguageHeaderRequestCultureStrategy"/></description></item>
        /// </list>
        /// </summary>
        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Improves usability")]
        public IList<IRequestCultureStrategy> RequestCultureStrategies { get; set; }
    }
}