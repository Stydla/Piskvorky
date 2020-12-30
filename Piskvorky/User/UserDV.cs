using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piskvorky.User
{
  public class UserDV
  {
    public string Nickname { get; set; }
    public string Email { get; set; }
    public string Id { get; set; }
    public string Token { get; set; }

    public GameDV SelectedGame { get; set; }

    public ObservableCollection<GameDV> Games { get; set; } = new ObservableCollection<GameDV>();
  }
}
