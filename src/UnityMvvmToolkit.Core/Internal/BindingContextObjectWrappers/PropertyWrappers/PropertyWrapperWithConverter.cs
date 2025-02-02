﻿using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityMvvmToolkit.Core.Extensions;
using UnityMvvmToolkit.Core.Interfaces;

namespace UnityMvvmToolkit.Core.Internal.BindingContextObjectWrappers.PropertyWrappers
{
    internal class PropertyWrapperWithConverter<TObjectType, TValueType, TSourceType> : IProperty<TValueType>
    {
        private readonly TObjectType _obj;
        private readonly Func<TObjectType, TSourceType> _getPropertyDelegate;
        private readonly Action<TObjectType, TSourceType> _setPropertyDelegate;
        private readonly IPropertyValueConverter<TSourceType, TValueType> _valueConverter;

        public PropertyWrapperWithConverter(TObjectType obj, PropertyInfo propertyInfo,
            IPropertyValueConverter<TSourceType, TValueType> valueConverter)
        {
            _obj = obj;
            _valueConverter = valueConverter;
            _getPropertyDelegate = propertyInfo.CreateGetValueDelegate<TObjectType, TSourceType>();
            _setPropertyDelegate = propertyInfo.CreateSetValueDelegate<TObjectType, TSourceType>();
        }

        public TValueType Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _valueConverter.Convert(_getPropertyDelegate(_obj));
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _setPropertyDelegate(_obj, _valueConverter.ConvertBack(value));
        }
    }
}