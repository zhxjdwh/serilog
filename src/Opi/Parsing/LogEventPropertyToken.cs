﻿using System;
using System.Collections.Generic;
using System.IO;

namespace Opi.Parsing
{
    class LogEventPropertyToken : MessageTemplateToken
    {
        private readonly string _propertyName;
        private readonly string _format;
        private readonly DestructuringHint _destructuringHint;
        private readonly string _rawText;

        public LogEventPropertyToken(string propertyName, string rawText, string format = null, DestructuringHint destructuringHint = DestructuringHint.Default)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");
            if (rawText == null) throw new ArgumentNullException("rawText");
            _propertyName = propertyName;
            _format = format;
            _destructuringHint = destructuringHint;
            _rawText = rawText;
        }

        public override void Render(IReadOnlyDictionary<string, LogEventProperty> properties, TextWriter output)
        {
            if (properties == null) throw new ArgumentNullException("properties");
            if (output == null) throw new ArgumentNullException("output");
            LogEventProperty property;
            if (properties.TryGetValue(_propertyName, out property))
                property.Value.Render(output, _format);
            else
                output.Write(_rawText);
        }

        public string PropertyName { get { return _propertyName; } }

        public LogEventPropertyValue Destructure(object value)
        {
            if (value != null && _destructuringHint == DestructuringHint.Stringify)
                return LogEventPropertyValue.For(value.ToString());

            return LogEventPropertyValue.For(value);
        }
    }
}