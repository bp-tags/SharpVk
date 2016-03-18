﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace SharpVk.VkXml
{
    public class TypeDefCache
    {
        private IVkXmlCache xmlCache;
        private Dictionary<string, TypeDef> cache;

        private static readonly Dictionary<string, string> primitiveTypes = new Dictionary<string, string>()
        {
            {"void", "void"},
            {"char", "char"},
            {"float", "float"},
            {"uint8_t", "byte"},
            {"uint32_t", "uint"},
            {"uint64_t", "ulong"},
            {"int32_t", "int"},
            {"size_t", "UIntPtr"}
        };

        public TypeDefCache(IVkXmlCache xmlCache)
        {
            this.xmlCache = xmlCache;
        }

        public Dictionary<string, TypeDef> GetAllTypes()
        {
            if (cache == null)
            {
                var vkXml = this.xmlCache.GetVkXml();

                Category typeCategory = Category.None;

                var result = new Dictionary<string, TypeDef>();

                foreach (var vkType in vkXml.Element("registry").Element("types").Elements("type"))
                {
                    var additionalTypeNames = new List<string>();

                    string vkRequires = vkType.Attribute("requires")?.Value;
                    bool isPrimitive = false;
                    bool isReturnedOnly = vkType.Attribute("returnedonly")?.Value == "true";

                    if (vkType.Attribute("category") == null || !Enum.TryParse(vkType.Attribute("category").Value, out typeCategory))
                    {
                        if (vkRequires == "vk_platform")
                        {
                            typeCategory = Category.vk_platform;
                        }
                        else
                        {
                            typeCategory = Category.other_platform;
                        }
                    }

                    string vkTypeName = null;

                    if (vkType.Attribute("name") != null)
                    {
                        vkTypeName = vkType.Attribute("name").Value;
                    }
                    else
                    {
                        vkTypeName = vkType.Element("name").Value;
                    }

                    if (vkTypeName.Contains("SurfaceCreateInfo"))
                    {
                        // Skip these for now...

                        continue;
                    }

                    string typeName = vkTypeName;

                    if (primitiveTypes.ContainsKey(vkTypeName))
                    {
                        typeName = primitiveTypes[vkTypeName];
                        isPrimitive = true;
                    }
                    else if (typeCategory == Category.bitmask)
                    {
                        if (vkRequires == null)
                        {
                            typeName = "uint";
                        }
                        else
                        {
                            additionalTypeNames.Add(vkRequires);
                        }
                    }
                    else if (typeCategory == Category.funcpointer)
                    {
                        typeName = "IntPtr";
                    }

                    if (typeName.StartsWith("vk", true, null))
                    {
                        typeName = typeName.Substring(2);
                    }

                    additionalTypeNames.Add(vkTypeName);

                    //HACK
                    if(typeName.EndsWith("Bits"))
                    {
                        typeName = typeName.Substring(0, typeName.Length - 4) + "s";
                    }

                    var typeDef = new TypeDef
                    {
                        Category = typeCategory,
                        Name = typeName,
                        Xml = vkType,
                        Requires = vkRequires,
                        IsPrimitive = isPrimitive,
                        IsReturnedOnly = isReturnedOnly
                    };

                    foreach (var additionalTypeName in additionalTypeNames)
                    {
                        if (!result.ContainsKey(vkTypeName))
                        {
                            result.Add(vkTypeName, typeDef);
                        }
                    }
                }

                this.cache = result;

                var constantsEnum = vkXml.Element("registry").Elements("enums").First(x => x.Attribute("name")?.Value == "API Constants");

                var constants = new Dictionary<string, int>();

                foreach(var constant in constantsEnum.Elements("enum"))
                {
                    int value;
                    string name = constant.Attribute("name").Value;

                    if (int.TryParse(constant.Attribute("value").Value, out value))
                    {
                        constants.Add(name, value);
                    }
                }

                foreach (var type in this.cache.Values)
                {
                    if (type.Category == Category.@struct || type.Category == Category.union)
                    {
                        var members = type.Members;
                        
                        foreach (var typeMember in type.Xml.Elements("member"))
                        {
                            var nameNodes = new XNode[] { typeMember.Element("name") }.Concat(typeMember.Element("name").NodesAfterSelf());

                            string memberName = nameNodes.Select(x =>
                            {
                                var xAsText = x as XText;
                                if (xAsText != null)
                                {
                                    return xAsText.Value;
                                }
                                else
                                {
                                    var xAsElement = x as XElement;
                                    if (xAsElement?.Name == "enum")
                                    {
                                        return constants[xAsElement.Value].ToString();
                                    }
                                    else if (xAsElement?.Name == "name")
                                    {
                                        return xAsElement.Value;
                                    }
                                    else
                                    {
                                        throw new NotSupportedException();
                                    }
                                }
                            }).Aggregate((x, y) => x + y);

                            int count = 0;

                            int arraySuffixIndex = memberName.IndexOf('[');
                            if (arraySuffixIndex >= 0)
                            {
                                count = int.Parse(new string(memberName.Skip(arraySuffixIndex + 1).TakeWhile(x => char.IsDigit(x)).ToArray()));

                                memberName = memberName.Substring(0, arraySuffixIndex);
                            }

                            string vkMemberName = memberName;

                            int pointerCount = 0;

                            while (memberName[0] == 'p' && (memberName[1] == 'p' || char.IsUpper(memberName[1])))
                            {
                                pointerCount++;

                                memberName = memberName.Substring(1);
                            }

                            memberName = char.ToUpper(memberName[0]) + memberName.Substring(1);

                            string[] memberLen = typeMember.Attribute("len")?.Value?.Split(',');

                            var memberType = cache[typeMember.Element("type").Value];

                            members.Add(new TypeDef.MemberInfo
                            {
                                Type = memberType,
                                Name = memberName,
                                PointerCount = pointerCount,
                                Size = count,
                                Len = memberLen,
                                VkName = vkMemberName
                            });
                        }
                    }
                }
            }

            return cache;
        }
    }
}
