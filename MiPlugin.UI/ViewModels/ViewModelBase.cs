using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MiPlugin.UI.ViewModels
{
    //permite que el ViewModel avise a la ventana de que un dato ha cambiado.
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        //esto me sirve para avisar a la vista que una de las propiedades ha cambiado,
        //sin ello, la pantalla no se refresca
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
    //abstract--> Solo se va a heredar, no se va a instanciar
    //INotifyPropertyChanged --> Lo usa WPF para saber si tiene que escuchar cambios
    //protected --> Lo pueden llamar las clases hijas pero no desde fuera
}
