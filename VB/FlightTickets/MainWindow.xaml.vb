Imports DevExpress.Mvvm.Native
Imports DevExpress.Mvvm
Imports DevExpress.Xpf.Core
Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Linq
Imports System.Windows.Controls
Imports System.Windows.Input

Namespace FlightTickets

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Public Partial Class MainWindow

        Public Sub New()
            ApplicationThemeHelper.ApplicationThemeName = "Office2019Colorful"
            Me.InitializeComponent()
        End Sub
    End Class

    Friend Class ViewModel
        Inherits BindableBase

        Private ReadOnly allFligths As List(Of Flight)

        Public Sub New()
            allFligths = Enumerable.Range(0, 20).[Select](Function(__) NextFlight()).ToList()
            Min = Date.Today.AddDays(-1)
            SearchCommand = New DelegateCommand(AddressOf Search)
            Search()
        End Sub

        Public Property Min As Date
            Get
                Return GetValue(Of Date)()
            End Get

            Set(ByVal value As Date)
                SetValue(value)
            End Set
        End Property

        Public Property Start As Date?
            Get
                Return GetValue(Of Date?)()
            End Get

            Set(ByVal value As Date?)
                SetValue(value)
            End Set
        End Property

        Public Property [End] As Date?
            Get
                Return GetValue(Of Date?)()
            End Get

            Set(ByVal value As Date?)
                SetValue(value)
            End Set
        End Property

        Public ReadOnly Property Cities As List(Of String) = DataSource.Cities

        Public Property From As String
            Get
                Return GetValue(Of String)()
            End Get

            Set(ByVal value As String)
                SetValue(value)
            End Set
        End Property

        Public Property [To] As String
            Get
                Return GetValue(Of String)()
            End Get

            Set(ByVal value As String)
                SetValue(value)
            End Set
        End Property

        Public ReadOnly Property Flights As ObservableCollection(Of Flight) = New ObservableCollection(Of Flight)()

        Public ReadOnly Property SearchCommand As ICommand

        Private Sub Search()
            Flights.Clear()
            allFligths.Where(Function(f) String.IsNullOrEmpty(From) OrElse Equals(f.FromCity, From)).Where(Function(f) String.IsNullOrEmpty([To]) OrElse Equals(f.ToCity, [To])).Where(Function(f) Not Start.HasValue OrElse f.Start >= Start).Where(Function(f) Not [End].HasValue OrElse f.End <= [End]).ForEach(New Action(Of Flight)(AddressOf Flights.Add))
        End Sub
    End Class

    Friend Class Flight

        Public Property FromCity As String

        Public Property ToCity As String

        Public Property Start As Date

        Public Property [End] As Date
    End Class

    Friend Module DataSource

        Private ReadOnly random As Random = New Random()

        Public ReadOnly Property Cities As List(Of String) = New List(Of String) From {"Moscow", "Yerevan", "Beijing"}

        Public Function NextCity() As String
            Return Cities(random.Next(Cities.Count))
        End Function

        Public Function NextDate() As Date
            Dim offset = random.Next(-30, 30)
            Return Date.Today.AddDays(offset)
        End Function

        Public Function NextFlight() As Flight
            Dim flight = New Flight With {.FromCity = NextCity(), .ToCity = NextCity(), .Start = NextDate()}
            flight.End = flight.Start.AddDays(random.Next(14))
            Return flight
        End Function
    End Module
End Namespace
