using System.Windows;
using System.Windows.Controls;

namespace evius
{
    public class Data
    {
        public string Id { get; set; }
        public string Image { get; set; }
        public string Photo { get; set; }
        public string Type { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Follower { get; set; }
        public string Date { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string BornDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Settings { get; set; }
        public string Info { get; set; }
        public string City { get; set; }
        public string Vote { get; set; }
        public string Rating1 { get; set; }
        public string Rating2 { get; set; }
        public string Rating3 { get; set; }

        public string Waiting { get; set; }
        public string Confermati { get; set; }
        public string drive_type { get; set; }
        public string NrVoti { get; set; }
        public string UserImage { get; set; }
        public string Confermato { get; set; }
        public string UserId { get; set; }
    }


    public class TemplateSelector : DataTemplateSelector
    {
        public DataTemplate Header { get; set; }
        public DataTemplate Item { get; set; }
        public DataTemplate Return { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            Data items = item as Data;
            if (items != null)
            {
                if (items.Type == "Header")
                {
                    return Header;
                }
                else
                {
                    if (items.Type == "Item")
                    {
                        return Item;
                    }
                    else
                    {
                        return Return;
                    }
                }
            }

            return base.SelectTemplate(item, container);
        }
    }

    public abstract class DataTemplateSelector : ContentControl
    {
        public virtual DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return null;
        }

        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            ContentTemplate = SelectTemplate(newContent, this);
        }
    }

}