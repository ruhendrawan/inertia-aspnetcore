using InertiaAdapter.Extensions;
using InertiaAdapter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;

namespace InertiaAdapter.Core
{
    internal class ViewData
    {
        private readonly IDictionary<string, object>? _viewData;

        internal ViewDataDictionary ViewDataDictionary { get; }


        internal ViewData(Page? page, ActionContext? ac, IDictionary<string, object>? viewData)
        {
            _viewData = viewData;

            ViewDataDictionary =
                new ViewDataDictionary(new EmptyModelMetadataProvider(), ac.NotNull().ModelState) { 
                    Model = page is null? null : Page.ToMergedProps(page)
                };

            PrepareViewDataDictionary();
        }

        private void PrepareViewDataDictionary()
        {
            if (_viewData is null) return;

            foreach (var (key, value) in _viewData)
                ViewDataDictionary[key] = value;
        }
    }
}