using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System;

namespace InertiaAdapter.Models
{
    public class Page
    {
        public string? Component { get; set; }
        public string? Version { get; set; }
        public string? Url { get; set; }
        public Props? Props { get; set; }

        public static dynamic ToMergedProps(Page model) {
            var modelMerged = new ExpandoObject();
            var d = modelMerged as IDictionary<string, object>; //work with the Expando as a Dictionary

            if (model != null) {

                PropertyInfo [] pi = model.GetType().GetProperties();
                foreach (PropertyInfo p in pi)
                {
                    if (p == null) continue;
                    if (p.Name == "Props") {
                        var propsMerged = new ExpandoObject();
                        var props = p.GetValue(model);
                        if (props != null) {
                            Type tProps = props.GetType();
                            PropertyInfo [] pProps = tProps.GetProperties();
                            var dProps = propsMerged as IDictionary<string, object>; //work with the Expando as a Dictionary
                            foreach (PropertyInfo px in pProps)
                            {
                                if (px == null) continue;
                                if (px.Name == "Controller" || px.Name == "Share" || px.Name == "With") {
                                    var propsChild = px.GetValue(props);
                                    if (propsChild != null) {
                                        PropertyInfo [] pChild = propsChild.GetType().GetProperties();
                                        foreach (PropertyInfo pxx in pChild) {
                                            var val = pxx.GetValue(propsChild);
                                            if (val != null) {
                                                dProps[ToCamelCase(pxx.Name)] = val;
                                            }
                                        }
                                    }
                                } else {
                                    var val = px.GetValue(props);
                                    if (val != null) {
                                        dProps[ToCamelCase(px.Name)] = val;
                                    }
                                }
                            }
                        }
                        d[ToCamelCase(p.Name)] = propsMerged;
                    } else {
                        var val = p.GetValue(model);
                        if (val != null) {
                            d[ToCamelCase(p.Name)] = val;
                        }
                    }
                }

            }
            return modelMerged;
        }
 
        static string ToCamelCase(string name)
        {
            var sb = new StringBuilder();
            var i = 0;
            // While we encounter upper case characters (except for the last), convert to lowercase.
            while (i < name.Length - 1 && char.IsUpper(name[i + 1]))
            {
                sb.Append(char.ToLowerInvariant(name[i]));
                i++;
            }

            // Copy the rest of the characters as is, except if we're still on the first character - which is always lowercase.
            while (i < name.Length)
            {
                sb.Append(i == 0 ? char.ToLowerInvariant(name[i]) : name[i]);
                i++;
            }

            return sb.ToString();
        }

    }
}