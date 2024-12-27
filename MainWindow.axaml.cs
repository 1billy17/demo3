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
    
    public void UpdateListBox()
    {
        var pagedClients = clients.Skip(currentPage * ItemsPerPage).Take(ItemsPerPage).ToList();
        ClientsListBox.ItemsSource = pagedClients;
        
        BackButton.IsEnabled = currentPage > 0;
        NextButton.IsEnabled = (currentPage + 1) * ItemsPerPage < clients.Count;
    }
    
    public void Back_OnClick(object? sender, RoutedEventArgs e)
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateListBox();
        }
    }

    public void Next_OnClick(object? sender, RoutedEventArgs e)
    {
        if ((currentPage + 1) * ItemsPerPage < clients.Count)
        {
            currentPage++;
            UpdateListBox();
        }
    }
    
    public void AddClient_OnClick(object? sender, RoutedEventArgs e)
    {
        AddAndUpdate addAndUpdateButton = new AddAndUpdate();
        addAndUpdateButton.ShowDialog(this);
    }

    public void UpdateClient_OnClick(object? sender, RoutedEventArgs e)
    {
        AddAndUpdate updateButton = new AddAndUpdate();
        updateButton.ShowDialog(this);
    }
    
    public void DeleteClient_OnClick(object? sender, RoutedEventArgs e)
    {
        using var context = new Demo3Context();
        var client = ClientsListBox.SelectedItem as ClientPresenter;
        if (client == null) return;
        var removeClient = context.Clients.Include(it => it.Clientservices).First(it => it.Id == client.Id);
        if (removeClient == null) return;
        if (removeClient.Clientservices.Count() > 0) return;
        context.Clients.Remove(removeClient);
        
        if (context.SaveChanges() > 0)
        {
            clients.Remove(client);
            
            UpdateListBox();
        }
    }

    public void AttendanceClient_OnClick(object? sender, RoutedEventArgs e)
    {
        AttendanceWindow attendanceButton = new AttendanceWindow();
        attendanceButton.ShowDialog(this);
    }
}
