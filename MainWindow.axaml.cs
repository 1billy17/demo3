using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using demo3.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Avalonia.Layout;

namespace demo3;

public partial class MainWindow : Window
{
    ObservableCollection<ClientPresenter> clients;
    
    private const int ItemsPerPage = 25;
    private int currentPage = 0;
    
    public MainWindow()
    {
        InitializeComponent();
        using var context = new Demo3Context();
        var dataSource = context.Clients.Select(client => new ClientPresenter
        {
            Id = client.Id,
            Gendercode = client.Gendercode,
            Firstname = client.Firstname,
            Lastname = client.Lastname,
            Patronymic = client.Patronymic,
            Birthday = client.Birthday,
            Phone = client.Phone,
            Email = client.Email,
            Registrationdate = client.Registrationdate,
        });
        clients = new ObservableCollection<ClientPresenter>(dataSource);
        UpdateListBox();
        
        BackButton.IsEnabled = false;
    }

    public class ClientPresenter() : Client
    {
        
    }
    
    private void UpdateListBox()
    {
        var pagedClients = clients.Skip(currentPage * ItemsPerPage).Take(ItemsPerPage).ToList();
        ClientsListBox.ItemsSource = pagedClients;
        
        BackButton.IsEnabled = currentPage > 0;
        NextButton.IsEnabled = (currentPage + 1) * ItemsPerPage < clients.Count;
    }
    
    private void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateListBox();
        }
    }

    private void Next_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((currentPage + 1) * ItemsPerPage < clients.Count)
        {
            currentPage++;
            UpdateListBox();
        }
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
