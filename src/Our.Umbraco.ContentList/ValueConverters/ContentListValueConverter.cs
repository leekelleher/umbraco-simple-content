﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Our.Umbraco.ContentList.PropertyEditors;
using Our.Umbraco.InnerContent.ValueConverters;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Core.PropertyEditors;

namespace Our.Umbraco.ContentList.ValueConverters
{
    public class ContentListValueConverter : InnerContentValueConverter, IPropertyValueConverterMeta
    {
        public override bool IsConverter(PublishedPropertyType propertyType)
        {
            return propertyType.PropertyEditorAlias.InvariantEquals(ContentListPropertyEditor.PropertyEditorAlias);
        }

        public override object ConvertDataToSource(PublishedPropertyType propertyType, object source, bool preview)
        {
            var value = source?.ToString();
            if (string.IsNullOrWhiteSpace(value))
                return null;

            try
            {
                var items = JsonConvert.DeserializeObject<JArray>(value);
                return ConvertInnerContentDataToSource(items, null, 1, preview);
            }
            catch (Exception ex)
            {
                LogHelper.Error<ContentListValueConverter>("Error converting value", ex);
            }

            return null;
        }

        public Type GetPropertyValueType(PublishedPropertyType propertyType)
        {
            return typeof(IEnumerable<IPublishedContent>);
        }

        public PropertyCacheLevel GetPropertyCacheLevel(PublishedPropertyType propertyType, PropertyCacheValue cacheValue)
        {
            return PropertyCacheLevel.Content;
        }
    }
}