using MyFinances.Core.Dtos;
using MyFinances.Core.Response;
using MyFinances.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyFinances.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {

        public ObservableCollection<OperationDto> Operations { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command DeleteItemCommand { get; }
        public Command PreviousPageCommand { get; }
        public Command NextPageCommand { get; }

        public Command<OperationDto> ItemTapped { get; }
        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                PreviousPageCommand.ChangeCanExecute();
                NextPageCommand.ChangeCanExecute();
            }
        }
        public int LastPage
        {
            get => _lastPage;
            set
            {
                _lastPage = value;
                NextPageCommand.ChangeCanExecute();
            }
        }

        private int _currentPage = 1;
        private int _lastPage = 1;
        private const int pageSize = 4;


        public ItemsViewModel()
        {
            Title = "Operacje";
            Operations = new ObservableCollection<OperationDto>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<OperationDto>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
            DeleteItemCommand = new Command<OperationDto>(async (x) => await OnDeleteItem(x));
            PreviousPageCommand = new Command(async (x) => await ExecutePreviousPageCommand(), ValidatePreviousPage);
            NextPageCommand = new Command(async (x) => await ExecuteNextPageCommand(), ValidateNextPage);
        }

        private bool ValidateNextPage(object arg)
        {
            return CurrentPage != LastPage;
        }

        private bool ValidatePreviousPage(object arg)
        {
            return CurrentPage != 1;
        }

        private async Task ExecuteNextPageCommand()
        {
            IsBusy = true;
            CurrentPage += 1;
            await ExecuteLoadItemsCommand();
        }

        private async Task ExecutePreviousPageCommand()
        {
            IsBusy = true;
            CurrentPage -= 1;
            await ExecuteLoadItemsCommand();
        }


        private async Task OnDeleteItem(OperationDto operation)
        {
            if (operation == null)
                return;

            var dialog = await Shell.Current.DisplayAlert("Usuwanie!", $"Czy na pewno chcesz usunąć operację: {operation.Name}", "Tak", "Nie");

            if (!dialog)
                return;

            var response = await OperationService.DeleteAsync(operation.Id);

            if (!response.IsSuccess)
                await ShowErrorAlert(response);

            await ExecuteLoadItemsCommand();
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                var response = await OperationService.GetAsync(pageSize, CurrentPage);
                CurrentPage = response.Data.CurrentPage;
                LastPage = response.Data.LastPage;


                if (!response.IsSuccess)
                    await ShowErrorAlert(response);


                Operations.Clear();

                foreach (var item in response.Data.Operations)
                {
                    Operations.Add(item);
                }
            }
            catch (Exception exception)
            {
                await Shell.Current.DisplayAlert("Wystąpił Błąd!", exception.Message, "Ok");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }



        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(OperationDto operation)
        {
            if (operation == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={operation.Id}");
        }


    }
}