﻿using System;
using System.Runtime.CompilerServices;
using UnityMvvmToolkit.Core.Interfaces;
using UnityMvvmToolkit.Core.Internal.Interfaces;
using UnityMvvmToolkit.Core.Internal.StringParsers;

namespace UnityMvvmToolkit.Core
{
    public abstract class BindableCommandElement : IBindableElement
    {
        private readonly IObjectProvider _objectProvider;
        private readonly CommandStringParser _commandStringParser;

        protected BindableCommandElement(IObjectProvider objectProvider)
        {
            _objectProvider = objectProvider;
            _commandStringParser = new CommandStringParser();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected TCommand GetCommand<TCommand>(string propertyName) where TCommand : IBaseCommand
        {
            return _objectProvider.GetCommand<TCommand>(propertyName);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected ICommandWrapper GetCommandWrapper(int elementId, string commandStringData)
        {
            var commandData = _commandStringParser.GetCommandData(commandStringData.AsMemory());
            var commandWrapper = _objectProvider.GetCommandWrapper(commandData.PropertyName.ToString(),
                commandData.ParameterConverterName);

            if (commandWrapper is ICommandWrapperWithParameter commandWrapperWithParameter)
            {
                commandWrapperWithParameter.SetParameter(elementId, commandData.ParameterValue);
            }

            return commandWrapper;
        }
    }
}