﻿using System.Collections;

namespace MockDataDebugVisualizer.InitCodeDumper.ComplexTypeDumpers
{
    public class DictionaryTypeDumper : AbstractComplexTypeDumper
    {
        public DictionaryTypeDumper(object element, string name) : base(element, name) { }

        public override void ResolveTypeInitilization(CodeBuilder codeBuilder)
        {
            var genericArguments = Element.GetType().GetGenericArguments();

            var typeName = Element.GetType().Name;

            if (genericArguments.Length == 2)
            {
                var initCode = string.Format("var {0} = new {1}<{2}, {3}>();", ElementName,
                    typeName.Substring(0, typeName.Length - 2), Element.GetType().GetGenericArguments()[0].Name,
                    Element.GetType().GetGenericArguments()[1].Name);

                codeBuilder.AddCode(initCode);
            }
        }

        public override void ResolveMembers(CodeBuilder codeBuilder)
        {
            var enumerableElement = Element as IEnumerable;

            foreach (var element in enumerableElement)
            {
                dynamic keyValue = element;

                var keyDumper = GetDumper(keyValue.Key, keyValue.Key.GetType().Name);

                var valueDumper = GetDumper(keyValue.Value, keyValue.Value.GetType().Name);

                keyDumper.ResolveInitCode(codeBuilder);

                var line = string.Format("{0}.Add({1},", ElementName, codeBuilder.PopInitValue());

                valueDumper.ResolveInitCode(codeBuilder);
   
                line = string.Format("{0} {1});", line, codeBuilder.PopInitValue());

                codeBuilder.AddCode(line);
            }
        }
    }
}
