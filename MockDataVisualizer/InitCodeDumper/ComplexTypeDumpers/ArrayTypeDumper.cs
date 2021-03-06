﻿using System;
using System.Collections;

namespace MockDataDebugVisualizer.InitCodeDumper.ComplexTypeDumpers
{
    public class ArrayTypeDumper : AbstractComplexTypeDumper
    {
        private readonly int _arrayLength;

        public ArrayTypeDumper(object element, string name) : base(element, name)
        {
            _arrayLength = (Element as Array).Length;
        }

        public override void ResolveTypeInitilization(CodeBuilder codeBuilder)
        {
            var genericArguments = Element.GetType().GetGenericArguments();

            var typeName = Element.GetType().Name;

            if (genericArguments.Length == 0)
            {
                var initCode = string.Format("var {0} = new {1}[{2}];", ElementName, typeName.Substring(0, typeName.Length - 2), _arrayLength);

                codeBuilder.AddCode(initCode);
            }
        }

        public override void ResolveMembers(CodeBuilder codeBuilder)
        {
            var elementList = Element as IList;

            for (var i = 0; i < elementList.Count; i++)
            {
                if(elementList[i] == null) continue;

                var dumper = GetDumper(elementList[i], elementList[i].GetType().Name);

                dumper.ResolveInitCode(codeBuilder);

                codeBuilder.AddCode(string.Format("{0}[{1}] = {2};", ElementName, i, codeBuilder.PopInitValue()));
            }
        }
    }
}
