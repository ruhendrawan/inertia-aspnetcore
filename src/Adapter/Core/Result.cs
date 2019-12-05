using Adapter.Extensions;
using Adapter.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Adapter.Core
{
    internal class Result : IActionResult
    {
        private readonly string _component;
        private readonly Props _props;
        private readonly string _rootView;
        private readonly string? _version;
        private Page? _page;
        private ActionContext? _context;
        private IDictionary<string, object>? _viewData;

        internal Result(Props props, string component, string rootView, string? version) =>
            (_props, _component, _rootView, _version) = (props, component, rootView, version);

        internal IActionResult With(object with)
        {
            _props.With = with;
            return this;
        }

        internal IActionResult WithViewData(IDictionary<string, object> viewData)
        {
            _viewData = viewData;
            return this;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            SetContext(context);

            var (isPartial, only) = PartialRequest();

            if (isPartial)
                _props.Controller = InvokeIfLazy(only);

            ConstructPage();

            await GetResult().ExecuteResultAsync(_context);
        }

        private object InvokeIfLazy(IEnumerable<string> str) =>
            str.ToDictionary(o => o, o =>
            {
                var obj = _props.Controller.Value(o);

                return obj.IsLazy() ? ((dynamic) obj).Value : obj;
            });

        private ViewResult View() => new ViewResult
        {
            ViewName = _rootView,
            ViewData = ConstructViewData()
        };

        private JsonResult Json()
        {
            _context.ConfigureResponse();
            return new JsonResult(_page);
        }

        private void ConstructPage() => _page = new Page
        {
            Props = _props,
            Component = _component,
            Version = _version,
            Url = _context.RequestedUrl()
        };

        private IActionResult GetResult() =>
            IsInertiaRequest() ? (IActionResult) Json() : View();


        private (bool, IList<string>) PartialRequest()
        {
            var only = _props.Controller.Only(PartialData());

            return (ComponentName() != _component && only.Count > 0, only);
        }

        private ViewDataDictionary ConstructViewData() => new ViewData(_page, _context, _viewData).ViewDataDictionary;

        private IList<string> PartialData() => _context.GetPartialData();

        private bool IsInertiaRequest() => _context.IsInertiaRequest();

        private string ComponentName() => _context.ComponentName();

        private void SetContext(ActionContext context) => _context = context;
    }
}