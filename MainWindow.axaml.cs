using System;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Interactivity;
using demo3.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Avalonia.Layout;
using System.Collections.Generic;

namespace demo3;

public partial class MainWindow : Window
{
    ObservableCollection<ClientPresenter> clients = new ObservableCollection<ClientPresenter>();
    List<ClientPresenter> dataSourceClients;
    
    private const int ItemsPerPage = 20;
    private int currentPage = 0;
    
    public MainWindow()
    {
        InitializeComponent();
        using var context = new Demo3Context();
        dataSourceClients = context.Clients.Select(client => new ClientPresenter
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
        }).ToList();
        CountComboBox.SelectedIndex = 0;
        GenderSortCombobox.SelectedIndex = 0;
        SortComboBox.SelectedIndex = 0;
        DisplayServices();
        
        BackButton.IsEnabled = false;
    }

    public class ClientPresenter : Client
    {
        
    }

    public void DisplayServices()
    {
        var temp = dataSourceClients;
        clients.Clear();
        switch (CountComboBox.SelectedIndex) 
        {
            case 0: break;
            case 1: temp = temp.Take(10).ToList(); break;
            case 2: temp = temp.Take(50).ToList(); break;
            case 3: temp = temp.Take(200).ToList(); break;
            default: break;
        }

        switch (SortComboBox.SelectedIndex)
        {
            case 0: temp = temp.OrderBy(it => it.Lastname).ToList(); break;
            case 1: temp = temp.OrderByDescending(it => it.Registrationdate).ToList(); break;
            default: break;
        }

        switch (GenderSortCombobox.SelectedIndex)
        {
            case 0: break;
            case 1: temp = temp.Where(it => it.Gendercode == 'м').ToList(); break;
            case 2: temp = temp.Where(it => it.Gendercode == 'ж').ToList(); break;
            default: break;
        }

        foreach (var item in temp)
        {
            clients.Add(item);
        }
        
        CountServices.Text = $"{temp.Count}/{dataSourceClients.Count}";
        
        UpdateListBox();
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
    
    public void OnFilterOrSortChanged(object? sender, RoutedEventArgs e)
    {
        currentPage = 0;
        DisplayServices();
    }

    public void CountComboBox_OnSelectionChanged(object? sender, RoutedEventArgs e)
    {
        DisplayServices();
    }
    
    public void SortComboBox_OnSelectionChanged(object? sender, RoutedEventArgs e)
    {
        DisplayServices();
    }
    
    public void GenderSortCombobox_OnSelectionChanged(object? sender, RoutedEventArgs e)
    {
        DisplayServices();
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
            dataSourceClients.Remove(client);
            
            DisplayServices();
        }
    }

    public void AttendanceClient_OnClick(object? sender, RoutedEventArgs e)
    {
        AttendanceWindow attendanceButton = new AttendanceWindow();
        attendanceButton.ShowDialog(this);
    }
}
