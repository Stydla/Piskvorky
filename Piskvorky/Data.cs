using Piskvorky.BulkSolve;
using Piskvorky.FiveInARow;
using Piskvorky.Tests;
using Piskvorky.User;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Piskvorky
{
  public class Data : INotifyPropertyChanged
  {
    public UsersPanelDV UsersPanelDV { get; set; } = new UsersPanelDV();


    [XmlIgnore]
    public bool AutomaticMode { get; set; } = false;

    private DeskData _DeskData;
    [XmlIgnore]
    public DeskData DeskData { 
      get
      {
        return _DeskData;
      }
      set 
      {
        _DeskData = value;
        OnPropertyChanged();
      }
    }

    [XmlIgnore]
    public TestGame TestGame { get; set; } = new TestGame();

    [XmlIgnore]
    public BulkSolveData BulkSolveData { get; set; } = new BulkSolveData();

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }





    public static string FILENAME = "Data.xml";

    public event PropertyChangedEventHandler PropertyChanged;

    internal static Data Load()
    {
      if(!File.Exists(FILENAME))
      {
        return new Data();
      }

      using (StreamReader sr = new StreamReader(FILENAME))
      {
        XmlSerializer xs = new XmlSerializer(typeof(Data));
        Data data = (Data)xs.Deserialize(sr);
        return data;
      }
    }

    internal void Save()
    {
      using (StreamWriter sw = new StreamWriter(FILENAME))
      {
        XmlSerializer xs = new XmlSerializer(typeof(Data));
        xs.Serialize(sw, this);
      }
    }
  }
}
