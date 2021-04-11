using System;
using System.Drawing;

namespace Wallets.BusinessLayer
{
    public class Category : EntityBase
    {


        private Guid _guid;
        private string _name;
        private string _description;
        private Color _color;
        private System.Drawing.Icon _image;
        private Guid _userGuid;


        public Guid Guid { get => _guid; private set => _guid = value; }
        public string Name { get => _name; set => _name = value; }
        public string Description { get => _description; set => _description = value; }
        public Guid UserGuid { get => _userGuid; private set => _userGuid = value; }
        public Color Color { get => _color; set => _color = value; }
        public Icon Image { get => _image; set => _image = value; }

        public Category(Guid userGuid, string name, string description, Color color, Icon image)
        {
            _guid = Guid.NewGuid();
            _userGuid = userGuid;
            _name = name;
            _description = description;
            _color = color;
            _image = image;
        }

        public override bool Validate()
        {
            var result = true;
            if (String.IsNullOrWhiteSpace(Name))
                result = false;

            return result;
        }

    }
}
