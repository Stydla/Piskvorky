using Piskvorky.FiveInARow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Piskvorky.Tests
{
  public class TestGame : INotifyPropertyChanged
  {

    private DeskData _DeskData = new DeskData();
    public DeskData DeskData
    {
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

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }




    public static string FILENAME = "TestGame.xml";

    internal static TestGame Load()
    {
      if (!File.Exists(FILENAME))
      {
        return new TestGame();
      }

      using (StreamReader sr = new StreamReader(FILENAME))
      {
        XmlSerializer xs = new XmlSerializer(typeof(TestGame));
        TestGame data = (TestGame)xs.Deserialize(sr);
        return data;
      }
    }

    internal void Save()
    {
      using (StreamWriter sw = new StreamWriter(FILENAME))
      {
        XmlSerializer xs = new XmlSerializer(typeof(TestGame));
        xs.Serialize(sw, this);
      }
    }

  }
}
