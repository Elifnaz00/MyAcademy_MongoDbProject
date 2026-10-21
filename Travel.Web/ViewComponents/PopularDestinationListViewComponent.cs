

using Microsoft.AspNetCore.Mvc;
using Travel.Web.Services.DestinationServices;

namespace Travel.Web.ViewComponents
{
    public class PopularDestinationListViewComponent : ViewComponent
    {
        private readonly IDestinationService _destinationService;

        public PopularDestinationListViewComponent(IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var destinations= await _destinationService.GetPopularDestinationsAsync();
            return View(destinations);
        }
    }
}
