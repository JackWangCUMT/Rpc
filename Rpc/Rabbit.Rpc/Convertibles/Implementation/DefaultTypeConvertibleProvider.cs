﻿using Rabbit.Rpc.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit.Rpc.Convertibles.Implementation
{
    /// <summary>
    /// 一个默认的类型转换提供程序。
    /// </summary>
    public class DefaultTypeConvertibleProvider : ITypeConvertibleProvider
    {
        private readonly ISerializer _serializer;

        public DefaultTypeConvertibleProvider(ISerializer serializer)
        {
            _serializer = serializer;
        }

        #region Implementation of ITypeConvertibleProvider

        /// <summary>
        /// 获取类型转换器。
        /// </summary>
        /// <returns>类型转换器集合。</returns>
        public IEnumerable<TypeConvertDelegate> GetConverters()
        {
            yield return SimpleTypeConvert;
            yield return ComplexTypeConvert;
        }

        #endregion Implementation of ITypeConvertibleProvider

        #region Private Method

        private static object SimpleTypeConvert(object instance, Type conversionType)
        {
            if (instance is IConvertible && typeof(IConvertible).IsAssignableFrom(conversionType))
            {
                return Convert.ChangeType(instance, conversionType);
            }
            return null;
        }

        private object ComplexTypeConvert(object instance, Type conversionType)
        {
            //todo:应该使用更通用的协议byte[]
            return _serializer.Deserialize(Encoding.UTF8.GetBytes(instance.ToString()), conversionType);
        }

        #endregion Private Method
    }
}