using System;
using System.Reactive.Linq;
using YumaPos.Client.Services;
using YumaPos.Shared.API.Models;
using YumaPos.Shared.API.Models.Ordering;
using YumaPos.Shared.Core.MenuModels;

namespace Y_POS.Core.MockData
{
    public class MockMenuService : IMenuService
    {
        public MockMenuService()
        {
        }

        public IObservable<IMenuCategory> GetMenuCategoriesStream()
        {
            throw new NotImplementedException();
        }

        public IObservable<IMenuCategory[]> GetMenuCategoriesCollection()
        {
            throw new NotImplementedException();
        }

        public IObservable<IMenuItem> GetMenuItemsForCategoryStream(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public IObservable<IMenuItem[]> GetMenuItemsCollectionForCategory(Guid categoryId)
        {
            throw new NotImplementedException();
        }

        public IObservable<IMenuItem[]> GetSearchedMenuItems(string searchText)
        {
            if (string.IsNullOrEmpty(searchText))
                return Observable.Return(new IMenuItem[] { });

            throw new NotImplementedException();
        }

        public IObservable<IRelatedModifier> GetRelatedModifiersStream(Guid menuItemId)
        {
            throw new NotImplementedException();
        }

        public IObservable<IRelatedModifier[]> GetRelatedModifiersForMenuItemId(Guid menuItemId)
        {
            throw new NotImplementedException();
        }

        public IObservable<IModifierGroup[]> GetCommonModifiersGroupsForMenuItem(Guid menuItemId)
        {
            throw new NotImplementedException();
        }

        public IObservable<ICommonModifier[]> GetCommonModifiersForGroup(Guid groupId)
        {
            throw new NotImplementedException();
        }

        public IObservable<bool> MenuItemHasRelatedModifiers(Guid menuItemId)
        {
            throw new NotImplementedException();
        }

        public IObservable<MenuCacheDto> CacheMenu()
        {
            throw new NotImplementedException();
        }

        private static AvailabilityDto Map(IAvailability availability)
        {
            return new AvailabilityDto
            {
                AvailabilityId = availability.AvailabilityId,
                Days = availability.Days,
                IsChecked = availability.IsChecked,
                TimeFrom = availability.DaySecondsFrom,
                TimeTo = availability.DaySecondsTo
            };
        }
    }
}
