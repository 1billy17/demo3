using Avalonia.Controls;
using Avalonia.Interactivity;

namespace demo3;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void AddClient_OnClick(object? sender, RoutedEventArgs e)
    {
        AddAndUpdate addAndUpdateButton = new AddAndUpdate();
        addAndUpdateButton.ShowDialog(this);
    }

    private void Update_OnClick(object? sender, RoutedEventArgs e)
    {
        AddAndUpdate updateButton = new AddAndUpdate();
        updateButton.ShowDialog(this);
    }

    private void Attendance_OnClick(object? sender, RoutedEventArgs e)
    {
        AttendanceWindow attendanceButton = new AttendanceWindow();
        attendanceButton.ShowDialog(this);
    }
}