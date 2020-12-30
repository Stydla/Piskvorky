using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piskvorky.User
{
  public class UserListDV
  {
    public ObservableCollection<UserDV> Users { get; set; } = new ObservableCollection<UserDV>();

    public UserDV SelectedUser {get; set;}
  }
}
