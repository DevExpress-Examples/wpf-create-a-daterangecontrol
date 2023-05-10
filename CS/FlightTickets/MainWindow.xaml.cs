using DevExpress.Mvvm.Native;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightTickets {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow {
        public MainWindow() {
            ApplicationThemeHelper.ApplicationThemeName = "Office2019Colorful";
            InitializeComponent();
        }
    }
    class ViewModel : BindableBase {
        readonly List<Flight> allFligths;
        public ViewModel() {
            allFligths = Enumerable.Range(0, 20)
                .Select(_ => DataSource.NextFlight())
                .ToList();

            Min = DateTime.Today.AddDays(-1);
            SearchCommand = new DelegateCommand(Search);
            Search();
        }

        public DateTime Min {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public DateTime? Start {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public DateTime? End {
            get { return GetValue<DateTime?>(); }
            set { SetValue(value); }
        }

        public List<string> Cities { get; } = DataSource.Cities;

        public string From {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string To {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public ObservableCollection<Flight> Flights { get; } = new ObservableCollection<Flight>();

        public ICommand SearchCommand { get; }

        void Search() {
            Flights.Clear();
            allFligths.Where(f => string.IsNullOrEmpty(From) || f.FromCity == From)
                      .Where(f => string.IsNullOrEmpty(To) || f.ToCity == To)
                      .Where(f => !Start.HasValue || f.Start >= Start)
                      .Where(f => !End.HasValue || f.End <= End)
                      .ForEach(Flights.Add);
        }
    }

    class Flight {
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    static class DataSource {
        static readonly Random random = new Random();
        public static List<string> Cities { get; } = new List<string> {
            "Moscow", "Yerevan", "Beijing"
        };
        public static string NextCity() {
            return Cities[random.Next(Cities.Count)];
        }
        public static DateTime NextDate() {
            var offset = random.Next(-30, 30);
            return DateTime.Today.AddDays(offset);
        }
        public static Flight NextFlight() {
            var flight = new Flight {
                FromCity = NextCity(),
                ToCity = NextCity(),
                Start = NextDate()
            };
            flight.End = flight.Start.AddDays(random.Next(14));
            return flight;
        }
    }
}
