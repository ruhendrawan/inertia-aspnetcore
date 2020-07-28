using InertiaAdapter.Extensions;
using InertiaAdapter.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InertiaAdapter.Core
{
    public class Result : IActionResult
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

        public IActionResult With(object with)
        {
            _props.With = with;
            return this;
        }

        public IActionResult WithViewData(IDictionary<string, object> viewData)
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

        public dynamic? GetMergedPage()
        {
            ConstructPage();
            return _page is null? null : Page.ToMergedProps(_page);
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
            return new JsonResult(
                _page is null? null : Page.ToMergedProps(_page)
            );
        }

        private void ConstructPage() => _page = new Page
        {
            Props = _props,
            Component = _component,
            Version = _version,
            Url = _context is null? null : _context.RequestedUri()
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