using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RizvanovAutoservice
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {

        private Service _currentService = new Service();

        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                this._currentService = SelectedService;

            DataContext = _currentService;

            var _currentClient = Rizvanov_AutoserviceEntities.GetContext().Client.ToList();

            ComboClient.ItemsSource = _currentClient;

        }
        private ClientService _currentClientService = new ClientService();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (ComboClient.SelectedItem == null)
                errors.AppendLine("Укажите ФИО клиента");

            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");

            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);

            if (_currentClientService.ID == 0)
                Rizvanov_AutoserviceEntities.GetContext().ClientService.Add(_currentClientService);

            try
            {
                Rizvanov_AutoserviceEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            /*  string s = TBStart.Text;

              if (s.Length < 3 || !s.Contains(':'))
                  TBEnd.Text = "";
              else
              {
                  string[] start = s.Split(new char[] { ':' });
                  int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                  int startMin = Convert.ToInt32(start[1].ToString());

                  int sum = startHour + startMin + _currentService.DurationInIn;

                  int EndHour = sum / 60;
                  int EndMin = sum % 60;
                  s=EndHour.ToString() + ":" + EndMin.ToString();
                  TBEnd.Text = s;
              } */
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void TBStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[0-9:]");


            if (!regex.IsMatch(e.Text))
                e.Handled = true;

        }
        /*private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;
            if (s.Length < 3 || s.Length > 5 || !s.Contains(':'))
                TBEnd.Text = "";
            else
            {
                Regex regex = new Regex("\\b[0-2]?\\d:[0-5]\\d\\b");
                if (!regex.IsMatch(s))
                {
                    MessageBox.Show("Введите корректное время начала!");
                    TBStart.Clear();
                }
                else
                {
                    string[] start = s.Split(new char[] { ':' });
                    int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                    int startMin = Convert.ToInt32(start[1].ToString());

                    int sum = startHour + startMin + _currentService.DurationIn;

                    int EndHour = sum / 60;
                    int EndMin = sum % 60;
                    s = EndHour.ToString() + ":" + EndMin.ToString();
                    TBEnd.Text = s;
                }
            } */
            
            private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
            {
                string s = TBStart.Text;

                if (s.Length < 3 || s.Length > 5 || !s.Contains(':'))
                    TBEnd.Text = "";
                else
                {
                    string[] start = s.Split(new char[] { ':' });
                    try
                    {
                        if (Convert.ToInt32(start[0].ToString()) >= 0 && Convert.ToInt32(start[0].ToString()) <= 23 && Convert.ToInt32(start[1].ToString()) >= 0 && Convert.ToInt32(start[1].ToString()) <= 59 && start[1].Length == 2)
                        {
                            int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                            int startMin = Convert.ToInt32(start[1].ToString());

                            int sum = startHour + startMin + _currentService.DurationIn;

                            string EndHour = (sum / 60 % 24).ToString();
                            string EndMin = (sum % 60).ToString();
                            if (Convert.ToInt32(EndMin) / 10 == 0)
                            {
                                EndMin = '0' + EndMin;
                            }
                            s = EndHour.ToString() + ":" + EndMin;
                            TBEnd.Text = s;
                        }
                        else
                        {
                            TBEnd.Text = "";
                        }
                    }
                    catch
                    {
                        TBEnd.Text = "";
                    }

                }
            }
            
        //}
    }
}
