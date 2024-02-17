using Microsoft.Win32;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SystemProg_HW2;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    public Thread thread { get; set; }
    public string fromPath { get; set; }
    public string toPath { get; set; }

    private int maxValue;
    public int MaxValue
    {
        get { return maxValue; }
        set
        {
            maxValue = value;
            OnPropertyChanged();

        }
    }

    private int fileValue;
    public int FileValue
    {
        get { return fileValue; }
        set
        {
            fileValue = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
                => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    public MainWindow()
    {
        InitializeComponent();

        MaxValue = 100;
        DataContext = this;
    }

    private void from_btn_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.Filter = "all files (*.*)|*.*";

        if (fileDialog.ShowDialog() == true)
        {
            from_txtbox.Text = fileDialog.FileName;
            thread = new Thread(foo);
            thread.Start();

            void foo()
            {
                fromPath = fileDialog.FileName;
            }
        }

    }

    private void to_btn_Click(object sender, RoutedEventArgs e)
    {
        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.Filter = "all files (*.*)|*.*";

        if (fileDialog.ShowDialog() == true)
        {
            to_txtbox.Text = fileDialog.FileName;
            thread = new Thread(foo);
            thread.Start();

            void foo()
            {
                toPath = fileDialog.FileName;
            }
        }
    }

    private void copy_btn_Click(object sender, RoutedEventArgs e)
    {
        thread = new Thread(CopyData);
        thread.Start();
        pb.Value = 0;
    }

    public void CopyData()
    {
        if (!File.Exists(fromPath))
        {
            MessageBox.Show("From File Couldn't Find");

            return;
        }

        if (!File.Exists(toPath))
        {
            MessageBox.Show("To File Couldn't Find");

            return;
        }

        try
        {

            using (FileStream fsRead = new FileStream(fromPath, FileMode.Open, FileAccess.Read))
            {
                using (FileStream fsWrite = new FileStream(toPath, FileMode.Create, FileAccess.Write))
                {
                    var len = 100;
                    var fileSize = fsRead.Length;

                    MaxValue = (int)fileSize;
                    byte[] buffer = new byte[len];


                    do
                    {
                        Thread.Sleep(10);
                        len = fsRead.Read(buffer, 0, buffer.Length); 
                        fsWrite.Write(buffer, 0, len);

                        fileSize -= len;
                        FileValue += len;


                    } while (len != 0);

                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private void from_txtbox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        fromPath = from_txtbox.Text;
    }

    private void to_txtbox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        toPath = to_txtbox.Text;
    }
}