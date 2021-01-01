using Piskvorky.FiveInARow;
using Piskvorky.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Piskvorky.BulkSolve
{
  public class BulkSolveData
  {

    public ObservableCollection<BulkSolveDataItem> Data { get; set; } = new ObservableCollection<BulkSolveDataItem>();
    
  }

  public class BulkSolveDataItem : INotifyPropertyChanged
  {
    public GameDV Game { get; set; }
    public UserDV User { get; set; }

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

  }
}
